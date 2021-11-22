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
        private float t;

        public IEnumerable<Triangle> Triangles { get; private set; } = Enumerable.Empty<Triangle>();
        public LightSource LightSource { get; private set; } = new();

        public LightSource Reflector { get; private set; } = new();

        public void PrepareScene(float radius, float precisionFactor, float lightZ)
        {
            this.radius = radius;
            Triangles = SphereTriangulator.CreateSemiSphere(radius, precisionFactor);
            LightSource = new LightSource(new Vec3(0, 0, radius + lightZ), new Vec3(1, 1, 1));
            Reflector = new LightSource(new Vec3(0, 0, 2.5f * radius), new Vec3(1, 0, 0));
        }

        public void MoveLight(float newZ)
        {
            LightSource.Position.Z = radius + newZ;
        }

        public void AnimateLight(float dt)
        {
            t += dt / 10;
            var lightRadius = (float)Math.Sin(t / 25.0f) * radius;
            LightSource.Position.X = lightRadius * (float)Math.Cos(t);
            LightSource.Position.Y = lightRadius * (float)Math.Sin(t);
        }

        public Vec3 GetPointOnSurface(float x, float y)
        {
            var z = (float)Math.Sqrt(radius * radius - x * x - y * y);
            return new(x, y, z);
        }

        public (float u, float v) GetUV(float x, float y) => (0.5f * (x / radius + 1), 0.5f * (y / radius + 1));
    }
}