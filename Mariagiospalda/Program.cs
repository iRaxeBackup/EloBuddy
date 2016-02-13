using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Mariagiospalda
{
    public static class Program
    {
        public static string Version = "1.9.1.5";
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        private static SoundPlayer allahuAkbar; 
        public static Spell.Skillshot Q;
        public static Spell.Skillshot Q1;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active E1;
        public static Spell.Targeted R;
        public static int time;
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static bool fleeing;
        public static Vector2 MissilePosition;
        public static MissileClient PiuwPiuwMissile;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Lissandra") return;
            AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
            Chat.Print("Mariagiospalda Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT AFK!", Color.Red);
            MariagiospaldaMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;

        #region Skill

            Q = new Spell.Skillshot(SpellSlot.Q, 725, SkillShotType.Linear);
            Q1 = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 450);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear);
            E1 = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 550);

            #endregion
            }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }
            if (R.IsReady() && R.IsInRange(sender) && MariagiospaldaMenu.interruptR())
            {
                R.Cast(sender);
            }
        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            if (R.IsReady() && R.IsInRange(sender) && MariagiospaldaMenu.gapcloserR())
            {
                R.Cast(sender);
            }
            if (W.IsReady() && W.IsInRange(sender) && MariagiospaldaMenu.gapcloserW())
            {
                W.Cast();
            }
        }
        private static void GameOnDraw(EventArgs args)
        {
            if (MariagiospaldaMenu.Nodraw()) return;

            if (!MariagiospaldaMenu.OnlyReady())
            {
                if (MariagiospaldaMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (MariagiospaldaMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (MariagiospaldaMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (MariagiospaldaMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (MariagiospaldaMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && MariagiospaldaMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = 725, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && MariagiospaldaMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = 450, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && MariagiospaldaMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = 1050, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && MariagiospaldaMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = 550, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (MariagiospaldaMenu.DrawingsT() && wardLocation.Normal.Any())
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
            if (MyActivator.Barrier != null)
                Barrier();
            if (MyActivator.Heal != null)
                Heal();
            if (MyActivator.Ignite != null)
                Ignite();
            if (MariagiospaldaMenu.checkSkin())
            {
                Player.SetSkinId(MariagiospaldaMenu.SkinId());
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

        private static void Barrier()
        {
            if (MyActivator.Barrier.IsReady() && Player.HealthPercent <= MariagiospaldaMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= MariagiospaldaMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (MyActivator.Heal.IsReady() && Player.HealthPercent <= MariagiospaldaMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (MariagiospaldaMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) OnFlee();
            KillSteal();
            AutoCC();
            AutoPotions();
            AutoWard();
            MonitorMissilePosition();
        }

        private static void AutoWard()
        {
            if (MariagiospaldaMenu.checkWard())
            {
                foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() && MariagiospaldaMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() && MariagiospaldaMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() && MariagiospaldaMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() && MariagiospaldaMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && MariagiospaldaMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (MariagiospaldaMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= MariagiospaldaMenu.SpellsPotionsHP() &&
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
            if (MariagiospaldaMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= MariagiospaldaMenu.SpellsPotionsM() &&
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

            if (args.Buff.Type == BuffType.Taunt && MariagiospaldaMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && MariagiospaldaMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && MariagiospaldaMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && MariagiospaldaMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && MariagiospaldaMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && MariagiospaldaMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && MariagiospaldaMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && MariagiospaldaMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && MariagiospaldaMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && MariagiospaldaMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && MariagiospaldaMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && MariagiospaldaMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && MariagiospaldaMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && MariagiospaldaMenu.PoppyUlt())
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

        public static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var piuwpiuw = sender as MissileClient;
            if (piuwpiuw != null && piuwpiuw.IsValid)
            {
                if (piuwpiuw.SpellCaster.IsMe && piuwpiuw.SData.Name == "LissandraEMissile")
                {
                    PiuwPiuwMissile = piuwpiuw;
                }
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            var piuwpiuw = sender as MissileClient;
            if (piuwpiuw == null || !piuwpiuw.IsValid) return;
            if (piuwpiuw.SpellCaster.IsMe && piuwpiuw.SpellCaster.IsValid && piuwpiuw.SData.Name == "LissandraEMissile")
            {
                PiuwPiuwMissile = null;
                MissilePosition = new Vector2(0, 0);
            }
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
                var predE = E.GetPrediction(target);
                if (MariagiospaldaMenu.KillstealR() && R.IsReady() &&
                    target.Health + target.AttackShield + MariagiospaldaMenu.ComboR1() <= Player.GetSpellDamage(target, SpellSlot.R))
                {
                        R.Cast(target);
                }

                if (MariagiospaldaMenu.KillstealW() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.W) && Player.Mana >= 100)
                {
                    if (W.IsReady() && target.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }
                }

                if (MariagiospaldaMenu.KillstealQ() && Q.IsReady() &&
                    target.Health + target.AttackShield + MariagiospaldaMenu.ComboQ1() <
                    Player.GetSpellDamage(target, SpellSlot.Q))
                {
                    if (predQ.HitChance >= HitChance.High)
                    {
                        Q.Cast(target.Position);
                    }
                }

                if (MariagiospaldaMenu.KillstealE() && E.IsReady() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.E) && PiuwPiuwMissile == null 
                    && !MariagiospaldaMenu.ComboE2())
                {
                    if (predE.HitChance >= HitChance.High)
                    {
                        E.Cast(predE.CastPosition);
                    }
                }
            }
        }

     static void ExtendedQ()
    {
    var target = TargetSelector.GetTarget(825, DamageType.Magical);
    var QPred = Q1.GetPrediction(target);

    if ( QPred.CollisionObjects.Any( it => it.IsValidTarget(725) && it.Distance(target) <= 125) ) Q.Cast(target);

    return;
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
                if (MariagiospaldaMenu.ComboCC() && Q.IsReady())
                {
                    Q.Cast(autoCCTarget.Position);
                }
                if (MariagiospaldaMenu.ComboCC1() && E.IsReady() && PiuwPiuwMissile == null )
                {
                        E.Cast(autoCCTarget.Position);
                }
            }
        }


        private static void OnFlee()
        {
            if (E.IsReady() && Player.ManaPercent >= MariagiospaldaMenu.FleeM() && PiuwPiuwMissile == null && !MariagiospaldaMenu.ComboE2())
            {
                E.Cast(Player.ServerPosition.Extend(Game.CursorPos, W.Range).To3D());
                fleeing = true;
            }
        }
        private static void MonitorMissilePosition()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (PiuwPiuwMissile == null || Player.IsDead)
            {
                return;
            }

            MissilePosition = PiuwPiuwMissile.Position.To2D();
            if (fleeing)
            {
                if (Vector2.Distance(MissilePosition, PiuwPiuwMissile.EndPosition.To2D()) < 40)
                {
                    E.Cast(target);
                    fleeing = false;
                }
                Core.DelayAction(() => fleeing = false, 2000);
            }

        }

        private static void CastE()
        {
            if (PiuwPiuwMissile == null && !MariagiospaldaMenu.ComboE2())
            {
                var Pred =
                    EntityManager.Heroes.Enemies.Where(
                        h =>
                            h.IsValidTarget() && !h.IsZombie &&
                            Vector3.Distance(h.ServerPosition, Player.ServerPosition) <= E.Range)
                        .Select(hero => E.GetPrediction(hero));

                var BestLocation = Pred.FirstOrDefault();
                if (BestLocation.HitChance >= HitChance.Medium && E.IsReady())
                {
                    E.Cast(BestLocation.CastPosition);
                }
            }
            E2();
        }

        private static void E2()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (MariagiospaldaMenu.ComboE2() && Player.HealthPercent > 25 && PiuwPiuwMissile != null && E.IsReady())
            {
                if (Vector2.Distance(MissilePosition, target.ServerPosition.To2D()) < Vector3.Distance(Player.ServerPosition, target.ServerPosition) && Vector3.Distance(target.ServerPosition, PiuwPiuwMissile.EndPosition) > Vector3.Distance(Player.ServerPosition, target.ServerPosition))
                {
                    E1.Cast();
                    return;
                }
                var EndingCount = PiuwPiuwMissile.Position.CountEnemiesInRange(R.Range);

                if (EndingCount >= MariagiospaldaMenu.ComboE1() && R.IsReady() && Vector3.Distance(PiuwPiuwMissile.Position, target.ServerPosition) < Vector3.Distance(Player.ServerPosition, target.ServerPosition))
                {
                    E1.Cast();
                    return;
                }
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
            if (E.IsReady() && MariagiospaldaMenu.LcE() && MariagiospaldaMenu.LcE1() <= count &&
                Player.ManaPercent >= MariagiospaldaMenu.LcM() && PiuwPiuwMissile == null)
            {
                E.Cast(source.Position);
            }
            if (Q.IsReady() && Player.ManaPercent >= MariagiospaldaMenu.LcM() && MariagiospaldaMenu.LcQ1() <= count)
            {
                if (MariagiospaldaMenu.LcQ() && MariagiospaldaMenu.LcQ2() && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(source.Position);
                }

                if (Q.IsReady() && MariagiospaldaMenu.LcQ() && !MariagiospaldaMenu.LcQ2() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source.Position);
                }
            }

            if (W.IsReady()&& MariagiospaldaMenu.LcW() && MariagiospaldaMenu.LcW1() <= count &&
                Player.ManaPercent >= MariagiospaldaMenu.LcM())
                { 
                    W.Cast();
                }

        }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderBy(a => a.MaxHealth)
                    .FirstOrDefault(a => a.IsValidTarget(Q.Range));

            if (source == null) return;
            if (Q.IsReady() && MariagiospaldaMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
            }

            if (W.IsReady() && MariagiospaldaMenu.JungleW() && source.Distance(Player) <= W.Range)
            {
                    W.Cast();
            }

            if (E.IsReady() && MariagiospaldaMenu.JungleE() && source.Distance(Player) <= E.Range)
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

            if (E.IsReady() && target.IsValidTarget(E.Range) && PiuwPiuwMissile == null)
            {
                foreach (var eenemies in enemiese)
                {
                    var useE = MariagiospaldaMenu.MyHarass["harass.E"
                                                 + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    var prede = E.GetPrediction(eenemies);
                    if (useE)
                    {
                        if (prede.HitChance >= HitChance.High)
                        {
                            E.Cast(eenemies);
                        }
                    }
                }
            }

            if (Q.IsReady() && target.IsValidTarget(Q1.Range))
            {
                ExtendedQ();
            }

            if (target.IsValidTarget(Q.Range) && Player.ManaPercent >= MariagiospaldaMenu.HarassQE())
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = MariagiospaldaMenu.MyHarass["harass.Q"
                                                           + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        {
                            var predq = Q.GetPrediction(target);
                            if (predq.HitChance >= HitChance.High)
                            {
                                Q.Cast(predq.CastPosition);
                            }
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


            if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable && target.CountEnemiesInRange(R.Range) <= MariagiospaldaMenu.ComboR2())
                foreach (var ultenemies in enemiesr)
                {
                    var useR = MariagiospaldaMenu.MyCombo["combo.r"
                        + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useR)
                    {
                        R.Cast(ultenemies);
                    }
                }

            if (R.IsReady() && target.IsValidTarget(R.Range) && target.CountEnemiesInRange(R.Range) <= MariagiospaldaMenu.ComboR2())
                    {
                        R.Cast(Player);
                    }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = MariagiospaldaMenu.MyCombo["combo.e"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        CastE();
                    }
                }

            if (Q.IsReady() && target.IsValidTarget(Q1.Range))
            {
                ExtendedQ();
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = MariagiospaldaMenu.MyCombo["combo.Q"
                                                          + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    var predQ = Q.GetPrediction(qenemies);
                    if (useQ)
                        if (predQ.HitChancePercent >= MariagiospaldaMenu.ComboQH())
                        {
                            {
                                Q.Cast(predQ.CastPosition);
                            }
                        }
                }
            }

            if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                foreach (var jumpenemies in enemiesw)
                {
                    var useW = MariagiospaldaMenu.MyCombo["combo.w"
                                                 + jumpenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                    if (useW)
                    {
                        W.Cast();
                    }

                }
            }



            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 MariagiospaldaMenu.YoumusEnemies() || Player.HealthPercent >= MariagiospaldaMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && MariagiospaldaMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= MariagiospaldaMenu.BilgewaterHp() && MariagiospaldaMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= MariagiospaldaMenu.BotrkHp() && MariagiospaldaMenu.Botrk() && MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }

            if (Player.HealthPercent <= MariagiospaldaMenu.ZhonyaHP() && MariagiospaldaMenu.Zhonya() 
                && Player.CountEnemiesInRange(800) >= MariagiospaldaMenu.ZhonyaEnemies() && MyActivator.Zhonya.IsReady() && MyActivator.Zhonya.IsOwned())
            {
                MyActivator.Zhonya.Cast();
            }
        }
    }
}
