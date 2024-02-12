using Meadow.Units;
using System.Text;

namespace $safeprojectname$.Core
{

    public class SetPoints
    {
        public Temperature? HeatTo { get; set; }
        public Temperature? CoolTo { get; set; }

        public override string ToString()
        {
            if (HeatTo == null && CoolTo == null) return "{ null }";
            var s = new StringBuilder("{ ");
            if (HeatTo != null)
            {
                s.Append($"HeatTo: {HeatTo.Value.Fahrenheit}F  ");
            }
            if (CoolTo != null)
            {
                s.Append($"CoolTo: {CoolTo.Value.Fahrenheit}F  ");
            }

            return s.Append("}").ToString();
        }
    }
}