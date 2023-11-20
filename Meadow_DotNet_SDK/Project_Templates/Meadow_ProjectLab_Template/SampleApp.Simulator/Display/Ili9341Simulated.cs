using Meadow.Foundation;
using Meadow.Foundation.Graphics;

namespace ProjectLabSimulator.Displays
{
    internal class Ili9341Simulated : ISimulatedDisplay
    {
        public Ili9341Simulated(int width = 320, int height = 240, ColorMode colorMode = ColorMode.Format16bppRgb565)
        {
            this.Width = width;
            this.Height = height;
            this.ColorMode = colorMode;
        }

        public ColorMode ColorMode { get; private set; }
        public Color ForegroundColor => Color.White;
        public Color BackgroundColor => Color.Black;
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}