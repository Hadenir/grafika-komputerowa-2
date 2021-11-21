using GrafikaKomputerowa2.Algebra;
using System;
using System.Drawing;

namespace GrafikaKomputerowa2.Graphics
{
    public class Texture
    {
        public Vec3[] Pixels { get; }
        public int Width { get; }
        public int Height { get; }

        public Texture(string path)
        {
            var bitmap = new Bitmap(path);
            Width = bitmap.Width;
            Height = bitmap.Height;

            Pixels = new Vec3[Width * Height];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Pixels[x + y * Width] = new Vec3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
                }
            }
        }

        public Vec3 GetPixel(float u, float v)
        {
            int x = (int)(Math.Clamp(u, 0, 1) * Width);
            int y = (int)(Math.Clamp(v, 0, 1) * Height);

            return Pixels[x + y * Width];
        }
    }
}