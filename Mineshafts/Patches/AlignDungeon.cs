using HarmonyLib;
using System;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.Generate))]
    [HarmonyPatch(new Type[] { typeof(ZoneSystem.SpawnMode) })]
    [HarmonyPatch(new Type[] { typeof(int), typeof(ZoneSystem.SpawnMode) })]
    static class AlignDungeon
    {
        private static void Prefix(DungeonGenerator __instance)
        {
            if (__instance.gameObject.name.StartsWith("MS_D", StringComparison.Ordinal))
            {
                var t = __instance.transform;
                Util.AlignTransformToGrid(t);
                t.position = new Vector3(t.position.x, Util.GetInitialHeight(), t.position.z);
            }
        }
    }
}
