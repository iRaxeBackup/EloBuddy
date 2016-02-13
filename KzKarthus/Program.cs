using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Drawing;
using System.Linq;
using SharpDX;
using Font = SharpDX.Direct3D9.Font;
using SharpDX.Direct3D9;

namespace KzKarthus
{
    public static class Program
    {
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }
        public static string version = "2.0.0.0";
        public static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        public static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        private static Font Tahoma16B;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Karthus") return;
            AbilitySequence = new int[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
            Chat.Print("KzKarthus Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FEED!", Color.Red);
            Tahoma16B = new Font(Drawing.Direct3DDevice, new FontDescription { FaceName = "Tahoma", Height = 16, Weight = FontWeight.Bold, OutputPrecision = FontPrecision.Default, Quality = FontQuality.ClearType });
            KzKarthusMenu.loadMenu();
            Game.OnTick += GameOnTick;
            MyActivator.loadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, 1000, int.MaxValue, 160);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, int.MaxValue, 70);
            E = new Spell.Active(SpellSlot.E, 505);
            R = new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Circular, 3000, int.MaxValue, int.MaxValue);
            #endregion

            Gapcloser.OnGapcloser += AntiGapCloser;
            Drawing.OnDraw += GameOnDraw;
        }
        public static void GameOnDraw(EventArgs args)
        {
            if (KzKarthusMenu.nodraw()) return;

            if (!KzKarthusMenu.onlyReady())
            {
                if (KzKarthusMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (KzKarthusMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (KzKarthusMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (KzKarthusMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
            }
            else
            {
                if (!Q.IsOnCooldown && KzKarthusMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && KzKarthusMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && KzKarthusMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && KzKarthusMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                var EnemiesTxt = "";

                var enemies = EntityManager.Heroes.Enemies.Where(a => a.IsEnemy && a.IsValid);
                Vector2 WTS = Drawing.WorldToScreen(Player.Position);

                foreach (var enemy in enemies)
                {
                    if ((GetTargetHealth(enemy) - Player.GetSpellDamage(enemy, SpellSlot.R, DamageLibrary.SpellStages.Default)) <= 0)
                    {
                        if (!enemy.IsDead)
                        {
                            EnemiesTxt = enemy.BaseSkinName + " | ";

                        }
                    }
                }

                if (EnemiesTxt != "")
                {
                    if (KzKarthusMenu.alertR() && R.IsReady())
                    {
                        DrawFontTextScreen(Tahoma16B, "R Alert : " + EnemiesTxt + "Killable", (float)(WTS[0] - 150), (float)(WTS[1] + 80), SharpDX.Color.Red);
                    }
                }
            }
        }
        public static void DrawFontTextScreen(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int)vPosX, (int)vPosY, vColor);
        }
        private static void OnGameUpdate(EventArgs args)
        {
            if (MyActivator.heal != null)
                Heal();
            if (MyActivator.barrier != null)
                barrier();
            if (MyActivator.ignite != null)
                ignite();
            Player.SetSkinId(KzKarthusMenu.skinId());
        }
        public static float GetTargetHealth(AIHeroClient playerInfo)
        {
            if (playerInfo.IsVisible)
                return playerInfo.Health;

            var predictedhealth = playerInfo.Health + playerInfo.HPRegenRate * (R.CastDelay / 1000);

            return predictedhealth > playerInfo.MaxHealth ? playerInfo.MaxHealth : predictedhealth;
        }
        private static void Zhonya()
        {
            if (KzKarthusMenu.SpellsZhonyaCheck() && MyActivator.Zhonya.IsReady() && MyActivator.Zhonya.IsOwned())
            {
                    if (Player.HealthPercent <= KzKarthusMenu.SpellsZhonyaHP() && Player.CountEnemiesInRange(Q.Range) >= KzKarthusMenu.SpellsZhonyaEnemies())
                    {
                        MyActivator.Zhonya.Cast();
                    }
                }
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
            if (!e.Sender.IsValidTarget() || !KzKarthusMenu.gapcloserW() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            W.Cast(e.Sender);
        }
        public static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player, autoIgnite, MyActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= KzKarthusMenu.spellsIgniteFocus())
                MyActivator.ignite.Cast(autoIgnite);

        }
        public static void Heal()
        {
            if (MyActivator.heal.IsReady() && Player.HealthPercent <= KzKarthusMenu.spellsHealHP())
                MyActivator.heal.Cast();
        }
        public static void barrier()
        {
            if (MyActivator.barrier.IsReady() && Player.HealthPercent <= KzKarthusMenu.spellsBarrierHP())
                MyActivator.barrier.Cast();
        }
        private static void GameOnTick(EventArgs args)
        {
            if (KzKarthusMenu.lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) OnLastHit();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            KillSteal();
            AutoCast();
            Zhonya();
        }
        public static void AutoCast()
        {
            if (KzKarthusMenu.comboAC())
            {
                if (Player.IsDead || Player.IsZombie)
                {
                    if (KzKarthusMenu.comboQ() && Q.IsReady())
                    {
                        var Target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                        if (Target != null && Target.IsValid)
                        {
                            if (Prediction.Position.PredictCircularMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed).HitChance >= HitChance.High)
                            {
                                Q.Cast(Target);
                            }
                        }
                    }
                    if (KzKarthusMenu.comboW() && W.IsReady())
                    {
                        var Target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                        var Pred = W.GetPrediction(Target);
                        if (Target != null && Target.IsValid)
                        {
                            if (Pred.HitChance >= HitChance.High)
                            {
                                W.Cast(Pred.CastPosition);
                            }
                        }
                    }
                }
            }
        }

        private static void KillSteal()
        {
            foreach (var Target in EntityManager.Heroes.Enemies.Where(hero =>!hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25))
            {
                if (KzKarthusMenu.killstealQ() && Q.IsReady() && Target.IsValidTarget(Q.Range) && Target.Health + Target.AttackShield < Player.GetSpellDamage(Target, SpellSlot.Q, DamageLibrary.SpellStages.Default))
                {
                    if (Prediction.Position.PredictCircularMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed).HitChance >= HitChance.High)
                    {
                        Q.Cast(Target);
                    }
                }
            }
        }

        public static void OnLaneClear()
        {
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange, false).Count();
            var source = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Player.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (count == 0) return;
            if (Q.IsReady() && KzKarthusMenu.lcQ() && KzKarthusMenu.lcQ1() <= count && Player.ManaPercent >= KzKarthusMenu.lcM())
            {
                Q.Cast(source.Position);
            }
            if (E.IsReady() && KzKarthusMenu.lcE() && KzKarthusMenu.lcE2() <= count && Player.ManaPercent >= KzKarthusMenu.lcM())
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                    E.Cast();
            }
            else
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2)
                    E.Cast();
            }
        }
        static void OnLastHit()
        {
            var source = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (source == null || !source.IsValid) return;
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            if (KzKarthusMenu.lcQ2() && Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead && Player.ManaPercent >= KzKarthusMenu.lcM())
            {
                Q.Cast(source);
            }
        }
        public static void OnJungle()
        {
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (Q.IsReady() && KzKarthusMenu.jungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source.Position);
            }

            if (KzKarthusMenu.jungleE() && E.IsReady() && source.Distance(Player) <= E.Range)
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                    E.Cast();
            }
            else
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2)
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
            if (KzKarthusMenu.harassQ() && Target.IsValidTarget(Q.Range) && Player.ManaPercent >= KzKarthusMenu.harassQE())
            {
                if (Prediction.Position.PredictCircularMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed).HitChance >= HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (KzKarthusMenu.harassE() && E.IsReady() && Target.IsValidTarget(E.Range) && Player.ManaPercent >= KzKarthusMenu.harassQE())
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                    E.Cast();
            }
            else
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2)
                    E.Cast();
            }
        }
        private static void OnCombo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (!Target.IsValidTarget(Q.Range) || Target == null)
            {
                return;
            }
            if (KzKarthusMenu.comboW() && W.IsReady() && Target.IsValidTarget(W.Range) && Player.ManaPercent >= KzKarthusMenu.comboW1())
            {
                W.Cast(Target.Position);
            }
            if (KzKarthusMenu.comboQ() && Q.IsReady() && Target.IsValidTarget(Q.Range))
            {
                if (Prediction.Position.PredictCircularMissile(Target, Q.Range, Q.Width, Q.CastDelay, Q.Speed).HitChance >= HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (KzKarthusMenu.comboE() && E.IsReady() && Target.IsValidTarget(E.Range) && Player.ManaPercent >= KzKarthusMenu.comboE2())
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                    E.Cast();
            }
            else
            {
                if (Player.Spellbook.GetSpell(SpellSlot.E).ToggleState == 2)
                        E.Cast();
            }
        }
    }
    }


