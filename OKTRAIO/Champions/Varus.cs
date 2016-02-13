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
    internal class Varus : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Active _w;
        private static Spell.Skillshot _e, _r;
        private static Spell.Chargeable _q;
        private static readonly Vector2 Offset = new Vector2(1, 0);
        private static bool _move;

        public override void Init()
        {
            try
            {
                //spells
                _q = new Spell.Chargeable(SpellSlot.Q, 925, 1600, 1250, 0, 1500, 70)
                {
                    AllowedCollisionCount = int.MaxValue
                };
                _w = new Spell.Active(SpellSlot.W);
                _e = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 250, 1750, 250)
                {
                    AllowedCollisionCount = int.MaxValue
                };
                _r = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Linear, 250, 1200, 120)
                {
                    AllowedCollisionCount = int.MaxValue
                };

                //menu

                //combo test
                MainMenu.ComboKeys(useW: false);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                MainMenu.Combo.AddCheckBox("combo.q.stacks", "Use Q only for stacks", false, true);
                MainMenu.Combo.AddSlider("combo.q.stacks.min", "Min. {0} stacks for Q", 3, 1, 3, true);
                MainMenu.Combo.AddCheckBox("combo.e.stacks", "Use E only for stacks", true, true);
                MainMenu.Combo.AddSlider("combo.e.stacks.min", "Min. {0} stacks for E", 3, 1, 3, true);
                MainMenu.Combo.Add("combo.r.assist",
                    new KeyBind("Semi-Auto Ult", false, KeyBind.BindTypes.HoldActive, 'G'));
                MainMenu.Combo.AddCheckBox("combo.r.aoe", "Use R for AOE", true, true);
                MainMenu.Combo.AddSlider("combo.r.slider", "{0} enemies hit with R snare", 3, 1, 5, true);

                //harass
                MainMenu.HarassKeys(useW: false, useR: false);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddCheckBox("harass.q.stacks", "Use Q only for stacks", false, true);
                MainMenu.Harass.AddSlider("harass.q.stacks.min", "Min. {0} stacks for Q", 3, 1, 3, true);
                MainMenu.Harass.AddCheckBox("harass.e.stacks", "Use E only for stacks", false, true);
                MainMenu.Harass.AddSlider("harass.e.stacks.min", "Min. {0} stacks for E", 3, 1, 3, true);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu", true);
                MainMenu.HarassManaManager(true, false, true, false, 20, 0, 20, 0);

                //laneclear
                MainMenu.LaneKeys(useW: false, useR: false);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddGroupLabel("Harass Preferences", "lane.grouplabel.addonmenu", true);
                MainMenu.Lane.AddCheckBox("lane.q.harass", "Use Q", false, true);
                MainMenu.Lane.AddCheckBox("lane.e.harass", "Use E", false, true);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddCheckBox("lane.q.stacks", "Use Q only for stacks", false, true);
                MainMenu.Lane.AddSlider("lane.q.stacks.min", "Min. {0} stacks for Q", 3, 1, 3, true);
                MainMenu.Lane.AddCheckBox("lane.e.stacks", "Use E only for stacks", false, true);
                MainMenu.Lane.AddSlider("lane.e.stacks.min", "Min. {0} stacks for E", 3, 1, 3, true);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddGroupLabel("Farm Preferences", "lane.grouplabel1.addonmenu", true);
                MainMenu.Lane.AddSlider("lane.q.min", "Min. {0} minions for Q", 3, 1, 7, true);
                MainMenu.Lane.AddSlider("lane.e.min", "Min. {0} minions for E", 3, 1, 7, true);
                MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel2.addonmenu", true);
                MainMenu.LaneManaManager(true, false, true, false, 20, 0, 20, 0);

                //jungleclear
                MainMenu.JungleKeys(useW: false, useR: false);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddSlider("jungle.q.min", "Min. {0} minions for Q", 3, 1, 4, true);
                MainMenu.Jungle.AddSlider("jungle.e.min", "Min. {0} minions for E", 3, 1, 4, true);
                MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu", true);
                MainMenu.JungleManaManager(true, false, true, false, 20, 0, 20, 0);

                //lasthit
                MainMenu.LastHitKeys(false, useW: false, useE: false, useR: false);
                MainMenu.Lasthit.AddCheckBox("lasthit.q.siege", "Use Q on Siege Minions");
                MainMenu.Lasthit.AddSeparator();
                MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu", true);
                MainMenu.LasthitManaManager(true, false, false, false, 15, 0, 0, 0);

                //flee
                MainMenu.FleeKeys(false, useW: false, useR: false);
                MainMenu.Flee.AddSeparator();
                MainMenu.Flee.AddGroupLabel("Mana Manager:", "flee.grouplabel.addonmenu", true);
                MainMenu.FleeManaManager(false, false, true, false, 0, 0, 20, 0);

                //ks
                MainMenu.KsKeys(useW: false, useR: false);
                MainMenu.Ks.AddSeparator();
                MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                MainMenu.KsManaManager(true, false, true, false, 20, 0, 10, 0);

                //misc
                MainMenu.MiscMenu();
                MainMenu.Misc.AddCheckBox("misc.r.interrupt", "Use R to Interrupt");
                MainMenu.Misc.AddCheckBox("misc.e.gapcloser", "Use E for Anti-Gapcloser");
                MainMenu.Misc.AddCheckBox("misc.r.auto", "Anti Rengar + Khazix");
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Auto Q/E Settings", "misc.grouplabel.addonmenu", true);
                MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.knockup", "Use Q on Knocked Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.suppression", "Use Q on Suppressed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddCheckBox("misc.e.charm", "Use E on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.e.stun", "Use E on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.e.knockup", "Use E on Knocked Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.e.snare", "Use E on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.e.suppression", "Use E on Suppressed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.e.taunt", "Use E on Taunted Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Prediction", "misc.grouplabel1.addonmenu", true);
                MainMenu.Misc.AddSlider("misc.q.prediction", "Hitchance Percentage for Q", 80, 0, 100, true);
                MainMenu.Misc.AddSlider("misc.e.prediction", "Hitchance Percentage for E", 80, 0, 100, true);
                MainMenu.Misc.AddSlider("misc.r.prediction", "Hitchance Percentage for R", 80, 0, 100, true);

                //draw
                MainMenu.DrawKeys(useW: false);
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
                Drawing.OnDraw += GameOnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                Obj_AI_Base.OnBuffGain += Player_OnBuffGain;
                Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
                GameObject.OnCreate += GameObject_OnCreate;
                Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
                Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
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
            var targetq = TargetSelector.GetTarget(_q.MaximumRange, DamageType.Physical);
            var targete = TargetSelector.GetTarget(_e.Range, DamageType.Physical);
            var targetr = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

            if (_q.IsReady())
            {
                if (targetq != null && Value.Use("combo.q".AddName()))
                {
                    var qpred = _q.GetPrediction(targetq);

                    if (Value.Use("combo.q.stacks"))
                    {
                        if (targetq.GetBuffCount("varuswdebuff") >= Value.Get("combo.q.stacks.min"))
                        {
                            if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                            {
                                _q.Cast(qpred.CastPosition);
                            }
                            else
                            {
                                _q.StartCharging();
                            }
                        }
                    }
                    else
                    {
                        if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                        {
                            _q.Cast(qpred.CastPosition);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }
                }
            }

            if (_e.IsReady())
            {
                if (targete != null && Value.Use("combo.e".AddName()))
                {
                    var epred = _e.GetPrediction(targete);

                    if (Value.Use("combo.e.stacks"))
                    {
                        if (targete.GetBuffCount("varuswdebuff") >= Value.Get("combo.e.stacks.min") &&
                            epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                        {
                            _e.Cast(epred.CastPosition);
                        }
                    }

                    else
                    {
                        if (epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                        {
                            _e.Cast(epred.CastPosition);
                        }
                    }
                }
            }

            if (_r.IsReady())
            {
                if (targetr != null)
                {
                    var rpred = _r.GetPrediction(targetr);

                    if (Value.Use("combo.r".AddName()))
                    {
                        if (targetr.TotalShieldHealth() <= ComboDamage(targetr) &&
                            rpred.HitChancePercent >= Value.Get("misc.r.prediction") && !Overkill(targetr))
                        {
                            _r.Cast(rpred.CastPosition);
                        }
                    }

                    if (Value.Use("combo.r.aoe"))
                    {
                        if (targetr.CountEnemiesInRange(550) >= Value.Get("combo.r.slider") &&
                            rpred.HitChancePercent >= Value.Get("misc.r.prediction"))
                        {
                            _r.Cast(rpred.CastPosition);
                        }
                    }
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            var targetq = TargetSelector.GetTarget(_q.MaximumRange, DamageType.Physical);
            var targete = TargetSelector.GetTarget(_e.Range, DamageType.Physical);

            if (_q.IsReady())
            {
                if (targetq != null && _q.IsReady() && Value.Use("harass.q") &&
                    Player.Instance.ManaPercent >= Value.Get("harass.q.mana") || _q.IsCharging && targetq != null)
                {
                    var qpred = _q.GetPrediction(targetq);

                    if (Value.Use("harass.q.stacks"))
                    {
                        if (targetq.GetBuffCount("varuswdebuff") >= Value.Get("harass.q.stacks.min"))
                        {
                            if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                            {
                                _q.Cast(qpred.CastPosition);
                            }
                            else
                            {
                                _q.StartCharging();
                            }
                        }
                    }
                    else
                    {
                        if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                        {
                            _q.Cast(qpred.CastPosition);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }
                }
            }

            if (_e.IsReady())
            {
                if (targete != null && Value.Use("harass.e") &&
                    Player.Instance.ManaPercent >= Value.Get("harass.e.mana"))
                {
                    var epred = _e.GetPrediction(targete);

                    if (Value.Use("harass.e.stacks"))
                    {
                        if (targete.GetBuffCount("varuswdebuff") >= Value.Get("harass.e.stacks.min") &&
                            epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                        {
                            _e.Cast(epred.CastPosition);
                        }
                    }

                    else
                    {
                        if (epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                        {
                            _e.Cast(epred.CastPosition);
                        }
                    }
                }
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            if (Player.Instance.CountEnemiesInRange(_q.MaximumRange) == 0)
            {
                if (_q.IsReady())
                {
                    if (Value.Use("lane.q") && Player.Instance.ManaPercent >= Value.Get("lane.q.mana") ||
                        _q.IsCharging)
                    {
                        var minionq = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                            Player.Instance.Position, _q.MaximumRange);
                        var qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionq, _q.Width,
                            (int) _q.MaximumRange);

                        if (qfarm.HitNumber >= Value.Get("lane.q.min"))
                        {
                            if (_q.IsCharging && _q.IsFullyCharged)
                            {
                                _q.Cast(qfarm.CastPosition);
                            }
                            else
                            {
                                _q.StartCharging();
                            }
                        }
                    }
                }

                if (_e.IsReady())
                {
                    if (Value.Use("lane.e") && Player.Instance.ManaPercent >= Value.Get("lane.e.mana"))
                    {
                        var minione = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                            Player.Instance.Position, _e.Range);
                        var efarm = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minione, _e.Width,
                            (int) _e.Range);

                        if (efarm.HitNumber >= Value.Get("lane.e.min"))
                        {
                            _e.Cast(efarm.CastPosition);
                        }
                    }
                }
            }

            else
            {
                var targetq = TargetSelector.GetTarget(_q.MaximumRange, DamageType.Physical);
                var targete = TargetSelector.GetTarget(_e.Range, DamageType.Physical);

                if (_q.IsReady())
                {
                    if (targetq != null && Value.Use("lane.q.harass") &&
                        Player.Instance.ManaPercent >= Value.Get("lane.q.mana") || _q.IsCharging && targetq != null)
                    {
                        var qpred = _q.GetPrediction(targetq);

                        if (Value.Use("lane.q.stacks"))
                        {
                            if (targetq.GetBuffCount("varuswdebuff") >= Value.Get("lane.q.stacks.min"))
                            {
                                if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                                {
                                    _q.Cast(qpred.CastPosition);
                                }
                                else
                                {
                                    _q.StartCharging();
                                }
                            }
                        }
                        else
                        {
                            if (_q.IsCharging && qpred.HitChancePercent >= Value.Get("misc.q.prediction"))
                            {
                                _q.Cast(qpred.CastPosition);
                            }
                            else
                            {
                                _q.StartCharging();
                            }
                        }
                    }
                }

                if (_e.IsReady())
                {
                    if (targete != null && Value.Use("lane.e.harass") &&
                        Player.Instance.ManaPercent >= Value.Get("lane.e.mana"))
                    {
                        var epred = _e.GetPrediction(targete);

                        if (Value.Use("lane.e.stacks"))
                        {
                            if (targete.GetBuffCount("varuswdebuff") >= Value.Get("lane.e.stacks.min") &&
                                epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                            {
                                _e.Cast(epred.CastPosition);
                            }
                        }

                        else
                        {
                            if (epred.HitChancePercent >= Value.Get("misc.e.prediction"))
                            {
                                _e.Cast(epred.CastPosition);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Jungleclear

        public override void Jungleclear()
        {
            if (_q.IsReady())
            {
                if (Value.Use("jungle.q") && Player.Instance.ManaPercent >= Value.Get("jungle.q.mana") || _q.IsCharging)
                {
                    var bigmonsters =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .FirstOrDefault(
                                a =>
                                    a.IsValidTarget(_q.MaximumRange) &&
                                    Variables.SummonerRiftJungleList.Contains(a.BaseSkinName) &&
                                    a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.Q));
                    var minionq = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                        _q.MaximumRange);
                    var qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionq, _q.Width,
                        (int) _q.MaximumRange);

                    if (qfarm.HitNumber >= Value.Get("jungle.q.min"))
                    {
                        if (_q.IsCharging && _q.IsFullyCharged)
                        {
                            _q.Cast(qfarm.CastPosition);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }

                    else if (bigmonsters != null)
                    {
                        if (_q.IsCharging && _q.IsFullyCharged)
                        {
                            _q.Cast(bigmonsters);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }
                }
            }

            if (_e.IsReady())
            {
                if (Value.Use("jungle.e") && Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                {
                    var bigmonsters =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .FirstOrDefault(
                                a =>
                                    a.IsValidTarget(_q.MaximumRange) &&
                                    Variables.SummonerRiftJungleList.Contains(a.BaseSkinName) &&
                                    a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.E));
                    var minione = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, _e.Range);
                    var efarm = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minione, _e.Width,
                        (int) _e.Range);

                    if (efarm.HitNumber >= Value.Get("jungle.e.min"))
                    {
                        _e.Cast(efarm.CastPosition);
                    }

                    else if (bigmonsters != null)
                    {
                        _e.Cast(bigmonsters);
                    }
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var targete = TargetSelector.GetTarget(_e.Range, DamageType.Physical);

            if (_e.IsReady())
            {
                if (targete != null && Value.Use("flee.e"))
                {
                    _e.Cast(_e.GetPrediction(targete).CastPosition);
                }
            }
        }

        #endregion

        #region Lasthit

        public override void LastHit()
        {
            var siege =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .FirstOrDefault(a => a.IsValidTarget(_q.MaximumRange) && a.BaseSkinName.Contains("Siege"));

            if (_q.IsReady())
            {
                if (Value.Use("lasthit.q.siege") &&
                    Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
                {
                    if (siege != null && siege.Health <= Player.Instance.GetSpellDamage(siege, SpellSlot.Q))
                    {
                        if (_q.IsCharging)
                        {
                            _q.Cast(siege);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Utils

        #region OnUpdate

        private static void GameOnUpdate(EventArgs args)
        {
            if (_move &&
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)))
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            Ks();

            AutoQr();

            RAsssist();
        }

        #endregion

        #region AntiGapCloser

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var rengar = EntityManager.Heroes.Enemies.Find(r => r.ChampionName.Equals("Rengar"));
            var khazix = EntityManager.Heroes.Enemies.Find(z => z.ChampionName.Equals("Khazix"));

            try
            {
                if (Value.Use("misc.r.auto"))
                {
                    if (khazix != null)
                    {
                        if (sender.Name == "Khazix_Base_E_Tar.troy" &&
                            sender.Position.Distance(Player.Instance) <= 400)
                            _r.Cast(khazix);
                    }
                    if (rengar == null) return;
                    if (sender.Name == "Rengar_LeapSound.troy" && sender.Position.Distance(Player.Instance) < 600)
                        _r.Cast(rengar);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code ONCREATE)</font>");
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsAlly || !Value.Use("misc.e.gapcloser"))
            {
                return;
            }

            if (_e.IsReady() && _e.IsInRange(sender) && e.End.Distance(Player.Instance.Position) <= 100)
            {
                _e.Cast(_e.GetPrediction(sender).CastPosition);
            }
        }

        #endregion

        #region Interrupter

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsAlly || e.Slot == SpellSlot.R && sender.BaseSkinName == "Katarina" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Janna" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Fiddlesticks" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Velkoz" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Pantheon" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Karthus" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Nunu" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Malzahar" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Caitlyn" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Galio" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Xerath" ||
                e.Slot == SpellSlot.R && sender.BaseSkinName == "Miss Fortune")
            {
                return;
            }

            if (_r.IsReady() && Value.Use("misc.r.interrupt"))
            {
                if (e.DangerLevel == DangerLevel.High && _r.IsInRange(sender))
                {
                    _r.Cast(_r.GetPrediction(sender).CastPosition);
                }
            }
        }

        #endregion

        #region Buffs

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (sender.IsMe && args.Buff.Name == "VarusQ")
            {
                _move = false;
            }
        }

        private static void Player_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe && args.Buff.Name == "VarusQ")
            {
                _move = true;
            }
        }

        #endregion

        #region Damage

        private static float ComboDamage(Obj_AI_Base target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target);

            if (_q.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);
            }

            if (_e.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.E);
            }

            if (_r.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.R);
            }

            return damage;
        }

        private static bool Overkill(Obj_AI_Base enemy)
        {
            var damage = Player.Instance.GetAutoAttackDamage(enemy);

            if (_r.IsReady())
            {
                if (enemy != null)
                {
                    var qpred = _q.GetPrediction(enemy);
                    var epred = _e.GetPrediction(enemy);

                    if (_q.IsReady())
                    {
                        if (_q.IsInRange(enemy) && qpred.HitChance >= HitChance.Medium)
                        {
                            damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.Q);
                        }
                    }

                    if (_e.IsReady())
                    {
                        if (_e.IsInRange(enemy) && epred.HitChance >= HitChance.Medium)
                        {
                            damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.E);
                        }
                    }
                }
            }
            return enemy.TotalShieldHealth() <= damage;
        }

        #endregion

        #region KillSteal

        private static void Ks()
        {
            var targetq = TargetSelector.GetTarget(_q.MaximumRange, DamageType.Physical);
            var targete = TargetSelector.GetTarget(_e.Range, DamageType.Physical);

            if (_q.IsReady())
            {
                if (targetq != null && Value.Use("killsteal.q") &&
                    Player.Instance.ManaPercent >= Value.Get("killsteal.q.mana") || _q.IsCharging && targetq != null)
                {
                    var qpred = _q.GetPrediction(targetq);
                    if (!targetq.IsDead &&
                        targetq.TotalShieldHealth() <= Player.Instance.GetSpellDamage(targetq, SpellSlot.Q))
                    {
                        if (_q.IsCharging && qpred.HitChance >= HitChance.High)
                        {
                            _q.Cast(qpred.CastPosition);
                        }
                        else
                        {
                            _q.StartCharging();
                        }
                    }
                }
            }

            if (_e.IsReady())
            {
                if (targete != null && Value.Use("killsteal.e") &&
                    Player.Instance.ManaPercent >= Value.Get("killsteal.e.mana"))
                {
                    var epred = _e.GetPrediction(targete);

                    if (!targete.IsDead &&
                        targete.TotalShieldHealth() <= Player.Instance.GetSpellDamage(targete, SpellSlot.E) &&
                        epred.HitChance >= HitChance.High)
                    {
                        _e.Cast(epred.CastPosition);
                    }
                }
            }
        }

        #endregion

        #region Ultimate Assistant

        private static void RAsssist()
        {
            var targetr = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

            if (_r.IsReady())
            {
                if (Value.Active("combo.r.assist") && targetr != null)
                {
                    _r.Cast(_r.GetPrediction(targetr).CastPosition);
                }
            }
        }

        #endregion

        #region Autospell

        private static void AutoQr()
        {
            var targetq =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.IsValidTarget(_q.MaximumRange) &&
                         (a.HasBuffOfType(BuffType.Charm) || a.HasBuffOfType(BuffType.Knockup) ||
                          a.HasBuffOfType(BuffType.Snare) || a.HasBuffOfType(BuffType.Stun) ||
                          a.HasBuffOfType(BuffType.Suppression) || a.HasBuffOfType(BuffType.Taunt)));
            var targete =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.IsValidTarget(_e.Range) &&
                         (a.HasBuffOfType(BuffType.Charm) || a.HasBuffOfType(BuffType.Knockup) ||
                          a.HasBuffOfType(BuffType.Snare) || a.HasBuffOfType(BuffType.Stun) ||
                          a.HasBuffOfType(BuffType.Suppression) || a.HasBuffOfType(BuffType.Taunt)));

            if (_q.IsReady())
            {
                if (targetq != null &&
                    (Value.Use("misc.q.charm") && targetq.IsCharmed || Value.Use("misc.q.knockup") ||
                     Value.Use("misc.q.stun") && targetq.IsStunned || Value.Use("misc.q.snare") && targetq.IsRooted ||
                     Value.Use("misc.q.suppression") && targetq.IsSuppressCallForHelp ||
                     Value.Use("misc.q.taunt") && targetq.IsTaunted))
                {
                    if (_q.IsCharging)
                    {
                        _q.Cast(_q.GetPrediction(targetq).CastPosition);
                    }
                    else
                    {
                        _q.StartCharging();
                    }
                }
            }

            if (_e.IsReady())
            {
                if (targete != null &&
                    (Value.Use("misc.e.charm") && targete.IsCharmed || Value.Use("misc.e.knockup") ||
                     Value.Use("misc.e.stun") && targete.IsStunned || Value.Use("misc.e.snare") && targete.IsRooted ||
                     Value.Use("misc.e.suppression") && targete.IsSuppressCallForHelp ||
                     Value.Use("misc.e.taunt") && targete.IsTaunted))
                {
                    _e.Cast(_e.GetPrediction(targete).CastPosition);
                }
            }
        }

        #endregion

        #endregion

        #region Drawings

        private static void GameOnDraw(EventArgs args)
        {
            var colorQ = MainMenu.Draw.GetColor("color.q");
            var widthQ = MainMenu.Draw.GetWidth("width.q");
            var colorE = MainMenu.Draw.GetColor("color.e");
            var widthE = MainMenu.Draw.GetWidth("width.e");
            var colorR = MainMenu.Draw.GetColor("color.r");
            var widthR = MainMenu.Draw.GetWidth("width.r");

            if (!Value.Use("draw.disable"))
            {
                if (Value.Use("draw.q") && ((Value.Use("draw.ready") && _w.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorQ,
                        Radius = _q.MaximumRange,
                        BorderWidth = widthQ
                    }.Draw(Player.Instance.Position);
                }

                if (Value.Use("draw.e") && ((Value.Use("draw.ready") && _w.IsReady()) || !Value.Use("draw.ready")))
                {
                    new Circle
                    {
                        Color = colorE,
                        Radius = _e.Range,
                        BorderWidth = widthE
                    }.Draw(Player.Instance.Position);
                }

                if (Value.Use("draw.r") && ((Value.Use("draw.ready") && _w.IsReady()) || !Value.Use("draw.ready")))
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
    }
}