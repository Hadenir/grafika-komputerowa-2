using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            WriteableBitmap = CreateBitmap((int)width, (int)height);

            Clear();
        }

        public void Resize(double newWidth, double newHeight)
        {
            var width = (int)newWidth;
            var height = (int)newHeight;
            if (width == WriteableBitmap.PixelWidth && height == WriteableBitmap.PixelHeight) return;

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
                    int width = WriteableBitmap.PixelWidth;
                    int height = WriteableBitmap.PixelHeight;
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

                var color = GetColor(Colors.Red);
                var color2 = GetColor(Colors.Black);
                foreach (var triangle in triangles)
                {
                    //int width = (int)WriteableBitmap.Width;
                    //int height = (int)WriteableBitmap.Height;
                    //int x0 = (int)triangle[0].X + width / 2;
                    //int y0 = (int)triangle[0].Y + height / 2;
                    //int x1 = (int)triangle[1].X + width / 2;
                    //int y1 = (int)triangle[1].Y + height / 2;
                    //int x2 = (int)triangle[2].X + width / 2;
                    //int y2 = (int)triangle[2].Y + height / 2;

                    FillTriangle(triangle, color2);

                    //DrawLine(x0, y0, x1, y1, color);
                    //DrawLine(x1, y1, x2, y2, color);
                    //DrawLine(x2, y2, x0, y0, color);
                }

                MarkDirty();
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        private void MarkDirty() => WriteableBitmap.AddDirtyRect(new Int32Rect(0, 0, WriteableBitmap.PixelWidth, WriteableBitmap.PixelHeight));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PutPixel(int x, int y, int color)
        {
            if (x < 0 || y < 0 || x >= WriteableBitmap.PixelWidth || y >= WriteableBitmap.PixelHeight) return;

            unsafe
            {
                int* backBuffer = (int*)WriteableBitmap.BackBuffer;
                int index = x + (y * WriteableBitmap.BackBufferStride / 4);
                backBuffer[index] = color;
            }
        }

        private void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            var a = x1;
            var b = y1;
            var c = x2;
            var d = y2;

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

        private class AetEntry
        {
            public float x;
            public float dx;
            public Vec3 end;

            public AetEntry(Vec3 v1, Vec3 v2)
            {
                x = v1.X;
                dx = (v2.X - v1.X) / (v2.Y - v1.Y);
                end = v2;

                if (float.IsInfinity(dx)) x = v2.X;
            }
        }

        private unsafe void FillTriangle(Triangle triangle, int color)
        {
            var width = (int)WriteableBitmap.Width;
            var height = (int)WriteableBitmap.Height;
            int* backBuffer = (int*)WriteableBitmap.BackBuffer;

            int GetIndex(int x, int y) => x + (width / 2) + ((y + height / 2) * WriteableBitmap.BackBufferStride / 4);

            var vertices = triangle.Vertices.ToList();
            var ind = vertices
                .Select((v, i) => (v, i))
                .OrderBy(p => p.v.Y)
                .Select(p => p.i)
                .ToList();

            var yMin = vertices[ind[0]].Y;
            var yMax = vertices[ind[2]].Y;
            var AET = new List<AetEntry>()
            {
                new(vertices[ind[0]], vertices[ind[1]]),
                new(vertices[ind[0]], vertices[ind[2]]),
            };

            if (float.IsInfinity(AET[0].dx))
            {
                if (yMin >= -height / 2 && yMin < height / 2)
                    for (var x = (int)AET[0].x; x <= AET[1].x; x++)
                        if (x >= -width / 2 && x < width / 2)
                            backBuffer[GetIndex(x, (int)yMin)] = color;

                AET[0] = new(AET[0].end, vertices[ind[2]]);
            }

            if (float.IsInfinity(AET[1].dx))
            {
                if (yMin >= -height / 2 && yMin < height / 2)
                    for (var x = (int)AET[0].x; x <= AET[1].x; x++)
                        if (x >= -width / 2 && x < width / 2)
                            backBuffer[GetIndex(x, (int)yMin)] = color;

                AET[1] = new(AET[1].end, vertices[ind[2]]);
            }

            for (int y = (int)yMin; y <= yMax; y++)
            {
                AET.Sort((e1, e2) => (int)(e1.x - e2.x));

                if (y >= -height / 2 && y < height / 2)
                    for (var x = (int)AET[0].x; x <= AET[1].x; x++)
                        if (x >= -width / 2 && x < width / 2)
                            backBuffer[GetIndex(x, y)] = color;

                foreach (var e in AET) e.x += e.dx;

                foreach (var v in AET.Select(ae => ae.end).Where(v => (int)v.Y == y).ToList())
                {
                    AET.RemoveAll(e => e.end == v);

                    var i = vertices.IndexOf(v);
                    Vec3 previous;
                    if (i == 0)
                        previous = vertices[2];
                    else
                        previous = vertices[i - 1];
                    Vec3 next;
                    if (i == 2)
                        next = vertices[0];
                    else
                        next = vertices[i + 1];

                    if (previous.Y >= v.Y)
                        AET.Add(new AetEntry(v, previous));

                    if (next.Y >= v.Y)
                        AET.Add(new AetEntry(v, next));
                }
            }
        }

        private static WriteableBitmap CreateBitmap(int width, int height)
            => new(width, height, 96, 96, PixelFormats.Bgr32, null);

        private static int GetColor(byte r, byte g, byte b) => (r << 16) | (g << 8) | b;

        private static int GetColor(Color color) => GetColor(color.R, color.G, color.B);
    }
}