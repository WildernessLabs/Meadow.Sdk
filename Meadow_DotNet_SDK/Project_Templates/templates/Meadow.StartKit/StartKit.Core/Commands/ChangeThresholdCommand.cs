using Meadow.Cloud;

namespace $safeprojectname$.Core
{
    /*
    JSON for copy/paste into Meadow.Cloud
    ChangeThreshold
    {
        "tempC": 26.2
    }

    ChangeThreshold
    {
        "tempF": 68.5
    }
    */

    public class ChangeThresholdCommand : IMeadowCommand
    {
        public double? TempC { get; set; } = default!;
        public double? TempF { get; set; } = default!;
    }
}