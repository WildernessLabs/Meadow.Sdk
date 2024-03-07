using Meadow.Cloud;

namespace StartKit.Core;

public class ChangeDisplayUnitsCommand : IMeadowCommand
{
    public string Units { get; set; } = default!;
}