using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Menu_Settings;
using SharpDX;
using Color = System.Drawing.Color;

namespace OKTRAIO.Champions
{
    internal class Ezreal : AIOChampion
    {
        #region Drawings

        private static void GameOnDraw(EventArgs args)
        {
            var colorW = MainMenu.Draw.GetColor("color.w");
            var widthW = MainMenu.Draw.GetWidth("width.w");
            var colorE = MainMenu.Draw.GetColor("color.e");
            var widthE = MainMenu.Draw.GetWidth("width.e");
            var colorR = MainMenu.Draw.GetColor("color.r");
            var widthR = MainMenu.Draw.GetWidth("width.r");

            if (!Value.Use("draw.disable"))
            {
                if (Value.Use("draw.w") && ((Value.Use("draw.ready") && _w.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorW,
                        Radius = _w.Range,
                        BorderWidth = widthW
                    }.Draw(Player.Instance.Position);
                }
                if (Value.Use("draw.e") && ((Value.Use("draw.ready") && _e.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorE,
                        Radius = _e.Range,
                        BorderWidth = widthE
                    }.Draw(Player.Instance.Position);
                }
                if (Value.Use("draw.r") && ((Value.Use("draw.ready") && _r.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorR,
                        Radius = _q.Range,
                        BorderWidth = widthR
                    }.Draw(Player.Instance.Position);
                }
            }
        }

        #endregion

        #region Initialize and Declare

        //Spells
        private static Spell.Skillshot _q, _w, _r;
        private static Spell.Targeted _e;
        private int _minionId;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            //Spells

            _q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, (int) 60f);
            _w = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, (int) 80f)
            {
                AllowedCollisionCount = int.MaxValue
            };
            _e = new Spell.Targeted(SpellSlot.E, 475);
            _r = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, 2000, (int) 160f);


