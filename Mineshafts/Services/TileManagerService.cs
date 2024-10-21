// Services/TileManagerService.cs
using Mineshafts.Components;
using Mineshafts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Services
{
    public class TileManagerService : ITileManagerService
    {
        private readonly ITileService _tileService;
        private readonly List<MineTile> _activeTiles = new List<MineTile>();
        private readonly Queue<MineTile> _singleUpdateQueue = new Queue<MineTile>();
        private readonly Queue<MineTile> _updateNearQueue = new Queue<MineTile>();
        private readonly Queue<Vector3> _placementQueue = new Queue<Vector3>();

        public int MaximumBatchSize { get; set; } = 10;

        public TileManagerService(ITileService tileService)
        {
            _tileService = tileService;
        }

        public void RequestUpdateAll()
        {
            _singleUpdateQueue.Clear();
            _activeTiles.ForEach(t => _singleUpdateQueue.Enqueue(t));
        }

        public void RequestPlacement(Vector3 tileToPlace)
        {
            _placementQueue.Enqueue(tileToPlace);
        }

        public void RequestNearUpdate(MineTile tileToUpdate)
        {
            _updateNearQueue.Enqueue(tileToUpdate);
        }

        public void RegisterTile(MineTile tile)
        {
            if (!_activeTiles.Contains(tile))
                _activeTiles.Add(tile);
        }

        public void UnregisterTile(MineTile tile)
        {
            _activeTiles.Remove(tile);
        }

        public void UpdateRequests()
        {
            if (_updateNearQueue.Count > 0)
            {
                MineTile tile = _updateNearQueue.Dequeue();
                tile?.UpdateNear();
            }

            if (_singleUpdateQueue.Count > 0)
            {
                MineTile tile = _singleUpdateQueue.Dequeue();
                tile?.UpdateAdjacency();
            }

            if (_placementQueue.Count > 0)
            {
                Vector3 position = _placementQueue.Dequeue();
                _tileService.InstantiateTileOnGrid(position);
            }
        }
    }
}
