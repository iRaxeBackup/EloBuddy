using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ThugThresh
{
    public static class Program
    {
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }
        public static string version = "1.0.3.7";
        public static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        public static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Active Q2;
        public static Spell.Active R;
        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Thresh") return;
            AbilitySequence = new int[] { 3, 1, 2, 3, 1, 4, 1, 1, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
            Chat.Print("Thug Thresh Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FLAME!", Color.CornflowerBlue);
            ThreshMenu.loadMenu();
            Game.OnTick += GameOnTick;
            MyActivator.loadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 1075, SkillShotType.Linear, (int)0.35f, 1200, 60);
            Q2 = new Spell.Active(SpellSlot.Q, 1075);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, (int)0.25f, 1750, 300);
            E = new Spell.Skillshot(SpellSlot.E, 500, SkillShotType.Linear, 1, 2000, 110);
            R = new Spell.Active(SpellSlot.R, 350);
            #endregion

            Obj_AI_Base.OnNewPath += Obj_AI_Base_OnNewPath;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;
            AIHeroClient.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Drawing.OnDraw += GameOnDraw;
        }

        public static void GameOnDraw(EventArgs args)
        {
            if (ThreshMenu.nodraw()) return;

            if (!ThreshMenu.onlyReady())
            {
                if (ThreshMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (ThreshMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (ThreshMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (ThreshMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (ThreshMenu.Allydrawn())
                {
                    DrawHealths();
                }

            }
            else
            {
                if (!Q.IsOnCooldown && ThreshMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && ThreshMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && ThreshMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && ThreshMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (ThreshMenu.Allydrawn())
                {
                    DrawHealths();
                }
            }
        }
        private static void DrawHealths()
        {

            {
                float i = 0;
                foreach (
                    var hero in EntityManager.Heroes.Allies.Where(hero => hero.IsAlly && !hero.IsMe && !hero.IsDead))
                {
                    var playername = hero.Name;
                    if (playername.Length > 13)
                    {
                        playername = playername.Remove(9) + "..";
                    }
                    var champion = hero.ChampionName;
                    if (champion.Length > 12)
                    {
                        champion = champion.Remove(7) + "..";
                    }
                    var percent = (int)(hero.Health / hero.MaxHealth * 100);
                    var color = Color.Red;
                    if (percent > 25)
                    {
                        color = Color.Orange;
                    }
                    if (percent > 50)
                    {
                        color = Color.Yellow;
                    }
                    if (percent > 75)
                    {
                        color = Color.LimeGreen;
                    }
                    Drawing.DrawText(
                        Drawing.Width * 0.8f, Drawing.Height * 0.1f + i, color, playername + " (" + champion + ") ");
                    Drawing.DrawText(
                        Drawing.Width * 0.9f, Drawing.Height * 0.1f + i, color,
                        ((int)hero.Health).ToString() + " (" + percent.ToString() + "%)");
                    i += 20f;
                }
            }
        }
        private static void OnGameUpdate(EventArgs args)
        {
            if (MyActivator.heal != null)
                Heal();
            if (MyActivator.ignite != null)
                ignite();
            if (MyActivator.exhaust != null)
                exhaust();
            Player.SetSkinId(ThreshMenu.skinId());
        }

        public static void ChangeDSkin(Object sender, EventArgs args)
        {
            Player.SetSkinId(ThreshMenu.skinId());
        }
        public static void LevelUpSpells()
        {
            int qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level + qOff;
            int wL = Player.Spellbook.GetSpell(SpellSlot.W).Level + wOff;
            int eL = Player.Spellbook.GetSpell(SpellSlot.E).Level + eOff;
            int rL = Player.Spellbook.GetSpell(SpellSlot.R).Level + rOff;
            if (qL + wL + eL + rL < ObjectManager.Player.Level)
            {
                int[] level = new int[] { 0, 0, 0, 0 };
                for (int i = 0; i < ObjectManager.Player.Level; i++)
                {
                    level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
                }
                if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
                if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
                if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
                if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
            }
        }


        static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || sender.IsMinion || sender.IsAlly || sender.IsValidTarget(Q.Range)) return;
            if (args.SData.Name == "summonerflash")
            {
                if (Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue,
                    args.End).HitChance >= HitChance.High)
                {
                    Q.Cast(args.End);
                }
            }
            else if (args.SData.Name == "EzrealArcaneShift")
            {
                if (Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue,
                    args.End).HitChance >= HitChance.High)
                {
                    Q.Cast(args.End);
                }
            }
            else if (args.SData.Name == "Deceive")
            {
                if (Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue,
                    args.End).HitChance >= HitChance.High)
                {
                    Q.Cast(args.End);
                }
            }
        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !ThreshMenu.MyHarrass["gapcloser.e"].Cast<CheckBox>().CurrentValue || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            E.Cast(e.Sender);
        }
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }
            if (E.IsReady() && E.IsInRange(sender) && ThreshMenu.MyHarrass["interrupt.e"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
            }
            else if (Q.IsReady() && ThreshMenu.MyOtherFunctions["interrupt.q"].Cast<CheckBox>().CurrentValue && Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).HitChance >= HitChance.Low)
            {
                Q.Cast(sender);
            }
        }
        private static void SolariItem()
        {
            if (MyActivator.ironsolari.IsReady() && MyActivator.ironsolari.IsOwned())
            {
                var solari = ThreshMenu.MyActivator["ironsolari"].Cast<CheckBox>().CurrentValue;
                var enemies = ThreshMenu.MyActivator["items.enemiesinrange"].Cast<Slider>().CurrentValue;
                var hpcheck = ThreshMenu.MyActivator["items.sliderHP"].Cast<Slider>().CurrentValue;
                foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                {
                    if (EntityManager.Heroes.Enemies.Where(enemy => enemy != Player && enemy.Distance(Player) <= 1000).Count() > enemies && ally.Distance(Player) <= 600 && MyActivator.ironsolari.IsReady() && solari)
                    {
                        if (ally.HealthPercent < hpcheck)
                        {
                            MyActivator.ironsolari.Cast();
                        }
                    }
                }
            }
        }
        private static void AscensionItem()
        {
            if (MyActivator.talisman.IsReady() && MyActivator.talisman.IsOwned())
            {
                var ascension = ThreshMenu.MyActivator["talisman"].Cast<CheckBox>().CurrentValue;
                var enemies = ThreshMenu.MyActivator["items.enemiesinrange"].Cast<Slider>().CurrentValue;
                var hpcheck = ThreshMenu.MyActivator["items.sliderHP"].Cast<Slider>().CurrentValue;
                if (ascension && Player.HealthPercent <= hpcheck && Player.CountEnemiesInRange(800) >= enemies)
                {
                    MyActivator.talisman.Cast();
                }
            }
        }
        private static void RanduinItem()
        {
            if (MyActivator.randuin.IsReady() && MyActivator.randuin.IsOwned())
            {
                var randuin = ThreshMenu.MyActivator["randuin"].Cast<CheckBox>().CurrentValue;
                if (randuin && Target.IsValidTarget(MyActivator.randuin.Range) && MyActivator.randuin.IsReady())
                {
                    MyActivator.randuin.Cast();
                }
            }
        }
        private static void GloryItem()
        {
            if (MyActivator.glory.IsReady() && MyActivator.glory.IsOwned())
            {
                var glory = ThreshMenu.MyActivator["glory"].Cast<CheckBox>().CurrentValue;
                if (EntityManager.Heroes.Allies.Where(ally => ally != Player && ally.Distance(Player) <= 700).Count() > 0 && MyActivator.glory.IsReady() && glory)
                {
                    MyActivator.glory.Cast();
                }
            }
        }
        public static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player, autoIgnite, MyActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= ThreshMenu.spellsHealignite())
                MyActivator.ignite.Cast(autoIgnite);

        }
        public static void Heal()
        {
            if (MyActivator.heal.IsReady() && Player.HealthPercent <= ThreshMenu.spellsHealhp())
                MyActivator.heal.Cast();
        }
        private static void exhaust()
        {
            if (!MyActivator.exhaust.IsReady() || Player.IsDead) return;
            if (ThreshMenu.MySpells["useexhaust"].Cast<CheckBox>().CurrentValue && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies
                        .Where(a => a.IsValidTarget(MyActivator.exhaust.Range))
                        .Where(enemy => ThreshMenu.MyOtherFunctions[enemy.ChampionName + "exhaust"].Cast<CheckBox>().CurrentValue))
            {
                if (enemy.IsFacing(Player))
                {
                    if (!(Player.HealthPercent < 50)) continue;
                    MyActivator.exhaust.Cast(enemy);
                    return;
                }
                if (!(enemy.HealthPercent < 50)) continue;
                MyActivator.exhaust.Cast(enemy);
                return;
            }
        }

        private static void AutoQ()
        {
            if (!ThreshMenu.MyOtherFunctions["immobile.q"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }
            var autoQTarget =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x =>
                    x.HasBuffOfType(BuffType.Charm) || x.HasBuffOfType(BuffType.Knockup)
                    || x.HasBuffOfType(BuffType.Stun) || x.HasBuffOfType(BuffType.Suppression)
                    || x.HasBuffOfType(BuffType.Snare));
            if (autoQTarget != null && !autoQTarget.HasBuff("ThreshQ") && Prediction.Position.PredictLinearMissile(
                    autoQTarget,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).HitChance >= HitChance.High && !Prediction.Position.PredictLinearMissile(
                    autoQTarget,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).CollisionObjects.Any())
            {
                Q.Cast(autoQTarget);
            }
        }
        private static void AutoW()
        {
            var lanternLowAllies = ThreshMenu.MyOtherFunctions["lowallies.w"].Cast<CheckBox>().CurrentValue;
            var lanternHealthPercent = ThreshMenu.MyOtherFunctions["allypercent.w"].Cast<Slider>().CurrentValue;

            if (lanternLowAllies)
            {
                var ally =
                    EntityManager.Heroes.Allies
                        .FirstOrDefault(x => x.IsValidTarget(W.Range) && x.HealthPercent <= lanternHealthPercent);

                if (ally != null && ally.CountEnemiesInRange(700) >= 1)
                {
                    W.Cast();
                }
            }
        }
        private static void Obj_AI_Base_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (!sender.IsValid() || !args.IsDash || !sender.IsValidTarget(Q.Range) || sender.Type != Player.Type || !sender.IsEnemy)
            {
                return;
            }
            if (Q.IsReady() && !E.IsInRange(sender) && ThreshMenu.MyOtherFunctions["dash.q"].Cast<CheckBox>().CurrentValue)
            {
                var endPosition = args.Path.Last();
                if (Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue,
                    endPosition).HitChance < HitChance.High && Prediction.Position.PredictLinearMissile(
                    sender,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).CollisionObjects.Any())
                {
                    return;
                }
                Q.Cast(endPosition);
            }
            else if (E.IsReady() && E.IsInRange(sender) && ThreshMenu.MyHarrass["dash.e"].Cast<CheckBox>().CurrentValue)
            {
                var endPosition = args.Path.Last();
                var isFleeing = endPosition.Distance(Player.ServerPosition) > Player.Distance(sender);

                var prediction = E.GetPrediction(sender);

                if (prediction.HitChance < HitChance.High)
                {
                    return;
                }

                var x = Player.ServerPosition.X - endPosition.X;
                var y = Player.ServerPosition.Y - endPosition.Y;

                var vector = new Vector3(
                    Player.ServerPosition.X + x,
                    Player.ServerPosition.Y + y,
                    Player.ServerPosition.Z);

                E.Cast(
                    !isFleeing
                        ? prediction.CastPosition
                        : vector);
            }
        }
        private static void GameOnTick(EventArgs args)
        {
            var fotmountainitem = (ThreshMenu.MyActivator["fotmountain"].Cast<CheckBox>().CurrentValue);
            var crucible = (ThreshMenu.MyActivator["mikael"].Cast<CheckBox>().CurrentValue);
            var hpcheck = (ThreshMenu.MyActivator["items.sliderHP"].Cast<Slider>().CurrentValue);
            var enemies = (ThreshMenu.MyActivator["items.enemiesinrange"].Cast<Slider>().CurrentValue);
            if (Player.IsDead)
            {
                return;
            }

            if (Player.CountEnemiesInRange(1000) <= enemies)
            {
                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.IsFacing(enemy) && ally.HealthPercent <= hpcheck && Player.Distance(ally) <= 750)
                        {

                            if (fotmountainitem && MyActivator.fotmountain.IsReady())
                            {
                                MyActivator.fotmountain.Cast(ally);
                            }

                            if (crucible && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt) || ally.HasBuffOfType(BuffType.Polymorph)) && MyActivator.mikael.IsReady())
                            {
                                MyActivator.mikael.Cast(ally);
                            }
                        }
                    }
                }
            }
            if (ThreshMenu.MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (ThreshMenu.MyOtherFunctions["lantern"].Cast<KeyBind>().CurrentValue)
            {
                TrowLantern();
            }
            if (ThreshMenu.MyOtherFunctions["push"].Cast<KeyBind>().CurrentValue)
            {
                var Target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if (Target != null)
                {
                    Push(Target);
                }
            }
            if (ThreshMenu.MyOtherFunctions["pull"].Cast<KeyBind>().CurrentValue)
            {
                var Target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if (Target != null)
                {
                    Pull(Target);
                }
            }
            AscensionItem();
            RanduinItem();
            GloryItem();
            SolariItem();
            AutoW();
            AutoQ();

        }

        private static void Pull(AIHeroClient Target)
        {
            if (Target != null && Player.Distance(Target) <= E.Range)
            {
                var pX = Player.Position.X + (Player.Position.X - Target.Position.X);
                var pY = Player.Position.Y + (Player.Position.Y - Target.Position.Y);
                E.Cast(new Vector3(new Vector2(pX), pY));

            }
        }

        private static void Push(AIHeroClient Target)
        {
            if (Target != null && Player.Distance(Target) <= E.Range)
            {
                E.Cast(Target.ServerPosition);
            }
        }
        public static void OnLaneClear()
        {

            Orbwalker.ForcedTarget = null;
            var useE = ThreshMenu.MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, E.Range, false).Count();
            var source = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, E.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (E.IsReady() && useE && ThreshMenu.MyFarm["lc.MinionsE"].Cast<Slider>().CurrentValue <= count)
            {
                var prediction = E.GetPrediction(source);
                if (prediction.HitChance >= HitChance.High)
                {
                    E.Cast(source);
                }
            }
            return;

        }

        public static void OnJungle()
        {
            Orbwalker.ForcedTarget = null;

            var useQ = ThreshMenu.MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
            var useW = ThreshMenu.MyFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
            var useE = ThreshMenu.MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (source == null) return;

            if (Q.IsReady() && useQ && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
                return;
            }

            if (W.IsReady() && useW && source.Distance(Player) < Player.GetAutoAttackRange(source))
            {
                W.Cast(W.GetPrediction(Player).CastPosition);
                return;

            }
            if (E.IsReady() && useE && source.Distance(Player) < Player.GetAutoAttackRange(source))
            {
                E.Cast(source);
                return;
            }
            return;
        }

        private static void OnHarrass()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (!Target.IsValidTarget())
            {
                return;
            }

            var useQ1 = ThreshMenu.MyHarrass["harrass.Q1"].Cast<CheckBox>().CurrentValue;
            var useQ2 = ThreshMenu.MyHarrass["harrass.Q2"].Cast<CheckBox>().CurrentValue;
            var useE = ThreshMenu.MyHarrass["harrass.E"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady())
            {
                if (useQ1 && !Target.HasBuff("ThreshQ") && Prediction.Position.PredictLinearMissile(
                    Target,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).HitChance >= HitChance.High && !Prediction.Position.PredictLinearMissile(
                    Target,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).CollisionObjects.Any())
                {
                    Q.Cast(Target);
                }

                if (useQ2 && Target.HasBuff("ThreshQ") && !Target.IsMinion)
                {
                    Q2.Cast();
                }
            }
            if (useE && E.IsReady() || !Target.HasBuff("ThreshQ"))
            {
                var isFleeing = Player.Distance(Target) < Target.Distance(Game.CursorPos);
                var prediction = E.GetPrediction(Target);

                if (prediction.HitChance < HitChance.High)
                {
                    return;
                }

                var x = Player.ServerPosition.X - Target.ServerPosition.X;
                var y = Player.ServerPosition.Y - Target.ServerPosition.Y;

                var vector = new Vector3(
                    Player.ServerPosition.X + x,
                    Player.ServerPosition.Y + y,
                    Player.ServerPosition.Z);

                E.Cast(
                    isFleeing
                        ? prediction.CastPosition
                        : vector);
            }
        }
        private static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (!Target.IsValidTarget(Q.Range) || Target == null)
            {
                return;
            }

            var useQ = ThreshMenu.MyCombo["combo.Q"].Cast<CheckBox>().CurrentValue;
            var useW = ThreshMenu.MyCombo["combo.W"].Cast<CheckBox>().CurrentValue;
            var useE = ThreshMenu.MyCombo["combo.E"].Cast<CheckBox>().CurrentValue;


            if (useQ && Q.IsReady())
            {
                if (Target.HasBuff("ThreshQ") && !Target.IsMinion)
                {
                    Q2.Cast();
                }
                else if (!Target.HasBuff("ThreshQ") && Prediction.Position.PredictLinearMissile(
                    Target,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).HitChance >= HitChance.High && !Prediction.Position.PredictLinearMissile(
                    Target,
                    Q.Range,
                    Q.Width,
                    Q.CastDelay,
                    Q.Speed,
                    int.MaxValue).CollisionObjects.Any())
                {
                    Q.Cast(Target);
                }
            }
            if (useW && W.IsReady() && Target.HasBuff("ThreshQ"))
            {
                var ally =
                    EntityManager.Heroes.Allies
                        .FirstOrDefault(x => !x.IsMe && x.IsValidTarget(W.Range) && x.Distance(Player) > 300);

                if (ally != null)
                {
                    W.Cast(W.GetPrediction(ally).CastPosition);
                }
                if (Player.HealthPercent < 50)
                {
                    W.Cast(W.GetPrediction(Player).CastPosition);
                }
            }
            if (useE && E.IsReady() || !Target.HasBuff("ThreshQ"))
            {
                var isFleeing = Player.Distance(Target) < Target.Distance(Game.CursorPos);
                var prediction = E.GetPrediction(Target);

                if (prediction.HitChance >= HitChance.High)
                {
                    var x = Player.ServerPosition.X - Target.ServerPosition.X;
                    var y = Player.ServerPosition.Y - Target.ServerPosition.Y;

                    var vector = new Vector3(
                        Player.ServerPosition.X + x,
                        Player.ServerPosition.Y + y,
                        Player.ServerPosition.Z);

                    E.Cast(
                        isFleeing
                            ? prediction.CastPosition
                            : vector);
                }
            }
            var useR = ThreshMenu.MyCombo["combo.R"].Cast<CheckBox>().CurrentValue;
            var ultEnemies = ThreshMenu.MyCombo["combo.REnemies"].Cast<Slider>().CurrentValue;
            if (Target.IsValidTarget(R.Range) && useR && R.IsReady() && Player.CountEnemiesInRange(R.Range) >= ultEnemies)
            {
                R.Cast();
            }
        }
        private static void TrowLantern()
        {
            var ally =
                                EntityManager.Heroes.Allies
                                    .FirstOrDefault(x => !x.IsMe && x.IsValidTarget(W.Range));
            if (ally != null)

                if (W.IsReady())
                {
                    W.Cast(ally);
                }
            }
        }
    }


