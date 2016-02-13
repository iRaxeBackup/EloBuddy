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

namespace OKTRAIO.Champions
{
    internal class Teemo : AIOChampion
    {
        #region Initialize and Declare

        private static Spell.Targeted _q;
        private static Spell.Active _w, _e;
        private static Spell.Skillshot _r;
        private static int _rDelay;
        private static readonly Vector2 Offset = new Vector2(1, 0);

        public override void Init()
        {
            try
            {
                //spells
                _q = new Spell.Targeted(SpellSlot.Q, 680);
                _w = new Spell.Active(SpellSlot.W);
                _e = new Spell.Active(SpellSlot.E);
                _r = new Spell.Skillshot(SpellSlot.R, 400, SkillShotType.Circular, 500, 1000, 120)
                {
                    AllowedCollisionCount = int.MaxValue
                };

                //menu

                //combo
                MainMenu.ComboKeys(useE: false);
                MainMenu.Combo.AddSeparator();
                MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.grouplabel.addonmenu", true);
                MainMenu.Combo.AddGroupLabel("Use Q only on:", "combo.grouplabe2.addonmenu");
                if (EntityManager.Heroes.Enemies.Count > 0)
                {
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        MainMenu.Combo.Add(enemy.ChampionName, new CheckBox(enemy.ChampionName));
                    }
                }
                MainMenu.Combo.AddSlider("combo.w.distance", "Use W when there is an enemy in {0} range", 600, 1, 1200,
                    true);
                MainMenu.Combo.AddSlider("combo.r.stacks", "Keep shrooms at {0} stacks", 1, 0, 3, true);

                //flee
                MainMenu.FleeKeys(useE: false);
                MainMenu.Flee.AddSeparator();
                MainMenu.Flee.AddSlider("flee.r.stacks", "Keep shrooms at {0} stacks", 1, 0, 3, true);
                MainMenu.Flee.AddGroupLabel("Mana Manager:", "flee.grouplabel.addonmenu", true);
                MainMenu.FleeManaManager(true, true, false, true, 20, 20, 0, 20);

                //lasthit
                MainMenu.LastHitKeys(useW: false, useE: false, useR: false);
                MainMenu.Lasthit.AddSeparator();
                MainMenu.Lasthit.AddGroupLabel("Mana Manager:", "lasthit.grouplabel.addonmenu", true);
                MainMenu.LasthitManaManager(true, false, false, false, 20, 0, 0, 0);

                //laneclear
                MainMenu.LaneKeys(useW: false, useE: false);
                MainMenu.Lane.AddSeparator();
                MainMenu.Lane.AddSlider("lane.r.min", "Min. {0} minions for R", 3, 1, 7, true);
                MainMenu.Lane.AddSlider("lane.r.stacks", "Keep shrooms at {0} stacks", 1, 0, 3, true);
                MainMenu.Lane.AddGroupLabel("Mana Manager:", "lane.grouplabel.addonmenu", true);
                MainMenu.LaneManaManager(true, false, false, true, 60, 0, 0, 60);

                //jungleclear
                MainMenu.JungleKeys(useW: false, useE: false);
                MainMenu.Jungle.AddSeparator();
                MainMenu.Jungle.AddSlider("jungle.r.min", "Min. {0} minions for R", 3, 1, 4, true);
                MainMenu.Jungle.AddSlider("jungle.r.stacks", "Keep shrooms at {0} stacks", 1, 0, 3, true);
                MainMenu.Jungle.AddGroupLabel("Mana Manager:", "jungle.grouplabel.addonmenu", true);
                MainMenu.JungleManaManager(true, false, false, true, 60, 0, 0, 60);

                //harass
                MainMenu.HarassKeys(useW: false, useE: false, useR: false);
                MainMenu.Harass.AddSeparator();
                MainMenu.Harass.AddGroupLabel("Mana Manager:", "harass.grouplabel.addonmenu", true);
                MainMenu.HarassManaManager(true, false, false, false, 60, 0, 0, 0);

                //Ks
                MainMenu.KsKeys(useW: false, useE: false, useR: false);
                MainMenu.Ks.AddSeparator();
                MainMenu.Ks.AddGroupLabel("Mana Manager:", "killsteal.grouplabel.addonmenu", true);
                MainMenu.KsManaManager(true, false, false, false, 20, 0, 0, 0);

                //misc
                MainMenu.MiscMenu();
                MainMenu.Misc.Add("misc.r.auto",
                    new KeyBind("Auto shroom locations", true, KeyBind.BindTypes.PressToggle, 'G'));
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddSlider("misc.r.stacks", "Keep shrooms at {0} stacks", 1, 0, 3);
                MainMenu.Misc.AddSlider("misc.r.mana", "Auto Shroom until {0}% mana", 30);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddCheckBox("misc.q.gapcloser", "Use Q on gapcloser");
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Auto Q/R Settings", "misc.grouplabel1.addonmenu", true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddCheckBox("misc.q.charm", "Use Q on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.stun", "Use Q on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.knockup", "Use Q on Knocked Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.snare", "Use Q on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.suppression", "Use Q on Suppressed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.q.taunt", "Use Q on Taunted Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddCheckBox("misc.r.charm", "Use R on Charmed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.r.stun", "Use R on Stunned Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.r.knockup", "Use R on Knocked Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.r.snare", "Use R on Snared Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.r.suppression", "Use R on Suppressed Enemy", true, true);
                MainMenu.Misc.AddCheckBox("misc.r.taunt", "Use R on Taunted Enemy", true, true);
                MainMenu.Misc.AddSeparator();
                MainMenu.Misc.AddGroupLabel("Prediction", "combo.grouplabel2.addonmenu", true);
                MainMenu.Misc.AddSlider("misc.r.prediction", "Hitchance Percentage for R", 80, 0, 100, true);
                MainMenu.Misc.AddSlider("misc.r.delay", "Shroom Cast Delay", 1500, 1000, 4000, true);

                //draw
                MainMenu.DrawKeys(useW: false, useE: false);
                MainMenu.Draw.AddSeparator();
                MainMenu.Draw.AddCheckBox("draw.hp.bar", "Draw Combo Damage", true, true);
                MainMenu.Draw.AddCheckBox("draw.r.auto", "Draw Auto Shroom Locations", true, true);
                MainMenu.Draw.AddCheckBox("draw.status", "Draw Auto Shroom Status", true, true);
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
                Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
                Drawing.OnDraw += GameOnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
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
            if (_q.IsReady() && Value.Use("combo.q".AddName()))
            {
                foreach (
                    var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            enemy =>
                                MainMenu.Combo[enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                                enemy.IsValidTarget() && !enemy.HasBuffOfType(BuffType.SpellShield) &&
                                !enemy.IsZombie && _q.IsInRange(enemy) && !Orbwalker.IsAutoAttacking))
                {
                    _q.Cast(enemy);
                }
            }

            if (_w.IsReady() && Value.Use("combo.w".AddName()))
            {
                if (Player.Instance.CountEnemiesInRange(Value.Get("combo.w.distance")) > 0)
                {
                    _w.Cast();
                }
            }

            if (_r.IsReady() && Value.Use("combo.r".AddName()) && Rstacks > Value.Get("combo.r.stacks"))
            {
                var targetr = TargetSelector.GetTarget(_r.Range, DamageType.Magical);

                if (targetr != null)
                {
                    var rpred = _r.GetPrediction(targetr);
                    if (rpred.HitChancePercent >= Value.Get("misc.r.prediction") && !Orbwalker.IsAutoAttacking)
                    {
                        Rcast(rpred.CastPosition + 50);
                    }
                }
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);


            if (_q.IsReady() && Value.Use("harass.q") && Player.Instance.ManaPercent >= Value.Get("harass.q.mana"))
            {
                if (target != null && !Orbwalker.IsAutoAttacking)
                {
                    _q.Cast(target);
                }
            }
        }

