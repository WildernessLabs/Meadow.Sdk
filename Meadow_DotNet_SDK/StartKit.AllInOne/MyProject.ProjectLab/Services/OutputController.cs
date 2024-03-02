using Meadow;
using Meadow.Devices;
using MyProject.Core;

namespace MyProject.ProjectLab;

internal class OutputController : IOutputController
{
    /* TODO private IProjectLabHardware _projLab;

    public OutputController(IProjectLabHardware projLab)
    {
        _projLab = projLab;
    }*/

    public Task SetMode(ThermostatMode mode)
    {
        var color = mode switch
        {
            ThermostatMode.Heat => Color.Red,
            ThermostatMode.Cool => Color.Blue,
            _ => Color.Black
        };

        // TODO _projLab.RgbLed?.SetColor(color);

        return Task.CompletedTask;
    }
}
