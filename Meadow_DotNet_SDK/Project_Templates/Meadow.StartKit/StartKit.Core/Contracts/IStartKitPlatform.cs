using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;

namespace StartKit.Core.Contracts;

public interface IStartKitPlatform
{
    // hardware
    ITemperatureSensor? GetTemperatureSensor();
    IHumiditySensor? GetHumiditySensor();
    IGraphicsDisplay? GetDisplay();

    // platform-dependent services
    IOutputService GetOutputService(); // required service
    IBluetoothService? GetBluetoothService(); // optional service
}
