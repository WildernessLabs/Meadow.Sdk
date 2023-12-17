using Meadow.Cloud;

namespace StartKit.Core;

public class ChangeSetpointsCommand : IMeadowCommand
{
    public double? HeatToC { get; set; }
    public double? HeatToF { get; set; }
    public double? CoolToC { get; set; }
    public double? CoolToF { get; set; }
}
