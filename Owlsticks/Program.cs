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

namespace Owlsticks
{
    public static class Program
    {
        public static string Version = "1.1.5.6";
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        public static Spell.Targeted Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Skillshot E1;
        public static Spell.Skillshot R;
        public static int time;
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static bool Healing;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "FiddleSticks") return; 
            AbilitySequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
            Chat.Print("Owlsticks Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT AFK!", Color.Red);
            OwlsticksMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnBuffLose += OnBuffLose;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;


            #region Skill

            Q = new Spell.Targeted(SpellSlot.Q, 575);
            W = new Spell.Targeted(SpellSlot.W, 575);
            E = new Spell.Targeted(SpellSlot.E, 750);
            E1 = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 880, SkillShotType.Circular);

            #endregion
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "Drain" )
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Healing = true;
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }
            if (Q.IsReady() && Q.IsInRange(sender) && OwlsticksMenu.interruptQ())
            {
                Q.Cast(sender);
            }
        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            if (E.IsReady() && E.IsInRange(sender) && OwlsticksMenu.gapcloserE())
            {
                E.Cast(sender);
            }
            if (Q.IsReady() && Q.IsInRange(sender) && OwlsticksMenu.gapcloserQ())
            {
                Q.Cast(sender);
            }
        }
        private static void GameOnDraw(EventArgs args)
        {
            if (OwlsticksMenu.Nodraw()) return;

            if (!OwlsticksMenu.OnlyReady())
            {
                if (OwlsticksMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (OwlsticksMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (OwlsticksMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (OwlsticksMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (OwlsticksMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && OwlsticksMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = 575, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && OwlsticksMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = 575, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && OwlsticksMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = 750, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && OwlsticksMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (OwlsticksMenu.DrawingsT() && wardLocation.Normal.Any())
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
            if (OwlsticksMenu.checkSkin())
            {
                Player.SetSkinId(OwlsticksMenu.SkinId());
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
            && OwlsticksMenu.MyActivator[a.BaseSkinName].Cast<CheckBox>().CurrentValue
            && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void Barrier()
        {
            if (MyActivator.Barrier.IsReady() && Player.HealthPercent <= OwlsticksMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= OwlsticksMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (MyActivator.Heal.IsReady() && Player.HealthPercent <= OwlsticksMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (OwlsticksMenu.Lvlup()) LevelUpSpells();
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
            if (OwlsticksMenu.checkWard())
            {
                foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() && OwlsticksMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() && OwlsticksMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() && OwlsticksMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() && OwlsticksMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && OwlsticksMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (OwlsticksMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= OwlsticksMenu.SpellsPotionsHP() &&
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
            if (OwlsticksMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= OwlsticksMenu.SpellsPotionsM() &&
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

            if (args.Buff.Type == BuffType.Taunt && OwlsticksMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && OwlsticksMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && OwlsticksMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && OwlsticksMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && OwlsticksMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && OwlsticksMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && OwlsticksMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && OwlsticksMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && OwlsticksMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && OwlsticksMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && OwlsticksMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && OwlsticksMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && OwlsticksMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && OwlsticksMenu.PoppyUlt())
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

        private static void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (sender.IsMe && args.Buff.DisplayName == "Drain")
            {
                Healing = false;
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
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

                if (OwlsticksMenu.KillstealR() && R.IsReady() &&
                    target.Health + target.AttackShield + OwlsticksMenu.ComboR1() <= Player.GetSpellDamage(target, SpellSlot.R))
                {
                    R.Cast(target.Position);
                }

                if (OwlsticksMenu.KillstealW() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.W) && Player.Mana >= 100)
                {
                    if (W.IsReady() && target.IsValidTarget(W.Range))
                    {
                        W.Cast(target);
                    }
                }

                if (OwlsticksMenu.KillstealE() && E.IsReady() &&
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
                if (OwlsticksMenu.ComboCC() && Q.IsReady())
                {
                    Q.Cast(autoCCTarget);
                }
                if (OwlsticksMenu.ComboCC2() && E.IsReady())
                {
                    E.Cast(autoCCTarget);
                }
                if (OwlsticksMenu.ComboCC1() && W.IsReady())
                {
                    W.Cast(autoCCTarget);
                }
            }
        }


        private static void OnFlee()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Q.IsReady() && Player.ManaPercent >= OwlsticksMenu.FleeM())
            {
                Q.Cast(target);
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
            if (E.IsReady() && OwlsticksMenu.LcE() && OwlsticksMenu.LcE1() <= count &&
                Player.ManaPercent >= OwlsticksMenu.LcM())
            {
                E.Cast(source);
            }
            if (Q.IsReady() && Player.ManaPercent >= OwlsticksMenu.LcM())
            {
                if (OwlsticksMenu.LcQ() && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(source);
                }

                if (Q.IsReady() && OwlsticksMenu.LcQ() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source);
                }
            }

            if (W.IsReady() && OwlsticksMenu.LcW() &&
                Player.ManaPercent >= OwlsticksMenu.LcM())
            {
                W.Cast(source);
            }

        }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderBy(a => a.MaxHealth)
                    .FirstOrDefault(a => a.IsValidTarget(Q.Range));

            if (source == null) return;
            if (Q.IsReady() && OwlsticksMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
            }

            if (W.IsReady() && OwlsticksMenu.JungleW() && source.Distance(Player) <= W.Range)
            {
                W.Cast(source);
            }

            if (E.IsReady() && OwlsticksMenu.JungleE() && source.Distance(Player) <= E.Range)
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
                    var useE = OwlsticksMenu.MyHarass["harass.E"
                                                 + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            }

            if (target.IsValidTarget(Q.Range) && Player.ManaPercent >= OwlsticksMenu.HarassQE())
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = OwlsticksMenu.MyHarass["harass.Q"
                                                           + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        {
                            Q.Cast(qenemies);
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


            if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable && target.CountEnemiesInRange(R.Range) <= OwlsticksMenu.ComboR2())
                foreach (var ultenemies in enemiesr)
                {
                    var useR = OwlsticksMenu.MyCombo["combo.r"
                        + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useR)
                    {
                        R.Cast(ultenemies.Position);
                    }
                }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = OwlsticksMenu.MyCombo["combo.e"
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
                    var useQ = OwlsticksMenu.MyCombo["combo.Q"
                                                          + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        Q.Cast(qenemies);
                    }
                }
            }

            if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                foreach (var healenemies in enemiesw)
                {
                    var useW = OwlsticksMenu.MyCombo["combo.w"
                                                 + healenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                    if (useW)
                    {
                        W.Cast(healenemies);
                    }

                }
            }

            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 OwlsticksMenu.YoumusEnemies() || Player.HealthPercent >= OwlsticksMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && OwlsticksMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= OwlsticksMenu.BilgewaterHp() && OwlsticksMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= OwlsticksMenu.BotrkHp() && OwlsticksMenu.Botrk() && MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }

            if (Player.HealthPercent <= OwlsticksMenu.ZhonyaHP() && OwlsticksMenu.Zhonya()
                && Player.CountEnemiesInRange(800) >= OwlsticksMenu.ZhonyaEnemies() && MyActivator.Zhonya.IsReady() && MyActivator.Zhonya.IsOwned())
            {
                MyActivator.Zhonya.Cast();
            }
        }

    }
}
