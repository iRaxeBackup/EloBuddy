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

namespace Fappadred
{
    public static class Program
    {
        public static string Version = "2.2.5.6";
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        private static SoundPlayer allahuAkbar;
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        public static int time;
        public static readonly AIHeroClient Player = ObjectManager.Player;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Kindred") return;
            AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
            Chat.Print("Fappadred Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT AFK!", Color.Red);
            FappadredMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            Gapcloser.OnGapcloser += AntiGapCloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            ProcessSpells.Initialize();

            #region Skill

            Q = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 500);
            R = new Spell.Targeted(SpellSlot.R, 500);
            #endregion
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly || args.SData.IsAutoAttack()) return;
            var articunoPerfectCheck = Player.Position.PointOnLineSegment(args.Start,
                args.Start.Extend(args.End, args.SData.CastRangeDisplayOverride).To3D());
            if (ProcessSpells.DB.Contains(args.SData.Name) &&
                R.IsReady() && Player.HealthPercent <= FappadredMenu.UltiHP() && FappadredMenu.ComboR() &&
                (articunoPerfectCheck || (args.Target != null && args.Target.IsMe)))
            {
                R.Cast(Player);
            }
        }

        public static bool PointOnLineSegment(this Vector3 CheckPoint, Vector3 Start, Vector3 End)
        {
            return (Start.X <= CheckPoint.X && CheckPoint.X <= End.X || End.X <= CheckPoint.X && CheckPoint.X <= Start.X) &&
                   (Start.Y <= CheckPoint.Y && CheckPoint.Y <= End.Y || End.Y <= CheckPoint.Y && CheckPoint.Y <= Start.Y);
        }

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            if (E.IsReady() && E.IsInRange(sender) && FappadredMenu.gapcloserE())
            {
                E.Cast(sender);
            }
            if (W.IsReady() && W.IsInRange(sender) && FappadredMenu.gapcloserW())
            {
                W.Cast();
            }
        }
        private static void GameOnDraw(EventArgs args)
        {
            if (FappadredMenu.Nodraw()) return;

            if (!FappadredMenu.OnlyReady())
            {
                if (FappadredMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FappadredMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FappadredMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FappadredMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FappadredMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && FappadredMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && FappadredMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && FappadredMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && FappadredMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (FappadredMenu.DrawingsT() && wardLocation.Normal.Any())
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
            if (FappadredMenu.checkSkin())
            {
                Player.SetSkinId(FappadredMenu.SkinId());
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
            && FappadredMenu.MyActivator[a.BaseSkinName].Cast<CheckBox>().CurrentValue
            && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void Barrier()
        {
            if (MyActivator.Barrier.IsReady() && Player.HealthPercent <= FappadredMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= FappadredMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (MyActivator.Heal.IsReady() && Player.HealthPercent <= FappadredMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (FappadredMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) OnFlee();
            KillSteal();
            AutoCC();
            AutoPotions();
            AutoWard();
        }

        private static void AutoWard()
        {
            if (FappadredMenu.checkWard())
            {
                foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() && FappadredMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() && FappadredMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() && FappadredMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() && FappadredMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && FappadredMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (FappadredMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= FappadredMenu.SpellsPotionsHP() &&
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
            if (FappadredMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= FappadredMenu.SpellsPotionsM() &&
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

            if (args.Buff.Type == BuffType.Taunt && FappadredMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && FappadredMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && FappadredMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && FappadredMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && FappadredMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && FappadredMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && FappadredMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && FappadredMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && FappadredMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && FappadredMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && FappadredMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && FappadredMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && FappadredMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && FappadredMenu.PoppyUlt())
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

                if (FappadredMenu.KillstealQ() && Q.IsReady() &&
                    target.Health + target.AttackShield + FappadredMenu.ComboQ1() <
                    Player.GetSpellDamage(target, SpellSlot.Q))
                {
                        Q.Cast(Game.CursorPos);
                }

                if (FappadredMenu.KillstealE() && E.IsReady() &&
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
                if (FappadredMenu.ComboCC() && Q.IsReady())
                {
                    Q.Cast(autoCCTarget.Position);
                }
                if (FappadredMenu.ComboCC1() && E.IsReady())
                {
                    E.Cast(autoCCTarget);
                }
            }
        }


        private static void OnFlee()
        {
            if (W.IsReady() && Player.ManaPercent >= FappadredMenu.FleeM())
            {
                W.Cast();
            }

            if (Q.IsReady() && Player.ManaPercent >= FappadredMenu.FleeM())
            {
                Q.Cast(Player.ServerPosition.Extend(Game.CursorPos, Q.Range).To3D());
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
            if (source == null) return;
            if (E.IsReady() && FappadredMenu.LcE() &&
                Player.ManaPercent >= FappadredMenu.LcM())
            {
                E.Cast(source);
            }
            if (W.IsReady() && FappadredMenu.LcW() &&
                Player.ManaPercent >= FappadredMenu.LcM() 
                && FappadredMenu.LcW1() <= count)
            {
                W.Cast();
            }
            if (Q.IsReady() && Player.ManaPercent >= FappadredMenu.LcM() 
                && FappadredMenu.LcQ1() <= count)
            {
                if (FappadredMenu.LcQ() && FappadredMenu.LcQ2() && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(Game.CursorPos);
                }

                if (Q.IsReady() && FappadredMenu.LcQ() && !FappadredMenu.LcQ2() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(Game.CursorPos);
                }
            }
        }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();

            if (source == null) return;
            
            if (E.IsReady() && FappadredMenu.JungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source);
            }
            if (W.IsReady() && FappadredMenu.JungleW() && source.Distance(Player) <= W.Range)
            {
                W.Cast();
            }
            if (Q.IsReady() && FappadredMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(Game.CursorPos);
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
                    var useE = FappadredMenu.MyHarass["harass.E"
                                                 + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            }

            if (target.IsValidTarget(Q.Range) && Player.ManaPercent >= FappadredMenu.HarassQE())
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = FappadredMenu.MyHarass["harass.Q"
                        + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                            Q.Cast(Game.CursorPos);
                    }
                }
            }
        }

        private static void OnCombo()
        {
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = FappadredMenu.MyCombo["combo.e"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }

            if (W.IsReady() && FappadredMenu.ComboW() && target.Distance(Player) <= W.Range)
            {
                W.Cast();
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = FappadredMenu.MyCombo["combo.Q"
                        + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }
            }

            if (Player.HealthPercent <= FappadredMenu.gunbladeHP() && FappadredMenu.gunblade() && MyActivator.Hextech.IsReady() && MyActivator.Hextech.IsOwned() && MyActivator.Hextech.IsInRange(target))
            {
                MyActivator.Hextech.Cast(target);
                return;
            }

            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 FappadredMenu.YoumusEnemies() || Player.HealthPercent >= FappadredMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && FappadredMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= FappadredMenu.BilgewaterHp() && FappadredMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= FappadredMenu.BotrkHp() && FappadredMenu.Botrk() && MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }
        }
    }
}
