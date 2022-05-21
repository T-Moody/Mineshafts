using System;
using UnityEngine;
using Mineshafts.Configuration;

namespace Mineshafts.Components
{
    public class MineTileDestructible : MonoBehaviour, IDestructible
    {
		public string zdoName = string.Empty;

		public MineTile parentTile;

		private string DamageRpcString => "damage_" + zdoName;
		private string HealthZdoString => "health_" + zdoName;

		public void Awake()
		{
			if (parentTile.znview && parentTile.znview.GetZDO() != null)
			{
				parentTile.znview.Register(DamageRpcString, new Action<long, HitData>(RPC_Damage));
			}
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
			if (!parentTile.znview.IsValid() || !parentTile.znview.IsOwner()) return;

			var zdo = parentTile.znview.GetZDO();
			var t = transform;

			if(t.position.y + Main.gridSize > Main.gridMaxHeight || t.position.y - Main.gridSize < Main.gridMinHeight)
            {
				DamageText.instance.ShowText(DamageText.TextType.Immune, hit.m_point, 0f, false);
				return;
			}

			hit.ApplyResistance(Util.GetPickaxeOnlyDamageMods(), out var type);
			float totalDamage = hit.GetTotalDamage();

			var tileInFrontPos = parentTile.transform.position + t.forward * Main.gridSize;
			var veinInFront = ModConfig.GetVeinConfigForPosition(tileInFrontPos);

			if (hit.m_toolTier < ModConfig.general.min_pickaxe_tier || (veinInFront != null && veinInFront.min_pickaxe_tier > hit.m_toolTier))
			{
				DamageText.instance.ShowText(DamageText.TextType.TooHard, hit.m_point, 0f, false);
				return;
			}

			DamageText.instance.ShowText(type, hit.m_point, totalDamage, false);

			if (totalDamage <= 0f) return;

			//clamp for when a config lowers the health than what was saved previously
			var currentHealth = Mathf.Clamp(zdo.GetFloat(HealthZdoString, ModConfig.general.wall_health), 0, ModConfig.general.wall_health);
			currentHealth -= totalDamage;

			parentTile.onHitEffects.ForEach(effect => GameObject.Instantiate(effect, t.position, Quaternion.identity));

			if (currentHealth <= 0f)
			{
				
				TileManager.RequestPlacement(tileInFrontPos);

				parentTile.onDestroyEffects.ForEach(effect => GameObject.Instantiate(effect, tileInFrontPos, Quaternion.identity));

				var defaultDrop = ZNetScene.instance.GetPrefab(ModConfig.general.default_drop);
				if (defaultDrop != null)
				{
					var dropAmount = UnityEngine.Random.Range(ModConfig.general.default_drop_min, ModConfig.general.default_drop_max);
					if (dropAmount > 0)
					{
						var spawnedDrop = Util.InstantiateOnGrid(defaultDrop, tileInFrontPos);
						spawnedDrop.GetComponent<ItemDrop>()?.SetStack(dropAmount);
					}
				}

				if (veinInFront != null)
				{
					var veinDrop = ZNetScene.instance.GetPrefab(veinInFront.drop);
					if (veinDrop != null)
					{
						var dropAmount = UnityEngine.Random.Range(veinInFront.drop_min, veinInFront.drop_max);
						if (dropAmount > 0)
						{
							var spawnedDrop = Util.InstantiateOnGrid(veinDrop, tileInFrontPos);
							spawnedDrop.GetComponent<ItemDrop>()?.SetStack(dropAmount);
						}
					}
				}

				zdo.ReleaseFloats();//release wall healths from memory
			}
			else
            {
				zdo.Set(HealthZdoString, currentHealth);
			}
		}

		public DestructibleType GetDestructibleType()
		{
			return DestructibleType.Default;
		}
	}
}
