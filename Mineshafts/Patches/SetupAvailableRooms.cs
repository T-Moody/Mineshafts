using HarmonyLib;
using Mineshafts.Components;
using System;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.SetupAvailableRooms))]
    static class SetupAvailableRooms
    {
        public static void Postfix(DungeonGenerator __instance)
        {
            var parent = __instance.transform.root.gameObject;

            var isMsDungeon = parent.name.StartsWith("MS_D", StringComparison.Ordinal);

            Main.log.LogWarning($"starting room count {DungeonGenerator.m_availableRooms.Count}");

            if (isMsDungeon)
            {
                //DungeonGenerator.m_availableRooms.RemoveAll(room => !room.m_room.gameObject.name.StartsWith("MS_R", StringComparison.Ordinal));

                //remove rooms that do not have matching bruh with dungeon
                var dungeonBruh = parent.GetComponent<Bruh>();
                DungeonGenerator.m_availableRooms.RemoveAll(r =>
                {
                    if (!r.m_room.gameObject.name.StartsWith("MS_R", StringComparison.Ordinal)) return true;

                    var roomBruh = r.m_room.gameObject.GetComponent<Bruh>();

                    if (dungeonBruh == null) { Main.log.LogWarning($"dungeonbruh null for {parent.name}"); return true; }
                    if (roomBruh == null) { Main.log.LogWarning($"roombruh null for {r.m_room.gameObject.name}"); return true; }
                    if (dungeonBruh.bruh != roomBruh.bruh) { Main.log.LogWarning($"{dungeonBruh.bruh} is not equal to {roomBruh.bruh}"); return true; }

                    return false;
                });
            }
            else DungeonGenerator.m_availableRooms.RemoveAll(room => room.m_room.gameObject.name.StartsWith("MS_R", StringComparison.Ordinal));

            Main.log.LogWarning($"ending room count {DungeonGenerator.m_availableRooms.Count}");
        }
    }
}
