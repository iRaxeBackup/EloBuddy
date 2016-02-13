using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;
using SharpDX;
using Color = System.Drawing.Color;

namespace OKTRAIO.Champions
{
    internal class Vayne : AIOChampion
    {
        private void Draw(EventArgs args)
        {
        }

        #region Initialize and Declare

        private static Spell.Skillshot _q;
        private static Spell.Targeted _e;
        private static int _randomizerOne, _randomizerTwo;
        private static AttackableUnit _target;

        public override void Init()
        {
            _q = new Spell.Skillshot(SpellSlot.Q, 300, SkillShotType.Linear);
            _e = new Spell.Targeted(SpellSlot.E, 550);

            MainMenu.ComboKeys(useW: false, useR: false);
            MainMenu.Combo.AddGroupLabel("Combo Preferences", "combo.gl.pref", true);

            MainMenu.HarassKeys(useW: false, useR: false);
            MainMenu.Harass.AddGroupLabel("Harass Preferences", "harass.gl.pref", true);

            MainMenu.JungleKeys(useW: false, useR: false);
            MainMenu.Jungle.AddSlider("jungle.mana", "ManaSlider", 80, 0, 100, true);

            MainMenu.LaneKeys(useW: false, useR: false);
            MainMenu.Lane.AddGroupLabel("LaneClear Preferences", "lane.gl.pref", true);
            MainMenu.Lane.AddSlider("lane.mana", "ManaSlider", 80, 0, 100, true);

            MainMenu.KsKeys(useW: false, useR: false);

            MainMenu.DrawKeys(useW: false, useE: false, useR: false);
            MainMenu.Draw.AddSeparator();
            MainMenu.DamageIndicator(true);

            MainMenu.FleeKeys(useW: false, useR: false);

            Value.Init();

            _randomizerOne = 5;
            _randomizerTwo = 12;
            DamageIndicator.DamageToUnit += GetRawDamage;
            Drawing.OnDraw += Draw;
            if (MainMenu.Menu["useonupdate"].Cast<CheckBox>().CurrentValue)
            {
                Game.OnUpdate += Game_OnTick;
            }
            else
            {
                Game.OnTick += Game_OnTick;
            }
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Orbwalker.OnPostAttack += OrbwalkerOnPostAttack;
        }

        #endregion

        #region Gamerelated Logic

        public override void Combo()
        {
            if (!_e.IsReady() || !Value.Use("combo.e".AddName())) return;
            foreach (var enemy in Variables.CloseEnemies(_e.Range))
            {
                CondemnLogic(enemy);
            }
        }

        public override void Harass()
        {
            if (!_e.IsReady() || !Value.Use("harass.e")) return;
            foreach (var enemy in Variables.CloseEnemies(_e.Range))
            {
                CondemnLogic(enemy);
            }
        }

        public override void Laneclear()
        {
            //no need
        }

        public override void Jungleclear()
        {
            //no need
        }

        public override void Flee()
        {
            if (Value.Use("flee.q") && _q.IsReady())
            {
                _q.Cast(Player.Instance.Position.Extend(Game.CursorPos, _q.Range).To3D());
            }
            else if (Value.Use("flee.e") && _e.IsReady())
            {
                if (EntityManager.Heroes.Enemies.Any(e => e.IsValidTarget(_e.Range)))
                    _e.Cast(EntityManager.Heroes.Enemies.OrderBy(e => e.Distance(Player.Instance)).First());
            }
        }

        public override void LastHit()
        {
            //no need
        }

        private static void OrbwalkerOnPostAttack(AttackableUnit target, EventArgs args)
        {
            _target = target;
            Core.DelayAction(QLogic, GetSpellDelay);
        }

