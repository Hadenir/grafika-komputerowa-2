using GrafikaKomputerowa2.Algebra;
using GrafikaKomputerowa2.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GrafikaKomputerowa2
{
    public class Scene
    {
        public IEnumerable<Triangle> Triangles { get; } = new List<Triangle>();

        public Scene()
        { }

        public IEnumerable<Vec3> GetAllVertices() => Triangles.SelectMany(t => t.Vertices).Distinct();
    }
}