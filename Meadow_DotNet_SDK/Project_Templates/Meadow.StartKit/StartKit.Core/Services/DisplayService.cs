using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayService
{
    private readonly DisplayScreen? _screen;

    private Temperature _currentTemp;
    private SetPoints _setpoints;

    private DisplayMode _displayMode = DisplayMode.None;
    private ThermostatMode _thermostatMode = ThermostatMode.Off;

    private NormalLayout _normalLayout;
    private EditHeatLayout _editHeatLayout;
    private EditCoolLayout _editCoolLayout;

    public DisplayService(
        IGraphicsDisplay? display,
        Temperature currentTemp,
        SetPoints setPoints)
    {
        _currentTemp = currentTemp;
        _setpoints = setPoints;
        if (display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20()
            };

            _screen = new DisplayScreen(
                display,
                theme: theme);

            GenerateLayouts(_screen);
        }
    }

    private void GenerateLayouts(DisplayScreen screen)
    {
        _normalLayout = new NormalLayout(screen)
        {
            Visible = true,
        };

        _editHeatLayout = new EditHeatLayout(screen)
        {
            Visible = false
        };

        _editCoolLayout = new EditCoolLayout(screen)
        {
            Visible = false
        };

        screen.Controls.Add(
            _normalLayout,
            _editHeatLayout,
            _editCoolLayout);
    }

    public Task UpdateCurrentTemperature(Temperature temperature)
    {
        _currentTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateHeatTo(Temperature temperature)
    {
        _setpoints.HeatTo = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateCoolTo(Temperature temperature)
    {
        _setpoints.CoolTo = temperature;
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

    private void UpdateDisplay()
    {
        if (_screen == null)
        {
            return;
        }

        switch (_displayMode)
        {
            case DisplayMode.None:
                // just running
                _normalLayout.Visible = true;
                _editCoolLayout.Visible = false;
                _editHeatLayout.Visible = false;
                _normalLayout.DisplayTemperature = _currentTemp;
                _normalLayout.Mode = _thermostatMode;
                break;
            case DisplayMode.EditHeatTo:
                // editing heat to setpoint
                _normalLayout.Visible = false;
                _editCoolLayout.Visible = false;
                _editHeatLayout.Visible = true;
                _editHeatLayout.SetPoint = _setpoints.HeatTo!.Value;
                break;
            case DisplayMode.EditCoolTo:
                // editing cool to setpoint
                _normalLayout.Visible = false;
                _editCoolLayout.Visible = true;
                _editHeatLayout.Visible = false;
                _editCoolLayout.SetPoint = _setpoints.CoolTo!.Value;
                break;
        }

        /*
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
        */
    }
}
