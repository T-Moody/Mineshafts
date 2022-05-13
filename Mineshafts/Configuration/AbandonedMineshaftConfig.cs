using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineshafts.Configuration
{
    public class AbandonedMineshaftConfig
    {
        public int spawn_chance { get; set; } = 100;
        public int quantity { get; set; } = 50;
        public int min_spacing { get; set; } = 512;
        public int rooms { get; set; } = 30;

        public void Apply()
        {
            if (ZoneSystem.instance == null) return;
            var location = ZoneSystem.instance.m_locations.Find(loc => loc.m_prefabName == "MS_D_AbandonedMineshaft");
            if (location == null) return;
            location.m_chanceToSpawn = spawn_chance;
            location.m_quantity = quantity;
            location.m_minDistanceFromSimilar = min_spacing;
            //rooms are set in DungeonGenerate patch
        }
    }
}
