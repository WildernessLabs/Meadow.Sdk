using Meadow.Peripherals.Leds;
using StartKit.Core;

namespace StartKit.F7Feather;

internal class OutputService : IOutputService
{
    private readonly IRgbLed _led;

    public OutputService(IRgbLed led)
    {
        _led = led;
    }

    public Task SetMode(ThermostatMode mode)
    {
        switch (mode)
        {
            case ThermostatMode.Off:
                _led.IsOn = false;
                break;
            case ThermostatMode.Heat:
                _led.SetColor(RgbLedColors.Red);
                break;
            case ThermostatMode.Cool:
                _led.SetColor(RgbLedColors.Blue);
                break;
        }

        return Task.CompletedTask;
    }
}
