﻿using CommandLine;
using System;
using MeadowCLI.DeviceManagement;
using System.IO.Ports;

namespace MeadowCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                MeadowDeviceManager.CurrentDevice.SerialPort.Close();
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
                    SyncArgsCache(options);
                    ProcessHcom(options);
                }
            });

            Console.ReadKey();
        }

        static bool IsSerialPortValid(SerialPort serialPort)
        {
            if (serialPort == null)
            {
                Console.WriteLine($"A serial port has not been selected or the serial port isn't available (--SerialPort option)");
                return false;
            }

            return true;
        }

        static void SyncArgsCache(Options options)
        {
            State state = null;

            if (options.ClearCache)
            {
                StateCache.Clear();
            }
            else
            {
                state = StateCache.Load();
            }

            if (string.IsNullOrWhiteSpace(options.SerialPort))
            {
                options.SerialPort = state.SerialPort;
            }
            else
            {
                state.SerialPort = options.SerialPort;
                StateCache.Save(state);
            }
        }

        //Probably rename
        static void ProcessHcom(Options options)
        {
            ConnectToMeadowDevice(options.SerialPort);

            if(IsSerialPortValid(MeadowDeviceManager.CurrentDevice.SerialPort) == false)
                return;

            if (options.WriteFile)
            {
                Console.WriteLine($"Writing {options.FileName} to partition {options.Partition}");
                MeadowFileManager.WriteFileToFlash(MeadowDeviceManager.CurrentDevice,
                    options.FileName, options.TargetFileName, options.Partition);
            }
            else if (options.DeleteFile)
            {
                Console.WriteLine($"Deleting {options.FileName} from partion {options.Partition}");
                MeadowFileManager.DeleteFile(MeadowDeviceManager.CurrentDevice,
                    options.TargetFileName, options.Partition);
            }
            else if (options.EraseFlash)
            {
                Console.WriteLine("Erasing flash");
                MeadowFileManager.EraseFlash(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.VerifyErasedFlash)
            {
                Console.WriteLine("Verifying flash is erased");
                MeadowFileManager.VerifyErasedFlash(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.PartitionFileSystem)
            {
                Console.WriteLine($"Partioning file system into {options.NumberOfPartitions} partion(s)");
                MeadowFileManager.PartitionFileSystem(MeadowDeviceManager.CurrentDevice, options.NumberOfPartitions);
            }
            else if (options.MountFileSystem)
            {
                Console.WriteLine($"Mounting partition {options.Partition}");
                MeadowFileManager.MountFileSystem(MeadowDeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.InitFileSystem)
            {
                Console.WriteLine($"Intializing filesystem in partition {options.Partition}");
                MeadowFileManager.InitializeFileSystem(MeadowDeviceManager.CurrentDevice, options.Partition);
            }
            else if(options.CreateFileSystem) //should this have a partition???
            {
                Console.WriteLine($"Creating file system");
                MeadowFileManager.CreateFileSystem(MeadowDeviceManager.CurrentDevice);
            }
            else if(options.FormatFileSystem)
            {
                Console.WriteLine($"Format file system on partition {options.Partition}");
                MeadowFileManager.FormatFileSystem(MeadowDeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.ListFiles)
            {
                Console.WriteLine($"Getting list of a files on partition {options.Partition}");
                MeadowFileManager.ListFiles(MeadowDeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.ListFilesAndCrcs)
            {
                Console.WriteLine($"Getting list of a files and Crcs on partition {options.Partition}");
                MeadowFileManager.ListFilesAndCrcs(MeadowDeviceManager.CurrentDevice, options.Partition);
            }
            //Device manager
            else if(options.SetTraceLevel)
            {
                Console.WriteLine($"Setting trace level to {options.TraceLevel}");
                MeadowDeviceManager.SetTraceLevel(MeadowDeviceManager.CurrentDevice, options.TraceLevel);
            }
            else if(options.SetDeveloper1)
            {
                Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                MeadowDeviceManager.SetDeveloper1(MeadowDeviceManager.CurrentDevice, options.DeveloperValue);
            }
            else if (options.SetDeveloper2)
            {
                Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                MeadowDeviceManager.SetDeveloper2(MeadowDeviceManager.CurrentDevice, options.DeveloperValue);
            }
            else if (options.SetDeveloper3)
            {
                Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                MeadowDeviceManager.SetDeveloper3(MeadowDeviceManager.CurrentDevice, options.DeveloperValue);
            }
            else if (options.SetDeveloper4)
            {
                Console.WriteLine($"Setting developer level to {options.DeveloperValue}");
                MeadowDeviceManager.SetDeveloper4(MeadowDeviceManager.CurrentDevice, options.DeveloperValue);
            }

            else if (options.ToggleNsh)
            {
                Console.WriteLine($"Toggling Nsh");
                MeadowDeviceManager.ToggleNsh(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.MonoDisable)
            {
                MeadowDeviceManager.MonoDisable(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.MonoEnable)
            {
                MeadowDeviceManager.MonoEnable(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.MonoRunState)
            {
                MeadowDeviceManager.MonoRunState(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.GetDeviceId)
            {
                MeadowDeviceManager.DeviceId(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.ResetTargetMcu)
            {
                Console.WriteLine("Resetting Mcu");
                MeadowDeviceManager.ResetTargetMcu(MeadowDeviceManager.CurrentDevice);
            }
            else if (options.EnterDfuMode)
            {
                Console.WriteLine("Entering Dfu mode");
                MeadowDeviceManager.EnterDfuMode(MeadowDeviceManager.CurrentDevice);
            }
        }

        //temp code until we get the device manager logic in place 
        static void ConnectToMeadowDevice (string commPort)
		{
            var device = new MeadowDevice(commPort);
            try
            {
                device.Initialize();
                Console.WriteLine($"Port {commPort} opened");
            }
            catch (MeadowDeviceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            MeadowDeviceManager.CurrentDevice = device;
        }
    }
}
