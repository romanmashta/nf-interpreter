//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using nanoFramework.nanoCLR.DebugLibrary;
using nanoFramework.nanoCLR.Host;
using nanoFramework.nanoCLR.Host.Debugger;
using nanoFramework.Tools.Debugger;

namespace nanoFramework.nanoCLR.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            LogErrors(() =>
            {
                NanoClrHostBuilder hostBuilder = NanoClrHost.CreateBuilder();
                hostBuilder.UseConsoleDebugPrint();

                Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o =>
                {
                    if (o.Assemblies.Any())
                        hostBuilder.LoadAssemblies(o.Assemblies);

                    if (o.AssembliesSet != null)
                        hostBuilder.LoadAssembliesSet(o.AssembliesSet);

                    if (o.TryResolve)
                        hostBuilder.TryResolve();

                    if (o.DebugPort != null)
                    {
                        hostBuilder.WaitForDebugger = true;
                        hostBuilder.EnterDebuggerLoopAfterExit = true;
                        hostBuilder.UseSerialPortWireProtocol(o.DebugPort, o.TraceWire);
                    }

                    if (o.DeviceInfo)
                    {
                        hostBuilder.WaitForDebugger = true;
                        hostBuilder.EnterDebuggerLoopAfterExit = true;
                        hostBuilder.UseLocalDebugger();
                    }

                    var host = hostBuilder.Build();
                    Task.Run(() => host.Run());

                    if (o.DeviceInfo)
                        DisplayVirtualDeviceInfo(host);

                    Console.ReadLine();
                });
            });
        }

        private static void DisplayVirtualDeviceInfo(NanoClrHost nanoClrHost)
        {
            Task.Run(() =>
            {
                var device = nanoClrHost.Device;
                device.CreateDebugEngine();
                device.DebugEngine.Connect(false, requestCapabilities: true);
                var deviceInfo = device.GetDeviceInfo();
                Console.WriteLine(deviceInfo);
            });
        }

        private void ConnectLocalDebugger()
        {

        }

        private static void LogErrors(Action scope)
        {
            try
            {
                scope();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
