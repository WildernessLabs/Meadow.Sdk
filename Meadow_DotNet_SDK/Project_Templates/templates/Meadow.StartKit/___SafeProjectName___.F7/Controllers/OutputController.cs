using System;
using System.Threading.Tasks;
using Meadow.Peripherals.Leds;
using ___SafeProjectName___.Core;

namespace ___SafeProjectName___.F7;

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