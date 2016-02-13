using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Owlyze
{
    public static class Program
    {
        public static string Version = "1.0.3.1";
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static int time;
        public static readonly AIHeroClient Player = ObjectManager.Player;

        internal static void Main(string[] args)
        {
         Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Ryze") return;
            AbilitySequence = new[] {3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
            Chat.Print("Owlyze Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT AFK!", Color.Red);

            #region Loading functions

            //Here the program will call function by function everything we need
            OwlyzeMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += GameOnDraw;
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            Gapcloser.OnGapcloser += AntiGapCloser;

            #endregion


            #region Skill

            //Here the program will learn the instructions for the spells 
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100)
            {AllowedCollisionCount = int.MaxValue};
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);

            #endregion
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
            if (OwlyzeMenu.checkSkin())
            {
                Player.SetSkinId(OwlyzeMenu.SkinId());
            }
        }

        private static void GameOnTick(EventArgs args)
        {
            if (OwlyzeMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                ComboMode();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                OnLaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                OnLastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                OnJungle();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                OnFlee();
            }
            KillSteal();
            AutoCC();
            AutoPotions();
            AutoWard();
            AutoHarass();
            AutoStack();
        }

        private static void GameOnDraw(EventArgs args)
        {
            if (OwlyzeMenu.Nodraw()) return;

            if (!OwlyzeMenu.OnlyReady())
            {
                if (OwlyzeMenu.DrawingsQ())
                {
                    new Circle {Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (OwlyzeMenu.DrawingsW())
                {
                    new Circle {Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (OwlyzeMenu.DrawingsE())
                {
                    new Circle {Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (OwlyzeMenu.DrawingsR())
                {
                    new Circle {Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (OwlyzeMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (
                        var place in
                            wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && OwlyzeMenu.DrawingsQ())
                {
                    new Circle {Color = Color.AliceBlue, Radius = 900, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && OwlyzeMenu.DrawingsW())
                {
                    new Circle {Color = Color.OrangeRed, Radius = 600, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && OwlyzeMenu.DrawingsE())
                {
                    new Circle {Color = Color.Cyan, Radius = 600, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && OwlyzeMenu.DrawingsR())
                {
                    new Circle {Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f}.Draw(Player.Position);
                }
                if (OwlyzeMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (
                        var place in
                            wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
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

        private static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => MyMobs.MinionNames.Contains(a.BaseSkinName)
                                                                   &&
                                                                   DamageLibrary.GetSummonerSpellDamage(Player, a,
                                                                       DamageLibrary.SummonerSpells.Smite) >= a.Health
                                                                   &&
                                                                   OwlyzeMenu.MyActivator[a.BaseSkinName].Cast<CheckBox>
                                                                       ().CurrentValue
                                                                   && MyActivator.smite.IsInRange(a))
                .OrderByDescending(a => a.MaxHealth)
                .FirstOrDefault();
            if (unit != null && MyActivator.smite.IsReady())
                MyActivator.smite.Cast(unit);
        }

        private static void Barrier()
        {
            if (MyActivator.Barrier.IsReady() && Player.HealthPercent <= OwlyzeMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }

        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= OwlyzeMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (MyActivator.Heal.IsReady() && Player.HealthPercent <= OwlyzeMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void AutoZhonya()
        {
            if (Player.HealthPercent <= OwlyzeMenu.ZhonyaHP() && OwlyzeMenu.Zhonya()
                && Player.CountEnemiesInRange(800) >= OwlyzeMenu.ZhonyaEnemies() && MyActivator.Zhonya.IsReady() &&
                MyActivator.Zhonya.IsOwned())
            {
                MyActivator.Zhonya.Cast();
            }
        }

        private static void AutoSeraph()
        {
            if (Player.HealthPercent <= OwlyzeMenu.SeraphHP() && OwlyzeMenu.Seraph()
                && Player.CountEnemiesInRange(800) >= OwlyzeMenu.SeraphEnemies() && MyActivator.Seraph.IsReady() &&
                MyActivator.Seraph.IsOwned())
            {
                MyActivator.Seraph.Cast();
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

                if (OwlyzeMenu.KillstealQ() && Q.IsReady() &&
                    target.Health + target.AttackShield <=
                    Player.GetSpellDamage(target, SpellSlot.Q) + OwlyzeMenu.ComboQ1())
                {
                    Q.Cast(target.Position);
                }

                if (OwlyzeMenu.KillstealW() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.W) + OwlyzeMenu.ComboW1() && Player.Mana >= 100)
                {
                    if (W.IsReady() && target.IsValidTarget(W.Range))
                    {
                        W.Cast(target);
                    }
                }

                if (OwlyzeMenu.KillstealE() && E.IsReady() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.E) + OwlyzeMenu.ComboE1())
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
                         x.HasBuffOfType(BuffType.Snare));
            ;

            if (autoCCTarget != null && autoCCTarget.IsValidTarget(Q.Range))
            {
                if (OwlyzeMenu.ComboCC() && Q.IsReady())
                {
                    Q.Cast(autoCCTarget);
                }
                if (OwlyzeMenu.ComboCC1() && W.IsReady())
                {
                    W.Cast(autoCCTarget);
                }
                if (OwlyzeMenu.ComboCC2() && E.IsReady())
                {
                    E.Cast(autoCCTarget);
                }

            }
        }

       
        public static bool IsWall(Vector3 vector)
        {
            return NavMesh.GetCollisionFlags(vector.X, vector.Y).HasFlag(CollisionFlags.Wall);
        }

        private static void AutoPotions()
        {
            if (OwlyzeMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= OwlyzeMenu.SpellsPotionsHP() &&
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
            if (OwlyzeMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= OwlyzeMenu.SpellsPotionsM() &&
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


        private static bool IsWarded(Vector3 position)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(obj => obj.IsWard() && obj.Distance(position) <= 200);
        }

        private static void AutoWard()
        {
            if (OwlyzeMenu.checkWard())
            {
                foreach (
                    var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() &&
                        OwlyzeMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() &&
                        OwlyzeMenu.greaterStealthTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() &&
                        OwlyzeMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() &&
                        OwlyzeMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && OwlyzeMenu.pinkWard() &&
                        !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoHarass()
        {
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= 1000);
            if (OwlyzeMenu.HarassA())
            {
                if (!OwlyzeMenu.HarassS() && OwlyzeMenu.HarassS1() <= Stacks)
                {
                    return;
                }

                foreach (var qenemies in enemies)
                {
                    var useQ = OwlyzeMenu.MyHarass["harass.Q" + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        if (Q.IsReady() && Q.IsInRange(qenemies))
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                foreach (var wenemies in enemies)
                {
                    var useW = OwlyzeMenu.MyHarass["harass.W" + wenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useW)
                    {
                        if (W.IsReady() && W.IsInRange(wenemies))
                        {
                            W.Cast(wenemies);
                        }
                    }
                }

                foreach (var eenemies in enemies)
                {
                    var useE = OwlyzeMenu.MyHarass["harass.E" + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        if (E.IsReady() && E.IsInRange(eenemies))
                        {
                            E.Cast(eenemies);
                        }
                    }
                }
            }
        }

        private static void AutoStack()
        {
            //Calls the count of the Ryze passive stacks
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            //Checks if there are monsters in a 1050 range
            if (EntityManager.MinionsAndMonsters.CombinedAttackable.Any(x => x.IsValidTarget(Q.Range + 100)))
            {
                return;
            }
            //Checks if there are enemies in a 1100 range
            if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(Q.Range + 150)))
            {
                return;
            }
            //Checks if player is recalling or the cursor is in 0 
            if (Player.IsRecalling() || Game.CursorPos.IsZero)
            {
                return;
            }
            //Checks if Manapercent is more than what is declared in Mana limit in Misc Settings
            if (Player.ManaPercent < OwlyzeMenu.passiveS3())
            {
                return;
            }
            //Checks if the autostack is checked it the Misc Settings and cast
            if (OwlyzeMenu.passiveS())
            {
                if (Stacks >= OwlyzeMenu.passiveS2())
                {
                    return;
                }
                if (Q.IsReady())
                {
                    Q.Cast(Player.ServerPosition.Extend(Game.CursorPos, Q.Range).To3D());
                }
            }
        }

        #region  Take the value of the MyCombo slider

        static void ComboMode()
        {
            //Slider contains Normal or Slutty mode combo
            var options = OwlyzeMenu.MyCombo["combo.Mode"].DisplayName;
            //Here we split the options
            switch (options)
            {
                //Will execute the normal combo
                case "Mode: Normal Combo":
                    AABlock();
                    NormalCombo();
                    AutoZhonya();
                    AutoSeraph();
                    break;
                //Will execute the slutty combo
                case "Mode: Slutty Combo":
                    AABlock();
                    SluttyCombo();
                    AutoZhonya();
                    AutoSeraph();
                    break;
            }
        }

        #endregion

        #region AABlock for  Combo

        //Blocks the AA if the checkbox is toggled in Combo page
        public static void AABlock()
        {
            //Checks if the chechbox is toggled
            var Combo = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
            if (OwlyzeMenu.ComboAA() && Combo)
            {
                //Will execute the command of disable AA
                Orbwalker.DisableAttacking = true;
            }
        }

        #endregion

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }
            if (W.IsReady() && W.IsInRange(sender) && OwlyzeMenu.gapcloserW())
            {
                W.Cast(sender);
            }
            if (R.IsReady() && R.IsInRange(sender) && OwlyzeMenu.gapcloserR())
            {
                R.Cast();
            }
        }


        private static void OnFlee()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Snared = target.HasBuff("RyzeW");
            if (W.IsReady() && Player.ManaPercent >= OwlyzeMenu.FleeM())
            {
                W.Cast(target);
            }
            if (R.IsReady() && Player.ManaPercent >= OwlyzeMenu.FleeM() && Snared)
            {
                R.Cast();
            }
        }
        #region LaneClear
        private static void OnLaneClear()
        {
            //The source now gives priority to the nearest minions
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Distance(Player))
                    .FirstOrDefault(a => a.IsValidTarget(Q.Range));
            //The var stacks is usefull for get the count of the passive
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            /*Now we check if the function laneclear with stack limiter
            is active and if the stacks are < the limit*/
            if (OwlyzeMenu.LcQ1() || OwlyzeMenu.LcW1() || OwlyzeMenu.LcE1()) OnLastHit();
            if (source == null) return;
            if (OwlyzeMenu.LcS() && OwlyzeMenu.LcS1() <= Stacks) return;
            //Uses q in laneclear mode 
            if (Q.IsReady() && Player.ManaPercent >= OwlyzeMenu.LcM())
            {
                if (OwlyzeMenu.LcQ() && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source);
                }
            }
            //Uses W in laneclear mode 
            if (W.IsReady() && Player.ManaPercent >= OwlyzeMenu.LcM())
            {
                if (OwlyzeMenu.LcW() && source.IsValidTarget(W.Range))
                {
                    W.Cast(source);
                }
            }
            //Uses E in laneclear mode
            if (E.IsReady() && Player.ManaPercent >= OwlyzeMenu.LcM())
            {
                if (OwlyzeMenu.LcE() && source.IsValidTarget(E.Range))
                {
                    E.Cast(source);
                }
            }
            //Uses R in laneclear mode 
            if (R.IsReady() && OwlyzeMenu.LcR() && Player.ManaPercent >= OwlyzeMenu.LcM() &&
                source.IsValidTarget(R.Range))
            {
                R.Cast();
            }
        }
        #endregion

        private static void OnLastHit()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Distance(Player))
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            //var mana = Player.MaxMana;
            /*Will check if the priority is the harass or laneclear 
            If the priority is the harass then doesnt execute lashit*/
            if (OwlyzeMenu.LcH() && Player.CountEnemiesInRange(Q.Range) <= 1)
            {
                OnHarrass();
                return;
            }
            //If there are no minions then dont execute
            if (source == null) return;
            //If Q is toggled and ready then execute
            if (OwlyzeMenu.LcQ1())
            {
               /* Player.CalculateDamageOnUnit(source, DamageType.Magical,
                    new float[] { 60, 85, 110, 135, 160 }[Q.Level] +
                             0.55f * Player.FlatMagicDamageMod +
                             new[] { 0.02f * mana, 0.025f * mana,
                                 0.03f * mana, 0.035f * mana, 0.04f * mana}[Q.Level]))*/

                if (Q.IsReady() && source.Health <= Player.GetSpellDamage(source, SpellSlot.Q))
                    {
                        Q.Cast(source);
                    }
            }
            //If W is Toggled and ready then execute
            if (OwlyzeMenu.LcW1())
            {
                /*Player.CalculateDamageOnUnit(source, DamageType.Magical,
                    new float[] {80, 100, 120, 140, 160}[W.Level] +
                    0.4f* Player.FlatMagicDamageMod + 2.5f*mana, true, true))*/
                if (W.IsReady() && source.Health

                    <= Player.GetSpellDamage(source, SpellSlot.W))
                {
                    W.Cast(source);
                }
            }
            //If E is Toggled and ready then execute
            if (OwlyzeMenu.LcE1())
            {
                /*Player.CalculateDamageOnUnit(source, DamageType.Magical,
                    new float[] { 54, 78, 102, 126, 150 }[E.Level] 
                    + 0.3f * Player.FlatMagicDamageMod + 0.03f * mana, true, true))*/

                if (E.IsReady() && source.Health <= Player.GetSpellDamage(source, SpellSlot.E))
                {
                    E.Cast(source);
                }
            }
         }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            if (source == null) return;

            if (OwlyzeMenu.JungleS() && OwlyzeMenu.JungleS1() <= Stacks)
            {
                return;
            }

            if (Q.IsReady() && OwlyzeMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
            }

            if (W.IsReady() && OwlyzeMenu.JungleW() && source.Distance(Player) <= W.Range)
            {
                W.Cast(source);
            }

            if (E.IsReady() && OwlyzeMenu.JungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source);
            }
            if (R.IsReady() && OwlyzeMenu.JungleR() && source.Distance(Player) <= R.Range)
            {
                R.Cast();
            }
        }


        private static void OnHarrass()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= 1100);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
            {
                foreach (var eenemies in enemies)
                {
                    var useE = OwlyzeMenu.MyHarass["harass.E"
                                                   + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            }

            if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                foreach (var wenemies in enemies)
                {
                    var useW = OwlyzeMenu.MyHarass["harass.W"
                                                   + wenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useW)
                    {
                        W.Cast(wenemies);
                    }
                }
            }

            if (target.IsValidTarget(Q.Range) && Player.ManaPercent >= OwlyzeMenu.HarassQE())
            {
                foreach (var qenemies in enemies)
                {
                    var useQ = OwlyzeMenu.MyHarass["harass.Q"
                                                   + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                    }
                }
            }
        }

        private static void NormalCombo()
        {
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var enemiesr = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesw = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= W.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            var StacksOn = Player.HasBuff("ryzepassivecharged");
            if (Stacks <= 1 && !StacksOn)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }


                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (Stacks == 2)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }

            if (Stacks == 3)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (Stacks == 4)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }

            if (StacksOn)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (StacksOn)
            {

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            else if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                foreach (var cageenemies in enemiesw)
                {
                    var useW = OwlyzeMenu.MyCombo["combo.w"
                                                  + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                    if (useW)
                    {
                        W.Cast(cageenemies);
                    }

                }
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                  + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                    }
                }
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = OwlyzeMenu.MyCombo["combo.e"
                                                  + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            if (R.IsReady() && (Stacks == 4 || StacksOn))
            {
                if (Q.IsReady() && !W.IsReady() && !E.IsReady())
                {
                    R.Cast();
                }
            }
        }

        private static void SluttyCombo()
        {
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var enemiesr = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesw = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= W.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);
            var Stacks = Player.GetBuffCount("ryzepassivestack");
            var StacksOn = Player.HasBuff("ryzepassivecharged");
            if (Stacks <= 1 && !StacksOn)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }


                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (Stacks == 2)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }

            if (Stacks == 3)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (Stacks == 4)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }

            if (StacksOn)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    foreach (var cageenemies in enemiesw)
                    {
                        var useW = OwlyzeMenu.MyCombo["combo.w"
                                                      + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                        if (useW)
                        {
                            W.Cast(cageenemies);
                        }

                    }
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            if (StacksOn)
            {

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    foreach (var qenemies in enemiesq)
                    {
                        var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                      + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useQ)
                        {
                            Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                        }
                    }
                }

                if (E.IsReady() && target.IsValidTarget(E.Range))
                    foreach (var eenemies in enemies)
                    {
                        var useE = OwlyzeMenu.MyCombo["combo.e"
                                                      + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useE)
                        {
                            E.Cast(eenemies);
                        }
                    }

                if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                    foreach (var ultenemies in enemiesr)
                    {
                        var Snared = ultenemies.HasBuff("RyzeW");
                        var useR = OwlyzeMenu.MyCombo["combo.r"
                                                      + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR && Snared)
                        {
                            R.Cast();
                        }
                    }
            }
            else if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                foreach (var cageenemies in enemiesw)
                {
                    var useW = OwlyzeMenu.MyCombo["combo.w"
                                                  + cageenemies.ChampionName].Cast<CheckBox>().CurrentValue;

                    if (useW)
                    {
                        W.Cast(cageenemies);
                    }

                }
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = OwlyzeMenu.MyCombo["combo.Q"
                                                  + qenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useQ)
                    {
                        Q.Cast(Q.GetPrediction(qenemies).CastPosition);
                    }
                }
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = OwlyzeMenu.MyCombo["combo.e"
                                                  + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            if (R.IsReady() && (Stacks == 4 || StacksOn))
            {
                if (Q.IsReady() && !W.IsReady() && !E.IsReady())
                {
                   R.Cast();
                }
            }
        }
    }
}
