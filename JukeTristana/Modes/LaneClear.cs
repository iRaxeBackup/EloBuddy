using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = Boostana.Config.Modes.LaneClear;

namespace Boostana.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            var tawah = EntityManager.Turrets.Enemies.FirstOrDefault
                (t => !t.IsDead && t.IsInRange(Player.Instance, E.Range));

            var minionE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .FirstOrDefault(m => m.IsValidTarget(Player.Instance.AttackRange)
                    && m.GetBuffCount("tristanaecharge") > 0);

            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions
                (EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition,
                 Player.Instance.AttackRange, false).Count();

            if (minion == null && count == 0) return;

            if (E.IsReady() && Settings.UseE && minion.IsValidTarget(E.Range) &&
                Player.Instance.ManaPercent >= Settings.UseQWEmana)
            {
                E.Cast(minion);
                Orbwalker.ForcedTarget = minionE;
            }
            if (Q.IsReady() && Settings.UseQ && minion.IsValidTarget(Q.Range) &&
                Player.Instance.ManaPercent >= Settings.UseQWEmana)
            {
                Q.Cast();
            }
            if (W.IsReady() && Settings.UseW && Settings.UseWminions <= count &&
                Player.Instance.ManaPercent >= Settings.UseWminions)
            {
                if (minion != null) W.Cast(minion.Position);
            }

            if (tawah == null) return;
            if (Settings.UseEtower && tawah.IsInRange(Player.Instance, E.Range) && E.IsReady() &&
                Player.Instance.ManaPercent >= Settings.UseQWEmana)
            {
                E.Cast(tawah);
                Q.Cast();
            }

            if (Settings.UseQ && tawah.IsInRange(Player.Instance, Q.Range) && Q.IsReady() &&
                Player.Instance.ManaPercent >= Settings.UseQWEmana)
            {
                Q.Cast();
            }
        }
    }
}
