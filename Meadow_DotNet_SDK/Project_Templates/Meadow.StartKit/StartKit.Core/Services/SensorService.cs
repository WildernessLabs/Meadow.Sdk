using StartKit.Core.Contracts;

namespace StartKit.Core;

public class SensorService
{
    public SensorService(IStartKitPlatform platform)
    {
        if (platform.GetTemperatureSensor() is { } t)
        {
            t.Updated += OnTemperatureUpdated;
        }
    }

    private void OnTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
    {
        // TODO: should we raise an event?  PropertyChanged? Other?
    }
}
