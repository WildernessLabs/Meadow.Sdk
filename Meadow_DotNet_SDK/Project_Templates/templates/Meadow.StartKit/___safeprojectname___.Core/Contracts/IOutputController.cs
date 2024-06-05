using System;
using System.Threading.Tasks;

namespace ___safeprojectname___.Core;

public interface IOutputController
{
    Task SetState(bool state);
}