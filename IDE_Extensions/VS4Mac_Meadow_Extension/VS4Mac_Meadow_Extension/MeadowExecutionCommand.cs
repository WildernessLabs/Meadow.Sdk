using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    // is this used at all??
    public class MeadowExecutionCommand : ProcessExecutionCommand
    {
        public MeadowExecutionCommand()
        {
        }

        // TODO: why the eff is this `Task<>`'d?
        public Task<List<string>> ReferencedAssemblies {
            get;
            set;
        }

        public FilePath OutputDirectory { get; set; }
    }
}
