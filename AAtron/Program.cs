using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace AAtron
{
    public static class Program
    {
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static string version = "1.5.0.0";
        private static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        private static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot E;
        public static Spell.Active W;
        public static Spell.Active R;
        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Aatrox") return;
            AbilitySequence = new int[] { 3, 2, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
            Chat.Print("AAtron Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT BLAME!", Color.Red);
            AatroxMenu.loadMenu();
            Game.OnTick += GameOnTick;
            MyActivator.loadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Circular, (int)0.6f, 250, 2000);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Linear, (int)0.25f, 35, 1250);
            R = new Spell.Active(SpellSlot.R, 550);
            #endregion

            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
        }

        private static void GameOnDraw(EventArgs args)
        {
            if (AatroxMenu.nodraw()) return;

            if (!AatroxMenu.onlyReady())
            {
                if (AatroxMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (AatroxMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (AatroxMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (AatroxMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
            }
            else
            {
                if (!Q.IsOnCooldown && AatroxMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 600, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && AatroxMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 300, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && AatroxMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 1000, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && AatroxMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 550, BorderWidth = 2f }.Draw(Player.Position);
                }
            }
        }
        private static void OnGameUpdate(EventArgs args)
        {
            if (MyActivator.Barrier != null)
                Barrier();
            if (MyActivator.heal != null)
                Heal();
            if (MyActivator.ignite != null)
                ignite();
            if (MyActivator.smite != null)
                smite();
            Player.SetSkinId(AatroxMenu.skinId());
        }

        private static void LevelUpSpells()
        {
            var qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level + qOff;
            var wL = Player.Spellbook.GetSpell(SpellSlot.W).Level + wOff;
            var eL = Player.Spellbook.GetSpell(SpellSlot.E).Level + eOff;
            var rL = Player.Spellbook.GetSpell(SpellSlot.R).Level + rOff;
            if (qL + wL + eL + rL >= ObjectManager.Player.Level) return;
            var level = new[] { 0, 0, 0, 0 };
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
            if (!e.Sender.IsValidTarget() || !AatroxMenu.MyHarrass["gapcloser.e"].Cast<CheckBox>().CurrentValue || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
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
            if (Q.IsReady() && AatroxMenu.MyOtherFunctions["interrupt.q"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(sender);
            }
        }
        private static void RanduinItem()
        {
            if (!MyActivator.randuin.IsReady() || !MyActivator.randuin.IsOwned()) return;
            var randuin = AatroxMenu.MyActivator["randuin"].Cast<CheckBox>().CurrentValue;
            if (randuin && Target.IsValidTarget(MyActivator.randuin.Range) && MyActivator.randuin.IsReady())
            {
                MyActivator.randuin.Cast();
            }
        }
        private static void GloryItem()
        {
            if (!MyActivator.glory.IsReady() || !MyActivator.glory.IsOwned()) return;
            var glory = AatroxMenu.MyActivator["glory"].Cast<CheckBox>().CurrentValue;
            if (EntityManager.Heroes.Allies.Any(ally => ally != Player && ally.Distance(Player) <= 700) && MyActivator.glory.IsReady() && glory)
            {
                MyActivator.glory.Cast();
            }
        }

        private static void Barrier()
        {
            if (Player.IsFacing(Target) && MyActivator.Barrier.IsReady() && Player.HealthPercent <= AatroxMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }


        private static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => MyMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player, a, DamageLibrary.SummonerSpells.Smite) >= a.Health && AatroxMenu.MySpells[a.BaseSkinName].Cast<CheckBox>().CurrentValue && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= AatroxMenu.spellsHealignite())
                MyActivator.ignite.Cast(autoIgnite);

        }

        private static void Heal()
        {
            if (MyActivator.heal.IsReady() && Player.HealthPercent <= AatroxMenu.spellsHealhp())
                MyActivator.heal.Cast();
        }
        private static void GameOnTick(EventArgs args)
        {
            if (AatroxMenu.MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            RanduinItem();
            GloryItem();
            if (AatroxMenu.SpellsPotionsCheck() && !Player.IsInShopRange() && Player.HealthPercent <= AatroxMenu.SpellsPotionsHP() && !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") || Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") || Player.HasBuff("ItemDarkCrystalFlask")))
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
            if (AatroxMenu.SpellsPotionsCheck() && !Player.IsInShopRange() && Player.ManaPercent <= AatroxMenu.SpellsPotionsM() && !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") || Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") || Player.HasBuff("ItemDarkCrystalFlask")))
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

            if (args.Buff.Type == BuffType.Taunt && AatroxMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && AatroxMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && AatroxMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && AatroxMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && AatroxMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && AatroxMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && AatroxMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && AatroxMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && AatroxMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && AatroxMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && AatroxMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && AatroxMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && AatroxMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && AatroxMenu.PoppyUlt())
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

        private static void OnLaneClear()
        {
            Orbwalker.ForcedTarget = null;
            var useQ = AatroxMenu.MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
            var useE = AatroxMenu.MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, E.Range, false).Count();
            var sourceq = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            var sourcee = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, E.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (Q.IsReady() && useQ && AatroxMenu.MyFarm["lc.MinionsQ"].Cast<Slider>().CurrentValue >= count)
            {
                Q.Cast(sourceq);
            }
            if (!E.IsReady() || !useE || AatroxMenu.MyFarm["lc.MinionsE"].Cast<Slider>().CurrentValue > count) return;
            var prediction = E.GetPrediction(sourcee);
            if (prediction.HitChance >= HitChance.High)
            {
                E.Cast(sourcee);
            }
        }

        private static void OnJungle()
        {
            Orbwalker.ForcedTarget = null;

            var useQ = AatroxMenu.MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
            var useW = AatroxMenu.MyFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
            var useE = AatroxMenu.MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition).OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.Distance(Player) <= Player.GetAutoAttackRange());

            if (source == null) return;

            if (E.IsReady() && useE)
            {
                E.Cast(source.Position);
                return;
            }

            if (Q.IsReady() && useQ && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
                return;
            }
            if (!W.IsReady() || !useW) return;
            if (W.IsReady() && Player.HealthPercent < AatroxMenu.MyCombo["jungle.minw"].Cast<Slider>().CurrentValue)
            {
                if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2)
                {
                    W.Cast();
                    return;
                }
            }
            if (!W.IsReady() ||
                !(Player.HealthPercent > AatroxMenu.MyCombo["jungle.maxw"].Cast<Slider>().CurrentValue)) return;
            if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1) return;
            W.Cast();
            return;
        }

        private static void OnHarrass()
        {
            var Target = TargetSelector.GetTarget(E.Range, DamageType.Physical);

            if (!Target.IsValidTarget())
            {
                return;
            }

            var useE = AatroxMenu.MyHarrass["harrass.E"].Cast<CheckBox>().CurrentValue;
            if (useE && E.IsReady())
            {
                E.Cast(Target.Position);
            }
        }
        private static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (!Target.IsValidTarget(700) || Target == null)
            {
                return;
            }

            var useQ = AatroxMenu.MyCombo["combo.Q"].Cast<CheckBox>().CurrentValue;
            var useW = AatroxMenu.MyCombo["combo.W"].Cast<CheckBox>().CurrentValue;
            var useE = AatroxMenu.MyCombo["combo.E"].Cast<CheckBox>().CurrentValue;


            if (useE && E.IsReady())
            {
                E.Cast(Target.Position);
            }
            if (useQ && Q.IsReady())
            {
                if (!Target.HasBuff("AatroxQ"))
                {
                    Q.Cast(Target);
                }
            }
            if (W.IsReady() && useW)
            {
                if (W.IsReady() && Player.HealthPercent < AatroxMenu.MyCombo["combo.minw"].Cast<Slider>().CurrentValue)
                {
                    if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2)
                    {
                        W.Cast();
                    }
                }
                if (W.IsReady() && Player.HealthPercent > AatroxMenu.MyCombo["combo.maxw"].Cast<Slider>().CurrentValue)
                {
                    if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    {
                        W.Cast();
                    }
                }
            }
            var useR = AatroxMenu.MyCombo["combo.R"].Cast<CheckBox>().CurrentValue;
            var ultEnemies = AatroxMenu.MyCombo["combo.REnemies"].Cast<Slider>().CurrentValue;
            if (useR && R.IsReady() && Player.ServerPosition.CountEnemiesInRange(500f) <= ultEnemies)
            {
                R.Cast();
            }
            if (Player.ServerPosition.CountEnemiesInRange(500f) >= AatroxMenu.checkenemies() && Player.HealthPercent <= AatroxMenu.checkhp() && MyActivator.youmus.IsReady() && AatroxMenu.MyActivator["youmus"].Cast<CheckBox>().CurrentValue && MyActivator.youmus.IsOwned())
            {
                MyActivator.youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= AatroxMenu.checkhp() && AatroxMenu.tiamat() && MyActivator.Tiamat.IsReady() && MyActivator.Tiamat.IsOwned() && MyActivator.Tiamat.IsInRange(Target))
            {
                MyActivator.Tiamat.Cast();
                return;
            }
            if (Player.HealthPercent <= AatroxMenu.checkhp() && AatroxMenu.hydra() && MyActivator.Hydra.IsReady() && MyActivator.Hydra.IsOwned() && MyActivator.Hydra.IsInRange(Target))
            {
                MyActivator.Hydra.Cast();
                return;
            }

            if (Player.HealthPercent <= AatroxMenu.checkhp() && AatroxMenu.MyActivator["bilgewater"].Cast<CheckBox>().CurrentValue && MyActivator.bilgewater.IsReady() && MyActivator.bilgewater.IsOwned())
            {
                MyActivator.bilgewater.Cast(Target);
                return;
            }

            if (!(Player.HealthPercent <= AatroxMenu.checkhp()) ||
                !AatroxMenu.MyActivator["botrk"].Cast<CheckBox>().CurrentValue || !MyActivator.botrk.IsReady() ||
                !MyActivator.botrk.IsOwned()) return;
            MyActivator.botrk.Cast(Target);
        }
    }
}





