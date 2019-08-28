using MonoDevelop.Core.Execution;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    /// <summary>
    /// Represents a Meadow Device execution target; which is the actual
    /// device that gets deployed to when executing.
    /// </summary>
    public class MeadowDeviceExecutionTarget : ExecutionTarget
    {
        public string Model { get; set; }

        public override string Id => _id;
        private string _id;

        public override string Name => _name;
        private string _name;

        public MeadowDeviceExecutionTarget(string name, string id)
        {
            _name = name;
            _id = id;
        }
    }
}