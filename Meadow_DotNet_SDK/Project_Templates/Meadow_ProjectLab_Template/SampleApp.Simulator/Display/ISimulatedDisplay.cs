using Meadow.Foundation;
using Meadow.Foundation.Graphics;

namespace ProjectLabSimulator.Displays
{
    internal interface ISimulatedDisplay
    {
        public ColorMode ColorMode { get; }

        public Color ForegroundColor { get; }
        public Color BackgroundColor { get; }

        public int Width { get; }

        public int Height { get; }
    }
}