using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace MissInopportune
{
    public static class Program
    {
        public static string Version = "1.0.4.1";
        public static AIHeroClient castOn = null;
        public static AIHeroClient Target = null;
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation _wardLocation;
        public static Spell.Targeted Q;
        public static Spell.Skillshot Q1;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        private static int time;
        private static int LastSeen;
        private static long LastUpdate;
        public static bool Channeling = false;
        public static bool Out = false;
        public static int CurrentSkin = 0;

        public static readonly AIHeroClient Player = ObjectManager.Player;


        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "MissFortune") return;
            AbilitySequence = new[] {1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
            Chat.Print("MissInopportune Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FLAME!", Color.Red);
            MissInopportuneMenu.LoadMenu();
            _wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;


            #region Skill

            Q = new Spell.Targeted(SpellSlot.Q, 650);
            Q1 = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Circular, 250,2000, 50);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 500, int.MaxValue, 200);
            R = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Cone, 0, int.MaxValue);
            R.ConeAngleDegrees = 30;

            #endregion

            Gapcloser.OnGapcloser += AntiGapCloser;
            Obj_AI_Base.OnProcessSpellCast += OnSpellCast;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Obj_AI_Base.OnBuffLose += OnBuffLose;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);

        }

        private static void GameOnDraw(EventArgs args)
        {
            if (MissInopportuneMenu.Nodraw()) return;

            if (!MissInopportuneMenu.OnlyReady())
            {
                if (MissInopportuneMenu.DrawingsQ())
                {
                    new Circle {Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (MissInopportuneMenu.DrawingsW())
                {
                    new Circle {Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (MissInopportuneMenu.DrawingsE())
                {
                    new Circle {Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (MissInopportuneMenu.DrawingsR())
                {
                    new Circle {Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (MissInopportuneMenu.DrawingsT() && _wardLocation.Normal.Any())
                {
                    foreach (
                        var place in
                            _wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && MissInopportuneMenu.DrawingsQ())
                {
                    new Circle {Color = Color.AliceBlue, Radius = 650, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && MissInopportuneMenu.DrawingsW())
                {
                    new Circle {Color = Color.OrangeRed, Radius = 500, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && MissInopportuneMenu.DrawingsE())
                {
                    new Circle {Color = Color.Cyan, Radius = 1000, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && MissInopportuneMenu.DrawingsR())
                {
                    new Circle {Color = Color.SkyBlue, Radius = 1400, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (MissInopportuneMenu.DrawingsT() && _wardLocation.Normal.Any())
                {
                    foreach (
                        var place in
                            _wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
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
            if (MissInopportuneMenu.checkSkin())
            {
                if (MissInopportuneMenu.SkinId() != CurrentSkin)
                {
                    Player.SetSkinId(MissInopportuneMenu.SkinId());
                    CurrentSkin = MissInopportuneMenu.SkinId();
                }
            }
        }

        private static void LevelUpSpells()
        {
            var qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            var wL = Player.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            var eL = Player.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            var rL = Player.Spellbook.GetSpell(SpellSlot.R).Level + ROff;
            if (qL + wL + eL + rL >= ObjectManager.Player.Level) return;
            int[] level = {0, 0, 0, 0};
            for (var i = 0; i < ObjectManager.Player.Level; i++)
            {
                level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
            }
            if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
            if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
            if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
        }

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !MissInopportuneMenu.GapcloserE() || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy || e.Sender.IsAlly && Channeling == true)
            {
                return;
            }

            E.Cast(e.Sender.ServerPosition);
        }

        private static void Barrier()
        {
            if (Player.IsFacing(Target) && MyActivator.Barrier.IsReady() &&
                Player.HealthPercent <= MissInopportuneMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= MissInopportuneMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (Player.IsFacing(Target) && MyActivator.Heal.IsReady() &&
                Player.HealthPercent <= MissInopportuneMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (MissInopportuneMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) OnFlee();
            KillSteal();
            AutoE();
            AutoPotions();
            AutoWard();
        }

        private static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "MissFortuneBulletSound")
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Out = true;
                Channeling = true;
            }
        }

        private static void AutoWard()
        {
            if (MissInopportuneMenu.checkWard())
            {
                foreach (
                    var place in _wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() &&
                        MissInopportuneMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() &&
                        MissInopportuneMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() &&
                        MissInopportuneMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() &&
                        MissInopportuneMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() &&
                        MissInopportuneMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (MissInopportuneMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= MissInopportuneMenu.SpellsPotionsHP() &&
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
            if (MissInopportuneMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= MissInopportuneMenu.SpellsPotionsM() &&
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

            if (args.Buff.Type == BuffType.Taunt && MissInopportuneMenu.Taunt())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Stun && MissInopportuneMenu.Stun())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Snare && MissInopportuneMenu.Snare())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Polymorph && MissInopportuneMenu.Polymorph())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Blind && MissInopportuneMenu.Blind())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Flee && MissInopportuneMenu.Fear())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Charm && MissInopportuneMenu.Charm())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Suppression && MissInopportuneMenu.Suppression())
            {
                DoQss();
            }
            if (args.Buff.Type == BuffType.Silence && MissInopportuneMenu.Silence())
            {
                DoQss();
            }
            if (args.Buff.Name == "zedulttargetmark" && MissInopportuneMenu.ZedUlt())
            {
                UltQss();
            }
            if (args.Buff.Name == "VladimirHemoplague" && MissInopportuneMenu.VladUlt())
            {
                UltQss();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && MissInopportuneMenu.FizzUlt())
            {
                UltQss();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && MissInopportuneMenu.MordUlt())
            {
                UltQss();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && MissInopportuneMenu.PoppyUlt())
            {
                UltQss();
            }
        }

        private static void DoQss()
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

        private static void UltQss()
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

        private static void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (sender.IsMe && args.Buff.DisplayName == "MissFortuneBulletSound")
            {
                Channeling = false;
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
                Out = false;
            }
        }

        private static bool IsWarded(Vector3 position)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(obj => obj.IsWard() && obj.Distance(position) <= 200);
        }

        private static void KillSteal()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent)
                .Where(
                    a =>
                        !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range && !a.IsDead && !a.IsZombie &&
                        a.HealthPercent <= 25);
            foreach (
                var target2 in
                    enemies)
            {
                if (!target2.IsValidTarget() && Channeling == true)
            {
                return;
            }

                if (MissInopportuneMenu.KillstealQ() && Q.IsReady() &&
                    target2.Health + target2.AttackShield <
                    Player.GetSpellDamage(target2, SpellSlot.Q))

                {
                    Q.Cast(target2);
                }

                if (MissInopportuneMenu.KillstealE() && E.IsReady() &&
                    target2.Health + target2.AttackShield <
                    Player.GetSpellDamage(target2, SpellSlot.E) && Player.Mana >= 80)
                {
                    E.Cast(target2.ServerPosition);
                }
            }

        }

        private static void AutoE()
        {
            if (!MissInopportuneMenu.MyCombo["combo.CC"].Cast<CheckBox>().CurrentValue && Channeling == true)
            {
                return;
            }
            var autoETarget =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x =>
                        x.HasBuffOfType(BuffType.Charm) || x.HasBuffOfType(BuffType.Knockup) ||
                        x.HasBuffOfType(BuffType.Stun) || x.HasBuffOfType(BuffType.Suppression) ||
                        x.HasBuffOfType(BuffType.Snare));
            if (autoETarget != null && Channeling == true)
            {
                E.Cast(autoETarget.ServerPosition);
            }
        }

        private static void OnFlee()
        {
            if (W.IsReady() && Player.ManaPercent >= MissInopportuneMenu.FleeM())
            {
                W.Cast();
            }
            if (E.IsReady() && Player.HealthPercent <= MissInopportuneMenu.FleeHP())
            {
                E.Cast(Target.ServerPosition);
            }
        }

        public static void CastExtendedQ()
        {  var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;
           /* var t = TargetSelector.SelectedTarget != null &&
                    TargetSelector.SelectedTarget.Distance(ObjectManager.Player) < Q1.Range
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(Q1.Range, DamageType.Physical);

            if (!t.IsValidTarget(Q1.Range) ||
                t.IsZombie ||
                Orbwalker.IsAutoAttacking ||
                t.HasBuffOfType(BuffType.Invulnerability))
                return;
            {
                var q1Pred = Q1.GetPrediction(t);
                var q1Col = q1Pred.CollisionObjects;
                if (!q1Col.Any()) return;
                {
                    var qMinion = q1Col.Last();
                    if (!qMinion.IsValidTarget(Q.Range)) return;
                    {
                        if (qMinion.Distance(q1Pred.CastPosition) < 380 &&
                            qMinion.Distance(t.Position) < 380)
                        {
                            Q.Cast(qMinion);
                        }
                    }
                }
            }*/
              if (Q.IsReady() && target.IsValidTarget(Q1.Range))
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions().OrderByDescending(m => m.Health)
                        .FirstOrDefault(m => m.IsValidTarget(Q.Range) && m.Health <= Q.GetRealDamage(m));
                if (minionQ != null)
                {
                    const float rad = (float)Math.PI / 180f;

                    var cone = new Geometry.Polygon.Sector(target.ServerPosition, target.ServerPosition.Extend(Player, -400).To3D(), 60f * rad, 250f);

                    if (cone.IsInside(target))
                    {
                        Q.Cast(minionQ);
                    }
                }
            }
        }

        private static void OnLaneClear()
        {
            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                    Player.AttackRange, false).Count();

            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                    Player.AttackRange).OrderBy(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (E.IsReady() && MissInopportuneMenu.LcE() && source.IsValidTarget(E.Range) &&
                Player.ManaPercent >= MissInopportuneMenu.LcM() && MissInopportuneMenu.LcE1() <= count)
            {
                E.Cast(source.Position);
            }

            if (Q.IsReady() && Player.ManaPercent >= MissInopportuneMenu.LcM())
            {
                if (MissInopportuneMenu.LcQ() && MissInopportuneMenu.LcQ1() && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(source);
                }

                if (Q.IsReady() && MissInopportuneMenu.LcQ() && !MissInopportuneMenu.LcQ1() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source);
                }
            }

            if (W.IsReady() && MissInopportuneMenu.LcW() &&
                Player.ManaPercent >= MissInopportuneMenu.LcM())
            {
                W.Cast();
            }

        }
        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();
            if (source == null) return;
            if (Q.IsReady() && MissInopportuneMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
            }
            if (W.IsReady() && MissInopportuneMenu.JungleW() && source.Distance(Player) <= W.Range)
            {
                W.Cast();
            }
            if (E.IsReady() && MissInopportuneMenu.JungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source.Position);
            }
        }

        private static void OnHarrass()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (!target.IsValidTarget() && Channeling == true)
            {
                return;
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = MissInopportuneMenu.MyHarass["harass.E"
                                                           + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies.ServerPosition);
                    }
                }

            if (MissInopportuneMenu.HarassQ() &&
                Player.ManaPercent >= MissInopportuneMenu.HarassQe())
            {
                if (MissInopportuneMenu.HarassQ() && target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
                if (MissInopportuneMenu.HarassQ1() && target.IsValidTarget(Q1.Range))
                {
                    CastExtendedQ();
                }
            }
        }

        private static void OnCombo()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1400, DamageType.Physical);
            if (!target.IsValidTarget(Q.Range) || target == null && Channeling == true)
            {
                return;
            }
            if (MissInopportuneMenu.ComboW() && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsInvulnerable &&
                target.Position.CountEnemiesInRange(800) >= MissInopportuneMenu.ComboW1())
            {
                W.Cast();
            }
            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = MissInopportuneMenu.MyCombo["combo.e"
                                                           + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies.ServerPosition);
                    }
                }
           
            if (Q.IsReady() && target.IsValidTarget(Q.Range) && MissInopportuneMenu.ComboQ())
                    {
                        Q.Cast(target);
                    }

                    if (Q.IsReady() && target.IsValidTarget(Q1.Range) && MissInopportuneMenu.ComboQ1())
                        {
                            CastExtendedQ();
                        }

            
            if (MissInopportuneMenu.ComboR() && R.IsReady() && !target.IsInvulnerable)
            {

                if (target != null)
                {
                    int collCount = 1;
                    foreach (var e in enemies)
                    {
                        if (
                            Math.Abs(e.Position.AngleBetween(Player.ServerPosition) -
                                     target.ServerPosition.AngleBetween(Player.Position)) < 0.3)
                        {
                            collCount++;
                        }
                    }
                    if (MissInopportuneMenu.ComboR1() &&
                        (target.HasBuffOfType(BuffType.Suppression) || target.HasBuffOfType(BuffType.Charm) ||
                         target.HasBuffOfType(BuffType.Flee)
                         || target.HasBuffOfType(BuffType.Blind) || target.HasBuffOfType(BuffType.Polymorph) ||
                         target.HasBuffOfType(BuffType.Snare)
                         || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Taunt)))
                    {
                        if (collCount < MissInopportuneMenu.ComboREnemies())
                        {
                            R.Cast(target.Position);
                        }
                    }
                    else if (!MissInopportuneMenu.ComboR1() && collCount <= MissInopportuneMenu.ComboREnemies())
                    {
                        R.Cast(R.GetPrediction(target).CastPosition);
                    }
                }
         
            }
            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 MissInopportuneMenu.YoumusEnemies() || Player.HealthPercent >= MissInopportuneMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && MissInopportuneMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= MissInopportuneMenu.BilgewaterHp() && MissInopportuneMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= MissInopportuneMenu.BotrkHp() && MissInopportuneMenu.Botrk() &&
                MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }
                       

            }
    }
    }
