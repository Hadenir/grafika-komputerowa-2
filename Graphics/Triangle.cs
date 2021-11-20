using GrafikaKomputerowa2.Algebra;

namespace GrafikaKomputerowa2.Graphics
{
    public class Triangle
    {
        public Vec3[] Vertices { get; } = new Vec3[3];
        public Vec3[] Normals { get; } = new Vec3[3];

        public Triangle(Vec3 v1, Vec3 v2, Vec3 v3)
        {
            var e1 = v2 - v1;
            var e2 = v3 - v1;
            var n = e1.Cross(e2);

            Vertices[0] = v1;
            Vertices[1] = v2;
            Vertices[2] = v3;
            Normals[0] = n;
            Normals[1] = n;
            Normals[2] = n;
        }

        public Triangle(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 n1, Vec3 n2, Vec3 n3)
        {
            Vertices[0] = v1;
            Vertices[1] = v2;
            Vertices[2] = v3;
            Normals[0] = n1;
            Normals[1] = n2;
            Normals[2] = n3;
        }

        public ref Vec3 this[int i] => ref Vertices[i];
    }
}