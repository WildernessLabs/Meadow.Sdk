using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using MyProject.Core;
using MyProject.Core.Contracts;

namespace MyProject.Desktop
{
    internal class MyProjectPlatform : IStartKitHardware
    {
        private readonly Meadow.Desktop device;
        private readonly Keyboard keyboard;
        private readonly ITemperatureSensor temperatureSimulator;
        private readonly IOutputController outputController;
        private readonly IPixelDisplay display;

        public MyProjectPlatform(Meadow.Desktop device)
        {
            this.device = device;
            keyboard = new Keyboard();
            temperatureSimulator = new SimulatedTemperatureSensor(
                new Temperature(70, Temperature.UnitType.Fahrenheit),
                keyboard.Pins.Plus.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                keyboard.Pins.Minus.CreateDigitalInterruptPort(InterruptMode.EdgeRising));
            outputController = new OutputController();
            display = new GtkDisplay(320, 240);
        }

        public IBluetoothService? BluetoothService => null;

        public IPixelDisplay? Display => display;

        public IOutputController OutputController => outputController;

        public ITemperatureSensor? TemperatureSensor => temperatureSimulator;

        public IButton? DownButton => new PushButton(
                keyboard.Pins.Down.CreateDigitalInterruptPort(
                    InterruptMode.EdgeBoth));

        public IButton? LeftButton => new PushButton(
                keyboard.Pins.Left.CreateDigitalInterruptPort(
                    InterruptMode.EdgeBoth));

        public IButton? RightButton => new PushButton(
                keyboard.Pins.Right.CreateDigitalInterruptPort(
                    InterruptMode.EdgeBoth));

        public IButton? UpButton => new PushButton(
                keyboard.Pins.Up.CreateDigitalInterruptPort(
                    InterruptMode.EdgeBoth));
    }
}