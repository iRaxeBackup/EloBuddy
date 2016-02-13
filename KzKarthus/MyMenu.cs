using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace KzKarthus
{
    class KzKarthusMenu
    {
        public static Menu MyMenu, MyCombo, MyDraw, MyHarass, MySpells, MyFarm, MyOtherFunctions;
        public static void loadMenu()
        {
            MyKarthusPage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        public static void MyKarthusPage()
        {
            MyMenu = MainMenu.AddMenu("KzKarthus", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" KzKarthus - " + Program.version);
            MyMenu.AddLabel(" Made by -iRaxe & KzAshy");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Use SpaceBar for Combo");
            MyMenu.AddLabel(" - Use the key V For LaneClear/JungleClear");
        }

        public static void MyDrawPage()
        {
            MyDraw = MyMenu.AddSubMenu("Draw  settings", "Draw");
            MyDraw.AddGroupLabel("Draw Settings:");
            MyDraw.Add("nodraw", new CheckBox("No Display Drawing", false));
            MyDraw.Add("onlyReady", new CheckBox("Display only Ready", true));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", new CheckBox("Draw Lay Waste Range (Q Spell)", true));
            MyDraw.Add("draw.W", new CheckBox("Draw Wall of Pain  Range (W Spell)", true));
            MyDraw.Add("draw.E", new CheckBox("Draw Defile Range (E Spell)", true));
            MyDraw.Add("draw.R", new CheckBox("Draw Requiem Range (R Spell)", true));
            MyDraw.AddSeparator();
            MyDraw.AddLabel("Informations About the enemies:");
            MyDraw.Add("alert.R", new CheckBox("Draw R Notification when Ult can kill a Enemy"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific draw");
        }

        public static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.Add("combo.Q", new CheckBox("Use Lay Waste (Q Spell)"));
            MyCombo.Add("combo.W", new CheckBox("Use Wall of Pain  (W Spell)"));
            MyCombo.Add("combo.E", new CheckBox("Use Defile (E Spell)"));
            MyCombo.Add("combo.AC", new CheckBox("Use Automatic Combo if Death"));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo preferences:");
            MyCombo.Add("combo.W1", new Slider("Use Wall of Pain (W Spell) if mana is higher than {0}(%)", 60, 0, 100));
            MyCombo.Add("combo.E2", new Slider("Use Defile (E Spell) if mana is higher than {0}(%)", 60, 0, 100));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }
        public static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q", new CheckBox("Use Lay Waste (Q Spell)"));
            MyFarm.Add("lc.Q2", new CheckBox("Use Lay Waste (Q Spell) for Lasthit",false));
            MyFarm.Add("lc.Q1", new Slider("Min. Minions for Lay Waste ", 3, 0, 10));
            MyFarm.Add("lc.E", new CheckBox("Use Defile (E Spell)", false));
            MyFarm.Add("lc.E2", new Slider("Min. Minions for Defile ", 3, 0, 10));
            MyFarm.Add("lc.M", new Slider("Min. Mana for Laneclear Spells %", 30, 0, 100));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", new CheckBox("Use Lay Waste in Jungle (Q Spell)"));
            MyFarm.Add("jungle.E", new CheckBox("Use Defile (E Spell)"));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }
        public static void MyHarassPage()
        {
            MyHarass = MyMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            MyHarass.AddGroupLabel("Harass Settings:");
            MyHarass.Add("harass.Q", new CheckBox("Use Lay Waste (Q Spell)", false));
            MyHarass.Add("harass.E", new CheckBox("Use Defile (E Spell)", false));
            MyHarass.Add("harass.QE", new Slider("Min. Mana for Harass Spells %", 35, 0, 100));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Settings:");
            MyHarass.Add("killsteal.Q", new CheckBox("Use Lay Waste (Q Spell)", false));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Pro Tips");
            MyHarass.AddLabel(" -Remember to play safe and don't be a teemo");
        }
        public static void MyActivatorPage()
        {
            MySpells = MyMenu.AddSubMenu("Spells Settings");
            MySpells.AddSeparator();
            MySpells.AddGroupLabel("Heal settings:");
            MySpells.Add("spells.Heal.Hp", new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            MySpells.AddGroupLabel("Ignite settings:");
            MySpells.Add("spells.Ignite.Focus", new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1, 100));
            MySpells.AddGroupLabel("Barrier settings:");
            MySpells.Add("spells.Barrier.Hp", new Slider("Use Barrier when HP is lower than {0}(%)", 10, 1, 100));
            MySpells.AddGroupLabel("Zhonya settings:");
            MySpells.Add("spells.Zhonya.Check", new CheckBox("Use Zhonya", false));
            MySpells.Add("spells.Zhonya.Hp", new Slider("Use Zhonya when HP is lower than {0}(%)", 10, 1, 100));
            MySpells.Add("spells.Zhonya.Enemies", new Slider("Use Zhonya when there are min {0} enemies", 3, 1, 5));
        }
        public static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Anti Gap Closer/Interrupt");
            MyOtherFunctions.Add("gapcloser.W", new CheckBox("Use Wall of Pain  (W Spell)"));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup", new CheckBox("Auto Level Up Spells:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("skin.Id", new Slider("Skin Editor", 2, 0, 4));
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
        public static bool alertR()
        {
            return MyDraw["alert.R"].Cast<CheckBox>().CurrentValue;
        }
        public static bool comboQ()
        {
            return MyCombo["combo.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool comboW()
        {
            return MyCombo["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        public static float comboW1()
        {
            return MyCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        public static bool comboE()
        {
            return MyCombo["combo.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool comboAC()
        {
            return MyCombo["combo.AC"].Cast<CheckBox>().CurrentValue;
        }
        public static float comboE2()
        {
            return MyCombo["combo.E2"].Cast<Slider>().CurrentValue;
        }
        public static bool lcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool lcQ2()
        {
            return MyFarm["lc.Q2"].Cast<CheckBox>().CurrentValue;
        }
        public static bool lcE()
        {
            return MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }
        public static float lcE2()
        {
            return MyFarm["lc.E2"].Cast<Slider>().CurrentValue;
        }
        public static float lcQ1()
        {
            return MyFarm["lc.Q1"].Cast<Slider>().CurrentValue;
        }
        public static float lcM()
        {
            return MyFarm["lc.M"].Cast<Slider>().CurrentValue;
        }
        public static bool jungleQ()
        {
            return MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool jungleE()
        {
            return MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool harassQ()
        {
            return MyHarass["harass.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool harassE()
        {
            return MyHarass["harass.E"].Cast<CheckBox>().CurrentValue;
        }
        public static float harassQE()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }
        public static bool killstealQ()
        {
            return MyHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static float spellsHealHP()
        {
            return MySpells["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }
        public static float spellsBarrierHP()
        {
            return MySpells["spells.Barrier.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool SpellsZhonyaCheck()
        {
            return MySpells["spells.Zhonya.Check"].Cast<CheckBox>().CurrentValue;
        }
        public static float SpellsZhonyaHP()
        {
            return MySpells["spells.Zhonya.HP"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsZhonyaEnemies()
        {
            return MySpells["spells.Zhonya.Enemies"].Cast<Slider>().CurrentValue;
        }
        public static float spellsIgniteFocus()
        {
            return MySpells["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        public static int skinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }
        public static bool lvlup()
        {
            return MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue;
        }
        public static bool gapcloserW()
        {
            return MyOtherFunctions["gapcloser.W"].Cast<CheckBox>().CurrentValue;
        }
    }
}
