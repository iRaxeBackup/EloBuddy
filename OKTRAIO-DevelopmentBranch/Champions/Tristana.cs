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
    internal class Tristana : AIOChampion
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

        private static Spell.Active _q;
        private static Spell.Skillshot _w;
        private static Spell.Targeted _e, _r;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            try
            {
                //Creating Spells
                _q = new Spell.Active(SpellSlot.Q, 550);
                _w = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 450, int.MaxValue, 180);
                _e = new Spell.Targeted(SpellSlot.E, 550);
                _r = new Spell.Targeted(SpellSlot.R, 550);

                try
                {
                    //Combo Menu Settings
                    MainMenu.ComboKeys(defaultW: false);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.1", true);
                    MainMenu.Combo.AddCheckBox("combo.e.forced", "Force target with E ", true, true);
                    MainMenu.Combo.AddCheckBox("combo.e.r", "Use E + R Finisher", true, true);
                    MainMenu.Combo.AddCheckBox("combo.r.tower", "Use R to throw the enemy under ally tower", true, true);
                    MainMenu.Combo.AddSlider("combo.w.range",
                        "Don't Use W if in Range from Target there are more than {0} enemies", 1, 1, 5, true);
                    MainMenu.Combo.AddSlider("combo.w.e", "Max enemies for Jump", 1, 1, 5, true);
                    MainMenu.Combo.AddSlider("combo.e.r.damages", "E + R Overkill", 60, 1, 500, true);
                    MainMenu.Combo.AddSlider("combo.r.damage", "R Overkill", 50, 1, 500, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Prediction Settings", "combo.grouplabel.2", true);
                    MainMenu.Combo.AddSlider("combo.w.prediction", "Use W if Hitchance > {0}%", 80, 0, 100, true);
                    MainMenu.Combo.AddSeparator();
                    MainMenu.Combo.AddGroupLabel("Mana Manager:", "combo.grouplabel.3", true);
                    MainMenu.ComboManaManager(true, true, true, true, 60, 50, 40, 50);

                    //Lane Clear Menu Settings
                    MainMenu.LaneKeys(defaultW: false);
                    MainMenu.Lane.AddSeparator();
                    MainMenu.Lane.AddGroupLabel("Laneclear Preferences", "lane.grouplabel.1", true);
                    MainMenu.Lane.AddCheckBox("lane.e.forced", "Force minion with E ", true, true);
                    MainMenu.Lane.AddCheckBox("lane.save.tower", "Save E for use on Tower", true, true);
                    MainMenu.Lane.AddSeparator();
                    MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.2", true);
                    MainMenu.LaneManaManager(true, true, true, false, 60, 50, 40, 50);

                    //Jungle Clear Menu Settings
                    MainMenu.JungleKeys(defaultW: false);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Jungleclear Preferences", "jungle.grouplabel.1", true);
                    MainMenu.Jungle.AddCheckBox("jungle.monsters.spell", "Use Abilities on Big Monster", true, true);
                    MainMenu.Jungle.AddCheckBox("jungle.minimonsters.spell", "Use Abilities on Mini Monsters", false,
                        true);
                    MainMenu.Jungle.AddSeparator();
                    MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.2", true);
                    MainMenu.JungleManaManager(true, true, true, false, 60, 50, 40, 50);

                    //Last hit Menu Settings
                    MainMenu.LastHitKeys(defaultW: false, defaultR: false);
                    MainMenu.Lasthit.AddSeparator();
                    MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.1", true);
                    MainMenu.LasthitManaManager(false, true, false, false, 60, 50, 40, 50);

                    //Harras
                    MainMenu.HarassKeys(defaultW: false);
                    MainMenu.Harass.AddSeparator();
                    MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.1", true);
                    MainMenu.HarassManaManager(true, true, true, false, 60, 50, 40, 50);

                    //Flee Menu
                    MainMenu.FleeKeys();
                    MainMenu.Flee.AddSeparator();
                    MainMenu.Flee.AddGroupLabel("HP Manager", "flee.grouplabel.1", true);
                    MainMenu.Flee.AddSlider("flee.r.hp", "Use R when you HP are less than {0}%", 20, 0, 100, true);

                    //Ks
                    MainMenu.KsKeys(defaultW: false);
                    MainMenu.Ks.AddSeparator();
                    MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.5", true);
                    MainMenu.KsManaManager(false, true, true, true, 60, 50, 40, 50);

                    //Misc Menu
                    MainMenu.MiscMenu();
                    MainMenu.Misc.AddCheckBox("misc.w.gapcloser", "Use W for Anti-GapCloser", false);
                    MainMenu.Misc.AddCheckBox("misc.r.interrupter", "Use R for Interrupt");
                    MainMenu.Misc.AddCheckBox("misc.r.gapcloser", "Use R for Anti-GapCloser");
                    MainMenu.Misc.AddCheckBox("misc.e", "Use Auto E");
                    MainMenu.Misc.AddSeparator();
                    MainMenu.Misc.AddGroupLabel("Interrupter - AntiGapCloser settings", "misc.grouplabel.6", true);
                    MainMenu.Misc.AddSlider("misc.r.interrupter.mana", "Min. Mana to interrupt", 30, 0, 100, true);
                    MainMenu.Misc.AddSlider("misc.r.gapcloser.mana", "Min. Mana% to use R for Anti-GapCloser", 30, 0,
                        100, true);
                    MainMenu.Misc.AddGroupLabel("Auto E Settings", "misc.grouplabel.3", true);
                    MainMenu.Misc.AddCheckBox("misc.e.stun", "Use E on Stunned Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.e.charm", "Use E on Charmed Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.e.taunt", "Use E on Taunted Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.e.fear", "Use E on Feared Enemy", true, true);
                    MainMenu.Misc.AddCheckBox("misc.e.snare", "Use E on Snared Enemy", true, true);
                    MainMenu.Misc.AddSlider("misc.e.mana", "Use E on CC Enemy if Mana is above than {0}%", 10, 0, 100,
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
                Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
                Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
                Gapcloser.OnGapcloser += AntiGapCloser;
                GameObject.OnCreate += GameObject_OnCreate;

                if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
                {
                    Game.OnUpdate += GameOnUpdate;
                }
                else
                {
                    Game.OnTick += GameOnUpdate;
                }

                Orbwalker.OnPreAttack += OnPreAttack;
                Obj_AI_Base.OnBuffGain += BuffGain;

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
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Physical);

            var targetBoom =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.HasBuff("tristanaecharge") && a.Distance(Player.Instance) < Player.Instance.AttackRange);

            if (target == null || target.IsZombie) return;

            if (Player.Instance.ManaPercent >= Value.Get("combo.e.mana"))
            {
                if (_e.IsReady() && target.IsValidTarget(_e.Range) && Value.Use("combo.e".AddName()))
                {
                    _e.Cast(target);
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("combo.q.mana"))
            {
                if (_q.IsReady() && target.IsValidTarget(_q.Range) && Value.Use("combo.q".AddName()))
                {
                    _q.Cast();
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("combo.w.mana"))
            {
                if (_w.IsReady() && target.IsValidTarget(_w.Range) && Value.Use("combo.w".AddName()) &&
                    !target.IsInvulnerable &&
                    target.Position.CountEnemiesInRange(800) <= Value.Get("combo.w.e"))
                {
                    if (_w.GetPrediction(target).HitChancePercent >= Value.Get("combo.w.prediction"))
                    {
                        var optimizedCircleLocation = OKTRGeometry.GetOptimizedCircleLocation(_w, target);
                        if (optimizedCircleLocation != null)
                            _w.Cast(optimizedCircleLocation.Value.Position.To3D());
                    }
                }
            }

            if (targetBoom != null)
            {
                if (Value.Use("combo.e.r") && !_e.IsReady() && _r.IsReady() && targetBoom.IsValidTarget(_r.Range) &&
                    !targetBoom.IsInvulnerable &&
                    targetBoom.Health + targetBoom.AllShield + Value.Get("combo.e.r.damages") -
                    (Player.Instance.GetSpellDamage(targetBoom, SpellSlot.E) +
                     targetBoom.Buffs.Find(a => a.Name == "tristanaecharge").Count*
                     Player.Instance.GetSpellDamage(targetBoom, SpellSlot.E, DamageLibrary.SpellStages.Detonation)) <
                    Player.Instance.GetSpellDamage(targetBoom, SpellSlot.R))
                {
                    _r.Cast(targetBoom);
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("combo.r.mana"))
            {
                if (_r.IsReady() && Value.Use("combo.r".AddName()))
                {
                    if (_r.IsReady() && target.IsValidTarget(_r.Range) && !target.IsInvulnerable &&
                        target.Health + target.AttackShield + Value.Get("combo.r.damage") <
                        Player.Instance.GetSpellDamage(target, SpellSlot.R))
                    {
                        _r.Cast(target);
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
                    _q.Cast();
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("harass.w.mana"))
            {
                if (Value.Use("harass.w") && _w.IsReady())
                {
                    _w.Cast(target);
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("harass.e.mana"))
            {
                if (Value.Use("harass.e") && _e.IsReady())
                {
                    _e.Cast(target);
                }
            }
        }

        #endregion

        #region LastHit

        public override void LastHit()
        {
            var source =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m =>
                        m.IsValidTarget(_r.Range) &&
                        Player.Instance.GetSpellDamage(m, SpellSlot.R) > m.TotalShieldHealth())
                    .FirstOrDefault();

            var predW = OKTRGeometry.GetOptimizedCircleLocation(
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m =>
                        m.IsValidTarget(_w.Range) &&
                        Player.Instance.GetSpellDamage(m, SpellSlot.W) > m.TotalShieldHealth())
                    .Select(m => m.Position.To2D())
                    .ToList(),
                _w.Width, _w.Range).Position.To3D();

            if (source == null) return;

            if (Player.Instance.ManaPercent >= Value.Get("lasthit.w.mana"))
            {
                if (Value.Use("lasthit.w") && _w.IsReady())
                {
                    _w.Cast(predW);
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

            if (Value.Use("jungle.monsters.spell"))
            {
                if (monsters == null) return;

                if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                {
                    if (Value.Use("jungle.q") && _q.IsReady())
                    {
                        _q.Cast();
                    }
                }

                if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                {
                    if (Value.Use("jungle.w") && _w.IsReady())
                    {
                        _w.Cast(monsters);
                    }
                }

                if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                {
                    if (Value.Use("jungle.e") && _e.IsReady())
                    {
                        _e.Cast(monsters);
                    }
                }
            }

            if (Value.Use("jungle.minimonsters.spell"))
            {
                if (fappamonsters == null) return;

                if (Player.Instance.ManaPercent >= Value.Get("jungle.q.mana"))
                {
                    if (Value.Use("jungle.q") && _q.IsReady())
                    {
                        _q.Cast();
                    }
                }

                if (Player.Instance.ManaPercent >= Value.Get("jungle.w.mana"))
                {
                    if (Value.Use("jungle.w") && _w.IsReady())
                    {
                        _w.Cast(fappamonsters);
                    }
                }

                if (Player.Instance.ManaPercent >= Value.Get("jungle.e.mana"))
                {
                    if (Value.Use("jungle.e") && _w.IsReady())
                    {
                        _e.Cast(fappamonsters);
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
            var tawah = EntityManager.Turrets.Enemies.FirstOrDefault(t => !t.IsDead && t.IsInRange(Player.Instance, 775));
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    Player.Instance.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            var sourceE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .FirstOrDefault(m => m.IsValidTarget(Player.Instance.AttackRange) && m.HasBuff("tristanaecharge"));
            var predW = OKTRGeometry.GetOptimizedCircleLocation(
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, _w.Range, false).Select(m => m.Position.To2D()).ToList(),
                _w.Width, _w.Range).Position.To3D();

            if (Value.Use("lane.e.forced"))
            {
                if (sourceE != null)
                {
                    Orbwalker.ForcedTarget = sourceE;
                }
                else
                {
                    Orbwalker.ForcedTarget = null;
                }
            }

            if (count == 0) return;

            if (Value.Get("lane.e.mana") >= Player.Instance.ManaPercent)
            {
                if (_e.IsReady() && Value.Use("lane.e") && !Value.Use("lane.save.tower"))
                {
                    _e.Cast(source);
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
            {
                if (_q.IsReady() && Value.Use("lane.q"))
                {
                    _q.Cast();
                }
            }

            if (Player.Instance.ManaPercent >= Value.Get("lane.w.mana"))
            {
                if (_w.IsReady() && Value.Use("lane.w"))
                {
                    _w.Cast(predW);
                }
            }

            if (tawah == null) return;

            if (Player.Instance.ManaPercent >= Value.Get("lane.e.mana"))
            {
                if (Value.Use("lane.save.tower") && tawah.IsInRange(Player.Instance, _e.Range) && _e.IsReady())
                {
                    _e.Cast(tawah);
                    _q.Cast();
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);

            if (_w.IsReady() && Value.Use("flee.w") && _w.IsLearned)
            {
                _w.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _w.Range).To3D());
            }

            if (_r.IsReady() && Value.Use("flee.r") && Player.Instance.HealthPercent <= Value.Get("flee.r.hp") &&
                target != null)
            {
                _r.Cast(target);
            }
        }

        #endregion

        #endregion

        #region Utils 

        #region OnLVLUP

        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            try
            {
                if (!sender.IsMe) return;
                _q = new Spell.Active(SpellSlot.Q, 543 + 7*(uint) Player.Instance.Level);
                _e = new Spell.Targeted(SpellSlot.E, 543 + 7*(uint) Player.Instance.Level);
                _r = new Spell.Targeted(SpellSlot.R, 543 + 7*(uint) Player.Instance.Level);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code ONLVLUP)</font>");
            }
        }

        #endregion

        #region AntiGapCloser

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            try
            {
                if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Instance.Type || !e.Sender.IsEnemy)
                    return;
                if (_r.IsReady() && Value.Use("misc.r.gapcloser") &&
                    Player.Instance.ManaPercent <= Value.Get("misc.r.gapcloser.mana"))
                {
                    _r.Cast(e.Sender);
                }
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

        #region Interrupter

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            try
            {
                if (!sender.IsValidTarget(_q.Range) || e.DangerLevel != DangerLevel.High ||
                    e.Sender.Type != Player.Instance.Type || !e.Sender.IsEnemy ||
                    Player.Instance.ManaPercent <= Value.Get("misc.r.interrupter.mana"))
                    return;
                if (_r.IsReady() && _r.IsInRange(sender) && Value.Use("misc.r.interrupter"))
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

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var rengar = EntityManager.Heroes.Enemies.Find(r => r.ChampionName.Equals("Rengar"));
            var khazix = EntityManager.Heroes.Enemies.Find(z => z.ChampionName.Equals("Khazix"));

            try
            {
                if (Value.Use("misc.r.gapcloser") && Player.Instance.ManaPercent >= Value.Get("misc.r.gapcloser.mana"))
                {
                    if (khazix != null)
                    {
                        if (sender.Name == "Khazix_Base_E_Tar.troy" &&
                            sender.Position.Distance(Player.Instance) <= 400) _r.Cast(khazix);
                    }
                    if (rengar != null)
                    {
                        if (sender.Name == "Rengar_LeapSound.troy" &&
                            sender.Position.Distance(Player.Instance) < _r.Range) _r.Cast(rengar);
                    }
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

        #region OnUpdate

        private void GameOnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.HasBuff("Recall")
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed ||
                Orbwalker.IsAutoAttacking) return;

            if (Value.Use("killsteal.w") && _w.IsReady() ||
                Value.Use("killsteal.e") && _e.IsReady() ||
                Value.Use("killsteal.r") && _r.IsReady())
            {
                KillSteal();
            }
        }

        #endregion

        #region OnBuffGain

        private void BuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (Value.Use("misc.e") && _e.IsReady())
            {
                if (sender == null || !sender.HasBuff("tristanaecharge")) return;

                if (Player.Instance.ManaPercent >= Value.Get("misc.e.mana"))
                {
                    if (Value.Use("misc.e.stun") && sender.IsStunned)
                    {
                        _e.Cast(sender);
                    }
                    if (Value.Use("misc.e.snare") && sender.IsRooted)
                    {
                        _e.Cast(sender);
                    }
                    if (Value.Use("misc.e.charm") && sender.IsCharmed)
                    {
                        _e.Cast(sender);
                    }
                    if (Value.Use("misc.e.taunt") && sender.IsTaunted)
                    {
                        _e.Cast(sender);
                    }
                    if (Value.Use("misc.e.fear") && sender.IsFeared)
                    {
                        _e.Cast(sender);
                    }
                }
            }
        }


        #endregion

                #region ComboDamage

            private static
            float ComboDamage(Obj_AI_Base enemy)
        {
            var damage = Player.Instance.GetAutoAttackDamage(enemy);


            if (_w.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.W);
            }

            if (_e.IsReady())
            {
                if (enemy.HasBuff("tristanaecharge"))
                {
                    damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.E) +
                              Player.Instance.GetSpellDamage(enemy, SpellSlot.E)*
                              (enemy.GetBuffCount("tristanaecharge")*.30f);
                }
            }

            if (_r.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
            }

            return damage;
        }

        #endregion

        #region OnEndScene

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

        #region KillSteal

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
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.e.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.E) && !target.HasBuff("tristanaecharge"))

                        {
                            _e.Cast(target);
                        }
                    }


                    var tawah =
                        EntityManager.Turrets.Enemies.FirstOrDefault(
                            a =>
                                !a.IsDead && a.Distance(target) <= 775 + Player.Instance.BoundingRadius +
                                target.BoundingRadius/2 + 44.2);

                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.r.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.R) && !target.HasBuff("tristanaecharge"))

                        {
                            _r.Cast(target);
                        }
                    }
                    if (Player.Instance.ManaPercent >= Value.Get("killsteal.w.mana"))
                    {
                        if (target.Health + target.AttackShield <
                            Player.Instance.GetSpellDamage(target, SpellSlot.W) &&
                            target.Position.CountEnemiesInRange(800) == 1 && tawah == null &&
                            Player.Instance.Mana >= 120)
                        {
                            _w.Cast(_w.GetPrediction(target).CastPosition);
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

        #region OnPreAttack

        private void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                if (Value.Use("combo.e.forced"))
                {
                    var forcedtarget = Variables.CloseEnemies(_q.Range).Find
                        (a => a.HasBuff("tristanaecharge"));

                    Player.IssueOrder(GameObjectOrder.AttackUnit, forcedtarget, true);
                }
            }
        }

        #endregion

        #endregion
    }
}