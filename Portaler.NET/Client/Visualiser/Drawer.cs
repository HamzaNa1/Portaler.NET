using System;
using Blazor.Extensions.Canvas.Canvas2D;
using Portaler.NET.Shared;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Portaler.NET.Shared.GameInfo;

namespace Portaler.NET.Client.Visualiser
{
    public static class Drawer
    {
        public static ElementReference? HomeImage { get; set; }
        
        private static Canvas2DContext? _currentContext;
        private static double _currentWidth;
        private static double _currentHeight;

        public static async Task DrawMap(Map map, Ball? selectedBall, Canvas2DContext context, double width, double height)
        {
            _currentContext = context;
            _currentWidth = width;
            _currentHeight = height;

            foreach (Connection connection in map.Connections)
            {
                await DrawLine(connection.Start.Position, connection.End.Position, GetColorFromConnection(connection));

                double midX = Math.Round((connection.Start.Position.X + connection.End.Position.X) / 2.0);
                double midY = Math.Round((connection.Start.Position.Y + connection.End.Position.Y) / 2.0);
                string timeLeft = connection.GetTimeLeft();

                await DrawText(new Vector2d(midX, midY), timeLeft);
            }

            foreach(Ball ball in map.Balls)
            {
                await DrawBall(ball);

                if (ball == selectedBall)
                {
                    await DrawCircle(ball);
                }
            }
        }

        private static async Task DrawLine(Vector2d v1, Vector2d v2, string color)
        {
            if (_currentContext is null)
            {
                return;
            }

            double x1 = v1.X + _currentWidth / 2;
            double y1 = v1.Y + _currentHeight / 2;
            double x2 = v2.X + _currentWidth / 2;
            double y2 = v2.Y + _currentHeight / 2;

            await _currentContext.BeginPathAsync();
            await _currentContext.MoveToAsync(x1, y1);
            await _currentContext.LineToAsync(x2, y2);
            await _currentContext.SetStrokeStyleAsync(color);
            await _currentContext.StrokeAsync();
        }

        private static async Task DrawBall(Ball ball)
        {
            if (_currentContext is null || ball.Zone is null)
            {
                return;
            }

            double x = ball.Position.X + _currentWidth / 2;
            double y = ball.Position.Y + _currentHeight / 2;

            await _currentContext.BeginPathAsync();
            if (ball.Zone.Name == "Setent-Qintis" && HomeImage is not null)
            {
                await DrawImage(ball.Position, HomeImage.Value);
            }
            else
            {
                await _currentContext.ArcAsync(x, y, ball.Radius, 0, Math.PI * 2);
                await _currentContext.SetFillStyleAsync(GetColorFromZone(ball.Zone));
                await _currentContext.FillAsync();
                
                await DrawText(ball.Position, ball.Zone.Tier);
            }

            await DrawText(new Vector2d(ball.Position.X, ball.Position.Y + ball.Radius + 15), ball.Zone.Name);
        }

        private static async Task DrawCircle(Ball ball)
        {
            if(_currentContext is null)
            {
                return;
            }

            double x = ball.Position.X + _currentWidth / 2;
            double y = ball.Position.Y + _currentHeight / 2;

            await _currentContext.BeginPathAsync();
            await _currentContext.ArcAsync(x, y, ball.Radius, 0, Math.PI * 2);

            await _currentContext.SetStrokeStyleAsync("black");
            await _currentContext.StrokeAsync();
        }

        private static async Task DrawText(Vector2d position, string text)
        {
            if (_currentContext is null)
            {
                return;
            }

            double x = position.X + _currentWidth / 2;
            double y = position.Y + _currentHeight / 2;

            await _currentContext.BeginPathAsync();

            await _currentContext.SetFontAsync("12px Arial");
            await _currentContext.SetTextAlignAsync(TextAlign.Center);

            await _currentContext.SetFillStyleAsync("white");
            await _currentContext.FillTextAsync(text, x, y + 6);
        }
        
        private static async Task DrawImage(Vector2d position, ElementReference image)
        {
            if (_currentContext is null)
            {
                return;
            }

            double x = position.X + _currentWidth / 2;
            double y = position.Y + _currentHeight / 2;

            await _currentContext.BeginPathAsync();
            await _currentContext.DrawImageAsync(image, x -20, y - 20, 40, 40);
        }

        private static string GetColorFromZone(ZoneInfo zone)
        {
            return zone.Color switch
            {
                "blue" or "city" => "rgb(100, 149, 237)",
                "red" => "rgb(219, 112, 147)",
                "yellow" => "rgb(218, 165, 32)",
                "black" => "black",
                "road" => "rgb(64, 224, 208)",
                "road-ho" => "rgb(102, 51, 153)",
                _ => string.Empty
            };
        }

        private static string GetColorFromConnection(Connection connection)
        {
            return connection.ConnectionType switch
            {
                ConnectionType.Green => "rgb(0, 128, 0)",
                ConnectionType.Blue => "rgb(0, 0, 255)",
                ConnectionType.Gold => "rgb(255, 215, 0)",
                _ => string.Empty
            };
        }
    }
}
