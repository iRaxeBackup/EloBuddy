using EloBuddy;
using EloBuddy.SDK;

namespace AAtron
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
            const DamageType damageType = DamageType.Physical;
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
                    // 70/115/160/205/250 (+0.6*Bonus AD)
                    damage = new float[] {70, 115, 160, 205, 250}[spellLevel] +
                             0.6f*Program.Player.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.W:
                    // 60/90/120/150/180 
                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // 75/110/145/180/215 (+0.6*AP) (+0.6*Bonus AD)
                    damage = new float[] { 75, 110, 145, 180, 215}[spellLevel] + 0.6f * Program.Player.TotalMagicalDamage + 0.6f * Program.Player.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.R:
                    // 200/300/400 (+1*AP)
                    damage = new float[] { 200, 300, 400 }[spellLevel] + 1.0f * Program.Player.TotalMagicalDamage;
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
