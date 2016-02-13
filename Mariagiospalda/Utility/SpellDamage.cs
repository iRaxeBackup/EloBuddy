using EloBuddy;
using EloBuddy.SDK;

namespace Mariagiospalda
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
                damage += Program.Q.GetRealDamage(target) + MariagiospaldaMenu.ComboQ1();
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
                damage += Program.R.GetRealDamage(target) + MariagiospaldaMenu.ComboR1();
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
                    // 70/100/130/160/190 (+0.65*AP)
                    damage = new float[] { 70, 100, 130, 160, 190 }[spellLevel] + 0.65f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.W:
                    // 70/110/150/190/230 (+0.4*AP) 
                    damage = new float[] { 70, 110, 150, 190, 230 }[spellLevel] + 0.4f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // 70/115/160/205/250 (+0.6*AP)
                    damage = new float[] { 70, 115, 160, 205, 250 }[spellLevel] + 0.6f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    // 150/250/350 (+0.7*AP)
                    damage = new float[] { 150, 250, 350 }[spellLevel] + 0.7f * Program.Player.TotalMagicalDamage;
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