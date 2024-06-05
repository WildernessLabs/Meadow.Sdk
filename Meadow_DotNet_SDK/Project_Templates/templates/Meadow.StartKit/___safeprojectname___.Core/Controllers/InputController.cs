using System;
using System.Threading.Tasks;
using ___safeprojectname___.Core.Contracts;

namespace ___safeprojectname___.Core;

public class InputController
{
    public event EventHandler? UnitDownRequested;
    public event EventHandler? UnitUpRequested;

    public InputController(I___safeprojectname___Hardware platform)
    {
        if (platform.LeftButton is { } ub)
        {
            ub.PressStarted += (s, e) => UnitDownRequested?.Invoke(this, EventArgs.Empty);
        }
        if (platform.RightButton is { } db)
        {
            db.PressStarted += (s, e) => UnitDownRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
