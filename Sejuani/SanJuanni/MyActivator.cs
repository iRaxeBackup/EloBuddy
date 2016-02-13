using EloBuddy;
using EloBuddy.SDK;

namespace SanJuanni
{
    class MyActivator
    {
        public static Spell.Targeted ignite;
        public static Item talisman, randuin, mikael, ironsolari, fotmountain, glory, youmus, botrk, bilgewater;
        public static Spell.Targeted smite;
        public static Spell.Active heal;
        public static void loadSpells()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("smite"))
                smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("smite"))
                smite = new Spell.Targeted(SpellSlot.Summoner2, 570);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner1, 580);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("dot"))
                ignite = new Spell.Targeted(SpellSlot.Summoner2, 580);

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner1);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("heal"))
                heal = new Spell.Active(SpellSlot.Summoner2);
            talisman = new Item((int)ItemId.Talisman_of_Ascension);
            randuin = new Item((int)ItemId.Randuins_Omen);
            glory = new Item((int)ItemId.Righteous_Glory);
            fotmountain = new Item((int)ItemId.Face_of_the_Mountain);
            mikael = new Item((int)ItemId.Mikaels_Crucible);
            ironsolari = new Item((int)ItemId.Locket_of_the_Iron_Solari);
            youmus = new Item((int)ItemId.Youmuus_Ghostblade);
            botrk = new Item((int)ItemId.Blade_of_the_Ruined_King);
            bilgewater = new Item((int)ItemId.Bilgewater_Cutlass);
        }
    }
}
