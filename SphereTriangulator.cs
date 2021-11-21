using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System;
using System.Collections.Generic;

namespace GrafikaKomputerowa2
{
    public static class SphereTriangulator
    {
        public static IEnumerable<Triangle> CreateSemiSphere(float radius, float precisionFactor)
        {
            precisionFactor = Math.Clamp(precisionFactor, 0, 1);

            var parallelsCount = (int)(precisionFactor * 18 + 2);
            var meridiansCount = (int)(precisionFactor * 17 + 3);

            var vertices = new List<Vec3> { Utils.SphericalToCartesian(0, 0, radius) };
            for (var j = 1; j < parallelsCount; j++)
            {
                var parallel = Math.PI / 2 * j / parallelsCount;
                for (var i = 0; i < meridiansCount; i++)
                {
                    var meridian = 2 * Math.PI * i / meridiansCount;
                    vertices.Add(Utils.SphericalToCartesian(meridian, parallel, radius));
                }
            }

            int WrappedIndex(int meridian, int parallel)
            {
                while (meridian < 0) meridian += meridiansCount;
                while (meridian >= meridiansCount) meridian -= meridiansCount;

                return meridian + (parallel - 1) * meridiansCount + 1;
            }

            var center = vertices[0];
            for (var i = 0; i < meridiansCount; i++)
            {
                var previous = vertices[WrappedIndex(i - 1, 1)];
                var current = vertices[WrappedIndex(i, 1)];
                yield return new Triangle(center, previous, current);
            }

            for (var j = 2; j < parallelsCount; j++)
            {
                for (var i = 0; i < meridiansCount; i++)
                {
                    var previusParallel = vertices[WrappedIndex(i, j - 1)];
                    var previusParallelNext = vertices[WrappedIndex(i + 1, j - 1)];
                    var previous = vertices[WrappedIndex(i - 1, j)];
                    var current = vertices[WrappedIndex(i, j)];

                    yield return new Triangle(previusParallel, previous, current);
                    yield return new Triangle(previusParallel, current, previusParallelNext);
                }
            }
        }
    }
}