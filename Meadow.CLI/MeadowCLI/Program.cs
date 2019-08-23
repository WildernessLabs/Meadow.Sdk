using CommandLine;
using System;
using MeadowCLI.DeviceManagement;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace MeadowCLI
{
    class Program
    {
        static AutoResetEvent _abortEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                _abortEvent.Set();
            };

            if (args.Length == 0)
            {
                args = new string[] { "--help" };
            }
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(options =>
            {
                if (options.Dfu)
                {
                    //ToDo update to use command line args for os and user
                    DfuUpload.FlashNuttx(options.DfuOsPath, options.DfuUserPath);
                }
                else
                {
                    var ct = new CancellationTokenSource();
                    var t = Task.Run(() =>
                    {
                        ProcessHcom(options, ct.Token);
                    }, ct.Token);

                    while (!t.Wait(1000))
                    {
                        if (_abortEvent.WaitOne(0))
                        {
                            Console.WriteLine("\n<aborting...>");
                            ct.Cancel();
                            return;
                        }
                        else
                        {
                            Console.Write('.');
                        }
                    }
                }
			});
            
            for (int i = 0; i < 50; i++)
                Thread.Sleep(50);

//            Console.WriteLine("Press any key to exit");
//            Console.ReadKey();
        }

        private static bool ValidateSerialPort()
        {
            if (DeviceManager.CurrentDevice.SerialPort == null)
            {
                Console.WriteLine($"A serial port has not been selected (--SerialPort option)");
                return false;
            }

            return true;
        }

        //Probably rename
        static void ProcessHcom(Options options, CancellationToken ct)
        {
            // TODO: use the cancellation token to allow aborting file actions
            ConnectToMeadowDevice(options.SerialPort);

            if (options.IsFileOperation())
            {
                var fileTask = Task.Run(() =>
                {
                    if (options.WriteFile)
                    {
                        if (string.IsNullOrEmpty(options.FileName))
                        {
                            Console.WriteLine($"A source file name (--File option) is required");
                            return;
                        }

                        var fi = new FileInfo(options.FileName);
                        if (!fi.Exists)
                        {
                            Console.WriteLine($"Source file '{options.FileName}' not found");
                            return;
                        }

                        Console.WriteLine($"Writing {options.FileName} to partition {options.Partition}");

                        MeadowFileManager.WriteFileToFlash(DeviceManager.CurrentDevice,
                            options.FileName, options.TargetFileName, options.Partition);
                    }
                    else if (options.DeleteFile)
                    {
                        if (string.IsNullOrEmpty(options.FileName))
                        {
                            Console.WriteLine($"A file name to delete (--File flag) is required");
                            return;
                        }

                        Console.WriteLine($"Deleting {options.FileName} from partion {options.Partition}");
                        MeadowFileManager.DeleteFile(DeviceManager.CurrentDevice,
                            options.TargetFileName, options.Partition);
                    }
                    else if (options.EraseFlash)
                    {
                        if (ValidateSerialPort())
                        {
                            Console.WriteLine("Erasing flash");
                            MeadowFileManager.EraseFlash(DeviceManager.CurrentDevice);
                        }
                    }
                    else if (options.VerifyErasedFlash)
                    {
                        Console.WriteLine("Verifying flash is erased");
                        MeadowFileManager.VerifyErasedFlash(DeviceManager.CurrentDevice);
                    }
                    else if (options.PartitionFileSystem)
                    {
                        Console.WriteLine($"Partioning file system into {options.NumberOfPartitions} partion(s)");
                        MeadowFileManager.PartitionFileSystem(DeviceManager.CurrentDevice, options.NumberOfPartitions);
                    }
                    else if (options.MountFileSystem)
                    {
                        Console.WriteLine($"Mounting partition {options.Partition}");
                        MeadowFileManager.MountFileSystem(DeviceManager.CurrentDevice, options.Partition);
                    }
                    else if (options.InitFileSystem)
                    {
                        Console.WriteLine($"Intializing filesystem in partition {options.Partition}");
                        MeadowFileManager.InitializeFileSystem(DeviceManager.CurrentDevice, options.Partition);
                    }
                    else if (options.CreateFileSystem) //should this have a partition???
                    {
                        Console.WriteLine($"Creating file system");
                        MeadowFileManager.CreateFileSystem(DeviceManager.CurrentDevice);
                    }
                    else if (options.FormatFileSystem)
                    {
                        Console.WriteLine($"Format file system on partition {options.Partition}");
                        MeadowFileManager.FormatFileSystem(DeviceManager.CurrentDevice, options.Partition);
                    }
                    else if (options.ListFiles)
                    {
                        Console.WriteLine($"Getting list of a files on partition {options.Partition}");
                        MeadowFileManager.ListFiles(DeviceManager.CurrentDevice, options.Partition);
                    }
                    else if (options.ListFilesAndCrcs)
                    {
                        Console.WriteLine($"Getting list of a files and Crcs on partition {options.Partition}");
                        MeadowFileManager.ListFilesAndCrcs(DeviceManager.CurrentDevice, options.Partition);
                    }
                });

                while (!fileTask.Wait(500))
                {
                    // wait for completion or cancel request
                    if (ct.IsCancellationRequested)
                    {
                        // TODO: implement file manager clean up

                        DeviceManager.CurrentDevice.SerialPort.Close();

                        ct.ThrowIfCancellationRequested();
                    }
                }
            }
            else
            {
                //Device manager
                if (options.SetTraceLevel)
                {
                    Console.WriteLine($"Setting trace level to {options.TraceLevel}");
                    DeviceManager.SetTraceLevel(DeviceManager.CurrentDevice, options.TraceLevel);
                }
                else if (options.SetDeveloper1)
                {
                    Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                    DeviceManager.SetDeveloper1(DeviceManager.CurrentDevice, options.DeveloperValue);
                }
                else if (options.SetDeveloper2)
                {
                    Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                    DeviceManager.SetDeveloper2(DeviceManager.CurrentDevice, options.DeveloperValue);
                }
                else if (options.SetDeveloper3)
                {
                    Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                    DeviceManager.SetDeveloper3(DeviceManager.CurrentDevice, options.DeveloperValue);
                }
                else if (options.SetDeveloper4)
                {
                    Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                    DeviceManager.SetDeveloper4(DeviceManager.CurrentDevice, options.DeveloperValue);
                }

                else if (options.ToggleNsh)
                {
                    Console.WriteLine($"Toggling Nsh");
                    DeviceManager.ToggleNsh(DeviceManager.CurrentDevice);
                }
                else if (options.MonoDisable)
                {
                    DeviceManager.MonoDisable(DeviceManager.CurrentDevice);
                }
                else if (options.MonoEnable)
                {
                    DeviceManager.MonoEnable(DeviceManager.CurrentDevice);
                }
                else if (options.ResetTargetMcu)
                {
                    Console.WriteLine("Resetting Mcu");
                    DeviceManager.ResetTargetMcu(DeviceManager.CurrentDevice);
                }
                else if (options.EnterDfuMode)
                {
                    Console.WriteLine("Entering Dfu mode");
                    DeviceManager.EnterDfuMode(DeviceManager.CurrentDevice);
                }
            }
        }

        //temp code until we get the device manager logic in place 
        static void ConnectToMeadowDevice (string commPort)
		{
            var device = new MeadowDevice(commPort);
            try
            {
                device.OpenSerialPort();
                Console.WriteLine($"Port {commPort} opened");
            }
            catch (MeadowDeviceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            DeviceManager.CurrentDevice = device;
        }
    }
}
