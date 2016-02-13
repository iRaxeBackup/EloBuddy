using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

using Color = System.Drawing.Color;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nautilus
{
    public static class Program
    {

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static string version = "2.0.1";
        public static AIHeroClient Target = null;
        public static int qOff = 0, wOff = 0, eOff = 0, rOff = 0;
        public static int[] AbilitySequence;
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Targeted R;
        public static int CurrentSkin = 0;

        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            Bootstrap.Init(null);

        }



        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Nautilus") return;
            AbilitySequence = new int[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };

            Chat.Print("NautiBOSS Loaded!", Color.CornflowerBlue);
            Chat.Print("Enjoy the game and DONT TROLL!", Color.Red);
            NautibosMenu.loadMenu();
            NautibosActivator.loadSpells();
            Game.OnTick += GameOnTick;

            Game.OnUpdate += OnGameUpdate;

            #region Skill
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);
            #endregion

            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Orbwalker.OnPostAttack += OnPostAttack;
        }

        private static void GameOnTick(EventArgs args)
        {
            if (NautibosMenu.miscmenu["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) OnCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) OnLaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) OnJungle();
            Ascension();
            RanduinU();
        }

        private static void OnPostAttack(AttackableUnit Target, EventArgs args)
        {
            if (W.IsReady() && _Player.Distance(Target) <= W.Range)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    W.Cast();
                    Orbwalker.ResetAutoAttack();
                    Player.IssueOrder(GameObjectOrder.AttackTo, Target);
                }
            }
        }

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {

            if (gapcloser.End.Distance(_Player.ServerPosition) <= 300)
            {
                W.Cast(gapcloser.End.Extend(_Player.Position, _Player.Distance(gapcloser.End) + E.Range).To3D());
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (NautibosActivator.smite != null)
                smite();
            if (NautibosActivator.heal != null)
                Heal();
            if (NautibosActivator.ignite != null)
                ignite();
            if (NautibosActivator.exhaust != null)
                exhaust();
            if (NautibosMenu.skinId() != CurrentSkin)
            {
                Player.SetSkinId(NautibosMenu.skinId());
                CurrentSkin = NautibosMenu.skinId();
            }
        }


        public static void Drawing_OnDraw(EventArgs args)
        {
            if (NautibosMenu.nodraw()) return;
            if (!NautibosMenu.onlyReady())
            {
                if (NautibosMenu.drawingsQ())
                {
                    new Circle() { Color = Color.AliceBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (NautibosMenu.drawingsW())
                {
                    new Circle() { Color = Color.OrangeRed, Radius = W.Range, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (NautibosMenu.drawingsE())
                {
                    new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (NautibosMenu.drawingsR())
                {
                    new Circle() { Color = Color.SkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (NautibosMenu.Allydrawn())
                {
                    DrawHealths();
                }

            }
            else
            {
                if (!Q.IsOnCooldown && NautibosMenu.drawingsQ())
                {

                    new Circle() { Color = Color.AliceBlue, Radius = 340, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!W.IsOnCooldown && NautibosMenu.drawingsW())
                {

                    new Circle() { Color = Color.OrangeRed, Radius = 800, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!E.IsOnCooldown && NautibosMenu.drawingsE())
                {

                    new Circle() { Color = Color.Cyan, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (!R.IsOnCooldown && NautibosMenu.drawingsR())
                {

                    new Circle() { Color = Color.SkyBlue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
                }
                if (NautibosMenu.Allydrawn())
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
        public static void ChangeDSkin(Object sender, EventArgs args)
        {
            Player.SetSkinId(NautibosMenu.skinId());
        }


        public static void LevelUpSpells()
        {
            int qL = _Player.Spellbook.GetSpell(SpellSlot.Q).Level + qOff;
            int wL = _Player.Spellbook.GetSpell(SpellSlot.W).Level + wOff;
            int eL = _Player.Spellbook.GetSpell(SpellSlot.E).Level + eOff;
            int rL = _Player.Spellbook.GetSpell(SpellSlot.R).Level + rOff;
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

        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (NautibosMenu.miscmenu["interruptq"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.GetPrediction(Target).HitChance >= HitChance.High)
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Q.Range && Q.GetPrediction(Target).HitChance >= HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (NautibosMenu.miscmenu["interruptr"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= R.Range)
                {
                    R.Cast();
                }
            }
        }

        private static void exhaust()
        {
            if (!NautibosActivator.exhaust.IsReady() || Player.Instance.IsDead) return;
            if (NautibosMenu.miscmenu["useExhaust"].Cast<CheckBox>().CurrentValue && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies
                        .Where(a => a.IsValidTarget(NautibosActivator.exhaust.Range))
                        .Where(enemy => NautibosMenu.miscmenu[enemy.ChampionName + "exhaust"].Cast<CheckBox>().CurrentValue))
            {
                if (enemy.IsFacing(Player.Instance))
                {
                    if (!(Player.Instance.HealthPercent < 50)) continue;
                    NautibosActivator.exhaust.Cast(enemy);
                    return;
                }
                if (!(enemy.HealthPercent < 50)) continue;
                NautibosActivator.exhaust.Cast(enemy);
                return;
            }
        }

        private static void Ascension()
        {
            if (NautibosActivator.talisman.IsReady() && NautibosActivator.talisman.IsOwned())
            {
                var ascension = NautibosMenu.itemsMenu["talisman"].Cast<CheckBox>().CurrentValue;
                if (ascension && _Player.HealthPercent <= 15 && _Player.CountEnemiesInRange(800) >= 1 ||
                    _Player.CountEnemiesInRange(NautibosActivator.talisman.Range) >= 3)
                {
                    NautibosActivator.talisman.Cast();
                }
            }
        }
        private static void RanduinU()
        {
            if (NautibosActivator.randuin.IsReady() && NautibosActivator.randuin.IsOwned())
            {
                var randuin = NautibosMenu.itemsMenu["randuin"].Cast<CheckBox>().CurrentValue;
                if (randuin && _Player.HealthPercent <= 15 && _Player.CountEnemiesInRange(NautibosActivator.randuin.Range) >= 1 ||
                    _Player.CountEnemiesInRange(NautibosActivator.randuin.Range) >= 2)
                {
                    NautibosActivator.randuin.Cast();
                }
            }
        }

        public static void OnCombo()
        {
            Target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (Target == null) return;

            if (Target.IsValidTarget())
            {
                HitChance h = HitChance.Medium;
                if (NautibosMenu.HitchanceQ == 1)
                    h = HitChance.Low;
                else if (NautibosMenu.HitchanceQ == 2)
                    h = HitChance.Medium;
                else if (NautibosMenu.HitchanceQ == 3)
                    h = HitChance.High;
                if (Target != null)
                    if (Q.IsReady() && NautibosMenu.nauticombo["combo.Q"].Cast<CheckBox>().CurrentValue && Q.GetPrediction(Target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Target);

                    }
                var alvo = TargetSelector.GetTarget(1000, DamageType.Magical);

                if (E.IsReady() && NautibosMenu.nauticombo["combo.E"].Cast<CheckBox>().CurrentValue && Player.Instance.IsInAutoAttackRange(Target))
                {
                    E.Cast();

                }

                if (W.IsReady() && NautibosMenu.nauticombo["combo.W"].Cast<CheckBox>().CurrentValue && Player.Instance.IsInAutoAttackRange(Target))
                {
                    W.Cast();

                }
                var enemies = EntityManager.Heroes.Enemies.OrderByDescending(a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(_Player) <= R.Range);
                var checkhp = NautibosMenu.nauticombo["hptoult"].Cast<Slider>().CurrentValue;
                var manual = NautibosMenu.nautir["manu.ult"].Cast<CheckBox>().CurrentValue;
                if (R.IsReady() && !manual)
                {
                    foreach (var ultenemies in enemies)

                    {
                        var useR = NautibosMenu.nautir["r.ult" + ultenemies.ChampionName].Cast<CheckBox>().CurrentValue;
                        if (ultenemies.HealthPercent <= checkhp)
                        {
                            if (useR)
                            {
                                R.Cast(ultenemies);
                            }
                        }
                    }
                }
                return;
            }
        }



        public static void OnLaneClear()
        {

            Orbwalker.ForcedTarget = null;
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, E.Range, false).Count();
            if (count == 0) return;
            if (E.IsReady() && NautibosMenu.nautilcs["lc.E"].Cast<CheckBox>().CurrentValue && NautibosMenu.nautilcs["lc.MinionsW"].Cast<Slider>().CurrentValue <= count)
            {
                E.Cast();
                return;
            }
            return;

        }

        public static void OnJungle()
        {
            Orbwalker.ForcedTarget = null;

            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.ServerPosition, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (source == null) return;

            if (Q.IsReady() && NautibosMenu.nautilcs["jungle.Q"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) <= Q.Range)
            {
                Q.Cast(source);
                return;
            }

            if (W.IsReady() && NautibosMenu.nautilcs["jungle.W"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < _Player.GetAutoAttackRange(source))
            {
                W.Cast();
                return;

            }
            if (E.IsReady() && NautibosMenu.nautilcs["jungle.E"].Cast<CheckBox>().CurrentValue && source.Distance(_Player) < _Player.GetAutoAttackRange(source))
            {
                E.Cast();
                return;
            }
            return;
        }
        public static void ignite()
        {
            var autoIgnite = TargetSelector.GetTarget(NautibosActivator.ignite.Range, DamageType.True);
            if (autoIgnite != null && autoIgnite.Health <= DamageLibrary.GetSpellDamage(Player.Instance, autoIgnite, NautibosActivator.ignite.Slot) || autoIgnite != null && autoIgnite.HealthPercent <= NautibosMenu.spellsHealignite())
                NautibosActivator.ignite.Cast(autoIgnite);

        }
        public static void Heal()
        {
            if (NautibosActivator.heal.IsReady() && Player.Instance.HealthPercent <= NautibosMenu.spellsHealhp())
                NautibosActivator.heal.Cast();
        }
        public static void smite()
        {
            var unit = ObjectManager.Get<Obj_AI_Base>().Where(a => NautibosMobs.MinionNames.Contains(a.BaseSkinName) && DamageLibrary.GetSummonerSpellDamage(Player.Instance, a, DamageLibrary.SummonerSpells.Smite) >= a.Health && NautibosMenu.smitePage[a.BaseSkinName].Cast<CheckBox>().CurrentValue && NautibosActivator.smite.IsInRange(a)).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            if (unit != null && NautibosActivator.smite.IsReady())
                NautibosActivator.smite.Cast(unit);
        }

    }
}





