using Meadow.Units;

namespace StartKit.Core;

public class ConfigurationService
{
    public Temperature Deadband { get; }
    public TimeSpan StateCheckPeriod { get; }

    public Temperature HeatTo { get; set; }
    public Temperature CoolTo { get; set; }

    public ConfigurationService()
    {
        Deadband = new Temperature(1.5, Temperature.UnitType.Celsius);
        StateCheckPeriod = TimeSpan.FromSeconds(1);

        // if we're below this temperature, turn on heat
        HeatTo = new Temperature(68, Temperature.UnitType.Fahrenheit);
        // if we're above this temperature, turn on cool
        CoolTo = new Temperature(75, Temperature.UnitType.Fahrenheit);
    }
}
