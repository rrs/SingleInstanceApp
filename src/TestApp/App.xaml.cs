using Rrs.SingleInstanceApp;
using System;
using System.Windows;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        public string Id { get; } = new Guid("b6ce4268-e2f2-4a65-a2a5-a75fdf22681a").ToString();
        private Window _w;

        public void Activate()
        {
            _w.Activate();
        }

        public void Run(string[] args)
        {
            _w = new MainWindow();
            _w.ShowDialog();
        }

        private void Application_Startup(object sender, StartupEventArgs args)
        {
            try
            {
                SingleInstanceManager.Run(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Shutdown();
        }
    }
}
