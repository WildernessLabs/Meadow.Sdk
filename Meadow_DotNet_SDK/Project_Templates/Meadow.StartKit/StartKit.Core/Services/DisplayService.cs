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
        // TODO:
        _targetTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateCoolTo(Temperature temperature)
    {
        // TODO:
        _targetTemp = temperature;
        return Task.CompletedTask;
    }

    public Task UpdateDisplayMode(DisplayMode mode)
    {
        // TODO: change what the user is editing (background red/blue?)
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateThermostatMode(ThermostatMode mode)
    {
        // TODO: show/hide the proper icon
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

        if (_currentTemp == _targetTemp)
        {
            //  _graphics.DrawText(0, 90, "Target Reached", Color.White);
        }
        else if (_currentTemp < _targetTemp)
        {
            _graphics.DrawText(0, 90, "Heating", Color.Red);
        }
        else
        {
            _graphics.DrawText(0, 90, "Cooling", Color.Blue);
        }

        _graphics.Show();
    }
}
