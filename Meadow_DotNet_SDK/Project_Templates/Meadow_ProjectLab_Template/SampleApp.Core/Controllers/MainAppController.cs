using SampleApp.Commands;
using SampleApp.Controllers;
using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleApp.Hardware;

namespace SampleApp.Controllers
{
	public class MainAppController
	{
        protected ISampleAppHardware Hardware { get; set; }
        protected DisplayController displayController;
        protected CloudLogger cloudLogger;


        public MainAppController(ISampleAppHardware hardware, bool isSimulator = false)
		{
            Hardware = hardware;

            //==== setup our cloud logger
            cloudLogger = new CloudLogger(LogLevel.Warning);
            Resolver.Log.AddProvider(cloudLogger);
            Resolver.Services.Add(cloudLogger);
            Resolver.Log.Info($"{(cloudLogger is null ? "Cloud Logger is null." : "Cloud Logger initialized.")}");

            //==== LED to RED
            Hardware.RgbLed?.SetColor(Color.Red);

            //==== Display Controller
            if (Hardware.Display is { } display)
            {
                displayController = new DisplayController(display, isSimulator ? RotationType.Normal : RotationType._270Degrees);
            }

            //==== Meadow.Cloud integratino
            SubscribeToCloudConnectionEvents();

            if (!isSimulator)
            {
                SubscribeToCommands();

                //HandleRelayChanges();
            }

            //InitializeButtons();

            Hardware.RgbLed?.SetColor(Color.Green);
            Resolver.Log.Info("Initialization complete");

        }

        public void SetWiFiStatus(bool connected)
        {
            displayController.UpdateWifi(connected);
        }

        private void SubscribeToCommands()
        {
            Resolver.CommandService?.Subscribe<SampleCommand>(c =>
            {
                Resolver.Log.Info($"Received fan control: {c.IsOn}");
                //displayController.UpdateVents(c.IsOn);
                //if (Hardware.VentFan != null)
                //{
                //    Hardware.VentFan.IsOn = c.IsOn;
                //}
            });
        }

        private void SubscribeToCloudConnectionEvents()
        {
            displayController?.UpdateStatus(Resolver.UpdateService.State.ToString());

            Resolver.UpdateService.OnStateChanged += (sender, state) =>
            {
                displayController?.UpdateStatus(state.ToString());
            };
        }

            public Task Run()
        {
            //_ = StartUpdating(UpdateInterval);

            return Task.CompletedTask;
        }
    }
}