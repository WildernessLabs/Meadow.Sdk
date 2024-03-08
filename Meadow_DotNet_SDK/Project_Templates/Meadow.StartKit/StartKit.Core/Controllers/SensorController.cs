using Meadow.Units;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class SensorController
{
    private Temperature temperature;

    public event EventHandler<Temperature> CurrentTemperatureChanged = default!;

    public SensorController(IStartKitHardware platform)
    {
        if (platform.TemperatureSensor is { } t)
        {
            t.Updated += OnTemperatureUpdated;
            t.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }

    public Temperature CurrentTemperature
    {
        get => temperature;
        private set
        {
            if (value == CurrentTemperature) return;
            temperature = value;
            CurrentTemperatureChanged?.Invoke(this, CurrentTemperature);
        }
    }

    private void OnTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
    {
        CurrentTemperature = e.New;
    }
}
