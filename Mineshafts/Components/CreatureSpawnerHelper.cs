using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineshafts.Components
{
    public class CreatureSpawnerHelper : MonoBehaviour
    {
        public List<PerBiomeSetup> setups = new List<PerBiomeSetup>();

        public void Start()
        {
            ResolveReferences();
        }

        public void ResolveReferences()
        {
            if (ZNetScene.instance == null) return;
            var spawner = GetComponent<CreatureSpawner>();
            if (spawner == null) return;
            if (WorldGenerator.instance == null)
            {
                spawner.m_creaturePrefab = ZNetScene.instance.GetPrefab(setups.First().creature);
                return;
            }

            var t = transform;

            //apply creature from current biome
            var currentBiomeSetup = setups.Find(s => s.biome == WorldGenerator.instance.GetBiome(t.position));
            if (currentBiomeSetup != null)
            {
                spawner.m_creaturePrefab = ZNetScene.instance.GetPrefab(currentBiomeSetup.creature);
                return;
            }

            //apply global setup for creature in case current biome has no setup
            var globalBiomeSetup = setups.Find(s => s.biome == Heightmap.Biome.BiomesMax);
            spawner.m_creaturePrefab = ZNetScene.instance.GetPrefab(globalBiomeSetup.creature);

            return;
        }

        [System.Serializable]
        public class PerBiomeSetup
        {
            public Heightmap.Biome biome = Heightmap.Biome.BiomesMax;
            public string creature;
        }
    }
}
