using UnityEngine;

namespace Mineshafts.Interfaces
{
    public interface IGridService
    {
        int GetGridMinHeight();
        int GetGridMaxHeight();
        int GetGridSize();
        int GetInitialHeight(); 
        int RoundToNearestGridPoint(float sample);
        Vector3 ConvertVector3ToGridAligned(Vector3 position);
        bool IsPosWithinGridConstraints(Vector3 position);
        void AlignTransformToGrid(Transform t);
    }
}