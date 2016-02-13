using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Database.Spell_Library;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;
using SharpDX;
using Spell = EloBuddy.SDK.Spell;

namespace OKTRAIO.Champions
{
    internal class Lucian : AIOChampion
    {
        #region Drawings

        private static void Draw(EventArgs args)
        {
            var colorQ = MainMenu.Draw.GetColor("color.q");
            var widthQ = MainMenu.Draw.GetWidth("width.q");
            var colorQW = MainMenu.Draw.GetColor("color.qw");
            var widthQW = MainMenu.Draw.GetWidth("width.qw");
            var colorR = MainMenu.Draw.GetColor("color.r");
            var widthR = MainMenu.Draw.GetWidth("width.r");

            if (!Value.Use("draw.disable"))
            {
                if (Value.Use("draw.q") && ((Value.Use("draw.ready") && _q.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorQ,
                        Radius = _q.Range,
                        BorderWidth = widthQ
                    }.Draw(Player.Instance.Position);
                }
                if (Value.Use("draw.qw") && ((Value.Use("draw.ready") && _q.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorQW,
                        Radius = _q1.Range,
                        BorderWidth = widthQW
                    }.Draw(Player.Instance.Position);
                }
                if (Value.Use("draw.r") && ((Value.Use("draw.ready") && _r.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorR,
                        Radius = _r.Range,
                        BorderWidth = widthR
                    }.Draw(Player.Instance.Position);
                }
            }
        }

        #endregion

        #region Initialize and Declare

        private static Spell.Targeted _q;
        private static Spell.Skillshot _q1, _w, _e, _r;
        private static bool _passive;
        private static int _randomizerOne, _randomizerTwo;
        private static AttackableUnit _target;

        public override void Init()
        {
            _q = new Spell.Targeted(SpellSlot.Q, 675);
            _q1 = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, int.MaxValue, 65)
            {
                AllowedCollisionCount = int.MaxValue
            };
            _w = new Spell.Skillshot(SpellSlot.W, 1150, SkillShotType.Linear, 250, 1600, 80);
            _e = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
            _r = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Linear, 500, 2800, 110);

            MainMenu.ComboKeys(useR: false);
            MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.gl.pref", true);
            MainMenu.Combo.Add("combo.mode", new Slider("Combo Mode", 0, 0, 1)).OnValueChange += ModeSlider;
            Value.AdvancedMenuItemUiDs.Add("combo.mode");
            MainMenu.Combo["combo.mode"].IsVisible = MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
            MainMenu.Combo.Add("combo.speed", new Slider("Combo Speed", 0, 0, 2)).OnValueChange += SpeedSlider;
            Value.AdvancedMenuItemUiDs.Add("combo.speed");
            MainMenu.Combo["combo.speed"].IsVisible = MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
            MainMenu.Combo.AddSeparator();
            MainMenu.Combo.AddCheckBox("combo.eq", "Use Extended Q if Combo Mode = Normal", true, true);
            MainMenu.Combo.AddCheckBox("combo.wcol", "Check for W collision", false, true);
            MainMenu.Combo.AddCheckBox("combo.egap", "Use E to Gapclose (Not Recommended)", false, true);
            MainMenu.Combo.AddSeparator();
            MainMenu.Combo.Add("combo.emode", new Slider("E Mode: ", 0, 0, 1)).OnValueChange += ComboEModeSlider;
            Value.AdvancedMenuItemUiDs.Add("combo.emode");
            MainMenu.Combo["combo.emode"].Cast<Slider>().IsVisible =
                MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
            MainMenu.Combo.Add("combo.rbind",
                new KeyBind("Semi-Auto R (No Lock)", false, KeyBind.BindTypes.HoldActive, 'T'))
                .OnValueChange += OnUltButton;
            Value.AdvancedMenuItemUiDs.Add("combo.rbind");
            MainMenu.Combo["combo.rbind"].IsVisible = MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;

            MainMenu.HarassKeys(useR: false);
            MainMenu.Harass.AddGroupLabel("Harass Preferences", "harass.gl.pref", true);
            MainMenu.Harass.AddCheckBox("harass.eq", "Use Extended Q in Harass", true, true);
            MainMenu.Harass.AddCheckBox("harass.wcol", "Check for W collision", false, true);
            MainMenu.Harass.AddSeparator();
            MainMenu.Harass.Add("harass.emode", new Slider("E Mode: ", 0, 0, 1)).OnValueChange += HarassEModeSlider;
            Value.AdvancedMenuItemUiDs.Add("harass.emode");
            MainMenu.Harass["harass.emode"].IsVisible =
                MainMenu.Harass["harass.advanced"].Cast<CheckBox>().CurrentValue;
            MainMenu.Harass.AddSlider("harass.mana", "Mana Manager:", 60, 0, 100, true);

            MainMenu.JungleKeys(useR: false);
            MainMenu.Jungle.AddSlider("jungle.mana", "Mana Manager:", 80, 0, 100, true);

            MainMenu.LaneKeys(useR: false);
            MainMenu.Lane.AddGroupLabel("LaneClear Preferences", "lane.gl.pref", true);
            MainMenu.Lane.AddCheckBox("lane.qharass", "Q = Harass Enemies", false, true);
            MainMenu.Lane.AddSeparator();
            MainMenu.Lane.AddSlider("lane.minfarm", "Minions for Farm Q", 3, 0, 6, true);
            MainMenu.Lane.AddSlider("lane.mana", "Mana Manager:", 80, 0, 100, true);

            MainMenu.KsKeys(useW: false);

            MainMenu.DrawKeys(useW: false, useE: false);
            MainMenu.Draw.AddSeparator();
            MainMenu.Draw.AddLabel("W/Extended Q Settings");
            MainMenu.Draw.Add("draw.qw", new CheckBox("Draw W/Extended Q"));
            MainMenu.Draw.AddColorItem("color.qw");
            MainMenu.Draw.AddWidthItem("width.qw");
            MainMenu.DamageIndicator(true);

            MainMenu.FleeKeys(false, useW: false, useR: false);
            Value.Init();
            UpdateSlider(1);
            UpdateSlider(2);
            UpdateSlider(3);
            UpdateSlider(4);

            DamageIndicator.DamageToUnit += GetRawDamage;
            Drawing.OnDraw += Draw;
            if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
            {
                Game.OnUpdate += Game_OnTick;
            }
            else
            {
                Game.OnTick += Game_OnTick;
            }
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Orbwalker.OnPostAttack += OrbwalkerOnPostAttack;
        }

        #endregion

        #region Gamerelated Logic

        #region SpellLogic

        private static void SpellLogic()
        {
            if (_target is AIHeroClient)
            {
                if (Value.Mode(Orbwalker.ActiveModes.Combo))
                {
                    if (Value.Get("combo.mode") == 0)
                    {
                        if (_e.IsReady() && _passive == false && Value.Use("combo.e".AddName()))
                        {
                            ELogic();
                        }
                        else if (_q.IsReady() && _passive == false && Value.Use("combo.q".AddName()))
                        {
                            QLogic();
                        }
                        else if (_w.IsReady() && _passive == false && Value.Use("combo.w".AddName()))
                        {
                            WLogic();
                        }
                    }
                    else
                    {
                        if (_q.IsReady() && _passive == false && Value.Use("combo.q".AddName()))
                        {
                            QLogic();
                        }
                        else if (_w.IsReady() && _passive == false && Value.Use("combo.w".AddName()))
                        {
                            WLogic();
                        }
                        else if (_e.IsReady() && _passive == false && Value.Use("combo.e".AddName()))
                        {
                            ELogic();
                        }
                    }
                }
                else if (Value.Mode(Orbwalker.ActiveModes.Harass) &&
                         Player.Instance.ManaPercent > Value.Get("harass.mana"))
                {
                    if (_q.IsReady() && _passive == false && Value.Use("harass.q"))
                    {
                        QLogic();
                    }
                    else if (_w.IsReady() && _passive == false && Value.Use("harass.w"))
                    {
                        WLogic();
                    }
                    else if (_e.IsReady() && _passive == false && Value.Use("harass.e"))
                    {
                        ELogic();
                    }
                }
            }
            if (_target is Obj_AI_Base)
            {
                var targets = new List<Obj_AI_Minion>();
                if ((_target as Obj_AI_Base).IsMonster && Value.Mode(Orbwalker.ActiveModes.JungleClear) &&
                    Value.Get("jungle.mana") < Player.Instance.ManaPercent)
                {
                    targets =
                        EntityManager.MinionsAndMonsters.Monsters.Where(
                            m => m.Distance(Player.Instance) < Player.Instance.AttackRange)
                            .ToList();
                    if (!targets.Any()) return;
                    if (_q.IsReady() && _passive == false && Value.Use("jungle.q") &&
                        LaneQTarget() != null)
                    {
                        _q.Cast(LaneQTarget());
                    }
                    else if (_w.IsReady() && _passive == false && Value.Use("jungle.w"))
                    {
                        _w.Cast(targets[0].ServerPosition);
                    }
                    else if (_e.IsReady() && Value.Use("jungle.e"))
                    {
                        _e.Cast(OKTRGeometry.SafeDashPosRework(200, _target as Obj_AI_Base, 120));
                    }
                }
                else if ((_target as Obj_AI_Base).IsMinion && Value.Get("lane.mana") < Player.Instance.ManaPercent &&
                         Value.Mode(Orbwalker.ActiveModes.LaneClear))
                {
                    targets =
                        EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                            m => m.Distance(Player.Instance) < Player.Instance.AttackRange).ToList();
                    if (!targets.Any()) return;
                    if (_q.IsReady() && _passive == false && Value.Use("lane.q") && !Value.Use("lane.qharass") &&
                        LaneQTarget() != null)
                    {
                        _q.Cast(LaneQTarget());
                    }
                    else if (_w.IsReady() && _passive == false && Value.Use("lane.w"))
                    {
                        _w.Cast(targets[0].ServerPosition);
                    }
                    else if (_e.IsReady() && Value.Use("lane.e"))
                    {
                        _e.Cast(OKTRGeometry.SafeDashPosRework(200, _target as Obj_AI_Base, 120));
                    }
                }
            }
        }

