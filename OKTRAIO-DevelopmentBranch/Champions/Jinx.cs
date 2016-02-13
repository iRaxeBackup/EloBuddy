using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;
using SharpDX;
using Color = System.Drawing.Color;

namespace OKTRAIO.Champions
{
    internal class Jinx : AIOChampion
    {
        private static Spell.Active _q;
        private static Spell.Skillshot _w;
        private static Spell.Skillshot _e;
        private static Spell.Skillshot _r;

        private static bool MissleActive
        {
            get { return Player.HasBuff("JinxQ"); }
        }

        private static float MissleRange
        {
            get { return 670f + Player.Instance.BoundingRadius + new[] {75, 100, 125, 150, 175}[_q.Level - 1]; }
        }

        private static float NormalRange(GameObject target = null)
        {
            return 650f + Player.Instance.BoundingRadius + (target == null ? 0f : target.BoundingRadius);
        }

        private static float GetBoundingDistance(Obj_AI_Base target)
        {
            return Player.Instance.ServerPosition.Distance(target.ServerPosition) + Player.Instance.BoundingRadius +
                   target.BoundingRadius;
        }

        private static bool EnoughMana(bool countE = false, float extra = 0f)
        {
            var qMana = 20f;
            var wMana = _w.IsReady() ? new float[] {50, 60, 70, 80, 90}[_w.Level - 1] : 0f;
            var eMana = countE ? 50f : 0f;
            var rMana = _r.IsReady() ? 100f : 0f;

            return Player.Instance.Mana > qMana + wMana + eMana + rMana + extra;
        }

        public override void Init()
        {
            try
            {
                //Create spells
                _q = new Spell.Active(SpellSlot.Q);
                _w = new Spell.Skillshot(SpellSlot.W, 1500, SkillShotType.Linear, 600, 3300, 60)
                {
                    MinimumHitChance = HitChance.Medium,
                    AllowedCollisionCount = 0
                };
                _e = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 1200, 1750, 100);
                _r = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 600, 1700, 140)
                {
                    MinimumHitChance = HitChance.Medium
                };

