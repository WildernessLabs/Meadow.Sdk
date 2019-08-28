using System.Collections.Generic;
using System.Threading.Tasks;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    // is this used at all??
    public class MeadowExecutionCommand : ProcessExecutionCommand
    {
        // Adrian: Task because it's been assigned in a non-async method
        // i.e. it's a task to avoid awaiting the assignment (lazy but harmless)
        public Task<List<string>> ReferencedAssemblies { get; set; }

        public FilePath OutputDirectory { get; set; }
    }
}