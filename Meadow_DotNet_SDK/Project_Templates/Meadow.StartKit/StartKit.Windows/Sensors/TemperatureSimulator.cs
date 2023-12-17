using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Units;

namespace StartKit.Windows;

internal class TemperatureSimulator : ITemperatureSensor
{
    private Temperature? _temperature;
    private Timer _reportTimer;

    public event EventHandler<IChangeResult<Temperature>> Updated;

    public TimeSpan UpdateInterval { get; private set; }
    public bool IsSampling { get; private set; }

    public TemperatureSimulator(
        Temperature initialTemperature,
        IDigitalInterruptPort incrementPort,
        IDigitalInterruptPort decrementPort)
    {
        _temperature = initialTemperature;

        incrementPort.Changed += (s, e) =>
        {
            Temperature = new Temperature(Temperature!.Value.Fahrenheit + 0.5, Meadow.Units.Temperature.UnitType.Fahrenheit);
        };
        decrementPort.Changed += (s, e) =>
        {
            Temperature = new Temperature(Temperature!.Value.Fahrenheit - 0.5, Meadow.Units.Temperature.UnitType.Fahrenheit);
        };
    }

    private void ReportTimerProc(object? o)
    {
        Updated?.Invoke(this, new ChangeResult<Temperature>(this.Temperature!.Value, this.Temperature!.Value));
    }

    public Temperature? Temperature
    {
        get => _temperature;
        private set
        {
            if (value == Temperature) return;

            if (value != null)
            {
                var previous = _temperature;
                _temperature = value;
                Updated?.Invoke(this, new ChangeResult<Temperature>(this.Temperature!.Value, previous));
            }
        }
    }

    public Task<Temperature> Read()
    {
        return Task.FromResult(this.Temperature ?? Meadow.Units.Temperature.AbsoluteZero);
    }

    public void StartUpdating(TimeSpan? updateInterval = null)
    {
        UpdateInterval = updateInterval ?? TimeSpan.FromSeconds(1);
        IsSampling = true;
        _reportTimer = new Timer(ReportTimerProc, null, updateInterval!.Value, updateInterval.Value);
    }

    public void StopUpdating()
    {
        IsSampling = false;
        _reportTimer.Dispose();
    }
}
