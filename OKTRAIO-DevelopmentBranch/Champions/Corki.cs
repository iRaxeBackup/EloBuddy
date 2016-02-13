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
    internal class Corki : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Skillshot _q, _w, _w2, _r;
        private static Spell.Active _e;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            try
            {
                //Creating Spells
                _q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 300, 1000, 250)
                {
                    MinimumHitChance = HitChance.Medium
                    ,
                    AllowedCollisionCount = int.MaxValue
                };
                _w = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Linear);
                _w2 = new Spell.Skillshot(SpellSlot.W, 1800, SkillShotType.Linear);
                _e = new Spell.Active(SpellSlot.E, 600);
                _r = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 200, 1950, 40)
                {
                    AllowedCollisionCount = 0
                };


                try
                {
                    //Combo Menu Settings
                    MainMenu.ComboKeys(defaultW: false);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                    MainMenu.Combo.AddSlider("combo.e.range", "Min range for cast E", 600, 100, 600, true);
                    MainMenu.Combo.AddSlider("combo.w.e", "Max enemies for Jump", 1, 0, 5, true);
                    MainMenu.Combo.AddSlider("combo.q.damage", "Q Overkill", 60, 0, 500, true);
                    MainMenu.Combo.AddSlider("combo.r.damage", "R Overkill", 50, 0, 500, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Prediction Settings", "combo.grouplabel.1", true);
                    MainMenu.Combo.AddSlider("combo.q.prediction", "Use Q if Hitchance > {0}%", 80, 0, 100, true);
                    MainMenu.Combo.AddCheckBox("combo.q.multipred", "Use Q OKTR MultiTarget Prediction", true, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Mana Manager:", "combo.grouplabel.addonmenu.1", true);
                    MainMenu.ComboManaManager(true, true, true, true, 20, 10, 10, 5);

                    //Lane Clear Menu Settings
                    MainMenu.LaneKeys(defaultW: false);
                    MainMenu.Lane.AddSeparator();
                    MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.addonmenu.1", true);
                    MainMenu.LaneManaManager(true, true, true, true, 80, 80, 80, 50);
                    MainMenu.Lane.AddSlider("lane.stacks", "Limit Rockets to", 3, 0, 6, true);

                    //Jungle Clear Menu Settings
                    MainMenu.JungleKeys(defaultW: false);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Jungleclear Preferences", "jungle.grouplabel.addonmenu", true);
                    MainMenu.Jungle.AddCheckBox("jungle.monsters.spell", "Use Abilities on Big Monster", true, true);
                    MainMenu.Jungle.AddCheckBox("jungle.minimonsters.spell", "Use Abilities on Mini Monsters", false,
                        true);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu.1", true);
                    MainMenu.JungleManaManager(true, true, true, true, 40, 80, 50, 40);
                    MainMenu.Jungle.AddSlider("jungle.stacks", "Limit Rockets to", 3, 0, 6, true);

                    //Last hit Menu Settings
                    MainMenu.LastHitKeys(useW: false, useE: false);
                    MainMenu.Lasthit.AddSeparator();
                    MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu", true);
                    MainMenu.LasthitManaManager(true, false, false, true, 70, 90, 60, 50);
                    MainMenu.Lasthit.AddSlider("lasthit.stacks", "Limit Rockets to", 3, 0, 6, true);

                    //Harras
                    MainMenu.HarassKeys(defaultW: false);
                    MainMenu.Harass.AddSeparator();
                    MainMenu.Harass.AddGroupLabel("Harass Preferences", "harass.grouplabel.addonmenu.12", true);
                    MainMenu.Harass.AddSlider("harass.e.range", "Min range for cast E", 600, 100, 600, true);
                    MainMenu.Harass.AddSeparator();
                    MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu", true);
                    MainMenu.HarassManaManager(true, true, true, true, 60, 80, 50, 40);
                    MainMenu.Harass.AddSlider("harass.stacks", "Limit Rockets to", 3, 0, 6, true);

                    //Flee Menu
                    MainMenu.FleeKeys(false, useE: false);
                    MainMenu.Flee.AddSeparator();
                    MainMenu.Flee.AddGroupLabel("HP Manager Preferences", "flee.grouplabel.addonmenu", true);
                    MainMenu.Flee.AddSlider("flee.r.hp", "Use R when your HP are less than {0}%", 20, 0, 100, true);

                    //Ks
                    MainMenu.KsKeys(useE: false, defaultW: false);
                    MainMenu.Ks.AddSeparator();
                    MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                    MainMenu.KsManaManager(true, true, false, true, 20, 30, 10, 5);

                    //Misc Menu
                    MainMenu.MiscMenu();
                    MainMenu.Misc.AddCheckBox("misc.q", "Use Auto Q");
                    MainMenu.Misc.AddCheckBox("misc.w.gapcloser", "Use Auto W on GapCloser", false);
                    MainMenu.Misc.AddCheckBox("misc.r", "Use Auto R");
                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddGroupLabel("Auto Q Settings", "misc.grouplabel.addonmenu", true);
                    MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.r.stun", "Use R on Stunned Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.r.charm", "Use R on Charmed Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.r.taunt", "Use R on Taunted Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.fear", "Use Q on Feared Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.r.fear", "Use R on Feared Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.r.snare", "Use R on Snared Enemy", true, true);
                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddGroupLabel("Mana Manager:", "misc.grouplabel.addonmenu.1", true);
                    MainMenu.Misc.AddSlider("misc.q.mana", "Use Q on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                        true);
                    MainMenu.Misc.AddSlider("misc.r.mana", "Use R on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                        true);

                    //Draw Menu
                    MainMenu.DrawKeys();
                    MainMenu.Draw.AddSeparator();
                    MainMenu.Draw.AddCheckBox("draw.hp.bar", "Draw Combo Damage", true, true);
                    Value.Init();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code MENU)</font>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 503)</font>");
            }

            try
            {
                if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
                {
                    Game.OnUpdate += GameOnUpdate;
                }
                else
                {
                    Game.OnTick += GameOnUpdate;
                }
                Obj_AI_Base.OnBuffGain += BuffGain;
                Gapcloser.OnGapcloser += AntiGapCloser;
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
            var target = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

            if (target == null || target.IsZombie) return;

            if (Player.Instance.ManaPercent >= Value.Get("combo.e.mana"))
            {
                if (Value.Use("combo.e".AddName()) && _e.IsReady() &&
                    target.Distance(Player.Instance) <= Value.Get("combo.e.range"))
                {
                    _e.Cast();
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("combo.r.mana"))
            {
                if (Value.Use("combo.r".AddName()) && _r.IsReady() && _r.Handle.Ammo > 0)
                {
                    _r.Cast(_r.GetPrediction(target).CastPosition);
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("combo.q.mana"))
            {
                if (Value.Use("combo.q".AddName()) && _q.IsReady())
                {
                    if (_q.GetPrediction(target).HitChancePercent >= Value.Get("combo.q.prediction"))
                    {
                        if (Value.Use("combo.q.multipred"))
                        {
                            var optimizedCircleLocation = OKTRGeometry.GetOptimizedCircleLocation(_q, target);
                            if (optimizedCircleLocation != null)
                                _q.Cast(optimizedCircleLocation.Value.Position.To3D());
                        }
                        else
                        {
                            _q.Cast(_q.GetPrediction(target).CastPosition);
                        }
                    }
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("combo.w.mana"))
            {
                if (Value.Use("combo.w".AddName()) && target.CountEnemiesInRange(800) <= Value.Get("combo.w.e"))
                {
                    if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                    {
                        _w2.Cast(_w2.GetPrediction(target).CastPosition);
                    }
                    if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                    {
                        _w.Cast(_w.GetPrediction(target).CastPosition);
                    }
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);

            if (target == null || target.IsZombie) return;

            if (Player.Instance.ManaPercent >= Value.Get("harass.q.mana"))
            {
                if (Value.Use("harass.q") && _q.IsReady())
                {
                    _q.Cast(_q.GetPrediction(target).CastPosition);
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("harass.w.mana"))
            {
                if (Value.Use("harass.w"))
                {
                    if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                    {
                        _w2.Cast(_w2.GetPrediction(target).CastPosition);
                    }
                    if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                    {
                        _w.Cast(_w.GetPrediction(target).CastPosition);
                    }
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("harass.e.mana") &&
                target.Distance(Player.Instance) <= Value.Get("harass.e.range"))
            {
                if (Value.Use("harass.e") && _e.IsReady())
                {
                    _e.Cast();
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("harass.r.mana") &&
                _r.Handle.Ammo >= Value.Get("harass.stacks"))
            {
                if (Value.Use("harass.r") && _r.IsReady())
                {
                    _r.Cast(_r.GetPrediction(target).CastPosition);
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
            if (monsters != null)
            {
                if (Value.Use("jungle.monsters.spell"))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                    {
                        if (Value.Use("jungle.q") && _q.IsReady())
                        {
                            _q.Cast(monsters);
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                    {
                        if (Value.Use("jungle.w") && _w.IsReady())
                        {
                            if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                            {
                                _w2.Cast(monsters);
                            }
                            if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                            {
                                _w.Cast(monsters);
                            }
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                    {
                        if (Value.Use("jungle.e") && _e.IsReady())
                        {
                            _e.Cast();
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.r.mana"))
                    {
                        if (Value.Use("jungle.r") && _r.IsReady() && _r.Handle.Ammo >= Value.Get("jungle.stacks"))
                        {
                            _r.Cast(monsters);
                        }
                    }
                }
            }

            if (fappamonsters != null)
            {
                if (Value.Use("jungle.minimonsters.spell"))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                    {
                        if (Value.Use("jungle.q") && _q.IsReady())
                        {
                            _q.Cast(fappamonsters);
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                    {
                        if (Value.Use("jungle.w") && _w.IsReady())
                        {
                            if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                            {
                                _w2.Cast(fappamonsters);
                            }
                            if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                            {
                                _w.Cast(fappamonsters);
                            }
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                    {
                        if (Value.Use("jungle.e") && _e.IsReady())
                        {
                            _e.Cast();
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("jungle.r.mana"))
                    {
                        if (Value.Use("jungle.r") && _r.IsReady() && _r.Handle.Ammo >= Value.Get("jungle.stacks"))
                        {
                            _r.Cast(fappamonsters);
                        }
                    }
                }
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    Player.Instance.AttackRange, false).Count();

            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    Player.Instance.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            var predQ = OKTRGeometry.GetOptimizedCircleLocation(
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, _q.Range, false).Select(m => m.Position.To2D()).ToList(),
                _q.Width, _q.Range).Position.To3D();

            if (count == 0) return;

            if (Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
            {
                if (Value.Use("lane.q") && _q.IsReady())
                {
                    _q.Cast(predQ);
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("lane.w.mana"))
            {
                if (Value.Use("lane.w") && _w.IsReady())
                {
                    if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                    {
                        _w2.Cast(source);
                    }
                    if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                    {
                        _w.Cast(source);
                    }
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("lane.e.mana"))
            {
                if (Value.Use("lane.e") && _e.IsReady())
                {
                    _e.Cast();
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("lane.r.mana"))
            {
                if (Value.Use("lane.r") && _r.IsReady() && _r.Handle.Ammo >= Value.Get("lane.stacks"))
                {
                    _r.Cast(source);
                }
            }
        }

        #endregion

        #region Lasthit

        public override void LastHit()
        {
            var predQ = OKTRGeometry.GetOptimizedCircleLocation(
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m =>
                        m.IsValidTarget(_q.Range) &&
                        Player.Instance.GetSpellDamage(m, SpellSlot.Q) > m.TotalShieldHealth())
                    .Select(m => m.Position.To2D())
                    .ToList(),
                _q.Width, _q.Range).Position.To3D();

            var source =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m =>
                        m.IsValidTarget(_r.Range) &&
                        Player.Instance.GetSpellDamage(m, SpellSlot.R) > m.TotalShieldHealth())
                    .FirstOrDefault();

            if (source == null) return;

            if (Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
            {
                if (Value.Use("lasthit.q") && _q.IsReady())
                {
                    _q.Cast(predQ);
                }
            }
            if (Player.Instance.ManaPercent >= Value.Get("lasthit.r.mana"))
            {
                if (Value.Use("lasthit.q") && _q.IsReady())
                {
                    _r.Cast(source);
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var target = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

            if (Value.Use("flee.w"))
            {
                if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                {
                    _w2.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _w.Range).To3D());
                }
                if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                {
                    _w.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _w.Range).To3D());
                }
            }
            if (Value.Use("flee.r"))
            {
                if (Player.Instance.HealthPercent <= Value.Get("flee.r.hp"))
                {
                    if (target != null && _r.IsReady())
                    {
                        _r.Cast(_r.GetPrediction(target).CastPosition);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Utils

        #region OnUpdate

        private void GameOnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.HasBuff("Recall")
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed ||
                Orbwalker.IsAutoAttacking)
                return;

            if (Value.Use("killsteal.q") && _q.IsReady() ||
                Value.Use("killsteal.w") && _w.IsReady() ||
                Value.Use("killsteal.r") && _r.IsReady())
            {
                KillSteal();
            }
        }

        #endregion

        #region OnBuffGain

        private void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe) return;

            if (Value.Use("misc.q") && _q.IsReady())
            {
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
            if (Value.Use("misc.r") && _r.IsReady())
            {
                if (Player.Instance.IsInRange(sender, _r.Range) && Value.Get("misc.r.mana") >= Player.Instance.ManaPercent)
                {
                    if (Value.Use("misc.r.stun") && sender.IsStunned)
                    {
                        _r.Cast(sender);
                    }
                    if (Value.Use("misc.r.snare") && sender.IsRooted)
                    {
                        _r.Cast(sender);
                    }
                    if (Value.Use("misc.r.charm") && sender.IsCharmed)
                    {
                        _r.Cast(sender);
                    }
                    if (Value.Use("misc.r.taunt") && sender.IsTaunted)
                    {
                        _r.Cast(sender);
                    }
                    if (Value.Use("misc.r.fear") && sender.IsFeared)
                    {
                        _r.Cast(sender);
                    }
                }
            }
        }

        #endregion

        #region AntiGapCloser

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            try
            {
                if (!e.Sender.IsValidTarget() || !Value.Use("misc.w.gapcloser") || e.Sender.Type != Player.Instance.Type ||
                    !e.Sender.IsEnemy)
                    return;

                if (_w.IsReady() && _w.IsInRange(sender) && Value.Use("misc.w.gapcloser"))
                {
                    var pred = e.End;

                    _w.Cast(pred + 5*(Player.Instance.Position - e.End));
                }
            }

            catch (Exception a)
            {
                Console.WriteLine(a);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code ANTIGAP)</font>");
            }
        }

        #endregion

        #region Killsteal

        private static void KillSteal()
        {
            try
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies.Where(
                            hero =>
                                hero.IsValidTarget(_r.Range) && !hero.IsDead && !hero.IsZombie &&
                                hero.HealthPercent <= 25))
                {
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.q.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.Q))

                        {
                            _q.Cast(_q.GetPrediction(target).CastPosition);
                        }
                    }

                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.w.mana"))
                    {
                        var tawah =
                            EntityManager.Turrets.Enemies.FirstOrDefault(
                                a =>
                                    !a.IsDead && a.Distance(target) <= 775 + Player.Instance.BoundingRadius +
                                    target.BoundingRadius/2 + 44.2);

                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.W) &&
                            target.Position.CountEnemiesInRange(800) == 1 && tawah == null &&
                            Player.Instance.Mana >= 120)
                        {
                            if (Player.Instance.HasBuff("CorkiLoaded") && _w2.IsReady())
                            {
                                _w2.Cast(_w2.GetPrediction(target).CastPosition);
                            }
                            if (!Player.Instance.HasBuff("CorkiLoaded") && _w.IsReady())
                            {
                                _w.Cast(_w.GetPrediction(target).CastPosition);
                            }
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.r.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.R))

                        {
                            _r.Cast(_r.GetPrediction(target).CastPosition);
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code KILLSTEAL)</font>");
            }
        }

        #endregion

        #region Damage

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
                if (Player.Instance.HasBuff("mbcheck2"))
                {
                    damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R)*.5f;
                }
                else
                {
                    damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
                }
            }

            return damage;
        }

        #endregion

        #endregion

        #region Drawings

        private static void GameOnDraw(EventArgs args)
        {
            var colorQ = MainMenu.Draw.GetColor("color.q");
            var widthQ = MainMenu.Draw.GetWidth("width.q");
            var colorW = MainMenu.Draw.GetColor("color.w");
            var widthW = MainMenu.Draw.GetWidth("width.w");
            var colorE = MainMenu.Draw.GetColor("color.e");
            var widthE = MainMenu.Draw.GetWidth("width.e");
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

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Value.Use("draw.hp.bar"))
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && a.IsHPBarRendered))
                {
                    var damage = ComboDamage(enemy);
                    var damagepercent = (enemy.TotalShieldHealth() - damage > 0
                        ? enemy.TotalShieldHealth() - damage
                        : 0)/(enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
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
    }
}