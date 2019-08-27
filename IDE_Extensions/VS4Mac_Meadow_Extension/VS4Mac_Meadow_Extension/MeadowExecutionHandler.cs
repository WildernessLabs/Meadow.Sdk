using MonoDevelop.Debugger;
using MonoDevelop.Core.Execution;
using MeadowCLI.DeviceManagement;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    class MeadowExecutionHandler : IExecutionHandler
    {
        public bool CanExecute(ExecutionCommand command)
        {   //returning false here swaps the play button with a build button 
            return (command is MeadowExecutionCommand);
        }

        public ProcessAsyncOperation Execute(ExecutionCommand command, OperationConsole console)
        {
            var cmd = command as MeadowExecutionCommand;

            var cts = new CancellationTokenSource();
            var deployTask = DeployApp(cmd.OutputDirectory, cts.Token);

            return new ProcessAsyncOperation(deployTask, cts);
        }
        
        //lazy job with the cancellation token but more or less good enough
        async Task DeployApp(string folder, CancellationToken token)
        {
            //hack in the app deployment
            var meadow = MeadowDeviceManager.CurrentDevice;

            if (meadow.SerialPort != null)
                meadow.SerialPort.Close();
            
            meadow.Initialize(false);
            MeadowDeviceManager.MonoDisable(meadow);
            await Task.Delay(5000); //hack for testing
            meadow.SerialPort.Close();
            meadow.Initialize();

            var files = await meadow.GetFilesOnDevice();

            if (token.IsCancellationRequested)
                return;

            Debug.WriteLine("Checking for installed binaries");
            foreach (var f in files)
            {
                Debug.WriteLine($"Found {f} on Meadow");
            }

            if (token.IsCancellationRequested)
                return;

            Debug.WriteLine("Deploying required libraries (this may take several minutes)");
            await meadow.DeployRequiredLibs(folder);

            if (token.IsCancellationRequested)
                return;

            Debug.WriteLine("Deploying application");
            await meadow.DeployApp(folder);

            Debug.WriteLine("Resetting Meadow to start application");

            MeadowDeviceManager.MonoEnable(meadow);
            MeadowDeviceManager.ResetTargetMcu(meadow);
        }
    }
}