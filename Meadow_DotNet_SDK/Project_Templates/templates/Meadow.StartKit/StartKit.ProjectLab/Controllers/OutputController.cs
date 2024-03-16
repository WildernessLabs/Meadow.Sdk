using Meadow;
using Meadow.Peripherals.Leds;
using $safeprojectname$.Core;

namespace $safeprojectname$.ProjectLab
{
    internal class OutputController : IOutputController
    {
        private IRgbPwmLed? Led { get; }

        public OutputController(IRgbPwmLed? led)
        {
            Led = led;
        }

        public Task SetState(bool state)
        {
            if (Led != null)
            {
                if (state)
                {
                    Led.SetColor(Color.Red);
                }
                else
                {
                    Led.IsOn = false;
                }
            }

            return Task.CompletedTask;
        }
    }
}