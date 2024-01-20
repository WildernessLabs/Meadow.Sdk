using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System.Windows.Forms;

namespace MeadowApplication.Template
{
    public class MeadowApp : App<Meadow.Windows>
    {
        WinFormsDisplay? display;
        RgbLed? rgbLed;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing...");

            display = new WinFormsDisplay(320, 240);
            var displayController = new DisplayController(display);

            var expander = new Ft232h();

            rgbLed = new RgbLed(
                redPin: expander.Pins.C2,
                greenPin: expander.Pins.C1,
                bluePin: expander.Pins.C0);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Application.Run(display);

            while (true)
            {
                Resolver.Log.Info("Going through each color...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    rgbLed.SetColor((RgbLedColors)i);
                    await Task.Delay(500);
                }

                await Task.Delay(1000);

                Resolver.Log.Info("Blinking through each color (on 500ms / off 500ms)...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    await rgbLed.StartBlink((RgbLedColors)i);
                    await Task.Delay(3000);
                    await rgbLed.StopAnimation();
                    rgbLed.IsOn = false;
                }

                await Task.Delay(1000);

                Resolver.Log.Info("Blinking through each color (on 1s / off 1s)...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    await rgbLed.StartBlink((RgbLedColors)i, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                    await Task.Delay(3000);
                    await rgbLed.StopAnimation();
                    rgbLed.IsOn = false;
                }

                await Task.Delay(1000);
            }
        }

        public static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            await MeadowOS.Start(args);
        }
    }
}