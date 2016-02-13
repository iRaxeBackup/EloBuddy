using EloBuddy;
using EloBuddy.SDK;
using Settings = Boostana.Config.Modes.Misc;

namespace Boostana.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (W.IsReady() && Settings.UseWFlee)
            {
                W.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, W.Range).To3D()); //Surely not stole from MarioGK 
            }
            if (R.IsReady() && Settings.UseRFlee)
            {
                R.Cast(target);
            }
        }
    }
}
