using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations))]
    public static class AddLocation
    {
        public static void PostFix(ZoneSystem __instance)
        {
            if (__instance == null)
            {
                Debug.LogError("ZoneSystem instance is null");
                return;
            }

            var msLocationsName = "MS_Locations";

            List<GameObject> gos = Resources.FindObjectsOfTypeAll<GameObject>().ToList();
            if (gos == null)
            {
                Debug.LogError("GameObject list is null");
                return;
            }

            if (gos.Find(go => string.Equals(go.name, msLocationsName, StringComparison.Ordinal)) == null)
            {
                var bundle = Util.LoadBundle(Main.assetBundleName);
                if (bundle == null)
                {
                    Debug.LogError("Asset bundle is null");
                    return;
                }

                var msLocations = bundle.LoadAsset<GameObject>(msLocationsName);
                if (msLocations == null)
                {
                    Debug.LogError("MS_Locations asset is null");
                    return;
                }

                var locationParent = gos.Find(go => string.Equals(go.name, "_Locations", StringComparison.Ordinal))?.transform.Find("Meadows");
                if (locationParent == null)
                {
                    Debug.LogError("Location parent is null");
                    return;
                }

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
                m_minDistanceFromSimilar = 512,
                m_randomRotation = false,
                m_maxTerrainDelta = 2,
                m_minAltitude = 10,
                m_maxAltitude = 1000
            });
        }
    }
}
