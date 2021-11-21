using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrafikaKomputerowa2
{
    public class Scene
    {
        private float radius;
        private float lightRadius;
        private float t;

        public IEnumerable<Triangle> Triangles { get; private set; } = Enumerable.Empty<Triangle>();
        public LightSource LightSource { get; private set; } = new();

        public void PrepareScene(float radius, float precisionFactor)
        {
            this.radius = radius;
            Triangles = SphereTriangulator.CreateSemiSphere(radius, precisionFactor);
            LightSource = new LightSource(new Vec3(0, 0, radius + 300), new Vec3(1, 1, 1));
        }

        public void AnimateLight(float dt)
        {
            t += dt;
            lightRadius = (float)Math.Sin(t / 25.0f) * radius;
            LightSource.Position.X = lightRadius * (float)Math.Cos(t);
            LightSource.Position.Y = lightRadius * (float)Math.Sin(t);
        }

        public Vec3 GetPointOnSurface(float x, float y)
        {
            var z = (float)Math.Sqrt(radius * radius - x * x - y * y);
            return new(x, y, z);
        }
    }
}