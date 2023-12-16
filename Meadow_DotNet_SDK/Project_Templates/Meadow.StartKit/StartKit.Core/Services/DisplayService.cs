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

    public Task UpdateMode(ThermostatMode mode)
    {
        // TODO: show/hide the proper icon

        return Task.CompletedTask;
    }
}
