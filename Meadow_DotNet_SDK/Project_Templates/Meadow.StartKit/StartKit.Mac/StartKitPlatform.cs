using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Foundation.Simulation;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.Mac;

internal class StartKitPlatform : IStartKitPlatform
{
    private readonly Keyboard _keyboard;
    private readonly ITemperatureSensor _temperatureSimulator;
    private readonly IOutputService _outputService;

    public StartKitPlatform(Meadow.Mac device)
    {
        _keyboard = new Keyboard();
        _outputService = new OutputService();
        _temperatureSimulator = new SimulatedTemperatureSensor(
            new Temperature(70, Temperature.UnitType.Fahrenheit),
            _keyboard.Pins.Period.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
            _keyboard.Pins.Comma.CreateDigitalInterruptPort(InterruptMode.EdgeRising));
    }

    public IBluetoothService? GetBluetoothService()
    {
        return null;
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return null;
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
