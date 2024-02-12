using Meadow;
using Meadow.Cloud;
using Meadow.Units;

namespace $safeprojectname$.Core
{

    public class CloudService
    {
        private ICommandService _commandService;

        public event EventHandler<SetPoints> NewSetpointsReceived = default!;

        public CloudService(ICommandService commandService)
        {
            _commandService = commandService;
            _commandService.Subscribe<ChangeSetpointsCommand>(OnChangeSetpointsCommandReceived);
        }

        private void OnChangeSetpointsCommandReceived(ChangeSetpointsCommand command)
        {
            Temperature? heatTo = null;
            Temperature? coolTo = null;

            // only one set of units is allowable in a single commnd, C wins
            if (command.HeatToC is { } heatToC)
            {
                heatTo = new Temperature(heatToC, Temperature.UnitType.Celsius);
            }
            else if (command.HeatToF is { } heatToF)
            {
                heatTo = new Temperature(heatToF, Temperature.UnitType.Fahrenheit);
            }

            if (command.CoolToC is { } coolToC)
            {
                coolTo = new Temperature(coolToC, Temperature.UnitType.Celsius);
            }
            else if (command.CoolToF is { } coolToF)
            {
                coolTo = new Temperature(coolToF, Temperature.UnitType.Fahrenheit);
            }

            if (heatTo != null || coolTo != null)
            {
                var setpoints = new SetPoints
                {
                    HeatTo = heatTo,
                    CoolTo = coolTo
                };

                Resolver.Log.Info($"Command received: {setpoints}");

                NewSetpointsReceived?.Invoke(this, setpoints);
            }
        }

        public async Task RecordSetPointChange(SetPoints setpoints)
        {
            var @event = new CloudEvent
            {
                Timestamp = DateTimeOffset.UtcNow,
                Description = "Set point change",
                Measurements = new()
            {

                { "HeatTo", setpoints!.HeatTo!.Value.Celsius },
                { "CoolTo", setpoints!.CoolTo!.Value.Celsius }
            }
            };

            await Resolver.MeadowCloudService.SendEvent(@event);
        }

        public async Task RecordTransition(ThermostatMode previousMode, ThermostatMode newMode)
        {
            var @event = new CloudEvent
            {
                Timestamp = DateTimeOffset.UtcNow,
                Description = "Thermostat state change",
                Measurements = new()
            {

                { "PreviousMode", previousMode },
                { "NewMode", newMode }
            }
            };

            await Resolver.MeadowCloudService.SendEvent(@event);
        }
    }
}