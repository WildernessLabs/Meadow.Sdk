using Meadow.CLI.Core.DeviceManagement;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace Meadow.SDK
{
	public class MeadowDeployTask : Xamarin.Build.AsyncTask
	{
		[Required]
		public string OutputPath { get; set; }

		public string MeadowDevicePort { get; set; }

		public int DebugPort { get; set; } = -1;

		MSBuildLogger Logger;

		public override bool Execute()
		{
			Logger = new MSBuildLogger(nameof(MeadowDeployTask), Log);

			System.Threading.Tasks.Task.Run(async () =>
			{
				try
				{
					await ExecuteAsync();
				}
				catch (System.Exception ex)
				{
					Log.LogErrorFromException(ex);
				}
				finally
				{
					// Invoke Complete to signal you're done.
					Complete();
				}
			});

			return base.Execute();
		}

		async System.Threading.Tasks.Task ExecuteAsync()
		{
			if (string.IsNullOrEmpty(MeadowDevicePort))
			{
				var availableDevicePorts = await MeadowDeviceManager.GetSerialPorts() ?? new List<string>();

				MeadowDevicePort = availableDevicePorts?.FirstOrDefault();
			}

			if (string.IsNullOrEmpty (MeadowDevicePort))
			{
				throw new System.ArgumentNullException("No Meadow Device Port specified");
			}

			var deployer = new MeadowDeployer(Logger, MeadowDevicePort, CancellationToken);

			await deployer.Deploy(OutputPath, DebugPort);
		}
	}
}
