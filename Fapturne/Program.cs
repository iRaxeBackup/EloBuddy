using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Fapturne
{
    public static class Program
    {
        public static string Version = "1.2.5.6";
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        private static SoundPlayer allahuAkbar;
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Spell.Targeted R1;
        public static int time;
        public static readonly AIHeroClient Player = ObjectManager.Player;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Nocturne") return;
            AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
            Chat.Print("Fapturne Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT AFK!", Color.Red);
            FapturneMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            ProcessSpells.Initialize();

            #region Skill

            Q = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Active(SpellSlot.R, 2500);
            R1 = new Spell.Targeted(SpellSlot.R, R.Range);



            #endregion
        }
        private static void MoreRangeToParanoia()
        {
            if (R.Level > 0)
            {
                R = new Spell.Active(SpellSlot.R, 1750 + ((uint)R.Level) * 750);
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly || args.SData.IsAutoAttack()) return;
            var articunoPerfectCheck = Player.Position.PointOnLineSegment(args.Start,
                args.Start.Extend(args.End, args.SData.CastRangeDisplayOverride).To3D());
            if (ProcessSpells.DB.Contains(args.SData.Name) &&
                W.IsReady() &&
                (articunoPerfectCheck || (args.Target != null && args.Target.IsMe)))
            {
                W.Cast();
            }
        }

        public static bool PointOnLineSegment(this Vector3 CheckPoint, Vector3 Start, Vector3 End)
        {
            return (Start.X <= CheckPoint.X && CheckPoint.X <= End.X || End.X <= CheckPoint.X && CheckPoint.X <= Start.X) &&
                   (Start.Y <= CheckPoint.Y && CheckPoint.Y <= End.Y || End.Y <= CheckPoint.Y && CheckPoint.Y <= Start.Y);
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(E.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }
            if (E.IsReady() && E.IsInRange(sender) && FapturneMenu.interruptE())
            {
                E.Cast(sender);
            }
        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            if (E.IsReady() && E.IsInRange(sender) && FapturneMenu.gapcloserE())
            {
                E.Cast(sender);
            }
            if (Q.IsReady() && Q.IsInRange(sender) && FapturneMenu.gapcloserQ())
            {
                Q.Cast(sender.ServerPosition);
            }
        }
        private static void GameOnDraw(EventArgs args)
        {
            if (FapturneMenu.Nodraw()) return;

            if (!FapturneMenu.OnlyReady())
            {
                if (FapturneMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FapturneMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FapturneMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FapturneMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FapturneMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && FapturneMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = 1125, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && FapturneMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = 100, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && FapturneMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = 425, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && FapturneMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FapturneMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (MyActivator.smite != null)
                smite();
            if (MyActivator.Barrier != null)
                Barrier();
            if (MyActivator.Heal != null)
                Heal();
            if (MyActivator.Ignite != null)
                Ignite();
            if (FapturneMenu.checkSkin())
            {
                Player.SetSkinId(FapturneMenu.SkinId());
            }
        }

        private static void LevelUpSpells()
        {
            var qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            var wL = Player.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            var eL = Player.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            var rL = Player.Spellbook.GetSpell(SpellSlot.R).Level + ROff;
            if (qL + wL + eL + rL >= ObjectManager.Player.Level) return;
            int[] level = { 0, 0, 0, 0 };
            for (var i = 0; i < ObjectManager.Player.Level; i++)
            {
                level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
            }
            if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
            if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
            if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
        }

        private static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => MyMobs.MinionNames.Contains(a.BaseSkinName)
            && DamageLibrary.GetSummonerSpellDamage(Player, a, DamageLibrary.SummonerSpells.Smite) >= a.Health
            && FapturneMenu.MyActivator[a.BaseSkinName].Cast<CheckBox>().CurrentValue
            && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void Barrier()
        {
            if (MyActivator.Barrier.IsReady() && Player.HealthPercent <= FapturneMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= FapturneMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (MyActivator.Heal.IsReady() && Player.HealthPercent <= FapturneMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (FapturneMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) OnFlee();
            KillSteal();
            AutoCC();
            AutoPotions();
            AutoWard();
            MoreRangeToParanoia();
        }

        private static void AutoWard()
        {
            if (FapturneMenu.checkWard())
            {
                foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() && FapturneMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() && FapturneMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() && FapturneMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() && FapturneMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && FapturneMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (FapturneMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= FapturneMenu.SpellsPotionsHP() &&
                !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") ||
                  Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") ||
                  Player.HasBuff("ItemDarkCrystalFlask")))
            {
                if (MyActivator.HuntersPot.IsReady() && MyActivator.HuntersPot.IsOwned())
                {
                    MyActivator.HuntersPot.Cast();
                }
                if (MyActivator.CorruptPot.IsReady() && MyActivator.CorruptPot.IsOwned())
                {
                    MyActivator.CorruptPot.Cast();
                }
                if (MyActivator.Biscuit.IsReady() && MyActivator.Biscuit.IsOwned())
                {
                    MyActivator.Biscuit.Cast();
                }
                if (MyActivator.HPPot.IsReady() && MyActivator.HPPot.IsOwned())
                {
                    MyActivator.HPPot.Cast();
                }
                if (MyActivator.RefillPot.IsReady() && MyActivator.RefillPot.IsOwned())
                {
                    MyActivator.RefillPot.Cast();
                }
            }
            if (FapturneMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= FapturneMenu.SpellsPotionsM() &&
                !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") ||
                  Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") ||
                  Player.HasBuff("ItemDarkCrystalFlask")))
            {
                if (MyActivator.CorruptPot.IsReady() && MyActivator.CorruptPot.IsOwned())
                {
                    MyActivator.CorruptPot.Cast();
                }
            }
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.Buff.Type == BuffType.Taunt && FapturneMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && FapturneMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && FapturneMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && FapturneMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && FapturneMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && FapturneMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && FapturneMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && FapturneMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && FapturneMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && FapturneMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && FapturneMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && FapturneMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && FapturneMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && FapturneMenu.PoppyUlt())
            {
                UltQSS();
            }
        }
        private static void DoQSS()
        {
            if (MyActivator.Qss.IsOwned() && MyActivator.Qss.IsReady())
            {
                MyActivator.Qss.Cast();
            }

            if (MyActivator.Mercurial.IsOwned() && MyActivator.Mercurial.IsReady())
            {
                MyActivator.Mercurial.Cast();
            }
        }
        private static void UltQSS()
        {
            if (MyActivator.Qss.IsOwned() && MyActivator.Qss.IsReady())
            {
                MyActivator.Qss.Cast();
            }

            if (MyActivator.Mercurial.IsOwned() && MyActivator.Mercurial.IsReady())
            {
                MyActivator.Mercurial.Cast();
            }
        }

        private static bool IsWarded(Vector3 position)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(obj => obj.IsWard() && obj.Distance(position) <= 200);
        }

        private static void KillSteal()
        {

            foreach (
                var target in
                    EntityManager.Heroes.Enemies.Where(
                        hero =>
                            hero.IsValidTarget(R.Range) && !hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25))
            {

                var predQ = Q.GetPrediction(target);
                if (FapturneMenu.KillstealR() && R.IsReady() &&
                    target.Health + target.AttackShield + FapturneMenu.ComboR1() <= Player.GetSpellDamage(target, SpellSlot.R))
                {
                    R.Cast();
                    R1.Cast(target);
                }

                if (FapturneMenu.KillstealQ() && Q.IsReady() &&
                    target.Health + target.AttackShield + FapturneMenu.ComboQ1() <
                    Player.GetSpellDamage(target, SpellSlot.Q))
                {
                    if (predQ.HitChance >= HitChance.High)
                    {
                        Q.Cast(target.Position);
                    }
                }

                if (FapturneMenu.KillstealE() && E.IsReady() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.E))
                {
                    E.Cast(target);
                }
            }
        }

        private static void AutoCC()
        {
            var autoCCTarget =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x => x.IsValidTarget(Q.Range) &&
                        x.HasBuffOfType(BuffType.Charm) || x.HasBuffOfType(BuffType.Knockup) ||
                        x.HasBuffOfType(BuffType.Stun) || x.HasBuffOfType(BuffType.Suppression) ||
                        x.HasBuffOfType(BuffType.Snare)); ;
            if (autoCCTarget != null && autoCCTarget.IsValidTarget(Q.Range))
            {
                if (FapturneMenu.ComboCC() && Q.IsReady())
                {
                    Q.Cast(autoCCTarget.Position);
                }
                if (FapturneMenu.ComboCC1() && E.IsReady())
                {
                    E.Cast(autoCCTarget);
                }
            }
        }


        private static void OnFlee()
        {
            if (Q.IsReady() && Player.ManaPercent >= FapturneMenu.FleeM())
            {
                Q.Cast(Player.ServerPosition.Extend(Game.CursorPos, W.Range).To3D());
            }
        }

        private static void OnLaneClear()
        {
            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                    Player.AttackRange, false).Count();
            var source =
                            EntityManager.MinionsAndMonsters.GetLaneMinions().OrderBy(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(Q.Range));
            if (count == 0) return;
            if (E.IsReady() && FapturneMenu.LcE() &&
                Player.ManaPercent >= FapturneMenu.LcM())
            {
                E.Cast(source);
            }
            if (Q.IsReady() && Player.ManaPercent >= FapturneMenu.LcM() && FapturneMenu.LcQ1() <= count)
            {
                if (FapturneMenu.LcQ() && FapturneMenu.LcQ2() && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(source.Position);
                }

                if (Q.IsReady() && FapturneMenu.LcQ() && !FapturneMenu.LcQ2() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source.Position);
                }
            }
        }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderBy(a => a.MaxHealth)
                    .FirstOrDefault(a => a.IsValidTarget(Q.Range));

            if (source == null) return;
            if (Q.IsReady() && FapturneMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source.Position);
            }

            if (E.IsReady() && FapturneMenu.JungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source);
            }
        }

        private static void OnHarrass()
        {
            var enemiese = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
            {
                foreach (var eenemies in enemiese)
                {
                    var useE = FapturneMenu.MyHarass["harass.E"
                                                 + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            }

            if (target.IsValidTarget(Q.Range) && Player.ManaPercent >= FapturneMenu.HarassQE())
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = FapturneMenu.MyHarass["harass.Q"
                        + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        var predq = Q.GetPrediction(qenemies);
                        if (predq.HitChance >= HitChance.High)
                        {
                            Q.Cast(predq.CastPosition);
                        }
                    }
                }
            }
        }

        private static void OnCombo()
        {
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var enemiesr = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesw = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= W.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);


            if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                foreach (var ultenemies in enemiesr)
                {
                    var useR = FapturneMenu.MyCombo["combo.r"
                        + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useR)
                    {
                        R.Cast();
                        R1.Cast(ultenemies);
                    }
                }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = FapturneMenu.MyCombo["combo.e"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = FapturneMenu.MyCombo["combo.Q"
                        + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    var predQ = Q.GetPrediction(qenemies);
                    if (useQ && predQ.HitChancePercent >= FapturneMenu.ComboQH())
                    {
                        Q.Cast(predQ.CastPosition);
                    }
                }
            }

            if (Player.HealthPercent <= FapturneMenu.tiamatHP() && FapturneMenu.tiamat() && MyActivator.Tiamat.IsReady() && MyActivator.Tiamat.IsOwned() && MyActivator.Tiamat.IsInRange(target))
            {
                MyActivator.Tiamat.Cast();
                return;
            }
            if (Player.HealthPercent <= FapturneMenu.hydraHP() && FapturneMenu.hydra() && MyActivator.Hydra.IsReady() && MyActivator.Hydra.IsOwned() && MyActivator.Hydra.IsInRange(target))
            {
                MyActivator.Hydra.Cast();
                return;
            }

            if (Player.HealthPercent <= FapturneMenu.gunbladeHP() && FapturneMenu.gunblade() && MyActivator.Hextech.IsReady() && MyActivator.Hextech.IsOwned() && MyActivator.Hextech.IsInRange(target))
            {
                MyActivator.Hextech.Cast(target);
                return;
            }

            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 FapturneMenu.YoumusEnemies() || Player.HealthPercent >= FapturneMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && FapturneMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= FapturneMenu.BilgewaterHp() && FapturneMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= FapturneMenu.BotrkHp() && FapturneMenu.Botrk() && MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }
        }
    }
}
