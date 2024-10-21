using System;
using UnityEngine;
using Mineshafts.Configuration;
using System.Collections.Generic;
using System.Linq;
using Mineshafts.Interfaces;
using Mineshafts.Services;

namespace Mineshafts.Components
{
    public class MineTileDestructible : MonoBehaviour, IDestructible
    {
		private readonly IGridService _gridService = ServiceLocator.Get<IGridService>();
		private readonly IDamageService _damageService = ServiceLocator.Get<IDamageService>();
		private readonly ITileManagerService _tileManagerService = ServiceLocator.Get<ITileManagerService>();

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

			if(t.position.y + _gridService.GetGridSize() > _gridService.GetGridMaxHeight() || t.position.y - _gridService.GetGridSize() < _gridService.GetGridMinHeight())
            {
				DamageText.instance.ShowText(DamageText.TextType.Immune, hit.m_point, 0f, false);
				return;
			}

			hit.ApplyResistance(_damageService.GetPickaxeOnlyDamageMods(), out var type);
			float totalDamage = hit.GetTotalDamage();

			var tileInFrontPos = parentTile.transform.position + t.forward * _gridService.GetGridSize();

			if (hit.m_toolTier < ModConfig.General.min_pickaxe_tier)
			{
				DamageText.instance.ShowText(DamageText.TextType.TooHard, hit.m_point, 0f, false);
				return;
			}

			DamageText.instance.ShowText(type, hit.m_point, totalDamage, false);

			if (totalDamage <= 0f) return;

			//clamp for when a config lowers the health than what was saved previously
			var currentHealth = Mathf.Clamp(zdo.GetFloat(HealthZdoString, ModConfig.General.stone_health), 0, ModConfig.General.stone_health);
			currentHealth -= totalDamage;

			parentTile.onHitEffects.ForEach(effect => GameObject.Instantiate(effect, t.position, Quaternion.identity));

			if (currentHealth <= 0f)
			{
				_tileManagerService.RequestPlacement(tileInFrontPos);

				parentTile.onDestroyEffects.ForEach(effect => GameObject.Instantiate(effect, tileInFrontPos, Quaternion.identity));

				//drops here
				var dropConfigs = ModConfig.GetDropsForBiome(WorldGenerator.instance.GetBiome(t.position).ToString());
				dropConfigs.RemoveAll(c => c.min_pickaxe_tier > hit.m_toolTier); //remove drops that require a higher tier pickaxe
				var dropTables = dropConfigs.Select(d => d.ToDropTable()).ToList(); //convert configs to tables
				var drops = new List<GameObject>();
				dropTables.ForEach(t => drops.AddRange(t.GetDropList())); //roll drops and add them to drops list
				drops.ForEach(d => Instantiate(d, t.position, Quaternion.identity)); //spawn drops

				// removed in some update, don't care
				// zdo.ReleaseFloats(); // release wall healths from memory
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
