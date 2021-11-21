using GrafikaKomputerowa2.Algebra;

namespace GrafikaKomputerowa2.Graphics
{
    public class LightSource
    {
        public Vec3 Position { get; set; }
        public Vec3 Color { get; set; }

        public LightSource() : this(Vec3.Zero(), Vec3.One()) { }

        public LightSource(Vec3 position, Vec3 color)
        {
            Position = position;
            Color = color;
        }
    }
}