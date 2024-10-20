using Mineshafts.Interfaces;

namespace Mineshafts.Services
{
    public class DamageService : IDamageService
    {
        public HitData.DamageModifiers GetPickaxeOnlyDamageMods()
        {
            return new HitData.DamageModifiers()
            {
                m_blunt = HitData.DamageModifier.Immune,
                m_chop = HitData.DamageModifier.Immune,
                m_fire = HitData.DamageModifier.Immune,
                m_frost = HitData.DamageModifier.Immune,
                m_lightning = HitData.DamageModifier.Immune,
                m_pickaxe = HitData.DamageModifier.Normal,
                m_pierce = HitData.DamageModifier.Immune,
                m_poison = HitData.DamageModifier.Immune,
                m_slash = HitData.DamageModifier.Immune,
                m_spirit = HitData.DamageModifier.Immune
            };
        }
    }
}