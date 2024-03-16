using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace $safeprojectname$.Core.Contracts
{
    public interface I$safeprojectname$Hardware
    {
        // basic hardware
        IButton? LeftButton { get; }
        IButton? RightButton { get; }

        // complex hardware
        ITemperatureSensor? TemperatureSensor { get; }
        IPixelDisplay? Display { get; }
        RotationType DisplayRotation { get; }

        // platform-dependent services
        IOutputController OutputController { get; }
        INetworkController NetworkController { get; }
    }
}