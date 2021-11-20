using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa2
{
    public class Canvas
    {
        private readonly int backgroundColor = GetColor(Colors.White);

        public WriteableBitmap WriteableBitmap { get; private set; }

        public Canvas() : this(1, 1) { }

        public Canvas(double width, double height)
        {
            WriteableBitmap = CreateBitmap(width, height);

            Clear();
        }

        public void Resize(double width, double height)
        {
            if (width == WriteableBitmap.Width && height == WriteableBitmap.Height) return;

            var oldBitmap = WriteableBitmap;
            try
            {
                oldBitmap.Lock();
                WriteableBitmap = CreateBitmap(width, height);
            }
            finally
            {
                oldBitmap.Unlock();
            }

            Clear();
        }

        public void Clear()
        {
            try
            {
                WriteableBitmap.Lock();

                unsafe
                {
                    IntPtr backBuffer = WriteableBitmap.BackBuffer;
                    int width = (int)WriteableBitmap.Width;
                    int height = (int)WriteableBitmap.Height;
                    var bytesPerPixel = WriteableBitmap.Format.BitsPerPixel / 8;

                    for (int i = 0; i < width * height; i++)
                    {
                        *(int*)backBuffer = backgroundColor;
                        backBuffer += bytesPerPixel;
                    }

                    MarkDirty();
                }
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        public void Render(IEnumerable<Triangle> triangles)
        {
            try
            {
                WriteableBitmap.Lock();

                Clear();

                foreach (var triangle in triangles)
                {
                    // TODO: Implement rendering
                }

                MarkDirty();
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        public void Render(IEnumerable<Vec3> vertices)
        {
            try
            {
                WriteableBitmap.Lock();

                Clear();

                int width = (int)WriteableBitmap.Width;
                int height = (int)WriteableBitmap.Height;
                foreach (var vertex in vertices)
                {
                    int x = (int)(vertex.X + width / 2);
                    int y = (int)(vertex.Y + height / 2);
                    PutPixel(x, y, GetColor(Colors.Black));
                }

                MarkDirty();
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        private void MarkDirty() => WriteableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)WriteableBitmap.Width, (int)WriteableBitmap.Height));

        private void PutPixel(int x, int y, int color)
        {
            if (x < 0 || y < 0 || x >= WriteableBitmap.Width || y >= WriteableBitmap.Height) return;

            unsafe
            {
                IntPtr backBuffer = WriteableBitmap.BackBuffer;
                backBuffer += y * WriteableBitmap.BackBufferStride;
                backBuffer += x * WriteableBitmap.Format.BitsPerPixel / 8;

                *(int*)backBuffer = color;
            }
        }

        private void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            var dx = Math.Abs(x2 - x1);
            var sx = x1 < x2 ? 1 : -1;
            var dy = -Math.Abs(y2 - y1);
            var sy = y1 < y2 ? 1 : -1;
            var err = dx + dy;
            while (true)
            {
                PutPixel(x1, y1, color);
                if (x1 == x2 && y1 == y2) break;

                var e2 = err * 2;
                if (e2 >= dy)
                {
                    err += dy;
                    x1 += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        private static WriteableBitmap CreateBitmap(double width, double height)
            => new((int)width, (int)height, 96, 96, PixelFormats.Bgr32, null);

        private static int GetColor(byte r, byte g, byte b) => (r << 16) | (g << 8) | b;

        private static int GetColor(Color color) => GetColor(color.R, color.G, color.B);
    }
}