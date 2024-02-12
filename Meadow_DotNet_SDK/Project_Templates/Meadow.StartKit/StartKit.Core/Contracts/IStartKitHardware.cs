using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;

namespace StartKit.Core.Contracts;

public interface IStartKitHardware
{
    // basic hardware
    IButton? UpButton { get; }
    IButton? DownButton { get; }

    // complex hardware
    ITemperatureSensor? TemperatureSensor { get; }
    IPixelDisplay? Display { get; }

    // platform-dependent services
    IOutputController OutputController { get; } // required service
    IBluetoothService? BluetoothService { get; } // optional service
}
