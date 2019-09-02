using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;

namespace BasicMeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IDigitalOutputPort redLed;
        IDigitalOutputPort blueLed;
        IDigitalOutputPort greenLed;

        public MeadowApp()
        {
            ConfigurePorts();
            BlinkLeds();
        }

        public void ConfigurePorts()
        {
            Console.WriteLine("Creating Outputs...");
            redLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            blueLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedBlue);
            greenLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);
        }

        public void BlinkLeds()
        {
            var state = false;

            while (true)
            {
                state = !state;

                Console.WriteLine($"State: {state}");

                redLED.State = state;
                Thread.Sleep(500);
                blueLED.State = state;
                Thread.Sleep(500);
                greenLED.State = state;
                Thread.Sleep(500);
            }
        }
    }
}
