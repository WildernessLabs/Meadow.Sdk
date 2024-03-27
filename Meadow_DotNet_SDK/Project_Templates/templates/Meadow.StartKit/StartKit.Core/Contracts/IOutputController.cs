using System;
using System.Threading.Tasks;

namespace $safeprojectname$.Core
{
    public interface IOutputController
    {
        Task SetState(bool state);
    }
}