using MonoDevelop.Core.Execution;
using MeadowCLI.DeviceManagement;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MonoDevelop.Core;
using System.Collections.Generic;
using System;

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
            var deployTask = DeployApp(cmd.Target as MeadowDeviceExecutionTarget, cmd.OutputDirectory, cts);

            return new ProcessAsyncOperation(deployTask, cts);
        }

        //lazy job with the cancellation token but more or less good enough
        //https://stackoverflow.com/questions/29798243/how-to-write-to-the-tool-output-pad-from-within-a-monodevelop-add-in
        async Task DeployApp(MeadowDeviceExecutionTarget target, string folder, CancellationTokenSource cts)
        {
            ProgressMonitor monitor = MonoDevelop.Ide.IdeApp.Workbench.ProgressMonitors.GetToolOutputProgressMonitor(false, cts);
            monitor.BeginTask("Deploying to Meadow ...", 1);

            try
            {
                var meadow = target.MeadowDevice;

                if(await InitializeMeadowDevice(meadow, monitor, cts) == false)
                {
                    throw new Exception("Failed to initialize Meadow");
                }

                await GetFilesOnDevice(meadow, monitor, cts);

                await DeployRequiredLibraries(meadow, monitor, cts, folder);

                await DeployMeadowApp(meadow, monitor, cts, folder);

                await Task.Run(() => ResetMeadowAndStartMono(meadow, monitor, cts));
            }
            catch (Exception ex)
            {
                await monitor.ErrorLog.WriteLineAsync($"Error: {ex.Message}");
            }
            finally
            {
                monitor.EndTask();
                monitor.Dispose();
            }
        }

        async Task<bool> InitializeMeadowDevice(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested) return true;

            await monitor.Log.WriteLineAsync("Initializing Meadow");

            if (meadow == null)
            {
                monitor.ErrorLog.WriteLine("Can't read Meadow device");
                return false;
            }

            if (meadow.SerialPort != null)
            {
                meadow.SerialPort.Close();
            }

            meadow.Initialize(false);
            MeadowDeviceManager.MonoDisable(meadow);
            await Task.Delay(5000); //hack for testing

            if(meadow.Initialize() == false)
            {
                monitor.ErrorLog.WriteLine("Couldn't initialize serial port");
                return false;
            }
            return true;
        }

        async Task<List<string>> GetFilesOnDevice(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested) { return new List<string>(); }

            var files = await meadow.GetFilesOnDevice();

            await monitor.Log.WriteLineAsync("Checking files on device");

            foreach (var f in files)
            {
                await monitor.Log.WriteLineAsync($"Found {f}").ConfigureAwait(false);
            }

            return files;
        }

        async Task WriteFileToMeadow(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts, string folder, string file, bool overwrite = false)
        {
            if (cts.IsCancellationRequested) { return; }

            if (overwrite || await meadow.IsFileOnDevice(file).ConfigureAwait(false) == false)
            {
                await monitor.Log.WriteLineAsync($"Writing {file}").ConfigureAwait(false);
                await meadow.WriteFile(file, folder).ConfigureAwait(false);
            }
        }

        async Task DeployRequiredLibraries(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts, string folder)
        {
            await monitor.Log.WriteLineAsync("Deploying required libraries (this may take several minutes)");

            await WriteFileToMeadow(meadow, monitor, cts, folder, MeadowDevice.SYSTEM);
            await WriteFileToMeadow(meadow, monitor, cts, folder, MeadowDevice.SYSTEM_CORE);
            await WriteFileToMeadow(meadow, monitor, cts, folder, MeadowDevice.MEADOW_CORE);
            await WriteFileToMeadow(meadow, monitor, cts, folder, MeadowDevice.MSCORLIB);
        }

        async Task DeployMeadowApp(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts, string folder)
        {
            if (cts.IsCancellationRequested) { return; }

            await monitor.Log.WriteLineAsync("Deploying executable and dependencies");
            await meadow.DeployApp(folder);
        }

        void ResetMeadowAndStartMono(MeadowDevice meadow, ProgressMonitor monitor, CancellationTokenSource cts)
        {
            if(cts.IsCancellationRequested) { return; }

            monitor.ReportSuccess("Resetting Meadow and starting app (30-60s)");

            MeadowDeviceManager.MonoEnable(meadow);
            MeadowDeviceManager.ResetTargetMcu(meadow);
        }
    }
}
