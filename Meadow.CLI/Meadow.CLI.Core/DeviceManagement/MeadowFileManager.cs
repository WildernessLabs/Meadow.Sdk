using System;
using System.Diagnostics;
using System.IO;
using MeadowCLI.Hcom;

namespace MeadowCLI.DeviceManagement
{
    public static class MeadowFileManager
    {
        static HcomMeadowRequestType _meadowRequestType;

        const UInt16 REQUEST_HEADER_MASK = 0xff00;
        const UInt32 UNKNOWN_USER_DATA = 0xffffffff;

        const string MSCORLIB = "mscorlib.dll";
        const string SYSTEM = "System.dll";
        const string SYSTEM_CORE = "System.Core.dll";
        const string APP = "App.exe";

        static MeadowFileManager()
        {
        }

        public static bool DoesFileExist(MeadowDevice meadow, string fileName, int partition = 0)
        {
            //ToDo - need way to programmatically check for files
            return false;
        }

        public static void DeployRequiredBinaries(MeadowDevice meadow, bool forceDeploy = false)
        {
            //ToDo - should these be deployed via nuget?
            //Do we need any other dlls?
            if (forceDeploy || DoesFileExist(meadow, MSCORLIB, 0) == false)
                WriteFileToFlash(meadow, Path.Combine(@"./libs", MSCORLIB), MSCORLIB);

            if (forceDeploy || DoesFileExist(meadow, SYSTEM, 0) == false)
                WriteFileToFlash(meadow, Path.Combine(@"./libs", SYSTEM), SYSTEM);

            if (forceDeploy || DoesFileExist(meadow, MSCORLIB, 0) == false)
                WriteFileToFlash(meadow, Path.Combine(@"./libs", SYSTEM_CORE), SYSTEM_CORE);
        }

        public static void DeployAppInFolder(MeadowDevice meadow, string appFolder)
        {
            //ToDo - crawl current directory and look for dependencies to deploy 
            WriteFileToFlash(meadow, APP);

            var files = Directory.GetFiles(appFolder, "*.exe");

            foreach(var f in files)
            {
                if (f.ToLower().EndsWith(".exe"))
                {
                    WriteFileToFlash(meadow, f, "App.exe");
                }
                if (f.ToLower().EndsWith(".dll"))
                {
                    WriteFileToFlash(meadow, f, Path.GetFileName(f));
                }
            }
        }

        public static void WriteFileToFlash(MeadowDevice meadow, string fileName, string targetFileName = null, int partition = 0)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_START_FILE_TRANSFER;

            if (string.IsNullOrWhiteSpace(targetFileName))
                targetFileName = Path.GetFileName(fileName);

            TransmitFileInfoToExtFlash(meadow, _meadowRequestType, fileName, targetFileName, partition, false);
        }

        public static void DeleteFile(MeadowDevice meadow, string fileName, int partition)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DELETE_FILE_BY_NAME;

