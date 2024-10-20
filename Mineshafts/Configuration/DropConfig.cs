using System.Collections.Generic;

namespace Mineshafts.Configuration
{
    public class DropConfig
    {
        public List<string> biomes { get; set; } = new List<string>();
        public int min_pickaxe_tier { get; set; } = 0;
        public string prefab { get; set; } = string.Empty;
        public int chance { get; set; } = 0;
        public int min { get; set; } = 0;
        public int max { get; set; } = 0;

        public DropTable ToDropTable()
        {
            DropTable table = new DropTable()
            {
                m_dropChance = chance / 100f,
                m_dropMin = 1,
                m_dropMax = 1,
                m_oneOfEach = false,
                m_drops = new List<DropTable.DropData>()
                {
                    new DropTable.DropData()
                    {
                        m_item = ZNetScene.instance.GetPrefab(prefab),
                        m_stackMin = min,
                        m_stackMax = max,
                        m_weight = 100
                    }
                }
            };

            return table;
        }
    }
}
