using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GrafikaKomputerowa2
{
    public class Scene
    {
        public IEnumerable<Triangle> Triangles { get; private set; } = Enumerable.Empty<Triangle>();
        public LightSource LightSource { get; private set; }

        public void PrepareScene(float radius, float precisionFactor)
        {
            Triangles = SphereTriangulator.CreateSemiSphere(radius, precisionFactor);
            LightSource = new LightSource(new Vec3(0, 0, radius + 50), new Vec3(1, 1, 1));
        }

        public IEnumerable<Vec3> GetAllVertices() => Triangles.SelectMany(t => t.Vertices).Distinct();
    }
}