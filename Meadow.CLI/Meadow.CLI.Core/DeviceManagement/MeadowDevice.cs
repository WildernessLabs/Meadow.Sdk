using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using MeadowCLI.Hcom;

namespace MeadowCLI.DeviceManagement
{
    public class MeadowDeviceException : Exception
    {
        public MeadowDeviceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    //a simple model object that represents a meadow device including connection
    public class MeadowDevice
    {
        public SerialPort SerialPort { get; private set; }

        public string Name { get; private set; } = "Meadow Micro F7";

        public string Model { get; private set; } = "Micro F7";
        
        public string Id { get; set; } //guessing we'll need this

        private MeadowSerialDataProcessor dataProcessor;

        private string serialPortName;

        private List<string> filesOnDevice = new List<string>();

        public MeadowDevice(string serialPortName, string deviceName = null)
        {
            if(string.IsNullOrWhiteSpace(deviceName) == false)
                Name = deviceName; //otherwise use the default

            Id = new Guid().ToString();

            this.serialPortName = serialPortName;
        }

        //putting this here for now ..... 
        public void OpenSerialPort()
        {
            try
            {   // Create a new SerialPort object with default settings
                var port = new SerialPort
                {
                    PortName = serialPortName,
                    BaudRate = 115200,       // This value is ignored when using ACM
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,

                    // Set the read/write timeouts
                    ReadTimeout = 5000,
                    WriteTimeout = 5000
                };

                port.Open();

                //improves perf on Windows?
                port.BaseStream.ReadTimeout = 0;

                SerialPort = port;
            }
            catch (IOException ioEx)
            {
                throw new MeadowDeviceException($"The specified port '{serialPortName}' could not be found or opened.", ioEx);
            }
            catch (Exception ex)
            {
                throw new MeadowDeviceException($"Unknown exception", ex);
            }

            ListenForSerialData(); //I don't love this here .... move later
        }

        private void ListenForSerialData ()
        {
            if (SerialPort != null)
            {
                dataProcessor = new MeadowSerialDataProcessor(SerialPort);

                dataProcessor.OnReceivedData += DataReceived;
                dataProcessor.OnReceivedFileList += FileListReceived;
                dataProcessor.OnReceivedMonoMsg += MonoMsgReceived;
            }
        }

        void DataReceived (object sender, MeadowMessageEventArgs args)
        {
            //console out until we know we need to do something further
            Console.WriteLine("Data: " + args.Message);
        }

        void MonoMsgReceived(object sender, MeadowMessageEventArgs args)
        {
            //console out until we know we need to do something further
            Console.WriteLine("Mono: " + args.Message);
        }

        void FileListReceived(object sender, MeadowMessageEventArgs args)
        {
            var fileList = args.Message.Split(',');

            filesOnDevice.Clear();

            foreach(var path in fileList)
            {
                var file = path.Substring(path.LastIndexOf('/') + 1);
                filesOnDevice.Add(file);
                Console.WriteLine(file);
            }
        }
    }
}