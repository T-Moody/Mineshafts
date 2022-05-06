using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations))]
    public static class AddLocationsPatch
    {
        public static void Prefix(ZoneSystem __instance)
        {
            var MS_Locations = "MS_Locations";

            List<GameObject> gos = Resources.FindObjectsOfTypeAll<GameObject>().ToList();
            if (gos.Find(go => string.Equals(go.name, MS_Locations, StringComparison.Ordinal)) == null)
            {
                var bundle = Util.LoadBundle(Main.assetBundleName);
                var mineshaftsLocations = bundle.LoadAsset<GameObject>("MS_Locations");

                var valheimLocationsParent = gos.Find(go => string.Equals(go.name, "_Locations", StringComparison.Ordinal)).transform;

                UnityEngine.Object.Instantiate(mineshaftsLocations, valheimLocationsParent);

                bundle.Unload(false);
            }

            __instance.m_locations.Add(new ZoneSystem.ZoneLocation()
            {
                m_enable = true,
                m_prefabName = "MS_AbandonedMine_Shack",
                m_biome = Heightmap.Biome.BiomesMax,
                m_biomeArea = Heightmap.BiomeArea.Everything,
                m_quantity = 200,
                m_chanceToSpawn = 100,
                m_minDistanceFromSimilar = 512,
                m_randomRotation = true,
                m_maxTerrainDelta = 2,
                m_minAltitude = 1,
                m_maxAltitude = 1000
            });
        }
    }
}
