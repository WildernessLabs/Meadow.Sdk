using System;
using System.IO;
using System.IO.Ports;
using MeadowCLI.Hcom;

namespace MeadowCLI.DeviceManagement
{
    //a simple model object that represents a meadow device including connection
    public class MeadowDevice
    {
        public SerialPort SerialPort { get; set; }

        public string Name { get; private set; } = "Meadow Micro F7";

        public string Model { get; private set; } = "Micro F7";
        
        public string Id { get; set; } //guessing we'll need this

        private ReceiveTargetData receiveData;
        private HcomBufferReturn hcomBuffer;

        public MeadowDevice(string serialPortName, string deviceName = null)
        {
            if(string.IsNullOrWhiteSpace(deviceName) == false)
                Name = deviceName; //otherwise use the default

            SerialPort = OpenSerialPort(serialPortName);

            //wire up ReceiveTargetData
            //consider refactoring later
            if (SerialPort != null)
            {
                HostCommBuffer hostCommBuffer = new HostCommBuffer();

                receiveData = new ReceiveTargetData(SerialPort, hostCommBuffer);
            }
        }

        //putting this here for now ..... 
        private SerialPort OpenSerialPort(string portName)
        {
            try
            {   // Create a new SerialPort object with default settings
                var port = new SerialPort
                {
                    PortName = portName,
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
                Console.WriteLine($"Port {portName} opened");

                return port;
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"The specified port '{portName}' could not be found or opened. Exception:'{ioEx}'");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown exception: {ex}");
                throw;
            }
        }
    }
}
