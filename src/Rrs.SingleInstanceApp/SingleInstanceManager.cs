using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rrs.SingleInstanceApp
{
    public class SingleInstanceManager
    {
        public static void Run(ISingleInstanceApp app, string[] args = null)
        {
            var mutex = new Mutex(true, $"SingleInstance_Mutex_{app.Id}");
            var eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, $"SingleInstance_StartEvent_{app.Id}");
            try
            {
                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                    WaitOrTimerCallback callback = delegate
                    {
                        Task.Factory.StartNew(() => app.Activate(), CancellationToken.None, TaskCreationOptions.None, scheduler);
                    };

                    var registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(eventHandle, callback, null, Timeout.Infinite, false);

                    app.Run(args);

                    mutex.ReleaseMutex();
                    registeredWaitHandle.Unregister(null);
                }
                else
                {
                    eventHandle.Set();
                }
            }
            finally
            {
                eventHandle.Close();
                eventHandle.Dispose();
                mutex.Close();
                mutex.Dispose();
            }
        }
    }
}
