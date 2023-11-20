using Avalonia.Controls;
using Meadow;
using Meadow.Cloud;
using ProjectLabSimulator.Displays;
using SampleApp.Controllers;
using SampleApp.Simulator.Hardware;

namespace ProjectLabSimulator.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainAppController mainAppController;
        readonly SimulatedHardware simulatedHardware;

        readonly int scale = 2;

        public MainWindow()
        {
            InitializeComponent();

            simulatedHardware = new SimulatedHardware()
            {
                Display = LoadDisplay()
            };

            mainAppController = new MainAppController(simulatedHardware);

            mainAppController.Run();

            buttonDown.Click += ButtonDown_Click;
        }

        private void ButtonDown_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var cloudEvent = new CloudEvent()
            {
                Description = "Button Down",
            };

            Resolver.MeadowCloudService.SendEvent(cloudEvent);
        }

        PixelCanvas LoadDisplay()
        {
            var simDisplay = new Ili9341Simulated();

            var canvas = new PixelCanvas(simDisplay.Width, simDisplay.Height, simDisplay.ColorMode)
            {
                Width = simDisplay.Width * scale,
                Height = simDisplay.Height * scale,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                EnabledColor = simDisplay.ForegroundColor,
                DisabledColor = simDisplay.BackgroundColor,
            };

            displayBorder.Child = canvas;
            displayBorder.Width = canvas.Width;

            return canvas;
        }
    }
}