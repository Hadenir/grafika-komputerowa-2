using System;

namespace GrafikaKomputerowa2.Algebra
{
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float R => X;
        public float G => Y;
        public float B => Z;

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

        public float Dot(Vec3 other) => X * other.X + Y * other.Y + Z * other.Z;

        public Vec3 Cross(Vec3 other) => new(
            (Y * other.Z) - (Z * other.Y),
            (Z * other.X) - (X * other.Z),
            (X * other.Y) - (Y * other.X));

        public int ToInt() => ((int)Math.Clamp(X * 255, 0, 255) << 16) | ((int)Math.Clamp(Y * 255, 0, 255) << 8) | (int)Math.Clamp(Z * 255, 0, 255);

        private Vec3 Clone() => new(X, Y, Z);

        public static Vec3 Zero() => new(0, 0, 0);

        public static Vec3 One() => new(1, 1, 1);

        public static Vec3 operator -(Vec3 v) => new(-v.X, -v.Y, -v.Z);

        public static Vec3 operator +(Vec3 v1, Vec3 v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        public static Vec3 operator -(Vec3 v1, Vec3 v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        public static Vec3 operator *(float c, Vec3 v) => new(c * v.X, c * v.Y, c * v.Z);

        public static Vec3 operator /(Vec3 v, float c) => new(v.X / c, v.Y / c, v.Z / c);

        public static Vec3 operator *(Vec3 v1, Vec3 v2) => new(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
    }
}