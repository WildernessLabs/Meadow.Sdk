using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
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
        const string MSCORLIB = "mscorlib.dll";
        const string SYSTEM = "System.dll";
        const string SYSTEM_CORE = "System.Core.dll";
        const string MEADOW_CORE = "Meadow.Core.dll";
        const string APP_EXE = "App.exe";

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

        public void Initialize (bool listen = true)
        {
            if(SerialPort != null)
            {
                SerialPort.Close();
                SerialPort = null;
            }

            OpenSerialPort();

            if(listen == true)
                ListenForSerialData();
        }

        public async Task DeployRequiredLibs(string path, bool forceUpdate = false)
        {
            if(forceUpdate || await IsFileOnDevice(SYSTEM).ConfigureAwait(false) == false)
            {
                await WriteFile(SYSTEM, path).ConfigureAwait(false);
            }

            if (forceUpdate || await IsFileOnDevice(SYSTEM_CORE).ConfigureAwait(false) == false)
            {
                await WriteFile(SYSTEM_CORE, path).ConfigureAwait(false);
            }

            if (forceUpdate || await IsFileOnDevice(MSCORLIB).ConfigureAwait(false) == false)
            {
                await WriteFile(MSCORLIB, path).ConfigureAwait(false);
            }

            if (forceUpdate || await IsFileOnDevice(MEADOW_CORE).ConfigureAwait(false) == false)
            {
                await WriteFile(MEADOW_CORE, path).ConfigureAwait(false);
            }
        }

        public Task<bool> DeployApp(string path)
        {
            return WriteFile(APP_EXE, path);
        }

        public async Task<bool> WriteFile(string filename, string path, int timeoutInMs = 200000) //200s 
        {
            if (SerialPort == null)
            {
                throw new Exception("SerialPort not intialized");
            }

            bool result = false;

            var timeOutTask = Task.Delay(timeoutInMs);

            EventHandler<MeadowMessageEventArgs> handler = null;

            var tcs = new TaskCompletionSource<bool>();

            handler = (s, e) =>
            {
                if (e.Message.Contains("File Sent Successfully"))
                {
                    result = true;
                    tcs.SetResult(true);
                }
            };
            dataProcessor.OnReceivedData += handler;

            MeadowFileManager.WriteFileToFlash(this, Path.Combine(path, filename), filename);

            await Task.WhenAny(new Task[] { timeOutTask, tcs.Task });
            dataProcessor.OnReceivedData -= handler;

            return result;
        }

        public async Task<List<string>> GetFilesOnDevice(bool refresh = false, int timeoutInMs = 10000)
        {
            if (SerialPort == null)
            {
                throw new Exception("SerialPort not intialized");
            }

            if(filesOnDevice.Count == 0 || refresh == true)
            {
                var timeOutTask = Task.Delay(timeoutInMs);

                EventHandler<MeadowMessageEventArgs> handler = null;

                var tcs = new TaskCompletionSource<bool>();

                handler = (s, e) =>
                {
                    SetFilesOnDeviceFromMessage(e.Message);
                    tcs.SetResult(true);
                };
                dataProcessor.OnReceivedFileList += handler;

                MeadowFileManager.ListFiles(this);

                await Task.WhenAny(new Task[] { timeOutTask, tcs.Task});
                dataProcessor.OnReceivedFileList -= handler;
            }

            return filesOnDevice;
        }

        public Task<bool> IsFileOnDevice (string filename)
        {
            return Task.FromResult(filesOnDevice.Contains(filename));
        }

        //putting this here for now ..... 
        void OpenSerialPort()
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
            SetFilesOnDeviceFromMessage(args.Message);

            foreach (var f in filesOnDevice)
                Console.WriteLine(f);
        }

        void SetFilesOnDeviceFromMessage(string message)
        {
            var fileList = message.Split(',');

            filesOnDevice.Clear();

            foreach (var path in fileList)
            {
                var file = path.Substring(path.LastIndexOf('/') + 1);
                filesOnDevice.Add(file);
            }
        }
    }
}