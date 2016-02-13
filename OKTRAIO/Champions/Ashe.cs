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
    internal class Ashe : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Active _q;
        private static Spell.Skillshot _w, _e, _r;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            try
            {
                //spells
                _q = new Spell.Active(SpellSlot.Q, 600);
                _w = new Spell.Skillshot(SpellSlot.W, 1280, SkillShotType.Linear, 250, 2000, 20);
                _e = new Spell.Skillshot(SpellSlot.E, 25000, SkillShotType.Linear, 250, 1400, 300);
                _r = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 250, 1600, 130);
                _w.AllowedCollisionCount = 0;
                _e.AllowedCollisionCount = int.MaxValue;
                _r.AllowedCollisionCount = int.MaxValue;


                //menu
                MainMenu.ComboKeys(useE: false);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                MainMenu.Combo.Add("combo.rbind",
                    new KeyBind("Semi-Auto R (No Lock)", false, KeyBind.BindTypes.HoldActive, 'T'))
                    .OnValueChange += OnUltButton;
                MainMenu.Combo.AddCheckBox("combo.r.aoe", "Use R for AOE", true, true);
                MainMenu.Combo.AddSlider("combo.r.slider", "{0} Enemies hit with R explosion", 3, 0, 5, true);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Prediction", "combo.grouplabel1.addonmenu", true);
                MainMenu.Combo.AddSlider("combo.wr.prediction", "Hitchance Percentage for W/R", 80, 0, 100, true);

                //flee
                MainMenu.FleeKeys(false, useE: false, useR: false);
                MainMenu.Flee.AddSeparator();
                MainMenu.Flee.AddGroupLabel("Mana Manager:", "flee.grouplabel.addonmenu", true);
                MainMenu.FleeManaManager(false, true, false, false, 0, 20, 0, 0);

                //lasthit
                MainMenu.LastHitKeys(false, useE: false, useR: false);
                MainMenu.Lasthit.AddSeparator();
                MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu", true);
                MainMenu.LasthitManaManager(false, true, false, false, 0, 80, 0, 0);


                //laneclear
                MainMenu.LaneKeys(useE: false, useR: false);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddSlider("lane.w.min", "Min. {0} minions for W", 3, 1, 7, true);
                MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.addonmenu", true);
                MainMenu.LaneManaManager(true, true, false, false, 60, 70, 0, 0);

                //jungleclear
                MainMenu.JungleKeys(useE: false, useR: false);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu", true);
                MainMenu.JungleManaManager(true, true, false, false, 60, 30, 0, 0);

                //harass
                MainMenu.HarassKeys(useE: false, useR: false);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu", true);
                MainMenu.HarassManaManager(true, true, false, false, 20, 20, 0, 0);

                //ks
                MainMenu.KsKeys(false, useE: false);
                MainMenu.Ks.AddSeparator();
                MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                MainMenu.KsManaManager(false, true, false, true, 20, 30, 10, 5);

                //misc
                MainMenu.MiscMenu();
                MainMenu.Misc.AddCheckBox("misc.w", "Use Auto W");
                MainMenu.Misc.AddCheckBox("misc.e", "Use Auto E");
                MainMenu.Misc.AddCheckBox("misc.r.interrupter", "Use R for Interrupt");
                MainMenu.Misc.AddCheckBox("misc.w.gapcloser", "Use W for Anti-Gapcloser");
                MainMenu.Misc.AddCheckBox("misc.r.auto", "Anti Rengar + Khazix");
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Auto W Settings", "misc.grouplabel1.addonmenu", true);
                MainMenu.Misc.AddCheckBox("misc.w.charm", "Use W on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.stun", "Use W on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.knockup", "Use W on Knocked Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.snare", "Use W on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.suppression", "Use W on Suppressed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.w.taunt", "Use W on Taunted Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Mana Manger", "misc.grouplabel.mana", true);
                MainMenu.Misc.AddSlider("misc.w.mana", "Use W on CC Enemy if Mana is above than {0}%", 30, 0, 100,
                        true);

                //draw
                MainMenu.DrawKeys(false, useE: false, useR: false);
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
                Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
                Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
                GameObject.OnCreate += GameObject_OnCreate;
                Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
                Drawing.OnDraw += GameOnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                Obj_AI_Base.OnBuffGain += BuffGain; 
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

        #region UltButton

        private static void OnUltButton(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && TargetSelector.GetTarget(_r.Range, DamageType.Physical) != null)
                _r.Cast(_r.GetPrediction(TargetSelector.GetTarget(_r.Range, DamageType.Physical)).CastPosition);
        }

        #endregion

        #region Combo

        public override void Combo()
        {
            var target = TargetSelector.GetTarget(_w.Range, DamageType.Physical);
            var targetr = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

            if (Value.Use("combo.w".AddName()) && _w.IsReady())
            {
                if (target != null)
                {
                    var wpred = _w.GetPrediction(target);
                    if (wpred.HitChancePercent >= Predslider)
                    {
                        _w.Cast(wpred.CastPosition);
                    }
                }
            }

            if (_r.IsReady() && (Value.Use("combo.r.aoe") || Value.Use("combo.r".AddName())))
            {
                if (targetr != null)
                {
                    var rpred = _r.GetPrediction(targetr);

                    //Raoe
                    if (targetr.CountEnemiesInRange(200) >= Value.Get("combo.r.slider"))
                    {
                        if (rpred.HitChancePercent >= Predslider)
                        {
                            _r.Cast(rpred.CastPosition);
                        }
                    }

                    //Rcombo
                    if (targetr.TotalShieldHealth() <= ComboDamage(targetr) && !Overkill(targetr) &&
                        !Player.Instance.IsInAutoAttackRange(targetr))
                    {
                        if (rpred.HitChancePercent >= Predslider)
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
            var target = TargetSelector.GetTarget(_w.Range, DamageType.Physical);

            if (target == null)
            {
                return;
            }
            if (Value.Use("harass.w") && _w.IsReady() && Player.Instance.ManaPercent >= Value.Get("harass.w.mana"))
            {
                if (target.IsValid)
                {
                    var wpred = _w.GetPrediction(target);
                    if (wpred.HitChancePercent >= Predslider)
                    {
                        _w.Cast(wpred.CastPosition);
                    }
                }
            }

            if (Value.Use("harass.q") && _q.IsReady() &&
                Player.Instance.ManaPercent >= Value.Get("harass.q.mana") && Player.Instance.IsInAutoAttackRange(target))
            {
                foreach (var a in Player.Instance.Buffs)
                    if (a.Name == "asheqcastready" && a.Count == 4)
                    {
                        Player.IssueOrder(GameObjectOrder.AutoAttack, target);
                        _q.Cast();
                    }
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            var lanemonsters =
                EntityManager.MinionsAndMonsters.GetLaneMinions().FirstOrDefault(a => a.IsValidTarget(_w.Range));

            if (lanemonsters == null || Player.Instance.ManaPercent < Value.Get("lane.w.mana"))
            {
                return;
            }

            if (Value.Use("lane.w") && _w.IsReady() && Player.Instance.CountEnemiesInRange(1300) == 0)
            {
                var minionsw =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .Where(a => a.IsInRange(Player.Instance.ServerPosition, _w.Range));

                var wfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionsw, _w.Width*3, (int) _w.Range);

                if (wfarm.HitNumber >= Value.Get("lane.w.min"))
                {
                    _w.Cast(wfarm.CastPosition);
                }
            }
        }

        #endregion

        #region Lasthit

        public override void LastHit()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(
                        a =>
                            a.IsValidTarget(_w.Range) &&
                            a.Distance(Player.Instance.Position) > Player.Instance.GetAutoAttackRange() + 50 &&
                            a.Health <= Player.Instance.GetSpellDamage(a, SpellSlot.W));

            if (Value.Use("lasthit.w") && _w.IsReady() && Player.Instance.ManaPercent >= Value.Get("lasthit.w.mana"))
            {
                if (minion != null)
                {
                    _w.Cast(minion);
                }
            }
        }

        #endregion

        #region Jungleclear

        public override void Jungleclear()
        {
            var monsters =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, _w.Range)
                    .FirstOrDefault(a => a.IsValidTarget(_w.Range));

            if (monsters == null || Player.Instance.ManaPercent <= Value.Get("jungle.w.mana"))
            {
                return;
            }

            if (Value.Use("jungle.w") && _w.IsReady())
            {
                if (monsters.Health > ObjectManager.Player.GetSpellDamage(monsters, SpellSlot.W) &&
                    Variables.SummonerRiftJungleList.Contains(monsters.BaseSkinName))
                {
                    _w.Cast(monsters);
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var target = TargetSelector.GetTarget(_w.Range, DamageType.Physical);

            if (Value.Use("flee.w") && _w.IsReady())
            {
                if (target != null && _w.IsInRange(target))
                {
                    var wpred = _w.GetPrediction(target);
                    if (wpred.HitChancePercent >= Predslider)
                    {
                        _w.Cast(wpred.CastPosition);
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
            if (Player.Instance.IsDead || Player.Instance.HasBuff("Recall")
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed ||
                Orbwalker.IsAutoAttacking)
                return;

            Ks();

            if (Value.Use("misc.e"))
            {
                AutoE();
            }
        }

        #endregion

        #region Interrupter

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            try
            {
                if (sender.IsAlly || !Value.Use("misc.r.interrupter") ||
                    e.DangerLevel != DangerLevel.High)
                {
                    return;
                }

                if (_r.IsReady() && sender.IsInRange(Player.Instance.Position, 2000))
                {
                    _r.Cast(sender);
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code INTERRUPTER)</font>");
            }
        }

        #endregion

        #region AntiGapCloser

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            try
            {
                if (sender == null || sender.IsAlly || !Value.Use("misc.w.gapcloser"))
                {
                    return;
                }

                if (_w.IsReady() && _w.IsInRange(sender))
                {
                    _w.Cast(sender);
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code Gapcloser)</font>");
            }
        }

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

        #endregion

        #region Orbwalker

        private void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Value.Use("combo.q".AddName()) && _q.IsReady() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (var a in Player.Instance.Buffs)
                    if (a.Name == "asheqcastready" && a.Count == 4)
                    {
                        _q.Cast();
                    }
            }


            if (Value.Use("jungle.q") && _q.IsReady() &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
            {
                if (Variables.SummonerRiftJungleList.Contains(target.Name))
                {
                    foreach (var a in Player.Instance.Buffs)
                        if (a.Name == "asheqcastready" && a.Count == 4)
                        {
                            _q.Cast();
                        }
                }
            }

            if (Value.Use("lane.q") && _q.IsReady() &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
            {
                var lmonsters = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, Player.Instance.GetAutoAttackRange() + 50);
                if (lmonsters.Count() > 1)
                {
                    foreach (var a in Player.Instance.Buffs)
                        if (a.Name == "asheqcastready" && a.Count == 4)
                        {
                            _q.Cast();
                        }
                }
            }
        }

        #endregion
        
        #region OnSpellCast

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || !Value.Use("misc.e"))
            {
                return;
            }

            if (args.SData.Name.ToLower() == "summonerflash" && sender.Distance(Player.Instance.Position) < 2000 &&
                _e.IsReady())
            {
                _e.Cast(args.End);
            }
        }

        #endregion

        #region Prediciton

        public static int Predslider
        {
            get { return Value.Get("combo.wr.prediction"); }
        }

        #endregion

        #region OnBuffGain

        private static void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe) return;

            if (Player.Instance.IsInRange(sender, _w.Range) && Value.Use("misc.w") && Player.Instance.ManaPercent >= Value.Get("misc.w.mana"))
            {
                if (Value.Use("misc.w.charm") && sender.IsCharmed)
                {
                    _w.Cast(sender);
                }
                if (Value.Use("misc.w.knockup"))
                {
                    _w.Cast(sender);
                }
                if (Value.Use("misc.w.stun") && sender.IsStunned)
                {
                    _w.Cast(sender);
                }
                if (Value.Use("misc.w.snare") && sender.IsRooted)
                {
                    _w.Cast(sender);
                }
                if (Value.Use("misc.w.suppression") && sender.IsSuppressCallForHelp)
                {
                    _w.Cast(sender);
                }
                if (Value.Use("misc.w.taunt") && sender.IsTaunted)
                {
                    _w.Cast(sender);
                }
            }
        }

        #endregion

        #region AutoE

        private static void AutoE()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => a.IsValidTarget(3000)))
            {
                if (enemy != null)
                {
                    var tpred = _e.GetPrediction(enemy);
                    var tpredcast = tpred.CastPosition.To2D();
                    var flags = NavMesh.GetCollisionFlags(tpredcast);

                    if (flags.HasFlag(CollisionFlags.Grass) && !enemy.VisibleOnScreen)
                    {
                        _e.Cast(tpredcast.To3D());
                    }
                }
            }
        }

        #endregion

        #region KillSteal

        private static void Ks()
        {
            try
            {
                foreach (
                    var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            a => a.IsValidTarget(2000) && !a.IsDead && !a.IsZombie && a.HealthPercent <= 30))
                {
                    if (Value.Use("killsteal.w") && _w.IsReady() &&
                        Player.Instance.ManaPercent >= Value.Get("killsteal.w.mana"))
                    {
                        if (_w.IsInRange(enemy) &&
                            enemy.TotalShieldHealth() < Player.Instance.GetSpellDamage(enemy, SpellSlot.W))
                        {
                            var wpred = _w.GetPrediction(enemy);
                            if (wpred.HitChancePercent >= Predslider)
                            {
                                _w.Cast(wpred.CastPosition);
                            }
                        }
                    }

                    if (Value.Use("killsteal.r") && _r.IsReady() &&
                        Player.Instance.ManaPercent >= Value.Get("killsteal.r.mana"))
                    {
                        if (_r.IsInRange(enemy) &&
                            enemy.TotalShieldHealth() < RDamage(enemy) &&
                            enemy.Distance(Player.Instance.Position) > 1000 && enemy.CountAlliesInRange(700) == 0 &&
                            !Overkill(enemy))
                        {
                            var rpred = _r.GetPrediction(enemy);
                            if (rpred.HitChancePercent >= Predslider)
                            {
                                _r.Cast(enemy);
                            }
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

        public static double RDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.R).IsLearned) return 0;
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                (float) new double[] {250, 425, 600}[_r.Level - 1] + 1*Player.Instance.FlatMagicDamageMod);
        }

        private static double ComboDamage(Obj_AI_Base target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target);


            if (_q.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);
            }

            if (_w.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.W);
            }

            if (_r.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.R);
            }

            return damage;
        }

        private static bool Overkill(Obj_AI_Base enemy)
        {
            var damage = 0f;

            if (Player.Instance.IsInAutoAttackRange(enemy) && Player.Instance.CanAttack)
            {
                damage += Player.Instance.GetAutoAttackDamage(enemy);
            }

            if (_r.IsReady())
            {
                if (enemy != null)
                {
                    var wpred = _w.GetPrediction(enemy);
                    if (_w.IsReady() && enemy.IsValidTarget(_w.Range) && !wpred.CollisionObjects.Any())
                    {
                        damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.W);
                    }
                }
            }
            return enemy.TotalShieldHealth() <= damage;
        }

        #endregion

        #endregion

        #region Drawings

        private static void GameOnDraw(EventArgs args)
        {
            var colorW = MainMenu.Draw.GetColor("color.w");
            var widthW = MainMenu.Draw.GetWidth("width.w");

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