using UnityEngine;

namespace Mineshafts.Interfaces
{
    public interface IRandomService
    {
        float GetRandomNumberForPosition(Vector3 position, float minNumber, float maxNumber);
    }
}