using HarmonyLib;
using Mineshafts.Interfaces;
using Mineshafts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations))]
    public static class AddLocation
    {
        [HarmonyPostfix]
        public static void Postfix(ZoneSystem __instance)
        {
            var assetService = ServiceLocator.Get<IAssetService>();
            var msLocationsName = "MS_Locations";

            List<GameObject> gos = Resources.FindObjectsOfTypeAll<GameObject>().ToList();
            if (gos.Find(go => string.Equals(go.name, msLocationsName, StringComparison.Ordinal)) == null)
            {
                var bundle = assetService.LoadMineshaftsAssetBundle();
                var msLocations = bundle.LoadAsset<GameObject>(msLocationsName);

                var locationParent = gos.Find(go => string.Equals(go.name, "_Locations", StringComparison.Ordinal)).transform.Find("Meadows");

                var instantiated = UnityEngine.Object.Instantiate(msLocations, locationParent);
                instantiated.FixReferences();
                instantiated.GetComponentInChildren<DungeonGenerator>(true).m_themes = (Room.Theme)Main.roomTheme;

                bundle.Unload(false);
            }

            __instance.m_locations.Add(new ZoneSystem.ZoneLocation()
            {
                m_enable = true,
                m_prefabName = "MS_D_AbandonedMineshaft",
                m_biome = (Heightmap.Biome)Enum.GetValues(typeof(Heightmap.Biome)).Cast<int>().Sum(),
                m_biomeArea = Heightmap.BiomeArea.Everything,
                m_quantity = 50,
                //m_chanceToSpawn = 100,
                m_minDistanceFromSimilar = 512,
                m_randomRotation = false,
                m_maxTerrainDelta = 2,
                m_minAltitude = 10,
                m_maxAltitude = 1000
            });
        }
    }
}
