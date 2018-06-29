using System;

namespace Rrs.SingleInstanceApp
{
    public interface ISingleInstanceApp
    {
        void Run(string[] args);
        void Activate();
        string Id { get; }
    }
}
