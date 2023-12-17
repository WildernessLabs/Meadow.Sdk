using Meadow;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class MainController
{
    private IStartKitPlatform _platform;
    private ThermostatMode _thermostatMode;

    private CloudService _cloudService;
    private ConfigurationService _configurationService;
    private DisplayService _displayService;
    private InputService _inputService;
    private NetworkService _networkService;
    private SensorService _sensorService;
    private StorageService _storageService;
    private IOutputService _outputService;
    private IBluetoothService? _bluetoothService;

    private Timer _setpointUpdatingTimer;

    public MainController()
    {
        _setpointUpdatingTimer = new Timer(SetpointUpdatingTimerProc);
    }

    public Task Initialize(IStartKitPlatform platform)
    {
        _platform = platform;

        // create generic services
        _configurationService = new ConfigurationService();
        _cloudService = new CloudService(Resolver.CommandService);
        _displayService = new DisplayService(_platform.GetDisplay());
        _sensorService = new SensorService(platform);
        _inputService = new InputService(platform);

        // retrieve platform-dependent services
        _outputService = platform.GetOutputService();
        _bluetoothService = platform.GetBluetoothService();

        // connect events
        _sensorService.CurrentTemperatureChanged += (s, t) =>
        {
            // update the UI
            _displayService.UpdateCurrentTemperature(t);

            Resolver.Log.Info($"Room temperature is now {t.Fahrenheit:0.0}F");
        };

        _inputService.HeatToIncremented += (s, t) =>
        {
            _configurationService.IncrementHeatTo();
            _displayService.UpdateHeatTo(_configurationService.HeatTo);
            // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
            _setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        };

        _inputService.HeatToDecremented += (s, t) =>
        {
            _configurationService.DecrementHeatTo();
            _displayService.UpdateHeatTo(_configurationService.HeatTo);
            // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
            _setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        };

        _inputService.CoolToIncremented += (s, t) =>
        {
            _configurationService.IncrementCoolTo();
            _displayService.UpdateCoolTo(_configurationService.CoolTo);
            // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
            _setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        };

        _inputService.CoolToDecremented += (s, t) =>
        {
            _configurationService.DecrementCoolTo();
            _displayService.UpdateCoolTo(_configurationService.CoolTo);
            // send the change to the cloud after 5 seconds to rpevent sending a message for ever increment
            _setpointUpdatingTimer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        };

        _inputService.DisplayModeChanged += (s, t) =>
        {
            _displayService.UpdateDisplayMode(t);
        };

        _cloudService.NewSetpointsReceived += (s, setpoints) =>
        {
            // you could add sanity by checking the valid range to heat and cool here
            if (setpoints.CoolTo != null)
            {
                _configurationService.CoolTo = setpoints.CoolTo.Value;
            }
            if (setpoints.HeatTo != null)
            {
                _configurationService.HeatTo = setpoints.HeatTo.Value;
            }

            // make sure nothing is null
            setpoints.HeatTo = _configurationService.HeatTo;
            setpoints.CoolTo = _configurationService.CoolTo;

            _ = _cloudService.RecordSetPointChange(setpoints);
        };

        return Task.CompletedTask;
    }

    private void SetpointUpdatingTimerProc(object o)
    {
        var setpoints = new SetPoints
        {
            HeatTo = _configurationService.HeatTo,
            CoolTo = _configurationService.CoolTo
        };

        _ = _cloudService.RecordSetPointChange(setpoints);
    }

    public async Task Run()
    {
        while (true)
        {
            // get the current temperature

            switch (_thermostatMode)
            {
                case ThermostatMode.Off:
                    // are we above the "cool to"?
                    if (_sensorService.CurrentTemperature > _configurationService.CoolTo)
                    {
                        SetSystemMode(ThermostatMode.Cool);
                    }
                    // are we below "heat to"?
                    else if (_sensorService.CurrentTemperature < _configurationService.HeatTo)
                    {
                        SetSystemMode(ThermostatMode.Heat);
                    }

                    break;
                case ThermostatMode.Heat:
                    // are we above "heat to" by > deadband?
                    if (_sensorService.CurrentTemperature > (_configurationService.HeatTo + _configurationService.Deadband))
                    {
                        // turn off
                        SetSystemMode(ThermostatMode.Off);
                    }
                    break;
                case ThermostatMode.Cool:
                    // are we above "cool to" by < deadband?
                    if (_sensorService.CurrentTemperature < (_configurationService.CoolTo + _configurationService.Deadband))
                    {
                        // turn off
                        SetSystemMode(ThermostatMode.Off);
                    }
                    break;
            }

            await Task.Delay(_configurationService.StateCheckPeriod);
        }
    }

    private void SetSystemMode(ThermostatMode mode)
    {
        if (mode == _thermostatMode) return;

        // set the output
        _outputService.SetMode(mode);

        // update the display
        _displayService.UpdateThermostatMode(mode);

        // publish the transition
        _ = _cloudService.RecordTransition(_thermostatMode, mode);

        _thermostatMode = mode;
    }

}