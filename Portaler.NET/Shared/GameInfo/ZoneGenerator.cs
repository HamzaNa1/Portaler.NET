using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Portaler.NET.Shared.GameInfo
{
    public static class ZoneGenerator
    {
        public static ZoneInfo[]? Zones { get; set; }

        public static async Task Initialize()
        {
            Zones = await new HttpClient().GetFromJsonAsync<ZoneInfo[]>("https://raw.githubusercontent.com/HamzaNa1/data-dump/main/zones.json");
        }

        public static ZoneInfo? GetZone(string zoneName)
        {
            return Zones?.FirstOrDefault(x => x.Name == zoneName);
        }
    }
}