                try
                {
                    #region Create menu

                    //Combo Menu Settings
                    MainMenu.ComboKeys();
                    MainMenu.ComboManaManager(false, true, false, false, 0, 40, 0, 0);

                    //Lane Clear Menu Settings
                    MainMenu.LaneKeys(useW: false, useE: false, useR: false);
                    MainMenu.Lane.Add("lane.mana", new Slider("Minimum {0}% mana to laneclear with Q", 80));

                    //Jungle Clear Menu Settings
                    MainMenu.JungleKeys(useE: false, useR: false);
                    MainMenu.JungleManaManager(false, true, false, false, 0, 40, 0, 0);

                    //Harras Menu Settings
                    MainMenu.HarassKeys(useR: false);
                    MainMenu.HarassManaManager(false, true, false, false, 0, 40, 0, 0);

                    //Killsteal Menu
                    MainMenu.KsKeys(false, useW: false, useE: false);


                    //Misc Menu
                    MainMenu.MiscMenu();
                    MainMenu.Misc.Add("misc.farmQAARange", new CheckBox("Use Q when minion out of AA range"));
                    MainMenu.Misc.Add("misc.teleportE", new CheckBox("Use E on teleport position"));
                    MainMenu.Misc.Add("misc.spellcastE", new CheckBox("Use E OnProcessSpellCast"));
                    MainMenu.Misc.Add("misc.enemyTurretR", new CheckBox("Don't use R under enemy turret"));

                    //Draw Menu
                    MainMenu.DrawKeys(false, useR: false);
                    MainMenu.DamageIndicator(false, "Ultimate (R) damage");

                    Value.Init();

                    #endregion
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
                DamageIndicator.DamageToUnit = DamageInidicatorDmg;
                Game.OnTick += OnTick;
                Drawing.OnDraw += OnDraw;
                Orbwalker.OnPreAttack += OnPreAttack;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
                Chat.Print("Jinx Loaded!", Color.Chartreuse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code INIT)</font>");
            }

            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                Recalls.Add(new Recall(hero, RecallStatus.Inactive));
            }
        }

        #region Drawings

        private static void OnDraw(EventArgs args)
        {
            if (Value.Use("draw.disable") || Player.Instance.IsDead) return;

            if (Value.Use("draw.w"))
            {
                if (!(Value.Use("draw.ready") && !_w.IsReady()))
                {
                    new Circle
                    {
                        Radius = _w.Range,
                        Color = MainMenu.Draw.GetColor("color.w"),
                        BorderWidth = MainMenu.Draw.GetWidth("width.w")
                    }.Draw(Player.Instance.Position);
                }
            }

            if (Value.Use("draw.e"))
            {
                if (!(Value.Use("draw.ready") && !_e.IsReady()))
                {
                    new Circle
                    {
                        Radius = _e.Range,
                        Color = MainMenu.Draw.GetColor("color.e"),
                        BorderWidth = MainMenu.Draw.GetWidth("width.e")
                    }.Draw(Player.Instance.Position);
                }
            }
        }

        #endregion

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead) return;

            PermaActive();
        }

        public override void Combo()
        {
            if (Value.Use("combo.q".AddName()) && _q.IsReady())
            {
                var qTarget = GetValidMissleTarget();
                if (qTarget != null && !MissleActive)
                {
                    if (EnoughMana()
                        ||
                        (Player.Instance.GetAutoAttackDamage(qTarget)*3 > qTarget.TotalShieldHealth() &&
                         Player.Instance.Mana > 60))
                    {
                        _q.Cast();
                    }
                }
                else if (MissleActive && !EnoughMana())
                {
                    _q.Cast();
                }
                else if (MissleActive && Player.Instance.CountEnemiesInRange(2000) == 0)
                {
                    _q.Cast();
                }
            }

            if (Value.Use("combo.w".AddName()) && Player.Instance.ManaPercent > Value.Get("combo.w.mana"))
            {
                WLogic();
            }

            if (Value.Use("combo.e".AddName())
                && _e.IsReady()
                && EnoughMana())
            {
                ELogic();

                if (Player.Instance.IsMoving)
                {
                    var target = TargetSelector.GetTarget(_e.Range, DamageType.Physical);
                    if (target != null && target.IsValidTarget(_e.Range))
                    {
                        var ePrediction = _e.GetPrediction(target);
                        var predictionDistance = ePrediction.CastPosition.Distance(target.Position);

                        if (predictionDistance > 200
                            && ePrediction.HitChance >= HitChance.High)
                        {
                            if (target.HasBuffOfType(BuffType.Slow))
                            {
                                _e.Cast(ePrediction.CastPosition);
                            }
                        }
                    }
                }
            }
            if (!Value.Use("killsteal.r"))
            {
                if (Value.Use("combo.r".AddName()))
                {
                    RLogic();
                }
            }
        }

        public override void Harass()
        {
            if (Value.Use("harass.q"))
            {
                QFarmMode();
            }
            if (Value.Use("harass.w") && Player.Instance.ManaPercent > Value.Get("harass.w.mana"))
            {
                WLogic();
            }
            if (Value.Use("harass.e"))
            {
                ELogic();
            }
        }

        public override void Laneclear()
        {
            if (Value.Use("lane.q"))
            {
                QFarmMode();
            }
        }

        public override void LastHit()
        {
            if (MissleActive)
            {
                _q.Cast();
            }
        }

        public override void Jungleclear()
        {
            if (Value.Use("jungle.w") && Player.Instance.ManaPercent > Value.Get("jungle.w.mana"))
            {
                if (!Orbwalker.IsAutoAttacking) return;

                var monsters =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,
                        _q.Range)
                        .FirstOrDefault(x => x.IsValidTarget(_q.Range));

                if (monsters != null)
                {
                    _w.Cast(monsters);
                }
            }
        }

        private static void PermaActive()
        {
            if (Value.Use("killsteal.r"))
            {
                RLogic();
            }
        }

        private static void QFarmMode()
        {
            if (!_q.IsReady()) return;

            if (!MissleActive
                && EnoughMana()
                && !Orbwalker.IsAutoAttacking
                && Orbwalker.CanAutoAttack
                && Value.Use("misc.farmQAARange"))
            {
                var qMissleKillableMinion = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, Player.Instance.Position, MissleRange)
                    .FirstOrDefault(
                        minion =>
                            !Player.Instance.IsInAutoAttackRange(minion)
                            && minion.Health < Player.Instance.GetAutoAttackDamage(minion)*1.2
                            && MissleRange > GetBoundingDistance(minion));

                if (qMissleKillableMinion != null)
                {
                    Orbwalker.ForcedTarget = qMissleKillableMinion;
                    _q.Cast();
                    return;
                }
            }

            var qTarget = GetValidMissleTarget();
            if (qTarget != null && !MissleActive)
            {
                var distance = GetBoundingDistance(qTarget);
                if (Value.Use("harass.q")
                    && !Orbwalker.IsAutoAttacking
                    && Orbwalker.CanAutoAttack
                    && !Player.Instance.IsUnderEnemyturret()
                    && EnoughMana()
                    && MissleRange + qTarget.BoundingRadius + Player.Instance.BoundingRadius > distance)
                {
                    _q.Cast();
                }
            }

            else if (MissleActive)
            {
                _q.Cast();
            }
        }

        private static AIHeroClient GetValidMissleTarget()
        {
            if (!_q.IsReady()) return null;

            var enemy = TargetSelector.GetTarget(MissleRange + 60, DamageType.Physical);
            if (enemy == null || !enemy.IsValidTarget()) return null;

            if (!MissleActive && (!Player.Instance.IsInAutoAttackRange(enemy) || enemy.CountEnemiesInRange(250) > 2) &&
                TargetSelector.GetTarget(_q.Range, DamageType.Physical) == null)
            {
                return enemy;
            }

            return null;
        }

        private static void WLogic()
        {
            if (!_w.IsReady()) return;

            var target = TargetSelector.SelectedTarget != null && _w.IsInRange(TargetSelector.SelectedTarget)
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(_w.Range, DamageType.Physical);

            if (target != null && target.IsValidTarget() && Player.Instance.Distance(target) > 400)
            {
                _w.Cast(target);
            }
        }

        private static void ELogic()
        {
            if (!_e.IsReady() || !EnoughMana()) return;

            var enemyE = EntityManager.Heroes.Enemies.FirstOrDefault(x => x.IsValidTarget(_e.Range) && !x.CanMove);
            if (enemyE != null)
            {
                _e.Cast(enemyE.Position);
                return;
            }

            if (Value.Use("misc.teleportE"))
            {
                var teleportE = ObjectManager.Get<Obj_AI_Base>()
                    .FirstOrDefault(
                        x =>
                            x.IsEnemy && _e.IsInRange(x) &&
                            (x.HasBuff("teleport_target") || x.HasBuff("Pantheon_GrandSkyfall_Jump")));

                if (teleportE != null)
                {
                    _e.Cast(teleportE.Position);
                }
            }
        }

        private static void RLogic()
        {
            if ((Player.Instance.IsUnderEnemyturret() && Value.Use("misc.enemyTurretR"))
                || !_r.IsReady()) return;

            var rTarget = EntityManager.Heroes.Enemies.FirstOrDefault(
                x =>
                    x.IsEnemy
                    && ValidTarget(x)
                    && RDamage(x) > x.TotalShieldHealth()
                    && !x.IsDead
                    && !Player.Instance.IsInAutoAttackRange(x));

            if (rTarget != null && _r.IsReady())
            {
                var pred = _r.GetPrediction(rTarget);
                _r.Cast(pred.CastPosition);
            }
        }

        public static bool ValidTarget(Obj_AI_Base target)
        {
            if (target.HasBuffOfType(BuffType.PhysicalImmunity)
                || target.HasBuffOfType(BuffType.SpellImmunity)
                || target.IsZombie
                || target.IsInvulnerable
                || target.HasBuffOfType(BuffType.Invulnerability)
                || target.HasBuffOfType(BuffType.SpellShield))
                return false;
            return true;
        }

        public static bool ShouldUseE(string SpellName)
        {
            switch (SpellName)
            {
                case "ThreshQ":
                    return true;
                case "KatarinaR":
                    return true;
                case "AlZaharNetherGrasp":
                    return true;
                case "GalioIdolOfDurand":
                    return true;
                case "LuxMaliceCannon":
                    return true;
                case "MissFortuneBulletTime":
                    return true;
                case "RocketGrabMissile":
                    return true;
                case "CaitlynPiltoverPeacemaker":
                    return true;
                case "EzrealTrueshotBarrage":
                    return true;
                case "InfiniteDuress":
                    return true;
                case "VelkozR":
                    return true;
            }
            return false;
        }

        private static void OnPreAttack(AttackableUnit unit, Orbwalker.PreAttackArgs args)
        {
            if (!_q.IsReady()) return;

            var target = args.Target as AIHeroClient;

            if (MissleActive && target != null && target.IsValidTarget())
            {
                var distance = GetBoundingDistance(target);

                if (Value.Use("combo.q".AddName())
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && NormalRange(target) > distance
                    && target.IsValidTarget())
                {
                    _q.Cast();
                }

                else if (Value.Use("lane.q")
                         && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                         && (distance > MissleRange || distance < NormalRange(target) || !EnoughMana(true)))
                {
                    _q.Cast();
                }

                else if (Value.Use("harass.q")
                         && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                         && (distance > MissleRange || distance < NormalRange(target) || !EnoughMana(true)))
                {
                    _q.Cast();
                }
            }

            else if (Value.Use("lane.q")
                     && !MissleActive
                     && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                     && Player.Instance.ManaPercent > Value.Get("lane.mana")
                     && EnoughMana())
            {
                var qMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, Player.Instance.Position, MissleRange)
                    .FirstOrDefault(
                        x =>
                            args.Target.NetworkId != x.NetworkId
                            && x.Distance(args.Target.Position) < 200
                            && (5 - _q.Level)*Player.Instance.GetAutoAttackDamage(x) < args.Target.Health
                            && (5 - _q.Level)*Player.Instance.GetAutoAttackDamage(x) < x.Health);

                if (qMinions != null)
                {
                    _q.Cast();
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {
            if (unit.IsMinion || !_e.IsReady()) return;

            if (unit.IsEnemy && Value.Use("misc.spellcastE") && unit.IsValidTarget(_e.Range) &&
                ShouldUseE(args.SData.Name))
            {
                _e.Cast(unit.ServerPosition);
            }
        }

        public static float DamageInidicatorDmg(Obj_AI_Base target)
        {
            var damage = 0f;
            if (!_r.IsReady()) return damage;

            damage += RDamage(target);

            if (Player.Instance.HasBuff("summonerexhaust"))
            {
                damage *= 0.6f;
            }

            if (target.HasBuff("FerociousHowl"))
            {
                damage *= 0.7f;
            }

            return damage;
        }

        private static float WDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(
                target, DamageType.Physical, new float[] {10, 60, 110, 160, 210}[_w.Level - 1]
                                             + Player.Instance.TotalAttackDamage*1.4f);
        }

        private static float EDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(
                target, DamageType.Magical, new float[] {80, 135, 190, 245, 300}[_e.Level - 1]
                                            + Player.Instance.TotalMagicalDamage*1.0f);
        }

        public static float RDamage(Obj_AI_Base target)
        {
            if (!_r.IsReady()) return 0f;

            var distance = target.Distance(Player.Instance);
            var percentage = 0.1f + (distance < 100f ? 0f : distance/100*0.06f);
            var dmgPercentage = percentage > 1f ? 1f : percentage;

            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                new float[] {250, 350, 450}[_r.Level - 1]*dmgPercentage +
                new float[] {25, 30, 35}[_r.Level - 1]/100*(target.MaxHealth - target.Health) +
                dmgPercentage*Player.Instance.FlatPhysicalDamageMod);
        }

        #region ult speed

        private static readonly List<Recall> Recalls = new List<Recall>();

        private static float UltTime(Vector3 location, float delay, float speed)
        {
            var distance = Vector3.Distance(Player.Instance.ServerPosition, location);
            var missilespeed = speed;

            if (distance > 1350)
            {
                const float accelerationrate = 0.3f;
                var acceldifference = distance - 1350f;
                if (acceldifference > 150f) acceldifference = 150f;
                var difference = distance - 1500f;
                missilespeed = (1350f*speed + acceldifference*(speed + accelerationrate*acceldifference) +
                                difference*2200f)/distance;
            }

            return distance/missilespeed + _r.CastDelay/1000f;
        }

        #endregion
    }
}