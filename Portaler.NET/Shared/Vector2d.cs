using System;

namespace Portaler.NET.Shared
{
    public struct Vector2d
    {
        private static readonly Random Rand = new Random();

        public static readonly Vector2d Zero = new Vector2d(0, 0);
        public static readonly Vector2d One = new Vector2d(1, 1);

        public static Vector2d Random => new Vector2d(Rand.NextDouble() * 2 - 1, Rand.NextDouble() * 2 - 1);
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Distance(Vector2d other)
        {
            return (this - other).Length();
        }

        public double DistanceSquared(Vector2d other)
        {
            double distance = Distance(other);

            return distance * distance;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public Vector2d Normalize()
        {
            return new Vector2d(X, Y) / Length();
        }

        public double DotProduct(Vector2d other)
        {
            return X * other.X + Y * other.Y;
        }

        public static Vector2d operator +(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2d operator +(Vector2d v1, double d)
        {
            return new Vector2d(v1.X + d, v1.Y + d);
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2d operator -(Vector2d v1, double d)
        {
            return new Vector2d(v1.X - d, v1.Y - d);
        }

        public static Vector2d operator *(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2d operator *(Vector2d v1, double d)
        {
            return new Vector2d(v1.X * d, v1.Y * d);
        }

        public static Vector2d operator /(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Vector2d operator /(Vector2d v1, double d)
        {
            return new Vector2d(v1.X / d, v1.Y / d);
        }
    }
}
