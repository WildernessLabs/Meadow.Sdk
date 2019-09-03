using MonoDevelop.Core.Execution;
using MeadowCLI.DeviceManagement;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

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
            var deployTask = DeployApp(cmd.Target, cmd.OutputDirectory, cts);

            return new ProcessAsyncOperation(deployTask, cts);
        }

        //lazy job with the cancellation token but more or less good enough
        //https://stackoverflow.com/questions/29798243/how-to-write-to-the-tool-output-pad-from-within-a-monodevelop-add-in
        async Task DeployApp(ExecutionTarget target, string folder, CancellationTokenSource cts)
        {
            var monitor = MonoDevelop.Ide.IdeApp.Workbench.ProgressMonitors.GetToolOutputProgressMonitor(false, cts);
            monitor.BeginTask("Deploying to Meadow ...", 1);

            //hack in the app deployment
            MeadowDevice meadow = MeadowDeviceManager.AttachedDevices.Where(d => d.Id == target.Id).First();

            if(meadow == null)
            {
                monitor.ErrorLog.Write("Can't read Meadow device");
                monitor.EndTask();
                monitor.Dispose();
                return;
            }

            if (meadow.SerialPort != null)
            {
                meadow.SerialPort.Close();
            }
            
            meadow.Initialize(false);
            MeadowDeviceManager.MonoDisable(meadow);
            await Task.Delay(5000); //hack for testing
            meadow.Initialize();

            var files = await meadow.GetFilesOnDevice();

            if (cts.IsCancellationRequested) { return; }

            monitor.Log.Write("Checking for installed binaries");
            foreach (var f in files)
            {
                await monitor.Log.WriteLineAsync($"Found {f}").ConfigureAwait(false);
            }

            if (cts.IsCancellationRequested) { return; }

            monitor.Log.Write("Deploying required libraries (this may take several minutes)");
            await meadow.DeployRequiredLibs(folder);

            if (cts.IsCancellationRequested) { return; }

            monitor.Log.Write("Deploying executable");
            await meadow.DeployApp(folder);

            monitor.EndTask();
            monitor.ReportSuccess("Resetting Meadow and starting application");

            MeadowDeviceManager.MonoEnable(meadow);
            MeadowDeviceManager.ResetTargetMcu(meadow);

            monitor.Dispose();
        }
    }
}