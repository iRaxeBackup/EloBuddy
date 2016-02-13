using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;
using SharpDX;

namespace OKTRAIO.Champions
{
    internal class Kalista : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Skillshot _q;
        private static Spell.Targeted _w;
        private static Spell.Active _e, _r;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            try
            {
                //Creating Spells
                _q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2500, 40);
                _w = new Spell.Targeted(SpellSlot.W, 5000);
                _e = new Spell.Active(SpellSlot.E, 950);
                _r = new Spell.Active(SpellSlot.R, 1400);


                try
                {
                    //Combo Menu Settings
                    MainMenu.ComboKeys(useW: false, useR: false);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                    MainMenu.Combo.AddCheckBox("combo.aa", "Force autoattack on W-E Buff", true, true);
                    MainMenu.Combo.AddCheckBox("combo.q.aa", "Use Q after autoattack", true, true);
                    MainMenu.Combo.AddCheckBox("combo.q.collision", "Use Q checking collision", true, true);
                    MainMenu.Combo.AddCheckBox("combo.q.minions", "Use Q through minions for hit the target", true,
                        true);
                    MainMenu.Combo.AddCheckBox("combo.e.auto", "Use E if target leave the range", false, true);
                    MainMenu.Combo.AddCheckBox("combo.e.death", "Use E if u are nearly to death", false, true);
                    MainMenu.Combo.AddSlider("combo.e.range", "Min range for Use E {0} ", 600, 100, 600, true);
                    MainMenu.Combo.AddSlider("combo.e.damage", "E Overkill", 60, 0, 500, true);
                    MainMenu.Combo.AddSlider("combo.e.damage.me", "Use E if HP goes under {0} ", 10, 0, 100, true);
                    MainMenu.Combo.AddCheckBox("combo.r.allin.prevent", "Use R if the ally can use R too", false, true);
                    MainMenu.Combo.AddCheckBox("combo.r.allin", "Use R if u guys can kill those faggs", false, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Prediction Settings", "combo.grouplabel.addonmenu.2", true);
                    MainMenu.Combo.AddSlider("combo.q.prediction", "Use Q if Hitchance > {0}%", 80, 0, 100, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Mana Manager:", "combo.grouplabel.addonmenu.3", true);
                    MainMenu.Combo.AddCheckBox("combo.e.save.mana", "Save mana for Use E", true, true);
                    MainMenu.Combo.AddCheckBox("combo.r.save.mana", "Save mana for Use R", true, true);
                    MainMenu.ComboManaManager(true, false, true, true, 20, 10, 10, 5);

                    //Lane Clear Menu Settings
                    MainMenu.LaneKeys(useW: false, useE: false, useR: false);
                    MainMenu.Lane.AddSeparator();
                    MainMenu.Lane.AddGroupLabel("Laneclear Preferences", "lane.grouplabel.addonmenu", true);
                    MainMenu.Lane.AddCheckBox("lane.refresh.buff", "Refresh the minion target if E Killable", true, true);
                    MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.addonmenu.2", true);
                    MainMenu.LaneManaManager(true, false, false, false, 80, 80, 80, 50);

                    //Jungle Clear Menu Settings
                    MainMenu.JungleKeys(useW: false, useR: false, junglesteal: true);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Jungleclear Preferences", "jungle.grouplabel.addonmenu", true);
                    MainMenu.Jungle.AddCheckBox("jungle.monsters.spell", "Use Abilities on Big Monster", true, true);
                    MainMenu.Jungle.AddCheckBox("jungle.minimonsters.spell", "Use Abilities on Mini Monsters", false,
                        true);
                    MainMenu.Jungle.AddCheckBox("jungle.monsters.ally", "Dont steal buffs", true, true);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu.2", true);
                    MainMenu.JungleManaManager(true, false, true, false, 40, 80, 50, 40);

                    //Last hit Menu Settings
                    MainMenu.LastHitKeys(useW: false, useR: false);
                    MainMenu.Lasthit.AddSeparator();
                    MainMenu.Lasthit.AddGroupLabel("LastHit Preferences", "lasthit.grouplabel.addonmenu", true);
                    MainMenu.Lasthit.AddSlider("lasthit.q.count", "Execute {0} minions with Q", 2, 1, 10, true);
                    MainMenu.Lasthit.AddSlider("lasthit.e.count", "Execute {0} minions with E", 4, 1, 10, true);
                    MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu.2", true);
                    MainMenu.LasthitManaManager(true, false, true, false, 70, 90, 60, 50);

                    //Harras
                    MainMenu.HarassKeys(useW: false, useR: false);
                    MainMenu.Harass.AddSeparator();
                    MainMenu.Harass.AddGroupLabel("Harass Preferences", "harass.grouplabel.addonmenu", true);
                    MainMenu.Harass.AddCheckBox("harass.q.trough", "Use Q for transfer the E Stacks", true, true);
                    MainMenu.Harass.AddCheckBox("harass.e.minions", "Use E on minions for Harass", true, true);
                    MainMenu.Harass.AddCheckBox("harass.aa.mark", "Force AA to target with W Passive", true, true);
                    MainMenu.Harass.AddSeparator();
                    MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu.2", true);
                    MainMenu.HarassManaManager(true, true, true, true, 60, 80, 50, 40);

                    //Flee Menu
                    MainMenu.FleeKeys(useW: false, useR: false);
                    MainMenu.Flee.AddSeparator();
                    MainMenu.Flee.AddGroupLabel("Flee Preferences", "flee.grouplabel.addonmenu", true);
                    MainMenu.Flee.AddCheckBox("flee.q.wall", "Use Q for jump the wall", true, true);
                    MainMenu.Flee.AddCheckBox("flee.aa.gapcloser", "Use AA for reach the target / run away", true, true);

                    //Ks
                    MainMenu.KsKeys(useW: false);
                    MainMenu.Ks.AddSeparator();
                    MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                    MainMenu.KsManaManager(true, false, true, false, 20, 30, 10, 5);

                    //Misc Menu
                    MainMenu.MiscMenu();
                    MainMenu.Misc.AddCheckBox("misc.q.gapcloser", "Use Auto Q on GapCloser", false);
                    MainMenu.Misc.AddCheckBox("misc.r.gapcloser", "Use Auto R on GapCloser if not braindead", false);
                    MainMenu.Misc.AddCheckBox("misc.q", "Use Auto Q on CC");
                    MainMenu.Misc.AddCheckBox("misc.w.auto", "Use Auto W", false);
                    MainMenu.Misc.AddCheckBox("misc.r.save", "Use Auto R for save Ally");
                    MainMenu.Misc.AddCheckBox("misc.aa.exploit", "Use DoubleJump Exploit");
                    MainMenu.Misc.AddCheckBox("misc.passive.pact", "Use AutoPact", false);
                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddGroupLabel("Auto Q Settings", "misc.grouplabel.addonmenu", true);
                    MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.fear", "Use Q on Feared Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddGroupLabel("W Settings", "misc.grouplabel.addonmenu.2", true);
                    MainMenu.Misc.AddCheckBox("misc.w.alarm", "Alert if Sentinel is receiving damage", true, true);
                    MainMenu.Misc.AddCheckBox("misc.w.alarm.sound", "Active Sentinel notification Sound", true, true);
                    MainMenu.Misc.AddCheckBox("misc.w.interrupt", "Interrupt W if Combo Key pressed", true, true);

                    MainMenu.Misc.Add("misc.w.dragon",
                        new KeyBind("Send sentinel to Drake", false, KeyBind.BindTypes.HoldActive, 'P'))
                        .OnValueChange += OnDrake;
                    Value.AdvancedMenuItemUiDs.Add("misc.w.dragon");
                    MainMenu.Misc["misc.w.dragon"].IsVisible =
                        MainMenu.Misc["misc.advanced"].Cast<CheckBox>().CurrentValue;

                    MainMenu.Misc.Add("misc.w.baron",
                        new KeyBind("Send sentinel to Baron", false, KeyBind.BindTypes.HoldActive, 'O'))
                        .OnValueChange += OnBaron;
                    Value.AdvancedMenuItemUiDs.Add("misc.w.baron");
                    MainMenu.Misc["misc.w.baron"].IsVisible =
                        MainMenu.Misc["misc.advanced"].Cast<CheckBox>().CurrentValue;

                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddCheckBox("misc.r.save.prevent", "Prevent to cast R if ally is channeling", true,
                        true);
                    MainMenu.Misc.AddSlider("misc.r.save.ally", "Use R if ally is under {0}  HP", 15, 0, 100, true);
                    MainMenu.Misc.AddSeparator();
                    if (EntityManager.Heroes.Allies.Any(a => a.ChampionName == "Blitzcrank"))
                    {
                        MainMenu.Misc.AddGroupLabel("Balista Settings", "misc.grouplabel.addonmenu.3", true);
                        MainMenu.Misc.AddCheckBox("misc.r.balista", "Use Balista", true, true);
                        MainMenu.Misc.AddSlider("misc.r.balista.range", "Use Balista if the range is more then {0}", 600,
                            100,
                            1200, true);
                        MainMenu.Misc.AddSeparator();
                    }
                    MainMenu.Misc.AddGroupLabel("Mana Manager:", "misc.grouplabel.addonmenu.3", true);
                    MainMenu.Misc.AddSlider("misc.q.mana", "Use Q on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                        true);
                    MainMenu.Misc.AddSlider("misc.w.mana", "Use W if Mana is above than {0}%", 70, 0, 100,
                        true);
                    MainMenu.Misc.AddSlider("misc.r.mana", "Use R if Mana is above than {0}%", 30, 0, 100,
                        true);

                    //Draw Menu
                    MainMenu.DrawKeys();
                    MainMenu.Draw.AddSeparator();
                    MainMenu.Draw.AddCheckBox("draw.hp.bar", "Draw Combo Damage", true, true);
                    MainMenu.Draw.AddCheckBox("draw.w.alarm.text", "Active Sentinel notification Text", true, true);
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
                Obj_AI_Base.OnProcessSpellCast += AaReset;
                Orbwalker.OnPreAttack += OnPreAttack;
                Orbwalker.OnPostAttack += OnPostAttack;
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
            var target = Variables.CloseEnemies(_q.Range).FirstOrDefault();
            var forcedtarget = Variables.CloseEnemies(_q.Range).Find
                (a => a.HasBuff("kalistaexpungemarker"));

            if (Value.Use("combo.e.save.mana") && Player.Instance.Mana <= EMana() ||
                Value.Use("combo.r.save.mana") && Player.Instance.Mana <= RMana()) return;

            if (Value.Use("combo.aa") && forcedtarget != null)
            {
                if (Value.Use("combo.q".AddName()) && _q.IsReady() && !Value.Use("combo.q.aa"))
                {
                    var pred = _q.GetPrediction(forcedtarget);
                    if (pred.HitChancePercent >= Value.Get("combo.q.prediction") &&
                        Player.Instance.ManaPercent >= Value.Get("combo.q.mana"))
                    {
                        if (Value.Use("combo.q.minions"))
                        {
                            var minion =
                                EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                                    m =>
                                        m.IsValidTarget(_q.Range) && m.TotalShieldHealth() <= QDamage(m) &&
                                        forcedtarget.Distance(m) <= 110);

                            _q.Cast(minion);
                        }
                        if (Value.Use("combo.q.collision") && pred.CollisionObjects.Any())
                        {
                            _q.Cast(pred.CastPosition);
                        }
                        else
                        {
                            _q.Cast(pred.CastPosition);
                        }
                    }
                }
                if (!Value.Use("killsteal.e"))
                {
                    if (Value.Use("combo.e".AddName()) && _e.IsReady())
                    {
                        if (Value.Use("combo.e.auto") &&
                            forcedtarget.Distance(Player.Instance) >= Value.Get("combo.e.range"))
                        {
                            _e.Cast();
                        }
                        else if (Value.Use("combo.e.death") &&
                                 Player.Instance.HealthPercent <= Value.Get("combo.e.damage.me"))
                        {
                            _e.Cast();
                        }
                    }
                }
            }
            else if (target != null)
            {
                if (Value.Use("combo.q".AddName()) && _q.IsReady() && !Value.Use("combo.q.aa"))
                {
                    var pred = _q.GetPrediction(target);
                    if (pred.HitChancePercent >= Value.Get("combo.q.prediction") &&
                        Player.Instance.ManaPercent >= Value.Get("combo.q.mana"))
                    {
                        if (Value.Use("combo.q.minions"))
                        {
                            var minion =
                                EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                                    m =>
                                        m.IsValidTarget(_q.Range) && m.TotalShieldHealth() <= QDamage(m) &&
                                        forcedtarget.Distance(m) <= 110);

                            _q.Cast(minion);
                        }
                        if (Value.Use("combo.q.collision") && pred.CollisionObjects.Any())
                        {
                            _q.Cast(pred.CastPosition);
                        }
                        else
                        {
                            _q.Cast(pred.CastPosition);
                        }
                    }
                }
            }

            if (Value.Use("combo.r.allin") && _r.IsReady() && (target != null || forcedtarget != null))
            {
                if (Player.Instance.ChampionsKilled > target.ChampionsKilled ||
                    Player.Instance.Level > target.Level ||
                    Player.Instance.GoldTotal > target.GoldTotal) return;

                var ally = Variables.CloseAllies(1000).FirstOrDefault(a => Variables.IsSupport(a)
                                                                           && a.HasBuff("kalistacoopstrikeally"));
                if (ally != null)
                {
                    if (Value.Use("combo.r.allin.prevent") && ally.Spellbook.GetSpell(SpellSlot.R).IsReady)
                    {
                        _r.Cast();
                    }
                    else
                    {
                        _r.Cast();
                    }
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            if (Player.Instance.ManaPercent <= Value.Get("lane.q.mana")) return;
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(_q.Range));
            if (Value.Use("lane.q") && _q.IsReady())
            {
                _q.Cast(minion);
            }
        }

        #endregion

        #region Jungleclear

        public override void Jungleclear()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Lasthit

        public override void LastHit()
        {
            var unit = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                Player.Instance.ServerPosition, _e.Range);
            var objAiMinions = unit as Obj_AI_Minion[] ?? unit.ToArray();
            var count = objAiMinions.Count(c => c.HasBuff("kalistaexpungemarker") && c.Health <= EDamage(c));
            var spaghetti = objAiMinions.FirstOrDefault(lol => lol.Health <= QDamage(lol));
            if (unit == null) return;
            if (Value.Use("lasthit.e") && count >= Value.Get("lasthit.e.count") && _e.IsReady() &&
                Player.Instance.ManaPercent >= Value.Get("lasthit.e.mana"))
            {
                _e.Cast();
            }
            if (Value.Use("lasthit.q") && _q.IsReady() && Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
            {
                _q.Cast(spaghetti);
            }
        }

        #endregion

        #region Exploit

        private static void Exploit()
        {
            if (Value.Use("misc.aa.exploit") && Player.Instance.AttackDelay/1 > 1.70)
            {
                if (Value.Mode(Orbwalker.ActiveModes.Combo) || Value.Mode(Orbwalker.ActiveModes.Harass) ||
                    Value.Mode(Orbwalker.ActiveModes.LaneClear) || Value.Mode(Orbwalker.ActiveModes.JungleClear) ||
                    Value.Mode(Orbwalker.ActiveModes.Flee) || Value.Mode(Orbwalker.ActiveModes.LastHit))
                {
                    var target = TargetSelector.GetTarget(_q.Range + Player.Instance.GetAutoAttackRange() + 50,
                        DamageType.Physical);

                    if (Game.Time*1000 >= Orbwalker.LastAutoAttack + 1)
                    {
                        Orbwalker.OrbwalkTo(OKTRGeometry.SafeDashPosRework(_q.Range, target, 190));
                    }

                    if (Game.Time*1000 > Orbwalker.LastAutoAttack + Player.Instance.AttackDelay*1000 - 150)
                    {
                        Orbwalker.OrbwalkTo(OKTRGeometry.SafeDashPosRework(_q.Range, target, 190));
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
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed)
                return;

            //Tahmlista();
            Pact();
            AutoSentinel();
            AutoSave();
            Exploit();
            Ks();
            JungleSteal();
            IWillGetYou();
        }

        private void IWillGetYou()
        {
            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                var target = Variables.CloseEnemies().FirstOrDefault();
                if (Value.Use("flee.aa.gapcloser") && target != null && !target.IsInAutoAttackRange(Player.Instance))
                {
                    var faggots =
                        EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(f => f.IsValidTarget(_q.Range));

                    Orbwalker.ForcedTarget = faggots;
                    Orbwalker.MoveTo(target.ServerPosition);
                }
            }
        }

        private static void JungleSteal()
        {
            var unit =
                EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(
                    u =>
                        u.IsValidTarget(_e.Range) && u.HasBuff("kalistaexpungemarker") &&
                        u.TotalShieldHealth() <= EDamage(u));
            var ally = Variables.CloseAllies(_e.Range).FirstOrDefault();
            if (unit == null) return;
            if (Value.Use("jungle.monsters.ally") && ally.IsFacing(unit)) return;
            if (_e.IsReady() && Value.Use("jungle.e"))
            {
                if (Value.Use("jungle.SRU_Baron") && unit.BaseSkinName == "SRU_Baron" ||
                    Value.Use("jungle.SRU_Dragon") && unit.BaseSkinName == "SRU_Dragon" ||
                    Value.Use("jungle.SRU_Blue") && unit.BaseSkinName == "SRU_Blue" ||
                    Value.Use("jungle.SRU_Red") && unit.BaseSkinName == "SRU_Red" ||
                    Value.Use("jungle.SRU_Gromp") && unit.BaseSkinName == "SRU_Gromp" ||
                    Value.Use("jungle.SRU_Murkwolf") && unit.BaseSkinName == "SRU_Murkwolf" ||
                    Value.Use("jungle.SRU_Krug") && unit.BaseSkinName == "SRU_Krug" ||
                    Value.Use("jungle.SRU_Razorbeak") && unit.BaseSkinName == "SRU_Razorbeak" ||
                    Value.Use("jungle.SRU_Crab") && unit.BaseSkinName == "SRU_Crab")

                {
                    _e.Cast();
                }
            }
            else if (_q.IsReady() && Value.Use("jungle.q"))
            {
                if (Value.Use("jungle.SRU_Baron") && unit.BaseSkinName == "SRU_Baron" ||
                    Value.Use("jungle.SRU_Dragon") && unit.BaseSkinName == "SRU_Dragon" ||
                    Value.Use("jungle.SRU_Blue") && unit.BaseSkinName == "SRU_Blue" ||
                    Value.Use("jungle.SRU_Red") && unit.BaseSkinName == "SRU_Red" ||
                    Value.Use("jungle.SRU_Gromp") && unit.BaseSkinName == "SRU_Gromp" ||
                    Value.Use("jungle.SRU_Murkwolf") && unit.BaseSkinName == "SRU_Murkwolf" ||
                    Value.Use("jungle.SRU_Krug") && unit.BaseSkinName == "SRU_Krug" ||
                    Value.Use("jungle.SRU_Razorbeak") && unit.BaseSkinName == "SRU_Razorbeak" ||
                    Value.Use("jungle.SRU_Crab") && unit.BaseSkinName == "SRU_Crab")

                {
                    _q.Cast(unit);
                }
            }
        }

        #region Tahmlista

        private static void Tahmlista()
        {
            if (EntityManager.Heroes.Allies.Any(a => a.ChampionName == "TahmKench"))
            {
                MainMenu.Misc.AddGroupLabel("TahmLista Settings", "misc.grouplabel.addonmenu.4", true);
                MainMenu.Misc.AddCheckBox("misc.r.tahmlista", "Use Tahmlista", true, true);
                MainMenu.Misc.AddSlider("misc.r.balista.range", "Use Tahmlista if the range is more then {0}", 600, 100,
                    1200, true);
                MainMenu.Misc.AddSeparator();
            }
        }

        #endregion

        #region Sentinel

        #region Vector Check

        private static bool HasSentinel(Vector3 castposition)
        {
            return
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.Name == "KalistaSentinel").Any(a => castposition.Distance(a.Position) <= 300);
        }

        private static void Wcast(Vector3 location)
        {
            if (!HasSentinel(location))
            {
                _w.Cast(location);
            }
        }

        #endregion

        #region AutoSentinel

        private static void AutoSentinel()
        {
            if (Player.Instance.IsRecalling() && Game.MapId != GameMapId.SummonersRift ||
                Value.Use("misc.w.interrupt") || !Value.Use("misc.w.auto") || !_w.IsReady() ||
                !(Player.Instance.ManaPercent >= Value.Get("misc.w.mana")))
                return;
            var kalista = Player.Instance.Position;
            var baron = new Vector2(5007.124f, 10471.45f);
            var dragon = new Vector2(9866.148f, 4414.014f);

            if (Player.Instance.CountEnemiesInRange(_q.Range) == 0)
            {
                //Baron
                if (kalista.Distance(baron) < _w.Range)
                {
                    Wcast(baron.To3D());
                }
                //Dragon
                else if (kalista.Distance(dragon) < _w.Range)
                {
                    Wcast(dragon.To3D());
                }
            }
        }

        #endregion

        #region Drakebutton

        private static void OnDrake(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            var dragon = new Vector2(9866.148f, 4414.014f);

            if (args.NewValue && Player.Instance.Position.Distance(dragon) < _w.Range)
            {
                Wcast(dragon.To3D());
            }
        }

        #endregion

        #region Baronbutton

        private static void OnBaron(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            var baron = new Vector2(5007.124f, 10471.45f);

            if (args.NewValue && Player.Instance.Position.Distance(baron) < _w.Range)
            {
                Wcast(baron.To3D());
            }
        }

        #endregion

        #region KillSteal

        private static void Ks()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(
                a =>
                    a.IsValidTarget(_e.Range) && !a.IsDead && !a.IsZombie && a.HasBuff("kalistaexpungemarker") &&
                    !a.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (Value.Use("killsteal.e") && _e.IsReady() &&
                    Player.Instance.ManaPercent >= Value.Get("killsteal.e.mana"))
                {
                    if (enemy.TotalShieldHealth() <= EDamage(enemy))
                    {
                        _e.Cast();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Pact

        private static void Pact()
        {
            var ally =
                Variables.CloseAllies(UtilityManager.Activator.KalistaSpear.Range)
                    .FirstOrDefault(a => Variables.IsSupport(a)
                                         &&
                                         !a.HasBuff(
                                             "kalistacoopstrikeally"));

            if (ally == null) return;

            if (Value.Use("misc.passive.pact")
                && UtilityManager.Activator.KalistaSpear.IsOwned()
                && Shop.CanShop)
            {
                UtilityManager.Activator.KalistaSpear.Cast(ally);
            }
        }

        #endregion

        #region AutoSave

        private static void AutoSave()
        {
            if (Value.Use("misc.r.save") && Player.Instance.ManaPercent >= Value.Get("misc.r.mana"))
            {
                var ally = Variables.CloseAllies(1000).FirstOrDefault(a => Variables.IsSupport(a)
                                                                           && a.HasBuff("kalistacoopstrikeally"));

                if (ally == null) return;

                var target = TargetSelector.GetTarget(_r.Range, DamageType.Mixed, ally.ServerPosition);

                if (Value.Get("misc.r.save.ally") >= ally.HealthPercent && ally.IsFacing(target))
                {
                    if (Value.Use("misc.r.save.prevent") && !ally.Spellbook.IsChanneling)
                    {
                        _r.Cast();
                    }
                    else
                    {
                        _r.Cast();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region OnAntiGap

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            var ally = Variables.CloseAllies(1000).FirstOrDefault(a => Variables.IsSupport(a)
                                                                       && a.HasBuff("kalistacoopstrikeally"));

            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Instance.Type || !e.Sender.IsEnemy)
                return;

            if (_r.IsReady() && Value.Use("misc.r.gapcloser") &&
                Player.Instance.ManaPercent <= Value.Get("misc.r.mana") &&
                (ally != null))
            {
                if (Value.Use("misc.r.save.prevent") && !ally.Spellbook.IsChanneling)
                {
                    _r.Cast();
                }
                else
                {
                    _r.Cast();
                }
            }

            if (_q.IsReady() && _q.IsInRange(sender) && Value.Use("misc.q.gapcloser"))
            {
                var pred = e.End;

                _q.Cast(_q.GetPrediction(sender).CastPosition);
                Orbwalker.OrbwalkTo(pred + 5*(Player.Instance.Position - e.End));
            }
        }

        #endregion

        #region Buffgain

        private static void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (EntityManager.Heroes.Allies.Any(a => a.ChampionName == "Blitzcrank") && Value.Use("misc.r.balista") &&
                _r.IsReady())
            {
                var blizzzyboz =
                    EntityManager.Heroes.Enemies.Find(
                        a => a.ChampionName.Equals("Blitzcrank") && a.HasBuff("kalistacoopstrikeally"));

                if (blizzzyboz != null)
                {
                    if ((Player.Instance.Distance(blizzzyboz) >= Value.Get("misc.r.balista.range")) &&
                        _r.IsInRange(blizzzyboz)
                        && sender.HasBuff("rocketgrab2") && sender.IsEnemy)
                    {
                        _r.Cast();
                    }
                }
            }

            if (Value.Use("misc.q") && Player.Instance.ManaPercent >= Value.Get("misc.q.mana") && _q.IsInRange(sender))
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

        #region Orbwalker

        #region AAReset

        private static void AaReset(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsInAutoAttackRange(sender) && sender.HasBuff("KalistaExpungeWrapper"))
            {
                Orbwalker.ResetAutoAttack();
            }
        }

        #endregion

        #region OnPreAttack

        private void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                if (Value.Use("combo.aa"))
                {
                    var forcedtarget = Variables.CloseEnemies(_q.Range).Find
                        (a => a.HasBuff("kalistaexpungemarker"));

                    Orbwalker.ForcedTarget = forcedtarget;
                }
                else
                {
                    Orbwalker.ForcedTarget = target;
                }
            }
            if (Value.Mode(Orbwalker.ActiveModes.LaneClear))
            {
                var unit =
                    EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                        u =>
                            u.IsValidTarget(Player.Instance.GetAutoAttackRange(u)));

                if (unit == null) return;

                if (Value.Use("lane.refresh.buff") && EDamage(unit) <= unit.Health &&
                    unit.HasBuff("kalistaexpungemarker"))
                {
                    Orbwalker.ForcedTarget = unit;
                }
                else
                {
                    Orbwalker.ForcedTarget = unit;
                }
            }
        }

        #endregion

        #region OnPostAttack

        private static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Value.Use("combo.q".AddName()) && Value.Use("combo.q.aa") && _q.IsReady())
            {
                var current = Variables.CloseEnemies(_q.Range).FirstOrDefault();
                var pred = _q.GetPrediction(current);

                if (pred.HitChancePercent >= Value.Get("combo.q.prediction"))
                {
                    if (_q.IsInRange(current) && Value.Use("combo.q.collision") && !pred.CollisionObjects.Any())
                    {
                        _q.Cast(pred.CastPosition);
                    }
                    else if (!Value.Use("combo.q.collision"))
                    {
                        _q.Cast(pred.CastPosition);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Math OP

        #region Damage

        private static float QDamage(Obj_AI_Base target)
        {
            if (!_q.IsReady())
            {
                return 0.0f;
            }

            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                new float[] {70, 130, 190, 250}[_q.Level - 1] + .1f*Player.Instance.TotalAttackDamage);
        }

        private static readonly int[] EStackDamage = {10, 14, 19, 25, 32};
        private static readonly int[] EBaseDamage = {20, 30, 40, 50, 60};
        private static readonly float[] EBaseDamageMultiplier = {.6f, .6f, .6f, .6f, .6f};
        private static readonly float[] EStackDamageMultiplier = {.2f, .225f, .25f, .275f, .3f};

        private static float EDamage(Obj_AI_Base target)
        {
            var stacks = target.GetBuffCount("kalistaexpungemarker");

            if (!_e.IsReady() || stacks == 0)
            {
                return 0.0f;
            }

            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                EBaseDamage[_e.Level - 1] + stacks*EStackDamage[_e.Level - 1] +
                Player.Instance.TotalAttackDamage*(EBaseDamageMultiplier[_e.Level - 1] +
                                                   stacks*EStackDamageMultiplier[_e.Level - 1]) -
                Value.Get("combo.e.damage"));
        }

        #endregion

        #region Mana

        private static float QMana()
        {
            return
                new float[] {50, 55, 60, 65, 70}[_q.Level - 1];
        }

        private static float WMana()
        {
            return
                new float[] {90, 80, 70, 60, 50}[_w.Level - 1];
        }

        private static float EMana()
        {
            return 30;
        }

        private static float RMana()
        {
            return 100;
        }

        #endregion

        #endregion

        #endregion
    }
}