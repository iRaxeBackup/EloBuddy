using System;
using System.Timers;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Champions
{
    internal class Kindred : AIOChampion
    {
        private static int qcombomode;
        private Spell.Targeted E, R;
        private Spell.Skillshot Q;
        private Spell.Active W;

        public Geometry.Polygon.Circle WArea { get; set; }

        public override void Init()
        {
            //Spells Init
            try
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 340, SkillShotType.Linear);
                W = new Spell.Active(SpellSlot.W, 900);
                E = new Spell.Targeted(SpellSlot.E, 500);
                R = new Spell.Targeted(SpellSlot.R, 500);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "< font color = '#23ADDB' > Marksman AIO:</ font >< font color = '#E81A0C' > can't declare spells </ font >");
            }

            //Menu
            try
            {
                MainMenu.ComboKeys(useR: false);
                MainMenu.Combo.AddGroupLabel("Combo Settings", "combo.settings.label", true);
                MainMenu.Combo.AddSlider("combo.q.mode", "Q mode on Combo", 1, 1, 3, true);
                MainMenu.Combo["combo.q.mode"].Cast<Slider>().OnValueChange += ComboQMode;
                MainMenu.Combo.AddCheckBox("combo.q.inside", "Lock Player position in W position", true, true);
                MainMenu.Combo.AddSlider("combo.w.enemy", "Use W only if is {0} enemies in Range", 1, 1, 5, true);
                MainMenu.Combo.AddCheckBox("combo.e.focus", "Focus E Passive target", true, true);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Mana Manager", "combo.manamanager.label", true);
                MainMenu.ComboManaManager(true, true, true, false, 10, 10, 10, 0);

                Value.Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "< font color = '#23ADDB' > Marksman AIO:</ font >< font color = '#E81A0C' > can't load the menus </ font >");
            }

            W.OnSpellCasted += WOnOnSpellCasted;
            Game.OnUpdate += GameOnOnUpdate;
        }

        private void GameOnOnUpdate(EventArgs args)
        {
        }

        private void WOnOnSpellCasted(Spell.SpellBase spell, GameObjectProcessSpellCastEventArgs args)
        {
            WArea = new Geometry.Polygon.Circle(args.Start, W.Range);
            var timer = new Timer
            {
                Interval = Math.Max(0, Player.Instance.GetBuff( /*Todo Passive name*/"").EndTime - Game.Time)*100
            };
            timer.Start();
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Chat.Print("Teste");
            WArea = null;
        }

        public override void Combo()
        {
            if (!Player.Instance.IsValid) return;

            var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (target == null) return;

            if (Q.IsReady() && Value.Use("combo.q".AddName()) && Value.Get("combo.q.mana") <= Player.Instance.ManaPercent)
            {
                if (qcombomode == 1)
                {
                    if (Value.Use("combo.q.inside") && WArea != null)
                    {
                        if (WArea.IsOutside(Game.CursorPos2D))
                        {
                            Q.Cast();
                        }
                    }
                }
            }
        }

        private static void ComboQMode(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(1);
        }

        private static void UpdateSlider(int id)
        {
            try
            {
                string displayName;
                if (id == 1)
                {
                    displayName = "Q Mode on Combo: ";
                    if (Value.Get("combo.q.mode") == 1)
                    {
                        displayName = displayName + "Q to Mouse";
                        qcombomode = 1;
                    }
                    else if (Value.Get("combo.speed") == 2)
                    {
                        displayName = displayName + "Safe Q";
                        qcombomode = 2;
                    }
                    else if (Value.Get("combo.speed") == 3)
                    {
                        displayName = displayName + "Burst";
                        qcombomode = 3;
                    }
                    MainMenu.Combo["combo.speed"].Cast<Slider>().DisplayName = displayName;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code anal)</font>");
            }
        }
    }
}