        #endregion

        #region Laneclear

        public override void Laneclear()
        {
            var minionq =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(
                        a =>
                            a.IsValidTarget(_q.Range) && a.Health <= QDamage(a) &&
                            a.Health >= Player.Instance.GetAutoAttackDamage(a, true));


            if (_q.IsReady() && Value.Use("lane.q") && Player.Instance.ManaPercent >= Value.Get("lane.q.mana"))
            {
                if (minionq != null && !Orbwalker.IsAutoAttacking)
                {
                    _q.Cast(minionq);
                }
            }

            if (_r.IsReady() && Value.Use("lane.r") && Player.Instance.ManaPercent >= Value.Get("lane.r.mana"))
            {
                var minionr = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, _r.Range).Where(a => !a.HasBuff("bantamtrapslow") && !a.IsMoving);
                var farmr = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minionr, _r.Width + 80,
                    (int) _r.Range);

                if (farmr.HitNumber >= Value.Get("lane.r.min") && Rstacks > Value.Get("lane.r.stacks"))
                {
                    Rcast(farmr.CastPosition);
                }
            }
        }

        #endregion

        #region Jungleclear

        public override void Jungleclear()
        {
            var monsterq =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(
                        a =>
                            a.IsValidTarget(_q.Range) && Variables.SummonerRiftJungleList.Contains(a.BaseSkinName) &&
                            a.Health >= QDamage(a));


            if (_q.IsReady() && Value.Use("jungle.q") && Player.Instance.ManaPercent >= Value.Get("jungle.q.mana") &&
                !Orbwalker.IsAutoAttacking)
            {
                if (monsterq != null)
                {
                    _q.Cast(monsterq);
                }
            }


            if (_r.IsReady() && Value.Use("jungle.r") && Player.Instance.ManaPercent >= Value.Get("jungle.r.mana"))
            {
                var monsterr =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                        _r.Range).Where(a => !a.HasBuff("bantamtrapslow") && !a.IsMoving);
                var farmr = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(monsterr, _r.Width + 80,
                    (int) _r.Range);

                if (farmr.HitNumber >= Value.Get("jungle.r.min") && Rstacks > Value.Get("jungle.r.stacks"))
                {
                    Rcast(farmr.CastPosition);
                }
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            var target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);


            if (_w.IsReady() && Value.Use("flee.w") && Player.Instance.ManaPercent >= Value.Get("flee.w.mana"))
            {
                _w.Cast();
            }


            if (target == null)
            {
                return;
            }

            var tpos = Player.Instance.Position.Extend(target.Position, 100).To3D();


            if (_q.IsReady() && Value.Use("flee.q") && Player.Instance.ManaPercent >= Value.Get("flee.q.mana"))
            {
                _q.Cast(target);
            }


            if (_r.IsReady() && Value.Use("flee.r") && Player.Instance.ManaPercent >= Value.Get("flee.r.mana"))
            {
                if (Rstacks > Value.Get("flee.r.stacks"))
                {
                    Rcast(tpos);
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
                            a.IsValidTarget(_q.Range) && a.Health <= QDamage(a) &&
                            a.Health >= Player.Instance.GetAutoAttackDamage(a, true));


            if (_q.IsReady() && Value.Use("lasthit.q") && Player.Instance.ManaPercent >= Value.Get("lasthit.q.mana"))
            {
                if (minion != null)
                {
                    _q.Cast(minion);
                }
            }
        }

        #endregion

        #endregion

        #region Utils

        #region OnUpdate

        private static void GameOnUpdate(EventArgs args)
        {
            if (_r.IsLearned)
            {
                _r = new Spell.Skillshot(SpellSlot.R, (uint) new[] {400, 650, 900}[_r.Level - 1], SkillShotType.Circular,
                    500, 1000, 120);
            }

            Ks();

            AutoRcc();

            AutoShroom();
        }

        #endregion

        #region OnSpellCast

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Slot == SpellSlot.R)
            {
                _rDelay = Core.GameTickCount;
            }
        }

        #endregion

        #region AntiGapCloser

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsAlly || !Value.Use("misc.q.gapcloser"))
            {
                return;
            }

            if (_q.IsReady() && e.End.Distance(Player.Instance.Position) <= _q.Range)
            {
                _q.Cast(sender);
            }
        }

        #endregion

        #region Check RStacks

        private static int Rstacks
        {
            get { return _r.Handle.Ammo; }
        }

        #endregion

        #region Vector Shroomed

        private static bool Shroomed(Vector3 castposition)
        {
            return
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.Name == "Noxious Trap").Any(a => castposition.Distance(a.Position) <= 300);
        }

        private static void Rcast(Vector3 location)
        {
            if (!Shroomed(location) && Core.GameTickCount - _rDelay > Value.Get("misc.r.delay"))
            {
                _r.Cast(location);
            }
        }

        #endregion

        #region Killsteal

        private static void Ks()
        {
            if (_q.IsReady() && Value.Use("killsteal.q") && Player.Instance.ManaPercent >= Value.Get("killsteal.q.mana"))
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies.Where(
                            a =>
                                a.IsValid && _q.IsInRange(a) && !Orbwalker.IsAutoAttacking && !a.IsDead &&
                                !a.IsZombie && !a.HasBuff("ChronoShift") &&
                                !a.HasBuffOfType(BuffType.Invulnerability) && a.TotalShieldHealth() <= QDamage(a)))
                {
                    _q.Cast(target);
                }
            }
        }

        #endregion

        #region AutoR

        private static void AutoRcc()
        {
            var targetq =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.IsValidTarget(_q.Range) &&
                         (a.HasBuffOfType(BuffType.Charm) || a.HasBuffOfType(BuffType.Knockup) ||
                          a.HasBuffOfType(BuffType.Snare) || a.HasBuffOfType(BuffType.Stun) ||
                          a.HasBuffOfType(BuffType.Suppression) || a.HasBuffOfType(BuffType.Taunt)));


            if (_q.IsReady() && targetq != null)
            {
                if (Value.Use("misc.q.charm") && targetq.IsCharmed ||
                    Value.Use("misc.q.knockup") ||
                    Value.Use("misc.q.stun") && targetq.IsStunned ||
                    Value.Use("misc.q.snare") && targetq.IsRooted ||
                    Value.Use("misc.q.suppression") && targetq.IsSuppressCallForHelp ||
                    Value.Use("misc.q.taunt") && targetq.IsTaunted)
                {
                    _q.Cast(targetq);
                }
            }


            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.IsValidTarget(_r.Range) &&
                         (a.HasBuffOfType(BuffType.Charm) || a.HasBuffOfType(BuffType.Knockup) ||
                          a.HasBuffOfType(BuffType.Snare) || a.HasBuffOfType(BuffType.Stun) ||
                          a.HasBuffOfType(BuffType.Suppression) || a.HasBuffOfType(BuffType.Taunt)));

            if (_r.IsReady() && target != null)
            {
                if (Value.Use("misc.r.charm") && target.IsCharmed ||
                    Value.Use("misc.r.knockup") ||
                    Value.Use("misc.r.stun") && target.IsStunned ||
                    Value.Use("misc.r.snare") && target.IsRooted ||
                    Value.Use("misc.r.suppression") && target.IsSuppressCallForHelp ||
                    Value.Use("misc.r.taunt") && target.IsTaunted)
                {
                    Rcast(_r.GetPrediction(target).CastPosition);
                }
            }
        }

        #endregion

        #region AutoPlaceShroom

        private static void AutoShroom()
        {
            if (_r.IsReady())
            {
                if (Value.Active("misc.r.auto") && Rstacks > Value.Get("misc.r.stacks") &&
                    Player.Instance.ManaPercent >= Value.Get("misc.r.mana") && !Player.Instance.IsRecalling() &&
                    !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                    Game.MapId == GameMapId.SummonersRift)
                {
                    var teemo = Player.Instance.Position;

                    //top
                    var topTri = new Vector2(4467, 11853);
                    var topRiver = new Vector2(3099, 10826);
                    var topPixel = new Vector2(5229, 9150);
                    var topEntrance = new Vector2(3916, 12830);
                    var baron = new Vector2(4677, 10075);

                    //mid
                    var midbrushleft = new Vector2(6512, 8368);
                    var midbushright = new Vector2(8411, 6498);
                    var midleftbrush = new Vector2(5029, 8503);
                    var midrighbrush = new Vector2(9856, 6520);

                    //bot
                    var botPixel = new Vector2(9427, 5675);
                    var botTri = new Vector2(10392, 3094);
                    var botRiver = new Vector2(11791, 4108);
                    var botEntrance = new Vector2(10894, 1995);
                    var dragon = new Vector2(10169, 4838);

                    //red jungle
                    var rkrugsBrush = new Vector2(5643, 12789);
                    var rredbrus1 = new Vector2(7994, 11852);
                    var rredbrush2 = new Vector2(6731, 11463);
                    var rredbrush3 = new Vector2(6291, 10150);
                    var rbasebrush = new Vector2(9230, 11503);
                    var rwraitshbrush = new Vector2(8291, 10263);
                    var rwolvesbrush = new Vector2(9964, 7929);
                    var rbluebrush = new Vector2(11486, 7172);
                    var rgrompbrush = new Vector2(12493, 5223);
                    var rtoplanebrush = new Vector2(7160, 14120);
                    var rbotlanebrush = new Vector2(14108, 7031);

                    //blue jungle
                    var bkrugsBrush = new Vector2(9219, 2181);
                    var bredbrus1 = new Vector2(6848, 3120);
                    var bredbrush2 = new Vector2(8051, 3525);
                    var bredbrush3 = new Vector2(8543, 4848);
                    var bbasebrush = new Vector2(5609, 3509);
                    var bwraitshbrush = new Vector2(6545, 4711);
                    var bwolvesbrush = new Vector2(4820, 7149);
                    var bbluebrush = new Vector2(3384, 7787);
                    var bgrompbrush = new Vector2(2311, 9752);
                    var btoplanebrush = new Vector2(826, 8171);
                    var bbotlanebrush = new Vector2(7778, 831);

                    if (Player.Instance.CountEnemiesInRange(_q.Range) == 0)
                    {
                        //top
                        if (teemo.Distance(topTri) < _r.Range)
                        {
                            Rcast(topTri.To3D());
                        }
                        if (teemo.Distance(topRiver) < _r.Range)
                        {
                            Rcast(topRiver.To3D());
                        }
                        if (teemo.Distance(topPixel) < _r.Range)
                        {
                            Rcast(topPixel.To3D());
                        }
                        if (teemo.Distance(topEntrance) < _r.Range)
                        {
                            Rcast(topEntrance.To3D());
                        }
                        if (teemo.Distance(baron) < _r.Range)
                        {
                            Rcast(baron.To3D());
                        }

                        //mid
                        if (teemo.Distance(midbrushleft) < _r.Range)
                        {
                            Rcast(midbrushleft.To3D());
                        }
                        if (teemo.Distance(midbushright) < _r.Range)
                        {
                            Rcast(midbushright.To3D());
                        }
                        if (teemo.Distance(midleftbrush) < _r.Range)
                        {
                            Rcast(midleftbrush.To3D());
                        }
                        if (teemo.Distance(midrighbrush) < _r.Range)
                        {
                            Rcast(midrighbrush.To3D());
                        }

                        //bot
                        if (teemo.Distance(botTri) < _r.Range)
                        {
                            Rcast(botTri.To3D());
                        }
                        if (teemo.Distance(botPixel) < _r.Range)
                        {
                            Rcast(botPixel.To3D());
                        }
                        if (teemo.Distance(botEntrance) < _r.Range)
                        {
                            Rcast(botEntrance.To3D());
                        }
                        if (teemo.Distance(botRiver) < _r.Range)
                        {
                            Rcast(botRiver.To3D());
                        }
                        if (teemo.Distance(dragon) < _r.Range)
                        {
                            Rcast(dragon.To3D());
                        }

                        //red
                        if (teemo.Distance(rbasebrush) < _r.Range)
                        {
                            Rcast(rbasebrush.To3D());
                        }
                        if (teemo.Distance(rbluebrush) < _r.Range)
                        {
                            Rcast(rbluebrush.To3D());
                        }
                        if (teemo.Distance(rbotlanebrush) < _r.Range)
                        {
                            Rcast(rbotlanebrush.To3D());
                        }
                        if (teemo.Distance(rgrompbrush) < _r.Range)
                        {
                            Rcast(rgrompbrush.To3D());
                        }
                        if (teemo.Distance(rkrugsBrush) < _r.Range)
                        {
                            Rcast(rkrugsBrush.To3D());
                        }
                        if (teemo.Distance(rredbrus1) < _r.Range)
                        {
                            Rcast(rredbrus1.To3D());
                        }
                        if (teemo.Distance(rredbrush2) < _r.Range)
                        {
                            Rcast(rredbrush2.To3D());
                        }
                        if (teemo.Distance(rredbrush3) < _r.Range)
                        {
                            Rcast(rredbrush3.To3D());
                        }
                        if (teemo.Distance(rtoplanebrush) < _r.Range)
                        {
                            Rcast(rtoplanebrush.To3D());
                        }
                        if (teemo.Distance(rwolvesbrush) < _r.Range)
                        {
                            Rcast(rwolvesbrush.To3D());
                        }
                        if (teemo.Distance(rwraitshbrush) < _r.Range)
                        {
                            Rcast(rwraitshbrush.To3D());
                        }

                        //blue
                        if (teemo.Distance(bbasebrush) < _r.Range)
                        {
                            Rcast(bbasebrush.To3D());
                        }
                        if (teemo.Distance(bbluebrush) < _r.Range)
                        {
                            Rcast(bbluebrush.To3D());
                        }
                        if (teemo.Distance(bbotlanebrush) < _r.Range)
                        {
                            Rcast(bbotlanebrush.To3D());
                        }
                        if (teemo.Distance(bgrompbrush) < _r.Range)
                        {
                            Rcast(bgrompbrush.To3D());
                        }
                        if (teemo.Distance(bkrugsBrush) < _r.Range)
                        {
                            Rcast(bkrugsBrush.To3D());
                        }
                        if (teemo.Distance(bredbrus1) < _r.Range)
                        {
                            Rcast(bredbrus1.To3D());
                        }
                        if (teemo.Distance(bredbrush2) < _r.Range)
                        {
                            Rcast(bredbrush2.To3D());
                        }
                        if (teemo.Distance(bredbrush3) < _r.Range)
                        {
                            Rcast(bredbrush3.To3D());
                        }
                        if (teemo.Distance(btoplanebrush) < _r.Range)
                        {
                            Rcast(btoplanebrush.To3D());
                        }
                        if (teemo.Distance(bwolvesbrush) < _r.Range)
                        {
                            Rcast(bwolvesbrush.To3D());
                        }
                        if (teemo.Distance(bwraitshbrush) < _r.Range)
                        {
                            Rcast(bwraitshbrush.To3D());
                        }
                    }
                }
            }
        }

        #endregion

        #region Timing

        private static float EDotTime(Obj_AI_Base target)
        {
            if (target.HasBuff("toxicshotparticle"))
            {
                return Math.Max(0, target.GetBuff("toxicshotparticle").EndTime) - Game.Time;
            }
            return 0;
        }

        private static float RTime(Obj_AI_Base target)
        {
            if (target.HasBuff("bantamtrapslow"))
            {
                return Math.Max(0, target.GetBuff("bantamtrapslow").EndTime) - Game.Time;
            }
            return 0;
        }

        #endregion

        #region Damages

        private static float EDotdamageRaw(Obj_AI_Base target)
        {
            float dmg = 0;
            if (!target.HasBuff("toxicshotparticle") || !_e.IsLearned) return 0;

            if (_e.Level == 1)
            {
                dmg = 6 + .10f*Player.Instance.TotalMagicalDamage;
            }
            if (_e.Level == 2)
            {
                dmg = 12 + .10f*Player.Instance.TotalMagicalDamage;
            }
            if (_e.Level == 3)
            {
                dmg = 18 + .10f*Player.Instance.TotalMagicalDamage;
            }
            if (_e.Level == 4)
            {
                dmg = 24 + .10f*Player.Instance.TotalMagicalDamage;
            }
            if (_e.Level == 5)
            {
                dmg = 30 + .10f*Player.Instance.TotalMagicalDamage;
            }
            return dmg*EDotTime(target) - target.HPRegenRate;
        }

        private static float RdamageRaw(Obj_AI_Base target)
        {
            float dmg = 0;
            if (!target.HasBuff("bantamtrapslow")) return 0;

            if (_r.Level == 1)
            {
                dmg = 50 + .125f*Player.Instance.TotalMagicalDamage;
            }
            if (_r.Level == 2)
            {
                dmg = 81.25f + .125f*Player.Instance.TotalMagicalDamage;
            }
            if (_r.Level == 3)
            {
                dmg = 112.5f + .125f*Player.Instance.TotalMagicalDamage;
            }
            return dmg*RTime(target) - target.HPRegenRate;
        }

        private static float Rdamagecalc(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, RdamageRaw(target));
        }

        private static float EPassivedamagecalc(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, EDotdamageRaw(target));
        }

        private static float QDamage(Obj_AI_Base target)
        {
            if (_q.IsLearned)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {80, 125, 170, 215, 260}[_q.Level - 1] + .80f*Player.Instance.TotalMagicalDamage);
            }
            return 0f;
        }

        private static float EDamageonhit(Obj_AI_Base target)
        {
            if (_e.IsLearned)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {10, 20, 30, 40, 50}[_e.Level - 1] + .30f*Player.Instance.TotalMagicalDamage);
            }
            return 0f;
        }

        private static float ComboDamage(Obj_AI_Base target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target) + EDamageonhit(target);

            if (_q.IsReady())
            {
                damage += QDamage(target);
            }

            if (target.HasBuff("toxicshotparticle"))
            {
                damage += EPassivedamagecalc(target);
            }

            if (target.HasBuff("bantamtraptarget"))
            {
                damage += Rdamagecalc(target);
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

                if (Value.Use("draw.r.auto") && Game.MapId == GameMapId.SummonersRift)
                {
                    //top
                    var topTri = new Vector2(4467, 11853);
                    var topRiver = new Vector2(3099, 10826);
                    var topPixel = new Vector2(5229, 9150);
                    var topEntrance = new Vector2(3916, 12830);
                    var baron = new Vector2(4677, 10075);

                    //mid
                    var midbrushleft = new Vector2(6512, 8368);
                    var midbushright = new Vector2(8411, 6498);
                    var midleftbrush = new Vector2(5029, 8503);
                    var midrighbrush = new Vector2(9856, 6520);

                    //bot
                    var botPixel = new Vector2(9427, 5675);
                    var botTri = new Vector2(10392, 3094);
                    var botRiver = new Vector2(11791, 4108);
                    var botEntrance = new Vector2(10894, 1995);
                    var dragon = new Vector2(10169, 4838);

                    //red jungle
                    var rkrugsBrush = new Vector2(5643, 12789);
                    var rredbrus1 = new Vector2(7994, 11852);
                    var rredbrush2 = new Vector2(6731, 11463);
                    var rredbrush3 = new Vector2(6291, 10150);
                    var rbasebrush = new Vector2(9230, 11503);
                    var rwraitshbrush = new Vector2(8291, 10263);
                    var rwolvesbrush = new Vector2(9964, 7929);
                    var rbluebrush = new Vector2(11486, 7172);
                    var rgrompbrush = new Vector2(12493, 5223);
                    var rtoplanebrush = new Vector2(7160, 14120);
                    var rbotlanebrush = new Vector2(14108, 7031);

                    //blue jungle
                    var bkrugsBrush = new Vector2(9219, 2181);
                    var bredbrus1 = new Vector2(6848, 3120);
                    var bredbrush2 = new Vector2(8051, 3525);
                    var bredbrush3 = new Vector2(8543, 4848);
                    var bbasebrush = new Vector2(5609, 3509);
                    var bwraitshbrush = new Vector2(6545, 4711);
                    var bwolvesbrush = new Vector2(4820, 7149);
                    var bbluebrush = new Vector2(3384, 7787);
                    var bgrompbrush = new Vector2(2311, 9752);
                    var btoplanebrush = new Vector2(826, 8171);
                    var bbotlanebrush = new Vector2(7778, 831);

                    //top
                    new Circle(Color.Green, 70).Draw(topTri.To3D());
                    new Circle(Color.Green, 70).Draw(topPixel.To3D());
                    new Circle(Color.Green, 70).Draw(topRiver.To3D());
                    new Circle(Color.Green, 70).Draw(topEntrance.To3D());
                    new Circle(Color.Green, 70).Draw(baron.To3D());

                    //mid
                    new Circle(Color.Green, 70).Draw(midrighbrush.To3D());
                    new Circle(Color.Green, 70).Draw(midleftbrush.To3D());
                    new Circle(Color.Green, 70).Draw(midbrushleft.To3D());
                    new Circle(Color.Green, 70).Draw(midbushright.To3D());

                    //bot
                    new Circle(Color.Green, 70).Draw(botTri.To3D());
                    new Circle(Color.Green, 70).Draw(botPixel.To3D());
                    new Circle(Color.Green, 70).Draw(botRiver.To3D());
                    new Circle(Color.Green, 70).Draw(botEntrance.To3D());
                    new Circle(Color.Green, 70).Draw(dragon.To3D());

                    //red
                    new Circle(Color.Green, 70).Draw(rwolvesbrush.To3D());
                    new Circle(Color.Green, 70).Draw(rwraitshbrush.To3D());
                    new Circle(Color.Green, 70).Draw(rbasebrush.To3D());
                    new Circle(Color.Green, 70).Draw(rbluebrush.To3D());
                    new Circle(Color.Green, 70).Draw(rbotlanebrush.To3D());
                    new Circle(Color.Green, 70).Draw(rgrompbrush.To3D());
                    new Circle(Color.Green, 70).Draw(rkrugsBrush.To3D());
                    new Circle(Color.Green, 70).Draw(rredbrus1.To3D());
                    new Circle(Color.Green, 70).Draw(rredbrush2.To3D());
                    new Circle(Color.Green, 70).Draw(rredbrush3.To3D());
                    new Circle(Color.Green, 70).Draw(rtoplanebrush.To3D());

                    //blue
                    new Circle(Color.Green, 70).Draw(bwolvesbrush.To3D());
                    new Circle(Color.Green, 70).Draw(bwraitshbrush.To3D());
                    new Circle(Color.Green, 70).Draw(bbasebrush.To3D());
                    new Circle(Color.Green, 70).Draw(bbluebrush.To3D());
                    new Circle(Color.Green, 70).Draw(bbotlanebrush.To3D());
                    new Circle(Color.Green, 70).Draw(bgrompbrush.To3D());
                    new Circle(Color.Green, 70).Draw(bkrugsBrush.To3D());
                    new Circle(Color.Green, 70).Draw(bredbrus1.To3D());
                    new Circle(Color.Green, 70).Draw(bredbrush2.To3D());
                    new Circle(Color.Green, 70).Draw(bredbrush3.To3D());
                    new Circle(Color.Green, 70).Draw(btoplanebrush.To3D());
                }

                if (Value.Use("draw.status") && Game.MapId == GameMapId.SummonersRift)
                {
                    if (Value.Active("misc.r.auto"))
                    {
                        Drawing.DrawText(1342, 1019, System.Drawing.Color.Chartreuse, "Auto-Shroom Activated (G)", 20);
                    }
                    else
                    {
                        Drawing.DrawText(1342, 1019, System.Drawing.Color.Red, "Auto-Shroom Disabled (G)", 20);
                    }
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

                    Drawing.DrawLine(start, end, 9, System.Drawing.Color.Chartreuse);
                }
            }
        }

        #endregion
    }
}