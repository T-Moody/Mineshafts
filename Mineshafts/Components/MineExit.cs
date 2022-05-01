using UnityEngine;

namespace Mineshafts.Components
{
    public class MineExit : MonoBehaviour
    {
		public void OnEnable()
		{
			var t = transform;

			if (t.position.y < Main.gridMinHeight)
            {
				var pos = t.position;
				pos.y = Util.GetInitialHeight();
				t.position = pos;
				Util.AlignTransformToGrid(t);

				TryPlaceEntryTile();
			}
		}

		public void TryPlaceEntryTile()
		{
			var t = transform;
			var heightToSpawnAt = Util.GetInitialHeight();
			var posToSpawnAt = new Vector3(t.position.x, heightToSpawnAt, t.position.z);
			Util.InstantiateTileOnGrid(posToSpawnAt);
		}
	}
}
