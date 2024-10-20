using Mineshafts.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Interfaces
{
    public interface ITileService
    {
        GameObject InstantiateTileOnGrid(Vector3 position);
        GameObject InstantiateOnGrid(GameObject go, Vector3 position);
        List<MineTile> GetTilesInArea(Vector3 position, int tileReach = 2);
        bool SameTile(Vector3 a, Vector3 b);
    }
}