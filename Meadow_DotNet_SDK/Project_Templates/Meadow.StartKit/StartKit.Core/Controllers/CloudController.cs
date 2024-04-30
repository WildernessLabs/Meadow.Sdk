using Meadow;
using Meadow.Cloud;
using Meadow.Units;

namespace StartKit.Core;

public class CloudController
{
    private ICommandService commandService;

    public event EventHandler<Temperature.UnitType> UnitsChangeRequested;
    public event EventHandler<Temperature> ThresholdTemperatureChangeRequested;

    public CloudController(ICommandService commandService)
    {
        this.commandService = commandService;

        this.commandService.Subscribe<ChangeDisplayUnitsCommand>(OnChangeDisplayUnitsCommandReceived);
        this.commandService.Subscribe<ChangeThresholdCommand>(OnChangeThresholdCommandReceived);
    }

    private void OnChangeDisplayUnitsCommandReceived(ChangeDisplayUnitsCommand command)
    {
        Temperature.UnitType? requestedUnits = null;

        switch (command.Units.ToUpper())
        {
            case "CELSIUS":
            case "C":
                requestedUnits = Temperature.UnitType.Celsius;
                break;
            case "FAHRENHEIT":
            case "F":
                requestedUnits = Temperature.UnitType.Fahrenheit;
                break;
            case "KELVIN":
            case "K":
                requestedUnits = Temperature.UnitType.Kelvin;
                break;
        }

        if (requestedUnits == null)
        {
            Resolver.Log.Info($"Change units command received. Requested units {command.Units} is unknown");
        }
        else
        {
            UnitsChangeRequested?.Invoke(this, requestedUnits.Value);
        }
    }

    private void OnChangeThresholdCommandReceived(ChangeThresholdCommand command)
    {
        Temperature? thresholdRequest = null;

        if (command.TempC != null)
        {
            thresholdRequest = command.TempC.Value.Celsius();
            Resolver.Log.Info($"Change threshold command received. Requested threshold: {thresholdRequest.Value.Celsius:N1}C");
        }
        else if (command.TempF != null)
        {
            thresholdRequest = command.TempF.Value.Fahrenheit();
            Resolver.Log.Info($"Change threshold command received. Requested threshold: {thresholdRequest.Value.Fahrenheit:N1}F");
        }
        else
        {
            Resolver.Log.Info($"Change threshold command received. Requested threshold value is missing");
        }

        if (thresholdRequest != null)
        {
            ThresholdTemperatureChangeRequested?.Invoke(this, thresholdRequest.Value);
        }
    }
}
