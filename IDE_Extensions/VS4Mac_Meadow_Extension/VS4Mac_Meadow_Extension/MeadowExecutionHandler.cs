using MonoDevelop.Debugger;
using MonoDevelop.Core.Execution;
using MeadowCLI.DeviceManagement;
using System.IO;
using System.Threading.Tasks;

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

          //  var ret = handler.Execute(command, console);

            var cmd = command as MeadowExecutionCommand;

            DeployApp(cmd.OutputDirectory);
            

            return null;
        }

        async Task DeployApp(string folder)
        {
            //hack in the app deployment
            var meadow = MeadowDeviceManager.CurrentDevice;

            if (meadow.SerialPort == null)
                meadow.Initialize();

            var files = await meadow.GetFilesOnDevice();

            MeadowFileManager.DeployRequiredBinaries(meadow);

       //     DeviceManager.MonoDisable(DeviceManager.CurrentDevice);

            MeadowDeviceManager.SetTraceLevel(MeadowDeviceManager.CurrentDevice, 2);

         //   MeadowFileManager.ListFilesAndCrcs(MeadowDeviceManager.CurrentDevice);

         //   MeadowFileManager.DeployRequiredBinaries(DeviceManager.CurrentDevice);
            
         //   MeadowFileManager.DeployAppInFolder(MeadowDeviceManager.CurrentDevice, folder);

         //   MeadowDeviceManager.MonoEnable(MeadowDeviceManager.CurrentDevice);
        }
    }
}