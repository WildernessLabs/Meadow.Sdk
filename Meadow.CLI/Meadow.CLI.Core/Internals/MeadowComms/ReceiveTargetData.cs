using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace MeadowCLI.Hcom
{
    public class ReceiveTargetData
    {
        const int MAX_RECEIVED_BYTES = 2048;

        SerialPort _serialPort;
        
        string F7ReadFileListPrefix { get { return "FileList: "; } }
        string F7MonoMessagePrefix { get { return "MonoMsg: "; } }

        //Timer _readTimer;
        //readonly byte[] _prevRecvUnusedBytes = new byte[MAX_RECEIVED_BYTES * 2];

        // It seems that the .Net SerialPort class is not all it could be.
        // To acheive reliable operation some SerialPort class methods must
        // not be used. When receiving, the BaseStream must be used.
        // The following blog post was very helpful.
        // http://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport

        //-------------------------------------------------------------
        // Constructor
        public ReceiveTargetData(SerialPort serialPort)
        {
            _serialPort = serialPort;
            Task.Run(() => ReadPortAsync());
        }

        //-------------------------------------------------------------
        // All received data handled here
        private async Task ReadPortAsync()
        {
            int receivedLength = 0;
            int unusedOffset = 0;
            byte[] buffer = new byte[MAX_RECEIVED_BYTES * 2];
            _serialPort.BaseStream.ReadTimeout = 0;     // Improves behavior?

            while(true)
            {
                try
                {
                    receivedLength = await _serialPort.BaseStream.ReadAsync(buffer, unusedOffset, MAX_RECEIVED_BYTES);
                    unusedOffset = AddDataToBuffer(buffer, receivedLength + unusedOffset);
                    Debug.Assert(unusedOffset > -1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex} usually means the Target dropped the connection");
                    break;
                }
            }
        }

        int AddDataToBuffer(byte[] buffer, int availableBytes)
        {
            // Because of the way characters are received we must buffer until the terminating cr/lf
            // is detected. This implememtation is a quick and dirty way.
            byte[] foundData  = new byte [MAX_RECEIVED_BYTES];
            int bytesUsed = 0;
            int recvOffset = 0;
            int foundOffset;
            do
            {
                Array.Clear(foundData, 0, MAX_RECEIVED_BYTES);      // FOR DEBUGGING

                for (foundOffset = 0;
                    recvOffset < availableBytes;
                    recvOffset++, foundOffset++)
                {
                    if (buffer[recvOffset] == '\r' && buffer[recvOffset + 1] == '\n')
                    {
                        foundData[foundOffset] = buffer[recvOffset];
                        foundData[foundOffset + 1] = buffer[recvOffset + 1];
                        recvOffset += 2;
                        break;
                    }
                    else
                    {
                        foundData[foundOffset] = buffer[recvOffset];
                    }
                }

                if (foundData[foundOffset + 1] == '\n')
                {
                    var rcvdString = Encoding.UTF8.GetString(foundData, 0, foundOffset + 2);
                    bytesUsed += foundOffset + 2;

                    if (rcvdString.StartsWith(F7ReadFileListPrefix))
                    {
                        // This is a comma separated list
                        string baseMessage = rcvdString.Substring(F7ReadFileListPrefix.Length);
                        DisplayFileList(baseMessage);
                    }
                    else if (rcvdString.StartsWith(F7MonoMessagePrefix))
                    {
                        string baseMessage = rcvdString.Substring(F7MonoMessagePrefix.Length);
                        Console.Write($"runtime: {baseMessage}");
                    }
                    else
                    {
                        Console.Write($"Received: {rcvdString}");
                    }
                }

            } while (foundData[foundOffset + 1] == '\n');

            return availableBytes - bytesUsed;        // No full message remains
        }

        //-------------------------------------------------------------
        void DisplayFileList(string receivedTextMsg)
        {
            Console.WriteLine($"File List:");

            var fileList = receivedTextMsg.Split(',');

            for (int i = 0; i < fileList.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {fileList[i]}");
            }
        }
    }
}