namespace $safeprojectname$.Core
{
    public interface IOutputController
    {
        Task SetState(bool state);
    }
}