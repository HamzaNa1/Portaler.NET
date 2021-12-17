using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portaler.NET.Shared;
using Portaler.NET.Shared.GameInfo;

namespace Portaler.NET.Client.Visualiser
{
    public class Ball
    {
        private static int IDCounter = 0;

        public int ID { get; }

        private Vector2d _forces;

        public Vector2d Acceleration { get; set; }
        public Vector2d Velocity { get; set; }
        public Vector2d Position { get; set; }

        public ZoneInfo? Zone { get; set; }

        public double Radius { get; set; }

        public HashSet<Ball> Masters { get; set; }

        public Ball(Vector2d position, double radius)
        {
            ID = IDCounter--;

            Position = position;
            Radius = radius;

            Acceleration = Vector2d.Zero;
            Velocity = Vector2d.Zero;

            Masters = new HashSet<Ball>();
        }

        public Ball()
        {
            ID = IDCounter++;

            Position = Vector2d.Random * 50;
            Radius = 20;

            Acceleration = Vector2d.Zero;
            Velocity = Vector2d.Zero;
        }

        public void Update(double deltaTime)
        {
            Acceleration = (_forces + Velocity * -Config.Main.Drag);
            Velocity += Acceleration * deltaTime;
            Position += Velocity * deltaTime;

            if (Velocity.Length() < 0.0001)
            {
                Velocity = Vector2d.Zero;
            }

            _forces = Vector2d.Zero;
        }
        
        public void ApplyForces(IEnumerable<Vector2d> forces)
        {
            if (!forces.TryGetNonEnumeratedCount(out int count))
            {
                count = forces.Count();
            }

            for (int i = 0; i < count; i++)
            {
                _forces += forces.ElementAt(i);
            }
        }

        public bool IsCollidingWith(Ball other)
        {
            return IsCollidingWith(other.Position, other.Radius);
        }

        public bool IsCollidingWith(Vector2d position, double radius)
        {
            return Position.Distance(position) <= Radius + radius;
        }

        public bool IsInside(double x, double y)
        {
            return IsInside(new Vector2d(x, y));
        }

        public bool IsInside(Vector2d point)
        {
            return Position.Distance(point) <= Radius;
        }

        public void Move(Vector2d offset)
        {
            Position += offset;
        }

        public void Move(double offsetX, double offsetY)
        {
            Position += new Vector2d(offsetX, offsetY);
        }
    }
}
