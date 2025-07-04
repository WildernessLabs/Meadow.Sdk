﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using ___SafeProjectName___.Core;
using ___SafeProjectName___.Core.Contracts;

namespace ___SafeProjectName___.F7;

internal class ___SafeProjectName___F7FeatherHardware : I___SafeProjectName___Hardware
{
    private readonly ITemperatureSensor temperatureSensor;

    public RotationType DisplayRotation => RotationType.Default;
    public ITemperatureSensor? TemperatureSensor => temperatureSensor;
    public IOutputController OutputController { get; }
    public IButton? RightButton => null;
    public IButton? LeftButton => null;
    public IPixelDisplay? Display => null;
    public INetworkController NetworkController { get; }

    public ___SafeProjectName___F7FeatherHardware(F7FeatherBase device)
    {
        temperatureSensor = new SimulatedTemperatureSensor(
            22.Celsius(), 20.Celsius(), 24.Celsius());

        OutputController = new OutputController(
            new RgbLed(
                device.Pins.OnboardLedRed.CreateDigitalOutputPort(),
                device.Pins.OnboardLedGreen.CreateDigitalOutputPort(),
                device.Pins.OnboardLedBlue.CreateDigitalOutputPort()
                )
            );

        NetworkController = new NetworkController(device);
    }
}