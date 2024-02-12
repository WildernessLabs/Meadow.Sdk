using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.ProjectLab;

internal class OutputService : IOutputService
{
    private IProjectLabHardware _projLab;

    public OutputService(IProjectLabHardware projLab)
    {
        _projLab = projLab;
    }

    public Task SetMode(ThermostatMode mode)
    {
        var color = mode switch
        {
            ThermostatMode.Heat => Color.Red,
            ThermostatMode.Cool => Color.Blue,
            _ => Color.Black
        };

        _projLab.RgbLed?.SetColor(color);

        return Task.CompletedTask;
    }
}
