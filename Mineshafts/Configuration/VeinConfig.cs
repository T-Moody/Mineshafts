using System.Collections.Generic;

namespace Mineshafts.Configuration
{
    public class VeinConfig
    {
        public List<string> biomes { get; set; } = new List<string>();

        public int weight { get; set; } = 0;

        public int min_pickaxe_tier { get; set; } = 0;

        public string color { get; set; } = "#ffffff";
        public string emission_color { get; set; } = "#000000";
        public bool metallic { get; set; } = false;
        public int shine { get; set; } = 0;

        public string drop { get; set; } = string.Empty;
        public int drop_min { get; set; } = 0;
        public int drop_max { get; set; } = 0;
    }
}