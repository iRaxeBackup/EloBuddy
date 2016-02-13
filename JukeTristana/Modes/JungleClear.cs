using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = Boostana.Config.Modes.LaneClear;

namespace Boostana.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jgminion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(Q.Range));

            if (jgminion == null) return;

            if (Q.IsReady() && Settings.UseQ && jgminion.Distance(Player.Instance) <= Q.Range)
            {
                Q.Cast();
            }
            if (W.IsReady() && Settings.UseW && jgminion.Distance(Player.Instance) <= W.Range)
            {
                W.Cast(jgminion.ServerPosition);
            }
            if (E.IsReady() && Settings.UseE && jgminion.Distance(Player.Instance) <= E.Range)
            {
                E.Cast(jgminion);
            }
        }
    }
}
