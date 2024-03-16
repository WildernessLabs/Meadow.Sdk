using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace $safeprojectname$.Core
{
    public class DisplayController
    {
        private readonly DisplayScreen? screen;

        private Label displayTempLabel;

        private Picture networkIcon;
        private Image connectedImage;
        private Image disconnectedImage;

        private Temperature displayTemp;
        private Temperature.UnitType displayUnits;
        private bool isNetworkConnected = false;

        public DisplayController(
            IPixelDisplay? display,
            RotationType displayRotation,
            Temperature.UnitType unit)
        {
            if (display != null)
            {
                var theme = new DisplayTheme
                {
                    Font = new Font12x20(),
                    BackgroundColor = Color.Black,
                    TextColor = Color.White
                };

                screen = new DisplayScreen(
                    display,
                    rotation: displayRotation,
                    theme: theme);

                GenerateLayout(screen);
            }

            UpdateDisplay();
        }

        private void GenerateLayout(DisplayScreen screen)
        {

            displayTempLabel = new Label(0, 0, screen.Width, screen.Height)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            connectedImage = Image.LoadFromResource("$safeprojectname$.Core.Assets.net-connected.bmp");
            disconnectedImage = Image.LoadFromResource("$safeprojectname$.Core.Assets.net-disconnected.bmp");

            networkIcon = new Picture(screen.Width - disconnectedImage.Width, 0, disconnectedImage.Width, disconnectedImage.Height, disconnectedImage);

            screen.Controls.Add(displayTempLabel, networkIcon);
        }

        public void SetNetworkStatus(bool isConnected)
        {
            isNetworkConnected = isConnected;
            UpdateDisplay();
        }

        public void UpdateCurrentTemperature(Temperature temperature)
        {
            displayTemp = temperature;
            UpdateDisplay();
        }

        public void UpdateDisplayUnits(Temperature.UnitType units)
        {
            displayUnits = units;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var unitLabel = displayUnits switch
            {
                Temperature.UnitType.Celsius => "C",
                Temperature.UnitType.Fahrenheit => "F",
                _ => "K"
            };

            var text = $"{displayTemp.From(displayUnits):N1}°{unitLabel}";

            if (screen != null)
            {
                screen.BeginUpdate();
                displayTempLabel.Text = text;
                networkIcon.Image = isNetworkConnected ? connectedImage : disconnectedImage;
                screen.EndUpdate();
            }
            else
            {
                Resolver.Log.Info(text);
            }
        }
    }
}