using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using StartKit.Core;

namespace StartKit.Windows;

internal class OutputService : IOutputService
{
    private readonly IRelay heatRelay;
    private readonly IRelay coolRelay;

    public OutputService()
    {
        heatRelay = new SimulatedRelay("HEAT")
        {
            State = RelayState.Open
        };
        coolRelay = new SimulatedRelay("COOL")
        {
            State = RelayState.Open
        };
    }

    public Task SetMode(ThermostatMode mode)
    {
        switch (mode)
        {
            case ThermostatMode.Off:
                heatRelay.IsClosed = false;
                coolRelay.IsClosed = false;
                break;
            case ThermostatMode.Heat:
                heatRelay.IsClosed = true;
                coolRelay.IsClosed = false;
                break;
            case ThermostatMode.Cool:
                heatRelay.IsClosed = false;
                coolRelay.IsClosed = true;
                break;
        }

        Console.WriteLine($"HEAT: {(heatRelay.IsClosed ? "ON" : "OFF")}  COOL: {(coolRelay.IsClosed ? "ON" : "OFF")}");

        return Task.CompletedTask;
    }
}
