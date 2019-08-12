﻿using CommandLine;

namespace MeadowCLI
{
    public class Options
    {
        [Option('d', "Dfu", Required = false, HelpText = "DFU copy os and user files. Looks for files in execution direction. To override, user 'OsFile' and 'UserFile'.")]
        public bool Dfu { get; set; }
        [Option(longName: "OsFile", Default = null, Required = false, HelpText = "File path to os file. Usage: --osFile mypath")]
        public string DfuOsPath { get; set; }
        [Option(longName: "UserFile", Default = null, Required = false, HelpText = "File path to user file. Usage: --userFile mypath")]
        public string DfuUserPath { get; set; }

        [Option(longName: "WriteFile", Required = false, HelpText = "Write an external file to Meadow's internal flash")]
        public bool WriteFile { get; set; }
        [Option(longName: "DeleteFile", Required = false, HelpText = "Delete a file in Meadow's internal flash")]
        public bool DeleteFile { get; set; }
        [Option(longName: "EraseFlash", Required = false, HelpText = "Delete all content in Meadow flash")]
        public bool EraseFlash { get; set; }
        [Option(longName: "VerifyErasedFlash", Required = false, HelpText = "Verify the contents of the flash were deleted")]
        public bool VerifyErasedFlash { get; set; }
        [Option(longName: "PartitionFileSystem", Required = false, HelpText = "Partition Meadow's internal flash")]
        public bool PartitionFileSystem { get; set; }
        [Option(longName: "MountFileSystem", Required = false, HelpText = "Mount file system in Meadow's internal flash")]
        public bool MountFileSystem { get; set; }
        [Option(longName: "InitializeFileSystem", Required = false, HelpText = "Initialize file system in Meadow's internal flash")]
        public bool InitFileSystem { get; set; }
        [Option(longName: "CreateFileSystem", Required = false, HelpText = "Create a new file system in Meadow's internal flash")]
        public bool CreateFileSystem { get; set; }
        [Option(longName: "FormatFileSystem", Required = false, HelpText = "Format file system in Meadow's internal flash")]
        public bool FormatFileSystem { get; set; }

        [Option(longName: "SetDeveloperLevel", Required = false, HelpText = "Set the developer level (1 - 4)")]
        public bool SetDeveloperLevel { get; set; }
        [Option(longName: "SetTraceLevel", Required = false, HelpText = "Change the debug trace level (0 - 3)")]
        public bool SetTraceLevel { get; set; }
        [Option('r', longName: "ResetTargetMcu", Required = false, HelpText = "Reset the MCU on Meadow")]
        public bool ResetTargetMcu { get; set; }
        [Option(longName: "EnterDfuMode", Required = false, HelpText = "Set Meadow in DFU mode")]
        public bool EnterDfuMode { get; set; }
        [Option(longName: "ToggleNsh", Required = false, HelpText = "Turn NSH mode on or off")]
        public bool ToggleNsh { get; set; }
        [Option(longName: "ListFiles", Required = false, HelpText = "List all files in a Meadow partition")]
        public bool ListFiles { get; set; }
        [Option(longName: "ListFilesAndCrcs", Required = false, HelpText = "List all files and CRCs in a Meadow partition")]
        public bool ListFilesAndCrcs { get; set; }

        [Option('s', longName: "SerialPort", Default = "/dev/tty.usbmodem01", Required = false, HelpText = "Specify the serial port used by Meadow")]
        public string SerialPort { get; set; }
        [Option('f', longName: "File", Default = null, Required = false, HelpText = "Local file to send to Meadow")]
        public string FileName { get; set; }
        [Option(longName: "TargetFileName", Default = null, Required = false, HelpText = "Filename to be written to Meadow (can be different from source name")]
        public string TargetFileName { get; set; }
        [Option('p', "Parition", Default = 0, Required = false, HelpText = "Destination partition on Meadow")]
        public int Partition { get; set; }
        [Option('n', "NumberOfPartitions", Default = 1, Required = false, HelpText = "The number of partitions to create on Meadow")]
        public int NumberOfPartitions { get; set; }
        [Option('t', "TraceLevel", Default = 1, Required = false, HelpText = "Change the amount of debug information provided by the OS")]
        public int TraceLevel { get; set; }
        [Option('d', "DeveloperLevel", Default = 1, Required = false, HelpText = "Change the developer level")]
        public int DeveloperLevel { get; set; }
    }
}