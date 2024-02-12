using Meadow;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class MainController
{
    private IStartKitHardware hardware;

    private CloudService cloudService;
    private ConfigurationService configurationService;
    private DisplayController displayController;
    private InputService inputService;
    private readonly NetworkService networkService;
    private SensorService sensorService;
    private readonly StorageService storageService;

    private readonly Timer _setpointUpdatingTimer;

    private IOutputController? OutputController => hardware.OutputController;
    private IBluetoothService? BluetoothService => hardware.BluetoothService;

    public MainController()
    {
        _setpointUpdatingTimer = new Timer(SetpointUpdatingTimerProc);
    }

    public Task Initialize(IStartKitHardware platform)
    {
        hardware = platform;

        // create generic services
        configurationService = new ConfigurationService();
        cloudService = new CloudService(Resolver.CommandService);
        sensorService = new SensorService(platform);
        inputService = new InputService(platform);

        displayController = new DisplayController(
            hardware.Display,
            sensorService.CurrentTemperature,
            new SetPoints
            {
                CoolTo = configurationService.CoolTo,
                HeatTo = configurationService.HeatTo
            }
            );

        // connect events
        sensorService.CurrentTemperatureChanged += (s, t) =>
        {
            // update the UI
            displayController.UpdateCurrentTemperature(t);

            Resolver.Log.Info($"Room temperature is now {t.Fahrenheit:0.0}F");
        };

        return Task.CompletedTask;
    }

    private void SetpointUpdatingTimerProc(object o)
    {
        _ = cloudService.RecordSetPointChange(setpoints);
    }

    public async Task Run()
    {
        while (true)
        {
            // get the current temperature


            await Task.Delay(configurationService.StateCheckPeriod);
        }
    }
}