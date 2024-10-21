// Interfaces/ITileManagerService.cs
using Mineshafts.Components;
using UnityEngine;

namespace Mineshafts.Interfaces
{
    public interface ITileManagerService
    {
        void RequestUpdateAll();
        void RequestPlacement(Vector3 tileToPlace);
        void RequestNearUpdate(MineTile tileToUpdate);
        void RegisterTile(MineTile tile);
        void UnregisterTile(MineTile tile);
        void UpdateRequests();
    }
}
