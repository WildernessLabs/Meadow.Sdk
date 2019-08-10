using CommandLine;
using System;
using MeadowCLI.DeviceManagement;

namespace MeadowCLI
{
    class Program
    {
        static void Main(string[] args)
        {
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
                    ProcessHcom(options);
				}
			});

            Console.ReadKey();
        }

        //Probably rename
        static void ProcessHcom(Options options)
        {
            ConnectToMeadowDevice(options.SerialPort);

            if (options.WriteFile)
            {
                MeadowFileManager.WriteFileToFlash(DeviceManager.CurrentDevice,
                    options.FileName, options.TargetFileName, options.Partition);
            }
            else if (options.DeleteFile)
            {
                MeadowFileManager.DeleteFile(DeviceManager.CurrentDevice,
                    options.TargetFileName, options.Partition);
            }
            else if (options.EraseFlash)
            {
                MeadowFileManager.EraseFlash(DeviceManager.CurrentDevice);
            }
            else if (options.VerifyErasedFlash)
            {
                MeadowFileManager.VerifyErasedFlash(DeviceManager.CurrentDevice);
            }
            else if (options.PartitionFileSystem)
            {
                MeadowFileManager.PartitionFileSystem(DeviceManager.CurrentDevice, options.NumberOfPartitions);
            }
            else if (options.MountFileSystem)
            {
                MeadowFileManager.MountFileSystem(DeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.InitFileSystem)
            {
                MeadowFileManager.InitializeFileSystem(DeviceManager.CurrentDevice, options.Partition);
            }
            else if(options.CreateFileSystem)
            {
                MeadowFileManager.CreateFileSystem(DeviceManager.CurrentDevice);
            }
            else if(options.FormatFileSystem)
            {
                MeadowFileManager.FormatFileSystem(DeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.ListFiles)
            {
                MeadowFileManager.ListFiles(DeviceManager.CurrentDevice, options.Partition);
            }
            else if (options.ListFilesAndCrcs)
            {
                MeadowFileManager.ListFilesAndCrcs(DeviceManager.CurrentDevice, options.Partition);
            }
            //Device manager
            else if(options.SetTraceLevel)
            {
                DeviceManager.SetTraceLevel(DeviceManager.CurrentDevice, options.TraceLevel);
            }
            else if(options.SetDeveloperLevel)
            {
                DeviceManager.SetDeveloperLevel(DeviceManager.CurrentDevice, options.DeveloperLevel);
            }
            else if (options.ToggleNsh)
            {
                DeviceManager.ToggleNsh(DeviceManager.CurrentDevice);
            }
            else if (options.ResetTargetMcu)
            {
                DeviceManager.ResetTargetMcu(DeviceManager.CurrentDevice);
            }
            else if (options.EnterDfuMode)
            {
                DeviceManager.EnterDfuMode(DeviceManager.CurrentDevice);
            }
        }

        //temp code until we get the device manager logic in place 
        static void ConnectToMeadowDevice (string commPort)
		{
			DeviceManager.CurrentDevice = new MeadowDevice(commPort);

		}
    }
}