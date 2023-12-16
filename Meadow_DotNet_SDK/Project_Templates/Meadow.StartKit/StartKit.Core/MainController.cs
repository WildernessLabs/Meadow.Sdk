using StartKit.Core.Contracts;

namespace StartKit.Core;

public class MainController
{
    private IStartKitPlatform _platform;

    private CloudService _cloudService;
    private ConfigurationService _configurationService;
    private DisplayService _displayService;
    private InputService _inputService;
    private NetworkService _networkService;
    private SensorService _sensorService;
    private StorageService _storageService;
    private IOutputService _outputService;
    private IBluetoothService? _bluetoothService;

    public MainController()
    {
    }

    public Task Initialize(IStartKitPlatform platform)
    {
        _platform = platform;

        // create generic services
        _displayService = new DisplayService(_platform.GetDisplay());

        // retrieve platform-dependent services
        _outputService = platform.GetOutputService();
        _bluetoothService = platform.GetBluetoothService();

        return Task.CompletedTask;
    }

    public Task Run()
    {
        /*
        _sensorService.TemperatureChanged += () =>
        {
            // update the UI
            // change state if necessary
        };
        */

        return Task.CompletedTask;
    }
}