            TransmitFileInfoToExtFlash(meadow, _meadowRequestType, string.Empty, fileName, partition, true);

        }

        public static void EraseFlash(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_BULK_FLASH_ERASE;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void VerifyErasedFlash(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_VERIFY_ERASED_FLASH;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void PartitionFileSystem(MeadowDevice meadow, int numberOfPartitions)
        {
            if (numberOfPartitions > 1 || numberOfPartitions > 8)
                throw new IndexOutOfRangeException("Number of partitions must be between 1 & 8 inclusive");

            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_PARTITION_FLASH_FS;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)numberOfPartitions);
        }

        public static void MountFileSystem(MeadowDevice meadow, int partition)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_MOUNT_FLASH_FS;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)partition);
        }

        public static void InitializeFileSystem(MeadowDevice meadow, int partition)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_INITIALIZE_FLASH_FS;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)partition);
        }

        public static void CreateFileSystem(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_CREATE_ENTIRE_FLASH_FS;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void FormatFileSystem(MeadowDevice meadow, int partition)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_FORMAT_FLASH_FILE_SYS;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)partition);
        }

        //ToDo - find the output 
        public static void ListFiles(MeadowDevice meadow, int partition = 0)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_LIST_PARTITION_FILES;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)partition);
        }

        //ToDo - find the output 
        public static void ListFilesAndCrcs(MeadowDevice meadow, int partition = 0)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_LIST_PART_FILES_AND_CRC;
            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)partition);
        }

        //I don't think this is needed
        //it was used in the original code to determine if the request type alligned to a simple command
        //but simple commands are just commands that only require 0 or 1 numerical args 
        static HcomRqstHeaderType GetRequestHeaderType(HcomMeadowRequestType request)
        {
            if (((UInt16)_meadowRequestType & REQUEST_HEADER_MASK) == (UInt16)HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_UNDEFINED)
                return HcomRqstHeaderType.Undefined;

            if (((UInt16)_meadowRequestType & REQUEST_HEADER_MASK) == (UInt16)HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE)
                return HcomRqstHeaderType.Simple;

            if (((UInt16)_meadowRequestType & REQUEST_HEADER_MASK) == (UInt16)HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_FILE)
                return HcomRqstHeaderType.FileType;

            throw new InvalidOperationException(string.Format("Unknown request header type: {0}",
                _meadowRequestType));
        }


        private static void TransmitFileInfoToExtFlash(MeadowDevice meadow,
                            HcomMeadowRequestType requestType,
                            string sourceFileName, string targetFileName, int partition,
                            bool deleteFile)
        {
            var sw = new Stopwatch();

            var sendTargetData = new SendTargetData(meadow.SerialPort);

            try
            {
                //----------------------------------------------
                if (deleteFile == true)
                {
                    // No data packets and no end-of-file message
                    sendTargetData.BuildAndSendFileRelatedCommand(requestType,
                        (UInt32)partition, 0, 0, sourceFileName);
                    return;
                }

                // Open, read and close the data file
                var fileBytes = File.ReadAllBytes(sourceFileName);
                var fileCrc32 = CrcTools.Crc32part(fileBytes, fileBytes.Length, 0);
                var fileLength = fileBytes.Length;

                sw.Start();
                sw.Restart();

                sendTargetData.SendTheEntireFile(targetFileName, (uint)partition,
                    fileBytes, fileCrc32);

                sw.Stop();

                Console.WriteLine("It took {0:N0} millisec to send {1} bytes. FileCrc:{2:x08}",
                    sw.ElapsedMilliseconds, fileLength, fileCrc32);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown exception:{ex}");
            }
        }

        enum HcomProtocolHeaderTypes : UInt16
        {
            HCOM_PROTOCOL_HEADER_TYPE_UNDEFINED = 0x0000,
            // Simple request types, include 4-byte user data
            HCOM_PROTOCOL_HEADER_TYPE_SIMPLE = 0x0100,
            // File releted request types, includes 4-byte user data (for the
            // destination partition id), 4-byte file size, 4-byte checksum and
            // variable length destition file name.
            HCOM_PROTOCOL_HEADER_TYPE_FILE = 0x0200,
        }

        // Messages to be sent to Meadow board
        public enum HcomMeadowRequestType : UInt16
        {
            HCOM_MDOW_REQUEST_UNDEFINED_REQUEST = 0x00 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_UNDEFINED,

            HCOM_MDOW_REQUEST_CREATE_ENTIRE_FLASH_FS = 0x01 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_CHANGE_TRACE_LEVEL = 0x02 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_FORMAT_FLASH_FILE_SYS = 0x03 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_END_FILE_TRANSFER = 0x04 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_RESET_PRIMARY_MCU = 0x05 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_VERIFY_ERASED_FLASH = 0x06 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_PARTITION_FLASH_FS = 0x07 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_MOUNT_FLASH_FS = 0x08 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_INITIALIZE_FLASH_FS = 0x09 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_BULK_FLASH_ERASE = 0x0a | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_ENTER_DFU_MODE = 0x0b | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_ENABLE_DISABLE_NSH = 0x0c | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_LIST_PARTITION_FILES = 0x0d | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_LIST_PART_FILES_AND_CRC = 0x0e | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_MONO_DISABLE = 0x0f | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_MONO_ENABLE = 0x10 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,

            // Only used for testing
            HCOM_MDOW_REQUEST_DEVELOPER_1 = 0xf0 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_DEVELOPER_2 = 0xf1 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_DEVELOPER_3 = 0xf2 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,
            HCOM_MDOW_REQUEST_DEVELOPER_4 = 0xf3 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_SIMPLE,

            HCOM_MDOW_REQUEST_START_FILE_TRANSFER = 0x01 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_FILE,
            HCOM_MDOW_REQUEST_DELETE_FILE_BY_NAME = 0x02 | HcomProtocolHeaderTypes.HCOM_PROTOCOL_HEADER_TYPE_FILE,
        }

        public enum HcomRqstHeaderType
        {
            Undefined = 0x0000,
            Simple = 0x0000,
            FileType = 0xff0000,
        }
    }
}