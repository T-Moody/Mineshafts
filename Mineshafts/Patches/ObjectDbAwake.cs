using HarmonyLib;
using Mineshafts.Configuration;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
    public static class ObjectDbAwake
    {
        public static void Postfix(ObjectDB __instance)
        {
            var bundle = Util.LoadBundle(Main.assetBundleName);

            if(ZNetScene.instance != null)
            {
                var zns = ZNetScene.instance;

                var prefabList = new List<string>()
                {
                    "MS_MineTile",
                    "MS_MineTile_Brick",
                    "MS_Entrance",

                    "MS_FX_Tile_Destroyed",
                    "MS_FX_Tile_Hit",

                    "MS_chest_T1",
                    "MS_chest_T2",

                    "MS_bonepile",

                    "MS_woodstairs",
                    "MS_woodstairs_damaged",
                    "MS_woodwall_2x1",
                    "MS_woodwall_2x1_damaged",
                    "MS_woodwall_2x2",
                    "MS_woodwall_2x2_damaged",
                    "MS_woodwall_2x4",
                    "MS_woodwall_2x4_damaged"
                };

                var hammerPrefabList = new List<string>()
                {
                    "MS_Entrance"
                };

                var hammer = zns.GetPrefab("Hammer");
                var hammerPieces = hammer.GetComponent<ItemDrop>().m_itemData.m_shared.m_buildPieces.m_pieces;

                foreach(string prefabName in prefabList)
                {
                    var prefab = bundle.GetPrefab(prefabName);
                    prefab.FixReferences();
                    zns.AddPrefab(prefab);

                    if (hammerPrefabList.Contains(prefabName)) hammerPieces.AddPiece(prefab);
                }

                ModConfig.PieceRecipes.ForEach(r => r.Apply());
            }
            bundle.Unload(false);

            ModConfig.Localization.InsertLocalization();
        }

        private static GameObject GetPrefab(this AssetBundle bundle, string prefabName)
        {
            return bundle.LoadAsset<GameObject>(prefabName);
        }

        private static void AddItem(this ObjectDB db, GameObject item)
        {
            if(!db.m_items.Exists(i => string.Equals(i.name, item.name, System.StringComparison.Ordinal))) db.m_items.Add(item);
            if(!db.m_itemByHash.ContainsKey(item.name.GetStableHashCode())) db.m_itemByHash.Add(item.name.GetStableHashCode(), item);
        }

        private static void AddPrefab(this ZNetScene zns, GameObject prefab)
        {
            if(!zns.m_prefabs.Exists(p => string.Equals(p.name, prefab.name, System.StringComparison.Ordinal))) zns.m_prefabs.Add(prefab);
            //if (zns.m_prefabs.Find(_prefab => string.Equals(_prefab.name, prefab.name, System.StringComparison.Ordinal)) == null) zns.m_prefabs.Add(prefab);
            if(!zns.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode())) zns.m_namedPrefabs.Add(prefab.name.GetStableHashCode(), prefab);
        }

        private static void AddPiece(this List<GameObject> hammerPieces, GameObject piece)
        {
            //hammerPieces.Remove(hammerPieces.Find(p => string.Equals(p.name, piece.name, System.StringComparison.Ordinal)));
            if (!hammerPieces.Exists(_piece => string.Equals(_piece.name, piece.name, System.StringComparison.Ordinal))) hammerPieces.Add(piece);
        }
    }
}