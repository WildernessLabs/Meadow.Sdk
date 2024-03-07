using Meadow.Cloud;

namespace StartKit.Core;

public class CloudService
{
    private ICommandService commandService;

    public CloudService(ICommandService commandService)
    {
        this.commandService = commandService;
        this.commandService.Subscribe<ChangeDisplayUnitsCommand>(OnChangeDisplayUnitsCommandReceived);
    }

    private void OnChangeDisplayUnitsCommandReceived(ChangeDisplayUnitsCommand command)
    {
    }
}
