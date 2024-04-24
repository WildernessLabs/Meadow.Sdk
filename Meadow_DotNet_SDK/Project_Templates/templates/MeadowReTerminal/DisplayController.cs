using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace MeadowApplication.Template
{
    public class DisplayController
    {
        private DisplayScreen displayScreen;

        public DisplayController(IPixelDisplay display)
        {
            displayScreen = new DisplayScreen(display)
            {
                BackgroundColor = Color.FromHex("14607F")
            };

            displayScreen.Controls.Add(new Label(
                left: 0,
                top: 0,
                width: displayScreen.Width,
                height: displayScreen.Height)
            {
                Text = "Hello World",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Font = new Font12x20(),
                ScaleFactor = ScaleFactor.X2
            });
        }
    }
}