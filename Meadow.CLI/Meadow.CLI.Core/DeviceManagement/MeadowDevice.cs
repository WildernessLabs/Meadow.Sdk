using System;
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
        public SerialPort SerialPort { get; set; }

        public string Name { get; private set; } = "Meadow Micro F7";

        public string Model { get; private set; } = "Micro F7";
        
        public string Id { get; set; } //guessing we'll need this

        private ReceiveTargetData receiveData;
        private HcomBufferReturn hcomBuffer;

        private string serialPortName;

        public MeadowDevice(string serialPortName, string deviceName = null)
        {
            if(string.IsNullOrWhiteSpace(deviceName) == false)
                Name = deviceName; //otherwise use the default

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
                    ReadTimeout = 500,
                    WriteTimeout = 500
                };

                port.Open();

                SerialPort = port;

                //wire up ReceiveTargetData
                //consider refactoring later
                if (SerialPort != null)
                {
                    HostCommBuffer hostCommBuffer = new HostCommBuffer();

                    receiveData = new ReceiveTargetData(SerialPort, hostCommBuffer);
                }
            }
            catch (IOException ioEx)
            {
                throw new MeadowDeviceException($"The specified port '{serialPortName}' could not be found or opened.", ioEx);
            }
            catch (Exception ex)
            {
                throw new MeadowDeviceException($"Unknown exception", ex);
            }
        }
    }
}
