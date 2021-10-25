using System;

namespace DTNL.LL.Logic.Options
{
    public class ServiceWorkerOptions
    {
        public const string ServiceWorker = "ServiceWorker";

        private int _tickTimeInSeconds;
        public int TickTimeInSeconds
        {
            get => _tickTimeInSeconds;
            set => _tickTimeInSeconds = Math.Max(1, value);
        }
    }
}