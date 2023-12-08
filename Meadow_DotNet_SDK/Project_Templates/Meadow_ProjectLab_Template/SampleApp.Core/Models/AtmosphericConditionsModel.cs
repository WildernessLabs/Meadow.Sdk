using Meadow.Units;

namespace SampleApp.Models
{
    public class AtmosphericConditionsModel
    {
        public Temperature? Temperature { get; set; }
        public Pressure? Pressure { get; set; }
        public RelativeHumidity? Humidity { get; set; }
    }
}