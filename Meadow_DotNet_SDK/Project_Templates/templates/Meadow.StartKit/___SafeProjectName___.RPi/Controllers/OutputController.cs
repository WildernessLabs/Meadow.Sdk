using System;
using System.Threading.Tasks;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using ___SafeProjectName___.Core;

namespace ___SafeProjectName___.RPi;

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
        Relay.State = state ? RelayState.Closed : RelayState.Open;
        return Task.CompletedTask;
    }
}