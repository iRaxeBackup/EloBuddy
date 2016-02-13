using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MissInopportune
{
    internal static class MissInopportuneMenu
    {
        private static Menu MyMenu;
        public static Menu MyCombo, MyDraw, MyHarass, MyActivator, MyFarm, MyOtherFunctions;

        public static void LoadMenu()
        {
            MyMissInopportunePage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        private static void MyMissInopportunePage()
        {
            MyMenu = MainMenu.AddMenu("MissInopportune", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" MissInopportune - " + Program.Version);
            MyMenu.AddLabel(" Made by -iRaxe");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Use SpaceBar for Combo");
            MyMenu.AddLabel(" - Use the key V For LaneClear/JungleClear");
            MyMenu.AddLabel(" - Use the key T For Flee");
        }

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
                new CheckBox("Draw Double Up Range (Q Spell)"));
            MyDraw.Add("draw.W",
                new CheckBox("Draw Strut Range (W Spell)"));
            MyDraw.Add("draw.E",
                new CheckBox("Draw Make it Rain Range (E Spell)"));
            MyDraw.Add("draw.R",
                new CheckBox("Draw Bullet Time Range (R Spell)"));
            MyDraw.Add("draw.T",
                new CheckBox("Draw Wards Position"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxeses if you wish to dont see a specific draw");
        }

        private static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.Add("combo.Q",
                new CheckBox("Use Double Up (Q Spell)"));
            MyCombo.Add("combo.w",
                new CheckBox("Use Strut (W Spell)"));
            MyCombo.AddLabel("Use Make it Rain (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.e" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.Add("combo.R",
                new CheckBox("Use Bullet Time (R Spell)"));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo preferences:");
            MyCombo.Add("combo.Q1",
                new CheckBox("Use Double Up as Extended (Q Spell)"));
            MyCombo.Add("combo.R1",
                new CheckBox("Use Bullet Time (R Spell) on CC"));
            MyCombo.Add("combo.CC",
                new CheckBox("Use Make it Rain (E Spell) on CC"));
            MyCombo.Add("combo.REnemies",
                new Slider("Min Enemyes for Bullet Time OverKill (R Spell)", 3, 0, 5));
            MyCombo.Add("combo.W1",
                new Slider("Max enemies for the Strut (W Spell)", 3, 0, 5));
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }

        private static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q",
                new CheckBox("Use Double Up (Q Spell)"));
            MyFarm.Add("lc.Q1",
                new CheckBox("Use Double Up LastHit (Q Spell)"));
            MyFarm.Add("lc.W",
                new CheckBox("Use Strut (W Spell)", false));
            MyFarm.AddSeparator();
            MyFarm.Add("lc.E",
                new CheckBox("Use Make it Rain (E Spell)", false));
            MyFarm.Add("lc.E1",
                new Slider("Min. Minions for Make it Rain (E Spell", 3, 0, 10));
            MyFarm.Add("lc.QWE",
                new Slider("Min. Mana for Laneclear Spells %", 30));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q",
                new CheckBox("Use Double Up in Jungle (Q Spell)"));
            MyFarm.Add("jungle.W",
                new CheckBox("Use Strut in Jungle (W Spell)"));
            MyFarm.Add("jungle.E",
                new CheckBox("Use Make it Rain in Jungle (E Spell)"));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }

        private static void MyHarassPage()
        {
            MyHarass = MyMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            MyHarass.AddGroupLabel("Harass Settings:");
            MyHarass.AddSeparator();
            MyHarass.Add("harass.Q",
                new CheckBox("Use Double Up (Q Spell)"));
            MyHarass.Add("harass.Q1",
                new CheckBox("Use Double Up as Extended (Q Spell)"));
            MyHarass.AddLabel("Use Make it Rain (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.Add("harass.QE",
                new Slider("Min. Mana for Harass Spells %", 35));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Settings:");
            MyHarass.Add("killsteal.Q",
                new CheckBox("Use Double Up (Q Spell)", false));
            MyHarass.Add("killsteal.E",
                new CheckBox("Use Make it Rain (E Spell)"));
            MyHarass.AddLabel("Use KillSteal spells on");
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Pro Tips");
            MyHarass.AddLabel(" -Remember to play safe and don't be a teemo");
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
            MyActivator.Add("bilgewater",
                new CheckBox("Use Bilgewater Cutlass"));
            MyActivator.Add("bilgewater.HP",
                new Slider("Use Bilgewater Cutlass if hp is lower than {0}(%)", 60));
            MyActivator.AddSeparator();
            MyActivator.Add("botrk",
                new CheckBox("Use Blade of The Ruined King"));
            MyActivator.Add("botrk.HP",
                new Slider("Use Blade of The Ruined King if hp is lower than {0}(%)", 60));
            MyActivator.AddSeparator();
            MyActivator.Add("youmus",
                new CheckBox("Use Youmus Ghostblade"));
            MyActivator.Add("items.Youmuss.HP",
                new Slider("Use Youmuss Ghostblade if hp is lower than {0}(%)", 60, 1));
            MyActivator.Add("youmus.Enemies",
                new Slider("Use Youmus Ghostblade when there are {0} enemies in range", 3, 1, 5));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Potion Settings");
            MyActivator.Add("spells.Potions.Check",
                new CheckBox("Use Potions"));
            MyActivator.Add("spells.Potions.HP",
                new Slider("Use Potions when HP is lower than {0}(%)", 60, 1));
            MyActivator.Add("spells.Potions.Mana",
                new Slider("Use Potions when Mana is lower than {0}(%)", 60, 1));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Spells settings:");
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

        private static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Settings for Flee");
            MyOtherFunctions.Add("flee.HP",
                new Slider("Use Strut (W Spell) for flee if HP is lower than {0}(%)", 10, 1));
            MyOtherFunctions.Add("flee.M",
                new Slider("Use Strut for Flee if mana is higher than {0}(%)", 10, 1));
            MyOtherFunctions.AddGroupLabel("Anti Gap Closer/Interrupt");
            MyOtherFunctions.Add("gapcloser.E",
                new CheckBox("Use Make it Rain (E Spell)"));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup",
                new CheckBox("Auto Level Up Spells:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("checkSkin",
                new CheckBox("Use skin changer:"));
            MyOtherFunctions.Add("skin.Id",
                new Slider("Skin Editor", 5, 0, 10));
        }

        public static bool Nodraw()
        {
            return MyDraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool OnlyReady()
        {
            return MyDraw["onlyReady"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return MyDraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsW()
        {
            return MyDraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsE()
        {
            return MyDraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsR()
        {
            return MyDraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }
        public static bool DrawingsT()
        {
            return MyDraw["draw.T"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQ()
        {
            return MyCombo["combo.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQ1()
        {
            return MyCombo["combo.Q1"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboW()
        {
            return MyCombo["combo.W"].Cast<CheckBox>().CurrentValue;
        }

        public static float ComboW1()
        {
            return MyCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        public static float ComboREnemies()
        {
            return MyCombo["combo.REnemies"].Cast<Slider>().CurrentValue;
        }

        public static bool ComboR1()
        {
            return MyCombo["combo.R1"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboR()
        {
            return MyCombo["combo.R"].Cast<CheckBox>().CurrentValue;
        }
        public static bool LcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcQ1()
        {
            return MyFarm["lc.Q1"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcW()
        {
            return MyFarm["lc.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcE()
        {
            return MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }

        public static float LcE1()
        {
            return MyFarm["lc.E1"].Cast<Slider>().CurrentValue;
        }

        public static float LcW1()
        {
            return MyFarm["lc.W1"].Cast<Slider>().CurrentValue;
        }

        public static float LcM()
        {
            return MyFarm["lc.QWE"].Cast<Slider>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleW()
        {
            return MyFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleE()
        {
            return MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassQ()
        {
            return MyHarass["harass.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassQ1()
        {
            return MyHarass["harass.Q1"].Cast<CheckBox>().CurrentValue;
        }


        public static float HarassQe()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealE()
        {
            return MyHarass["killsteal.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealQ()
        {
            return MyHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Bilgewater()
        {
            return MyActivator["bilgewater"].Cast<CheckBox>().CurrentValue;
        }

        public static float BilgewaterHp()
        {
            return MyActivator["bilgewater.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Botrk()
        {
            return MyActivator["botrk"].Cast<CheckBox>().CurrentValue;
        }

        public static float BotrkHp()
        {
            return MyActivator["botrk.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Youmus()
        {
            return MyActivator["youmus"].Cast<CheckBox>().CurrentValue;
        }
        public static bool checkWard()
        {
            return MyActivator["checkward"].Cast<CheckBox>().CurrentValue;
        }
        public static bool pinkWard()
        {
            return MyActivator["pinkvision"].Cast<CheckBox>().CurrentValue;
        }
        public static bool greaterStealthTotem()
        {
            return MyActivator["greaterstealthtotem"].Cast<CheckBox>().CurrentValue;
        }
        public static bool greaterVisionTotem()
        {
            return MyActivator["greatervisiontotem"].Cast<CheckBox>().CurrentValue;
        }
        public static bool farsightAlteration()
        {
            return MyActivator["farsightalteration"].Cast<CheckBox>().CurrentValue;
        }
        public static bool wardingTotem()
        {
            return MyActivator["wardingtotem"].Cast<CheckBox>().CurrentValue;
        }

        public static float YoumusEnemies()
        {
            return MyActivator["youmus.Enemies"].Cast<Slider>().CurrentValue;
        }

        public static float ItemsYoumuShp()
        {
            return MyActivator["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool SpellsPotionsCheck()
        {
            return MyActivator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }
        public static float SpellsPotionsHP()
        {
            return MyActivator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsPotionsM()
        {
            return MyActivator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsHealHp()
        {
            return MyActivator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsIgniteFocus()
        {
            return MyActivator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
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

        public static int SkinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }

        public static bool Lvlup()
        {
            return MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserE()
        {
            return MyOtherFunctions["gapcloser.E"].Cast<CheckBox>().CurrentValue;
        }


        public static float FleeM()
        {
            return MyOtherFunctions["flee.M"].Cast<Slider>().CurrentValue;
        }

        public static float FleeHP()
        {
            return MyOtherFunctions["flee.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool checkSkin()
        {
            return MyOtherFunctions["checkSkin"].Cast<CheckBox>().CurrentValue;
        }


    }
}
