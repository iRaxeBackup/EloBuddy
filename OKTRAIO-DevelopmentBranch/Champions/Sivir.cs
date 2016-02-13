using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Menu_Settings;
using SharpDX;
using Color = System.Drawing.Color;

namespace OKTRAIO.Champions
{
    internal class Sivir : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Active _r, _w, _e;
        private static Spell.Skillshot _q;
        private static readonly Vector2 Offset = new Vector2(1, 0);
        private static float _qmana, _wmana, _rmana;


        public override void Init()
        {
            try
            {
                //spells
                _q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1350, 90)
                {
                    AllowedCollisionCount = int.MaxValue
                };
                _w = new Spell.Active(SpellSlot.W, (uint) Player.Instance.GetAutoAttackRange());
                _e = new Spell.Active(SpellSlot.E);
                _r = new Spell.Active(SpellSlot.R, 1000);


                //menu

                //combo
                MainMenu.ComboKeys(useE: false);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                MainMenu.Combo.AddSlider("combo.r.enemy", "Min. {0} Enemies in Range for R", 3, 0, 5, true);
                MainMenu.Combo.AddSlider("combo.r.ally", "Min. {0} Allys in Range for R", 3, 0, 5, true);
                MainMenu.Combo.AddCheckBox("combo.mana.management", "Smart Mana Management", true, true);
                MainMenu.Combo.AddSlider("combo.mana.health", "Ignore Mana Management when below {0}% Health", 30, 0,
                    100, true);

                //flee
                MainMenu.FleeKeys(false, useW: false, useE: false);
                MainMenu.Flee.AddSeparator();
                MainMenu.Flee.AddSlider("flee.r.enemy", "Min. {0} Enemies in Range for R", 2, 0, 5, true);
                MainMenu.Flee.AddSeparator();
                MainMenu.Flee.AddGroupLabel("Mana Manager:", "flee.grouplabel.addonmenu", true);
                MainMenu.FleeManaManager(false, false, false, true, 0, 0, 0, 20);

                //lasthit
                MainMenu.LastHitKeys(useW: false, useE: false, useR: false);
                MainMenu.Lasthit.AddSeparator();
                MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu", true);
                MainMenu.LasthitManaManager(true, false, false, false, 20, 0, 0, 0);

                //laneclear
                MainMenu.LaneKeys(useE: false, useR: false);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddSlider("lane.q.min", "Min. {0} Minions for Q", 4, 1, 7, true);
                MainMenu.Lane.AddSlider("lane.w.min", "Min. {0} Minions for W", 3, 1, 7, true);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddCheckBox("lane.q.harass", "Use Q for Harass", true, true);
                MainMenu.Lane.AddCheckBox("lane.w.turret", "Use W on Turret/Inhib/Nexus", true, true);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.addonmenu", true);
                MainMenu.LaneManaManager(true, true, false, false, 60, 60, 0, 0);

                //jungleclear
                MainMenu.JungleKeys(useE: false, useR: false);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu", true);
                MainMenu.JungleManaManager(true, true, false, false, 60, 60, 0, 0);

                //harass
                MainMenu.HarassKeys(useE: false, useR: false);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu", true);
                MainMenu.HarassManaManager(true, true, false, false, 40, 40, 0, 0);

                //Ks
                MainMenu.KsKeys(useW: false, useE: false, useR: false);
                MainMenu.Ks.AddSeparator();
                MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                MainMenu.KsManaManager(true, false, false, false, 20, 0, 0, 0);

                //misc
                MainMenu.MiscMenu();
                MainMenu.Misc.AddGroupLabel("Auto E - Spell Settings", "misc.grouplabel.addonmenu");
                MainMenu.Misc.AddSeparator();
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => a.Team != Player.Instance.Team))
                {
                    foreach (
                        var spell in
                            enemy.Spellbook.Spells.Where(
                                a =>
                                    a.Slot == SpellSlot.Q || a.Slot == SpellSlot.W || a.Slot == SpellSlot.E ||
                                    a.Slot == SpellSlot.R))
                    {
                        if (spell.Slot == SpellSlot.Q)
                        {
                            MainMenu.Misc.Add(spell.SData.Name,
                                new CheckBox(enemy.ChampionName + " - Q - " + spell.Name, false));
                        }
                        else if (spell.Slot == SpellSlot.W)
                        {
                            MainMenu.Misc.Add(spell.SData.Name,
                                new CheckBox(enemy.ChampionName + " - W - " + spell.Name, false));
                        }
                        else if (spell.Slot == SpellSlot.E)
                        {
                            MainMenu.Misc.Add(spell.SData.Name,
                                new CheckBox(enemy.ChampionName + " - E - " + spell.Name, false));
                        }
                        else if (spell.Slot == SpellSlot.R)
                        {
                            MainMenu.Misc.Add(spell.SData.Name,
                                new CheckBox(enemy.ChampionName + " - R - " + spell.Name, false));
                        }
                    }
                }
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Auto Q Settings", "misc.grouplabel.addonmenu.1", true);
                MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.fear", "Use Q on Feared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Prediction", "misc.grouplabel.addonmenu.2", true);
                MainMenu.Misc.AddSlider("misc.q.prediction", "Hitchance Percentage for Q", 80, 0, 100, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Mana Manager:", "misc.grouplabel.addonmenu.3", true);
                MainMenu.Misc.AddSlider("misc.q.mana", "Use Q on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                    true);
                
                //draw
                MainMenu.DrawKeys(useW: false, useE: false);
                MainMenu.Draw.AddSeparator();
                MainMenu.Draw.AddCheckBox("draw.hp.bar", "Draw Combo Damage", true, true);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code MENU)</font>");
            }

            try
            {
                Value.Init();
                if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
                {
                    Game.OnUpdate += GameOnUpdate;
                }
                else
                {
                    Game.OnTick += GameOnUpdate;
                }
                Obj_AI_Base.OnBuffGain += BuffGain; 
                Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
                Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
                Drawing.OnDraw += GameOnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code INIT)</font>");
            }
        }

        #endregion

        #region Gamerelated Logic

        #region Combo

        public override void Combo()
        {
            var targetq = TargetSelector.GetTarget(_q.Range, DamageType.Physical);
            var targetr = EntityManager.Heroes.Enemies.Where(a => a.IsValidTarget(1400));
            var ally = EntityManager.Heroes.Allies.Where(a => a.IsValidTarget(_r.Range - 50));

            if (targetq == null) return;

            if (_q.IsReady() && Value.Use("combo.q".AddName()))
            {
                var qpred = _q.GetPrediction(targetq);
                if (qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                {
                    _q.Cast(qpred.CastPosition);
                }
            }

            if (_r.IsReady() && Value.Use("combo.r".AddName()))
            {
                if (ally.Count() >= Value.Get("combo.r.ally") && targetr.Count() >= Value.Get("combo.r.enemy"))
                {
                    _r.Cast();
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            var targetq = TargetSelector.GetTarget(_q.Range, DamageType.Physical);
            if (targetq == null)
            {
                return;
            }
            var qpred = _q.GetPrediction(targetq);

            if (_q.IsReady() && Value.Use("harass.q") && Player.Instance.ManaPercent >= Value.Get("harass.q.mana"))
            {
                if (qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                {
                    _q.Cast(qpred.CastPosition);
                }
            }
        }

        #endregion

        #region LaneClear

        public override void Laneclear()
        {
            if (Player.Instance.CountEnemiesInRange(_q.Range) == 0)
            {
                var minionq = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, _q.Range);
                var qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionq, _q.Width, (int) _q.Range);

                if (_q.IsReady() && Value.Use("lane.q") && Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
                {
                    if (qfarm.HitNumber >= Value.Get("lane.q.min"))
                    {
                        _q.Cast(qfarm.CastPosition);
                    }
                }
            }
            else
            {
                var targetq = TargetSelector.GetTarget(_q.Range, DamageType.Physical);
                if (targetq == null)
                {
                    return;
                }
                var qpred = _q.GetPrediction(targetq);

                if (_q.IsReady() && Value.Use("lane.q.harass") &&
                    Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
                {
                    if (qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                    {
                        _q.Cast(qpred.CastPosition);
                    }
                }
            }
        }

        #endregion

        #region JungleClear

        public override void Jungleclear()
        {
            var bigboy =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .FirstOrDefault(
                        a =>
                            a.IsValidTarget(_q.Range) && Variables.SummonerRiftJungleList.Contains(a.BaseSkinName) &&
                            a.Health >= QMaxDamage(a));

            if (_q.IsReady() && Value.Use("jungle.q") && Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
            {
                if (bigboy != null)
                {
                    _q.Cast(bigboy);
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var targetr = EntityManager.Heroes.Enemies.Where(a => a.IsValidTarget(1400));

            if (_r.IsReady() && Value.Use("flee.r") && Player.Instance.ManaPercent >= Value.Get("flee.r.mana"))
            {
                if (targetr.Count() >= Value.Get("flee.r.enemy"))
                {
                    _r.Cast();
                }
            }
        }

        #endregion

        #region LastHit

        public override void LastHit()
        {
            var minionq =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(
                        a => a.IsValidTarget(_q.Range) && a.Health <= QDamage(a));

            if (_q.IsReady() && Value.Use("lasthit.q") && Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
            {
                if (minionq != null && Player.Instance.IsInAutoAttackRange(minionq) &&
                    minionq.Health > Player.Instance.GetAutoAttackDamage(minionq, true))
                {
                    _q.Cast(minionq);
                }
                else if (minionq != null)
                {
                    _q.Cast(minionq);
                }
            }
        }

        #endregion

        #endregion

        #region Utils

        #region GameOnUpdate

        private static void GameOnUpdate(EventArgs args)
        {
            Ks();

            ManaManagement();
        }

        #endregion

        #region OnPostAttack

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Player.Instance.IsInAutoAttackRange(target))
            {
                if (_w.IsReady() && Value.Use("combo.w".AddName()) &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                    Player.Instance.Mana > _wmana + _rmana)
                {
                    _w.Cast();
                    Orbwalker.ResetAutoAttack();
                }

                if (_w.IsReady() && Value.Use("harass.w") &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                    Player.Instance.ManaPercent >= Value.Get("harass.w.mana"))
                {
                    var targetw = TargetSelector.GetTarget(_w.Range, DamageType.Physical);
                    if (targetw != null && targetw.NetworkId == target.NetworkId)
                    {
                        _w.Cast();
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (_w.IsReady() && Value.Use("jungle.w") &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                    Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                {
                    var bigboys =
                        EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(
                            a =>
                                a.IsValidTarget(_q.Range) && Variables.SummonerRiftJungleList.Contains(a.BaseSkinName) &&
                                a.Health >= Player.Instance.GetAutoAttackDamage(a, true));


                    if (bigboys != null && target.NetworkId == bigboys.NetworkId)
                    {
                        _w.Cast();
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (_w.IsReady() && Value.Use("lane.w") &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                    Player.Instance.ManaPercent >= Value.Get("lane.w.mana"))
                {
                    var turrets = EntityManager.Turrets.Enemies.Find(a => a.IsValidTarget(_w.Range));

                    if (LaneWFarm() >= Value.Get("lane.w.min"))
                    {
                        _w.Cast();
                        Orbwalker.ResetAutoAttack();
                    }

                    else if (Value.Use("lane.w.turret") && turrets != null && turrets.NetworkId == target.NetworkId)
                    {
                        _w.Cast();
                        Orbwalker.ResetAutoAttack();
                    }
                }
            }
        }

        #endregion

        #region OnProcessSpellCast

        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if ((args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E ||
                 args.Slot == SpellSlot.R) && sender.IsEnemy && _e.IsReady())
            {
                if (args.SData.TargettingType == SpellDataTargetType.Unit ||
                    args.SData.TargettingType == SpellDataTargetType.SelfAndUnit ||
                    args.SData.TargettingType == SpellDataTargetType.Self)
                {
                    if ((args.Target.NetworkId == Player.Instance.NetworkId && args.Time < 1.5 ||
                         args.End.Distance(Player.Instance.ServerPosition) <= Player.Instance.BoundingRadius*3) &&
                        MainMenu.Misc[args.SData.Name].Cast<CheckBox>().CurrentValue)
                    {
                        _e.Cast();
                    }
                }
                else if (args.SData.TargettingType == SpellDataTargetType.LocationAoe)
                {
                    var castvector =
                        new Geometry.Polygon.Circle(args.End, args.SData.CastRadius).IsInside(
                            Player.Instance.ServerPosition);

                    if (castvector && MainMenu.Misc[args.SData.Name].Cast<CheckBox>().CurrentValue)
                    {
                        _e.Cast();
                    }
                }

                else if (args.SData.TargettingType == SpellDataTargetType.Cone)
                {
                    var castvector =
                        new Geometry.Polygon.Arc(args.Start, args.End, args.SData.CastConeAngle, args.SData.CastRange)
                            .IsInside(Player.Instance.ServerPosition);

                    if (castvector && MainMenu.Misc[args.SData.Name].Cast<CheckBox>().CurrentValue)
                    {
                        _e.Cast();
                    }
                }

                else if (args.SData.TargettingType == SpellDataTargetType.SelfAoe)
                {
                    var castvector =
                        new Geometry.Polygon.Circle(sender.ServerPosition, args.SData.CastRadius).IsInside(
                            Player.Instance.ServerPosition);

                    if (castvector && MainMenu.Misc[args.SData.Name].Cast<CheckBox>().CurrentValue)
                    {
                        _e.Cast();
                    }
                }
                else
                {
                    var castvector =
                        new Geometry.Polygon.Rectangle(args.Start, args.End, args.SData.LineWidth).IsInside(
                            Player.Instance.ServerPosition);

                    if (castvector && MainMenu.Misc[args.SData.Name].Cast<CheckBox>().CurrentValue)
                    {
                        _e.Cast();
                    }
                }
            }
        }

        #endregion

        #region Damage

        private static float QMaxDamage(Obj_AI_Base target)
        {
            if (_q.IsLearned)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    new[] {46.25f, 83.25f, 120.25f, 159.1f, 194.25f}[_q.Level - 1] +
                    (new[] {1.295f, 1.48f, 1.665f, 1.85f, 2.035f}[_q.Level - 1]*Player.Instance.TotalAttackDamage +
                     .925f*Player.Instance.TotalMagicalDamage));
            }
            return 0f;
        }

        private static float QDamage(Obj_AI_Base target)
        {
            if (_q.IsLearned)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    new[] {25f, 45f, 65f, 85f, 105f}[_q.Level - 1] +
                    (new[] {.7f, .8f, .9f, 1f, 1.1f}[_q.Level - 1]*Player.Instance.TotalAttackDamage +
                     .925f*Player.Instance.TotalMagicalDamage));
            }
            return 0f;
        }

        private static float ComboDamage(Obj_AI_Base target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target);

            if (_q.IsReady())
            {
                damage += QMaxDamage(target);
            }
            return damage;
        }

        #endregion

        #region OnBuffGain

        private void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
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

        #endregion 

        #region KS

        private static void Ks()
        {
            var ksq =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a =>
                        a.IsValidTarget(_q.Range) && !a.IsZombie && !a.IsDead &&
                        !a.HasBuffOfType(BuffType.Invulnerability) && !a.HasBuff("ChronoShift") &&
                        a.TotalShieldHealth() <= QMaxDamage(a));

            if (_q.IsReady() && Value.Use("killsteal.q") && Player.Instance.ManaPercent >= Value.Get("killsteal.q.mana"))
            {
                if (ksq != null)
                {
                    _q.Cast(_q.GetPrediction(ksq).CastPosition);
                }
            }
        }

        #endregion

        #region ManaManager

        private static void ManaManagement()
        {
            if (Value.Use("combo.mana.management") && Player.Instance.HealthPercent > Value.Get("combo.mana.health"))
            {
                _qmana = _q.Handle.SData.Mana;
                _wmana = _w.Handle.SData.Mana;
                _rmana = _r.IsReady() ? _r.Handle.SData.Mana : 0;
            }
            else
            {
                _qmana = 0;
                _wmana = 0;
                _rmana = 0;
            }
        }

        private static int LaneWFarm()
        {
            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                Player.Instance.Position, _q.Range);
            var wfarm = minions.Count();
            return wfarm > 0 ? wfarm : 0;
        }

        #endregion

        #endregion

        #region Drawings

        #region SkillDrawings

        private static void GameOnDraw(EventArgs args)
        {
            var colorQ = MainMenu.Draw.GetColor("color.q");
            var widthQ = MainMenu.Draw.GetWidth("width.q");
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

        #region ComboDamage

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

        #endregion

        #endregion
    }
}