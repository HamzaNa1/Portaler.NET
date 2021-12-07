namespace Portaler.NET.Shared.GameInfo
{
    public class ZoneInfo
    {
        public int Id { get; set; } = 0;
        public string AlbionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Tier { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsDeep { get; set; } = false;
    }
}