using HarmonyLib;
using System.Collections.Generic;
/*
namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonDB), nameof(DungeonDB.SetupRooms))]
    static class SetRoomTheme
    {
        private static void Postfix(DungeonDB __instance, List<DungeonDB.RoomData> __result)
        {

            return;
            foreach(DungeonDB.RoomData roomData in __result)
            {
                
                if (Utils.GetPrefabName(roomData.m_room.gameObject).StartsWith("MS_R", System.StringComparison.Ordinal))
                {
                    roomData.m_room.m_theme = (Room.Theme)Main.roomTheme;
                    Main.log.LogWarning(roomData.m_room.name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.Awake))]
    static class SetDungeonTheme
    {
        private static void Prefix(DungeonGenerator __instance)
        {
            return;
            var go = __instance.transform.root.gameObject;
            if(go == null || go.name.StartsWith("MS_D"))
            {
                __instance.m_themes = (Room.Theme)Main.roomTheme;
            }
        }
    }
}*/