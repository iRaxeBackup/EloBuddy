using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Trollaloi
{
    class IllaoiMenu
    {
        public static Menu MyMenu, MyCombo, MyDraw, MyHarass, MyActivator, MySpells, MyFarm, MyOtherFunctions;
        public static void loadMenu()
        {
            MyIllaoiPage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        public static void MyIllaoiPage()
        {
            MyMenu = MainMenu.AddMenu("Trollaloi", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" Trollaloi - " + Program.version);
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
            MyDraw.Add("nodraw", 
                new CheckBox("No Display Drawing", false));
            MyDraw.Add("onlyReady", 
                new CheckBox("Display only Ready", true));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", 
                new CheckBox("Draw Tentacle Smash Range (Q Spell)", true));
            MyDraw.Add("draw.W",
                new CheckBox("Draw Harsh Lesson  Range (W Spell)", true));
            MyDraw.Add("draw.E",
                new CheckBox("Draw Test of Spirit Range (E Spell)", true));
            MyDraw.Add("draw.R", 
                new CheckBox("Draw Leap of Faith  Range (R Spell)", true));
            MyDraw.AddSeparator();
            MyDraw.AddLabel("Informations About the allies:");
            MyDraw.Add("allyhp", 
                new CheckBox("Draw HP info"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxeses if you wish to dont see a specific draw");
        }

        public static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo settings:");
            MyCombo.AddLabel("Use Tentacle Smash (Q Spell) on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName, false));
            }
            MyCombo.Add("combo.Q1",
                new CheckBox("Use Tentacle Smash on Ghost(Q Spell)"));
            MyCombo.Add("combo.qh", new Slider("Set your prediction value", 70));
            MyCombo.Add("combo.W",
                new CheckBox("Use Harsh Lesson  (W Spell)"));
            MyCombo.AddLabel("Use Test of Spirit (E Spell) on:");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.Add("combo.eh", new Slider("Set your prediction value", 70));
            MyCombo.Add("combo.R",
                new CheckBox("Use Leap of Faith  (R Spell)"));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo preferences:");
            MyCombo.Add("combo.W1", 
                new Slider("Min enemies for the Harsh Lesson  (W Spell)", 3, 0, 5));
            MyCombo.Add("combo.R2",
                new Slider("Min enemies for the Leap of Faith  (R Spell)", 3, 0, 5));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }
        public static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("Lane clear settings:");
            MyFarm.Add("lc.Q", 
                new CheckBox("Use Tentacle Smash (Q Spell)"));
            MyFarm.Add("lc.Q1",
                new Slider("Min. Minions for Tentacle Smash ", 3, 0, 10));
            MyFarm.Add("lc.W",
                new CheckBox("Use Harsh Lesson (W Spell)", false));
            MyFarm.Add("lc.W1",
                new Slider("Min. Minions for Harsh Lesson ", 3, 0, 10));
            MyFarm.Add("lc.E",
                new CheckBox("Use Test of Spirit (E Spell)", false));
            MyFarm.Add("lc.E2",
                new Slider("Min. Minions for Test of Spirit ", 3, 0, 10));
            MyFarm.Add("lc.M",
                new Slider("Min. Mana for Laneclear Spells %", 30));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Jungle Settings");
            MyFarm.Add("jungle.Q", 
                new CheckBox("Use Tentacle Smash in Jungle (Q Spell)"));
            MyFarm.Add("jungle.W", 
                new CheckBox("Use Harsh Lesson  in Jungle (W Spell)"));
            MyFarm.Add("jungle.E", 
                new CheckBox("Use Test of Spirit in Jungle (E Spell)"));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }
        public static void MyHarassPage()
        {
            MyHarass = MyMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            MyHarass.AddGroupLabel("Harass Settings:");
            MyCombo.AddLabel("Use Tentacle Smash (Q Spell) on:");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.Add("harass.qh", new Slider("Set your prediction value", 70));
            MyHarass.Add("harass.Q1",
                new CheckBox("Use Tentacle Smash on Ghost(Q Spell)"));
            MyHarass.AddLabel("Use Test of Spirit (E Spell) on:");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.Add("harass.eh", new Slider("Set your prediction value", 70));
            MyHarass.Add("harass.QE",
                new Slider("Min. Mana for Harass Spells %", 35, 0, 100));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Settings:");
            MyHarass.Add("killsteal.Q",
                new CheckBox("Use Tentacle Smash (Q Spell)", false));
            MyHarass.Add("killsteal.qh", new Slider("Set your prediction value", 70));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Pro Tips");
            MyHarass.AddLabel(" -Remember to play safe and don't be a teemo");
        }
        public static void MyActivatorPage()
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
            MyActivator.AddLabel("Items avaible to use with Activator:");
            MyActivator.Add("talisman", 
                new CheckBox("Use Talisman of Ascension"));
            MyActivator.Add("randuin",
                new CheckBox("Use Randuin"));
            MyActivator.Add("glory", 
                new CheckBox("Use Righteous Glory"));
            MyActivator.Add("fotmountain", 
                new CheckBox("Use Face of the Mountain"));
            MyActivator.Add("mikael",
                new CheckBox("Use Mikaels Crucible"));
            MyActivator.Add("ironsolari",
                new CheckBox("Use Locket of the Iron Solari"));
            MyActivator.Add("items.sliderHP",
                new Slider("Use items when HP is lower than {0}(%)", 30, 1, 100));
            MyActivator.Add("items.enemiesinrange",
                new Slider("Use items when there are {0} enemies in range", 3, 1, 5));
            MyActivator.Add("bilgewater", 
                new CheckBox("Use Bilgewater Cutlass"));
            MyActivator.Add("bilgewater.HP",
                new Slider("Use Bilgewater Cutlass if hp is lower than {0}(%)", 60, 0, 100));
            MyActivator.Add("botrk", 
                new CheckBox("Use Blade of The Ruined King"));
            MyActivator.Add("botrk.HP", 
                new Slider("Use Blade of The Ruined King if hp is lower than {0}(%)", 60, 0, 100));
            MyActivator.Add("youmus", 
                new CheckBox("Use Youmus Ghostblade"));
            MyActivator.Add("items.Youmuss.HP",
                new Slider("Use Youmuss Ghostblade if hp is lower than {0}(%)", 60, 1, 100));
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
            MySpells = MyMenu.AddSubMenu("Spells Settings");
            MySpells.AddSeparator();
            MySpells.AddGroupLabel("Smite settings");
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
            MySpells.AddGroupLabel("Spells settings:");
            MySpells.AddSeparator();
            MySpells.AddGroupLabel("Heal settings:");
            MySpells.Add("spells.Heal.Hp", 
                new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            MySpells.AddGroupLabel("Ignite settings:");
            MySpells.Add("spells.Ignite.Focus", 
                new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1, 100));
        }
        public static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Anti Gap Closer/Interrupt");
            MyOtherFunctions.Add("gapcloser.Q", 
                new CheckBox("Use Tentacle Smash (Q Spell)"));
            MyOtherFunctions.Add("gapcloser.W",
                new CheckBox("Use Harsh Lesson (W Spell) as Gap", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Level Up Function");
            MyOtherFunctions.Add("lvlup", 
                new CheckBox("Auto Level Up Spells:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin settings");
            MyOtherFunctions.Add("skin.Id", 
                new Slider("Skin Editor", 1, 0, 2));
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
        public static bool Allydrawn()
        {
            return MyDraw["allyhp"].Cast<CheckBox>().CurrentValue;
        }
        public static bool comboQ1()
        {
            return MyCombo["combo.Q1"].Cast<CheckBox>().CurrentValue;
        }
        public static float comboQH()
        {
            return MyCombo["combo.qh"].Cast<Slider>().CurrentValue;
        }
        public static bool comboW()
        {
            return MyCombo["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        public static float comboEH()
        {
            return MyCombo["combo.eh"].Cast<Slider>().CurrentValue;
        }
        public static float comboW1()
        {
            return MyCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        public static bool comboR()
        {
            return MyCombo["combo.R"].Cast<CheckBox>().CurrentValue;
        }
        public static float comboR2()
        {
            return MyCombo["combo.R2"].Cast<Slider>().CurrentValue;
        }
        public static bool lcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool lcW()
        {
            return MyFarm["lc.W"].Cast<CheckBox>().CurrentValue;
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
        public static float lcW1()
        {
            return MyFarm["lc.W1"].Cast<Slider>().CurrentValue;
        }
        public static float lcM()
        {
            return MyFarm["lc.M"].Cast<Slider>().CurrentValue;
        }
        public static bool jungleQ()
        {
            return MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool jungleW()
        {
            return MyFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool jungleE()
        {
            return MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool harassQ1()
        {
            return MyHarass["harass.Q1"].Cast<CheckBox>().CurrentValue;
        }
        public static float harassQE()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }
        public static float harassQH()
        {
            return MyHarass["harass.qh"].Cast<Slider>().CurrentValue;
        }
        public static bool killstealQ()
        {
            return MyHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
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
        public static bool talisman()
        {
            return MyActivator["talisman"].Cast<CheckBox>().CurrentValue;
        }
        public static bool randuin()
        {
            return MyActivator["randuin"].Cast<CheckBox>().CurrentValue;
        }
        public static bool glory()
        {
            return MyActivator["glory"].Cast<CheckBox>().CurrentValue;
        }
        public static bool fotmountain()
        {
            return MyActivator["fotmountain"].Cast<CheckBox>().CurrentValue;
        }
        public static bool mikael()
        {
            return MyActivator["mikael"].Cast<CheckBox>().CurrentValue;
        }
        public static bool ironsolari()
        {
            return MyActivator["ironsolari"].Cast<CheckBox>().CurrentValue;
        }
        public static float itemssliderHP()
        {
            return MyActivator["items.sliderHP"].Cast<Slider>().CurrentValue;
        }
        public static float itemsenemiesinrange()
        {
            return MyActivator["items.enemiesinrange"].Cast<Slider>().CurrentValue;
        }
        public static bool bilgewater()
        {
            return MyActivator["bilgewater"].Cast<CheckBox>().CurrentValue;
        }
        public static float bilgewaterHP()
        {
            return MyActivator["bilgewater.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool botrk()
        {
            return MyActivator["botrk"].Cast<CheckBox>().CurrentValue;
        }
        public static float botrkHP()
        {
            return MyActivator["botrk.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool youmus()
        {
            return MyActivator["youmus"].Cast<CheckBox>().CurrentValue;
        }
        public static float youmusEnemies()
        {
            return MyActivator["youmus.Enemies"].Cast<Slider>().CurrentValue;
        }
        public static float itemsYOUMUShp()
        {
            return MyActivator["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }
        public static float checkhp()
        {
            return MyActivator["items.sliderHP"].Cast<Slider>().CurrentValue;
        }
        public static float spellsHealHP()
        {
            return MySpells["spells.Heal.HP"].Cast<Slider>().CurrentValue;
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
        public static bool gapcloserQ()
        {
            return MyOtherFunctions["gapcloser.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool gapcloserW()
        {
            return MyOtherFunctions["gapcloser.W"].Cast<CheckBox>().CurrentValue;
        }
    }
}
