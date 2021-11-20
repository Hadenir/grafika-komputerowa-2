using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GrafikaKomputerowa2
{
    public class Scene
    {
        public IEnumerable<Triangle> Triangles { get; private set; } = Enumerable.Empty<Triangle>();

        public Scene()
        { }

        public void PrepareScene(float radius, float precisionFactor)
        {
            Triangles = SphereTriangulator.CreateSemiSphere(radius, precisionFactor);
        }

        public IEnumerable<Vec3> GetAllVertices() => Triangles.SelectMany(t => t.Vertices).Distinct();
    }
}