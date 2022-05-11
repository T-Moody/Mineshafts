/*
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SpawnAreaHelper : MonoBehaviour
{
    [SerializeField]
    public List<PerBiomeSpawnAreaSetup> setups = new List<PerBiomeSpawnAreaSetup>();

    void Start()
    {
        ResolveReferences();
    }

    public void ResolveReferences()
    {
        if (ZNetScene.instance == null) return;
        var spawner = GetComponent<SpawnArea>();
        if (spawner == null) return;
        if (WorldGenerator.instance == null)
        {
            spawner.m_prefabs.Clear();
            spawner.m_prefabs.Add(setups.First().ToData());
        }

        Main.log.LogWarning(setups.Count);
        Main.log.LogWarning(WorldGenerator.instance.GetBiome(transform.position));

        spawner.m_prefabs.Clear();
        var currentBiomeSetups = setups.FindAll(s => s.biome == WorldGenerator.instance.GetBiome(transform.position));
        if(currentBiomeSetups.Any())
        {
            spawner.m_prefabs.AddRange(currentBiomeSetups.Select(s => s.ToData()));
            Main.log.LogWarning("area helper setting biome preset");
            return;
        }

        var globalBiomeSetups = setups.FindAll(s => s.biome == Heightmap.Biome.BiomesMax);
        if(globalBiomeSetups.Any())
        {
            spawner.m_prefabs.AddRange(globalBiomeSetups.Select(s => s.ToData()));
            Main.log.LogWarning("area helper setting global preset");
            return;
        }

        Main.log.LogWarning("bwuh");
    }

    [System.Serializable]
    public struct PerBiomeSpawnAreaSetup
    {
        public Heightmap.Biome biome;
        public string creature;
        public int weight;
        public int minLevel;
        public int maxLevel;

        public SpawnArea.SpawnData ToData()
        {
            return new SpawnArea.SpawnData()
            {
                m_prefab = ZNetScene.instance.GetPrefab(creature),
                m_weight = weight,
                m_minLevel = minLevel,
                m_maxLevel = maxLevel
            };
        }
    }

}
}*/
