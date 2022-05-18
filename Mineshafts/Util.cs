using Mineshafts.Components;
using System.Collections.Generic;
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
            var prevState = UnityEngine.Random.state;
            UnityEngine.Random.InitState(WorldGenerator.instance.GetSeed() + $"{position.x}{position.y-Util.GetInitialHeight()}{position.z}".GetStableHashCode());
            var num = Random.Range(minNumber, maxNumber);
            UnityEngine.Random.state = prevState; 

            return num;
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
            var tileGo = ZNetScene.instance.GetPrefab("MS_MineTile");

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
    }
}