            try
            {
                //Menu Init
                //Combo
                MainMenu.ComboKeys(useR: false, defaultE: false);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.mode", true);
                MainMenu.Combo.Add("combo.mode", new Slider("Combo Mode", 0, 0, 1)).OnValueChange += ModeSlider;
                Value.AdvancedMenuItemUiDs.Add("combo.mode");
                MainMenu.Combo["combo.mode"].IsVisible =
                    MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
                MainMenu.Combo.Add("combo.emode", new Slider("E Mode: ", 0, 0, 2)).OnValueChange += ComboEModeSlider;
                Value.AdvancedMenuItemUiDs.Add("combo.emode");
                MainMenu.Combo["combo.emode"].Cast<Slider>().IsVisible =
                    MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
                MainMenu.Combo.Add("combo.rbind",
                    new KeyBind("Semi-Auto R (No Lock)", false, KeyBind.BindTypes.HoldActive, 'T'))
                    .OnValueChange += OnUltButton;
                Value.AdvancedMenuItemUiDs.Add("combo.rbind");
                MainMenu.Combo["combo.rbind"].IsVisible =
                    MainMenu.Combo["combo.advanced"].Cast<CheckBox>().CurrentValue;
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Prediction Settings", "combo.advanced.predctionlabel", true);
                MainMenu.Combo.AddSlider("combo.q.pred", "Use Q if HitChance is above than {0}%", 45, 0, 100, true);
                MainMenu.Combo.AddSlider("combo.w.pred", "Use W if HitChance is above than {0}%", 30, 0, 100, true);
                MainMenu.Combo.AddSlider("combo.r.pred", "Use R if HitChance is above than {0}%", 70, 0, 100, true);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Mana Manager:", "combo.advanced.manamanagerlabel", true);
                MainMenu.ComboManaManager(true, true, true, true, 10, 10, 10, 10);

                //Harass
                MainMenu.HarassKeys(useR: false, defaultE: false);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Harass Preferences", "harass.grouplabel.mode", true);
                MainMenu.Harass.AddCheckBox("harass.auto", "Use AUTO HARASS", false, true);
                MainMenu.Harass.Add("harass.emode", new Slider("E Mode: ", 0, 0, 2)).OnValueChange += HarassEModeSlider;
                Value.AdvancedMenuItemUiDs.Add("harass.emode");
                MainMenu.Harass["harass.emode"].IsVisible =
                    MainMenu.Harass["harass.advanced"].Cast<CheckBox>().CurrentValue;
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Prediction Settings", "harass.advanced.predctionlabel", true);
                MainMenu.Harass.AddSlider("harass.q.pred", "Use Q if HitChance is above than {0}%", 45, 0, 100, true);
                MainMenu.Harass.AddSlider("harass.w.pred", "Use W if HitChance is above than {0}%", 30, 0, 100, true);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.advanced.manamanagerlabel", true);
                MainMenu.HarassManaManager(true, true, true, false, 60, 60, 0, 0);

                //Farm
                MainMenu.LaneKeys(useE: false, useR: false);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddGroupLabel("Q Settings", "lane.advanced.qsettingslabel", true);
                MainMenu.Lane.AddCheckBox("lane.q.aa", "Use Q only when can't AA", true, true);
                MainMenu.Lane.AddCheckBox("lane.q.lasthit", "Use Q as LastHit also in this mode", false, true);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddGroupLabel("Mana Manager:", "harass.advanced.manamanagerlabel", true);
                MainMenu.LaneManaManager(true, true, false, false, 65, 0, 0, 0);

                //Jungle Clear Menu Settings
                MainMenu.JungleKeys(useR: false, defaultE: false);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddGroupLabel("Jungleclear Preferences", "jungle.grouplabel.1", true);
                MainMenu.Jungle.AddCheckBox("jungle.monsters.spell", "Use Abilities on Big Monster", true, true);
                MainMenu.Jungle.AddCheckBox("jungle.minimonsters.spell", "Use Abilities on Mini Monsters", false, true);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.2", true);
                MainMenu.JungleManaManager(true, true, true, false, 60, 50, 40, 50);

                //Last hit Menu Settings
                MainMenu.LastHitKeys(useW: false, useE: false, useR: false);
                MainMenu.Lasthit.AddSeparator();
                MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.1", true);
                MainMenu.LasthitManaManager(true, false, false, false, 60, 50, 40, 50);

                //Ks
                MainMenu.KsKeys(defaultE: false);
                MainMenu.Ks.AddSeparator();
                MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.5", true);
                MainMenu.KsManaManager(true, true, true, true, 60, 50, 40, 50);

                //Flee Menu
                MainMenu.FleeKeys(false, useW: false, useR: false);

                //Misc
                MainMenu.MiscMenu();
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddCheckBox("misc.q", "Use Auto Q");
                MainMenu.Misc.AddCheckBox("misc.w", "Use Auto W");
                MainMenu.Misc.AddCheckBox("misc.e.gapcloser", "Use Auto E on GapCloser", false);
                MainMenu.Misc.AddCheckBox("misc.e.gapcloser.wall", "Safe GapCloser E (Wall)");
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Auto Q-W Settings", "misc.grouplabel.addonmenu", true);
                MainMenu.Misc.AddCheckBox("misc.w.ally", "Use W on Allies/Yourself", false, true);
                MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.stun", "Use W on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.charm", "Use W on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.taunt", "Use W on Taunted Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.fear", "Use Q on Feared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.fear", "Use W on Feared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.snare", "Use W on Snared Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Mana Manager:", "misc.grouplabel.addonmenu.1", true);
                MainMenu.Misc.AddSlider("misc.q.mana", "Use Q on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                    true);
                MainMenu.Misc.AddSlider("misc.w.mana", "Use W on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                    true);
                MainMenu.DrawKeys();
                MainMenu.Draw.AddCheckBox("draw.hp.bar", "Draw Combo Damage", true, true);
                MainMenu.DamageIndicator(true);
                //Value
                Value.Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code MENU)</font>");
            }
            try
            {
                Drawing.OnDraw += GameOnDraw;
                Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
                Orbwalker.OnPreAttack += OrbwalkerOnOnPreAttack;
                Orbwalker.OnPostAttack += OrbwalkerOnOnPostAttack;
                if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
                {
                    Game.OnUpdate += GameOnUpdate;
                }
                else
                {
                    Game.OnTick += GameOnUpdate;
                }
                Obj_AI_Base.OnBuffGain += BuffGain;
                Drawing.OnEndScene += Drawing_OnEndScene;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 503)</font>");
            }
        }

