using GrafikaKomputerowa2.Algebra;
using System;

namespace GrafikaKomputerowa2
{
    public static class Utils
    {
        public static Vec3 SphericalToCartesian(double meridian, double parallel, float radius)
            => new((float)(radius * Math.Cos(meridian) * Math.Sin(parallel)),
                   (float)(radius * Math.Sin(meridian) * Math.Sin(parallel)),
                   (float)(radius * Math.Cos(parallel)));
    }
}