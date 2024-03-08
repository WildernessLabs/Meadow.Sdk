using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.F7Feather;

internal class F7FeatherHardware : IStartKitHardware
{
    private readonly F7FeatherBase device;
    private readonly ITemperatureSensor temperatureSensor;
    private readonly IOutputController outputService;

    public F7FeatherHardware(F7FeatherBase device)
    {
        this.device = device;
        temperatureSensor = new SimulatedTemperatureSensor(
            22.Celsius(), 20.Celsius(), 24.Celsius());

        outputService = new OutputController(
            new RgbLed(
                this.device.Pins.OnboardLedRed.CreateDigitalOutputPort(),
                this.device.Pins.OnboardLedGreen.CreateDigitalOutputPort(),
                this.device.Pins.OnboardLedBlue.CreateDigitalOutputPort()
                )
            );
    }

    public IBluetoothService? BluetoothService => null;
    public IOutputController OutputController => outputService;
    public IPixelDisplay? Display => null;
    public ITemperatureSensor? TemperatureSensor => temperatureSensor;
    public IButton? RightButton => null;
    public IButton? LeftButton => null;
}
