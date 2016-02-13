using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace OKTRAIO.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using OKTRAIO.Menu_Settings;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;

    public class BaseUlt : UtilityAddon
    {
        private const int LineThickness = 4;

        private const int Length = 260;

        private const int Height = 25;

        private readonly List<Recall> Recalls = new List<Recall>();

        private readonly List<BaseUltUnit> BaseUltUnits = new List<BaseUltUnit>();

        private readonly List<BaseUltSpell> BaseUltSpells = new List<BaseUltSpell>();

        public readonly List<Champion> CompatibleChampions = new List<Champion>
        {
            Champion.Ashe,
            Champion.Draven,
            Champion.Ezreal,
            Champion.Jinx
        };

        private int BarX
        {
            get { return Value.Get("baseult.x"); }
        }

        private int BarY
        {
            get { return Value.Get("baseult.y"); }
        }

        private int BarWidth
        {
            get { return Value.Get("baseult.width"); }
        }

        public bool IsCompatibleChamp()
        {
            return CompatibleChampions.Any(x => x.Equals(Player.Instance.Hero));
        }

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "BaseUlt", "baseult", "Unknown", Champion.Ashe, Champion.Draven, Champion.Ezreal, Champion.Jinx);
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - BaseULT for " + Player.Instance.ChampionName,
                "baseult.grouplabel.utilitymenu");
            Menu.AddCheckBox("baseult.use", "Use BaseUlt");
            Menu.Add("baseult.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange +=
                Value.AdvancedModeChanged;
            Menu.AddCheckBox("baseult.recallsEnemy", "Show enemy recalls", true, true);
            Menu.AddCheckBox("baseult.recallsAlly", "Show ally recalls", true, true);
            Menu.AddSlider("baseult.x", "Recall location X", (int) (Drawing.Width * 0.4), 0, Drawing.Width, true);
            Menu.AddSlider("baseult.y", "Recall location Y", (int) (Drawing.Height * 0.75), 0, Drawing.Height, true);
            Menu.AddSlider("baseult.width", "Recall width", 300, 200, 500, true);
            Menu.AddSeparator();
            Menu.AddLabel("Use BaseULT for:", 25, "baseult.label", true);
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                Menu.AddCheckBox("baseult." + enemy.ChampionName, enemy.ChampionName, true, true);
            }
        }

        public override void Initialize()
        {
            Teleport.OnTeleport += OnTeleport;

            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                Recalls.Add(new Recall(hero, RecallStatus.Inactive));
            }

            #region Spells

            BaseUltSpells.Add(new BaseUltSpell("Ashe", SpellSlot.R, 250, 1600, 130, true));
            BaseUltSpells.Add(new BaseUltSpell("Draven", SpellSlot.R, 400, 2000, 160, true));
            BaseUltSpells.Add(new BaseUltSpell("Ezreal", SpellSlot.R, 1000, 2000, 160, false));
            BaseUltSpells.Add(new BaseUltSpell("Jinx", SpellSlot.R, 600, 1700, 140, true));

            #endregion
        }

        protected override void Game_OnTick(EventArgs args)
        {
            foreach (var recall in Recalls)
            {
                if (recall.Status != RecallStatus.Inactive)
                {
                    var recallDuration = recall.Duration;
                    var cd = recall.Started + recallDuration - Game.Time;
                    var percent = cd > 0 && Math.Abs(recallDuration) > float.Epsilon ? 1f - cd / recallDuration : 1f;
                    var textLength = (recall.Unit.ChampionName.Length + 6) * 7;
                    var myLength = percent * Length;
                    var freeSpaceLength = myLength + textLength;
                    var freeSpacePercent = freeSpaceLength / Length > 1 ? 1 : freeSpaceLength / Length;
                    if (
                        Recalls.Any(
                            h =>
                                GetRecallPercent(h) > percent && GetRecallPercent(h) < freeSpacePercent
                                && h.TextPos == recall.TextPos && recall.Started > h.Started))
                    {
                        recall.TextPos += 1;
                    }

                    if (recall.Status == RecallStatus.Finished
                        && Recalls.Any(
                            h =>
                                h.Started > recall.Started && h.TextPos == recall.TextPos
                                && recall.Started + 3 < h.Started + recall.Duration))
                    {
                        recall.TextPos += 1;
                    }
                }

                if (recall.Status == RecallStatus.Active)
                {
                    if (recall.Unit.IsEnemy
                        && BaseUltUnits.All(h => h.Unit.NetworkId != recall.Unit.NetworkId))
                    {
                        var spell = BaseUltSpells.Find(h => h.Name == Player.Instance.ChampionName);
                        if (Player.Instance.Spellbook.GetSpell(spell.Slot).IsReady
                            && Player.Instance.Spellbook.GetSpell(spell.Slot).Level > 0)
                        {
                            BaseUltCalcs(recall);
                        }
                    }
                }

                if (recall.Status != RecallStatus.Active)
                {
                    var baseultUnit = BaseUltUnits.Find(h => h.Unit.NetworkId == recall.Unit.NetworkId);
                    if (baseultUnit != null)
                    {
                        BaseUltUnits.Remove(baseultUnit);
                    }
                }
            }

            foreach (var unit in BaseUltUnits)
            {
                if (unit.Collision)
                {
                    continue;
                }

                if (unit.Unit.IsVisible)
                {
                    unit.LastSeen = Game.Time;
                }

                if (Math.Round(unit.FireTime, 1) == Math.Round(Game.Time, 1) && Game.Time >= unit.LastSeen)
                {
                    var spell =
                        Player.Instance.Spellbook.GetSpell(
                            BaseUltSpells.Find(h => h.Name == Player.Instance.ChampionName).Slot);
                    if (spell.IsReady)
                    {
                        Player.Instance.Spellbook.CastSpell(spell.Slot, GetFountainPos());
                    }
                }
            }
        }

        public void DrawRect(float x, float y, float width, float height, float thickness, Color color)
        {
            for (var i = 0; i < height; i++)
            {
                Drawing.DrawLine(x, y + i, x + width, y + i, thickness, color);
            }
        }
        protected override void Drawing_OnEndScene(EventArgs args)
        {

            if (Value.Use("baseult.use"))
            {
                foreach (var unit in BaseUltUnits)
                {
                    var duration = Recalls.Find(h => h.Unit.NetworkId == unit.Unit.NetworkId).Duration;
                    var barPos = (unit.FireTime - Recalls.Find(h => unit.Unit.NetworkId == h.Unit.NetworkId).Started)
                                 / duration;

                    Drawing.DrawLine(
                        (int) (barPos * BarWidth) + BarX,
                        BarY - 15,
                        (int) (barPos * BarWidth) + BarX,
                        BarY - 5,
                        LineThickness,
                        Color.Lime);
                }
            }
        }

        private Vector3 GetFountainPos()
        {
            switch (Game.MapId)
            {
                case GameMapId.SummonersRift:
                {
                    return Player.Instance.Team == GameObjectTeam.Order
                        ? new Vector3(14296, 14362, 171)
                        : new Vector3(408, 414, 182);
                }
            }

            return new Vector3();
        }

        private double GetRecallPercent(Recall recall)
        {
            var recallDuration = recall.Duration;
            var cd = recall.Started + recallDuration - Game.Time;
            var percent = cd > 0 && Math.Abs(recallDuration) > float.Epsilon ? 1f - cd / recallDuration : 1f;
            return percent;
        }

        private float GetBaseUltTravelTime(float delay, float speed)
        {
            if (Player.Instance.ChampionName == "Karthus")
            {
                return delay / 1000;
            }

            var distance = Vector3.Distance(Player.Instance.ServerPosition, GetFountainPos());
            var missilespeed = speed;
            if (Player.Instance.ChampionName == "Jinx" && distance > 1350)
            {
                const float accelerationrate = 0.3f;
                var acceldifference = distance - 1350f;
                if (acceldifference > 150f)
                {
                    acceldifference = 150f;
                }

                var difference = distance - 1500f;
                missilespeed = (1350f * speed + acceldifference * (speed + accelerationrate * acceldifference)
                                + difference * 2200f) / distance;
            }

            return distance / missilespeed + (delay - 65) / 1000;
        }

        private double GetBaseUltSpellDamage(BaseUltSpell spell, AIHeroClient target)
        {
            var level = Player.Instance.Spellbook.GetSpell(spell.Slot).Level - 1;
            switch (spell.Name)
            {
                case "Ashe":
                {
                    var damage = new float[] { 250, 425, 600 }[level] + 1 * Player.Instance.FlatMagicDamageMod;
                    return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, damage);
                }

                case "Draven":
                {
                    var damage = new float[] { 175, 275, 375 }[level] + 1.1f * Player.Instance.FlatPhysicalDamageMod;
                    return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, damage) * 0.7;
                }

                case "Ezreal":
                {
                    var damage = new float[] { 350, 500, 650 }[level] + 0.9f * Player.Instance.FlatMagicDamageMod
                                 + 1 * Player.Instance.FlatPhysicalDamageMod;
                    return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, damage) * 0.7;
                }

                case "Jinx":
                {
                    var damage = new float[] { 250, 350, 450 }[level]
                                 + new float[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health)
                                 + 1 * Player.Instance.FlatPhysicalDamageMod;
                    Chat.Print("Flat Damage: " + new float[] { 250, 350, 450 }[level]);
                    Chat.Print("Bonus Damage: " + new float[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health));
                    Chat.Print("Damage On Unit: " + Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, damage));
                    Chat.Print("Unit Health: " + target.Health);
                    return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, damage);
                }
            }

            return 0;
        }

        private void BaseUltCalcs(Recall recall)
        {
            var finishedRecall = recall.Started + recall.Duration;
            var spellData = BaseUltSpells.Find(h => h.Name == Player.Instance.ChampionName);
            var timeNeeded = GetBaseUltTravelTime(spellData.Delay, spellData.Speed);
            var fireTime = finishedRecall - timeNeeded;
            var spellDmg = GetBaseUltSpellDamage(spellData, recall.Unit) - recall.Unit.MaxHealth * 0.1;
            var collision = GetCollision(spellData.Radius, spellData).Any();
            if (fireTime > Game.Time && fireTime < recall.Started + recall.Duration && recall.Unit.Health < spellDmg
                && Value.Use("baseult." + recall.Unit.ChampionName) && Value.Use("baseult.use"))
            {
                BaseUltUnits.Add(new BaseUltUnit(recall.Unit, fireTime, collision));
            }
            else if (BaseUltUnits.Any(h => h.Unit.NetworkId == recall.Unit.NetworkId))
            {
                BaseUltUnits.Remove(BaseUltUnits.Find(h => h.Unit.NetworkId == recall.Unit.NetworkId));
            }
        }

        public void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            var unit = Recalls.Find(h => h.Unit.NetworkId == sender.NetworkId);
            if (unit == null || args.Type != TeleportType.Recall || unit.Unit.IsAlly)
            {
                return;
            }
            switch (args.Status)
            {
                case TeleportStatus.Start:
                {
                    unit.Status = RecallStatus.Active;
                    unit.Started = Game.Time;
                    unit.TextPos = 0;
                    unit.Duration = (float) args.Duration / 1000;
                    break;
                }

                case TeleportStatus.Abort:
                {
                    unit.Status = RecallStatus.Abort;
                    unit.Ended = Game.Time;
                    break;
                }

                case TeleportStatus.Finish:
                {
                    unit.Status = RecallStatus.Finished;
                    unit.Ended = Game.Time;
                    break;
                }
            }
        }

        private IEnumerable<Obj_AI_Base> GetCollision(float spellwidth, BaseUltSpell spell)
        {
            return (from unit in EntityManager.Heroes.Enemies.Where(h => Player.Instance.Distance(h) < 2000)
                    let pred =
                        Prediction.Position.PredictLinearMissile(
                            unit,
                            2000,
                            (int) spell.Radius,
                            (int) spell.Delay,
                            spell.Speed,
                            -1)
                    let endpos = Player.Instance.ServerPosition.Extend(GetFountainPos(), 2000)
                    let projectOn = pred.UnitPosition.To2D().ProjectOn(Player.Instance.ServerPosition.To2D(), endpos)
                    where projectOn.SegmentPoint.Distance(endpos) < spellwidth + unit.BoundingRadius
                    select unit).Cast<Obj_AI_Base>().ToList();
        }

        public BaseUlt(Menu menu, Champion champion) : base(menu, champion)
        {
        }
    }

    public class Recall
    {
        public int TextPos;

        public Recall(AIHeroClient unit, RecallStatus status)
        {
            Unit = unit;
            Status = status;
        }

        public AIHeroClient Unit { get; set; }

        public RecallStatus Status { get; set; }

        public float Started { get; set; }

        public float Ended { get; set; }

        public float Duration { get; set; }
    }

    public class BaseUltUnit
    {
        public BaseUltUnit(AIHeroClient unit, float fireTime, bool collision)
        {
            Unit = unit;
            FireTime = fireTime;
            Collision = collision;
        }

        public AIHeroClient Unit { get; set; }

        public float FireTime { get; set; }

        public bool Collision { get; set; }

        public float LastSeen { get; set; }

        public float PredictedPos { get; set; }
    }

    public class BaseUltSpell
    {
        public BaseUltSpell(string name, SpellSlot slot, float delay, float speed, float radius, bool collision)
        {
            Name = name;
            Slot = slot;
            Delay = delay;
            Speed = speed;
            Radius = radius;
            Collision = collision;
        }

        public string Name { get; set; }

        public SpellSlot Slot { get; set; }

        public float Delay { get; set; }

        public float Speed { get; set; }

        public float Radius { get; set; }

        public bool Collision { get; set; }
    }

    public enum RecallStatus
    {
        Active,
        Inactive,
        Finished,
        Abort
    }
}
