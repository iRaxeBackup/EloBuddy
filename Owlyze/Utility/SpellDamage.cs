using EloBuddy;
using EloBuddy.SDK;

namespace Owlyze
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
                damage += Program.Q.GetRealDamage(target) + OwlyzeMenu.ComboQ1();
            }

            // W
            if (Program.W.IsReady())
            {
                damage += Program.W.GetRealDamage(target) + OwlyzeMenu.ComboW1();
            }

            // E
            if (Program.E.IsReady())
            {
                damage += Program.E.GetRealDamage(target) + OwlyzeMenu.ComboE1();
            }

            // R
            if (Program.R.IsReady())
            {
                damage += Program.R.GetRealDamage(target);
            }

            return damage;
        }

        private static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        private static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Program.Player.Spellbook.GetSpell(slot).Level;
            var mana = Program.Player.MaxMana;
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

                    damage = new float[] { 60, 85, 110, 135, 160 }[spellLevel] +
                             0.55f * Program.Player.FlatMagicDamageMod +
                             new[] { 0.02f * mana, 0.025f * mana, 0.03f * mana, 0.035f * mana, 0.04f * mana }[spellLevel];
                    break;

                case SpellSlot.W:

                    damage = new float[] { 80, 100, 120, 140, 160 }[spellLevel] +
                             0.4f * Program.Player.FlatMagicDamageMod + 2.5f * mana;
                    break;

                case SpellSlot.E:

                    damage = new float[] { 54, 78, 102, 126, 150 }[spellLevel] + 0.3f * Program.Player.FlatMagicDamageMod + 0.03f * mana;
                    break;

                case SpellSlot.R:

                    damage = new float[] { 0, 0, 0 }[spellLevel] + 0.0f * Program.Player.FlatPhysicalDamageMod;
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
