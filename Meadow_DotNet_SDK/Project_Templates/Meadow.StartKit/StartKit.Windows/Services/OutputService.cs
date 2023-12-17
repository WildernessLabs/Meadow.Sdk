using Meadow.Peripherals.Relays;
using StartKit.Core;

namespace StartKit.Windows;

internal class OutputService : IOutputService
{
    private readonly IRelay _heatRelay;
    private readonly IRelay _coolRelay;

    public OutputService()
    {
        _heatRelay = new RelaySimulator("HEAT")
        {
            IsOn = false
        };
        _coolRelay = new RelaySimulator("COOL")
        {
            IsOn = false
        };
    }

    public Task SetMode(ThermostatMode mode)
    {
        switch (mode)
        {
            case ThermostatMode.Off:
                _heatRelay.IsOn = false;
                _coolRelay.IsOn = false;
                break;
            case ThermostatMode.Heat:
                _heatRelay.IsOn = true;
                _coolRelay.IsOn = false;
                break;
            case ThermostatMode.Cool:
                _heatRelay.IsOn = false;
                _coolRelay.IsOn = true;
                break;
        }

        Console.WriteLine($"HEAT: {(_heatRelay.IsOn ? "ON" : "OFF")}  COOL: {(_coolRelay.IsOn ? "ON" : "OFF")}");

        return Task.CompletedTask;
    }
}
