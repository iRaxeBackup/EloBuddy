using EloBuddy;
using EloBuddy.SDK;

namespace Owlsticks
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
                damage += Program.Q.GetRealDamage(target);
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
                damage += Program.R.GetRealDamage(target) + OwlsticksMenu.ComboR1();
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
                    // 0/0/0/0
                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel];
                    break;

                case SpellSlot.W:
                    // 60/90/120/150/180 (+0.45*AP)
                    damage = new float[] { 60, 90, 120, 150, 180 }[spellLevel] + 0.45f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // 65/85/105/125/145 (+0.45*AP) 
                    damage = new float[] { 65, 85, 105, 125, 145 }[spellLevel] + 0.45f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    // 125/225/325 (+0.45*AP) 
                    damage = new float[] { 125, 225, 325 }[spellLevel] + 0.45f * Program.Player.TotalMagicalDamage;
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
