using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using StartKit.Core;

namespace StartKit.Windows;

internal class OutputService : IOutputService
{
    private readonly IRelay _heatRelay;
    private readonly IRelay _coolRelay;

    public OutputService()
    {
        _heatRelay = new SimulatedRelay("HEAT")
        {
            State = RelayState.Open
        };
        _coolRelay = new SimulatedRelay("COOL")
        {
            State = RelayState.Open
        };
    }

    public Task SetMode(ThermostatMode mode)
    {
        switch (mode)
        {
            case ThermostatMode.Off:
                _heatRelay.IsClosed = false;
                _coolRelay.IsClosed = false;
                break;
            case ThermostatMode.Heat:
                _heatRelay.IsClosed = true;
                _coolRelay.IsClosed = false;
                break;
            case ThermostatMode.Cool:
                _heatRelay.IsClosed = false;
                _coolRelay.IsClosed = true;
                break;
        }

        Console.WriteLine($"HEAT: {(_heatRelay.IsClosed ? "ON" : "OFF")}  COOL: {(_coolRelay.IsClosed ? "ON" : "OFF")}");

        return Task.CompletedTask;
    }
}
