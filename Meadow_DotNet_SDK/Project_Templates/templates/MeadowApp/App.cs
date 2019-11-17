﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;

namespace MeadowApp
{
    public class App : MeadowApp<F7Micro, MeadowApp>
    {
        IDigitalOutputPort redLed;
        IDigitalOutputPort blueLed;
        IDigitalOutputPort greenLed;

        public App()
        {
            ConfigurePorts();
            BlinkLeds();
        }

        public void ConfigurePorts()
        {
            Console.WriteLine("Creating output ports...");
            redLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            blueLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedBlue);
            greenLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);
        }

        public void BlinkLeds()
        {
            var state = false;

            while (true)
            {
                state = !state;

                Console.WriteLine($"State: {state}");

                redLed.State = state;
                Thread.Sleep(500);
                blueLed.State = state;
                Thread.Sleep(500);
                greenLed.State = state;
                Thread.Sleep(500);
            }
        }
    }
}