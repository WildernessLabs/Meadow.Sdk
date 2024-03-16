using System;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.Core
{
    public class InputController
    {
        public event EventHandler? UnitDownRequested;
        public event EventHandler? UnitUpRequested;

        public InputController(I$safeprojectname$Hardware platform)
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
}
