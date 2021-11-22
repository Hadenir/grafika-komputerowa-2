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

        private Texture(int width, int height, Vec3[] pixels)
        {
            Width = width;
            Height = height;
            Pixels = pixels;
        }

        public static Texture FromFile(string path)
        {
            using var bitmap = new Bitmap(path);
            var width = bitmap.Width;
            var height = bitmap.Height;

            var pixels = new Vec3[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    pixels[x + y * width] = new Vec3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
                }
            }

            return new Texture(width, height, pixels);
        }

        public static Texture FromNormalMap(string path)
        {
            using var bitmap = new Bitmap(path);
            var width = bitmap.Width;
            var height = bitmap.Height;

            var pixels = new Vec3[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    pixels[x + y * width] = new Vec3(color.R / 255.0f * 2 - 1, color.G / 255.0f * 2 - 1, color.B / 255.0f);
                }
            }

            return new Texture(width, height, pixels);
        }

        public Vec3 GetPixel(float u, float v)
        {
            int x = (int)(Math.Clamp(u, 0, 1) * Width);
            int y = (int)(Math.Clamp(v, 0, 1) * Height);

            return Pixels[x + y * Width];
        }
    }
}