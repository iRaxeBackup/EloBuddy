using EloBuddy;
using EloBuddy.SDK;

namespace Fapturne
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
                damage += Program.Q.GetRealDamage(target) + FapturneMenu.ComboQ1();
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
                damage += Program.R.GetRealDamage(target) + FapturneMenu.ComboR1();
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
                    // 60/105/150/195/240 (+0.75*Bonus AD) physical damage
                    damage = new float[] { 60, 105, 150, 195, 240 }[spellLevel] + 0.75f * Program.Player.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.W:
                    // 
                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // 80/125/170/215/260 (+1*AP) 
                    damage = new float[] { 80, 125, 170, 215, 260 }[spellLevel] + 1.0f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    // 150/250/350 (+1.2*Bonus AD)
                    damage = new float[] { 150, 250, 350 }[spellLevel] + 1.2f * Program.Player.FlatPhysicalDamageMod;
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