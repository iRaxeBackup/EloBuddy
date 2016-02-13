using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace AAtron
{
    static class AatroxMenu
    {
        private static Menu MyMenu;
        public static Menu MyCombo;
        private static Menu MyDraw;
        public static Menu MyHarrass, MyActivator, MySpells, MyFarm, MyOtherFunctions;

        public static void loadMenu()
        {
            MyAatroxPage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarrassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        private static void MyAatroxPage()
        {
            MyMenu = MainMenu.AddMenu("AAtron", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" AAtron - " + Program.version);
            MyMenu.AddLabel(" Made by -iRaxe");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Use SpaceBar for Combo");
        }

        private static void MyDrawPage()
        {
            MyDraw = MyMenu.AddSubMenu("Draw  settings", "Draw");
            MyDraw.AddGroupLabel("Draw Settings:");
            MyDraw.Add("nodraw", 
                new CheckBox("No Display Drawing", false));
            MyDraw.Add("onlyReady", 
                new CheckBox("Display only Ready", true));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", 
                new CheckBox("Draw Dark Flight Range (Q Spell)", true));
            MyDraw.Add("draw.W", 
                new CheckBox("Draw Blood Thirst/Blood Price Range (W Spell)", true));
            MyDraw.Add("draw.E", 
                new CheckBox("Draw Blades of Torment (E Spell)", true));
            MyDraw.Add("draw.R", 
                new CheckBox("Draw Massacre Range (R Spell)", true));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific draw");
        }

        private static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.Add("combo.Q", 
                new CheckBox("Use Dark Flight (Q Spell)"));
            MyCombo.Add("combo.W",
                new CheckBox("Use Blood Thirst/Blood Price (W Spell)"));
            MyCombo.Add("combo.minw",
                new Slider("Min hp to Blood Thirst / Blood Price (W Spell)", 50, 0, 100));
            MyCombo.Add("combo.maxw", 
                new Slider("Max hp to Blood Thirst / Blood Price (W Spell)", 80, 0, 100));
            MyCombo.Add("combo.E", 
                new CheckBox("Use Blades of Torment (E Spell)"));
            MyCombo.Add("combo.R",
                new CheckBox("Use Massacre (R Spell)"));
            MyCombo.Add("combo.REnemies", 
                new Slider("R Min Enemies >=", 2, 1, 5));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }

        private static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q",
                new CheckBox("Use Dark Flight (Q Spell)", false));
            MyFarm.Add("lc.MinionsQ", 
                new Slider("Min. Minions for Dark Flight ", 3, 0, 10));
            MyFarm.Add("lc.E", 
                new CheckBox("Use Blades of Torment (E Spell)", false));
            MyFarm.Add("lc.MinionsE",
                new Slider("Min. Minions for Blades of Torment ", 3, 0, 10));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", 
                new CheckBox("Use Dark Flight in Jungle (Q Spell)"));
            MyFarm.Add("jungle.E",
                new CheckBox("Use Blades of Torment in Jungle (E Spell)"));
            MyFarm.Add("jungle.W",
                new CheckBox("Use Blood Thirst/Blood Price in Jungle (W Spell)"));
            MyFarm.Add("jungle.minw",
                new Slider("Min hp to Blood Thirst / Blood Price (W Spell)", 50, 0, 100));
            MyFarm.Add("jungle.maxw",
                new Slider("Max hp to Blood Thirst / Blood Price (W Spell)", 80, 0, 100));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }

        private static void MyHarrassPage()
        {
            MyHarrass = MyMenu.AddSubMenu("Harrass/Blades of Torment Settings", "harrass");
            MyHarrass.AddGroupLabel("Harrass Settings:");
            MyHarrass.AddSeparator();
            MyHarrass.Add("harrass.E", 
                new CheckBox("Use Blades of Torment (E Spell)"));
            MyHarrass.AddGroupLabel("Blades of Torment Settings:");
            MyHarrass.Add("interrupt.q",
                new CheckBox("Blades of Torment (Q Spell) to Interrupt"));
            MyHarrass.Add("gapcloser.e", 
                new CheckBox("Blades of Torment (E Spell) on Incoming Gapcloser"));
            MyHarrass.AddGroupLabel("Pro Tips");
            MyHarrass.AddLabel(" -Remember to play safe and don't be a teemo");
        }

        private static void MyActivatorPage()
        {
            MyActivator = MyMenu.AddSubMenu("Activator Settings", "Items");
            MyActivator.AddGroupLabel("Auto QSS if :");
            MyActivator.Add("Blind",
                new CheckBox("Blind", false));
            MyActivator.Add("Charm",
                new CheckBox("Charm"));
            MyActivator.Add("Fear",
                new CheckBox("Fear"));
            MyActivator.Add("Polymorph",
                new CheckBox("Polymorph"));
            MyActivator.Add("Stun",
                new CheckBox("Stun"));
            MyActivator.Add("Snare",
                new CheckBox("Snare"));
            MyActivator.Add("Silence",
                new CheckBox("Silence", false));
            MyActivator.Add("Taunt",
                new CheckBox("Taunt"));
            MyActivator.Add("Suppression",
                new CheckBox("Suppression"));
            MyActivator.AddGroupLabel("Ults");
            MyActivator.Add("ZedUlt",
                new CheckBox("Zed Ult"));
            MyActivator.Add("VladUlt",
                new CheckBox("Vlad Ult"));
            MyActivator.Add("FizzUlt",
                new CheckBox("Fizz Ult"));
            MyActivator.Add("MordUlt",
                new CheckBox("Mordekaiser Ult"));
            MyActivator.Add("PoppyUlt",
                new CheckBox("Poppy Ult"));
            MyActivator.AddGroupLabel("Items usage:");
            MyActivator.AddSeparator();
            MyActivator.Add("items.sliderHP",
                new Slider("Use items when HP is lower than {0}(%)", 30, 1, 100));
            MyActivator.Add("items.enemiesinrange",
                new Slider("Use items when there are {0} enemies in range", 3, 1, 5));
            MyActivator.AddSeparator();
            MyActivator.AddLabel("Items avaible to use with Activator:");
            MyActivator.Add("randuin",
                new CheckBox("Use Randuin"));
            MyActivator.Add("glory",
                new CheckBox("Use Righteous Glory"));
            MyActivator.Add("bilgewater", 
                new CheckBox("Use Bilgewater Cutlass"));
            MyActivator.Add("botrk",
                new CheckBox("Use Blade of The Ruined King"));
            MyActivator.Add("youmus", 
                new CheckBox("Use Youmus Ghostblade"));
            MyActivator.Add("hydra",
                new CheckBox("Use Hydra"));
            MyActivator.Add("tiamat",
                new CheckBox("Use Tiamat"));
            MySpells = MyMenu.AddSubMenu("Spells Settings");
            MySpells.AddGroupLabel("Smite settings");
            MySpells.AddSeparator();
            MySpells.Add("SRU_Red",
                new CheckBox("Smite Red Buff"));
            MySpells.Add("SRU_Blue", 
                new CheckBox("Smite Blue Buff"));
            MySpells.Add("SRU_Dragon", 
                new CheckBox("Smite Dragon"));
            MySpells.Add("SRU_Baron",
                new CheckBox("Smite Baron"));
            MySpells.Add("SRU_Gromp",
                new CheckBox("Smite Gromp"));
            MySpells.Add("SRU_Murkwolf", 
                new CheckBox("Smite Wolf"));
            MySpells.Add("SRU_Razorbeak",
                new CheckBox("Smite Bird"));
            MySpells.Add("SRU_Krug", 
                new CheckBox("Smite Golem"));
            MySpells.Add("Sru_Crab", 
                new CheckBox("Smite Crab"));
            MySpells.AddSeparator();
            MySpells.AddGroupLabel("Heal settings:");
            MySpells.Add("spells.Heal.Hp", 
                new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            MySpells.AddGroupLabel("Ignite settings:");
            MySpells.Add("spell.Ignite.Use", 
                new CheckBox("Use Ignite for KillSteal"));
            MySpells.Add("spells.Ignite.Focus",
                new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1, 100));
            MySpells.Add("spells.Ignite.Kill",
                new CheckBox("Use ignite if killable"));
        }

        private static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup", 
                new CheckBox("Auto Level Up Spells:", false));;
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("skin.Id", 
                new Slider("Skin Editor", 3, 1, 4));
        }

        public static int skinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }
        public static float spellsHealignite()
        {
            return MySpells["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        public static float checkenemies()
        {
            return MySpells["items.enemiesinrange"].Cast<Slider>().CurrentValue;
        }
        public static float checkhp()
        {
            return MySpells["items.sliderHP"].Cast<Slider>().CurrentValue;
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
            return MyDraw["onlyReady"].Cast<CheckBox>().CurrentValue;
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
        public static bool SpellsPotionsCheck()
        {
            return MyActivator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }
        public static bool tiamat()
        {
            return MyActivator["tiamat"].Cast<CheckBox>().CurrentValue;
        }
        public static bool hydra()
        {
            return MyActivator["hydra"].Cast<CheckBox>().CurrentValue;
        }
        public static float SpellsPotionsHP()
        {
            return MyActivator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsPotionsM()
        {
            return MyActivator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }
        public static float spellsBarrierHP()
        {
            return MyActivator["spells.Barrier.Hp"].Cast<Slider>().CurrentValue;
        }

        public static bool Blind()
        {
            return MyActivator["Blind"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Charm()
        {
            return MyActivator["Charm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fear()
        {
            return MyActivator["Fear"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Polymorph()
        {
            return MyActivator["Polymorph"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Stun()
        {
            return MyActivator["Stun"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Snare()
        {
            return MyActivator["Snare"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Silence()
        {
            return MyActivator["Silence"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Taunt()
        {
            return MyActivator["Taunt"].Cast<CheckBox>().CurrentValue;
        }
        public static bool Suppression()
        {
            return MyActivator["Suppression"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ZedUlt()
        {
            return MyActivator["ZedUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool VladUlt()
        {
            return MyActivator["VladUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool FizzUlt()
        {
            return MyActivator["FizzUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool MordUlt()
        {
            return MyActivator["MordUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool PoppyUlt()
        {
            return MyActivator["PoppyUlt"].Cast<CheckBox>().CurrentValue;
        }
    }
}
