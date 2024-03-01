using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace $safeprojectname$.Core.Contracts
{

    public interface I$safeprojectname$Platform
    {
        // basic hardware
        IButton? GetUpButton();
        IButton? GetDownButton();
        IButton? GetLeftButton();
        IButton? GetRightButton();

        // complex hardware
        ITemperatureSensor? GetTemperatureSensor();
        IHumiditySensor? GetHumiditySensor();
        IPixelDisplay? GetDisplay();

        // platform-dependent services
        IOutputService GetOutputService(); // required service
        IBluetoothService? GetBluetoothService(); // optional service
    }

}