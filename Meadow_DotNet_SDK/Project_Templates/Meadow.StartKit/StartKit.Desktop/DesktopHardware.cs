using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.Windows;

internal class DesktopHardware : IStartKitHardware
{
    private readonly Desktop device;
    private readonly Keyboard keyboard;
    private readonly ITemperatureSensor temperatureSimulator;
    private readonly IOutputController outputController;
    private readonly PushButton downButton;
    private readonly PushButton upButton;
    private readonly IPixelDisplay display;

    public DesktopHardware(Desktop device)
    {
        this.device = device;
        keyboard = new Keyboard();
        temperatureSimulator = new SimulatedTemperatureSensor(
            new Temperature(70, Temperature.UnitType.Fahrenheit),
            keyboard.Pins.Plus.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
            keyboard.Pins.Minus.CreateDigitalInterruptPort(InterruptMode.EdgeRising));
        outputController = new OutputController();

        downButton = new PushButton(
            keyboard.Pins.Down.CreateDigitalInterruptPort(
                InterruptMode.EdgeBoth));
        upButton = new PushButton(
            keyboard.Pins.Up.CreateDigitalInterruptPort(
                InterruptMode.EdgeBoth));

    }

    public IBluetoothService? BluetoothService => null;
    public IPixelDisplay? Display => device.Display;
    public IOutputController OutputController => outputController;
    public ITemperatureSensor? TemperatureSensor => temperatureSimulator;
    public IButton? DownButton => downButton;
    public IButton? UpButton => upButton;
}
