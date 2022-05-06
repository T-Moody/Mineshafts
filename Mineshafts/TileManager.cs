using Mineshafts.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts
{
    public static class TileManager
    {
        private static List<MineTile> activeTiles = new List<MineTile>();

        private static Queue<MineTile> singleUpdateQueue = new Queue<MineTile>();
        private static Queue<MineTile> updateNearQueue = new Queue<MineTile>();
        private static Queue<Vector3> placementQueue = new Queue<Vector3>();

        public static int maximumBatchSize = 10;

        public static void RequestUpdateAll()
        {
            singleUpdateQueue.Clear();
            activeTiles.ForEach(t => singleUpdateQueue.Enqueue(t));
        }

        public static void RequestPlacement(Vector3 tileToPlace)
        {
            placementQueue.Enqueue(tileToPlace);
        }

        public static void RequestNearUpdate(MineTile tileToUpdate)
        {
            updateNearQueue.Enqueue(tileToUpdate);
        }

        public static void RegisterTile(MineTile tile)
        {
            if(!activeTiles.Contains(tile))
                activeTiles.Add(tile);
        }

        public static void UnegisterTile(MineTile tile)
        {
            activeTiles.Remove(tile);
        }

        public static void UpdateRequests()
        {
            if (updateNearQueue.Count > 0)
            {
                var tile = updateNearQueue.Dequeue();
                if (tile || tile != null) tile.UpdateNear();
            }

            if (singleUpdateQueue.Count > 0)
            {
                var tile = singleUpdateQueue.Dequeue();
                if (tile || tile != null) tile.UpdateAdjacency();
            }

            if (placementQueue.Count > 0)
            {
                var tile = placementQueue.Dequeue();
                Util.InstantiateTileOnGrid(tile);
            }
        }
    }
}