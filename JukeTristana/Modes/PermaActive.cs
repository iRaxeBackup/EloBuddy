using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Boostana.Config.Modes.Combo;
using Alias = Boostana.Config.Modes.Misc;

namespace Boostana.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = EntityManager.Heroes.Enemies.FirstOrDefault(
                x =>
                    x.HasBuffOfType(BuffType.Charm) || x.HasBuffOfType(BuffType.Knockup) ||
                    x.HasBuffOfType(BuffType.Stun) || x.HasBuffOfType(BuffType.Suppression) ||
                    x.HasBuffOfType(BuffType.Snare));

            if (target == null || target.IsZombie || !target.HasBuff("tristanaecharge")) return;
            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseEcc)
            {
                E.Cast(target);
            }
            if (Alias.Rks && R.IsReady() &&
                target.Health + target.AttackShield < Player.Instance.GetSpellDamage(target, SpellSlot.R))

            {
                R.Cast(target);
            }

            var tawah =
                EntityManager.Turrets.Enemies.FirstOrDefault(
                    a =>
                        !a.IsDead && a.Distance(target) <= 775 + Player.Instance.BoundingRadius +
                        (target.BoundingRadius / 2) + 44.2);

            if (Alias.Wks && W.IsReady() &&
                target.Health + target.AttackShield <
                Player.Instance.GetSpellDamage(target, SpellSlot.W) &&
                target.Position.CountEnemiesInRange(800) == 1 && tawah == null && Player.Instance.Mana >= 120)
            {
                W.Cast(target.ServerPosition);
            }
        }
    }
}