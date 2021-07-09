using System;
using System.Collections.Generic;
using System.Linq;
using nanoFramework.nanoCLR.Host.Interop;
using nanoFramework.Tools.Debugger;

namespace nanoFramework.nanoCLR.Host.Debugger
{
    public class DebuggerInteropSink : IDeviceSink, IPort
    {
        private readonly object _syncRoot = new();
        private readonly List<byte> _buffer = new();
        private Action<byte[]> _transmitToDevice;

        public void Initialize(Action<byte[]> transmitToDevice)
        {
            _transmitToDevice = transmitToDevice;
        }

        public void ReceivedFromDevice(byte[] bytes) => Sync(() => _buffer.AddRange(bytes));

        public void Open()
        {
        }

        public void Close()
        {
        }

        public int AvailableBytes => Sync(() => _buffer.Count);

        public int SendBuffer(byte[] buffer)
        {
            _transmitToDevice(buffer);
            return buffer.Length;
        }

        public byte[] ReadBuffer(int bytesToRead) => Sync(() =>
        {
            var buffer = _buffer.Take(bytesToRead).ToArray();
            _buffer.RemoveRange(0, bytesToRead);
            return buffer;
        });

        public ConnectPortResult ConnectDevice() => ConnectPortResult.Connected;

        public void DisconnectDevice(bool force = false)
        {
        }

        private T Sync<T>(Func<T> func)
        {
            lock (_syncRoot)
            {
                return func();
            }
        }

        private void Sync(Action func)
        {
            lock (_syncRoot)
            {
                func();
            }
        }
    }
}
