using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using Settings = Boostana.Config.Modes.Misc;

namespace Boostana
{
    internal static class EventsManager
    {

        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.R.Range) && Settings.RGap && Player.Instance.Distance(e.End) < 150)
            {
                SpellManager.R.Cast(sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (e.DangerLevel == DangerLevel.High && Settings.RInt)
            {
                SpellManager.R.Cast(sender);
            }
        }
    }
}
