using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Projects.MSBuild;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    public class MeadowProject : DotNetProjectExtension
    {
        protected override void OnEndLoad()
        {
            base.OnEndLoad();
            if (Project.CompileTarget != CompileTarget.Library)
                DeploymentTargetsManager.DeviceListChanged += OnExecutionTargetsChanged;
        }

        protected override void OnPrepareForEvaluation(MSBuildProject project)
        {
            base.OnPrepareForEvaluation(project);
        }

        public override void Dispose()
        {
            base.Dispose();
            DeploymentTargetsManager.DeviceListChanged -= OnExecutionTargetsChanged;
        }

        private void OnExecutionTargetsChanged(object dummy)
        {
            Runtime.RunInMainThread(delegate {
                base.OnExecutionTargetsChanged();
            });
        }

        protected override IEnumerable<ExecutionTarget> OnGetExecutionTargets(ConfigurationSelector configuration)
        {
            return DeploymentTargetsManager.Targets;
        }

        //protected override bool OnGetSupportsFormat(Projects.MSBuild.MSBuildFileFormat format)
        //{
        //    return format.Id == "MSBuild10" || format.Id == "MSBuild12";
        //}

        //protected override bool OnGetSupportsFramework(TargetFramework framework)
        //{
        //    return framework.Id.Identifier == ".NETMicroFramework";
        //}

        //protected override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration, SolutionItemRunConfiguration runConfiguration)
        //{
        //    if (IdeApp.Workspace.GetAllSolutions().Any((s) => s.StartupItem == this.Project)) {
        //        return context.ExecutionTarget is MicroFrameworkExecutionTarget && base.OnGetCanExecute(context, configuration, runConfiguration);
        //    } else {
        //        return base.OnGetCanExecute(context, configuration, runConfiguration);
        //    }
        //}

        //protected override TargetFrameworkMoniker OnGetDefaultTargetFrameworkId()
        //{
        //    return new TargetFrameworkMoniker(".NETMicroFramework", "4.3");
        //}

        //protected override TargetFrameworkMoniker OnGetDefaultTargetFrameworkForFormat(string toolsVersion)
        //{
        //    //Keep default version invalid(1.0) or MonoDevelop will omit from serialization
        //    return new TargetFrameworkMoniker(".NETMicroFramework", "1.0");
        //}

        //protected override ExecutionCommand OnCreateExecutionCommand(ConfigurationSelector configSel, DotNetProjectConfiguration configuration, ProjectRunConfiguration runConfiguration)
        //{
        //    var references = Project.GetReferencedAssemblies(configSel, true).ContinueWith(t => {
        //        return t.Result.Select<AssemblyReference, string>((r) => {
        //            if (r.FilePath.IsAbsolute)
        //                return r.FilePath;
        //            return Project.GetAbsoluteChildPath(r.FilePath).FullPath;
        //        }).ToList();
        //    });
        //    return new MicroFrameworkExecutionCommand() {
        //        OutputDirectory = configuration.OutputDirectory,
        //        ReferencedAssemblies = references
        //    };
        //}
    }
}