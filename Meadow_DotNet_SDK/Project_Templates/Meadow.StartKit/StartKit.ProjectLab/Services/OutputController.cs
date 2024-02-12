using Meadow;
using Meadow.Devices;
using StartKit.Core;

namespace StartKit.ProjectLab;

internal class OutputController : IOutputController
{
    private IProjectLabHardware Hardware { get; }

    public OutputController(IProjectLabHardware projLab)
    {
        Hardware = projLab;
    }

    public Task SetState(bool state)
    {
        Hardware.RgbLed?.SetColor(state ? Color.Red : Color.Black);
        return Task.CompletedTask;
    }
}
