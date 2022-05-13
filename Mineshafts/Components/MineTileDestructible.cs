using System;
using UnityEngine;
using Mineshafts.Configuration;

namespace Mineshafts.Components
{
    public class MineTileDestructible : MonoBehaviour, IDestructible
    {
		public string zdoName = string.Empty;
		public float health = ModConfig.general.wall_health;

		public MineTile parentTile;

		private string DamageRpcString => "damage_" + zdoName;

		public void Awake()
		{
			if (parentTile.znview && parentTile.znview.GetZDO() != null)
			{
				parentTile.znview.Register(DamageRpcString, new Action<long, HitData>(RPC_Damage));
			}
			health = ModConfig.general.wall_health;
		}

		public void Damage(HitData hit)
		{
			if (parentTile.m_firstFrame)
			{
				return;
			}
			if (!parentTile.znview.IsValid())
			{
				return;
			}
			parentTile.znview.InvokeRPC(DamageRpcString, new object[]
			{
				hit
			});
		}

		private void RPC_Damage(long sender, HitData hit)
		{
			if (!parentTile.znview.IsValid() || !parentTile.znview.IsOwner())
			{
				return;
			}
			var t = transform;

			if(t.position.y + Main.gridSize > Main.gridMaxHeight || t.position.y - Main.gridSize < Main.gridMinHeight)
            {
				DamageText.instance.ShowText(DamageText.TextType.Immune, hit.m_point, 0f, false);
				return;
			}

			hit.ApplyResistance(Util.GetPickaxeOnlyDamageMods(), out var type);
			float totalDamage = hit.GetTotalDamage();

			if (hit.m_toolTier < ModConfig.general.min_pickaxe_tier)
			{
				DamageText.instance.ShowText(DamageText.TextType.TooHard, hit.m_point, 0f, false);
				return;
			}

			DamageText.instance.ShowText(type, hit.m_point, totalDamage, false);

			if (totalDamage <= 0f)
			{
				return;
			}

			health -= totalDamage;

			parentTile.onHitEffects.ForEach(effect => GameObject.Instantiate(effect, t.position, Quaternion.identity));

			if (health <= 0f)
			{
				health = ModConfig.general.wall_health;

				var posToSpawnOn = parentTile.transform.position + t.forward * Main.gridSize;
				TileManager.RequestPlacement(posToSpawnOn);

				parentTile.onDestroyEffects.ForEach(effect => GameObject.Instantiate(effect, posToSpawnOn, Quaternion.identity));

				var defaultDrop = ZNetScene.instance.GetPrefab(ModConfig.general.default_drop);
				if (defaultDrop != null)
				{
					var dropAmount = UnityEngine.Random.Range(ModConfig.general.default_drop_min, ModConfig.general.default_drop_max);
					if (dropAmount > 0)
					{
						var spawnedDrop = Util.InstantiateOnGrid(defaultDrop, posToSpawnOn);
						spawnedDrop.GetComponent<ItemDrop>()?.SetStack(dropAmount);
					}
				}

				var vein = ModConfig.GetVeinConfigForPosition(posToSpawnOn);
				if (vein != null)
				{
					var veinDrop = ZNetScene.instance.GetPrefab(vein.drop);
					if (veinDrop != null)
					{
						var dropAmount = UnityEngine.Random.Range(vein.drop_min, vein.drop_max);
						if (dropAmount > 0)
						{
							var spawnedDrop = Util.InstantiateOnGrid(veinDrop, posToSpawnOn);
							spawnedDrop.GetComponent<ItemDrop>()?.SetStack(dropAmount);
						}
					}
				}
			}
		}

		public DestructibleType GetDestructibleType()
		{
			return DestructibleType.Default;
		}
	}
}
