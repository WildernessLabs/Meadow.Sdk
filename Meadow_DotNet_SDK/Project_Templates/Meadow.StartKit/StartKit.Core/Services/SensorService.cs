using Meadow.Units;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class SensorService
{
    private Temperature _temperature;

    public event EventHandler<Temperature> CurrentTemperatureChanged = default!;

    public SensorService(IStartKitPlatform platform)
    {
        if (platform.GetTemperatureSensor() is { } t)
        {
            t.Updated += OnTemperatureUpdated;
            t.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }

    public Temperature CurrentTemperature
    {
        get => _temperature;
        private set
        {
            if (value == CurrentTemperature) return;
            _temperature = value;
            CurrentTemperatureChanged?.Invoke(this, CurrentTemperature);
        }
    }

    private void OnTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
    {
        CurrentTemperature = e.New;
    }
}
