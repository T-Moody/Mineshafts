using Mineshafts.Components;
using System.Collections.Generic;

namespace Mineshafts
{
    public static class TileManager
    {
        private static List<MineTile> activeTiles = new List<MineTile>();

        private static Queue<MineTile> singleUpdateQueue = new Queue<MineTile>();
        private static Queue<MineTile> updateNearQueue = new Queue<MineTile>();

        public static int maximumBatchSize = 10;

        public static void RequestUpdateAll()
        {
            //if (activeCoroutine != null) Main.instance.StopCoroutine(activeCoroutine);
            //activeCoroutine =  Main.instance.StartCoroutine(UpdateAllTiles());

            singleUpdateQueue.Clear();
            activeTiles.ForEach(t => singleUpdateQueue.Enqueue(t));
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
                var batchSize = updateNearQueue.Count > maximumBatchSize ? maximumBatchSize : updateNearQueue.Count;
                for (int i = 0; i < batchSize; i++)
                {
                    var tile = updateNearQueue.Dequeue();
                    if (!tile || tile == null) continue;
                    tile.UpdateNear();
                }
            }

            if (singleUpdateQueue.Count > 0)
            {
                var batchSize = singleUpdateQueue.Count > maximumBatchSize ? maximumBatchSize : singleUpdateQueue.Count;
                for (int i = 0; i < batchSize; i++)
                {
                    var tile = singleUpdateQueue.Dequeue();
                    if (!tile || tile == null) continue;
                    tile.UpdateAdjacency();
                }
            }
        }
    }
}