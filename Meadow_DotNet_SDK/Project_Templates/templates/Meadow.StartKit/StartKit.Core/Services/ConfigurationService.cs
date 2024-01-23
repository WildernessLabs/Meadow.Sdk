using Meadow.Units;

namespace StartKit.Core
{

    public class ConfigurationService
    {
        private static Temperature OneF = new Temperature(1, Temperature.UnitType.Fahrenheit);
        private static Temperature OneC = new Temperature(1, Temperature.UnitType.Celsius);

        public DisplayUnits Units { get; set; }
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

        public void IncrementHeatTo()
        {
            HeatTo += Units switch
            {
                DisplayUnits.Fahrenheit => OneF,
                _ => OneC
            };
        }

        public void DecrementHeatTo()
        {
            HeatTo -= Units switch
            {
                DisplayUnits.Fahrenheit => OneF,
                _ => OneC
            };
        }

        public void IncrementCoolTo()
        {
            CoolTo += Units switch
            {
                DisplayUnits.Fahrenheit => OneF,
                _ => OneC
            };
        }

        public void DecrementCoolTo()
        {
            CoolTo -= Units switch
            {
                DisplayUnits.Fahrenheit => OneF,
                _ => OneC
            };
        }
    }
}