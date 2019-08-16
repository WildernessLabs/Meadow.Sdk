using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MeadowCLI.DeviceManagement;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Projects.MSBuild;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    [ExportProjectModelExtension, AppliesTo("Meadow.Sdk")]
    public class MeadowProject : DotNetProjectExtension
    {
        // Note: see https://github.com/mhutch/MonoDevelop.AddinMaker/blob/eff386bfcce05918dbcfe190e9c2ed8513fe92ff/MonoDevelop.AddinMaker/AddinProjectFlavor.cs#L16 for better implementation 

        // Called after the project finishes loading
        protected override void OnEndLoad()
        {
            base.OnEndLoad();

            Console.WriteLine("WLABS: OnEndLoad");

            // if the project is not a library
            // shouldn't this test if it's an executable?
            // TODO: if(AppliesTo("Meadow")) // i think we need to check the project SDK type
            if (Project.CompileTarget != CompileTarget.Library)
            {
                Console.WriteLine("WLABS: Not a lib.");
                // wire up execution targets, which actually starts listening (maybe fix)
                // TODO: call StartListening() here and get rid of the event subscriber hack.
                DeploymentTargetsManager.DeviceListChanged += OnExecutionTargetsChanged;
            }
        }

        protected override void OnPrepareForEvaluation(MSBuildProject project)
        {
            base.OnPrepareForEvaluation(project);
        }

        public override void Dispose()
        {
            base.Dispose();
            // stop listening
            DeploymentTargetsManager.DeviceListChanged -= OnExecutionTargetsChanged;
            // TODO: Call StopListening() here.
        }

        // targets changed event handler.
        private void OnExecutionTargetsChanged(object dummy)
        {
            // update UI on UI thread.
            Runtime.RunInMainThread(() => base.OnExecutionTargetsChanged());
        }

        // probably called when the configuration changes
        protected override IEnumerable<ExecutionTarget> OnGetExecutionTargets(ConfigurationSelector configuration)
        {
            // TODO: the implementation of the targets manager is insane, this is a blocking call. no joke.
            return DeploymentTargetsManager.Targets;
        }

        //protected override bool OnGetSupportsFormat(Projects.MSBuild.MSBuildFileFormat format)
        //{
        //    // Q: For MHutch, how does the new AppliesTo fit into this?
        //    return format.Id == "MSBuild10" || format.Id == "MSBuild12";
        //}

        protected override bool OnGetSupportsFramework(TargetFramework framework)
        {
            // this just checks to make sure it's a "NetFramework" project, not a Silverlight project.
            // leftover from the old model of extensions.
            Console.WriteLine($"WLABS: TargetFramework: { framework.Name }");
            return framework.Id.Identifier == TargetFrameworkMoniker.NET_4_5.Identifier;
        }

        // called by the IDE to determine whether or not the currently selected project and
        // device in the toolbar is good to go for deployment and execution.
        protected override bool OnGetCanExecute(
            ExecutionContext context,
            ConfigurationSelector configuration,
            SolutionItemRunConfiguration runConfiguration)
        {
            // find the selected solution's startup project
            if (IdeApp.Workspace.GetAllSolutions().Any((s) => s.StartupItem == this.Project))
            {
                // if the selection execution target is a meadow device, and the project is an executable.
                return context.ExecutionTarget is MeadowDeviceExecutionTarget && base.OnGetCanExecute(context, configuration, runConfiguration);
            }

            return base.OnGetCanExecute(context, configuration, runConfiguration);
        }

        //protected override TargetFrameworkMoniker OnGetDefaultTargetFrameworkId()
        //{
        //    return new TargetFrameworkMoniker(".NETMicroFramework", "4.3");
        //}

        //protected override TargetFrameworkMoniker OnGetDefaultTargetFrameworkForFormat(string toolsVersion)
        //{
        //    //Keep default version invalid(1.0) or MonoDevelop will omit from serialization
        //    return new TargetFrameworkMoniker(".NETMicroFramework", "1.0");
        //}

        protected override ExecutionCommand OnCreateExecutionCommand(
            ConfigurationSelector configSel,
            DotNetProjectConfiguration configuration,
            ProjectRunConfiguration runConfiguration)
        {
            // build out a list of all the referenced assemblies with _full_ file paths.
            var references = Project.GetReferencedAssemblies(configSel, true).ContinueWith(t => {
                return t.Result.Select<AssemblyReference, string>((r) => {
                    if (r.FilePath.IsAbsolute)
                        return r.FilePath;
                    return Project.GetAbsoluteChildPath(r.FilePath).FullPath;
                }).ToList();
            });

            return new MeadowExecutionCommand()
            {
                OutputDirectory = configuration.OutputDirectory,
                ReferencedAssemblies = references
            };
        }

        
    }
}