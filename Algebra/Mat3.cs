namespace GrafikaKomputerowa2.Algebra
{
    public class Mat3
    {
        private readonly float[] elements = new float[9];

        public Mat3() : this(0, 0, 0, 0, 0, 0, 0, 0, 0) { }

        public Mat3(
            float e1, float e2, float e3,
            float e4, float e5, float e6,
            float e7, float e8, float e9)
        {
            elements[0] = e1;
            elements[1] = e2;
            elements[2] = e3;
            elements[3] = e4;
            elements[4] = e5;
            elements[5] = e6;
            elements[6] = e7;
            elements[7] = e8;
            elements[8] = e9;
        }

        public static Mat3 FromRows(Vec3 v1, Vec3 v2, Vec3 v3)
            => new(v1.X, v1.Y, v1.Z, v2.X, v2.Y, v2.Z, v3.X, v3.Y, v3.Z);

        public ref float this[int i] => ref elements[i];
        public ref float this[int i, int j] => ref elements[i + (j * 3)];

        public static Mat3 operator +(Mat3 m1, Mat3 m2)
        {
            var result = new Mat3();
            for (var i = 0; i < 9; i++)
                result[i] = m1[i] + m2[i];
            return result;
        }

        public static Mat3 operator -(Mat3 m1, Mat3 m2)
        {
            var result = new Mat3();
            for (var i = 0; i < 9; i++)
                result[i] = m1[i] - m2[i];
            return result;
        }

        public static Mat3 operator *(Mat3 m1, Mat3 m2)
        {
            var result = new Mat3();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    result[i, j] = (m1[i, 0] * m2[0, j]) + (m1[i, 1] * m2[1, j]) + (m1[i, 2] * m2[2, j]);
                }
            }
            return result;
        }

        public static Vec3 operator *(Mat3 m, Vec3 v)
            => new((m[0, 0] * v.X) + (m[0, 1] * v.Y) + (m[0, 2] * v.Z),
                   (m[1, 0] * v.X) + (m[1, 1] * v.Y) + (m[1, 2] * v.Z),
                   (m[2, 0] * v.X) + (m[2, 1] * v.Y) + (m[2, 2] * v.Z));

        public static Mat3 operator *(Vec3 v, Mat3 m)
        {
            var result = new Mat3();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    result[i, j] = v[i] * (m[0, j] + m[1, j] + m[2, j]);
                }
            }
            return result;
        }

        public static Mat3 operator *(float c, Mat3 m)
        {
            var result = new Mat3();
            for (var i = 0; i < 9; i++)
                result[i] = c * m[i];
            return result;
        }

        public static Mat3 operator /(Mat3 m, float c)
        {
            var result = new Mat3();
            for (var i = 0; i < 9; i++)
                result[i] = m[i] / c;
            return result;
        }
    }
}