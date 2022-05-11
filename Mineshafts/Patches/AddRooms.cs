using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(DungeonDB), nameof(DungeonDB.SetupRooms))]
    public static class AddRooms
    {
        private static void Prefix(DungeonDB __instance)
        {
            var MS_Rooms = "MS_Rooms";

            var rooms = new List<string>()
            {
                "MS_R_start",
                "MS_R_cap",
                "MS_R_room_1",
                "MS_R_room_2",
                "MS_R_room_3",
                "MS_R_tunnel_1",
                "MS_R_excavation_1",
                "MS_R_excavation_2",
                "MS_R_stairwell_1",
                "MS_R_stairwell_2"
            };

            List<GameObject> gos = Resources.FindObjectsOfTypeAll<GameObject>().ToList();
            if (gos.Find(go => string.Equals(go.name, MS_Rooms, StringComparison.Ordinal)) == null)
            {
                var bundle = Util.LoadBundle(Main.assetBundleName);
                var mineshaftRooms = bundle.LoadAsset<GameObject>(MS_Rooms);

                var valheimRoomsParent = gos.Find(go => string.Equals(go.name, "_Rooms", StringComparison.Ordinal)).transform;

                foreach(string roomName in rooms)
                {
                    if(valheimRoomsParent.Find(roomName) == null)
                    {
                        var loadedRoom = bundle.LoadAsset<GameObject>(roomName);
                        var instantiatedRoom = UnityEngine.GameObject.Instantiate(loadedRoom, valheimRoomsParent);
                        instantiatedRoom.FixReferences();
                        instantiatedRoom.name = Utils.GetPrefabName(instantiatedRoom);
                        instantiatedRoom.GetComponent<Room>().m_theme = (Room.Theme)Main.roomTheme;
                    }
                }

                /*var bruh = UnityEngine.Object.Instantiate(mineshaftRooms, valheimRoomsParent);
                var bruhTransform = bruh.transform;

                Main.log.LogWarning(bruhTransform.childCount);

                for (int i = 0; i < 4; i++)
                {
                    Main.log.LogWarning(i);
                    var child = bruhTransform.GetChild(i);
                    Main.log.LogWarning(child);

                    child.parent = valheimRoomsParent;
                    //child.GetComponent<Room>().m_theme = (Room.Theme)Main.roomTheme;
                    child.gameObject.name = Utils.GetPrefabName(child.gameObject);

                    
                }

                UnityEngine.Object.DestroyImmediate(bruh);*/

                bundle.Unload(false);
            }
        }
    }
}
