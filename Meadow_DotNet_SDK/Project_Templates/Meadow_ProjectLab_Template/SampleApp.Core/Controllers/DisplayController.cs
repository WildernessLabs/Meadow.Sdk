using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using SampleApp.Models;

namespace SampleApp.Controllers
{
	public class DisplayController
	{
        private readonly DisplayScreen screen;

        private readonly Image imgWifi = Image.LoadFromResource("SampleAppCore.Assets.img-wifi.bmp");
        private readonly Image imgSync = Image.LoadFromResource("SampleAppCore.Assets.img-sync.bmp");
        private readonly Image imgWifiFade = Image.LoadFromResource("SampleAppCore.Assets.img-wifi-fade.bmp");
        private readonly Image imgSyncFade = Image.LoadFromResource("SampleAppCore.Assets.img-sync-fade.bmp");

        protected Label StatusLabel { get; set; }

        protected Label TemperatureLabel { get; set; }

        protected Label HumidityLabel { get; set; }

        protected Label PressureLabel { get; set; }

        protected Picture WiFi { get; set; }

        protected Picture Sync { get; set; }

        public DisplayController(IGraphicsDisplay _display, RotationType rotation)
        {
            screen = new DisplayScreen(_display, rotation);

            // background boxes
            screen.Controls.Add(new Box(0, 0, screen.Width, screen.Height) { ForeColor = Meadow.Foundation.Color.White });

            // Row 2 - Orange, Blue, Green
            screen.Controls.Add(new Box(0, 27, 106, 93) { ForeColor = Meadow.Foundation.Color.FromHex("#B35E2C") });
            screen.Controls.Add(new Box(106, 27, 108, 93) { ForeColor = Meadow.Foundation.Color.FromHex("#1A80AA") });
            screen.Controls.Add(new Box(214, 27, 106, 93) { ForeColor = Meadow.Foundation.Color.FromHex("#98A645") });

            screen.Controls.Add(new Box(160, 120, 1, screen.Height) { ForeColor = Meadow.Foundation.Color.Black, Filled = false });
            screen.Controls.Add(new Box(0, 180, screen.Width, 1) { ForeColor = Meadow.Foundation.Color.Black, Filled = false });

            StatusLabel = new Label(2, 6, 12, 16)
            {
                Text = "Hello",
                Font = new Font12x20(),
                TextColor = Meadow.Foundation.Color.Black
            };
            screen.Controls.Add(StatusLabel);

            WiFi = new Picture(286, 3, 30, 21, imgWifiFade);
            screen.Controls.Add(WiFi);

            Sync = new Picture(260, 3, 21, 21, imgSyncFade);
            screen.Controls.Add(Sync);

            screen.Controls.Add(new Label(5, 32, 12, 16)
            {
                Text = "Temp",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White
            });
            screen.Controls.Add(new Label(77, 99, 12, 16)
            {
                Text = "°C",
                Font = new Font12x20(),
                TextColor = Meadow.Foundation.Color.White
            });

            screen.Controls.Add(new Label(111, 32, 12, 16)
            {
                Text = "Humidity",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White
            });
            screen.Controls.Add(new Label(197, 99, 12, 16)
            {
                Text = "%",
                Font = new Font12x20(),
                TextColor = Meadow.Foundation.Color.White
            });

            screen.Controls.Add(new Label(219, 32, 12, 16)
            {
                Text = "Pressure",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White,
            });
            screen.Controls.Add(new Label(290, 99, 12, 16)
            {
                Text = "Atm",
                Font = new Font12x20(),
                TextColor = Meadow.Foundation.Color.White,
                HorizontalAlignment = HorizontalAlignment.Right,
            });

            TemperatureLabel = new Label(50, 70, 12, 16, ScaleFactor.X2)
            {
                Text = "0",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            screen.Controls.Add(TemperatureLabel);
            HumidityLabel = new Label(155, 70, 12, 16, ScaleFactor.X2)
            {
                Text = "0",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            screen.Controls.Add(HumidityLabel);
            PressureLabel = new Label(280, 70, 12, 16, ScaleFactor.X2)
            {
                Text = "0",
                Font = new Font12x16(),
                TextColor = Meadow.Foundation.Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            screen.Controls.Add(PressureLabel);
        }

        public void UpdateWifi(bool connected)
        {
            WiFi.Image = connected ? imgWifi : imgWifiFade;
        }

        public void UpdateSync(bool on)
        {
            Sync.Image = on ? imgSync : imgSyncFade;
        }

        public void UpdateStatus(string status)
        {
            StatusLabel.Text = status;
        }

        public void UpdateModel(SampleModel model)
        {
            screen.BeginUpdate();

            TemperatureLabel.Text = model.Temperature?.Celsius.ToString("N0") ?? "n/a";
            HumidityLabel.Text = model.Humidity?.Percent.ToString("N0") ?? "n/a";
            PressureLabel.Text = model.Pressure?.StandardAtmosphere.ToString("N1") ?? "n/a";

            screen.EndUpdate();
        }
    }
}