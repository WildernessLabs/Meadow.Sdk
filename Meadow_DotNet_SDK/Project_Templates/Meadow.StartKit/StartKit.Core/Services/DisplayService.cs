using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayService
{
    private readonly IGraphicsDisplay? _display;
    private readonly MicroGraphics? _graphics;

    private Temperature _currentTemp = new Temperature(25);
    private Temperature _targetTemp = new Temperature(25);

    private DisplayMode _displayMode = DisplayMode.None;
    private ThermostatMode _thermostatMode = ThermostatMode.Off;

    public DisplayService(IGraphicsDisplay? display)
    {
        _display = display;

        if (_display != null)
        {
            _graphics = new MicroGraphics(_display)
            {
                CurrentFont = new Font12x20()
            };
        }
    }

    public Task UpdateCurrentTemperature(Temperature temperature)
    {
        _currentTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateHeatTo(Temperature temperature)
    {
        _targetTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateCoolTo(Temperature temperature)
    {
        _targetTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateDisplayMode(DisplayMode mode)
    {
        _displayMode = mode;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateThermostatMode(ThermostatMode mode)
    {
        _thermostatMode = mode;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    void UpdateDisplay()
    {
        if (_graphics == null)
        {
            return;
        }

        _graphics.Clear();

        _graphics.DrawText(0, 0, "Hello Start Kit", Color.White);

        _graphics.DrawText(0, 30, $"Current: {_currentTemp.Celsius:N1}°C", Color.White);

        _graphics.DrawText(0, 60, $"Target: {_targetTemp.Celsius:N1}°C", Color.White);

        _graphics.DrawText(0, 90, $"Thermostat Mode: {_thermostatMode}", Color.White);

        _graphics.DrawText(0, 120, $"Display Mode: {_displayMode}", Color.White);

        if (_thermostatMode == ThermostatMode.Off)
        {
            _graphics.DrawText(0, 150, "Off", Color.LightGray);
        }
        else if (_currentTemp == _targetTemp)
        {
            _graphics.DrawText(0, 150, "At Target", Color.Green);
        }
        else if (_currentTemp < _targetTemp && _thermostatMode == ThermostatMode.Heat)
        {
            _graphics.DrawText(0, 150, "Heating", Color.Red);
        }
        else if (_currentTemp > _targetTemp && _thermostatMode == ThermostatMode.Cool)
        {
            _graphics.DrawText(0, 150, "Cooling", Color.Blue);
        }

        _graphics.Show();
    }
}
