using System.Threading;
using System.Threading.Tasks;
using Meadow;
using MyProject.Core.Contracts;

namespace MyProject.Core
{
    public class MainController
    {
        private IStartKitHardware platform;
        private ThermostatMode thermostatMode;

        private CloudService cloudService;
        private ConfigurationService configurationService;
        private DisplayService displayService;
        private InputService inputService;
        private readonly NetworkService networkService;
        private SensorService sensorService;
        private readonly StorageService storageService;
        private IOutputController outputController;
        private IBluetoothService? bluetoothService;

        private readonly Timer setpointUpdatingTimer;

        public MainController()
        {
            setpointUpdatingTimer = new Timer(SetpointUpdatingTimerProc);
        }

        public Task Initialize(IStartKitHardware platform)
        {
            this.platform = platform;

            // create generic services
            configurationService = new ConfigurationService();
            cloudService = new CloudService(Resolver.CommandService);
            sensorService = new SensorService(platform);
            inputService = new InputService(platform);

            displayService = new DisplayService(
                this.platform.Display,
                sensorService.CurrentTemperature,
                new SetPoints
                {
                    CoolTo = configurationService.CoolTo,
                    HeatTo = configurationService.HeatTo
                }
                );

            // retrieve platform-dependent services
            outputController = platform.OutputController;
            bluetoothService = platform.BluetoothService;

            // connect events
            sensorService.CurrentTemperatureChanged += (s, t) =>
            {
                // update the UI
                displayService.UpdateCurrentTemperature(t);

                Resolver.Log.Info($"Room temperature is now {t.Fahrenheit:0.0}F");
            };

            inputService.HeatToIncremented += (s, t) =>
            {
                configurationService.IncrementHeatTo();
                displayService.UpdateHeatTo(configurationService.HeatTo);
                // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
                setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            };

            inputService.HeatToDecremented += (s, t) =>
            {
                configurationService.DecrementHeatTo();
                displayService.UpdateHeatTo(configurationService.HeatTo);
                // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
                setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            };

            inputService.CoolToIncremented += (s, t) =>
            {
                configurationService.IncrementCoolTo();
                displayService.UpdateCoolTo(configurationService.CoolTo);
                // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
                setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            };

            inputService.CoolToDecremented += (s, t) =>
            {
                configurationService.DecrementCoolTo();
                displayService.UpdateCoolTo(configurationService.CoolTo);
                // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
                setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            };

            inputService.DisplayModeChanged += (s, t) =>
            {
                displayService.UpdateDisplayMode(t);
            };

            inputService.ThermostatModeChanged += (s, t) =>
            {
                displayService.UpdateThermostatMode(t);
            };

            cloudService.NewSetpointsReceived += (s, setpoints) =>
            {
                // you could add sanity by checking the valid range to heat and cool here
                if (setpoints.CoolTo != null)
                {
                    configurationService.CoolTo = setpoints.CoolTo.Value;
                }
                if (setpoints.HeatTo != null)
                {
                    configurationService.HeatTo = setpoints.HeatTo.Value;
                }

                // make sure nothing is null
                setpoints.HeatTo = configurationService.HeatTo;
                setpoints.CoolTo = configurationService.CoolTo;

                _ = cloudService.RecordSetPointChange(setpoints);
            };

            return Task.CompletedTask;
        }

        private void SetpointUpdatingTimerProc(object o)
        {
            var setpoints = new SetPoints
            {
                HeatTo = configurationService.HeatTo,
                CoolTo = configurationService.CoolTo
            };

            _ = cloudService.RecordSetPointChange(setpoints);
        }

        public async Task Run()
        {
            while (true)
            {
                // get the current temperature

                switch (thermostatMode)
                {
                    case ThermostatMode.Off:
                        // are we above the "cool to"?
                        if (sensorService.CurrentTemperature > configurationService.CoolTo)
                        {
                            SetSystemMode(ThermostatMode.Cool);
                        }
                        // are we below "heat to"?
                        else if (sensorService.CurrentTemperature < configurationService.HeatTo)
                        {
                            SetSystemMode(ThermostatMode.Heat);
                        }

                        break;
                    case ThermostatMode.Heat:
                        // are we above "heat to" by > deadband?
                        if (sensorService.CurrentTemperature > (configurationService.HeatTo + configurationService.Deadband))
                        {
                            // turn off
                            SetSystemMode(ThermostatMode.Off);
                        }
                        break;
                    case ThermostatMode.Cool:
                        // are we above "cool to" by < deadband?
                        if (sensorService.CurrentTemperature < (configurationService.CoolTo - configurationService.Deadband))
                        {
                            // turn off
                            SetSystemMode(ThermostatMode.Off);
                        }
                        break;
                }

                await Task.Delay(configurationService.StateCheckPeriod);
            }
        }

        private void SetSystemMode(ThermostatMode mode)
        {
            if (mode == thermostatMode) return;

            // set the output
            outputController.SetMode(mode);

            // update the display
            displayService.UpdateThermostatMode(mode);

            // publish the transition
            _ = cloudService.RecordTransition(thermostatMode, mode);

            thermostatMode = mode;
        }

    }
}