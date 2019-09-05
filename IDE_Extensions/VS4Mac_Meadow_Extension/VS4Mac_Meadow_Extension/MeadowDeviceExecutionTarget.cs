using MeadowCLI.DeviceManagement;
using MonoDevelop.Core.Execution;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    /// <summary>
    /// Represents a Meadow Device execution target; which is the actual
    /// device that gets deployed to when executing.
    /// </summary>
    public class MeadowDeviceExecutionTarget : ExecutionTarget
    {
        public override string Id => MeadowDevice?.Id;

        public override string Name => MeadowDevice?.Name;

        public MeadowDevice MeadowDevice { get; private set; }

        public MeadowDeviceExecutionTarget(MeadowDevice device)
        {
            MeadowDevice = device;
        }
    }
}