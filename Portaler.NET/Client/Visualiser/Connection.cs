using System;

namespace Portaler.NET.Client.Visualiser
{
    public struct Connection
    {
        public Ball Start { get; set; }
        public Ball End { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public DateTime EndTime { get; set; }


        public Connection(Ball start, Ball end, ConnectionType connectionType, DateTime endDate)
        {
            Start = start;
            End = end;
            ConnectionType = connectionType;
            EndTime = endDate;
        }

        public string GetTimeLeft()
        {
            TimeSpan timeLeft = (EndTime - DateTime.UtcNow) + TimeSpan.FromMinutes(1);
            return $"{timeLeft.Hours}h {timeLeft.Minutes}m";
        }

        public bool IsDone()
        {
            return EndTime < DateTime.UtcNow;
        }
    }
}
