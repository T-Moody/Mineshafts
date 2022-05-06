using UnityEngine;

namespace Mineshafts.Components
{
    public class MineExit : MonoBehaviour
    {
		private void OnEnable()
		{
			if (transform.position.y < Main.gridMinHeight)
			{
				Align();
				TryPlaceEntryTile();
			}
		}

		public void Align()
        {
			var t = transform;

			var pos = t.position;
			pos.y = Util.GetInitialHeight();
			t.position = pos;
			Util.AlignTransformToGrid(t);
		}

		public void TryPlaceEntryTile()
		{
			var t = transform;
			var heightToSpawnAt = Util.GetInitialHeight();
			var posToSpawnAt = new Vector3(t.position.x, heightToSpawnAt, t.position.z);
			TileManager.RequestPlacement(posToSpawnAt);
		}
	}
}
