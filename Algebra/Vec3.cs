using System;

namespace GrafikaKomputerowa2.Algebra
{
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float LengthSq => (X * X) + (Y * Y) + (Z * Z);
        public float Length => (float)Math.Sqrt(LengthSq);

        public Vec3() : this(0, 0, 0) { }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float this[int i] => i switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            _ => throw new ArgumentException("Argument out of bounds", paramName: nameof(i))
        };

        public void Normalize()
        {
            var length = Length;
            if (length == 0) return;

            X /= length;
            Y /= length;
            Z /= length;
        }

        public Vec3 Normalized()
        {
            var length = Length;
            if (length == 0) return new();

            return Clone() / length;
        }

        public Vec3 Dot(Vec3 other) => new(X * other.X, Y * other.Y, Z * other.Z);

        public Vec3 Cross(Vec3 other) => new(
            (Y * other.Z) - (Z * other.Y),
            (Z * other.X) - (X * other.Z),
            (X * other.Y) - (Y * other.X));

        private Vec3 Clone() => new(X, Y, Z);

        public static Vec3 operator -(Vec3 v) => new(-v.X, -v.Y, -v.Z);

        public static Vec3 operator +(Vec3 v1, Vec3 v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        public static Vec3 operator -(Vec3 v1, Vec3 v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        public static Vec3 operator *(float c, Vec3 v) => new(c * v.X, c * v.Y, c * v.Z);

        public static Vec3 operator /(Vec3 v, float c) => new(v.X / c, v.Y / c, v.Z / c);
    }
}