        private static void QLogic()
        {
            if (Value.Mode(Orbwalker.ActiveModes.Combo))
            {
                if (_q.IsReady() && Value.Use("combo.q".AddName()))
                {
                    _q.Cast(OKTRGeometry.SafeDashPosRework(_q.Range, TargetSelector.GetTarget(_q.Range + Player.Instance.GetAutoAttackRange() + 50, DamageType.Physical), 119));
                }
                /* for nobs
                var qPos = OKTRGeometry.SafeDashPos(_q.Range);
                if (qPos != Vector3.Zero)
                    _q.Cast(qPos);
                else
                {
                    if (Game.CursorPos.Distance(Player.Instance.Position) >
                    Player.Instance.AttackRange + Player.Instance.BoundingRadius * 2 &&
                    !Player.Instance.Position.Extend(Game.CursorPos, _e.Range).IsUnderTurret())
                    {
                        _q.Cast(Player.Instance.Position.Extend(Game.CursorPos, _e.Range).To3D());
                    }
                    else
                    {
                        if (InsideCone())
                        {
                            _q.Cast(
                                OKTRGeometry.Deviation(Player.Instance.Position.To2D(), _target.Position.To2D(), -65)
                                    .To3D());
                        }
                        else
                        {
                            _q.Cast(
                                OKTRGeometry.Deviation(Player.Instance.Position.To2D(), _target.Position.To2D(), 65)
                                    .To3D());
                        }
                    }
                }*/
            }
            List<Obj_AI_Minion> targets;
            var objAiBase = _target as Obj_AI_Base;
            if (objAiBase != null && (objAiBase.IsMonster && Value.Mode(Orbwalker.ActiveModes.JungleClear) &&
                                                   Value.Get("jungle.mana") < Player.Instance.ManaPercent))
            {
                targets =
                    EntityManager.MinionsAndMonsters.Monsters.Where(
                        m => m.Distance(Player.Instance) < Player.Instance.AttackRange)
                        .ToList();
                if (!targets.Any()) return;
                if (_q.IsReady() && Value.Use("jungle.q"))
                {
                    _q.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _q.Range).To3D());
                }
                else if (_e.IsReady() && Value.Use("jungle.e"))
                {
                    if (targets.Any(t => Variables.SummonerRiftJungleList.Contains(t.BaseSkinName)))
                        CondemnLogic(targets.First(t => Variables.SummonerRiftJungleList.Contains(t.BaseSkinName)));
                }
            }
            else
            {
                var aiBase = _target as Obj_AI_Base;
                if (aiBase != null && (aiBase.IsMinion && Value.Get("lane.mana") < Player.Instance.ManaPercent &&
                                       Value.Mode(Orbwalker.ActiveModes.LaneClear)))
                {
                    targets =
                        EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                            m => m.Distance(Player.Instance) < Player.Instance.AttackRange).ToList();
                    if (!targets.Any()) return;
                    if (_q.IsReady() && Value.Use("lane.q"))
                    {
                        _q.Cast(Player.Instance.ServerPosition.Extend(Game.CursorPos, _q.Range).To3D());
                    }
                }
            }
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Slot == SpellSlot.Q) Orbwalker.ResetAutoAttack();
        }

        private void Game_OnTick(EventArgs args)
        {
        }

        #endregion

        #region Utils

        private static void CondemnLogic(Obj_AI_Base target)
        {
            for (var i = 1; i <= 25; i++)
            {
                var pos =
                    Player.Instance.ServerPosition.Extend(
                        Prediction.Position.PredictUnitPosition(target,
                            (int)(target.ServerPosition.Distance(Player.Instance.ServerPosition) / 1200)),
                        target.ServerPosition.Distance(Player.Instance.ServerPosition) +
                        (400 - target.BoundingRadius) / 25 * i)
                        .To3D();
                var circ = new Geometry.Polygon.Circle(pos, target.BoundingRadius * 0.4f);
                if (
                    circ.Points.Any(
                        point =>
                            point.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) ||
                            point.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building)))
                {
                    _e.Cast(target);
                }
            }
        }

        private static bool InsideCone()
        {
            if (_target == null) return false;
            var cone = new Geometry.Polygon.Sector(Player.Instance.Position,
                OKTRGeometry.Rotatoes(Player.Instance.Position.To2D(), _target.Position.To2D(), 90).To3D(),
                Geometry.DegreeToRadian(180),
                Player.Instance.AttackRange + Player.Instance.BoundingRadius * 2);
            return cone.IsInside(Game.CursorPos.To2D());
        }

        private float GetRawDamage(Obj_AI_Base minion)
        {
            return 0;
        }

        public static int GetSpellDelay
        {
            get { return 10 * (new Random().Next(_randomizerOne, _randomizerTwo) / 10); }
        }

        #endregion
    }
}