        #endregion

        #region QLogic

        private static void QLogic()
        {
            _q.Cast(_target as Obj_AI_Base);
        }

        private static void QExtendLogic(AIHeroClient extendTarget)
        {
            if (extendTarget == null) return;
            if (extendTarget.Distance(Player.Instance) < _q.Range || !extendTarget.IsValidTarget(_q1.Range))
                return;
            var closeTargets =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.Distance(Player.Instance) < _q1.Range)
                    .Cast<Obj_AI_Base>()
                    .ToList();
            closeTargets.AddRange(
                EntityManager.MinionsAndMonsters.Monsters.Where(m => m.Distance(Player.Instance) < _q1.Range));
            closeTargets.AddRange(EntityManager.Heroes.Enemies.Where(m => m.Distance(Player.Instance) < _q1.Range));
            var qPred = _q1.GetPrediction(extendTarget);
            foreach (var minion in closeTargets.Select(minion => new
            {
                minion,
                polygon = new Geometry.Polygon.Rectangle(
                    Player.Instance.ServerPosition.To2D(),
                    Player.Instance.ServerPosition.Extend(minion.ServerPosition, _q1.Range), 65f)
            }).Where(@t => @t.polygon.IsInside(qPred.CastPosition)).Select(@t => @t.minion))
            {
                _q.Cast(minion);
            }
        }

