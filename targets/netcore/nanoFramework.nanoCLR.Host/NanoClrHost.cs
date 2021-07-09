//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using nanoFramework.nanoCLR.DebugLibrary;
using nanoFramework.nanoCLR.Host.Interop;

namespace nanoFramework.nanoCLR.Host
{
    public class NanoClrHost
    {
        public bool IsRunning { get; private set; }

        private NanoInterop.WireTransmitDelegate _wireReceiveCallback;
        internal List<Action> PreInitConfigureSteps { get; } = new();
        internal List<Action> ConfigureSteps { get; } = new();
        internal NanoClrSettings NanoClrSettings { get; set; } = NanoClrSettings.Default;
        internal IDeviceSink WireProtocolSink { get; set; }

        public VirtualDevice Device { get; internal set; }

        internal NanoClrHost()
        {
        }

        public void Run()
        {
            if (IsRunning)
                return;
            IsRunning = true;

            NanoInterop.NanoClr_SetConfigureCallback(ConfigureRuntime);

            if (WireProtocolSink != null)
                InitWireProtocolSink();

            PreInitConfigureRuntime();
            NanoInterop.NanoClr_Run(NanoClrSettings);

            Cleanup();
        }

        private void Cleanup()
        {
            if (WireProtocolSink != null)
                WireProtocolSink.Close();
        }

        private void InitWireProtocolSink()
        {
            _wireReceiveCallback = new NanoInterop.WireTransmitDelegate(WireReceiveCallback);
            NanoInterop.NanoClr_SetWireTransmitCallback(_wireReceiveCallback);
            WireProtocolSink.Initialize(WireTransmitToDevice);
            WireProtocolSink.Open();
        }

        private void WireReceiveCallback(byte[] bytes, int length)
        {
            WireProtocolSink.ReceivedFromDevice(bytes);
        }

        private void WireTransmitToDevice(byte[] bytes)
        {
            NanoInterop.NanoClr_WireReceive(bytes, bytes.Length);
        }

        private void PreInitConfigureRuntime() =>
            PreInitConfigureSteps.ForEach(s => s());

        private uint ConfigureRuntime()
        {
            ConfigureSteps.ForEach(s => s());
            return NanoInterop.ClrOk;
        }

        public static NanoClrHostBuilder CreateBuilder() => new NanoClrHostBuilder { };
    }
}
