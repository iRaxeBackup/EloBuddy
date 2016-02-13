using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Owlyze
{
    internal static class OwlyzeMenu
    {

        #region Main Menu
        //This function will create the Main Menu 
        private static Menu 
            MyMenu;
        #endregion

        #region Sub Menu
        /*This function will create the Sub Menu
        MyCombo = Here goes the entire Combo Settings
        MyDraw = Here goes the entire Draw Settings
        MyHarass = Here goes the entire Harass Settings
        MyActivator = Here goes the entire Activator Settings
        MyFarm = Here goes the entire Farm/Jungle Settings
        MyOtherFunctions = Here goes the entire Misc Settings*/

        public static Menu
            MyCombo,
            MyDraw,
            MyHarass,
            MyActivator,
            MyFarm,
            MyOtherFunctions;
        #endregion

        #region Menu Loading
        /*This allow the file to be charged in Program
        The function will load with this command in Program
        OwlyzeMenu.LoadMenu();
        Where OwlyzeMenu is the Class and LoadMenu is this function
        MyOwlyzePage = Will fill the MyMenu as First Page
        MyDrawPage = Will fill the MyDraw as Second Page 
        MyComboPage = Will fill the MyCombo as Third Page
        MyFarmPage = Will fill the MyFarm as Fourth Page
        MyHarassPage = Will fill the MyHarass as Fifth Page
        MyActivatorPage = Will fill the MyActivator as Sixth Page
        MyOtherFunctionsPage = Will fill the MyOtherFunctions as Seventh Page*/
        public static void LoadMenu()
        {
            MyOwlyzePage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }
        #endregion

        #region Info Page (Main)
        private static void MyOwlyzePage()
        {
            MyMenu = MainMenu.AddMenu("Owlyze", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" Owlyze - " + Program.Version);
            MyMenu.AddLabel(" Made by -iRaxe");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Use SpaceBar for Combo");
            MyMenu.AddLabel(" - Use the key V For LaneClear/JungleClear");
            MyMenu.AddLabel(" - Use the key T For Flee");
        }
        #endregion

        #region Drawings Page (Second Page)
        private static void MyDrawPage()
        {
            MyDraw = MyMenu.AddSubMenu("Draw  settings", "Draw");
            MyDraw.AddGroupLabel("Draw Settings:");
            MyDraw.Add("nodraw", 
                new CheckBox("No Display Drawing", false));
            MyDraw.Add("onlyReady", 
                new CheckBox("Display only Ready"));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", 
                new CheckBox("Draw Overload Range (Q Spell)"));
            MyDraw.Add("draw.W", 
                new CheckBox("Draw Rune Prison Range (W Spell)"));
            MyDraw.Add("draw.E", 
                new CheckBox("Draw Spell Flux Range (E Spell)"));
            MyDraw.Add("draw.R", 
                new CheckBox("Draw Desperate Power Range (R Spell)"));
            MyDraw.Add("draw.T",
                new CheckBox("Draw Wards Position"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxeses if you wish to dont see a specific draw");
        }
        #endregion

        #region Combo Page (Third Page)
        private static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.AddLabel("Use Overload (Q Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyCombo.Add("combo.q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddLabel("Use Rune Prison (W Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyCombo.Add("combo.w" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddLabel("Use Spell Flux (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyCombo.Add("combo.e" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddLabel("Use Desperate Power (R Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyCombo.Add("combo.r" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo preferences:");
            MyCombo.Add("combo.AA",
               new CheckBox("Dont Use AutoAttacks in this mode"));
            MyCombo.Add("combo.CC",
                new CheckBox("Use Overload (Q Spell) on CC"));
            MyCombo.Add("combo.CC1",
                new CheckBox("Use Rune Prison (W Spell) on CC"));
            MyCombo.Add("combo.CC2",
                new CheckBox("Use Spell Flux (E Spell) on CC"));
            var aa = MyCombo.Add("combo.Mode", new Slider("Combo Modes", 0, 0, 1));
            var ab = new[] { "Mode: Normal Combo", "Mode: Slutty Combo" };
            aa.DisplayName = ab[aa.CurrentValue];

            aa.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = ab[changeArgs.NewValue];
                };
            MyCombo.Add("combo.Q1",
                new Slider("Overload (Q Spell) Overkill", 60, 0, 500));
            MyCombo.Add("combo.W1",
                new Slider("Rune Prison (W Spell) Overkill", 60, 0, 500));
            MyCombo.Add("combo.E1", 
                new Slider("Spell Flux (E Spell) Overkill", 60, 0, 500));

            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }
        #endregion

        #region Farm Page (Fourth Page)
        private static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q", 
                new CheckBox("Use Overload (Q Spell)"));
            MyFarm.AddSeparator();
            MyFarm.Add("lc.W", 
                new CheckBox("Use Rune Prison (W Spell)", false));
            MyFarm.AddSeparator();
            MyFarm.Add("lc.E", 
                new CheckBox("Use Spell Flux (E Spell)", false));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Laneclear preferences:");
            MyFarm.Add("lc.H",
                new CheckBox("Prioritize Harass instead of LastHit"));
            MyFarm.Add("lc.Q1",
               new CheckBox("Use Overload (Q Spell) for LastHit", false));
            MyFarm.Add("lc.W1",
                new CheckBox("Use Rune Prison (W Spell) for LastHit", false));
            MyFarm.Add("lc.E1",
                new CheckBox("Use Spell Flux (E Spell) for LastHit", false));
            MyFarm.Add("lc.R",
                new CheckBox("Use Desperate Power (R Spell)", false));
            MyFarm.Add("lc.S",
                new CheckBox("Limit passive stacks", false));
            MyFarm.Add("lc.S1",
                new Slider("Maximium stacks", 3, 1, 5));
            MyFarm.Add("lc.M", 
                new Slider("Min. Mana for Laneclear Spells %", 30));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", 
                new CheckBox("Use Overload in Jungle (Q Spell)"));
            MyFarm.Add("jungle.W", 
                new CheckBox("Use Rune Prison in Jungle (W Spell)"));
            MyFarm.Add("jungle.E", 
                new CheckBox("Use Spell Flux in Jungle (E Spell)"));
            MyFarm.Add("jungle.R",
                new CheckBox("Use Desperate Power (R Spell)", false));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("JungleClear preferences:");
            MyFarm.Add("jungle.S",
                new CheckBox("Limit passive stacks", false));
            MyFarm.Add("jungle.S1",
                new Slider("Maximium stacks", 3, 1, 5));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }
        #endregion

        #region Harass/KillSteal Page (Fifth Page)
        private static void MyHarassPage()
        {
            MyHarass = MyMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            MyHarass.AddGroupLabel("Harass Settings:");
            MyHarass.AddLabel("Use Overload (Q Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyHarass.Add("harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.AddLabel("Use Rune Prison (W Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyHarass.Add("harass.W" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.AddLabel("Use Spell Flux (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies)
            {
                MyHarass.Add("harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Harass preferences:");
            MyHarass.Add("harass.A",
                new CheckBox("Auto Harass", false));
            MyHarass.Add("harass.S",
                new CheckBox("Limit passive stacks", false));
            MyHarass.Add("harass.S1",
                new Slider("Maximium stacks", 3, 1, 5));
            MyHarass.Add("harass.QE", 
                new Slider("Min. Mana for Harass Spells %", 35));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Settings:");
            MyHarass.Add("killsteal.Q",
                new CheckBox("Use Overload (Q Spell)", false));
            MyHarass.Add("killsteal.W",
                new CheckBox("Use Rune Prison (W Spell)", false));
            MyHarass.Add("killsteal.E",
                new CheckBox("Use Spell Flux (E Spell)", false));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Pro Tips");
            MyHarass.AddLabel(" -Remember to play safe and don't be a teemo");
        }
        #endregion

        #region Activator Page (Sixth Page)
        private static void MyActivatorPage()
        {
            MyActivator = MyMenu.AddSubMenu("Activator Settings", "Items");
            MyActivator.AddGroupLabel("Items usage:");
            MyActivator.AddSeparator();
            MyActivator.Add("checkward",
                new CheckBox("Use AutoWard", false));
            MyActivator.Add("pinkvision",
                new CheckBox("Use Pink Ward", false));
            MyActivator.Add("greatherstealthtotem",
                new CheckBox("Use Greather Stealth Totem", false));
            MyActivator.Add("greatervisiontotem",
                new CheckBox("Use Greater Vision Totem", false));
            MyActivator.Add("wardingtotem",
                new CheckBox("Use Warding Totem", false));
            MyActivator.Add("farsightalteration",
                new CheckBox("Use Farsight Alteration", false));
            MyActivator.AddSeparator();
            MyActivator.Add("seraph",
               new CheckBox("Use Seraph's Embrace"));
            MyActivator.Add("seraph.HP",
                new Slider("Use Seraph's Embrace if hp is lower than {0}(%)", 60));
            MyActivator.Add("seraph.Enemies",
                new Slider("Use Seraph's Embrace if when there are {0} enemies in range", 3, 1, 5));
            MyActivator.Add("zhonya",
                new CheckBox("Use Zhonyia's Hourglass"));
            MyActivator.Add("zhonya.HP",
                new Slider("Use Zhonyia's Hourglass if hp is lower than {0}(%)", 60));
            MyActivator.Add("zhonya.Enemies",
                new Slider("Use Zhonyia's Hourglass if when there are {0} enemies in range", 3, 1 , 5));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Potion Settings");
            MyActivator.Add("spells.Potions.Check", 
                new CheckBox("Use Potions"));
            MyActivator.Add("spells.Potions.HP", 
                new Slider("Use Potions when HP is lower than {0}(%)", 60, 1));
            MyActivator.Add("spells.Potions.Mana", 
                new Slider("Use Potions when Mana is lower than {0}(%)", 60, 1));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Smite settings");
            MyActivator.Add("SRU_Red", new CheckBox("Smite Red Buff"));
            MyActivator.Add("SRU_Blue", new CheckBox("Smite Blue Buff"));
            MyActivator.Add("SRU_Dragon", new CheckBox("Smite Dragon"));
            MyActivator.Add("SRU_Baron", new CheckBox("Smite Baron"));
            MyActivator.Add("SRU_Gromp", new CheckBox("Smite Gromp"));
            MyActivator.Add("SRU_Murkwolf", new CheckBox("Smite Wolf"));
            MyActivator.Add("SRU_Razorbeak", new CheckBox("Smite Bird"));
            MyActivator.Add("SRU_Krug", new CheckBox("Smite Golem"));
            MyActivator.Add("Sru_Crab", new CheckBox("Smite Crab"));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Spells settings:");
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Barrier settings:");
            MyActivator.Add("spells.Barrier.Hp", 
                new Slider("Use Barrier when HP is lower than {0}(%)", 30, 1));
            MyActivator.AddGroupLabel("Heal settings:");
            MyActivator.Add("spells.Heal.Hp", 
                new Slider("Use Heal when HP is lower than {0}(%)", 30, 1));
            MyActivator.AddGroupLabel("Ignite settings:");
            MyActivator.Add("spells.Ignite.Focus", 
                new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1));
        }
        #endregion

        #region Misc Page (Seventh Page)
        private static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Settings for GapCloser/Interrupter");
            MyOtherFunctions.Add("gapcloser.W",
                new CheckBox("Use Rune Prison (W Spell) For Anti-Gap"));
            MyOtherFunctions.Add("gapcloser.R",
                new CheckBox("Use Desperate Power (R Spell) For Anti-Gap", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Settings for Flee");
            MyOtherFunctions.Add("flee.M", 
                new Slider("Use Desperate Power (R Spell) for Flee if mana is higher than {0}(%)", 10, 1));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Passive Function");
            MyOtherFunctions.Add("passiveS",
                new CheckBox("Auto Stack Passive", false));
            MyOtherFunctions.Add("passiveS1",
                new CheckBox("Limit passive stacks", false));
            MyOtherFunctions.Add("passiveS2",
                new Slider("Maximium stacks", 2, 1, 5));
            MyOtherFunctions.Add("passiveS3",
                new Slider("Use Auto Stack Passive if mana is higher than {0}(%)", 10, 1));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup", 
                new CheckBox("Auto Level Up Spells:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("checkSkin",
                new CheckBox("Use skin changer:"));
            MyOtherFunctions.Add("skin.Id", 
                new Slider("Skin Editor", 5, 1, 10));
        }
        #endregion

        #region Info
        //Now we will check every CheckBox and Slider of each page and value here
        #endregion

        #region Drawings
        //Checkbox for Nodraw
        public static bool Nodraw()
        {
            return MyDraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for OnlyReady Spells in Drawing
        public static bool OnlyReady()
        {
            return MyDraw["onlyReady"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the Q Spell Range
        public static bool DrawingsQ()
        {
            return MyDraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the W Spell Range
        public static bool DrawingsW()
        {
            return MyDraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the E Spell Range
        public static bool DrawingsE()
        {
            return MyDraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the R spell Range
        public static bool DrawingsR()
        {
            return MyDraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the ideal positions of trinkets
        public static bool DrawingsT()
        {
            return MyDraw["draw.T"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region Combo Preferences
        //Checkbox for Dont Use AutoAttacks in Combo Mode
        public static bool ComboAA()
        {
            return MyCombo["combo.AA"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Overload (Q Spell) on CC
        public static bool ComboCC()
        {
            return MyCombo["combo.CC"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Rune Prison (W Spell) on CC
        public static bool ComboCC1()
        {
            return MyCombo["combo.CC1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Spell Flux (E Spell) on CC
        public static bool ComboCC2()
        {
            return MyCombo["combo.CC2"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Overload (Q Spell) Overkill
        public static float ComboQ1()
        {
            return MyCombo["combo.Q1"].Cast<Slider>().CurrentValue;
        }
        //Slider for Rune Prison (W Spell) Overkill
        public static float ComboW1()
        {
            return MyCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        //Slider for Spell Flux (E Spell) Overkill
        public static float ComboE1()
        {
            return MyCombo["combo.E1"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Laneclear
        //Checkbox for Prioritize Harass instead of LastHit
        public static bool LcH()
        {
            return MyFarm["lc.H"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Overload (Q Spell) in Farm
        public static bool LcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Overload (Q Spell) in Lasthit 
        public static bool LcQ1()
        {
            return MyFarm["lc.Q1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Rune Prison (W Spell) in Farm
        public static bool LcW()
        {
            return MyFarm["lc.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Rune Prison (W Spell) in Lasthit
        public static bool LcW1()
        {
            return MyFarm["lc.W1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Spell Flux (E Spell) in Farm
        public static bool LcE()
        {
            return MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Spell Flux (E Spell) in Lasthit
        public static bool LcE1()
        {
            return MyFarm["lc.E1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Desperate Power (R Spell) in Farm
        public static bool LcR()
        {
            return MyFarm["lc.R"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Limit passive stacks in Farm
        public static bool LcS()
        {
            return MyFarm["lc.S"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit passive stacks in Farm
        public static float LcS1()
        {
            return MyFarm["lc.S1"].Cast<Slider>().CurrentValue;
        }
        //Slider for limit mana in Farm
        public static float LcM()
        {
            return MyFarm["lc.M"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Jungle
        //Checkbox for Overload (Q Spell) in Jungle
        public static bool JungleQ()
        {
            return MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Rune Prison (W Spell) in Jungle
        public static bool JungleW()
        {
            return MyFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Spell Flux (E Spell) in Jungle
        public static bool JungleE()
        {
            return MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Desperate Power (R Spell) in Jungle
        public static bool JungleR()
        {
            return MyFarm["jungle.R"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Limit passive stacks in Jungle
        public static bool JungleS()
        {
            return MyFarm["jungle.S"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit passive stacks in Jungle
        public static float JungleS1()
        {
            return MyFarm["jungle.S1"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Harass
        //Checkbox for AutoHarass
        public static bool HarassA()
        {
            return MyHarass["harass.A"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit mana in Harass
        public static float HarassQE()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }
        //Checkbox for Limit passive stacks in Harass
        public static bool HarassS()
        {
            return MyHarass["harass.S"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit passive stacks in Harass
        public static float HarassS1()
        {
            return MyHarass["harass.S1"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region KillSteal
        //Checkbox for Overload (Q Spell) in KillSteal
        public static bool KillstealQ()
        {
            return MyHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Rune Prison (W Spell) in KillSteal
        public static bool KillstealW()
        {
            return MyHarass["killsteal.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Spell Flux (E Spell) in KillSteal
        public static bool KillstealE()
        {
            return MyHarass["killsteal.E"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region AutoWard
        //Checkbox for Use Autoward in Activator
        public static bool checkWard()
        {
            return MyActivator["checkward"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use PinkWard in Activator
        public static bool pinkWard()
        {
            return MyActivator["pinkvision"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use GSTotem in Activator
        public static bool greaterStealthTotem()
        {
            return MyActivator["greaterstealthtotem"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use GVTotem in Activator
        public static bool greaterVisionTotem()
        {
            return MyActivator["greatervisiontotem"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use Blue Trinket in Activator
        public static bool farsightAlteration()
        {
            return MyActivator["farsightalteration"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use Totem in Activator
        public static bool wardingTotem()
        {
            return MyActivator["wardingtotem"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region Items Cast
        //Checkbox for Use Seraph Embrace in Activator
        public static bool Seraph()
        {
            return MyActivator["seraph"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Use Seraph Embrace if under x HP in Activator
        public static float SeraphHP()
        {
            return MyActivator["seraph.HP"].Cast<Slider>().CurrentValue;
        }
        //Slider for Use Seraph Embrace if counts more then y Enemies in Activator
        public static float SeraphEnemies()
        {
            return MyActivator["seraph.Enemies"].Cast<Slider>().CurrentValue;
        }
        //Checkbox for Use Zhonya's Hourglass in Activator
        public static bool Zhonya()
        {
            return MyActivator["zhonya"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Use Zhonya's Hourglass if under x HP in Activator
        public static float ZhonyaHP()
        {
            return MyActivator["zhonya.HP"].Cast<Slider>().CurrentValue;
        }
        //Slider for Use Zhonya's Hourglass if counts more then y Enemies in Activator
        public static float ZhonyaEnemies()
        {
            return MyActivator["zhonya.Enemies"].Cast<Slider>().CurrentValue;
        }
        //Checkbox for Use Potions in Activator
        public static bool SpellsPotionsCheck()
        {
            return MyActivator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Use Potions if under x HP in Activator
        public static float SpellsPotionsHP()
        {
            return MyActivator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }
        //Slider for Use Potions if under y Mana in Activator
        public static float SpellsPotionsM()
        {
            return MyActivator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Spells Usage
        //Slider for Use Heal Spell if under x HP in Activator
        public static float SpellsHealHp()
        {
            return MyActivator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }
        //Slider for Use Ignite as KillSteal in Activator
        public static float SpellsIgniteFocus()
        {
            return MyActivator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        //Slider for Use Barrier if under x HP in Activator
        public static float spellsBarrierHP()
        {
            return MyActivator["spells.Barrier.Hp"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Skin Editor
        //Checkbox for Use Skin.ID in Activator
        public static bool checkSkin()
        {
            return MyOtherFunctions["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Change the Skin.ID in Activator
        public static int SkinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region LevelUp
        //Checkbox for Autolevelup function in Activator
        public static bool Lvlup()
        {
            return MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region Flee
        //Slider for Use Desperate Power (R Spell) if more then x Mana in Activator
        public static float FleeM()
        {
            return MyOtherFunctions["flee.M"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region GapCloser
        //Checkbox for Use Rune Prison (W Spell) for the Anti-Gapcloser
        public static bool gapcloserW()
        {
            return MyOtherFunctions["gapcloser.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Use Desperate Power (R Spell) for the Anti-Gapcloser
        public static bool gapcloserR()
        {
            return MyOtherFunctions["gapcloser.R"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region AutoPassive Stacks
        //Checkbox for Use AutoStack Passive function in PermaActive
        public static bool passiveS()
        {
            return MyOtherFunctions["passiveS"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Limit Auto-Passive Stacks in PermaActive
        public static bool passiveS1()
        {
            return MyOtherFunctions["passiveS1"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit Auto-Passive Stacks in PermaActive
        public static float passiveS2()
        {
            return MyOtherFunctions["passiveS2"].Cast<Slider>().CurrentValue;
        }
        //Slider for Limit Auto-Passive Stacks if mana is higher then x in PermaActive
        public static float passiveS3()
        {
            return MyOtherFunctions["passiveS3"].Cast<Slider>().CurrentValue;
        }
        #endregion
    }
}
