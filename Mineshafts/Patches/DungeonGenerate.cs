﻿using HarmonyLib;
using Mineshafts.Configuration;
using Mineshafts.Interfaces;
using Mineshafts.Services;
using System;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.Generate))]
    [HarmonyPatch(new Type[] { typeof(ZoneSystem.SpawnMode) })]
    [HarmonyPatch(new Type[] { typeof(int), typeof(ZoneSystem.SpawnMode) })]
    static class DungeonGenerate
    {
        private static void Prefix(DungeonGenerator __instance)
        {
            if (__instance.gameObject.name.StartsWith("MS_D", StringComparison.Ordinal))
            {
                var gridService = ServiceLocator.Get<IGridService>();

                var t = __instance.transform;
                gridService.AlignTransformToGrid(t);
                t.position = new Vector3(t.position.x, gridService.GetInitialHeight(), t.position.z);

                __instance.m_minRooms = ModConfig.AbandonedMineshaft.rooms;
                __instance.m_maxRooms = ModConfig.AbandonedMineshaft.rooms;
            }
        }
    }
}
