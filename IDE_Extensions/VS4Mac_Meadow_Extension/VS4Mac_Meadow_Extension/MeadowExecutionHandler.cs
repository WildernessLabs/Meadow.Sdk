using MonoDevelop.Debugger;
using MonoDevelop.Core.Execution;
using MeadowCLI.DeviceManagement;
using System.IO;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    class MeadowExecutionHandler : IExecutionHandler
    {
        public bool CanExecute(ExecutionCommand command)
        {
            return (command is MeadowExecutionCommand);
        }

        public ProcessAsyncOperation Execute(ExecutionCommand command, OperationConsole console)
        {
            System.Console.WriteLine("Executing");

            var handler = DebuggingService.GetExecutionHandler();

            var ret = DebuggingService.GetExecutionHandler().Execute(command, console);

            var cmd = command as MeadowExecutionCommand;

            DeployApp(cmd.OutputDirectory);


            return ret;
        }

        void DeployApp(string folder)
        {

            //hack in the app deployment

            if (DeviceManager.CurrentDevice.SerialPort == null)
                DeviceManager.CurrentDevice.OpenSerialPort();

       //     DeviceManager.MonoDisable(DeviceManager.CurrentDevice);

            DeviceManager.SetTraceLevel(DeviceManager.CurrentDevice, 2);

            MeadowFileManager.ListFilesAndCrcs(DeviceManager.CurrentDevice);

         //   MeadowFileManager.DeployRequiredBinaries(DeviceManager.CurrentDevice);
            
            MeadowFileManager.DeployAppInFolder(DeviceManager.CurrentDevice, folder);

            DeviceManager.MonoEnable(DeviceManager.CurrentDevice);
        }
    }
}