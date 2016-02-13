using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThugThresh
{
    class ThreshMenu
    {
        public static Menu MyMenu, MyCombo, MyDraw, MyHarrass, MyActivator, MySpells, MyFarm, MyOtherFunctions;
        public static void loadMenu()
        {
            MyThreshPage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarrassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        public static void MyThreshPage()
        {
            MyMenu = MainMenu.AddMenu("ThugThresh", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" Thugh Thresh - " + Program.version);
            MyMenu.AddLabel(" Made by -iRaxe");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Use SpaceBar for Combo");
            MyMenu.AddLabel(" - Use the key V For LaneClear/JungleClear");
            MyMenu.AddLabel(" - Use the key T For Flee");
        }

        public static void MyDrawPage()
        {
            MyDraw = MyMenu.AddSubMenu("Draw  settings", "Draw");
            MyDraw.AddGroupLabel("Draw Settings:");
            MyDraw.Add("nodraw", new CheckBox("No Display Drawing", false));
            MyDraw.Add("onlyReady", new CheckBox("Display only Ready", true));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", new CheckBox("Draw Death Sentence Range (Q Spell)", true));
            MyDraw.Add("draw.W", new CheckBox("Draw Dark Passage Range (W Spell)", true));
            MyDraw.Add("draw.E", new CheckBox("Draw Flay Range (E Spell)", true));
            MyDraw.Add("draw.R", new CheckBox("Draw The Box Range (R Spell)", true));
            MyDraw.AddSeparator();
            MyDraw.AddLabel("Informations About the allies:");
            MyDraw.Add("allyhp", new CheckBox("Draw HP info"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific draw");
        }

        public static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.Add("combo.Q", new CheckBox("Use Death Sentence (Q Spell)"));
            MyCombo.Add("combo.Q2", new CheckBox("Use Death Lap (Q2 Spell)"));
            MyCombo.Add("combo.W", new CheckBox("Use Dark Passage (W Spell)"));
            MyCombo.Add("combo.E", new CheckBox("Use Flay (E Spell)"));
            MyCombo.Add("combo.R", new CheckBox("Use The Box (R Spell)"));
            MyCombo.Add("combo.REnemies", new Slider("R Min Enemies >=", 2, 1, 5));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }
        public static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.E", new CheckBox("Use Flay (E Spell)", false));
            MyFarm.Add("lc.Mana", new Slider("Min. Mana%", 30));
            MyFarm.Add("lc.MinionsE", new Slider("Min. Minions for Flay ", 3, 0, 10));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", new CheckBox("Use Death Sentence in Jungle (Q Spell)"));
            MyFarm.Add("jungle.W", new CheckBox("Use Dark Passage in Jungle (W Spell)"));
            MyFarm.Add("jungle.E", new CheckBox("Use Flay in Jungle (E Spell)"));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }
        public static void MyHarrassPage()
        {
            MyHarrass = MyMenu.AddSubMenu("Harrass/Flay Settings", "rlogic");
            MyHarrass.AddGroupLabel("Harrass Settings:");
            MyHarrass.AddSeparator();
            MyHarrass.Add("harrass.Q1", new CheckBox("Use Death Sentence (Q Spell)"));
            MyHarrass.Add("harrass.Q2", new CheckBox("Use Death Lap (Q2 Spell)", false));
            MyHarrass.Add("harrass.E", new CheckBox("Use Flay (E Spell)"));
            MyHarrass.AddGroupLabel("Flay Settings:");
            MyHarrass.Add("dash.e", new CheckBox("Flay (E Spell) on Dash (Smart)"));
            MyHarrass.Add("interrupt.e", new CheckBox("Flay (E Spell) to Interrupt"));
            MyHarrass.Add("gapcloser.e", new CheckBox("Flay (E Spell) on Incoming Gapcloser"));
            MyHarrass.AddGroupLabel("Pro Tips");
            MyHarrass.AddLabel(" -Remember to play safe and don't be a teemo");
        }
        public static void MyActivatorPage()
        {
            MyActivator = MyMenu.AddSubMenu("Items Settings", "Items");
            MyActivator.AddGroupLabel("Items usage:");
            MyActivator.AddSeparator();
            MyActivator.Add("items.sliderHP", new Slider("Use items when HP is lower than {0}(%)", 30, 1, 100));
            MyActivator.Add("items.enemiesinrange", new Slider("Use items when there are {0} enemies in range", 3, 1, 5));
            MyActivator.AddSeparator();
            MyActivator.AddLabel("Items avaible to use with Activator:");
            MyActivator.Add("talisman", new CheckBox("Use Talisman of Ascension"));
            MyActivator.Add("randuin", new CheckBox("Use Randuin"));
            MyActivator.Add("glory", new CheckBox("Use Righteous Glory"));
            MyActivator.Add("fotmountain", new CheckBox("Use Face of the Mountain"));
            MyActivator.Add("mikael", new CheckBox("Use Mikaels Crucible"));
            MyActivator.Add("ironsolari", new CheckBox("Use Locket of the Iron Solari"));
            MySpells = MyMenu.AddSubMenu("Spells Settings");
            MySpells.AddGroupLabel("Spells settings:");
            MySpells.Add("useexhaust", new CheckBox("Use Exhaust on:"));
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                MySpells.Add(source.ChampionName + "exhaust",
                    new CheckBox("Exhaust " + source.ChampionName, false));
            }
            MySpells.AddSeparator();
            MySpells.AddGroupLabel("Heal settings:");
            MySpells.Add("spells.Heal.Hp", new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            MySpells.AddGroupLabel("Ignite settings:");
            MySpells.Add("spell.Ignite.Use", new CheckBox("Use Ignite for KillSteal"));
            MySpells.Add("spells.Ignite.Focus", new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1, 100));
            MySpells.Add("spells.Ignite.Kill", new CheckBox("Use ignite if killable"));
        }
        public static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup", new CheckBox("Auto Level Up Spells:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Death Sentence settings");
            MyOtherFunctions.Add("dash.q", new CheckBox("Use Death Sentence (Q Spell) Smart", false));
            MyOtherFunctions.Add("interrupt.q", new CheckBox("Use Death Sentence (Q Spell) to Interrupt"));
            MyOtherFunctions.Add("immobile.q", new CheckBox("Use Death Sentence (Q Spell) to Immobile"));
            MyOtherFunctions.AddGroupLabel("Dark Passage settings:");
            MyOtherFunctions.Add("lantern", new KeyBind("I NEED A TAXI!! ( Use W Spell )", false, KeyBind.BindTypes.HoldActive, 92));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.Add("lowallies.w", new CheckBox("Use Dark Passage (W Spell) on Low Allies"));
            MyOtherFunctions.Add("allypercent.w", new Slider("Set here the % for the Low Allies class ", 30));
            MyOtherFunctions.AddGroupLabel("Pull/Push Keybinds");
            MyOtherFunctions.Add("push", new KeyBind("Push Enemy", false, KeyBind.BindTypes.HoldActive, 88));
            MyOtherFunctions.Add("pull", new KeyBind("Pull Enemy", false, KeyBind.BindTypes.HoldActive, 90));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("skin.Id", new Slider("Skin Editor", 3, 1, 4));
        }
        public static bool Allydrawn()
        {
            return MyDraw["allyhp"].Cast<CheckBox>().CurrentValue;
        }
        public static int skinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }
        public static float spellsHealignite()
        {
            return MySpells["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        public static bool spellsIgniteOnlyHpLow()
        {
            return MySpells["spells.Ignite.Kill"].Cast<CheckBox>().CurrentValue;
        }
        public static bool spellsUseIgnite()
        {
            return MySpells["spells.Ignite.Use"].Cast<CheckBox>().CurrentValue;
        }
        public static float spellsHealhp()
        {
            return MySpells["spells.Heal.Hp"].Cast<Slider>().CurrentValue;
        }
        public static bool nodraw()
        {
            return MyDraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }
        public static bool onlyReady()
        {
            return MyDraw["onlyready"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsQ()
        {
            return MyDraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsW()
        {
            return MyDraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsE()
        {
            return MyDraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsR()
        {
            return MyDraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }
    }
}
