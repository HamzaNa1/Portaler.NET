using System;
using Portaler.NET.Shared;
using Portaler.NET.Shared.GameInfo;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Portaler.NET.Client.Visualiser
{
    public class Map
    {
        private const string SAVE_PATH = "save.txt";

        public readonly List<Ball> Balls;
        public readonly List<(Ball ball1, Ball ball2)> CollidingBalls;
        public readonly List<(Ball ball1, Ball ball2)> MasterCollidingBalls;

        public readonly List<Connection> Connections;
        private readonly ConcurrentQueue<ConnectionInfo> connectionsToAdd;

        public Map()
        {
            Balls = new List<Ball>();
            CollidingBalls = new List<(Ball ball1, Ball ball2)>();
            MasterCollidingBalls = new List<(Ball ball1, Ball ball2)>();

            Connections = new List<Connection>();
            connectionsToAdd = new ConcurrentQueue<ConnectionInfo>();
        }

        public void AddConnection(ZoneInfo start, ZoneInfo end, ConnectionType connectionType, int hours, int mins)
        {
            DateTime endTime = DateTime.UtcNow + (TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(mins));

            AddConnection(start, end, connectionType, endTime);
        }

        public bool AddConnection(ZoneInfo start, ZoneInfo end, ConnectionType connectionType, DateTime endTime)
        {
            connectionsToAdd.Enqueue(new ConnectionInfo(start, end, connectionType, endTime));

            return true;
        }

        private bool AddConnection(ConnectionInfo info)
        {
            if (info.Start is null)
            {
                throw new ArgumentNullException(nameof(info.Start));
            }
            else if (info.End is null)
            {
                throw new ArgumentNullException(nameof(info.End));
            }
            else if (info.Start == info.End)
            {
                return false;
            }

            Ball? ball1 = Balls.FirstOrDefault(x => x.Zone == info.Start);
            if (ball1 is null)
            {
                ball1 = AddBall();
                ball1.Zone = info.Start;
            }

            Ball? ball2 = Balls.FirstOrDefault(x => x.Zone == info.End);
            if (ball2 is null)
            {
                ball2 = AddBall();
                ball2.Zone = info.End;
            }

            Connections.Add(new Connection(ball1, ball2, info.ConnectionType, info.EndDate));

            return true;
        }

        public Ball AddBall()
        {
            Ball ball = new Ball(Vector2d.Random * 300, 20);
            Balls.Add(ball);

            return ball;
        }

        public void Clear()
        {
            Balls.Clear();
            Connections.Clear();

            Save();
        }

        public void Update(double deltaTime)
        {
            CheckConnections();

            /*CollidingBalls.Clear();
            MasterCollidingBalls.Clear();

            foreach (Ball ball in Balls)
            {
                ApplyForces(ball);
                ball.Update(deltaTime);
            }

            for (int i = 0; i < Balls.Count; i++)
            {
                for (int j = i + 1; j < Balls.Count; j++)
                {

                    if (Balls[i].Masters.Contains(Balls[j]))
                    {
                        if (Balls[i].IsCollidingWith(Balls[j].Position, Config.Main.LinkLength))
                        {
                            MasterCollidingBalls.Add((Balls[i], Balls[j]));

                            Vector2d fv = Balls[j].Position - Balls[i].Position;

                            double distanceToMove = (fv.Length() - Balls[i].Radius - Config.Main.LinkLength) / 2.0;

                            Balls[i].Move(fv.Normalize() * distanceToMove);
                        }

                    }
                    else if (Balls[j].Masters.Contains(Balls[i]))
                    {
                        if (Balls[j].IsCollidingWith(Balls[i].Position, Config.Main.LinkLength))
                        {
                            MasterCollidingBalls.Add((Balls[j], Balls[i]));

                            Vector2d fv = Balls[i].Position - Balls[j].Position;

                            double distanceToMove = (fv.Length() - Balls[j].Radius - Config.Main.LinkLength) / 2.0;

                            Balls[j].Move(fv.Normalize() * distanceToMove);
                        }

                    }
                    else if (Balls[i].IsCollidingWith(Balls[j]))
                    {
                        CollidingBalls.Add((Balls[i], Balls[j]));

                        Vector2d fv = Balls[j].Position - Balls[i].Position;
                        Vector2d sv = fv * -1;

                        double distanceToMove = (fv.Length() - Balls[i].Radius - Balls[j].Radius) / 2.0;

                        Balls[i].Move(fv.Normalize() * distanceToMove);
                        Balls[j].Move(sv.Normalize() * distanceToMove);
                    }
                }
            }

            foreach ((Ball ball1, Ball ball2) in CollidingBalls)
            {
                Vector2d dislocation = ball2.Position - ball1.Position;

                Vector2d normal = dislocation.Normalize();

                Vector2d k = ball1.Velocity - ball2.Velocity;
                double p = normal.DotProduct(k);

                ball1.Velocity -= normal * p;
                ball2.Velocity += normal * p;
            }

            foreach ((Ball ball1, Ball ball2) in MasterCollidingBalls)
            {
                Vector2d dislocation = ball2.Position - ball1.Position;

                Vector2d normal = dislocation.Normalize();
                Vector2d tangental = new Vector2d(-normal.Y, normal.X);

                double dpTangental1 = ball1.Velocity.DotProduct(tangental);
                double dpTangental2 = ball2.Velocity.DotProduct(tangental);

                ball1.Velocity = tangental * dpTangental1;
                ball2.Velocity = tangental * dpTangental2;
            }*/
        }

        private void CheckConnections()
        {
            bool updateMasters = false;
            if (!connectionsToAdd.IsEmpty)
            {
                while (!connectionsToAdd.IsEmpty)
                {
                    if (connectionsToAdd.TryDequeue(out ConnectionInfo connection))
                    {
                        AddConnection(connection);
                    }
                }

                updateMasters = true;
            }

            foreach (Connection connection in Connections.ToList())
            {
                if (connection.IsDone())
                {
                    Connections.Remove(connection);

                    if (!GetConnectedPoints(connection.Start).Any())
                    {
                        Balls.Remove(connection.Start);
                    }

                    if (!GetConnectedPoints(connection.End).Any())
                    {
                        Balls.Remove(connection.End);
                    }

                    updateMasters = true;
                }
            }

            if (updateMasters)
            {
                UpdateMasters();
            }
        }

        public Ball? GetBall(double x, double y)
        {
            foreach (Ball ball in Balls)
            {
                if (ball.IsInside(x, y))
                {
                    return ball;
                }
            }

            return null;
        }

        public List<Ball> GetConnectedPoints(Ball point)
        {
            return Connections
                .Where(x => x.Start == point)
                .Select(x => x.End)
                .Concat(Connections
                .Where(x => x.End == point)
                .Select(x => x.Start))
                .ToList();
        }

        public void UpdateMasters()
        {
            foreach (Ball ball in Balls)
            {
                ball.Masters = GetMastersBall(ball);
            }
        }

        public bool TryGetMasterBall(Ball ball, out Ball? master)
        {
            master = null;


            List<Ball> balls = GetConnectedPoints(ball);
            foreach (Ball b in balls)
            {
                int otherConnectedAmount = GetConnectedPoints(b).Count;

                if (otherConnectedAmount > balls.Count || (otherConnectedAmount == balls.Count && b.ID < ball.ID))
                {
                    master = b;
                }
            }

            return master != null;
        }

        public HashSet<Ball> GetMastersBall(Ball ball)
        {
            HashSet<Ball> masters = new HashSet<Ball>();

            List<Ball> balls = GetConnectedPoints(ball);
            int mostConnections = balls.Count;

            foreach (Ball b in balls)
            {
                int otherConnections = GetConnectedPoints(b).Count;

                if (otherConnections > mostConnections)
                {
                    mostConnections = otherConnections;
                    masters.Clear();
                    masters.Add(b);
                }
                else if (otherConnections == mostConnections)
                {
                    masters.Add(b);
                }
            }

            return masters;
        }

        public void ApplyForces(Ball ball)
        {
            List<Vector2d> forces = new List<Vector2d>();

            if (ball.Masters.Count == 0)
            {
                forces.Add(GetCenteralForce(ball));
            }

            forces.Add(GetRebelForce(ball));
            forces.Add(GetLinkForce(ball));

            ball.ApplyForces(forces);
        }

        public Vector2d GetCenteralForce(Ball ball)
        {
            return ball.Position.Normalize() * (-Config.Main.CenteralForce);
        }

        public Vector2d GetRebelForce(Ball ball)
        {
            Vector2d force = Vector2d.Zero;

            foreach (Ball other in Balls)
            {
                if (ball == other)
                {
                    continue;
                }

                Vector2d direction = (ball.Position - other.Position).Normalize();
                double strength = 1 / ball.Position.DistanceSquared(other.Position);

                force += direction * strength * Config.Main.RebelForce;
            }

            return force;
        }

        public Vector2d GetLinkForce(Ball ball)
        {
            Vector2d force = Vector2d.Zero;

            foreach (Ball other in GetConnectedPoints(ball))
            {
                if (ball == other)
                {
                    continue;
                }

                Vector2d direction = (other.Position - ball.Position).Normalize();
                double strength = ball.Position.DistanceSquared(other.Position); ;

                force += direction * strength * Config.Main.LinkForce;
            }

            return force;
        }

        public void Save()
        {
            string[] data = new string[Connections.Count];
            for (int i = 0; i < Connections.Count; i++)
            {
                Connection c = Connections[i];
                if (c.Start.Zone == null || c.End.Zone == null)
                    continue;

                data[i] = $"{c.Start.Zone.Name},{c.End.Zone.Name},{(int)c.ConnectionType},{c.EndTime}";
            }

            File.WriteAllLines(SAVE_PATH, data);
        }

        public void Load()
        {
            if (!File.Exists("save.txt"))
            {
                return;
            }

            string[] data = File.ReadAllLines(SAVE_PATH);
            foreach (string line in data)
            {
                string[] lineData = line.Split(',');

                ZoneInfo? start = ZoneGenerator.GetZone(lineData[0]);
                ZoneInfo? end = ZoneGenerator.GetZone(lineData[1]);
                if (start is null || end is null)
                    continue;

                ConnectionType connectionType = (ConnectionType)int.Parse(lineData[2]);
                DateTime endDate = DateTime.Parse(lineData[3]);

                AddConnection(start, end, connectionType, endDate);
            }
        }
    }
}
