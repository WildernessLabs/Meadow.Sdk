using Meadow.Cloud;

namespace ___safeprojectname___.Core;

/*
JSON for copy/paste into Meadow.Cloud:
ChangeDisplayUnits
{
    "units": "Fahrenheit"
}

PS command:
> meadow cloud command publish ChangeDisplayUnits -d MY_DEVICE_ID -a '{\"units\": \"Fahrenheit\"}'
*/

public class ChangeDisplayUnitsCommand : IMeadowCommand
{
    public string Units { get; set; } = default!;
}