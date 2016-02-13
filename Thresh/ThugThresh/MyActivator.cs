using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThugThresh
{
    class MyActivator
    {
        public static Spell.Targeted ignite;
        public static Item talisman, randuin, mikael, ironsolari, fotmountain, glory;
        public static Spell.Targeted exhaust;
        public static Spell.Active heal;
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
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Contains("exhaust"))
                exhaust = new Spell.Targeted(SpellSlot.Summoner1, 650);
            else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Contains("exhaust"))
                exhaust = new Spell.Targeted(SpellSlot.Summoner2, 650);
            talisman = new Item((int)ItemId.Talisman_of_Ascension);
            randuin = new Item((int)ItemId.Randuins_Omen);
            glory = new Item((int)ItemId.Righteous_Glory);
            fotmountain = new Item((int)ItemId.Face_of_the_Mountain);
            mikael = new Item((int)ItemId.Mikaels_Crucible);
            ironsolari = new Item((int)ItemId.Locket_of_the_Iron_Solari);
        }
    }
}
