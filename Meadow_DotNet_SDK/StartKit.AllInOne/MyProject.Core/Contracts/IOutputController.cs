namespace MyProject.Core
{

    public interface IOutputController
    {
        Task SetMode(ThermostatMode mode);
    }
}
