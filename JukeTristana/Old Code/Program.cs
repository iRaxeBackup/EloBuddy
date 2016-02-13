using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Boostana
{
    public static class Program
    {
        //The string version is called in MyMenu Class where we type Program.Version 
        public static string Version = "2.0.0.5"; 
        public static AIHeroClient Target = null;
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        private static int[] AbilitySequence;
        private static WardLocation wardLocation;
        //Now we set every spell like Q-W-E-R
        //Q is an active spell (Let tristana attack speed goes 12938561281273x)
        private static Spell.Active Q;
        //W is a SkillShot spell because we have to "normally" click with the mouse to the desired position
        private static Spell.Skillshot W;
        //E is a Targeted spell, like ignite, we have to click on the enemy for fappa fappa BUM
        private static Spell.Targeted E;
        //R is also a Targeted spell, Like E or Ignite, we have to KABUUUUUUUUUUUUM the enemy
        private static Spell.Targeted R;
        private static int time;
        //This works for find the Ally (Koka function but he is so nob for let insec works then we will check it)
        private static Obj_AI_Base AllyTarget;
        //This works for find the target (Koka function but he is so nob for let insec works then we will check it)
        private static AIHeroClient EnemyTarget;
        //This works for find the position for insec W+R (Koka function but he is so nob for let insec works then we will check it)
        private static Vector3 InsecPos;
        //This works for check if the keybind is active (Koka function but he is so nob for let insec works then we will check it)
        private static bool InsecActive;
        //I REALLY DONT KNOW WE WILL CHECK IT IN THE ADDON FAPPA
        public static bool WtfSecActive;
        private static long LastUpdate;
        //I REALLY DONT KNOW WE WILL CHECK IT IN THE ADDON FAPPA
        public static bool ShouldFlash;

        private static readonly AIHeroClient Player = ObjectManager.Player;

        private static readonly AIHeroClient InsecTarget = EnemyTarget;

        internal static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
            Bootstrap.Init(null);
        }


        private static void OnLoadingComplete(EventArgs args)
        //Let's start with the addon, when the loading screen finished then the addon have to start.
        {
            // With if player.ChampionName we are searching if out champion is Tristana, if we put != is If not tristana then Fack off
            if (Player.ChampionName != "Tristana") return;
            //Abilitysequence is needed for my autolvlup, we will see it in the program
            AbilitySequence = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
            //Like every nob addon, we want to write something in chat like our addon is loaded and some spam like this 
            //Using Chat.Print helps you to write this, you can also choose how to type it with Color.Red like Annie bad days
            Chat.Print("Boostana Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT FLAME!", Color.Red);
            //We start to load every single thing we need for let the addon works properly or every file we have separately to this
            TristanaMenu.LoadMenu();
            wardLocation = new WardLocation();
            Game.OnTick += GameOnTick;
            MyActivator.LoadSpells();
            Game.OnUpdate += OnGameUpdate;

            #region Skill
            //As i said first, When we load the game we have also to set the skills right?
            //Then let's do it with each skill 
            //Q was active then we have to set it as active 
            Q = new Spell.Active(SpellSlot.Q, 550);
            //W was a skillshot, precisely a circular one so we have to write Spell.Skillshot
            //For the skillshot we have to define if it is linear, circual or madafaker
            //We call it by SkillShotType
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 450, int.MaxValue, 180);
            E = new Spell.Targeted(SpellSlot.E, 550);
            R = new Spell.Targeted(SpellSlot.R, 550);

            #endregion

            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            GameObject.OnCreate += GameObject_OnCreate;
            Drawing.OnDraw += GameOnDraw;
        }

        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (!sender.IsMe) return;
            Q = new Spell.Active(SpellSlot.Q, 543 + (7 * (uint)Player.Level));
            E = new Spell.Targeted(SpellSlot.E, 543 + (7 * (uint)Player.Level));
            R = new Spell.Targeted(SpellSlot.R, 543 + (7 * (uint)Player.Level));
        }

        private static void GameOnDraw(EventArgs args)
        {
            if (TristanaMenu.Nodraw()) return;

            if (InsecTarget.IsValidTarget())
            {
                Circle.Draw(SharpDX.Color.Red, InsecTarget.BoundingRadius + 100, InsecTarget.Position);
            }
            if (AllyTarget.IsValidTarget())
            {
                Circle.Draw(SharpDX.Color.BlueViolet, AllyTarget.BoundingRadius + 100, AllyTarget.Position);
            }

            if (!TristanaMenu.OnlyReady())
            {
                if (TristanaMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (TristanaMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (TristanaMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (TristanaMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (TristanaMenu.DrawingsT() && wardLocation.Normal.Any())
                {
                    foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1500))
                    {
                        Drawing.DrawCircle(place, 100, IsWarded(place) ? Color.Red : Color.Green);
                    }
                }
            }
            else
            {
                if (!Q.IsOnCooldown && TristanaMenu.DrawingsQ())
                {
                    new Circle { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!W.IsOnCooldown && TristanaMenu.DrawingsW())
                {
                    new Circle { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!E.IsOnCooldown && TristanaMenu.DrawingsE())
                {
                    new Circle { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (!R.IsOnCooldown && TristanaMenu.DrawingsR())
                {
                    new Circle { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(Player.Position);
                }
                if (TristanaMenu.DrawingsT() && wardLocation.Normal.Any())
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
            if (TristanaMenu.checkSkin())
            {
            Player.SetSkinId(TristanaMenu.SkinId());
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

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !TristanaMenu.GapcloserR() || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }

            R.Cast(e.Sender);
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }
            if (R.IsReady() && R.IsInRange(sender) && TristanaMenu.GapcloserR1())
            {
                R.Cast(sender);
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var rengar = EntityManager.Heroes.Enemies.Find(r => r.ChampionName.Equals("Rengar"));
            var khazix = EntityManager.Heroes.Enemies.Find(z => z.ChampionName.Equals("Khazix"));
            if (khazix != null)
            {
                if (sender.Name == ("Khazix_Base_E_Tar.troy") && TristanaMenu.GapcloserR2() &&
                    sender.Position.Distance(Player) <= 400)
                    R.Cast(khazix);
            }
            if (rengar == null) return;
            if (sender.Name == ("Rengar_LeapSound.troy") && TristanaMenu.GapcloserR3() &&
                sender.Position.Distance(Player) < R.Range)
                R.Cast(rengar);
        }

        private static void Barrier()
        {
            if (Player.IsFacing(Target) && MyActivator.Barrier.IsReady() && Player.HealthPercent <= TristanaMenu.spellsBarrierHP())
                MyActivator.Barrier.Cast();
        }
           
        private static void Ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(MyActivator.Ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= Player.GetSpellDamage(autoIgnite, MyActivator.Ignite.Slot) ||
                autoIgnite != null && autoIgnite.HealthPercent <= TristanaMenu.SpellsIgniteFocus())
                MyActivator.Ignite.Cast(autoIgnite);
        }

        private static void Heal()
        {
            if (Player.IsFacing(Target) && MyActivator.Heal.IsReady() && Player.HealthPercent <= TristanaMenu.SpellsHealHp())
                MyActivator.Heal.Cast();
        }

        private static void GameOnTick(EventArgs args)
        {
            if (!InsecActive || LastUpdate + 200 <= Environment.TickCount)
            {
                InsecPos = new Vector3();
            }

            if (TristanaMenu.Lvlup()) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) OnHarrass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) OnFlee();
            if (TristanaMenu.MyCombo["combo.WR"].Cast<KeyBind>().CurrentValue)
            {
                Insec();
            }
            KillSteal();
            AutoE();
            AutoPotions();
            AutoWard();
        }

        private static void AutoWard()
        {
            if (TristanaMenu.checkWard())
            {
                foreach (var place in wardLocation.Normal.Where(pos => pos.Distance(ObjectManager.Player.Position) <= 1000))
                {
                    if (MyActivator.WardingTotem.IsOwned() && MyActivator.WardingTotem.IsReady() && TristanaMenu.wardingTotem() && !IsWarded(place))
                    {
                        MyActivator.WardingTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterStealthTotem.IsOwned() && MyActivator.GreaterStealthTotem.IsReady() && TristanaMenu.greaterStealthTotem() &&!IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterStealthTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.GreaterVisionTotem.IsOwned() && MyActivator.GreaterVisionTotem.IsReady() && TristanaMenu.greaterVisionTotem() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.GreaterVisionTotem.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.FarsightAlteration.IsOwned() && MyActivator.FarsightAlteration.IsReady() && TristanaMenu.farsightAlteration() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.FarsightAlteration.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                    if (MyActivator.PinkVision.IsOwned() && MyActivator.PinkVision.IsReady() && TristanaMenu.pinkWard() && !IsWarded(place) && (Environment.TickCount > time))
                    {
                        MyActivator.PinkVision.Cast(place);
                        time = Environment.TickCount + 5000;
                    }
                }
            }
        }

        private static void AutoPotions()
        {
            if (TristanaMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.HealthPercent <= TristanaMenu.SpellsPotionsHP() &&
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
            if (TristanaMenu.SpellsPotionsCheck() && !Player.IsInShopRange() &&
                Player.ManaPercent <= TristanaMenu.SpellsPotionsM() &&
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

            if (args.Buff.Type == BuffType.Taunt && TristanaMenu.Taunt())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Stun && TristanaMenu.Stun())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Snare && TristanaMenu.Snare())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Polymorph && TristanaMenu.Polymorph())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Blind && TristanaMenu.Blind())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Flee && TristanaMenu.Fear())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Charm && TristanaMenu.Charm())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Suppression && TristanaMenu.Suppression())
            {
                    DoQSS();
            }
            if (args.Buff.Type == BuffType.Silence && TristanaMenu.Silence())
            {
                    DoQSS();
            }
            if (args.Buff.Name == "zedulttargetmark" && TristanaMenu.ZedUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "VladimirHemoplague" && TristanaMenu.VladUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "FizzMarinerDoom" && TristanaMenu.FizzUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "MordekaiserChildrenOfTheGrave" && TristanaMenu.MordUlt())
            {
                UltQSS();
            }
            if (args.Buff.Name == "PoppyDiplomaticImmunity" && TristanaMenu.PoppyUlt())
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
        private static Vector3 InterceptionPoint(List<Obj_AI_Base> heroes)
        {
            var result = new Vector3();
            result = heroes.Aggregate(result, (current, hero) => current + hero.Position);
            result.X /= heroes.Count;
            result.Y /= heroes.Count;
            return result;
        }

        public static AIHeroClient GetTargetForInsec()
        {
            return TargetSelector.GetTarget(1400, DamageType.Physical);
        }

        private static Vector3 GetBestInsecPos()
        {
            switch (TristanaMenu.MyCombo["insecPositionMode"].Cast<Slider>().CurrentValue)
            {
                case 0:
                    var b =
                        ObjectManager.Get<Obj_AI_Base>()
                            .Where(
                                a =>
                                    (a is AIHeroClient || a is Obj_AI_Turret) && a.IsAlly &&
                                    a.Distance(InsecTarget.Position) < 2000 && !a.IsMe && a.Health > 0).ToList();

                    return b.Any() ? InterceptionPoint(b.ToList()) : Game.CursorPos;
                case 1:
                    return Game.CursorPos;
                case 2:
                    return AllyTarget.Position;
            }
            return new Vector3();
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            var enemyT =
                EntityManager.Heroes.Enemies
                    .Where(
                        a =>
                            a.IsValid && a.Health > 0 && (a.IsEnemy) && a.Distance(Game.CursorPos) < 200)
                    .ToList()
                    .OrderBy(a => a.Distance(Game.CursorPos))
                    .FirstOrDefault();

            if (enemyT != null)
            {
                EnemyTarget = enemyT;
                return;
            }

            var allyT =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(
                        a =>
                            a.IsValid && a.Health > 0 && (a.IsAlly) && a.Distance(Game.CursorPos) < 200 &&
                            (a is AIHeroClient || a is Obj_AI_Minion || a is Obj_AI_Turret) && !a.IsMe)
                    .ToList()
                    .OrderBy(a => a.Distance(Game.CursorPos))
                    .FirstOrDefault();
            if (allyT != null && TristanaMenu.MyCombo["insecPositionMode"].Cast<Slider>().CurrentValue == 2)
            {
                AllyTarget = allyT;
                return;
            }

            AllyTarget = null;
            EnemyTarget = null;
        }

        private static void Insec()
        {
            var target = EnemyTarget;

            Orbwalker.OrbwalkTo(TristanaMenu.MyCombo["insecPositionMode"].Cast<Slider>().CurrentValue == 1 &&
                                target != null || GetBestInsecPos() == Game.CursorPos && target != null
                ? target.Position
                : Game.CursorPos);


            if (target == null || !target.IsValidTarget())
                return;
            var allyPos = GetBestInsecPos();
            if (InsecPos == new Vector3())
            {
                var insecPos =
                    allyPos.Extend(target.Position,
                        target.Distance(allyPos) + TristanaMenu.MyCombo["insecDistance"].Cast<Slider>().CurrentValue)
                        .To3D();
                InsecPos = insecPos;
                LastUpdate = Environment.TickCount;
            }
            if (!R.IsReady())
            {
                OnCombo();
                return;
            }

            if (Player.Distance(InsecPos) < 200)
            {
                R.Cast(target);
                return;
            }
            if (Player.Distance(InsecPos) > 200)
            {
                R.Cast(InsecPos);
            }
        }

        private static void KillSteal()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range && !a.IsDead && !a.IsZombie && a.HealthPercent <= 25);
            foreach (
                var target2 in
                   enemies)
            {
                var tawah =
                    EntityManager.Turrets.Enemies.FirstOrDefault(
                        a =>
                            !a.IsDead &&
                            a.Distance(target2) <= 775 + Player.BoundingRadius + (target2.BoundingRadius / 2) + 44.2);
                var useWR = TristanaMenu.MyHarass["killsteal.WR"
                    + target2.ChampionName].Cast<CheckBox>().CurrentValue;
                if (useWR && TristanaMenu.KillstealR() && R.IsReady() &&
                    target2.Health + target2.AttackShield <
                    Player.GetSpellDamage(target2, SpellSlot.R) && !target2.HasBuff("tristanaecharge"))

                    {
                    R.Cast(target2);
                    }

                if (useWR && TristanaMenu.KillstealW() && W.IsReady() &&
                    target2.Health + target2.AttackShield <
                    Player.GetSpellDamage(target2, SpellSlot.W) &&
                    target2.Position.CountEnemiesInRange(800) == 1 && tawah == null && Player.Mana >= 120)
                {
                    W.Cast(target2.Position);
                }
            }

            }

        private static void AutoE()
        {
            if (!TristanaMenu.MyCombo["combo.CC"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }
            var autoETarget =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    x =>
                        x.HasBuffOfType(BuffType.Charm) || x.HasBuffOfType(BuffType.Knockup) ||
                        x.HasBuffOfType(BuffType.Stun) || x.HasBuffOfType(BuffType.Suppression) ||
                        x.HasBuffOfType(BuffType.Snare));
            if (autoETarget != null && !autoETarget.HasBuff("tristanaecharge"))
            {
                E.Cast(autoETarget);
            }
        }

        private static void OnFlee()
        {
            if (W.IsReady() && Player.ManaPercent >= TristanaMenu.FleeM())
            {
                W.Cast(Player.ServerPosition.Extend(Game.CursorPos, W.Range).To3D()); //Surely not stole from MarioGK 
            }
            if (R.IsReady() && Player.HealthPercent <= TristanaMenu.FleeHP())
            {
                R.Cast(Target);
            }
        }
        private static void OnLaneClear()
        {
            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                    Player.AttackRange, false).Count();
            var tawah = EntityManager.Turrets.Enemies.FirstOrDefault(t => !t.IsDead && t.IsInRange(Player, 800));
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                    Player.AttackRange).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            var sourceE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .FirstOrDefault(m => m.IsValidTarget(Player.AttackRange) && m.GetBuffCount("tristanaecharge") > 0);
            if (count == 0) return;
            if (E.IsReady() && TristanaMenu.LcE() && source.IsValidTarget(E.Range) &&
                Player.ManaPercent >= TristanaMenu.LcM())
            {
                E.Cast(source);
                Orbwalker.ForcedTarget = sourceE;
            }
            if (Q.IsReady() && TristanaMenu.LcQ() && source.IsValidTarget(Q.Range) &&
                Player.ManaPercent >= TristanaMenu.LcM())
            {
                Q.Cast();
            }
            if (W.IsReady() && TristanaMenu.LcW() && TristanaMenu.LcW1() <= count &&
                Player.ManaPercent >= TristanaMenu.LcM())
            {
                if (source != null) W.Cast(source.Position);
            }
            if (tawah == null) return;
            if (TristanaMenu.LcE1() && tawah.IsInRange(Player, E.Range) && E.IsReady() &&
                Player.ManaPercent >= TristanaMenu.LcM())
            {
                E.Cast(tawah);
                Q.Cast();
            }

            if (TristanaMenu.LcQ() && tawah.IsInRange(Player, Q.Range) && Q.IsReady() &&
                Player.ManaPercent >= TristanaMenu.LcM())
            {
                Q.Cast();
            }
        }

        private static void OnJungle()
        {
            var source =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Q.Range)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();
            if (source == null) return;
            if (Q.IsReady() && TristanaMenu.JungleQ() && source.Distance(Player) <= Q.Range)
            {
                Q.Cast();
            }
                if (W.IsReady() && TristanaMenu.JungleW() && source.Distance(Player) <= W.Range)
                {
                    W.Cast(source.ServerPosition);
                }
            if (E.IsReady() && TristanaMenu.JungleE() && source.Distance(Player) <= E.Range)
            {
                E.Cast(source);
            }
        }

        private static void OnHarrass()
        {
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                 foreach (var eenemies in enemies)
                {
                    var useE = TristanaMenu.MyCombo["harass.E"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }

            if (TristanaMenu.HarassQ() && target.IsValidTarget(Q.Range) && Player.ManaPercent >= TristanaMenu.HarassQe() &&
                target.GetBuffCount("tristanaecharge") > 0)
            {
                Q.Cast();
            }
        }

        private static void OnCombo()
        {
            var enemiesr = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesw = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= W.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var targetBoom =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    a => a.HasBuff("tristanaecharge") && a.Distance(Player) < Player.AttackRange);
            if (!target.IsValidTarget(Q.Range) || target == null)
            {
                return;
            }
            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = TristanaMenu.MyCombo["combo.e"
                        + eenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            if (TristanaMenu.ComboQ() && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                Q.Cast();
            }
            if (W.IsReady() && target.IsValidTarget(W.Range) && !target.IsInvulnerable &&
                target.Position.CountEnemiesInRange(800) <= TristanaMenu.ComboW1() )
                foreach (var jumpenemies in enemiesw)
                {
                    var useW = TristanaMenu.MyCombo["combo.w"
                        + jumpenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (useW && W.GetPrediction(jumpenemies).HitChancePercent >= TristanaMenu.ComboWH())
                    {
                        W.Cast(W.GetPrediction(jumpenemies).CastPosition);
                    }
                }

            if (targetBoom != null)
                if (TristanaMenu.ComboEr() && !E.IsReady() && R.IsReady() && targetBoom.IsValidTarget(R.Range) && !targetBoom.IsInvulnerable &&
                    (targetBoom.Health + targetBoom.AllShield + TristanaMenu.ComboEr1()) -
                    (Player.GetSpellDamage(targetBoom, SpellSlot.E) +
                     (targetBoom.Buffs.Find(a => a.Name == "tristanaecharge").Count *
                      Player.GetSpellDamage(targetBoom, SpellSlot.E, DamageLibrary.SpellStages.Detonation))) <
                    Player.GetSpellDamage(targetBoom, SpellSlot.R))
                {
                    R.Cast(targetBoom);
                }

            if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable &&
                target.Health + target.AttackShield + TristanaMenu.ComboR1() <
                Player.GetSpellDamage(target, SpellSlot.R))
                foreach (var ultenemies in enemiesr)
                {
                    var useR = TristanaMenu.MyCombo["combo.r" 
                        + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (useR)
                        {
                            R.Cast(ultenemies);
                        }
                }

            if ((ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange) >=
                 TristanaMenu.YoumusEnemies() || Player.HealthPercent >= TristanaMenu.ItemsYoumuShp()) &&
                MyActivator.Youmus.IsReady() && TristanaMenu.Youmus() && MyActivator.Youmus.IsOwned())
            {
                MyActivator.Youmus.Cast();
                return;
            }
            if (Player.HealthPercent <= TristanaMenu.BilgewaterHp() && TristanaMenu.Bilgewater() &&
                MyActivator.Bilgewater.IsReady() && MyActivator.Bilgewater.IsOwned())
            {
                MyActivator.Bilgewater.Cast(target);
                return;
            }

            if (Player.HealthPercent <= TristanaMenu.BotrkHp() && TristanaMenu.Botrk() && MyActivator.Botrk.IsReady() &&
                MyActivator.Botrk.IsOwned())
            {
                MyActivator.Botrk.Cast(target);
            }
        }
    }
}
