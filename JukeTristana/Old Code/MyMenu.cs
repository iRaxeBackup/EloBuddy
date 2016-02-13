using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Boostana
{
    internal static class TristanaMenu
    {
        private static Menu MyMenu;
        public static Menu MyCombo, MyDraw, MyHarass, MyActivator, MyFarm, MyOtherFunctions;

        public static void LoadMenu()
        {
            MyTristanaPage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        private static void MyTristanaPage()
        {
            MyMenu = MainMenu.AddMenu("Boostana", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" Boostana - " + Program.Version);
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
                new CheckBox("Draw Rapid Fire Range (Q Spell)"));
            MyDraw.Add("draw.W", 
                new CheckBox("Draw Rocket Jump Range (W Spell)"));
            MyDraw.Add("draw.E", 
                new CheckBox("Draw Explosive Charge Range (E Spell)"));
            MyDraw.Add("draw.R", 
                new CheckBox("Draw Buster Shot Range (R Spell)"));
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
                new CheckBox("Use Rapid Fire (Q Spell)"));
            MyCombo.AddLabel("Use Rocket Jump (W Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.w" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName, false));
            }
            MyCombo.Add("combo.wh", new Slider("Set your prediction value", 70));
            MyCombo.AddLabel("Use Explosive Charge (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.e" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddLabel("Use Buster Shot (R Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.r" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo preferences:");
            MyCombo.Add("combo.CC", 
                new CheckBox("Use Explosive Charge (E Spell) on CC"));
            MyCombo.Add("combo.ER", 
                new CheckBox("Use Explosive Charge + Buster Shot Finisher"));
            MyCombo.Add("combo.ER1", 
                new Slider("Explosive Charge + Buster Shot Overkill", 60, 0, 500));
            MyCombo.Add("combo.R1", 
                new Slider("Buster Shot OverKill", 50, 0, 500));
            MyCombo.Add("combo.W1", 
                new Slider("Max enemies for the Rocket Jump (W Spell)", 3, 0, 5));
            MyCombo.AddSeparator();
            MyCombo.Add("insecPositionMode", 
                new Slider("Insec Postion Mode", 2, 0, 2));
            MyCombo.Add("insecDistancee", 
                new Slider("Insec Distance", 200, 100, 350));
            MyCombo.AddSeparator();
            MyCombo.Add("combo.WR",
                new KeyBind("Use Rocket Jump + Buster Shot for Insec", false, KeyBind.BindTypes.HoldActive, 92));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }

        private static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q", 
                new CheckBox("Use Rapid Fire (Q Spell)"));
            MyFarm.AddSeparator();
            MyFarm.Add("lc.W", 
                new CheckBox("Use Rocket Jump (W Spell)", false));
            MyFarm.Add("lc.W1", 
                new Slider("Min. Minions for Rocket Jump", 3, 0, 10));
            MyFarm.AddSeparator();
            MyFarm.Add("lc.E", 
                new CheckBox("Use Explosive Charge (E Spell)", false));
            MyFarm.Add("lc.E1",
                new CheckBox("Use Explosive Charge (E Spell) on Tower", false));
            MyFarm.Add("lc.M", 
                new Slider("Min. Mana for Laneclear Spells %", 30));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", 
                new CheckBox("Use Rapid Fire in Jungle (Q Spell)"));
            MyFarm.Add("jungle.W", 
                new CheckBox("Use Rocket Jump in Jungle (W Spell)"));
            MyFarm.Add("jungle.E", 
                new CheckBox("Use Explosive Charge in Jungle (E Spell)"));
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
                new CheckBox("Use Rapid Fire (Q Spell)"));
            MyHarass.AddLabel("Use Explosive Charge (E Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.Add("harass.QE", 
                new Slider("Min. Mana for Harass Spells %", 35));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Settings:");
            MyHarass.Add("killsteal.W", 
                new CheckBox("Use Rocket Jump (W Spell)", false));
            MyHarass.Add("killsteal.R", 
                new CheckBox("Use Buster Shot (R Spell)"));
            MyHarass.AddLabel("Use KillSteal spells on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("killsteal.WR" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
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
                new Slider("Use Buster Shot (R Spell) for flee if HP is lower than {0}(%)", 10, 1));
            MyOtherFunctions.Add("flee.M", 
                new Slider("Use Rocket Jump for Flee if mana is higher than {0}(%)", 10, 1));
            MyOtherFunctions.AddGroupLabel("Anti Gap Closer/Interrupt");
            MyOtherFunctions.Add("gapcloser.R", 
                new CheckBox("Buster Shot (R Spell)"));
            MyOtherFunctions.Add("gapcloser.R1",
                new CheckBox("Buster Shot (R Spell) to Interrupt"));
            MyOtherFunctions.Add("gapcloser.R2", 
                new CheckBox("Buster Shot (R Spell) to Peel from Khazix"));
            MyOtherFunctions.Add("gapcloser.R3", 
                new CheckBox("Buster Shot (R Spell) to Peel from Rengar"));
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
            return MyDraw["onlyready"].Cast<CheckBox>().CurrentValue;
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

        public static float ComboW1()
        {
            return MyCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        public static float ComboWH()
        {
            return MyCombo["combo.wh"].Cast<Slider>().CurrentValue;
        }

        public static float ComboR1()
        {
            return MyCombo["combo.R1"].Cast<Slider>().CurrentValue;
        }

        public static bool ComboEr()
        {
            return MyCombo["combo.ER"].Cast<CheckBox>().CurrentValue;
        }

        public static float ComboEr1()
        {
            return MyCombo["combo.ER1"].Cast<Slider>().CurrentValue;
        }

        public static bool LcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcW()
        {
            return MyFarm["lc.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcE()
        {
            return MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcE1()
        {
            return MyFarm["lc.E1"].Cast<CheckBox>().CurrentValue;
        }

        public static float LcW1()
        {
            return MyFarm["lc.W1"].Cast<Slider>().CurrentValue;
        }

        public static float LcM()
        {
            return MyFarm["lc.M"].Cast<Slider>().CurrentValue;
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

        public static float HarassQe()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealW()
        {
            return MyHarass["killsteal.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealR()
        {
            return MyHarass["killsteal.R"].Cast<CheckBox>().CurrentValue;
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

        public static bool GapcloserR()
        {
            return MyOtherFunctions["gapcloser.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserR1()
        {
            return MyOtherFunctions["gapcloser.R1"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserR2()
        {
            return MyOtherFunctions["gapcloser.R2"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserR3()
        {
            return MyOtherFunctions["gapcloser.R3"].Cast<CheckBox>().CurrentValue;
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
