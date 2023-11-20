using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using System;

namespace ProjectLabSimulator
{
    public class PixelCanvas : Control, IGraphicsDisplay
    {
        public ColorMode ColorMode { get; private set; }

        public ColorMode SupportedColorModes => ColorMode;

        int IGraphicsDisplay.Width => width;
        int width;

        int IGraphicsDisplay.Height => height;
        int height;

        public IPixelBuffer PixelBuffer => pixelBuffer;

        /// <summary>
        /// The color to draw when a pixel is enabled
        /// </summary>
        public Meadow.Foundation.Color EnabledColor { get; set; } = Meadow.Foundation.Color.White;

        /// <summary>
        /// The color to draw when a pixel is disabled
        /// </summary>
        public Meadow.Foundation.Color DisabledColor { get; set; } = Meadow.Foundation.Color.Black;

        IPixelBuffer pixelBuffer;

        public PixelCanvas(int width, int height, ColorMode colorMode)
        {
            ColorMode = colorMode;
            SetResolution(this.width = width, this.height = height);
        }

        public void SetResolution(int width, int height)
        {
            CreateBuffer(this.width = width, this.height = height);
        }

        void CreateBuffer(int width, int height)
        {
            switch (ColorMode)
            {
                case ColorMode.Format16bppRgb565:
                    pixelBuffer = new BufferRgb565(width, height);
                    break;
                case ColorMode.Format12bppRgb444:
                    pixelBuffer = new BufferRgb444(width, height);
                    break;
                case ColorMode.Format8bppGray:
                    pixelBuffer = new BufferGray8(width, height);
                    break;
                case ColorMode.Format4bppGray:
                    pixelBuffer = new BufferGray4(width, height);
                    break;
                case ColorMode.Format1bpp:
                    pixelBuffer = new Buffer1bpp(width, height);
                    break;
                case ColorMode.Format32bppRgba8888:
                default:
                    pixelBuffer = new BufferRgb888(width, height);
                    break;
            }
        }

        private void RedrawPixels(DrawingContext context)
        {
            int bitmapWidth = width;
            int bitmapHeight = height;

            if (bitmapWidth <= 0 || bitmapHeight <= 0 || pixelBuffer == null)
            {
                return;
            }

            var writeableBitmap = new WriteableBitmap(new PixelSize(bitmapWidth, bitmapHeight), new Vector(96, 96), PixelFormat.Rgba8888);

            using (var lockedFrameBuffer = writeableBitmap.Lock())
            {
                unsafe
                {
                    IntPtr bufferPtr = new(lockedFrameBuffer.Address.ToInt64());

                    int stride = lockedFrameBuffer.RowBytes;

                    for (int y = 0; y < bitmapHeight; y++)
                    {
                        for (int x = 0; x < bitmapWidth; x++)
                        {
                            // Calculate the byte index for the current pixel
                            int index = (y * stride) + (x * 4);

                            // Read data from the buffer
                            //var color = pixelBuffer.GetPixel(x, y);
                            var color = GetPixel(x, y);

                            byte b = color.B;
                            byte g = color.G;
                            byte r = color.R;
                            byte a = color.A;

                            // Pack BGRA data into a uint
                            uint color_data = (uint)((a << 24) | (b << 16) | (g << 8) | r);

                            *((uint*)bufferPtr) = color_data;
                            bufferPtr += 4;
                        }
                    }
                }
            }

            context.DrawImage(writeableBitmap, Bounds);
        }


        public override void Render(DrawingContext context)
        {
            RedrawPixels(context);
        }

        public void Show()
        {
            InvalidateVisual();
        }

        public void Show(int left, int top, int right, int bottom)
        {
            InvalidateVisual();
        }

        public void Clear(bool updateDisplay = false)
        {
            pixelBuffer?.Clear();
        }

        public void Fill(Meadow.Foundation.Color fillColor, bool updateDisplay = false)
        {
            pixelBuffer?.Fill(fillColor);

            if (updateDisplay)
            {
                Show();
            }
        }

        public void Fill(int x, int y, int width, int height, Meadow.Foundation.Color fillColor)
        {
            pixelBuffer?.Fill(x, y, width, height, fillColor);
        }

        public void DrawPixel(int x, int y, Meadow.Foundation.Color color)
        {
            pixelBuffer?.SetPixel(x, y, color);
        }

        public void DrawPixel(int x, int y, bool enabled)
        {
            pixelBuffer?.SetPixel(x, y, enabled ? EnabledColor : DisabledColor);
        }

        public void InvertPixel(int x, int y)
        {
            pixelBuffer?.InvertPixel(x, y);
        }

        public Meadow.Foundation.Color GetPixel(int x, int y)
        {
            if (ColorMode == ColorMode.Format1bpp)
            {
                return pixelBuffer.GetPixel(x, y) == Meadow.Foundation.Color.White ? EnabledColor : DisabledColor;
            }
            return pixelBuffer.GetPixel(x, y);
        }

        public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
        {
            pixelBuffer?.WriteBuffer(x, y, displayBuffer);
        }
    }
}
