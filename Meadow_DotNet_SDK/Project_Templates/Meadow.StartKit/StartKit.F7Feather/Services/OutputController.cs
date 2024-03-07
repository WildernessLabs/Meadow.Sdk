using Meadow.Peripherals.Leds;
using StartKit.Core;

namespace StartKit.F7Feather;

internal class OutputController : IOutputController
{
    private IRgbLed Led { get; }

    public OutputController(IRgbLed led)
    {
        Led = led;
    }

    public Task SetState(bool state)
    {
        if (state)
        {
            Led.SetColor(RgbLedColors.Red);
        }
        else
        {
            Led.IsOn = false;
        }

        return Task.CompletedTask;
    }
}
