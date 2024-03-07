using Meadow.Units;

namespace StartKit.Core;

public class ConfigurationService
{
    public Temperature.UnitType Units { get; set; }

    public ConfigurationService()
    {
        // load/save state
    }
}
