using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;

namespace SanJuanni
{
    public static class Program
    {
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }
        public static string version = "1.5.5.2";
        public static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        public static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Sejuani") return;
            AbilitySequence = new int[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 2, 2, 1, 1, 4, 1, 1 };
            Chat.Print("SanJuanni Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT TROLL!", Color.Red);
            SanJuanniMenu.loadMenu();
            Game.OnTick += GameOnTick;
            MyActivator.loadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear); 
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Active(SpellSlot.W, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 1175, SkillShotType.Linear);
            #endregion

            Gapcloser.OnGapcloser += AntiGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Drawing.OnDraw += GameOnDraw;
        }
        public static void GameOnDraw(EventArgs args)
        {
            if (SanJuanniMenu.nodraw()) return;

            if (!SanJuanniMenu.onlyReady())
            {
                if (SanJuanniMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (SanJuanniMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (SanJuanniMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (SanJuanniMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (SanJuanniMenu.Allydrawn())
                {
                    DrawHealths();
                }
            }
            else
            {
                if (!Q.IsOnCooldown && SanJuanniMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && SanJuanniMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && SanJuanniMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && SanJuanniMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (SanJuanniMenu.Allydrawn())
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
            if (MyActivator.ignite != null)
                smite();
            Player.SetSkinId(SanJuanniMenu.skinId());
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

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }
            if (R.IsReady() && E.IsInRange(sender) && SanJuanniMenu.interruptR())
            {
                R.Cast(sender);
            }
            else if (Q.IsReady() && SanJuanniMenu.interruptQ() && Prediction.Position.PredictLinearMissile(
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
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !SanJuanniMenu.gapcloserQ() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
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
                    if (EntityManager.Heroes.Enemies.Where(enemy => enemy != Player && enemy.Distance(Player) <= 1000).Count() > SanJuanniMenu.itemsenemiesinrange() && ally.Distance(Player) <= 600 && MyActivator.ironsolari.IsReady() && SanJuanniMenu.ironsolari())
                    {
                        if (ally.HealthPercent < SanJuanniMenu.itemssliderHP())
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
                if (SanJuanniMenu.talisman() && Player.HealthPercent <= SanJuanniMenu.itemssliderHP() && Player.CountEnemiesInRange(800) >= SanJuanniMenu.itemsenemiesinrange())
                {
                    MyActivator.talisman.Cast();
                }
            }
        }
        private static void RanduinItem()
        {
            if (MyActivator.randuin.IsReady() && MyActivator.randuin.IsOwned())
            {
                if (SanJuanniMenu.randuin() && Target.IsValidTarget(MyActivator.randuin.Range) && MyActivator.randuin.IsReady())
                {
                    MyActivator.randuin.Cast();
                }
            }
        }
        private static void GloryItem()
        {
            if (MyActivator.glory.IsReady() && MyActivator.glory.IsOwned())
            {
                if (EntityManager.Heroes.Allies.Where(ally => ally != Player && ally.Distance(Player) <= 700).Count() > 0 && MyActivator.glory.IsReady() && SanJuanniMenu.glory())
                {
                    MyActivator.glory.Cast();
                }
            }
        }

        public static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player, autoIgnite, MyActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= SanJuanniMenu.spellsIgniteFocus())
                MyActivator.ignite.Cast(autoIgnite);

        }
        public static void Heal()
        {
            if (MyActivator.heal.IsReady() && Player.HealthPercent <= SanJuanniMenu.spellsHealHP())
                MyActivator.heal.Cast();
        }
        public static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => MyMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player, a, DamageLibrary.SummonerSpells.Smite) >= a.Health && SanJuanniMenu.MySpells[a.BaseSkinName].Cast<CheckBox>().CurrentValue && MyActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void GameOnTick(EventArgs args)
        {
            if (Player.CountEnemiesInRange(1000) <= SanJuanniMenu.itemsenemiesinrange())
            {
                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.IsFacing(enemy) && ally.HealthPercent <= SanJuanniMenu.itemssliderHP() && Player.Distance(ally) <= 750)
                        {

                            if (SanJuanniMenu.fotmountain() && MyActivator.fotmountain.IsReady())
                            {
                                MyActivator.fotmountain.Cast(ally);
                            }

                            if (SanJuanniMenu.mikael() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt) || ally.HasBuffOfType(BuffType.Polymorph)) && MyActivator.mikael.IsReady())
                            {
                                MyActivator.mikael.Cast(ally);
                            }
                        }
                    }
                }
            }
            if (SanJuanniMenu.lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            KillSteal();
            AscensionItem();
            RanduinItem();
            GloryItem();
            SolariItem();
        }
        private static void KillSteal()
        {
            foreach (var Target in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(W.Range) && !hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25))
            {
                if (SanJuanniMenu.killstealQ() && Q.IsReady() && Target.Health + Target.AttackShield < Player.GetSpellDamage(Target, SpellSlot.Q, DamageLibrary.SpellStages.Default))
                {
                    if (Prediction.Position.PredictLinearMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MaxValue).HitChance >= HitChance.High)
                    {
                        Q.Cast(Target);
                    }
                }
                if (SanJuanniMenu.killstealR() && R.IsReady() && Target.Health + Target.AttackShield < Player.GetSpellDamage(Target, SpellSlot.R, DamageLibrary.SpellStages.Default))
                {
                    if (Prediction.Position.PredictLinearMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MaxValue).HitChance >= HitChance.High && !Prediction.Position.PredictLinearMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MaxValue).CollisionObjects.Any())
                    {
                        R.Cast(Target);
                    }

                }

            }
        }
        public static void OnLaneClear()
        {
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange, false).Count();
            var source = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (Q.IsReady() && SanJuanniMenu.lcQ() && SanJuanniMenu.lcQ1() <= count && Player.ManaPercent >= SanJuanniMenu.lcM())
            {
                Q.Cast(source.Position);
            }
            if (W.IsReady() && SanJuanniMenu.lcW() && SanJuanniMenu.lcW1() <= count && Player.ManaPercent >= SanJuanniMenu.lcM())
            {
                W.Cast();
            }
            if (E.IsReady() && SanJuanniMenu.lcE() && SanJuanniMenu.lcE2() <= count && Player.ManaPercent >= SanJuanniMenu.lcM() && source.HasBuff("sejuanifrost"))
            {
                E.Cast();
            }

        }
        public static void OnJungle()
        {
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                if (Q.IsReady() && SanJuanniMenu.jungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source.Position);
            }
            if (W.IsReady() && SanJuanniMenu.jungleW())
            {
                W.Cast();
            }
            if (E.IsReady() && source.HasBuff("SejuaniFrost") && SanJuanniMenu.jungleES() && MyActivator.smite.IsReady() && source.Health <= Player.GetSpellDamage(source, SpellSlot.E, DamageLibrary.SpellStages.Default) && DamageLibrary.GetSummonerSpellDamage(Player, source, DamageLibrary.SummonerSpells.Smite) >= source.Health)
            {
                E.Cast();
                MyActivator.smite.Cast(source);
                return;
            }
            if (E.IsReady() && SanJuanniMenu.jungleE() && source.Health + (source.HPRegenRate / 2) <= Player.GetSpellDamage(source, SpellSlot.E, DamageLibrary.SpellStages.Default) && source.Distance(Player) <= E.Range && source.HasBuff("SejuaniFrost"))
            {
                E.Cast();
            }
        }


        private static void OnHarrass()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (!Target.IsValidTarget())
            {
                return;
            }
            if (SanJuanniMenu.harassQ() && Target.IsValidTarget(Q.Range) && Player.ManaPercent >= SanJuanniMenu.harassQWE())
            {
                if (Prediction.Position.PredictLinearMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MaxValue).HitChance >= HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (SanJuanniMenu.harassW() && Target.IsValidTarget(W.Range) && Player.ManaPercent >= SanJuanniMenu.harassQWE())
            {
                W.Cast();
            }

            if (SanJuanniMenu.harassE() && E.IsReady() && Target.IsValidTarget(E.Range) && Player.ManaPercent >= SanJuanniMenu.harassQWE() && Target.HasBuff("SejuaniFrost"))
            {
                E.Cast();
            }
        }
        private static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (!Target.IsValidTarget(Q.Range) || Target == null)
            {
                return;
            }
            if (SanJuanniMenu.comboQ() && Q.IsReady() && Target.IsValidTarget(Q.Range))
            {
                if (Prediction.Position.PredictLinearMissile(Target,Q.Range,Q.Width,Q.CastDelay,Q.Speed,int.MaxValue).HitChance >= HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (SanJuanniMenu.comboW() && W.IsReady() && Target.IsValidTarget(W.Range))
            {
                W.Cast();
            }
            if (SanJuanniMenu.comboE() && E.IsReady() && Target.IsValidTarget(E.Range) && Target.Position.CountEnemiesInRange(1000) >= SanJuanniMenu.comboE1() && Target.HasBuff("SejuaniFrost"))
            {
                E.Cast();
            }
            if (SanJuanniMenu.comboR() && R.IsReady() && Player.ServerPosition.CountEnemiesInRange(500f) >= SanJuanniMenu.comboR2())
            {
                if (Prediction.Position.PredictLinearMissile(Target,Q.Range,Q.Width,Q.CastDelay,Q.Speed,int.MaxValue).HitChance >= HitChance.High && !Prediction.Position.PredictLinearMissile(Target,Q.Range,Q.Width,Q.CastDelay,Q.Speed,int.MaxValue).CollisionObjects.Any())
                {
                    R.Cast(Target);
                }
            }

            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >= SanJuanniMenu.youmusEnemies() || Player.HealthPercent >= SanJuanniMenu.itemsYOUMUShp()) && MyActivator.youmus.IsReady() && SanJuanniMenu.youmus() && MyActivator.youmus.IsOwned())
            {
                MyActivator.youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= SanJuanniMenu.bilgewaterHP() && SanJuanniMenu.bilgewater() && MyActivator.bilgewater.IsReady() && MyActivator.bilgewater.IsOwned())
            {
                MyActivator.bilgewater.Cast(Target);
                return;
            }

            if (Player.HealthPercent <= SanJuanniMenu.botrkHP() && SanJuanniMenu.botrk() && MyActivator.botrk.IsReady() && MyActivator.botrk.IsOwned())
            {
                MyActivator.botrk.Cast(Target);
                return;
            }

        }
    }
    }


