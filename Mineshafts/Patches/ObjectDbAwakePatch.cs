using HarmonyLib;
using Mineshafts.Configuration;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
    public static class ObjectDbAwakePatch
    {
        public static void Postfix(ObjectDB __instance)
        {
            var bundle = Util.LoadBundle(Main.assetBundleName);

            if(ZNetScene.instance != null)
            {
                var zns = ZNetScene.instance;

                var mineTile = bundle.GetPrefab("MS_MineTile");
                var entrance = bundle.GetPrefab("MS_Entrance");

                var hitEffect = bundle.GetPrefab("MS_FX_Tile_Hit");
                var destroyedEffect = bundle.GetPrefab("MS_FX_Tile_Destroyed");

                

                zns.AddPrefab(mineTile);
                zns.AddPrefab(entrance);
                zns.AddPrefab(hitEffect);
                zns.AddPrefab(destroyedEffect);

                var hammer = zns.GetPrefab("Hammer");
                var hammerPieces = hammer.GetComponent<ItemDrop>().m_itemData.m_shared.m_buildPieces.m_pieces;
                if(hammerPieces.Find(piece => piece.name == entrance.name) == null) hammerPieces.Add(entrance);

                ModConfig.pieceRecipes.ForEach(r => r.ApplyConfig());
            }

            bundle.Unload(false);

            ModConfig.localization.InsertLocalization();
        }

        private static GameObject GetPrefab(this AssetBundle bundle, string prefabName)
        {
            return bundle.LoadAsset<GameObject>(prefabName);
        }

        private static void AddItem(this ObjectDB db, GameObject item)
        {
            if(db.m_items.Find(_item => string.Equals(_item.name, item.name, System.StringComparison.Ordinal)) == null) db.m_items.Add(item);
            if(!db.m_itemByHash.ContainsKey(item.name.GetStableHashCode())) db.m_itemByHash.Add(item.name.GetStableHashCode(), item);
        }

        private static void AddPrefab(this ZNetScene zns, GameObject prefab)
        {
            if(zns.m_prefabs.Find(_prefab => string.Equals(_prefab.name, prefab.name, System.StringComparison.Ordinal))) zns.m_prefabs.Add(prefab);
            if(!zns.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode())) zns.m_namedPrefabs.Add(prefab.name.GetStableHashCode(), prefab);
        }
    }
}
