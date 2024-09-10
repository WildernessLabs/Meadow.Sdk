using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using ___SafeProjectName___.Core;

namespace ___SafeProjectName___.DT;

internal class OutputController : IOutputController
{
    private IRelay Relay { get; }

    public OutputController()
    {
        Relay = new SimulatedRelay("OUTPUT")
        {
            State = RelayState.Open
        };
    }

    public Task SetState(bool state)
    {
        var requestedState = state ? RelayState.Closed : RelayState.Open;

        if (Relay.State != requestedState)
        {
            Relay.State = requestedState;
            Resolver.Log.Info($"RELAY IS NOW: {Relay.State}");
        }

        return Task.CompletedTask;
    }
}