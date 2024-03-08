using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.F7;

internal class ProjectLabHardware : IStartKitHardware
{
    private readonly IProjectLabHardware projLab;

    public RotationType DisplayRotation => RotationType._270Degrees;
    public IOutputController OutputController { get; }
    public IButton? LeftButton => projLab.LeftButton;
    public IButton? RightButton => projLab.RightButton;
    public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;
    public IPixelDisplay? Display => projLab.Display;
    public INetworkController NetworkController { get; }

    public ProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();

        OutputController = new OutputController(projLab.RgbLed);
        NetworkController = new NetworkController(device);
    }
}
