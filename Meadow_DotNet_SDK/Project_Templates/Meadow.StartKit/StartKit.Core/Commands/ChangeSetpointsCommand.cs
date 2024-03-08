using Meadow.Cloud;

namespace StartKit.Core;

/*
JSON for copy/paste into Meadow.Cloud
ChangeDisplayUnits
{
    "units": "Fahrenheit"
}
*/
public class ChangeDisplayUnitsCommand : IMeadowCommand
{
    public string Units { get; set; } = default!;
}
