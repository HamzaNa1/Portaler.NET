
using System;
using Portaler.NET.Shared.GameInfo;

namespace Portaler.NET.Client.Visualiser
{
    public record struct ConnectionInfo(ZoneInfo Start, ZoneInfo End, ConnectionType ConnectionType, DateTime EndDate);
}
