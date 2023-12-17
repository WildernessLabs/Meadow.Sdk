using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.Windows;

internal class StartKitPlatform : IStartKitPlatform
{
    private Meadow.Windows _device;
    private Keyboard _keyboard;
    private ITemperatureSensor _temperatureSimulator;
    private IOutputService _outputService;
    private IGraphicsDisplay _graphicsDisplay;

    public StartKitPlatform(Meadow.Windows device)
    {
        _device = device;
        _keyboard = new Keyboard();
        _temperatureSimulator = new TemperatureSimulator(
            new Temperature(70, Temperature.UnitType.Fahrenheit),
            _keyboard.Pins.Plus.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
            _keyboard.Pins.Minus.CreateDigitalInterruptPort(InterruptMode.EdgeRising));
        _outputService = new OutputService();
        _graphicsDisplay = new WinFormsDisplay(320, 240);
    }

    public IBluetoothService? GetBluetoothService()
    {
        return null;
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return _graphicsDisplay;
    }

    public IHumiditySensor? GetHumiditySensor()
    {
        return null;
    }

    public IOutputService GetOutputService()
    {
        return _outputService;
    }

    public ITemperatureSensor? GetTemperatureSensor()
    {
        return _temperatureSimulator;
    }

    public IButton? GetDownButton()
    {
        return new PushButton(
            _keyboard.Pins.Down.CreateDigitalInterruptPort(
                InterruptMode.EdgeRising));
    }

    public IButton? GetLeftButton()
    {
        return new PushButton(
            _keyboard.Pins.Left.CreateDigitalInterruptPort(
                Meadow.Hardware.InterruptMode.EdgeRising));
    }

    public IButton? GetRightButton()
    {
        return new PushButton(
            _keyboard.Pins.Right.CreateDigitalInterruptPort(
                Meadow.Hardware.InterruptMode.EdgeRising));
    }

    public IButton? GetUpButton()
    {
        return new PushButton(
            _keyboard.Pins.Up.CreateDigitalInterruptPort(
                Meadow.Hardware.InterruptMode.EdgeRising));
    }
}
