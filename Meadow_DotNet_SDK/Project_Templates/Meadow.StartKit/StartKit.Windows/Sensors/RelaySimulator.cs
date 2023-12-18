using Meadow.Peripherals.Relays;

namespace StartKit.Windows;

internal class RelaySimulator : IRelay
{
    private bool _state;

    public event EventHandler<bool> OnRelayChanged = default!;

    public RelayType Type => RelayType.NormallyOpen;
    public string Name { get; }

    public RelaySimulator(string name)
    {
        Name = name;
    }

    public bool IsOn
    {
        get => _state;
        set
        {
            if (value == IsOn) return;
            _state = value;
            OnRelayChanged?.Invoke(this, IsOn);
        }
    }

    public void Toggle()
    {
        IsOn = !IsOn;
    }
}
