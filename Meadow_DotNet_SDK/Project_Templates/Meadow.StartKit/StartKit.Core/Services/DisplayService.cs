using Meadow.Foundation.Graphics;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayService
{
    private IGraphicsDisplay? _display;

    public DisplayService(IGraphicsDisplay? display)
    {
        _display = display;
    }

    public Task UpdateCurrentTemperature(Temperature temperature)
    {
        // TODO:

        return Task.CompletedTask;
    }

    public Task UpdateHeatTo(Temperature temperature)
    {
        // TODO:

        return Task.CompletedTask;
    }

    public Task UpdateCoolTo(Temperature temperature)
    {
        // TODO:

        return Task.CompletedTask;
    }

    public Task UpdateDisplayMode(DisplayMode mode)
    {
        // TODO: change what the user is editing (background red/blue?)

        return Task.CompletedTask;
    }

    public Task UpdateThermostatMode(ThermostatMode mode)
    {
        // TODO: show/hide the proper icon

        return Task.CompletedTask;
    }
}
