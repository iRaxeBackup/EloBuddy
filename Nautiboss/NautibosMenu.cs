using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nautilus
{
    class NautibosMenu
    {
        public static Menu nautidum, nauticombo, nautidraw, nautir, itemsMenu, smitePage, spellsPage, nautilcs, miscmenu;
        public static void loadMenu()
        {
            nautidudepage();
            nautidrawpage();
            nauticombopage();
            nautilcspage();
            nautirpage();
            Activator();
            miscpage();


        }



        public static void nautidudepage()
        {
            nautidum = MainMenu.AddMenu("Nautiboss", "main");
            nautidum.AddGroupLabel("About this script");
            nautidum.AddLabel(" Nautiboss - " + Program.version);
            nautidum.AddLabel(" Made by -iRaxe");
            nautidum.AddSeparator();
            nautidum.AddGroupLabel("Hotkeys");
            nautidum.AddLabel(" - Use SpaceBar for Combo");
            nautidum.AddLabel(" - Use the key V For LaneClear/JungleClear");
            nautidum.AddLabel(" - Use the key T For Flee");

        }

        public static void nautidrawpage()
        {
            nautidraw = nautidum.AddSubMenu("Draw  settings", "Draw");
            nautidraw.AddGroupLabel("Draw Settings");
            nautidraw.Add("nodraw", new CheckBox("No Display Drawing", false));
            nautidraw.Add("onlyReady", new CheckBox("Display only Ready", true));
            nautidraw.AddSeparator();
            nautidraw.Add("draw.Q", new CheckBox("Draw Q Range", true));
            nautidraw.Add("draw.W", new CheckBox("Draw W Range", true));
            nautidraw.Add("draw.E", new CheckBox("Draw E Range", true));
            nautidraw.Add("draw.R", new CheckBox("Draw R Range", true));
            nautidraw.AddSeparator();
            nautidraw.AddGroupLabel("Combo Damage Draw");
            nautidraw.Add("draw.combo.q", new CheckBox("Use Q Damage to Calculate"));
            nautidraw.Add("draw.combo.e", new CheckBox("Use E Damage to Calculate"));
            nautidraw.Add("draw.combo.aa", new Slider("Use {0} AA to Calculate", 2, 1, 5));
            nautidraw.AddSeparator();
            nautidraw.AddLabel("HP info Ally");
            nautidraw.Add("allyhp", new CheckBox("Draw HP info"));
            nautidraw.AddSeparator();
            nautidraw.AddGroupLabel("Pro Tips");
            nautidraw.AddLabel(" - Uncheck the boxes if you wish to dont see a specific spell draw");
        }

        public static void nauticombopage()
        {
            nauticombo = nautidum.AddSubMenu("Combo settings", "Combo");
            nauticombo.AddGroupLabel("Combo settings");
            nauticombo.Add("combo.Q", new CheckBox("Use Q Spell"));
            nauticombo.Add("combo.W", new CheckBox("Use W Spell"));
            nauticombo.Add("combo.E", new CheckBox("Use E Spell"));
            nauticombo.Add("combo.R", new CheckBox("Use R Spell"));
            nauticombo.Add("hitChanceQ", new Slider("Hitchance for Q", 2, 1, 3));
            nauticombo.Add("hptoult", new Slider("Mim %HP to use R", 50, 0, 100));
            nauticombo.AddSeparator();
            nauticombo.AddGroupLabel("Pro Tips");
            nauticombo.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");

        }
        public static void nautilcspage()
        {
            nautilcs = nautidum.AddSubMenu("Lane Clear Settings", "laneclear");
            nautilcs.AddGroupLabel("Lane clear settings");
            nautilcs.Add("lc.E", new CheckBox("Use E Spell", false));
            nautilcs.Add("lc.Mana", new Slider("Min. Mana%", 30));
            nautilcs.Add("lc.MinionsW", new Slider("Min. Minions for E ", 3, 0, 10));
            nautilcs.AddSeparator();
            nautilcs.AddGroupLabel("Jungle Settings");
            nautilcs.Add("jungle.Q", new CheckBox("Use Q Spell in Jungle"));
            nautilcs.Add("jungle.W", new CheckBox("Use E Spell in Jungle"));
            nautilcs.Add("jungle.E", new CheckBox("Use W Spell in Jungle "));
            nautilcs.AddSeparator();
            nautilcs.AddGroupLabel("Pro Tips");
            nautilcs.AddLabel(" -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");

        }
        public static void nautirpage()
        {
            nautir = nautidum.AddSubMenu("Ultimate Menu", "rlogic");
            nautir.AddGroupLabel("Ultimate Menu");
            nautir.AddSeparator();
            nautir.AddLabel("Use ultimate on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                nautir.Add("r.ult" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName, false));
            }
            nautir.Add("manu.ult", new CheckBox("Use R Manual", false));
            nautir.AddGroupLabel("Pro Tips");
            nautir.AddLabel(" -Remember to play safe and don't be a teemo");
        }
        public static void Activator()
        {
            itemsMenu = nautidum.AddSubMenu("Items Settings", "Items");
            itemsMenu.AddGroupLabel("Items usage");
            itemsMenu.AddSeparator();
            itemsMenu.AddLabel("Talisman of Ascension");
            itemsMenu.Add("talisman", new CheckBox("Use Talisman of Ascension"));
            itemsMenu.AddSeparator();
            itemsMenu.AddLabel("Randuin");
            itemsMenu.Add("randuin", new CheckBox("Use Randuin"));
            smitePage = nautidum.AddSubMenu("Smite Settings", "Smite");
            smitePage.AddGroupLabel("Smite settings");
            smitePage.AddSeparator();
            smitePage.Add("SRU_Red", new CheckBox("Smite Red Buff"));
            smitePage.Add("SRU_Blue", new CheckBox("Smite Blue Buff"));
            smitePage.Add("SRU_Dragon", new CheckBox("Smite Dragon"));
            smitePage.Add("SRU_Baron", new CheckBox("Smite Baron"));
            smitePage.Add("SRU_Gromp", new CheckBox("Smite Gromp"));
            smitePage.Add("SRU_Murkwolf", new CheckBox("Smite Wolf"));
            smitePage.Add("SRU_Razorbeak", new CheckBox("Smite Bird"));
            smitePage.Add("SRU_Krug", new CheckBox("Smite Golem"));
            smitePage.Add("Sru_Crab", new CheckBox("Smite Crab"));
            smitePage.AddSeparator();
            spellsPage = nautidum.AddSubMenu("Spells Settings");
            spellsPage.AddGroupLabel("Spells settings");
            spellsPage.Add("useexhaust", new CheckBox("Use Exhaust"));
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                spellsPage.Add(source.ChampionName + "exhaust",
                    new CheckBox("Exhaust " + source.ChampionName, false));
            }
            spellsPage.AddSeparator();
            spellsPage.AddGroupLabel("Heal settings");
            spellsPage.Add("spells.Heal.Hp", new Slider("Use Heal when HP is lower than {0}(%)", 30, 1, 100));
            spellsPage.AddGroupLabel("Ignite settings");
            spellsPage.Add("spell.Ignite.Use", new CheckBox("Use Ignite for KillSteal"));
            spellsPage.Add("spells.Ignite.Focus", new Slider("Use Ignite when target HP is lower than {0}(%)", 10, 1, 100));
            spellsPage.Add("spells.Ignite.Kill", new CheckBox("Use ignite if killable"));


        }
        public static void miscpage()
        {
            miscmenu = nautidum.AddSubMenu("Misc Menu", "othermenu");
            miscmenu.AddGroupLabel("Level Up Function");
            miscmenu.Add("lvlup", new CheckBox("Auto Level Up Spells as SUPPORT", false));
            miscmenu.AddSeparator();
            miscmenu.AddGroupLabel("Interrupter settings");
            miscmenu.Add("interruptq", new CheckBox("Use Q Spell to Interrupt"));
            miscmenu.Add("interruptr", new CheckBox("Use R Spell to Interrupt"));
            miscmenu.AddSeparator();
            miscmenu.AddGroupLabel("Skin settings");
            miscmenu.Add("skin.Id", new Slider("Skin Editor", 3, 1, 4));
        }
        public static bool interruptq()
        {
            return miscmenu["interruptq"].Cast<CheckBox>().CurrentValue;
        }
        public static bool interruptr()
        {
            return miscmenu["interruptr"].Cast<CheckBox>().CurrentValue;
        }
        public static int HitchanceQ
        {
            get { return nauticombo["hitchanceQ"].Cast<Slider>().CurrentValue; }
        }
        public static bool Allydrawn()
        {
            return nautidraw["allyhp"].Cast<CheckBox>().CurrentValue;
        }
        public static float minQaggresive()
        {
            return miscmenu["combo.QminAG"].Cast<Slider>().CurrentValue;
        }
        public static float minQcombo()
        {
            return miscmenu["combo.Qmin"].Cast<Slider>().CurrentValue;
        }
        public static int skinId()
        {
            return miscmenu["skin.Id"].Cast<Slider>().CurrentValue;
        }


        //
        public static float spellsHealignite()
        {
            return spellsPage["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }
        public static bool spellsIgniteOnlyHpLow()
        {
            return spellsPage["spells.Ignite.Kill"].Cast<CheckBox>().CurrentValue;
        }
        public static bool spellsUseIgnite()
        {
            return spellsPage["spells.Ignite.Use"].Cast<CheckBox>().CurrentValue;
        }
        public static float spellsHealhp()
        {
            return spellsPage["spells.Heal.Hp"].Cast<Slider>().CurrentValue;
        }



        public static int rLogicMinHp()
        {
            return nautir["rlogic.minhp"].Cast<Slider>().CurrentValue;
        }
        public static int rLogicEnemyMinHp()
        {
            return nautir["rlogic.ehp"].Cast<Slider>().CurrentValue;
        }
        public static bool useWjungle()
        {
            return nautilcs["jungle.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useEjungle()
        {
            return nautilcs["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useQjungle()
        {
            return nautilcs["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useQ()
        {
            return nautilcs["combo.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useW()
        {
            return nautilcs["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useE()
        {
            return nautilcs["combo.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool nodraw()
        {
            return nautidraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }
        public static bool onlyReady()
        {
            return nautidraw["onlyready"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsQ()
        {
            return nautidraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsW()
        {
            return nautidraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsE()
        {
            return nautidraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool drawingsR()
        {
            return nautidraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool useWlc()
        {
            return nautilcs["lc.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool useElc()
        {
            return nautilcs["lc.E"].Cast<CheckBox>().CurrentValue;
        }
    }
}



