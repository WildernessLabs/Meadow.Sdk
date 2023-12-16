using Meadow.Units;
using StartKit.Core.Contracts;

namespace StartKit.Core;

public class SensorService
{
    public event EventHandler<Temperature> CurrentTemperatureChangedHandler = default!;

    private Queue<Temperature> _temperatureQueue = new();

    public Temperature CurrentTemperature
    {
        get
        {
            // temperature sensor accuracy is, at best, +/- 0.5
            var mean = _temperatureQueue.Average(t => t.Celsius);

            return new Temperature(
                Math.Round(2 * mean, MidpointRounding.AwayFromZero) * 2,
                Temperature.UnitType.Celsius);
        }
    }

    public SensorService(IStartKitPlatform platform)
    {
        if (platform.GetTemperatureSensor() is { } t)
        {
            t.Updated += OnTemperatureUpdated;
            t.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }

    private void OnTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
    {
        // we'll deal with larger fluctuations in the room by averaging
        _temperatureQueue.Enqueue(e.New);

        while (_temperatureQueue.Count > 5)
        {
            // toss out the oldest data
            _temperatureQueue.Dequeue();
        }

        CurrentTemperatureChangedHandler?.Invoke(this, CurrentTemperature);
    }
}
