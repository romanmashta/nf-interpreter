using nanoFramework.Tools.Debugger;

namespace nanoFramework.nanoCLR.DebugLibrary
{
    public class VirtualDevice : NanoDeviceBase, INanoDevice
    {
        public ConnectPortResult Connect() => ConnectionPort.ConnectDevice();

        public override void Disconnect(bool force = false) => ConnectionPort.DisconnectDevice(force);
    }
}
