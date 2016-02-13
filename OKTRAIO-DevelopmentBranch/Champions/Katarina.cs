using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Database.Spell_Library;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;
using Extensions = OKTRAIO.Utility.Extensions;
using MainMenu = OKTRAIO.Menu_Settings.MainMenu;
using Spell = EloBuddy.SDK.Spell;

namespace OKTRAIO.Champions
{
    internal class Katarina : AIOChampion
    {
        #region ComboMain

        public override void Combo()
        {
            try
            {
                var target = TargetSelector.GetTarget(1000, DamageType.Magical);
                if (target == null || !target.IsValid)
                    return;

                switch (Value.Get("combo.mode"))
                {
                        #region Mode QEWR

                    case 0:
                        if (_q.IsReady() && _q.IsInRange(target) && Value.Use("combo.q".AddName()))
                        {
                            Core.DelayAction(() => _q.Cast(target), new Random().Next(MinQDelay, MaxQDelay));
                        }
                        if (_e.IsReady() && _e.IsInRange(target) && Value.Use("combo.e".AddName()))
                        {
                            Core.DelayAction(() => _e.Cast(target), new Random().Next(MinWDelay, MaxWDelay));
                        }
                        if (_w.IsReady() && _w.IsInRange(target) && Value.Use("combo.w".AddName()))
                        {
                            Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                        }
                        if (_r.IsReady() && _r.IsInRange(target) && Value.Use("combo.r".AddName()))
                        {
                            Core.DelayAction(() => _r.Cast(), new Random().Next(MinRDelay, MaxRDelay));
                        }
                        break;

                        #endregion

                        #region Mode EQWR

                    case 1:
                        if (_e.IsReady() && _e.IsInRange(target) && Value.Use("combo.e".AddName()))
                        {
                            Core.DelayAction(() => _e.Cast(target), new Random().Next(MinWDelay, MaxWDelay));
                        }
                        if (_q.IsReady() && _q.IsInRange(target) && Value.Use("combo.q".AddName()))
                        {
                            Core.DelayAction(() => _q.Cast(target), new Random().Next(MinQDelay, MaxQDelay));
                        }
                        if (_w.IsReady() && _w.IsInRange(target) && Value.Use("combo.w".AddName()))
                        {
                            Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                        }
                        if (_r.IsReady() && _r.IsInRange(target) && Value.Use("combo.r".AddName()))
                        {
                            Core.DelayAction(() => _r.Cast(), new Random().Next(MinRDelay, MaxRDelay));
                        }
                        break;
                }

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code COMBO)</font>");
            }
        }

        #endregion

        #region HarassMain

