using EloBuddy;
using EloBuddy.SDK;

namespace Fappadred
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Program.Player.GetAutoAttackDamage(target);

            // Q
            if (Program.Q.IsReady())
            {
                damage += Program.Q.GetRealDamage(target) + FappadredMenu.ComboQ1();
            }

            // W
            if (Program.W.IsReady())
            {
                damage += Program.W.GetRealDamage(target);
            }

            // E
            if (Program.E.IsReady())
            {
                damage += Program.E.GetRealDamage(target);
            }

            // R
            if (Program.R.IsReady())
            {
                damage += Program.R.GetRealDamage(target);
            }

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Program.Player.Spellbook.GetSpell(slot).Level;
            const DamageType damageType = DamageType.Magical;
            float damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    // 60/90/120/150/180 
                    damage = new float[] { 60, 90, 120, 150, 180 }[spellLevel];
                    break;

                case SpellSlot.W:
                    // 20/30/35/40/45
                    damage = new float[] { 20, 30, 35, 40, 45 }[spellLevel];
                    break;

                case SpellSlot.E:
                    // 80/110/140/170/200
                    damage = new float[] { 80, 110, 140, 170, 200 }[spellLevel];
                    break;

                case SpellSlot.R:
                    // 0/0/0
                    damage = new float[] { 0, 0, 0 }[spellLevel];
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Program.Player.CalculateDamageOnUnit(target, damageType, damage) - 10;
        }
    }
}