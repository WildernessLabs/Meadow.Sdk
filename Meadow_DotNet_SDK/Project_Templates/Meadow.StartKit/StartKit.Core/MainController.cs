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

    private IOutputController? OutputController => hardware.OutputController;
    private IBluetoothService? BluetoothService => hardware.BluetoothService;

    public MainController()
    {
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
            configurationService.Units);

        // connect events
        sensorService.CurrentTemperatureChanged += (s, t) =>
        {
            // update the UI
            displayController.UpdateCurrentTemperature(t);
        };

        return Task.CompletedTask;
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