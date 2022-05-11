using Mineshafts.Components;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mineshafts
{
    public static class Util
    {
        public static AssetBundle LoadBundle(string bundleName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return AssetBundle.LoadFromStream(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources." + bundleName));
        }

        public static float GetRandomNumberForPosition(Vector3 position, float minNumber, float maxNumber)
        {
            var state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(WorldGenerator.instance.GetSeed() + (position.x + position.y + position.z).ToString().GetStableHashCode());
            var number = Random.Range(minNumber, maxNumber);
            UnityEngine.Random.state = state;
            return number;
        }

        public static HitData.DamageModifiers GetPickaxeOnlyDamageMods()
        {
            return new HitData.DamageModifiers()
            {
                m_blunt = HitData.DamageModifier.Immune,
                m_chop = HitData.DamageModifier.Immune,
                m_fire = HitData.DamageModifier.Immune,
                m_frost = HitData.DamageModifier.Immune,
                m_lightning = HitData.DamageModifier.Immune,
                m_pickaxe = HitData.DamageModifier.Normal,
                m_pierce = HitData.DamageModifier.Immune,
                m_poison = HitData.DamageModifier.Immune,
                m_slash = HitData.DamageModifier.Immune,
                m_spirit = HitData.DamageModifier.Immune
            };
        }

        public static int GetInitialHeight()
        {
            return RoundToNearestGridPoint((Main.gridMinHeight + Main.gridMaxHeight) / 2);
        }

        public static GameObject InstantiateTileOnGrid(Vector3 position)
        {
            var alignedPos = ConvertVector3ToGridAligned(position);
            if (!IsPosWithinGridConstraints(alignedPos)) return null;

            if (GetTilesInArea(alignedPos).Find(tile => SameTile(tile.transform.position, position)) != null)
            {
                return null;
            }

            //var bundle = Util.LoadBundle(Main.assetBundleName);
            //var tileGo = bundle.LoadAsset<GameObject>("MS_MineTile");
            var tileGo = ZNetScene.instance.GetPrefab("MS_MineTile");
            //bundle.Unload(false);

            var newTile = InstantiateOnGrid(tileGo, alignedPos);
            
            return newTile;
        }

        public static GameObject InstantiateOnGrid(GameObject go, Vector3 position)
        {
            var alignedPos = ConvertVector3ToGridAligned(position);

            var instantiatedGo = GameObject.Instantiate(go, alignedPos, Quaternion.identity);
            return instantiatedGo;
        }

        public static bool IsPosWithinGridConstraints(Vector3 position)
        {
            return position.y > Main.gridMinHeight && position.y < Main.gridMaxHeight;
        }

        public static Vector3 ConvertVector3ToGridAligned(Vector3 position)
        {
            return new Vector3(RoundToNearestGridPoint(position.x), RoundToNearestGridPoint(position.y), RoundToNearestGridPoint(position.z));
        }

        public static int RoundToNearestGridPoint(float sample)
        {
            return (int)Mathf.Round(sample / Main.gridSize) * Main.gridSize;
        }

        public static void AlignTransformToGrid(Transform t)
        {
            t.position = Util.ConvertVector3ToGridAligned(t.position);
        }

        public static List<MineTile> GetTilesInArea(Vector3 position, int tileReach = 2)
        {
            //~0 means all layers included
            var collidersInArea = Physics.OverlapBox(position, Vector3.one * (Main.gridSize / 2 * tileReach), Quaternion.identity, ~0, QueryTriggerInteraction.Collide).ToList();
            var tilesInArea = new List<MineTile>();

            foreach (Collider col in collidersInArea)
            {
                var objRoot = col.gameObject.transform.root.gameObject;

                var tile = objRoot.GetComponent<MineTile>();
                if (tile == null || tilesInArea.Contains(tile)) continue;

                tilesInArea.Add(tile);
            }

            return tilesInArea;
        }

        public static bool SameTile(Vector3 a, Vector3 b)
        {
            if (RoundToNearestGridPoint(a.x) == RoundToNearestGridPoint(b.x) &&
                RoundToNearestGridPoint(a.z) == RoundToNearestGridPoint(b.z) &&
                RoundToNearestGridPoint(a.y) == RoundToNearestGridPoint(b.y)) return true; else return false;
        }

        public static string NameFromTemp(this GameObject temp)
        {
            return temp.name.Split(new char[] { '_' }, 2)[1];
        }

        public static void FixReferences(this GameObject go)
        {
            var pieces = go.GetComponentsInChildren<Piece>(true);
            foreach(var piece in pieces)
            {
                foreach (EffectList.EffectData list in piece.m_placeEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.name.StartsWith("GJTEMP_", System.StringComparison.Ordinal))
                    {
                        list.m_prefab = ZNetScene.instance.GetPrefab(list.m_prefab.NameFromTemp());
                    }
                }
            }

            var destructibles = go.GetComponentsInChildren<Destructible>(true);
            foreach(var destructible in destructibles)
            {
                foreach (EffectList.EffectData list in destructible.m_hitEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.name.StartsWith("GJTEMP_", System.StringComparison.Ordinal))
                    {
                        list.m_prefab = ZNetScene.instance.GetPrefab(list.m_prefab.NameFromTemp());
                    }
                }

                foreach (EffectList.EffectData list in destructible.m_destroyedEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.name.StartsWith("GJTEMP_", System.StringComparison.Ordinal))
                    {
                        list.m_prefab = ZNetScene.instance.GetPrefab(list.m_prefab.NameFromTemp());
                    }
                }
            }

            var containers = go.GetComponentsInChildren<Container>(true);
            foreach(var container in containers)
            {
                var replacementData = new List<DropTable.DropData>();
                for (int i = 0; i < container.m_defaultItems.m_drops.Count; i++)
                {
                    var drop = container.m_defaultItems.m_drops[i];
                    var dropItem = drop.m_item.name.StartsWith("GJTEMP_", System.StringComparison.Ordinal) ? ZNetScene.instance.GetPrefab(drop.m_item.NameFromTemp()) : drop.m_item;
                    var r = new DropTable.DropData()
                    {
                        m_item = dropItem,
                        m_stackMin = drop.m_stackMin,
                        m_stackMax = drop.m_stackMax,
                        m_weight = drop.m_weight
                    };
                    replacementData.Add(r);
                }
                container.m_defaultItems.m_drops = replacementData;
            }

            var spawnAreas = go.GetComponentsInChildren<SpawnArea>(true);
            foreach(var spawnArea in spawnAreas)
            {
                foreach(var data in spawnArea.m_prefabs)
                {
                    if (data.m_prefab.name.StartsWith("GJTEMP_", System.StringComparison.Ordinal)) data.m_prefab = ZNetScene.instance.GetPrefab(data.m_prefab.NameFromTemp());
                }
            }
        }
    }
}