        private static Obj_AI_Minion LaneQTarget()
        {
            var targetMinions =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance) < _q.Range)
                    .ToList();
            var hitMinions =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance) < _q1.Range)
                    .ToList();
            foreach (var minion in from minion in targetMinions
                let qHit =
                    new Geometry.Polygon.Rectangle(Player.Instance.Position,
                        Player.Instance.Position.Extend(minion.Position, _q1.Range).To3D(), _q1.Width)
                where hitMinions.Count(x => qHit.IsInside(x.Position.To2D())) >= Value.Get("lane.minfarm")
                select minion)
            {
                return minion;
            }
            if (
                EntityManager.MinionsAndMonsters.Monsters.Any(
                    m => m.Distance(Player.Instance) < _q.Range))
            {
                var targetMonsters =
                    EntityManager.MinionsAndMonsters.Monsters.Where(
                        m => m.Distance(Player.Instance) < _q.Range)
                        .OrderByDescending(m => m.MinionLevel)
                        .ToList();
                return targetMonsters[0];
            }
            return null;
        }

        #endregion

        #region WLogic

        private static void WLogic()
        {
            if ((Value.Use("combo.wcol") && Value.Mode(Orbwalker.ActiveModes.Combo)) ||
                (Value.Use("harass.wcol") && Value.Mode(Orbwalker.ActiveModes.Harass)))
            {
                var wpred = _w.GetPrediction(_target as Obj_AI_Base);
                if (wpred.HitChance <= HitChance.Collision) return;
                _w.Cast(wpred.CastPosition);
            }
            else
            {
                _w.Cast(_target.Position);
            }
        }

        #endregion

        #region ELogic

        private static void ELogic()
        {
            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                if (Value.Get("combo.emode") == 0)
                {
                    var ePos = OKTRGeometry.SafeDashPosRework(300, _target as Obj_AI_Base, 120);
                    if (ePos != Vector3.Zero)
                        _e.Cast(ePos);
                }
                else if (Value.Get("combo.emode") == 1)
                {
                    _e.Cast(Game.CursorPos);
                }
            }
            else
            {
                if (Value.Get("harass.emode") == 0)
                {
                    var ePos = OKTRGeometry.SafeDashPosRework(200, _target as Obj_AI_Base, 120);
                    if (ePos != Vector3.Zero)
                        _e.Cast(ePos);
                }
                else if (Value.Get("harass.emode") == 1)
                {
                    _e.Cast(Game.CursorPos);
                }
            }
        }

        #endregion

        #region Combo

        public override void Combo()
        {
            if (!_passive && Value.Use("combo.q".AddName()) && Value.Use("combo.eq") && Value.Get("combo.mode") == 1)
            {
                QExtendLogic(TargetSelector.GetTarget(_q1.Range, DamageType.Physical));
            }
            if (Value.Use("combo.egap") && Value.Use("combo.e".AddName()))
            {
                var chaseTarget = TargetSelector.GetTarget(Player.Instance.AttackRange + _e.Range,
                    DamageType.Physical);
                if (chaseTarget != null &&
                    Player.Instance.Position.Extend(chaseTarget.Position, _e.Range)
                        .CountEnemiesInRange(1200) == 1)
                {
                    _e.Cast(Player.Instance.Position.Extend(chaseTarget.Position, _e.Range).To3D());
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            if (!_passive && Value.Use("harass.eq") && Player.Instance.ManaPercent > Value.Get("harass.mana"))
            {
                QExtendLogic(TargetSelector.GetTarget(_q1.Range, DamageType.Physical));
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            if (!_passive && Value.Use("lane.qharass") && Value.Use("lane.q") &&
                Player.Instance.ManaPercent > Value.Get("lane.mana"))
            {
                QExtendLogic(TargetSelector.GetTarget(_q1.Range, DamageType.Physical));
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            if (Value.Use("flee.e") && _e.IsReady())
            {
                _e.Cast(Player.Instance.Position.Extend(Game.CursorPos, _e.Range).To3D());
            }
        }

        #endregion

        #endregion

        #region Utils

        #region OnTick

        private void Game_OnTick(EventArgs args)
        {
            if (Value.Mode(Orbwalker.ActiveModes.Harass)) Harass();
            if (Value.Use("killsteal.e")) Killsteal();
        }

        #endregion

        #region OnUltButton

        private static void OnUltButton(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && TargetSelector.GetTarget(_r.Range, DamageType.Physical) != null)
                _r.Cast(TargetSelector.GetTarget(_r.Range, DamageType.Physical).ServerPosition);
        }

        #endregion

        #region KillSteal

        private static void Killsteal()
        {
            if (KsTarget() == null) return;
            if (Value.Use("killsteal.e") && _e.IsReady())
                if (Prediction.Health.GetPrediction(KsTarget(), Game.Ping + _e.CastDelay) <
                    Player.Instance.GetAutoAttackDamage(KsTarget()) + SpellDamage.LucianPassive())
                {
                    var safeE = OKTRGeometry.SafeDashPosRework(200, KsTarget(), 120);
                    if (safeE != Vector3.Zero)
                    {
                        _e.Cast(safeE);
                    }
                }
            if (Value.Use("killsteal.q") & _q.IsReady())
                if (Prediction.Health.GetPrediction(KsTarget(), Game.Ping + _q.CastDelay) <
                    Player.Instance.GetSpellDamage(KsTarget(), SpellSlot.Q))
                {
                    QExtendLogic(KsTarget());
                }
            if (KsTarget().Distance(Player.Instance) < Player.Instance.GetAutoAttackRange())
            {
                if (!Orbwalker.IsAutoAttacking && _passive &&
                    Prediction.Health.GetPrediction(KsTarget(), Game.Ping + _e.CastDelay) <
                    Player.Instance.GetAutoAttackDamage(KsTarget()) + SpellDamage.LucianPassive())
                {
                    ToggleOrb();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, KsTarget());
                    Core.DelayAction(ToggleOrb, (int) (Player.Instance.AttackCastDelay*1000));
                }
                else if (!Orbwalker.IsAutoAttacking &&
                         Prediction.Health.GetPrediction(KsTarget(), Game.Ping + _e.CastDelay) <
                         Player.Instance.GetAutoAttackDamage(KsTarget()))
                {
                    ToggleOrb();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, KsTarget(), true);
                    Core.DelayAction(ToggleOrb, (int) (Player.Instance.AttackCastDelay*1000));
                }
            }
        }


        private static AIHeroClient KsTarget()
        {
            return
                Variables.CloseEnemies(Player.Instance.AttackRange + _e.Range*1.1f)
                    .OrderBy(e => e.Distance(Player.Instance))
                    .ThenBy(e => e.Health)
                    .FirstOrDefault();
        }

        #endregion

        #region OnSpellCast

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.NetworkId != Player.Instance.NetworkId) return;
            if (args.Slot == SpellSlot.R)
            {
                if (Player.Instance.InventoryItems.Any(i => i.Id == ItemId.Youmuus_Ghostblade))
                    Player.Instance.InventoryItems.First(i => i.Id == ItemId.Youmuus_Ghostblade).Cast();
            }
            if (args.Slot == SpellSlot.E)
            {
                Orbwalker.ResetAutoAttack();
            }
            if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E)
            {
                _passive = true;
            }
        }

        #endregion

        #region Orbwalker

        private static void OrbwalkerOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (target.IsValidTarget())
            {
                if (Value.Mode(Orbwalker.ActiveModes.Combo) ||
                    Value.Mode(Orbwalker.ActiveModes.Harass) ||
                    Value.Mode(Orbwalker.ActiveModes.LaneClear) ||
                    Value.Mode(Orbwalker.ActiveModes.JungleClear))
                {
                    Core.DelayAction(SpellLogic, GetSpellDelay);
                    _target = target;
                }
            }
            _passive = false;
        }

        public static void ToggleOrb()
        {
            Orbwalker.DisableMovement = !Orbwalker.DisableMovement;
            Orbwalker.DisableAttacking = !Orbwalker.DisableAttacking;
        }

        #endregion
        
        #region Humanized Delay

        public static int GetSpellDelay
        {
            get { return 50*(new Random().Next(_randomizerOne, _randomizerTwo)/10); }
        }

        private static void SpeedSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(1);
        }

        #endregion

        #region Slider

        private static void ComboEModeSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(2);
        }

        private static void ModeSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(3);
        }

        private static void HarassEModeSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(4);
        }

        private static void UpdateSlider(int id)
        {
            try
            {
                string displayName;
                if (id == 1)
                {
                    displayName = "Speed: ";
                    if (Value.Get("combo.speed") == 0)
                    {
                        displayName = displayName + "Lightning (still humanized)";
                        _randomizerOne = 12;
                        _randomizerTwo = 15;
                    }
                    else if (Value.Get("combo.speed") == 1)
                    {
                        displayName = displayName + "Moderate";
                        _randomizerOne = 22;
                        _randomizerTwo = 26;
                    }
                    else if (Value.Get("combo.speed") == 2)
                    {
                        displayName = displayName + "Whyyyy";
                        _randomizerOne = 26;
                        _randomizerTwo = 34;
                    }
                    MainMenu.Combo["combo.speed"].Cast<Slider>().DisplayName = displayName;
                }
                else if (id == 2)
                {
                    displayName = "E Mode: ";

                    if (Value.Get("combo.emode") == 0)
                    {
                        displayName = displayName + "Best";
                    }
                    else if (Value.Get("combo.emode") == 1)
                    {
                        displayName = displayName + "To Mouse";
                    }
                    MainMenu.Combo["combo.emode"].Cast<Slider>().DisplayName = displayName;
                }
                else if (id == 3)
                {
                    displayName = "Combo Mode: ";

                    if (Value.Get("combo.mode") == 0)
                    {
                        displayName = displayName + "Burst (E->Q->W->E)";
                    }
                    else if (Value.Get("combo.mode") == 1)
                    {
                        displayName = displayName + "Normal (Q->W->E)";
                    }
                    MainMenu.Combo["combo.mode"].Cast<Slider>().DisplayName = displayName;
                }
                else if (id == 4)
                {
                    displayName = "E Mode: ";

                    if (Value.Get("harass.emode") == 0)
                    {
                        displayName = displayName + "Best";
                    }
                    else if (Value.Get("harass.emode") == 1)
                    {
                        displayName = displayName + "To Mouse";
                    }
                    MainMenu.Harass["harass.emode"].Cast<Slider>().DisplayName = displayName;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code anal)</font>");
            }
        }

        #endregion

        #region Damage

        internal static float GetRawDamage(Obj_AI_Base target)
        {
            float damage = 0;
            if (target != null)
            {
                if (_q.IsReady())
                {
                    damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);
                    damage += Player.Instance.GetAutoAttackDamage(target);
                    damage += SpellDamage.LucianPassive();
                }
                if (_w.IsReady())
                {
                    damage += Player.Instance.GetSpellDamage(target, SpellSlot.W);
                    damage += Player.Instance.GetAutoAttackDamage(target);
                    damage += SpellDamage.LucianPassive();
                }
                if (_e.IsReady())
                {
                    damage += Player.Instance.GetAutoAttackDamage(target);
                    damage += SpellDamage.LucianPassive();
                }
            }
            return damage;
        }

        #endregion

        #endregion
    }
}