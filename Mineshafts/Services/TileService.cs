using Mineshafts.Components;
using Mineshafts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineshafts.Services
{
    public class TileService : ITileService
    {
        private readonly IGridService _gridService;

        public TileService(IGridService gridService)
        {
            _gridService = gridService;
        }

        public GameObject InstantiateTileOnGrid(Vector3 position)
        {
            var alignedPos = _gridService.ConvertVector3ToGridAligned(position);
            if (!_gridService.IsPosWithinGridConstraints(alignedPos)) return null;

            if (GetTilesInArea(alignedPos).Find(tile => SameTile(tile.transform.position, position)) != null)
            {
                return null;
            }
            var tileGo = ZNetScene.instance.GetPrefab("MS_MineTile");

            var newTile = InstantiateOnGrid(tileGo, alignedPos);

            return newTile;
        }

        public GameObject InstantiateOnGrid(GameObject go, Vector3 position)
        {
            var alignedPos = _gridService.ConvertVector3ToGridAligned(position);

            var instantiatedGo = GameObject.Instantiate(go, alignedPos, Quaternion.identity);
            return instantiatedGo;
        }

        public List<MineTile> GetTilesInArea(Vector3 position, int tileReach = 2)
        {
            var collidersInArea = Physics.OverlapBox(position, Vector3.one * (_gridService.GetGridSize() / 2 * tileReach), Quaternion.identity, ~0, QueryTriggerInteraction.Collide).ToList();
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

        public bool SameTile(Vector3 a, Vector3 b)
        {
            if (_gridService.RoundToNearestGridPoint(a.x) == _gridService.RoundToNearestGridPoint(b.x) &&
                _gridService.RoundToNearestGridPoint(a.z) == _gridService.RoundToNearestGridPoint(b.z) &&
                _gridService.RoundToNearestGridPoint(a.y) == _gridService.RoundToNearestGridPoint(b.y)) return true;
            else return false;
        }
    }
}