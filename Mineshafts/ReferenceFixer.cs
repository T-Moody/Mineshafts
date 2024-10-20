using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts
{
    public static class ReferenceFixer
    {
        private static readonly string tempPrefix = "GJTEMP_";

        private static string NameFromTemp(this GameObject temp)
        {
            return temp.name.Split(new char[] { '_' }, 2)[1];
        }

        private static bool IsTemp(this GameObject go)
        {
            return go.name.StartsWith(tempPrefix, StringComparison.Ordinal);
        }

        private static GameObject GetReplacement(this GameObject temp)
        {
            return ZNetScene.instance.GetPrefab(temp.NameFromTemp());
        }

        public static void FixReferences(this GameObject go)
        {
            var pieces = go.GetComponentsInChildren<Piece>(true);
            foreach (var piece in pieces)
            {
                foreach (EffectList.EffectData list in piece.m_placeEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.IsTemp())
                    {
                        list.m_prefab = list.m_prefab.GetReplacement();
                    }
                }
            }

            var destructibles = go.GetComponentsInChildren<Destructible>(true);
            foreach (var destructible in destructibles)
            {
                foreach (EffectList.EffectData list in destructible.m_hitEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.IsTemp())
                    {
                        list.m_prefab = list.m_prefab.GetReplacement();
                    }
                }

                foreach (EffectList.EffectData list in destructible.m_destroyedEffect.m_effectPrefabs)
                {
                    if (list.m_prefab != null && list.m_prefab.IsTemp())
                    {
                        list.m_prefab = list.m_prefab.GetReplacement();
                    }
                }

                if (destructible.m_spawnWhenDestroyed != null && destructible.m_spawnWhenDestroyed.IsTemp())
                {
                    destructible.m_spawnWhenDestroyed = destructible.m_spawnWhenDestroyed.GetReplacement();
                }
            }

            var containers = go.GetComponentsInChildren<Container>(true);
            foreach (var container in containers)
            {
                var replacementData = new List<DropTable.DropData>();
                for (int i = 0; i < container.m_defaultItems.m_drops.Count; i++)
                {
                    var drop = container.m_defaultItems.m_drops[i];
                    var dropItem = drop.m_item.IsTemp() ? drop.m_item.GetReplacement() : drop.m_item;
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
            foreach (var spawnArea in spawnAreas)
            {
                foreach (var data in spawnArea.m_prefabs)
                {
                    if (data.m_prefab.IsTemp()) data.m_prefab = data.m_prefab.GetReplacement();
                }
            }
        }
    }
}
