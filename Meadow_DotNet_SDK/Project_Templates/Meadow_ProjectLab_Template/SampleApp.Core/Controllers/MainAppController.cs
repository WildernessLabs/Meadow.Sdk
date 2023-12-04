using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Logging;
using SampleApp.Commands;
using SampleApp.Hardware;
using SampleApp.Models;

namespace SampleApp.Controllers
{
    public class MainAppController
	{
        protected ISampleAppHardware Hardware { get; set; }
        protected DisplayController displayController;
        protected CloudLogger cloudLogger;
        protected bool IsRunning = false;
        protected SampleModel SampleModel { get; set; }
        protected TimeSpan UpdateInterval = TimeSpan.FromMinutes(2);

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

        public Task Run()
        {
            _ = StartUpdating(UpdateInterval);

            return Task.CompletedTask;
        }

        private void SubscribeToCloudConnectionEvents()
        {
            displayController?.UpdateStatus(Resolver.UpdateService.State.ToString());

            Resolver.UpdateService.OnStateChanged += (sender, state) =>
            {
                displayController?.UpdateStatus(state.ToString());
            };
        }

        private async Task StartUpdating(TimeSpan updateInterval)
        {
            Console.WriteLine("ClimateMonitorAgent.StartUpdating()");

            if (IsRunning)
            {
                return;
            }
            IsRunning = true;

            while (IsRunning)
            {
                // do a one-off read of all the sensors
                SampleModel = await ReadSensors();

                Resolver.Log.Info($"Temperature: {SampleModel.Temperature.Celsius:N2}°C | Humidity: {SampleModel.Humidity.Percent:N2}%");

                displayController.UpdateModel(SampleModel);

                try
                {
                    displayController.UpdateSync(true);
                    var cl = Resolver.Services.Get<CloudLogger>();
                    cl?.LogEvent(110, "Atmospheric reading", new Dictionary<string, object>()
                    {
                        { "TemperatureCelsius", SampleModel.Temperature.Celsius },
                        { "HumidityPercent", SampleModel.Humidity.Percent },
                        { "PressureAtmospheres", SampleModel.Pressure.StandardAtmosphere },
                    });
                    displayController.UpdateSync(false);
                }
                catch (Exception ex)
                {
                    Resolver.Log.Info($"Err: {ex.Message}");
                }

                await Task.Delay(updateInterval);
            }
        }

        private void StopUpdating()
        {
            if (!IsRunning)
            {
                return;
            }

            IsRunning = false;
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

        /// <summary>
        /// Performs a one-off read of all the sensors.
        /// </summary>
        /// <returns></returns>
        private async Task<SampleModel> ReadSensors()
        {
            Resolver.Log.Info($"Reading sensors.");
            var temperatureTask = Hardware.TemperatureSensor?.Read();
            var humidityTask = Hardware.HumiditySensor?.Read();
            var pressureTask = Hardware.PressureSensor?.Read();

            // run the tasks in serial with timeouts
            TimeSpan timeoutDuration = TimeSpan.FromSeconds(5);
            await Task.WhenAny(temperatureTask, Task.Delay(timeoutDuration));
            await Task.WhenAny(humidityTask, Task.Delay(timeoutDuration));
            await Task.WhenAny(humidityTask, Task.Delay(timeoutDuration));

            Resolver.Log.Info($"Sensor reads completed.");

            var climate = new SampleModel()
            {
                Temperature = temperatureTask.IsCompletedSuccessfully ? temperatureTask?.Result : null,
                Humidity = humidityTask.IsCompletedSuccessfully ? humidityTask?.Result : null,
                Pressure = pressureTask.IsCompletedSuccessfully ? pressureTask?.Result : null
            };

            return climate;
        }
    }
}