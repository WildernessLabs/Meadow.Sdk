
using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using ___safeprojectname___.Core;
using ___safeprojectname___.Core.Contracts;

namespace ___safeprojectname___.DT;

internal class ___safeprojectname___Hardware : I___safeprojectname___Hardware
{
    private readonly Desktop device;
    private readonly Keyboard keyboard;

    public RotationType DisplayRotation => RotationType.Default;
    public IOutputController OutputController { get; }
    public INetworkController? NetworkController { get; }
    public IPixelDisplay? Display => device.Display;
    public ITemperatureSensor? TemperatureSensor { get; }
    public IButton? RightButton { get; }
    public IButton? LeftButton { get; }

    public ___safeprojectname___Hardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();
        NetworkController = new NetworkController(keyboard);

        TemperatureSensor = new SimulatedTemperatureSensor(
            new Temperature(70, Temperature.UnitType.Fahrenheit),
            keyboard.Pins.Up.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
            keyboard.Pins.Down.CreateDigitalInterruptPort(InterruptMode.EdgeRising));

        LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
        RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        OutputController = new OutputController();
    }
}