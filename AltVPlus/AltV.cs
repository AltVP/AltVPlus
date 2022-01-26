using AltV.Net;
using AltV.Net.Async;
using AltV.Net.ColoredConsole;

namespace AltVPlus
{
    public class AltV : AsyncResource
    {
        public override void OnStart()
            => AltVDev.LogStarted();

        public override void OnStop()
            => AltVDev.LogStopped();
    }
}