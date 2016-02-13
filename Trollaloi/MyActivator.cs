using EloBuddy;
using EloBuddy.SDK;

namespace Trollaloi
{
    class MyActivator
    {
        public static Spell.Targeted ignite;
        public static Item talisman, randuin, mikael, ironsolari, fotmountain, glory, youmus, botrk, bilgewater, CorruptPot, HuntersPot, RefillPot, Biscuit, HPPot, Qss, Mercurial, Hydra, Tiamat;
        public static Spell.Targeted smite;
        public static Spell.Active heal, Barrier;
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

            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("barrier"))
                Barrier = new Spell.Active(SpellSlot.Summoner1);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("barrier"))
                Barrier = new Spell.Active(SpellSlot.Summoner2);

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
            Qss = new Item((int)ItemId.Quicksilver_Sash);
            Mercurial = new Item((int)ItemId.Mercurial_Scimitar);
            Hydra = new Item((int)ItemId.Ravenous_Hydra_Melee_Only);
            Tiamat = new Item((int)ItemId.Tiamat_Melee_Only);
            HPPot = new Item(2003);
            Biscuit = new Item(2010);
            CorruptPot = new Item(2033);
            HuntersPot = new Item(2032);
            RefillPot = new Item(2031);
        }
    }
}
