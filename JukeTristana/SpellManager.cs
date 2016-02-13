using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Boostana
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 550);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 450, int.MaxValue, 180);
            E = new Spell.Targeted(SpellSlot.E, 550);
            R = new Spell.Targeted(SpellSlot.R, 550);
        }

        public static void Initialize()
        {
            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (!sender.IsMe) return;

            var level = (uint)Player.Instance.Level;

            Q = new Spell.Active(SpellSlot.Q, 543 + 7 * level);
            E = new Spell.Targeted(SpellSlot.E, 543 + 7 * level);
            R = new Spell.Targeted(SpellSlot.R, 543 + 7 * level);
        }
    }
}
