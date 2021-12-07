namespace Portaler.NET.Visualiser
{
    public static class Generator
    {
        /*private static Pen pen = new Pen(Color.Black);
        private static Pen redPen = new Pen(Color.Red);
        private static Pen bluePen = new Pen(Color.Blue);

        private static Brush brush = new SolidBrush(Color.Black);
        private static Brush whiteBrush = new SolidBrush(Color.White);

        private static int curWidth;
        private static int curHeight;

        private static Graphics? curGraphics;

        private static readonly Dictionary<string, Brush> _brushes = new Dictionary<string, Brush>
        {
            {"blue"   , new SolidBrush(Color.CornflowerBlue) },
            {"city"   , new SolidBrush(Color.CornflowerBlue) },
            {"red"    , new SolidBrush(Color.PaleVioletRed)  },
            {"yellow" , new SolidBrush(Color.Goldenrod)      },
            {"black"  , new SolidBrush(Color.Black)          },
            {"road"   , new SolidBrush(Color.Turquoise)      },
            {"road-ho", new SolidBrush(Color.RebeccaPurple)  }
        };

        private static readonly Dictionary<ConnectionType, Pen> _pens = new Dictionary<ConnectionType, Pen>
        {
            {ConnectionType.Green, new Pen(Color.Green) },
            {ConnectionType.Blue , new Pen(Color.Blue)  },
            {ConnectionType.Gold , new Pen(Color.Gold)  }
        };

        private static Font font = new Font("Arial", 12);

        public static Image? Generate(Main main, int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return null;
            }

            Bitmap bitmap = new Bitmap(width, height);
            curGraphics = Graphics.FromImage(bitmap);
            curWidth = width;
            curHeight = height;

            //HashSet<Ball> drawnBalls = new HashSet<Ball>();

            foreach (Connection connection in main.Map.Connections)
            {
                DrawLine(_pens[connection.ConnectionType], connection.Start.Position, connection.End.Position);

                float midX = MathF.Round((float)(connection.Start.Position.X + connection.End.Position.X) / 2f);
                float midY = MathF.Round((float)(connection.Start.Position.Y + connection.End.Position.Y) / 2f);
                string timeleft = connection.GetTimeLeft();

                DrawText(whiteBrush, timeleft, new Vector2d(midX, midY));

                //if(!drawnBalls.Contains(connection.Start))
                //{
                //    DrawBall(connection.Start);
                //    drawnBalls.Add(connection.Start);
                //} 

                //if(!drawnBalls.Contains(connection.End))
                //{
                //    DrawBall(connection.End);
                //    drawnBalls.Add(connection.End);
                //}
            }

            foreach (Ball ball in main.Map.Balls)
            {
                DrawBall(ball);
            }

            //if (Config.Main.Debug)
            //{
            //    foreach ((Ball ball1, Ball ball2) in main.Map.CollidingBalls.ToList();)
            //    {
            //        DrawLine(redPen, ball1.Position, ball2.Position);
            //    }

            //    if (main.SelectedBall is not null)
            //    {
            //        DrawLine(bluePen, main.SelectedBall.Position, main.MousePos);
            //    }
            //}

            curGraphics.Dispose();

            return bitmap;
        }


        private static void DrawBall(Ball ball)
        {
            if (ball.Zone is null)
            {
                return;
            }

            Brush b;
            if (_brushes.ContainsKey(ball.Zone.Color))
            {
                b = _brushes[ball.Zone.Color];
            }
            else
            {
                b = brush;
            }

            DrawCircle(b, (float)ball.Radius, ball.Position);

            DrawText(whiteBrush, ball.Zone.Name, new Vector2d(ball.Position.X, ball.Position.Y - (float)ball.Radius - 30));
            DrawText(whiteBrush, ball.Zone.Tier, new Vector2d(ball.Position.X, ball.Position.Y - font.Height / 2));
        }

        private static void DrawLine(Pen pen, Vector2d v1, Vector2d v2)
        {
            if (curGraphics is null)
            {
                return;
            }

            float x1 = (float)(v1.X + curWidth / 2.0);
            float y1 = (float)(v1.Y + curHeight / 2.0);

            float x2 = (float)(v2.X + curWidth / 2.0);
            float y2 = (float)(v2.Y + curHeight / 2.0);
            pen.Width = 3;
            curGraphics.DrawLine(pen, x1, y1, x2, y2);
        }

        private static void DrawText(Brush brush, string s, Vector2d position)
        {
            if (curGraphics is null)
            {
                return;
            }

            float x = (float)(position.X + curWidth / 2.0);
            float y = (float)(position.Y + curHeight / 2.0);

            float textWidth = curGraphics.MeasureString(s, font).Width;
            curGraphics.DrawString(s, font, brush, x - textWidth / 2, y);
        }

        private static void DrawCircle(Brush brush, float radius, Vector2d position)
        {
            if (curGraphics is null)
            {
                return;
            }

            float x = (float)(position.X + curWidth / 2.0);
            float y = (float)(position.Y + curHeight / 2.0);

            curGraphics.FillEllipse(brush, x - radius, y - radius, radius * 2, radius * 2);
        }*/
    }
}
