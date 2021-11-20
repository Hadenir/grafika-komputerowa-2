using GrafikaKomputerowa2.Algebra;
using System;
using System.Collections.Generic;

namespace GrafikaKomputerowa2
{
    public static class SphereTriangulator
    {
        public static IEnumerable<Vec3> CreateSemiSphere(float radius, float precisionFactor)
        {
            precisionFactor = Math.Clamp(precisionFactor, 0, 1);

            var parallelsCount = (int)(precisionFactor * 48 + 2);
            var meridiansCount = (int)(precisionFactor * 46 + 4);

            for (var j = 0; j < parallelsCount; j++)
            {
                var parallel = Math.PI * (j + 1) / parallelsCount;
                for (var i = 0; i < meridiansCount; i++)
                {
                    var meridian = 2 * Math.PI * i / meridiansCount;
                    yield return Utils.SphericalToCartesian(meridian, parallel, radius);
                }
            }
        }
    }
}