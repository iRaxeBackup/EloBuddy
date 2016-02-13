using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = Boostana.Config.Modes.Combo;

namespace Boostana.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var targetBoom =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.HasBuff("tristanaecharge") && a.Distance(Player.Instance) < Player.Instance.AttackRange);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW && !target.IsInvulnerable && target.Position.CountEnemiesInRange(800) <= Settings.UseWe)
            {
                if (W.GetPrediction(target).HitChance >= HitChance.High)
                {
                    W.Cast(target);
                }

            }

            if (targetBoom != null)
                if (Settings.UseEr && !E.IsReady() && R.IsReady() && targetBoom.IsValidTarget(R.Range) && !targetBoom.IsInvulnerable &&
                    (targetBoom.Health + targetBoom.AllShield + Settings.UseErdmg) -
                    (Player.Instance.GetSpellDamage(targetBoom, SpellSlot.E) +
                     (targetBoom.Buffs.Find(a => a.Name == "tristanaecharge").Count *
                      Player.Instance.GetSpellDamage(targetBoom, SpellSlot.E, DamageLibrary.SpellStages.Detonation))) <
                    Player.Instance.GetSpellDamage(targetBoom, SpellSlot.R))
                {
                    R.Cast(targetBoom);
                }

            if (R.IsReady() && Settings.UseR)
            {
                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable &&
                target.Health + target.AttackShield + Settings.UseRdmg <
                Player.Instance.GetSpellDamage(target, SpellSlot.R))
                {
                    R.Cast(target);
                }
            }
        }
    }
}
