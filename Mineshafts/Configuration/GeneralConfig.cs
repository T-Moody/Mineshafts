namespace Mineshafts.Configuration
{
    public class GeneralConfig
    {
        public int wall_health { get; set; } = 0;
        public int min_pickaxe_tier { get; set; } = 0;
        public int vein_chance { get; set; } = 0;
        public string default_drop { get; set; } = string.Empty;
        public int default_drop_min { get; set; } = 0;
        public int default_drop_max { get; set; } = 0;
    }
}
