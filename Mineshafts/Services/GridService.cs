using Mineshafts.Interfaces;
using UnityEngine;

namespace Mineshafts.Services
{
    public class GridService : IGridService
    {
        private readonly int _gridMinHeight = 7000;
        private readonly int _gridMaxHeight = 7500;
        private readonly int _gridSize = 3;

        public int GetGridMinHeight()
        {
            return _gridMinHeight;
        }

        public int GetGridMaxHeight()
        {
            return _gridMaxHeight;
        }

        public int GetGridSize()
        {
            return _gridSize;
        }

        public int GetInitialHeight()
        {
            return RoundToNearestGridPoint((_gridMinHeight + _gridMaxHeight) / 2);
        }

        public int RoundToNearestGridPoint(float sample)
        {
            return (int)Mathf.Round(sample / _gridSize) * _gridSize;
        }

        public Vector3 ConvertVector3ToGridAligned(Vector3 position)
        {
            return new Vector3(RoundToNearestGridPoint(position.x), RoundToNearestGridPoint(position.y), RoundToNearestGridPoint(position.z));
        }

        public bool IsPosWithinGridConstraints(Vector3 position)
        {
            return position.y > _gridMinHeight && position.y < _gridMaxHeight;
        }

        public void AlignTransformToGrid(Transform transform)
        {
            transform.position = ConvertVector3ToGridAligned(transform.position);
        }

    }
}