using System.Threading.Tasks;
using Meadow.Peripherals.Leds;
using MyProject.Core;

namespace MyProject.F7Feather
{

    internal class OutputController : IOutputController
    {
        private readonly IRgbLed led;

        public OutputController(IRgbLed led)
        {
            this.led = led;
        }

        public Task SetMode(ThermostatMode mode)
        {
            switch (mode)
            {
                case ThermostatMode.Off:
                    led.IsOn = false;
                    break;
                case ThermostatMode.Heat:
                    led.SetColor(RgbLedColors.Red);
                    break;
                case ThermostatMode.Cool:
                    led.SetColor(RgbLedColors.Blue);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}