using System.Collections.Generic;
using UnityEngine;
using Mineshafts.Configuration;
using System.Linq;

namespace Mineshafts.Components
{
    public class MineTile : MonoBehaviour
    {
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

		[Header("veins")]
		public GameObject northVein;
		public GameObject eastVein;
		public GameObject southVein;
		public GameObject westVein;
		public GameObject ceilingVein;
		public GameObject floorVein;
		public List<GameObject> AllVeins => new List<GameObject>()
		{
			northVein, eastVein, southVein, westVein, ceilingVein, floorVein
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

			Util.AlignTransformToGrid(transform);
			transform.eulerAngles = Vector3.zero;
		}

		public void Start()
		{
			TileManager.RegisterTile(this);
			TileManager.RequestNearUpdate(this);
		}

		public void OnDestroy()
        {
			TileManager.UnegisterTile(this);
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
			var nearTiles = Util.GetTilesInArea(t.position, 3);
			nearTiles.ForEach(tile => tile.UpdateAdjacency());
        }

        public void UpdateAdjacency()
        {
			if (updatedThisFrame) return;

			updatedThisFrame = true;

			var thisTile = transform;
			var go = gameObject;

			Util.AlignTransformToGrid(transform);

			var surroundingTiles = Util.GetTilesInArea(transform.position);

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

			var northernVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.forward * Main.gridSize);
			SetupVein(northVein, northernVeinCfg);

			var easternVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.right * Main.gridSize);
			SetupVein(eastVein, easternVeinCfg);

			var southernVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.back * Main.gridSize);
			SetupVein(southVein, southernVeinCfg);

			var westernVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.left * Main.gridSize);
			SetupVein(westVein, westernVeinCfg);

			var ceilingVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.up * Main.gridSize);
			SetupVein(ceilingVein, ceilingVeinCfg);

			var floorVeinCfg = ModConfig.GetVeinConfigForPosition(t.position + Vector3.down * Main.gridSize);
			SetupVein(floorVein, floorVeinCfg);
		}

		public void SetupVein(GameObject vein, VeinConfig config)
        {
			if (vein == null) return;
			if (config == null)
            {
				vein.SetActive(false);
				return;
            }

			vein.SetActive(true);
			var renderers = vein.GetComponentsInChildren<MeshRenderer>(true).ToList();
			foreach(MeshRenderer r in renderers)
            {
				var mats = r.materials;
				foreach(Material mat in mats)
                {
					if (config.metallic)
                    {
						mat.color = Color.white;
						if (ColorUtility.TryParseHtmlString(config.color, out var metalColor)) mat.SetColor("_MetalColor", metalColor);
						mat.SetFloat("_Metallic", 1);
						mat.SetFloat("_Glossiness", 0);
						mat.SetFloat("_MetallicAlphaGloss", config.shine / 100f);
					}
                    else
                    {
						if (ColorUtility.TryParseHtmlString(config.color, out var matColor)) mat.color = matColor;
						mat.SetFloat("_Metallic", 0);
						mat.SetFloat("_MetallicAlphaGloss", 0);
						mat.SetFloat("_Glossiness", config.shine / 100f);
						mat.SetFloat("_GlossMapScale", 1);
					}

					if (ColorUtility.TryParseHtmlString(config.emission_color, out var emissionColor)) mat.SetColor("_EmissionColor", emissionColor);
				}
            }
        }
        #endregion
    }
}
