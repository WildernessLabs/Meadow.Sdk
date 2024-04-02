using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Units;
using ___safeprojectname___.Core.Contracts;

namespace ___safeprojectname___.Core
{
    public class MainController
    {
        private I___safeprojectname___Hardware hardware;

        private CloudController cloudController;
        private ConfigurationController configurationController;
        private DisplayController displayController;
        private InputController inputController;
        private SensorController sensorController;

        private IOutputController OutputController => hardware.OutputController;
        private INetworkController NetworkController => hardware.NetworkController;

        private Temperature.UnitType units;
        private Temperature currentTemperature;
        private Temperature thresholdTemperature;

        public MainController()
        {
        }

        public Task Initialize(I___safeprojectname___Hardware hardware)
        {
            this.hardware = hardware;

            this.thresholdTemperature = 68.Fahrenheit();

            // create generic services
            configurationController = new ConfigurationController();
            cloudController = new CloudController(Resolver.CommandService);
            sensorController = new SensorController(hardware);
            inputController = new InputController(hardware);

            units = configurationController.Units;
            thresholdTemperature = configurationController.ThresholdTemp;

            displayController = new DisplayController(
                this.hardware.Display,
                this.hardware.DisplayRotation,
                units);

            // connect events
            sensorController.CurrentTemperatureChanged += OnCurrentTemperatureChanged;
            cloudController.UnitsChangeRequested += OnUnitsChangeChangeRequested;
            cloudController.ThresholdTemperatureChangeRequested += OnThresholdTemperatureChangeRequested;
            inputController.UnitDownRequested += OnUnitDownRequested;
            inputController.UnitUpRequested += OnUnitUpRequested;
            NetworkController.NetworkStatusChanged += OnNetworkStatusChanged;

            NetworkController.Connect();

            return Task.CompletedTask;
        }

        private void OnNetworkStatusChanged(object sender, EventArgs e)
        {
            Resolver.Log.Info($"Network state changed to {NetworkController.IsConnected}");
            displayController.SetNetworkStatus(NetworkController.IsConnected);
        }

        private void CheckTemperaturesAndSetOutput()
        {
            OutputController?.SetState(currentTemperature < thresholdTemperature);
        }

        private void OnCurrentTemperatureChanged(object sender, Temperature temperature)
        {
            currentTemperature = temperature;

            CheckTemperaturesAndSetOutput();

            // update the UI
            displayController.UpdateCurrentTemperature(currentTemperature);
        }

        private void OnUnitsChangeChangeRequested(object sender, Temperature.UnitType units)
        {
            displayController.UpdateDisplayUnits(units);
        }

        private void OnThresholdTemperatureChangeRequested(object sender, Temperature e)
        {
            thresholdTemperature = e;
            configurationController.ThresholdTemp = e;
            configurationController.Save();
        }

        private void OnUnitDownRequested(object sender, EventArgs e)
        {
            units = units switch
            {
                Temperature.UnitType.Celsius => Temperature.UnitType.Kelvin,
                Temperature.UnitType.Fahrenheit => Temperature.UnitType.Celsius,
                _ => Temperature.UnitType.Fahrenheit,
            };

            displayController.UpdateDisplayUnits(units);
            configurationController.Units = units;
            configurationController.Save();
        }

        private void OnUnitUpRequested(object sender, EventArgs e)
        {
            units = units switch
            {
                Temperature.UnitType.Celsius => Temperature.UnitType.Fahrenheit,
                Temperature.UnitType.Fahrenheit => Temperature.UnitType.Kelvin,
                _ => Temperature.UnitType.Celsius,
            };

            displayController.UpdateDisplayUnits(units);
            configurationController.Units = units;
            configurationController.Save();
        }

        public async Task Run()
        {
            while (true)
            {
                // add any app logic here

                await Task.Delay(5000);
            }
        }
    }
}