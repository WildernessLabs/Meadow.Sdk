using MonoDevelop.Debugger;
using MonoDevelop.Core.Execution;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    class MeadowExecutionHandler : IExecutionHandler
    {
        public bool CanExecute(ExecutionCommand command)
        {
            return (command is MeadowExecutionCommand);
        }

        public ProcessAsyncOperation Execute(ExecutionCommand command, OperationConsole console)
        {
            System.Console.WriteLine("Executing");

            return DebuggingService.GetExecutionHandler().Execute(command, console);
        }
    }
}