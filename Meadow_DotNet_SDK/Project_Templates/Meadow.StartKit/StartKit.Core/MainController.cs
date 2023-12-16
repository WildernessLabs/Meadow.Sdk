﻿using StartKit.Core.Contracts;

namespace StartKit.Core;

public class MainController
{
    private IStartKitPlatform _platform;
    private ThermostatMode _currentMode;

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
        _sensorService = new SensorService(platform);

        // retrieve platform-dependent services
        _outputService = platform.GetOutputService();
        _bluetoothService = platform.GetBluetoothService();

        // connect events
        _sensorService.CurrentTemperatureChangedHandler += (s, t) =>
        {
            // update the UI
            _displayService.UpdateCurrentTemperature(t);
        };

        return Task.CompletedTask;
    }

    public async Task Run()
    {
        while (true)
        {
            // get the current temperature

            switch (_currentMode)
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
        if (mode == _currentMode) return;

        // set the output
        _outputService.SetMode(mode);

        // update the display
        _displayService.UpdateMode(mode);

        // publish the transition
        _cloudService.RecordTransition(_currentMode, mode);

        _currentMode = mode;
    }

}