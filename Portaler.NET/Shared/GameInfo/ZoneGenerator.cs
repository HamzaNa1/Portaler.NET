using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Portaler.NET.Shared.GameInfo
{
    public static class ZoneGenerator
    {
        public static ZoneInfo[]? Zones { get; private set; }

        public static async Task Initialize()
        {
            List<ZoneInfo>? zones = await new HttpClient().GetFromJsonAsync<List<ZoneInfo>>("https://raw.githubusercontent.com/HamzaNa1/data-dump/main/zones.json");
            if (zones is not null)
            {
                zones.RemoveAll(x => x.AlbionId.Contains("---"));
                Zones = zones.ToArray();
            }
        }

        public static ZoneInfo? GetZone(string zoneName)
        {
            return Zones?.FirstOrDefault(x => x.Name == zoneName);
        }
    }
}
