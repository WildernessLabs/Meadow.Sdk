using Meadow;
using Meadow.Units;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class MainController
{
    private IStartKitHardware hardware;

    private CloudController cloudController;
    private ConfigurationController configurationController;
    private DisplayController displayController;
    private InputController inputController;
    private readonly NetworkController networkController;
    private SensorController sensorController;

    private IOutputController? OutputController => hardware.OutputController;
    private IBluetoothService? BluetoothService => hardware.BluetoothService;

    private Temperature.UnitType units;

    public MainController()
    {
    }

    public Task Initialize(IStartKitHardware hardware)
    {
        this.hardware = hardware;

        // create generic services
        configurationController = new ConfigurationController();
        cloudController = new CloudController(Resolver.CommandService);
        sensorController = new SensorController(hardware);
        inputController = new InputController(hardware);

        units = configurationController.Units;

        displayController = new DisplayController(
            this.hardware.Display,
            units);

        // connect events
        sensorController.CurrentTemperatureChanged += (s, t) =>
        {
            // update the UI
            displayController.UpdateCurrentTemperature(t);
        };
        cloudController.UnitsChangeRequested += (s, u) =>
        {
            displayController.UpdateDisplayUnits(u);
        };

        inputController.UnitDownRequested += OnUnitDownRequested;
        inputController.UnitUpRequested += OnUnitUpRequested;

        return Task.CompletedTask;
    }

    private void CloudController_UnitsChangeRequested(object sender, Temperature.UnitType e)
    {
        throw new NotImplementedException();
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
    }

    public async Task Run()
    {
        /*
        while (true)
        {
            // get the current temperature


            await Task.Delay(configurationService.StateCheckPeriod);
        }
        */
    }
}