using Mineshafts.Interfaces;
using Mineshafts.Services;
using UnityEngine;

namespace Mineshafts.Components
{
    public class MineExit : MonoBehaviour
    {
		public bool alwaysAlign = false;
		public bool spawnEntryTile = false;
		public ZNetView znv;
		private readonly IGridService _gridService = ServiceLocator.Get<IGridService>();
		private readonly IRandomService _randomService = ServiceLocator.Get<IRandomService>();
		private readonly ITileManagerService _tileManagerService = ServiceLocator.Get<ITileManagerService>();

		private void Start()
		{
			if (transform.position.y < _gridService.GetGridMinHeight() || alwaysAlign)
			{
				Align();
				if(spawnEntryTile && znv != null && znv.IsValid() && znv.IsOwner())	TryPlaceEntryTile();
			}
		}

		public void Align()
        {
			var t = transform;

			var pos = t.position;
			pos.y = _gridService.GetInitialHeight();
			t.position = pos;
            _gridService.AlignTransformToGrid(t);
		}

		public void TryPlaceEntryTile()
		{
			var t = transform;
			var heightToSpawnAt = _gridService.GetInitialHeight();
			var posToSpawnAt = new Vector3(t.position.x, heightToSpawnAt, t.position.z);
            _tileManagerService.RequestPlacement(posToSpawnAt);
		}
	}
}
