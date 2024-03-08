using Meadow.Units;

namespace StartKit.Core;

public class ConfigurationController
{
    public Temperature.UnitType Units { get; set; }

    public ConfigurationController()
    {
        // load/save state
    }
}
