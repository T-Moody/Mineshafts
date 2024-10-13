using HarmonyLib;
using System;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.SetupAvailableRooms))]
    static class SetupAvailableRooms
    {
        public static void Postfix(DungeonGenerator __instance)
        {
            var isMsDungeon = __instance.transform.root.gameObject.name.StartsWith("MS_D", StringComparison.Ordinal);

            if (isMsDungeon) 
                DungeonGenerator.m_availableRooms.RemoveAll(room => !room.m_loadedRoom.gameObject.name.StartsWith("MS_R", StringComparison.Ordinal));
            else
                DungeonGenerator.m_availableRooms.RemoveAll(room => room.m_loadedRoom.gameObject.name.StartsWith("MS_R", StringComparison.Ordinal));
        }
    }
}
