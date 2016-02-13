using System.Linq;
using EloBuddy.SDK;

using Settings = Boostana.Config.Modes.LaneClear;

namespace Boostana.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
        }
    }
}
