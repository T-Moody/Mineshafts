using Mineshafts.Interfaces;
using UnityEngine;

namespace Mineshafts.Services
{
    public class RandomService : IRandomService
    {
        private readonly IGridService _gridService;

        public RandomService(IGridService gridService)
        {
            _gridService = gridService;
        }

        public float GetRandomNumberForPosition(Vector3 position, float minNumber, float maxNumber)
        {
            Random.State previousState = Random.state;

            int seed = WorldGenerator.instance.GetSeed() + $"{position.x}{position.y - _gridService.GetInitialHeight()}{position.z}".GetStableHashCode();
            
            Random.InitState(seed);

            float randomNumber = Random.Range(minNumber, maxNumber);

            Random.state = previousState;

            return randomNumber;
        }
    }
}