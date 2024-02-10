using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace Meadow.RasPi;

internal class RaspberryPiHardware : IStartKitHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;
    private readonly ITemperatureSensor temperatureSimulator;
    private readonly IOutputService outputService;

    public RaspberryPiHardware(RaspberryPi device, bool supportDisplay)
    {
        this.device = device;

        if (supportDisplay)
        { // only if we have a display attached
            display = new GtkDisplay(ColorMode.Format16bppRgb565);
        }
    }

    public IBluetoothService? BluetoothService => null;
    public IPixelDisplay? Display => display;
    public IOutputService OutputService => outputService;
    public ITemperatureSensor? TemperatureSensor => temperatureSimulator;
    public IButton? DownButton => null;
    public IButton? UpButton => null;
}
