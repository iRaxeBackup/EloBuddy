using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;

namespace Trollaloi
{
    public static class Program
    {
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }
        public static string version = "1.7.0.0";
        public static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        public static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Illaoi") return;
            AbilitySequence = new int[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
            Chat.Print("Trollaloi Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FEED!", Color.Red);
            IllaoiMenu.loadMenu();
            Game.OnTick += GameOnTick;
            MyActivator.loadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 750, int.MaxValue, 100);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1900, 50);
            R = new Spell.Active(SpellSlot.R, 450);
            #endregion

            Gapcloser.OnGapcloser += AntiGapCloser;
            Drawing.OnDraw += GameOnDraw;
        }
        public static void GameOnDraw(EventArgs args)
        {
            if (IllaoiMenu.nodraw()) return;

            if (!IllaoiMenu.onlyReady())
            {
                if (IllaoiMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (IllaoiMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (IllaoiMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (IllaoiMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (IllaoiMenu.Allydrawn())
                {
                    DrawHealths();
                }
            }
            else
            {
                if (!Q.IsOnCooldown && IllaoiMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && IllaoiMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && IllaoiMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && IllaoiMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (IllaoiMenu.Allydrawn())
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
            if (MyActivator.Barrier != null)
                Barrier();
            if (MyActivator.heal != null)
                Heal();
            if (MyActivator.ignite != null)
                ignite();
            if (MyActivator.smite != null)
                smite();
            Player.SetSkinId(IllaoiMenu.skinId());
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

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !IllaoiMenu.gapcloserQ() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            Q.Cast(e.Sender);
        }
        private static void SolariItem()
        {
            if (MyActivator.ironsolari.IsReady() && MyActivator.ironsolari.IsOwned())
            {
                foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                {
                    if (EntityManager.Heroes.Enemies.Where(enemy => enemy != Player && enemy.Distance(Player) <= 1000).Count() > IllaoiMenu.itemsenemiesinrange() && ally.Distance(Player) <= 600 && MyActivator.ironsolari.IsReady() && IllaoiMenu.ironsolari())
                    {
                        if (ally.HealthPercent < IllaoiMenu.itemssliderHP())
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
                if (IllaoiMenu.talisman() && Player.HealthPercent <= IllaoiMenu.itemssliderHP() && Player.CountEnemiesInRange(800) >= IllaoiMenu.itemsenemiesinrange())
                {
                    MyActivator.talisman.Cast();
                }
            }
        }
        private static void RanduinItem()
        {
            if (MyActivator.randuin.IsReady() && MyActivator.randuin.IsOwned())
            {
                if (IllaoiMenu.randuin() && Target.IsValidTarget(MyActivator.randuin.Range) && MyActivator.randuin.IsReady())
                {
                    MyActivator.randuin.Cast();
                }
            }
        }
        private static void GloryItem()
        {
            if (MyActivator.glory.IsReady() && MyActivator.glory.IsOwned())
            {
                if (EntityManager.Heroes.Allies.Where(ally => ally != Player && ally.Distance(Player) <= 700).Count() > 0 && MyActivator.glory.IsReady() && IllaoiMenu.glory())
                {
                    MyActivator.glory.Cast();
                }
            }
        }

        public static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player, autoIgnite, MyActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= IllaoiMenu.spellsIgniteFocus())
                MyActivator.ignite.Cast(autoIgnite);

        }
        private static void Barrier()
        {
            if (Player.IsFacing(Target) && MyActivator.Barrier.IsReady() && Player.HealthPercent <= IllaoiMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }
        public static void Heal()
        {
            if (MyActivator.heal.IsReady() && Player.HealthPercent <= IllaoiMenu.spellsHealHP())
                MyActivator.heal.Cast();
        }
        public static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => MyMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player, a, DamageLibrary.SummonerSpells.Smite) >= a.Health && IllaoiMenu.MySpells[a.BaseSkinName].Cast<CheckBox>().CurrentValue && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void GameOnTick(EventArgs args)
        {
            if (Player.CountEnemiesInRange(1000) <= IllaoiMenu.itemsenemiesinrange())
            {
                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.IsFacing(enemy) && ally.HealthPercent <= IllaoiMenu.itemssliderHP() && Player.Distance(ally) <= 750)
                        {

                            if (IllaoiMenu.fotmountain() && MyActivator.fotmountain.IsReady())
                            {
                                MyActivator.fotmountain.Cast(ally);
                            }

                            if (IllaoiMenu.mikael() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt) || ally.HasBuffOfType(BuffType.Polymorph)) && MyActivator.mikael.IsReady())
                            {
                                MyActivator.mikael.Cast(ally);
                            }
                        }
                    }
                }
            }
            if (IllaoiMenu.lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            KillSteal();
            AscensionItem();
            RanduinItem();
            GloryItem();
            SolariItem();
            if (IllaoiMenu.SpellsPotionsCheck() && !Player.IsInShopRange() && Player.HealthPercent <= IllaoiMenu.SpellsPotionsHP() && !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") || Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") || Player.HasBuff("ItemDarkCrystalFlask")))
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
            if (IllaoiMenu.SpellsPotionsCheck() && !Player.IsInShopRange() && Player.ManaPercent <= IllaoiMenu.SpellsPotionsM() && !(Player.HasBuff("RegenerationPotion") || Player.HasBuff("ItemCrystalFlaskJungle") || Player.HasBuff("ItemMiniRegenPotion") || Player.HasBuff("ItemCrystalFlask") || Player.HasBuff("ItemDarkCrystalFlask")))
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

            if (args.Buff.Type == BuffType.Taunt && IllaoiMenu.Taunt())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && IllaoiMenu.Stun())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && IllaoiMenu.Snare())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && IllaoiMenu.Polymorph())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && IllaoiMenu.Blind())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && IllaoiMenu.Fear())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && IllaoiMenu.Charm())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && IllaoiMenu.Suppression())
            {
                DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && IllaoiMenu.Silence())
            {
                DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && IllaoiMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && IllaoiMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && IllaoiMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && IllaoiMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && IllaoiMenu.PoppyUlt())
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

        private static void KillSteal()
        {
            foreach (var Target in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(W.Range) && !hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25))
            {
                if (IllaoiMenu.killstealQ() && Q.IsReady() && Target.Health + Target.AttackShield < Player.GetSpellDamage(Target, SpellSlot.Q, DamageLibrary.SpellStages.Default))
                {
                    if (Q.GetPrediction(Target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(Target).CastPosition);
                    }
                }

            }
        }
        public static void OnLaneClear()
        {
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange, false).Count();
            var source = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (Q.IsReady() && IllaoiMenu.lcQ() && IllaoiMenu.lcQ1() <= count && Player.ManaPercent >= IllaoiMenu.lcM())
            {
                Q.Cast(source.Position);
            }
            if (W.IsReady() && IllaoiMenu.lcW() && IllaoiMenu.lcW1() <= count && Player.ManaPercent >= IllaoiMenu.lcM())
            {
                W.Cast();
            }
            if (E.IsReady() && IllaoiMenu.lcE() && IllaoiMenu.lcE2() <= count && Player.ManaPercent >= IllaoiMenu.lcM())
            {
                E.Cast(source.Position);
            }

        }
        public static void OnJungle()
        {
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (source == null) return;
            if (Q.IsReady() && IllaoiMenu.jungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source.Position);
            }
            if (W.IsReady() && IllaoiMenu.jungleW() && source.Distance(Player) <= W.Range)
            {
                W.Cast();
            }
            if (E.IsReady() && IllaoiMenu.jungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source.Position);
            }
        }

        private static void OnHarrass()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var Buuuuuu = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(o => o.HasBuff("illaoiespirit"));
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);


            if (Target != null && Buuuuuu == null)
            {
                if (Q.IsReady() && Target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemies)
                    {
                        var useQ = IllaoiMenu.MyHarass["harass.q"
                                                       + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ && Q.GetPrediction(qenemies).HitChancePercent >= IllaoiMenu.harassQH())
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }
            }
            if (Target == null && Buuuuuu != null)
            {
                if (IllaoiMenu.harassQ1() && Q.IsReady())
                {
                    if (Q.GetPrediction(Buuuuuu).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(Buuuuuu).CastPosition);
                    }
                }
            }

            if (E.IsReady() && Target.IsValidTarget(E.Range) && Player.ManaPercent >= IllaoiMenu.harassQE())
            {
                foreach (var eenemies in enemies)
                {
                    var useE = IllaoiMenu.MyHarass["harass.e"
                                                   + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE && E.GetPrediction(eenemies).HitChance >= HitChance.High)
                    {
                        if (!E.GetPrediction(eenemies).CollisionObjects.Any())
                        {
                            E.Cast(E.GetPrediction(eenemies).CastPosition);
                        }
                    }
                }
            }
        }

        private static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var Target2 = TargetSelector.GetTarget(2550, DamageType.Physical);
            var Buuuuuu = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Name == Target2.Name);
            var tentacle = ObjectManager.Get<Obj_AI_Minion>().First(x => x.Name == "God");
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var wminion =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Position, W.Range + 350)
                    .Where(
                        m =>
                        m.IsValidTarget())
                    .OrderBy(m => m.Distance(Target))
                    .FirstOrDefault();

            if (IllaoiMenu.comboR() && R.IsReady() && Player.ServerPosition.CountEnemiesInRange(500f) >= IllaoiMenu.comboR2())
            {
                R.Cast();
            }

            if (Q.IsReady() && Target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemies)
                {
                    var useQ = IllaoiMenu.MyCombo["combo.q"
                                                  + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ && Q.GetPrediction(qenemies).HitChancePercent >= IllaoiMenu.comboQH())
                    {
                        Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                    }
                }
            }
             if (Target == null && Buuuuuu != null)
             {
                 if (IllaoiMenu.comboQ1() && Q.IsReady())
             {
                     Q.Cast(Buuuuuu.ServerPosition);
             }
             }
             if (tentacle != null)
             {
            if (IllaoiMenu.gapcloserW() && wminion != null &&
                    Target.Distance(Player) >= Player.GetAutoAttackRange(Target) &&
                    wminion.Distance(Target) <= Player.Distance(Target) &&
                    wminion.Distance(Player) <= W.Range)
                {
                    W.Cast(wminion);
                }

                if (IllaoiMenu.comboW() && W.IsReady() && Target.IsValidTarget(W.Range) && Target.Position.CountEnemiesInRange(800) >= IllaoiMenu.comboW1())
            {
                W.Cast();
            }
           }

            if (E.IsReady() && Target.IsValidTarget(E.Range))
            {
                foreach (var eenemies in enemies)
                {
                    var useE = IllaoiMenu.MyCombo["combo.e"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE && E.GetPrediction(eenemies).HitChancePercent >= IllaoiMenu.comboEH())
                    {
                        if (!E.GetPrediction(eenemies).CollisionObjects.Any())
                        {
                            E.Cast(E.GetPrediction(eenemies).CastPosition);
                        }
                    }
                }
            }
            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >= IllaoiMenu.youmusEnemies() || Player.HealthPercent >= IllaoiMenu.itemsYOUMUShp()) && MyActivator.youmus.IsReady() && IllaoiMenu.youmus() && MyActivator.youmus.IsOwned())
            {
                MyActivator.youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= IllaoiMenu.bilgewaterHP() && IllaoiMenu.bilgewater() && MyActivator.bilgewater.IsReady() && MyActivator.bilgewater.IsOwned())
            {
                MyActivator.bilgewater.Cast(Target);
                return;
            }

            if (Player.HealthPercent <= IllaoiMenu.checkhp() && IllaoiMenu.tiamat() && MyActivator.Tiamat.IsReady() && MyActivator.Tiamat.IsOwned() && MyActivator.Tiamat.IsInRange(Target))
            {
                MyActivator.Tiamat.Cast();
                return;
            }
            if (Player.HealthPercent <= IllaoiMenu.checkhp() && IllaoiMenu.hydra() && MyActivator.Hydra.IsReady() && MyActivator.Hydra.IsOwned() && MyActivator.Hydra.IsInRange(Target))
            {
                MyActivator.Hydra.Cast();
                return;
            }

            if (Player.HealthPercent <= IllaoiMenu.botrkHP() && IllaoiMenu.botrk() && MyActivator.botrk.IsReady() && MyActivator.botrk.IsOwned())
            {
                MyActivator.botrk.Cast(Target);
                return;
            }

        }
    }
    }


