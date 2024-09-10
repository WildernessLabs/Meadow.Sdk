using System;
using System.Threading.Tasks;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace ___SafeProjectName___.Core.Contracts;

public interface I___SafeProjectName___Hardware
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