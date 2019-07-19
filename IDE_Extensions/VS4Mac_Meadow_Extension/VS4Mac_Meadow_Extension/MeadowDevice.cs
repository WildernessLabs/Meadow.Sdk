using System;
using MonoDevelop.Core.Execution;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    public class MeadowDevice : ExecutionTarget
    {
        public string Model { get; set; }

        public override string Id {
            get { return _id; }
        } protected string _id;

        public override string Name {
            get { return _name; }
        } protected string _name;

        public MeadowDevice(string name, string id)
        {
            _name = name;
            _id = id;
        }
    }
}
