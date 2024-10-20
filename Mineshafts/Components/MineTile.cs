using Mineshafts.Interfaces;
using Mineshafts.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Mineshafts.Components
{
    public class MineTile : MonoBehaviour
    {
		private readonly IGridService _gridService = ServiceLocator.Get<IGridService>();
		private readonly ITileService _tileService = ServiceLocator.Get<ITileService>();
		private readonly ITileManagerService _tileManagerService = ServiceLocator.Get<ITileManagerService>();
		public ZNetView znview;
		public bool northAdjacent = false;
		public bool westAdjacent = false;
		public bool southAdjacent = false;
		public bool eastAdjacent = false;

		public bool upAdjacent = false;
		public bool downAdjacent = false;

		public bool m_firstFrame = true;

		[Header("mining hitboxes")]
		public MineTileDestructible northMiningHitbox;
		public MineTileDestructible eastMiningHitbox;
		public MineTileDestructible southMiningHitbox;
		public MineTileDestructible westMiningHitbox;
		public MineTileDestructible upMiningHitbox;
		public MineTileDestructible downMiningHitbox;

		[Header("walls")]
		public GameObject northWall;
		public GameObject eastWall;
		public GameObject southWall;
		public GameObject westWall;
		public GameObject ceilingWall;
		public GameObject floorWall;
		public List<GameObject> AllWalls => new List<GameObject>()
		{
			northWall, eastWall, southWall, westWall, ceilingWall, floorWall
		};

		[Header("effects")]
		public List<GameObject> onDestroyEffects = new List<GameObject>();
		public List<GameObject> onHitEffects = new List<GameObject>();

		private bool updatedThisFrame = false;

		public void LateUpdate()
        {
			updatedThisFrame = false;
        }

		public void Awake()
		{
			this.znview = base.GetComponent<ZNetView>();
			
			this.m_firstFrame = false;

			_gridService.AlignTransformToGrid(transform);
			transform.eulerAngles = Vector3.zero;
		}

		public void Start()
		{
			_tileManagerService.RegisterTile(this);
			_tileManagerService.RequestNearUpdate(this);
		}

		public void OnDestroy()
        {
			_tileManagerService.UnregisterTile(this);
        }

		#region[config, veins and biome stuff]

		public Heightmap.Biome GetBiome()
        {
			return WorldGenerator.instance.GetBiome(transform.position);
		}
        #endregion

        #region[adjacency stuff]

		public void UpdateNear()
        {
			var t = transform;
			var nearTiles = _tileService.GetTilesInArea(t.position, 3);
			nearTiles.ForEach(tile => tile.UpdateAdjacency());
        }

        public void UpdateAdjacency()
        {
			if (updatedThisFrame) return;

			updatedThisFrame = true;

			var thisTile = transform;
			var go = gameObject;

			_gridService.AlignTransformToGrid(transform);

			var surroundingTiles = _tileService.GetTilesInArea(transform.position);

			//if a tile exists in the same spot destroy this one
			if (surroundingTiles.Find(s =>
				s.transform.position.x == thisTile.position.x &&
				s.transform.position.y == thisTile.position.y &&
				s.transform.position.z == thisTile.position.y &&
				s != this))
			{
				UnityEngine.Object.Destroy(thisTile);
				return;
			}

			//north - same x, same y, higher z
			northAdjacent = surroundingTiles.Find(surroundingTile => 
				surroundingTile.transform.position.x == thisTile.position.x && 
				surroundingTile.transform.position.z > thisTile.position.z &&
				surroundingTile.transform.position.y == thisTile.position.y) != null;

			//east - higher x, same y, same z
			eastAdjacent = surroundingTiles.Find(surroundingTile =>
				surroundingTile.transform.position.x > thisTile.position.x &&
				surroundingTile.transform.position.z == thisTile.position.z &&
				surroundingTile.transform.position.y == thisTile.position.y) != null;

			//south - same x, same y, lower z
			southAdjacent = surroundingTiles.Find(surroundingTile =>
				surroundingTile.transform.position.x == thisTile.position.x &&
				surroundingTile.transform.position.z < thisTile.position.z &&
				surroundingTile.transform.position.y == thisTile.position.y) != null;

			//west - lower x, same y, same z
			westAdjacent = surroundingTiles.Find(surroundingTile =>
				surroundingTile.transform.position.x < thisTile.position.x &&
				surroundingTile.transform.position.z == thisTile.position.z &&
				surroundingTile.transform.position.y == thisTile.position.y) != null;

			//up - same x, higher y, same z
			upAdjacent = surroundingTiles.Find(surroundingTile =>
				surroundingTile.transform.position.x == thisTile.position.x &&
				surroundingTile.transform.position.z == thisTile.position.z &&
				surroundingTile.transform.position.y > thisTile.position.y) != null;

			//down - same x, lower y, same z
			downAdjacent = surroundingTiles.Find(surroundingTile =>
				surroundingTile.transform.position.x == thisTile.position.x &&
				surroundingTile.transform.position.z == thisTile.position.z &&
				surroundingTile.transform.position.y < thisTile.position.y) != null;

			//set breakable hitboxes active/inactive
			if (northAdjacent) northMiningHitbox.gameObject.SetActive(false); else northMiningHitbox.gameObject.SetActive(true);
			if (eastAdjacent) eastMiningHitbox.gameObject.SetActive(false); else eastMiningHitbox.gameObject.SetActive(true);
			if (southAdjacent) southMiningHitbox.gameObject.SetActive(false); else southMiningHitbox.gameObject.SetActive(true);
			if (westAdjacent) westMiningHitbox.gameObject.SetActive(false); else westMiningHitbox.gameObject.SetActive(true);
			if (upAdjacent) upMiningHitbox.gameObject.SetActive(false); else upMiningHitbox.gameObject.SetActive(true);
			if (downAdjacent) downMiningHitbox.gameObject.SetActive(false); else downMiningHitbox.gameObject.SetActive(true);

			UpdateVisuals();
		}

		public void UpdateVisuals()
        {
			AllWalls.ForEach(wall => wall?.SetActive(true));

			if (northAdjacent) northWall.SetActive(false);
			if (eastAdjacent) eastWall.SetActive(false);
			if (southAdjacent) southWall.SetActive(false);
			if (westAdjacent) westWall.SetActive(false);
			if (upAdjacent) ceilingWall.SetActive(false);
			if (downAdjacent) floorWall.SetActive(false);

			var t = transform;
		}
        #endregion
    }
}
