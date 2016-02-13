using EloBuddy;
using EloBuddy.SDK;

using Settings = Boostana.Config.Modes.Harass;

namespace Boostana.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && Player.Instance.ManaPercent >= Settings.UseQemana)
            {
                E.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.ManaPercent >= Settings.UseQemana)
            {
                Q.Cast();
            }
        }
    }
}
