using System.Threading;
using EloBuddy;
using EloBuddy.SDK;

namespace KzKarthus
{
    class MyActivator
    {
        public static Spell.Targeted ignite;
        public static Item Zhonya;
        public static Spell.Targeted smite;
        public static Spell.Active heal, barrier;
        public static void loadSpells()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner1, 580);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner2, 580);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner1);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner2);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("barrier"))
                barrier = new Spell.Active(SpellSlot.Summoner1);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("barrier"))
                barrier = new Spell.Active(SpellSlot.Summoner2);
            Zhonya = new Item((int)ItemId.Zhonyas_Hourglass);
        }
    }
}