        public override void Harass()
        {
            try
            {
                var target = TargetSelector.GetTarget(1000, DamageType.Magical);
                if (target == null || !target.IsValid)
                    return;


                if (_q.IsReady() && _q.IsInRange(target) && Value.Use("harass.q"))
                {
                    Core.DelayAction(() => _q.Cast(target), new Random().Next(MinQDelay, MaxQDelay));
                }
                if (_w.IsReady() && _w.IsInRange(target) && Value.Use("harass.w"))
                {
                    Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                }
                if (_e.IsReady() && _e.IsInRange(target) && Value.Use("harass.e"))
                {
                    if (Value.Use("harass.donteunderturret"))
                    {
                        if (!target.IsUnderEnemyturret())
                        {
                            Core.DelayAction(() => _e.Cast(target), new Random().Next(MinWDelay, MaxWDelay));
                        }
                    }
                    else
                    {
                        Core.DelayAction(() => _e.Cast(target), new Random().Next(MinWDelay, MaxWDelay));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code HARASS)</font>");
            }
        }

        #endregion

        #region LastHitMain

        public override void LastHit()
        {
            try
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions)
                {
                    if (minion == null || !minion.IsValid)
                        return;

                    #region Q

                    try
                    {
                        if (Prediction.Health.GetPrediction(minion, _q.CastDelay + Game.Ping/4) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.Q))
                        {
                            if (_q.IsInRange(minion) && _q.IsReady() && Value.Use("lasthit.q"))
                            {
                                Core.DelayAction(() => _q.Cast(minion), new Random().Next(MinQDelay, MaxQDelay));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Chat.Print(
                            "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LASTHIT.Q)</font>");
                    }

                    #endregion

                    #region W

                    try
                    {
                        if (Prediction.Health.GetPrediction(minion, _w.CastDelay + Game.Ping/4) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.W))
                        {
                            if (_w.IsInRange(minion) && _w.IsReady() && Value.Use("lasthit.w"))
                            {
                                Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Chat.Print(
                            "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LASTHIT.W)</font>");
                    }

                    #endregion

                    #region E

                    try
                    {
                        if (Prediction.Health.GetPrediction(minion, _e.CastDelay + Game.Ping/4) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.E))
                        {
                            if (_e.IsInRange(minion) && _e.IsReady() && Value.Use("lasthit.e"))
                            {
                                if (Value.Use("lasthit.donteunderturret"))
                                {
                                    if (!minion.IsUnderEnemyturret())
                                    {
                                        Core.DelayAction(() => _e.Cast(minion), new Random().Next(MinWDelay, MaxWDelay));
                                    }
                                }
                                else
                                {
                                    Core.DelayAction(() => _e.Cast(minion), new Random().Next(MinWDelay, MaxWDelay));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Chat.Print(
                            "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LASTHIT.W)</font>");
                    }

                    #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LASTHIT)</font>");
            }
        }

        #endregion

        #region FleeMain

        public override void Flee()
        {
            try
            {
                foreach (
                    var minion in
                        ObjectManager.Get<Obj_AI_Base>()
                            .Where(
                                o =>
                                    o.IsTargetable && o.IsValid && !o.IsDead && o.IsHPBarRendered &&
                                    (o.IsMinion || o.IsMonster || (o is AIHeroClient && !o.IsMe) || o.IsWard()))
                            .OrderBy(o => o.Distance(Game.CursorPos)))
                {
                    if (minion == null)
                    {
                        return;
                    }
                    if (Value.Use("flee.e") && _e.IsReady() && _e.IsInRange(minion))
                    {
                        if (minion.IsInRange(Game.CursorPos, 200))
                        {
                            Core.DelayAction(() => _e.Cast(minion), new Random().Next(MinWDelay, MaxWDelay));
                        }
                    }
                    else
                    {
                        try
                        {
                            if (Extensions.GetWardSlot() == null || !Extensions.GetWardSlot().IsWard)
                                return;

                            if (Value.Use("flee.ward") && Extensions.GetWardSlot().CanUseItem() && _e.IsReady() &&
                                Value.Use("flee.e"))
                            {
                                var pos = Player.Instance.Position.Extend(Game.CursorPos, 600);
                                Extensions.GetWardSlot().Cast(pos.To3D());
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Chat.Print(
                                "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code FLE_e.WARDJUMP)</font>");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code FLEE)</font>");
            }
        }

        #endregion

        #region LaneClearMain

        public override void Laneclear()
        {
            try
            {
                foreach (
                    var minion in
                        EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsInRange(Player.Instance, 1200)))
                {
                    if (minion == null || !minion.IsValid)
                        return;

                    if (_q.IsInRange(minion) && _q.IsReady() && Value.Use("lane.q"))
                    {
                        Core.DelayAction(() => _q.Cast(minion), new Random().Next(MinQDelay, MaxQDelay));
                    }

                    if (_w.IsInRange(minion) && _w.IsReady() && Value.Use("lane.w"))
                    {
                        Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                    }

                    if (_e.IsInRange(minion) && _e.IsReady() && Value.Use("lane.e"))
                    {
                        if (Value.Use("lane.donteunderturret"))
                        {
                            if (!minion.IsUnderEnemyturret())
                            {
                                Core.DelayAction(() => _e.Cast(minion), new Random().Next(MinWDelay, MaxWDelay));
                            }
                        }
                        else
                        {
                            Core.DelayAction(() => _e.Cast(minion), new Random().Next(MinWDelay, MaxWDelay));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LANECLEAR)</font>");
            }
        }

        #endregion

        #region KillStealMain

        private static void KillSteal()
        {
            try
            {
                var e = EntityManager.Heroes.Enemies.Where(ee => !ee.IsDead && ee.IsValid);

                foreach (var enemy in e)
                {
                    var damage = Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical,
                        GetActualRawComboDamage(enemy), true, true);
                    if (enemy.Health <= damage)
                    {
                        if (_q.IsReady() && _q.IsInRange(enemy) && Value.Use("killsteal.q"))
                        {
                            Core.DelayAction(() => _q.Cast(enemy), new Random().Next(MinQDelay, MaxQDelay));
                        }
                        if (_w.IsReady() && _w.IsInRange(enemy) && Value.Use("killsteal.w"))
                        {
                            Core.DelayAction(() => _w.Cast(), new Random().Next(MinEDelay, MaxEDelay));
                        }
                        if (_e.IsReady() && _e.IsInRange(enemy) && Value.Use("killsteal.e"))
                        {
                            if (Value.Use("killsteal.donteunderturret"))
                            {
                                if (!enemy.IsUnderEnemyturret())
                                {
                                    Core.DelayAction(() => _e.Cast(enemy), new Random().Next(MinWDelay, MaxWDelay));
                                }
                            }
                            else
                            {
                                Core.DelayAction(() => _e.Cast(enemy), new Random().Next(MinWDelay, MaxWDelay));
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

        #region Initialization

        #region SpellsDefine

        private static Spell.Targeted _q;
        private static Spell.Active _w;
        private static Spell.Targeted _e;
        private static Spell.Active _r;

        #endregion

        private static bool _isChannelingImportantSpell;
        private readonly InterrupterExtensions ext = new InterrupterExtensions();

        private static bool _isUlting;

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (args.Buff.Name.ToLower() == "katarinarsound" || args.Buff.Name.ToLower() == "katarinar" ||
                _isChannelingImportantSpell)
            {
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
                _isUlting = true;
            }
        }

        private static void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (args.Buff.Name.ToLower() == "katarinarsound" || args.Buff.Name.ToLower() == "katarinar")
            {
                Orbwalker.DisableMovement = false;
                Orbwalker.DisableAttacking = false;
                _isUlting = false;
            }
        }

        private static Menu _humanizerMenu;

        public override void Init()
        {
            try
            {
                try
                {
                    #region Spells

                    // Defining Spells
                    _q = new Spell.Targeted(SpellSlot.Q, 675);
                    _w = new Spell.Active(SpellSlot.W, 375);
                    _e = new Spell.Targeted(SpellSlot.E, 700);
                    _r = new Spell.Active(SpellSlot.R, 550);

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code SPELL)</font>");
                }

                try
                {
                    #region Menu

                    var combo = MainMenu.Combo;
                    string[] s = {"QEWR", "EQWR"};

                    combo.AddStringList("combo.mode", "Mode: ", s, 1);
                    MainMenu.ComboKeys();
                    MainMenu.HarassKeys();
                    MainMenu.Harass.Add("harass.autow", new CheckBox("Use Auto W"));
                    MainMenu.Harass.Add("harass.donteunderturret", new CheckBox("Dont E Under Turret"));

                    MainMenu.FleeKeys(false, useW: false, useR: false);
                    MainMenu.Flee.Add("flee.ward", new CheckBox("Use Wardjump"));

                    MainMenu.LaneKeys(useR: false);
                    MainMenu.Lane.Add("lane.donteunderturret", new CheckBox("Dont E Under Turret"));

                    MainMenu.LastHitKeys(useR: false);
                    MainMenu.Lasthit.Add("lasthit.donteunderturret", new CheckBox("Dont E Under Turret"));

                    MainMenu.KsKeys();
                    MainMenu.Ks.Add("killsteal.ignite", new CheckBox("Use Ignite"));
                    MainMenu.Ks.Add("killsteal.donteunderturret", new CheckBox("Dont E Under Turret"));

                    MainMenu.DamageIndicator();
                    MainMenu.DrawKeys();
                    MainMenu.Draw.AddSeparator();

                    MainMenu.Draw.AddGroupLabel("Flash Settings");
                    MainMenu.Draw.Add("draw.flash", new CheckBox("Draw flash"));
                    MainMenu.Draw.AddColorItem("color.flash");
                    MainMenu.Draw.AddWidthItem("width.flash");
                    MainMenu.Draw.AddSeparator();

                    MainMenu.Draw.AddGroupLabel("Ignite Settings");
                    MainMenu.Draw.Add("draw.ignite", new CheckBox("Draw ignite"));
                    MainMenu.Draw.AddColorItem("color.ignite");
                    MainMenu.Draw.AddWidthItem("width.ignite");

                    _humanizerMenu = MainMenu.Menu.AddSubMenu("Humanizer Menu");
                    _humanizerMenu.AddGroupLabel("Q Settings");
                    _humanizerMenu.Add("min.q", new Slider("Min Q Delay", 0, 0, 50));
                    _humanizerMenu.Add("max.q", new Slider("Max Q Delay", 0, 0, 50));
                    _humanizerMenu.AddSeparator(10);

                    _humanizerMenu.AddGroupLabel("W Settings");
                    _humanizerMenu.Add("min.w", new Slider("Min W Delay", 0, 0, 50));
                    _humanizerMenu.Add("max.w", new Slider("Max W Delay", 0, 0, 50));
                    _humanizerMenu.AddSeparator(10);

                    _humanizerMenu.AddGroupLabel("E Settings");
                    _humanizerMenu.Add("min.e", new Slider("Min E Delay", 0, 0, 50));
                    _humanizerMenu.Add("max.e", new Slider("Max E Delay", 0, 0, 50));
                    _humanizerMenu.AddSeparator(10);

                    _humanizerMenu.AddGroupLabel("R Settings");
                    _humanizerMenu.Add("min.r", new Slider("Min R Delay", 4, 0, 50));
                    _humanizerMenu.Add("max.r", new Slider("Max R Delay", 4, 0, 50));
                    _humanizerMenu.AddSeparator(10);

                    #endregion
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code MENU)</font>");
                }

                #region UtilityInit

                Obj_AI_Base.OnBuffGain += OnBuffGain;
                Obj_AI_Base.OnBuffLose += OnBuffLose;
                DamageIndicator.DamageToUnit = GetActualRawComboDamage;
                Value.Init();
                Value.MenuList.Add(_humanizerMenu);
                Drawing.OnDraw += DrawRanges;

                #region MenuValueChange

                _humanizerMenu["min.q"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["min.q"].Cast<Slider>().CurrentValue >
                        _humanizerMenu["max.q"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["min.q"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["max.q"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["max.q"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["max.q"].Cast<Slider>().CurrentValue <
                        _humanizerMenu["min.q"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["max.q"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["min.q"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["min.w"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["min.w"].Cast<Slider>().CurrentValue >
                        _humanizerMenu["max.w"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["min.w"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["max.w"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["max.w"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["max.w"].Cast<Slider>().CurrentValue <
                        _humanizerMenu["min.w"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["max.w"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["min.w"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["min.e"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["min.e"].Cast<Slider>().CurrentValue >
                        _humanizerMenu["max.e"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["min.e"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["max.e"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["max.e"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["max.e"].Cast<Slider>().CurrentValue <
                        _humanizerMenu["min.e"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["max.e"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["min.e"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["min.r"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["min.r"].Cast<Slider>().CurrentValue >
                        _humanizerMenu["max.r"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["min.r"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["max.r"].Cast<Slider>().CurrentValue;
                };
                _humanizerMenu["max.r"].Cast<Slider>().OnValueChange += delegate
                {
                    if (_humanizerMenu["max.r"].Cast<Slider>().CurrentValue <
                        _humanizerMenu["min.r"].Cast<Slider>().CurrentValue)
                        _humanizerMenu["max.r"].Cast<Slider>().CurrentValue =
                            _humanizerMenu["min.r"].Cast<Slider>().CurrentValue;
                };

                #endregion

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 503)</font>");
            }

            Game.OnUpdate += delegate
            {
                try
                {
                    #region AutoW

                    if (_isUlting) return;
                    if (MainMenu.Harass["harass.autow"].Cast<CheckBox>().CurrentValue)
                    {
                        var e = EntityManager.Heroes.Enemies.Where(ee => !ee.IsDead && ee.IsValid);
                        foreach (var enemy in e)
                        {
                            if (_w.IsInRange(enemy) && _w.IsReady())
                            {
                                _w.Cast();
                            }
                        }
                    }

                    #endregion

                    _isChannelingImportantSpell = ext.IsChannelingImportantSpell(Player.Instance);
                    KillSteal();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> san error ocurred. (Code 5)</font>");
                }
                //KillSteal();
            };
        }

        #endregion

        #region _humanizerMenuIndex

        private static int MinQDelay
        {
            get { return _humanizerMenu["min.q"].Cast<Slider>().CurrentValue; }
        }

        private static int MaxQDelay
        {
            get { return _humanizerMenu["max.q"].Cast<Slider>().CurrentValue; }
        }

        private static int MinWDelay
        {
            get { return _humanizerMenu["min.w"].Cast<Slider>().CurrentValue; }
        }

        private static int MaxWDelay
        {
            get { return _humanizerMenu["max.w"].Cast<Slider>().CurrentValue; }
        }

        private static int MinEDelay
        {
            get { return _humanizerMenu["min.e"].Cast<Slider>().CurrentValue; }
        }

        private static int MaxEDelay
        {
            get { return _humanizerMenu["max.e"].Cast<Slider>().CurrentValue; }
        }

        private static int MinRDelay
        {
            get { return _humanizerMenu["min.e"].Cast<Slider>().CurrentValue; }
        }

        private static int MaxRDelay
        {
            get { return _humanizerMenu["max.r"].Cast<Slider>().CurrentValue; }
        }

        #endregion

        #region Damages

        #region BaseDamages

        private static readonly float[] QDamage = {0, 60, 85, 110, 135, 160};
        private static readonly float[] BonusQDamage = {0, 15, 30, 45, 60, 75};
        private static readonly float[] WDamage = {0, 40, 75, 110, 145, 180};
        private static readonly float[] EDamage = {0, 40, 70, 100, 130, 160};
        private static readonly float[] RDamage = {0, 350, 550, 750};

        #endregion

        #region GetSpellDamage

        private static float GetSpellDamage(SpellSlot slot)
        {
            try
            {
                var qbasedamage = QDamage[_q.Level];
                var wbasedamage = WDamage[_w.Level];
                var ebasedamage = EDamage[_e.Level];
                var rbasedamage = RDamage[_r.Level];

                var qbonusdamage = 45f/100f*Player.Instance.FlatMagicDamageMod;
                var wbonusdamage = 25f/100f*Player.Instance.FlatMagicDamageMod;
                var ebonusdamage = 25f/100f*Player.Instance.FlatMagicDamageMod;
                var rbonusdamage = 25f/100f*Player.Instance.FlatMagicDamageMod;

                if (slot == SpellSlot.Q)
                    return qbasedamage + qbonusdamage +
                           (BonusQDamage[_q.Level] + 15f/100f*Player.Instance.FlatMagicDamageMod);
                if (slot == SpellSlot.W)
                    return wbasedamage + wbonusdamage + 60f/100f*Player.Instance.FlatPhysicalDamageMod;
                if (slot == SpellSlot.E)
                    return ebasedamage + ebonusdamage;
                if (slot == SpellSlot.R)
                    return rbasedamage + rbonusdamage + 375f/1000f*Player.Instance.FlatPhysicalDamageMod;

                //if (raw)
                //return Player.Instance.CalculateDamageOnUnit(target, DamageTyp_e.Magical, damage, true, true);
                return 0f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETSPELLDAMAGE)</font>");
                return 0f;
            }
        }

        #endregion

        #region RawComboDamage

        private static float GetActualRawComboDamage(Obj_AI_Base enemy)
        {
            try
            {
                var damage = 0f;

                var spells = new List<SpellSlot>();
                spells.Add(SpellSlot.Q);
                spells.Add(SpellSlot.W);
                spells.Add(SpellSlot.E);
                spells.Add(SpellSlot.R);
                foreach (
                    var spell in
                        spells.Where(
                            s => Player.Instance.Spellbook.CanUseSpell(s) == SpellState.Ready && s != SpellSlot.R))
                {
                    if (Player.Instance.Spellbook.CanUseSpell(spell) == SpellState.Ready)
                        damage += GetSpellDamage(spell);
                }
                if (Player.Instance.Spellbook.CanUseSpell(GetIgniteSpellSlot()) != SpellState.Cooldown &&
                    Player.Instance.Spellbook.CanUseSpell(GetIgniteSpellSlot()) != SpellState.NotLearned &&
                    Player.Instance.Spellbook.CanUseSpell(GetIgniteSpellSlot()) == SpellState.Ready &&
                    GetIgniteSpellSlot() != SpellSlot.Unknown)
                    damage += Player.Instance.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite);
                if (Player.Instance.Spellbook.CanUseSpell(SpellSlot.R) != SpellState.Cooldown &&
                    Player.Instance.Spellbook.CanUseSpell(SpellSlot.R) != SpellState.NotLearned)
                    damage += GetSpellDamage(SpellSlot.R);
                return damage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETACTUALRAWCOMBODAMAGE)</font>");
                return 0f;
            }
        }

        #endregion

        #region SummonersRanges

        private static readonly float flashrange = 425;
        private static readonly float igniterange = 600;

        #endregion

        private static void DrawRanges(EventArgs args)
        {
            try
            {
                if (Value.Use("draw.disable"))
                    return;
                try
                {
                    #region Q

                    if (Value.Use("draw.q"))
                    {
                        if (Value.Use("draw.ready"))
                        {
                            if (_q.IsReady())
                            {
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                    Color = MainMenu.Draw.GetColor("color.q"),
                                    Radius = _q.Range
                                }.Draw(Player.Instance.Position);
                            }
                        }
                        else
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                Color = MainMenu.Draw.GetColor("color.q"),
                                Radius = _q.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW.Q</font>");
                }

                try
                {
                    #region W

                    if (Value.Use("draw.w"))
                    {
                        if (Value.Use("draw.ready"))
                        {
                            if (_w.IsReady())
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.w"),
                                    Color = MainMenu.Draw.GetColor("color.w"),
                                    Radius = _w.Range
                                }.Draw(Player.Instance.Position);
                        }
                        else
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.w"),
                                Color = MainMenu.Draw.GetColor("color.w"),
                                Radius = _w.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW.W</font>");
                }

                try
                {
                    #region E

                    if (Value.Use("draw.e"))
                    {
                        if (Value.Use("draw.ready"))
                        {
                            if (_e.IsReady())
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                    Color = MainMenu.Draw.GetColor("color.e"),
                                    Radius = _e.Range
                                }.Draw(Player.Instance.Position);
                        }
                        else
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                Color = MainMenu.Draw.GetColor("color.e"),
                                Radius = _e.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW.E</font>");
                }

                try
                {
                    #region R

                    if (Value.Use("draw.r"))
                    {
                        if (Value.Use("draw.ready"))
                        {
                            if (!_r.IsOnCooldown)
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.r"),
                                    Color = MainMenu.Draw.GetColor("color.r"),
                                    Radius = _r.Range
                                }.Draw(Player.Instance.Position);
                        }
                        else
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.r"),
                                Color = MainMenu.Draw.GetColor("color.r"),
                                Radius = _r.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW.R)</font>");
                }

                #region Summoners

                try
                {
                    #region Flash

                    if (Value.Use("draw.flash"))
                    {
                        if (Player.CanUseSpell(GetFlashSpellSlot()) == SpellState.Ready)
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.flash"),
                                Color = MainMenu.Draw.GetColor("color.flash"),
                                Radius = flashrange
                            }.Draw(Player.Instance.Position);
                        if (Player.CanUseSpell(GetFlashSpellSlot()) == SpellState.Cooldown)
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.flash"),
                                Color = MainMenu.Draw.GetColor("color.flash"),
                                Radius = flashrange
                            }.Draw(Player.Instance.Position);
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW)</font>");
                }

                try
                {
                    #region Ignite

                    if (Value.Use("draw.ignite"))
                    {
                        if (Player.CanUseSpell(GetIgniteSpellSlot()) == SpellState.Ready)
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("color.flash"),
                                Color = MainMenu.Draw.GetColor("color.ignite"),
                                Radius = igniterange
                            }.Draw(Player.Instance.Position);
                        if (Player.CanUseSpell(GetIgniteSpellSlot()) == SpellState.Cooldown)
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("color.flash"),
                                Color = MainMenu.Draw.GetColor("color.ignite"),
                                Radius = igniterange
                            }.Draw(Player.Instance.Position);
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAW)</font>");
                }

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (DRAW)</font>");
            }
        }

        #endregion

        #region GetSpellSlots

        private static SpellSlot GetFlashSpellSlot()
        {
            try
            {
                if (Player.GetSpell(SpellSlot.Summoner1).Name == "summonerflash")
                    return SpellSlot.Summoner1;
                if (Player.GetSpell(SpellSlot.Summoner2).Name == "summonerflash")
                    return SpellSlot.Summoner2;
                return SpellSlot.Unknown;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETFLASHSPELLSLOT)</font>");
                return SpellSlot.Unknown;
            }
        }

        private static SpellSlot GetIgniteSpellSlot()
        {
            try
            {
                if (Player.GetSpell(SpellSlot.Summoner1).Name.ToLower() == "summonerignite")
                    return SpellSlot.Summoner1;
                if (Player.GetSpell(SpellSlot.Summoner2).Name.ToLower() == "summonerignite")
                    return SpellSlot.Summoner2;
                return SpellSlot.Unknown;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETIGNITESPELLSLOT)</font>");
                return SpellSlot.Unknown;
            }
        }

        #endregion
    }

    #region Misc

    internal static class KataExtensions
    {
        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values,
            int defaultValue)
        {
            try
            {
                var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
                mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
                mode.OnValueChange +=
                    delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                    {
                        sender.DisplayName = displayName + ": " + values[args.NewValue];
                    };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code ADDSTRINGLIST)</font>");
            }
        }
    }

    #endregion
}