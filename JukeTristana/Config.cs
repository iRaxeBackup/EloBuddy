using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Boostana.MenuSettings;
using Color = System.Drawing.Color;

// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Boostana
{
    public static class Config
    {
        private static readonly string MenuName = "KA " + Player.Instance.ChampionName;

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("KA " + Player.Instance.ChampionName);
            Menu.AddLabel("Made By: iRaxe", 50);

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu
                DrawMenu,
                ComboMenu,
                HarassMenu,
                LaneclearMenu,
                LastHitMenu,
                OtherMenu;


            static Modes()
            {
                DrawMenu = Menu.AddSubMenu("Draw");
                Draw.Initialize();
                ComboMenu = Menu.AddSubMenu("Combo");
                Combo.Initialize();
                HarassMenu = Menu.AddSubMenu("Harass");
                Harass.Initialize();
                LaneclearMenu = Menu.AddSubMenu("Laneclear");
                LaneClear.Initialize();
                OtherMenu = Menu.AddSubMenu("Misc Functions");
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;
                private static readonly Slider _useWe;
                private static readonly CheckBox _useEcc;
                private static readonly CheckBox _useEr;
                private static readonly Slider _useErdmg;
                private static readonly Slider _useRdmg;
                private static readonly CheckBox _useRtower;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }

                public static float UseWe
                {
                    get { return _useWe.CurrentValue; }
                }

                public static bool UseEcc
                {
                    get { return _useEcc.CurrentValue; }
                }

                public static bool UseEr
                {
                    get { return _useEr.CurrentValue; }
                }

                public static float UseErdmg
                {
                    get { return _useErdmg.CurrentValue; }
                }

                public static float UseRdmg
                {
                    get { return _useRdmg.CurrentValue; }
                }

                public static bool UseRTower
                {
                    get { return _useRtower.CurrentValue; }
                }
                static Combo()
                {
                    // Initialize the menu values
                    ComboMenu.AddGroupLabel("Combo");
                    _useQ = ComboMenu.Add("comboQ", new CheckBox("Use Q"));
                    _useW = ComboMenu.Add("comboW", new CheckBox("Use W"));
                    _useE = ComboMenu.Add("comboE", new CheckBox("Use E"));
                    _useR = ComboMenu.Add("comboR", new CheckBox("Use R to kill"));
                    ComboMenu.AddSeparator();
                    ComboMenu.AddGroupLabel("Combo preferences:");
                    _useWe = ComboMenu.Add("comboWe", new Slider("Max enemies for Jump", 3, 0, 5));
                    _useEcc = ComboMenu.Add("comboCc", new CheckBox("Use E on CC"));
                    _useEr = ComboMenu.Add("comboEr", new CheckBox("Use E+R Finisher"));
                    _useErdmg = ComboMenu.Add("comboErdmg", new Slider("E + R Overkill", 60, 0, 500));
                    _useRdmg = ComboMenu.Add("comboRdmg", new Slider("R Overkill", 50, 0, 500));
                    _useRtower = ComboMenu.Add("comboRtower", new CheckBox("Use R to throw the enemy under ally tower"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;
                private static readonly Slider _useQemana;


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static float UseQemana
                {
                    get { return _useQemana.CurrentValue; }
                }

                static Harass()
                {
                    // Initialize the menu values
                    HarassMenu.AddGroupLabel("Harass");
                    _useQ = HarassMenu.Add("harassQ", new CheckBox("Use Q"));
                    _useE = HarassMenu.Add("harassE", new CheckBox("Use E"));
                    _useQemana = HarassMenu.Add("harassQe", new Slider("Min. Mana for Harass Spells %", 35));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _useWminions;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useEtower;
                private static readonly Slider _useQWEmana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static float UseWminions
                {
                    get { return _useWminions.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static bool UseEtower
                {
                    get { return _useEtower.CurrentValue; }
                }

                public static float UseQWEmana
                {
                    get { return _useQWEmana.CurrentValue; }
                }

                static LaneClear()
                {
                    // Initialize the menu values
                    LaneclearMenu.AddGroupLabel("LaneClear");
                    _useQ = LaneclearMenu.Add("laneQ", new CheckBox("Use Q"));
                    _useW = LaneclearMenu.Add("laneW", new CheckBox("Use W"));
                    _useE = LaneclearMenu.Add("laneE", new CheckBox("Use E"));
                    LaneclearMenu.AddSeparator();
                    LaneclearMenu.AddGroupLabel("Laneclear Preferences:");
                    _useWminions = LaneclearMenu.Add("laneWminions", new Slider("Min. Minions for W", 3, 0, 10));
                    _useEtower = LaneclearMenu.Add("laneTower", new CheckBox("Use E on Tower"));
                    _useQWEmana = LaneclearMenu.Add("laneMana", new Slider("Min. Mana for Laneclear Spells %", 30));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly CheckBox _Wks;
                private static readonly CheckBox _Rks;
                private static readonly CheckBox _Rint;
                private static readonly CheckBox _Rgap;
                private static readonly CheckBox _fleeW;
                private static readonly CheckBox _fleeR;
                private static readonly CheckBox _lvlUp;

                public static bool Wks
                {
                    get { return _Wks.CurrentValue; }
                }

                public static bool Rks
                {
                    get { return _Rks.CurrentValue; }
                }

                public static bool RInt
                {
                    get { return _Rint.CurrentValue; }
                }

                public static bool RGap
                {
                    get { return _Rgap.CurrentValue; }
                }

                public static bool UseWFlee
                {
                    get { return _fleeW.CurrentValue; }
                }

                public static bool UseRFlee
                {
                    get { return _fleeR.CurrentValue; }
                }

                public static bool lvlUp
                {
                    get { return _lvlUp.CurrentValue; }
                }

                static Misc()
                {
                    // Initialize the menu values
                    OtherMenu.AddGroupLabel("Exclude E List");
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        OtherMenu.Add("dont e" + enemy.ChampionName,
                            new CheckBox("Don't use E on " + enemy.ChampionName, false));
                    }

                    OtherMenu.AddGroupLabel("Interrupt/Gapcloser/KillSteal");
                    _Rint = OtherMenu.Add("rint", new CheckBox("Use R On Interruptable Spell"));
                    _Rgap = OtherMenu.Add("rgap", new CheckBox("Use R On GapCloser"));
                    OtherMenu.AddGroupLabel("Flee");
                    _fleeW = OtherMenu.Add("_fleeW", new CheckBox("Use W to flee"));
                    _fleeR = OtherMenu.Add("_fleeR", new CheckBox("Use R when you`re low health"));
                    OtherMenu.AddGroupLabel("KillSteal & Misc");
                    _Wks = OtherMenu.Add("_wKs", new CheckBox("Killsteal with W", false));
                    _Rks = OtherMenu.Add("_rKs", new CheckBox("Killsteal with R"));
                    _lvlUp = OtherMenu.Add("_lvlUp", new CheckBox("Auto Level Up Spells"));
                }

                public static void Initialize()
                {
                }
            }
            public static class Draw
            {
                private static readonly CheckBox _drawHealth;
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _drawR;

                private static readonly CheckBox _drawReady;

                public static bool DrawHealth
                {
                    get { return _drawHealth.CurrentValue; }
                }

                public static bool DrawQ
                {
                    get { return _drawQ.CurrentValue; }
                }

                public static bool DrawW
                {
                    get { return _drawW.CurrentValue; }
                }

                public static bool DrawE
                {
                    get { return _drawE.CurrentValue; }
                }

                public static bool DrawR
                {
                    get { return _drawR.CurrentValue; }
                }

                public static bool DrawReady
                {
                    get { return _drawReady.CurrentValue; }
                }


                public static Color colorHealth
                {
                    get { return DrawMenu.GetColor("colorHealth"); }
                }

                public static Color colorQ
                {
                    get { return DrawMenu.GetColor("colorQ"); }
                }

                public static Color colorW
                {
                    get { return DrawMenu.GetColor("colorW"); }
                }

                public static Color colorE
                {
                    get { return DrawMenu.GetColor("colorE"); }
                }

                public static Color colorR
                {
                    get { return DrawMenu.GetColor("colorR"); }
                }

                public static float _widthQ
                {
                    get { return DrawMenu.GetWidth("widthQ"); }
                }

                public static float _widthW
                {
                    get { return DrawMenu.GetWidth("widthW"); }
                }

                public static float _widthE
                {
                    get { return DrawMenu.GetWidth("widthE"); }
                }

                public static float _widthR
                {
                    get { return DrawMenu.GetWidth("widthR"); }
                }

                static Draw()
                {
                    DrawMenu.AddGroupLabel("Draw");
                    _drawReady = DrawMenu.Add("drawReady", new CheckBox("Draw Only If The Spells Are Ready.", false));
                    DrawMenu.AddSeparator();
                    DrawMenu.AddLabel("Reload is required to aply the changes made in the damage indicator");
                    _drawHealth = DrawMenu.Add("drawHealth", new CheckBox("Draw Damage in HealthBar"));
                    DrawMenu.AddColorItem("colorHealth");
                    DrawMenu.AddSeparator();
                    //Q
                    _drawQ = DrawMenu.Add("drawQ", new CheckBox("Draw Q"));
                    DrawMenu.AddColorItem("colorQ");
                    DrawMenu.AddWidthItem("widthQ");
                    //W
                    _drawW = DrawMenu.Add("drawW", new CheckBox("Draw W"));
                    DrawMenu.AddColorItem("colorW");
                    DrawMenu.AddWidthItem("widthW");
                    //E
                    _drawE = DrawMenu.Add("drawE", new CheckBox("Draw E"));
                    DrawMenu.AddColorItem("colorE");
                    DrawMenu.AddWidthItem("widthE");
                    //R
                    _drawR = DrawMenu.Add("drawR", new CheckBox("Draw R"));
                    DrawMenu.AddColorItem("colorR");
                    DrawMenu.AddWidthItem("widthR");
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