        #endregion

        #region Gamerelated Logic

        #region Combo

        public override void Combo()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Mixed);

            if (target == null) return;

            if (Value.Get("combo.mode") == 0)
            {
                if (_e.IsReady() && Value.Use("combo.e".AddName()))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("combo.e.mana"))
                    {
                        ELogic();
                    }
                }
                else if (_q.IsReady() && Value.Use("combo.q".AddName()))
                {
                    if ((_q.GetPrediction(target).HitChancePercent >= Value.Get("combo.q.pred")) &&
                        (Player.Instance.ManaPercent >= Value.Get("combo.q.mana")))
                    {
                        _q.Cast(_q.GetPrediction(target).CastPosition);
                    }
                }
                else if (_w.IsReady() && Value.Use("combo.w".AddName()))
                {
                    var ally =
                        EntityManager.Heroes.Allies
                            .FirstOrDefault(x => x.IsValidTarget(_w.Range));

                    if (Value.Use("misc.w.ally"))
                    {
                        if (Player.Instance.Distance(ally) <= _w.Range)
                        {
                            _w.Cast(_w.GetPrediction(ally).CastPosition);
                        }
                        else
                        {
                            _w.Cast(Game.CursorPos);
                        }
                    }
                    else if ((_w.GetPrediction(target).HitChancePercent >= Value.Get("combo.w.pred")) &&
                             (Player.Instance.ManaPercent >= Value.Get("combo.w.mana")))
                    {
                        _w.Cast(_w.GetPrediction(target).CastPosition);
                    }
                }
            }
            else
            {
                if (_q.IsReady() && Value.Use("combo.q".AddName()))
                {
                    if ((_q.GetPrediction(target).HitChancePercent >= Value.Get("combo.q.pred")) &&
                        (Player.Instance.ManaPercent >= Value.Get("combo.q.mana")))
                    {
                        _q.Cast(_q.GetPrediction(target).CastPosition);
                    }
                }
                else if (_w.IsReady() && Value.Use("combo.w".AddName()))
                {
                    if ((_w.GetPrediction(target).HitChancePercent >= Value.Get("combo.w.pred")) &&
                        (Player.Instance.ManaPercent >= Value.Get("combo.w.mana")))
                    {
                        _w.Cast(_w.GetPrediction(target).CastPosition);
                    }
                }
                else if (_e.IsReady() && Value.Use("combo.e".AddName()))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("combo.e.mana"))
                    {
                        ELogic();
                    }
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Mixed);

            if (target == null) return;

            if (_q.IsReady() && Value.Use("harass.q"))
            {
                if ((_q.GetPrediction(target).HitChancePercent >= Value.Get("harass.q.pred")) &&
                    (Player.Instance.ManaPercent >= Value.Get("harass.q.mana")))
                {
                    _q.Cast(_q.GetPrediction(target).CastPosition);
                }
            }
            else if (_w.IsReady() && Value.Use("harass.w"))
            {
                var ally =
                    EntityManager.Heroes.Allies
                        .FirstOrDefault(x => x.IsValidTarget(_w.Range));

                if (Value.Use("misc.w.ally"))
                {
                    if (Player.Instance.Distance(ally) <= _w.Range)
                    {
                        _w.Cast(_w.GetPrediction(ally).CastPosition);
                    }
                    else
                    {
                        _w.Cast(Game.CursorPos);
                    }
                }

                else if ((_w.GetPrediction(target).HitChancePercent >= Value.Get("harass.w.pred")) &&
                         (Player.Instance.ManaPercent >= Value.Get("harass.w.mana")))
                {
                    _w.Cast(_w.GetPrediction(target).CastPosition);
                }
            }
            else if (_e.IsReady() && Value.Use("harass.e"))
            {
                if (Player.Instance.ManaPercent >= Value.Get("harass.e.mana"))
                {
                    ELogic();
                }
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    _q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (source == null) return;

            if (_q.IsReady() && Value.Use("lane.q"))
            {
                if (Value.Use("lane.q.lasthit") && _q.IsReady())
                {
                    if (Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana") &&
                        source.TotalShieldHealth() <= Player.Instance.GetSpellDamage(source, SpellSlot.Q))
                    {
                        _q.Cast(source);
                    }
                }
                else if (Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
                {
                    if (Value.Use("lane.q.aa"))
                    {
                        if (Player.Instance.GetAutoAttackRange() >= source.Distance(Player.Instance))
                        {
                            _q.Cast(source);
                        }
                    }
                    else
                    {
                        _q.Cast(source);
                    }
                }
            }
        }

        #endregion

        #region Jungleclear

        public override void Jungleclear()
        {
            var monsters =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,
                    _q.Range)
                    .FirstOrDefault(x => x.IsValidTarget(_q.Range));
            var fappamonsters =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,
                    _q.Range)
                    .LastOrDefault(x => x.IsValidTarget(_q.Range));

            if ((monsters == null) || (fappamonsters == null)) return;

            if (Value.Use("jungle.monsters.spell"))
            {
                if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                {
                    if (Value.Use("jungle.q") && _q.IsReady())
                    {
                        _q.Cast(monsters);
                    }
                }

                else if (_w.IsReady() && Value.Use("jungle.w"))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                    {
                        _w.Cast(monsters);
                    }
                }

                else if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                {
                    if (Value.Use("jungle.e") && _e.IsReady())
                    {
                        _e.Cast(DetectWall());
                    }
                }
            }

            if (Value.Use("jungle.minimonsters.spell"))
            {
                if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                {
                    if (Value.Use("jungle.q") && _q.IsReady())
                    {
                        _q.Cast();
                    }
                }

                else if (_w.IsReady() && Value.Use("jungle.w"))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                    {
                        _w.Cast(fappamonsters);
                    }
                }

                if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                {
                    if (Value.Use("jungle.e") && _w.IsReady())
                    {
                        _e.Cast(DetectWall());
                    }
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            if (_e.IsReady() && Value.Use("flee.e") && _e.IsLearned)
            {
                _e.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _e.Range).To3D());
            }
        }

        #endregion

        #region Lasthit

        public override void LastHit()
        {
            var source =
                EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault
                    (m =>
                        m.IsValidTarget(_q.Range) &&
                        (Player.Instance.GetSpellDamage(m, SpellSlot.Q) > m.TotalShieldHealth()));

            if (source == null) return;

            if (Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
            {
                if (Value.Use("lasthit.q") && _q.IsReady())
                {
                    _q.Cast(source);
                }
            }
        }

        #endregion

        #endregion

        #region Utils

        #region Orbwalker

        private void OrbwalkerOnOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (_minionId != target.NetworkId) return;
            _minionId = target.NetworkId;
        }

        private void OrbwalkerOnOnPreAttack(AttackableUnit target, EventArgs args)
        {
            if (!target.IsMe) return;
            if (_minionId != target.NetworkId) return;
            _minionId = target.NetworkId;
            if (_w.IsReady() && Value.Use("lane.w") && (target.Type == GameObjectType.obj_AI_Turret) &&
                target.IsValid && (Player.Instance.ManaPercent >= Value.Get("lane.w.mana")))
            {
                foreach (var allies in EntityManager.Heroes.Allies)
                {
                    if ((allies.Distance(Player.Instance.Position) < 600) && !allies.IsMe && allies.IsAlly)
                    {
                        _w.Cast(allies);
                    }
                }
            }
        }

        #endregion

        #region OnUpdate

        private void GameOnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.HasBuff("Recall")
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed ||
                Orbwalker.IsAutoAttacking)
                return;

            if (Value.Use("harass.auto"))
            {
                AutoHarass();
            }

            KillSteal();
        }

        #endregion

        #region Damage

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Value.Use("draw.hp.bar"))
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && a.IsHPBarRendered))
                {
                    var damage = ComboDamage(enemy);
                    var damagepercent = (enemy.TotalShieldHealth() - damage > 0 ? enemy.TotalShieldHealth() - damage : 0)/
                                        (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                    var hppercent = enemy.TotalShieldHealth()/
                                    (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                    var start = new Vector2((int) (enemy.HPBarPosition.X + Offset.X + damagepercent*104),
                        (int) (enemy.HPBarPosition.Y + Offset.Y) - 5);
                    var end = new Vector2((int) (enemy.HPBarPosition.X + Offset.X + hppercent*104) + 2,
                        (int) (enemy.HPBarPosition.Y + Offset.Y) - 5);

                    Drawing.DrawLine(start, end, 9, Color.Chartreuse);
                }
            }
        }

        private static float ComboDamage(Obj_AI_Base enemy)
        {
            var damage = Player.Instance.GetAutoAttackDamage(enemy);

            if (_q.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.Q);
            }

            if (_w.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.W);
            }

            if (_e.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.E);
            }

            if (_r.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
            }

            return damage;
        }

        #endregion

        #region AntiGapCloser

        private void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || (e.Sender.Type != Player.Instance.Type) || !e.Sender.IsEnemy)
                return;

            if (Value.Use("misc.e.gapcloser"))
            {
                if (Value.Use("misc.e.gapcloser.wall"))
                {
                    _e.Cast(DetectWall());
                }
                else
                {
                    _e.Cast(Player.Instance.Position.Extend(Game.CursorPos, _e.Range).To3D());
                }
            }
        }

        #endregion

        #region Geometry-Cone

        private static bool InsideCone()
        {
            var target = TargetSelector.GetTarget(_e.Range, DamageType.Mixed);
            if (target == null) return false;
            var cone = new Geometry.Polygon.Sector(Player.Instance.Position,
                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), target.Position.To2D(), 90).To3D(),
                Geometry.DegreeToRadian(180),
                Player.Instance.AttackRange + Player.Instance.BoundingRadius*2);
            return cone.IsInside(Game.CursorPos.To2D());
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
                if (id == 2)
                {
                    displayName = "E Mode: ";

                    if (Value.Get("combo.emode") == 0)
                    {
                        displayName = displayName + "Safe";
                    }
                    else if (Value.Get("combo.emode") == 1)
                    {
                        displayName = displayName + "Burst";
                    }
                    else if (Value.Get("combo.emode") == 2)
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
                        displayName = displayName + "Burst (E->Q->W->R)";
                    }
                    else if (Value.Get("combo.mode") == 1)
                    {
                        displayName = displayName + "Normal (Q->W->E->R)";
                    }
                    MainMenu.Combo["combo.mode"].Cast<Slider>().DisplayName = displayName;
                }
                else if (id == 4)
                {
                    displayName = "E Mode: ";

                    if (Value.Get("harass.emode") == 0)
                    {
                        displayName = displayName + "Safe";
                    }
                    else if (Value.Get("harass.emode") == 1)
                    {
                        displayName = displayName + "Burst";
                    }
                    else if (Value.Get("harass.emode") == 2)
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

        #region UltButton

        private static void OnUltButton(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && (TargetSelector.GetTarget(_r.Range, DamageType.Mixed) != null))
                _r.Cast(_r.GetPrediction(TargetSelector.GetTarget(_r.Range, DamageType.Mixed)).CastPosition);
        }

        #endregion

        #region ELogic

        private static void ELogic()
        {
            var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(), DamageType.Mixed);
            if (target == null) return;

            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                if (Value.Get("combo.emode") == 0)
                {
                    var ePos = OKTRGeometry.SafeDashPosRework(_e.Range/2f, target, _e.CastDelay);
                    if (ePos != Vector3.Zero)
                        _e.Cast(ePos);
                }
                else if (Value.Get("combo.emode") == 1)
                {
                    if ((Game.CursorPos.Distance(Player.Instance.Position) >
                         Player.Instance.AttackRange + Player.Instance.BoundingRadius*2) &&
                        !Player.Instance.Position.Extend(Game.CursorPos, _e.Range).IsUnderTurret())
                    {
                        _e.Cast(Player.Instance.Position.Extend(Game.CursorPos, _e.Range).To3D());
                    }
                    else
                    {
                        if (InsideCone())
                        {
                            _e.Cast(
                                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), target.Position.To2D(), -65)
                                    .To3D());
                        }
                        else
                        {
                            _e.Cast(
                                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), target.Position.To2D(), 65)
                                    .To3D());
                        }
                    }
                }
                else if (Value.Get("combo.emode") == 2)
                {
                    _e.Cast(Game.CursorPos);
                }
            }
            else
            {
                if (Value.Get("harass.emode") == 0)
                {
                    var ePos = OKTRGeometry.SafeDashPosRework(_e.Range/2f, target, _e.CastDelay);
                    if (ePos != Vector3.Zero)
                        _e.Cast(ePos);
                }
                else if (Value.Get("harass.emode") == 1)
                {
                    if ((Game.CursorPos.Distance(Player.Instance.Position) >
                         Player.Instance.AttackRange + Player.Instance.BoundingRadius*2) &&
                        !Player.Instance.Position.Extend(Game.CursorPos, _e.Range).IsUnderTurret())
                    {
                        _e.Cast(Player.Instance.Position.Extend(Game.CursorPos, _e.Range).To3D());
                    }
                    else
                    {
                        if (InsideCone())
                        {
                            _e.Cast(
                                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), target.Position.To2D(), 255)
                                    .To3D());
                        }
                        else
                        {
                            _e.Cast(
                                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), target.Position.To2D(), 65)
                                    .To3D());
                        }
                    }
                }
                else if (Value.Get("harass.emode") == 2)
                {
                    _e.Cast(Game.CursorPos);
                }
            }
        }

        #endregion

        #region AutoHarass

        private static void AutoHarass()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Mixed);

            if (_q.IsReady() && Value.Use("harass.q"))
            {
                if ((_q.GetPrediction(target).HitChancePercent >= Value.Get("harass.q.pred")) &&
                    (Player.Instance.ManaPercent >= Value.Get("harass.q.mana")))
                {
                    _q.Cast(_q.GetPrediction(target).CastPosition);
                }
            }
            else if (_w.IsReady() && Value.Use("harass.w"))
            {
                var ally =
                    EntityManager.Heroes.Allies
                        .FirstOrDefault(x => x.IsValidTarget(_w.Range));

                if (Value.Use("misc.w.ally"))
                {
                    if (Player.Instance.Distance(ally) <= _w.Range)
                    {
                        _w.Cast(_w.GetPrediction(ally).CastPosition);
                    }
                    else
                    {
                        _w.Cast(Game.CursorPos);
                    }
                }

                else if ((_w.GetPrediction(target).HitChancePercent >= Value.Get("harass.w.pred")) &&
                         (Player.Instance.ManaPercent >= Value.Get("harass.w.mana")))
                {
                    _w.Cast(_w.GetPrediction(target).CastPosition);
                }
            }
        }

        #endregion

        #region OnBuffGain

        private void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (Value.Use("misc.q") && _q.IsReady())
            {
                if (sender.IsMe) return;

                if (Player.Instance.IsInRange(sender, _q.Range) && Value.Get("misc.q.mana") >= Player.Instance.ManaPercent)
                {
                    if (Value.Use("misc.q.stun") && sender.IsStunned)
                    {
                        _q.Cast(sender);
                    }
                    if (Value.Use("misc.q.snare") && sender.IsRooted)
                    {
                        _q.Cast(sender);
                    }
                    if (Value.Use("misc.q.charm") && sender.IsCharmed)
                    {
                        _q.Cast(sender);
                    }
                    if (Value.Use("misc.q.taunt") && sender.IsTaunted)
                    {
                        _q.Cast(sender);
                    }
                    if (Value.Use("misc.q.fear") && sender.IsFeared)
                    {
                        _q.Cast(sender);
                    }
                }
            }
            if (Value.Use("misc.w") && _w.IsReady())
            {
                if (Player.Instance.IsInRange(sender, _w.Range) && Value.Get("misc.w.mana") >= Player.Instance.ManaPercent)
                {
                    if (Value.Use("misc.w.stun") && sender.IsStunned)
                    {
                        _w.Cast(sender);
                    }
                    if (Value.Use("misc.w.snare") && sender.IsRooted)
                    {
                        _w.Cast(sender);
                    }
                    if (Value.Use("misc.w.charm") && sender.IsCharmed)
                    {
                        _w.Cast(sender);
                    }
                    if (Value.Use("misc.w.taunt") && sender.IsTaunted)
                    {
                        _w.Cast(sender);
                    }
                    if (Value.Use("misc.w.fear") && sender.IsFeared)
                    {
                        _w.Cast(sender);
                    }
                }
            }
        }

        #endregion 

        #region KillSteal

        private static void KillSteal()
        {
            foreach (
                var target in
                    EntityManager.Heroes.Enemies.Where(
                        hero =>
                            hero.IsValidTarget(1200) && !hero.IsDead && !hero.IsZombie &&
                            (hero.HealthPercent <= 25)))
            {
                if (Value.Use("killsteal.q") && _q.IsReady())
                {
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.q.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.Q))

                        {
                            _q.Cast(_q.GetPrediction(target).CastPosition);
                        }
                    }
                }

                if (Value.Use("killsteal.w") && _w.IsReady())
                {
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.w.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.W))

                        {
                            _w.Cast(_w.GetPrediction(target).CastPosition);
                        }
                    }
                }

                if (Value.Use("killsteal.e") && _e.IsReady())
                {
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.e.mana"))
                    {
                        var tawah =
                            EntityManager.Turrets.Enemies.FirstOrDefault(
                                a =>
                                    !a.IsDead && (a.Distance(target) <= 775 + Player.Instance.BoundingRadius +
                                                  target.BoundingRadius/2 + 44.2));

                        if ((target.Health + target.AttackShield <
                             Player.Instance.GetSpellDamage(target, SpellSlot.E)) &&
                            (target.Position.CountEnemiesInRange(800) == 1) && (tawah == null) &&
                            (Player.Instance.Mana >= 120))
                        {
                            _e.Cast(target);
                        }
                    }
                }
                if (Value.Use("killsteal.r") && _r.IsReady())
                {
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.r.mana"))
                    {
                        if (target.Distance(Player.Instance) <= 1200)
                            if (target.Health + target.AttackShield <
                                Player.Instance.GetSpellDamage(target, SpellSlot.R))
                            {
                                _r.Cast(_r.GetPrediction(target).CastPosition);
                            }
                    }
                }
            }
        }

        #endregion

        #region DetectWall

        private Vector3 DetectWall()
        {
            const int circleLineSegmentN = 20;

            var outRadius = 700/(float) Math.Cos(2*Math.PI/circleLineSegmentN);
            var inRadius = 300/(float) Math.Cos(2*Math.PI/circleLineSegmentN);
            var bestPoint = ObjectManager.Player.Position;
            for (var i = 1; i <= circleLineSegmentN; i++)
            {
                var angle = i*2*Math.PI/circleLineSegmentN;
                var point =
                    new Vector2(ObjectManager.Player.Position.X + outRadius*(float) Math.Cos(angle),
                        ObjectManager.Player.Position.Y + outRadius*(float) Math.Sin(angle)).To3D();
                var point2 =
                    new Vector2(ObjectManager.Player.Position.X + inRadius*(float) Math.Cos(angle),
                        ObjectManager.Player.Position.Y + inRadius*(float) Math.Sin(angle)).To3D();
                if (((point.ToNavMeshCell().CollFlags & CollisionFlags.Wall) != 0) &&
                    ((point2.ToNavMeshCell().CollFlags & CollisionFlags.Wall) != 0) &&
                    (Game.CursorPos.Distance(point) < Game.CursorPos.Distance(bestPoint)))
                {
                    bestPoint = point;
                    return bestPoint;
                }
            }

            return new Vector3();
        }

        #endregion

        #endregion
    }
}