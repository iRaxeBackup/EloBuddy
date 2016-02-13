using EloBuddy;
using EloBuddy.SDK;


namespace MissInopportune
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
                    // 20/35/50/65/80 (+0.85*AD) (+0.35*AP)
                    damage = new float[] { 20, 35, 50, 65, 80 }[spellLevel] + 0.85f * Program.Player.TotalAttackDamage + 0.35f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.W:
                    // 
                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // 90/145/200/255/310 (+0.8*AP) 
                    damage = new float[] { 90, 145, 200, 255, 310 }[spellLevel] + 0.8f * Program.Player.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    // 288/336/384% AD
                    damage = new float[] { (0.35f * Program.Player.FlatPhysicalDamageMod + 0.2f * Program.Player.TotalMagicalDamage)*12  ,
                        (0.35f * Program.Player.FlatPhysicalDamageMod + 0.2f * Program.Player.TotalMagicalDamage) * 14,
                        (0.35f * Program.Player.FlatPhysicalDamageMod + 0.2f * Program.Player.TotalMagicalDamage) * 16}
                    [spellLevel];
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
