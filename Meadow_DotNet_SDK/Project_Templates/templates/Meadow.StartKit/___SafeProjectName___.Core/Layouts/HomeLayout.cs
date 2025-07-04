﻿using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace ___SafeProjectName___.Core;

internal class HomeLayout : AbsoluteLayout
{
    protected IFont MediumFont { get; }
    protected IFont LargeFont { get; }

    public HomeLayout(DisplayScreen screen)
        : base(screen)
    {
        LargeFont = new Font12x20();
        MediumFont = new Font8x16();

        this.BackgroundColor = Color.FromRgb(50, 50, 50);
    }

    public Temperature.UnitType Units { get; set; }
    public virtual Temperature DisplayTemperature { get; set; }
}