﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;

namespace MeadowApplication.Template;

public class DisplayController
{
    protected DisplayScreen DisplayScreen { get; set; }

    public DisplayController(IPixelDisplay display)
    {
        DisplayScreen = new DisplayScreen(display)
        {
            BackgroundColor = Color.FromHex("14607F")
        };

        DisplayScreen.Controls.Add(new Label(
            left: 0,
            top: 0,
            width: DisplayScreen.Width,
            height: DisplayScreen.Height)
        {
            Text = "Hello World",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Font = new Font12x20()
        });
    }
}