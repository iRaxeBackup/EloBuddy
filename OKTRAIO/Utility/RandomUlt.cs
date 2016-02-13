using EloBuddy.SDK.Menu.Values;
using Menu = EloBuddy.SDK.Menu.Menu;

namespace OKTRAIO.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    using OKTRAIO.Menu_Settings;

    using SharpDX;

    public class RandomUlt : UtilityAddon
    {
        private readonly List<RandomUltUnit> RandomUltUnits = new List<RandomUltUnit>();

        private readonly List<RandomUltSpell> RandomUltSpells = new List<RandomUltSpell>();

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "RandomUlt", "randomult", "Unknown", Champion.Ashe, Champion.Draven, Champion.Ezreal, Champion.Jinx, Champion.Gangplank, Champion.Lux, Champion.Ziggs);
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - RandomUlt for " + Player.Instance.ChampionName,
                "randomult.grouplabel.utilitymenu");
            Menu.AddCheckBox("randomult.use", "Use RandomUlt");
            Menu.Add("randomult.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange +=
                Value.AdvancedModeChanged;
            Menu.AddSeparator();
            Menu.AddSlider("randomult.range", "Don't Ult When Enemies In Range", 1000, 0, 2000, true);
            Menu.AddSlider("randomult.delay", "Delay Before Ulting", 600, 0, 3000, true);
            Menu.AddSlider("randomult.hitchance", "RandomUlt Hitchance (1 - More Ults, 5 - Less Ults)", 3, 1, 5,
                true);
            Menu.AddLabel("Who To RandomUlt:", 25, "randomult.label", true);
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                Menu.AddCheckBox("randomult." + enemy.ChampionName, enemy.ChampionName, true, true);
            }
        }

        public override void Initialize()
        {
            Teleport.OnTeleport += OnTeleport;

            foreach (var hero in EntityManager.Heroes.Enemies)
            {
                RandomUltUnits.Add(new RandomUltUnit(hero));
            }

            #region Spells

            RandomUltSpells.Add(new RandomUltSpell("Ashe", SpellSlot.R, 250, 1600, 130, int.MaxValue, true, DamageType.Magical));
            RandomUltSpells.Add(new RandomUltSpell("Draven", SpellSlot.R, 400, 2000, 160, int.MaxValue, true, DamageType.Physical));
            RandomUltSpells.Add(new RandomUltSpell("Ezreal", SpellSlot.R, 1000, 2000, 160, int.MaxValue, false, DamageType.Magical));
            RandomUltSpells.Add(new RandomUltSpell("Jinx", SpellSlot.R, 600, 1700, 140, int.MaxValue, true, DamageType.Physical));
            RandomUltSpells.Add(
                new RandomUltSpell("Gangplank", SpellSlot.R, 100, int.MaxValue, 600, int.MaxValue, false, DamageType.Magical));
            RandomUltSpells.Add(new RandomUltSpell("Lux", SpellSlot.R, 500, int.MaxValue, 190, 3000, false, DamageType.Magical));
            RandomUltSpells.Add(new RandomUltSpell("Ziggs", SpellSlot.R, 100, 1750, 525, 5000, false, DamageType.Magical));

            #endregion
        }

        protected override void Game_OnUpdate(EventArgs args)
        {
            foreach (var enemy in
                RandomUltUnits.Where(x => x.Unit.IsHPBarRendered && !x.Unit.IsDead && x.Unit.IsValidTarget()))
            {
                enemy.LastSeen = Game.Time;
                enemy.OldPos = enemy.Unit.Position;
                enemy.PredictedPos = Prediction.Position.PredictUnitPosition(enemy.Unit, 10).To3DWorld();
            }

            if (!Value.Use("randomult.use") || Player.Instance.CountEnemiesInRange(Value.Get("randomult.range")) >= 1
                || Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsOnCooldown
                || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None
                || (Player.Instance.ChampionName == "Draven"
                    && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name != "DravenRCast"))
            {
                return;
            }
            foreach (
                var enemy in
                    RandomUltUnits.Where(
                        x =>
                            x.Unit.IsValid() && !x.Unit.IsDead && Value.Use("randomult." + x.Unit.ChampionName)
                            && x.RecallData.Status == RecallStatus.Active)
                        .OrderByDescending(x => x.RecallData.Started)
                        .Where(
                            enemy =>
                                !CheckResurrect(enemy.Unit)
                                && (!(Game.Time - enemy.RecallData.Started <= Value.Get("randomult.delay")))))
            {
                var dist = ((enemy.RecallData.Started - enemy.LastSeen) / 1000 * enemy.Unit.MoveSpeed);
                // - enemy.Unit.MoveSpeed / 4;
                if (dist > 2000 || (dist * 2 > 1000 && !enemy.Unit.IsHPBarRendered))
                {
                    continue;
                }
                var pos = enemy.Unit.IsHPBarRendered
                    ? enemy.Unit.Position
                    : enemy.OldPos.Extend(enemy.PredictedPos, dist).To3D();
                if (Player.Instance.Distance(pos)
                    > RandomUltSpells.Find(x => x.Name == Player.Instance.ChampionName).Range)
                {
                    continue;
                }
                CastRandomUlt(enemy, pos);
            }
        }

        private void CastRandomUlt(RandomUltUnit target, Vector3 pos)
        {
            if (!Player.Instance.Spellbook.GetSpell(SpellSlot.R).IsReady)
            {
                return;
            }

            if (GetRandomUltSpellDamage(RandomUltSpells.Find(x => x.Name == Player.Instance.ChampionName), target.Unit)
                && UltTime(pos) < target.RecallData.GetRecallTime() && Value.Use("randomult.use"))
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.R, pos);
            }
        }

        private bool CheckResurrect(AIHeroClient enemy)
        {
            switch (enemy.ChampionName)
            {
                case "Anivia":
                    return !enemy.HasBuff("rebirthcooldown");

                case "Aatrox":
                    return enemy.HasBuff("aatroxpassiveready");

                case "Zac":
                    ;
                    return enemy.HasBuff("ZacRebirthReady");

            }
            return enemy.HasUndyingBuff() || enemy.HasBuffOfType(BuffType.SpellShield) || enemy.IsInvulnerable || enemy.HasBuff("ChronoRevive");
        }

        private float UltTime(Vector3 pos)
        {
            var distance = Player.Instance.Distance(pos);
            var spell = RandomUltSpells.Find(x => x.Name == Player.Instance.ChampionName);
            var delay = spell.Delay;
            var speed = spell.Speed;
            if (Player.Instance.ChampionName == "Jinx" && distance > 1350)
            {
                var accelDif = distance - 1350;
                if (accelDif > 150)
                {
                    accelDif = 150;
                }
                var difference = distance - 1500;
                return (distance / ((1350 * 1700 + accelDif * (1700 + 0.3f * accelDif) + difference * 1700f) / distance))
                       * 1000 + 250;
            }
            return (distance / speed) * 1000 + delay;
        }

        private bool GetRandomUltSpellDamage(RandomUltSpell spell, Obj_AI_Base target)
        {
            var level = Player.Instance.Spellbook.GetSpell(spell.Slot).Level - 1;
            var damageType = spell.DamageType;
            var damage = 0f;
            switch (spell.Name)
            {
                case "Ashe":
                {
                    damage = new float[] { 250, 425, 600 }[level] + 1 * Player.Instance.FlatMagicDamageMod;
                    break;
                }

                case "Draven":
                {
                    damage = (new float[] { 175, 275, 375 }[level] + 1.1f * Player.Instance.FlatPhysicalDamageMod)
                             * 2;
                    break;
                }

                case "Ezreal":
                {
                    damage = new float[] { 350, 500, 650 }[level] + 0.9f * Player.Instance.FlatMagicDamageMod
                             + 1 * Player.Instance.FlatPhysicalDamageMod;
                    break;
                }

                case "Gangplank":
                {
                    damage = (new float[] { 50, 70, 90 }[level] + 0.1f * Player.Instance.FlatMagicDamageMod) * 5;
                    break;
                }

                case "Jinx":
                {
                    damage = new float[] { 250, 350, 450 }[level]
                             + new float[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health)
                             + 1 * Player.Instance.FlatPhysicalDamageMod;
                    break;
                }

                case "Lux":
                {
                    damage = new float[] { 300, 400, 500 }[level] + 0.75f * Player.Instance.FlatMagicDamageMod;
                    break;
                }

                case "Ziggs":
                {
                    damage = new float[] { 200, 300, 400 }[level] + 0.72f * Player.Instance.FlatMagicDamageMod;
                    break;
                }
            }
            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) * 0.7 > target.Health;
        }

        public void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            var enemy = RandomUltUnits.Find(x => x.Unit.NetworkId == sender.NetworkId);
            if (enemy == null || args.Type != TeleportType.Recall)
            {
                return;
            }
            var recall = enemy.RecallData;
            switch (args.Status)
            {
                case TeleportStatus.Start:
                {
                    recall.Status = RecallStatus.Active;
                    recall.Started = Game.Time;
                    recall.Duration = args.Duration;
                    recall.Ended = recall.Started + recall.Duration;
                    break;
                }

                case TeleportStatus.Abort:
                {
                    recall.Status = RecallStatus.Abort;
                    recall.Ended = Game.Time;
                    break;
                }

                case TeleportStatus.Finish:
                {
                    recall.Status = RecallStatus.Finished;
                    recall.Ended = Game.Time;
                    break;
                }
            }
        }

        public RandomUlt(Menu menu, Champion? champion) : base(menu, champion)
        {

        }
    }

    internal class RandomUltUnit
    {
        public AIHeroClient Unit;

        public float LastSeen;

        public Vector3 PredictedPos;

        public Vector3 OldPos;

        public RandomUltRecall RecallData;

        public RandomUltUnit(AIHeroClient unit)
        {
            Unit = unit;
            RecallData = new RandomUltRecall(unit, RecallStatus.Inactive);
        }
    }

    public class RandomUltSpell
    {
        public RandomUltSpell(
            string name,
            SpellSlot slot,
            float delay,
            float speed,
            float radius,
            float range,
            bool collision,
            DamageType dmgType)
        {
            Name = name;
            Slot = slot;
            Delay = delay;
            Speed = speed;
            Radius = radius;
            Range = range;
            Collision = collision;
            DamageType = dmgType;
        }

        public string Name { get; set; }

        public SpellSlot Slot { get; set; }

        public float Delay { get; set; }

        public float Speed { get; set; }

        public float Radius { get; set; }

        public float Range { get; set; }

        public bool Collision { get; set; }
        public DamageType DamageType { get; set; }
    }

    public class RandomUltRecall
    {
        public RandomUltRecall(AIHeroClient unit, RecallStatus status)
        {
            Unit = unit;
            Status = status;
            Started = 0;
            Ended = 0;
            Duration = 0;
        }

        public AIHeroClient Unit { get; set; }

        public RecallStatus Status { get; set; }

        public float Started { get; set; }

        public float Ended { get; set; }

        public float Duration { get; set; }

        public float GetRecallTime()
        {
            return (Started + Duration - Game.Time) * 1000;
        }
    }
}