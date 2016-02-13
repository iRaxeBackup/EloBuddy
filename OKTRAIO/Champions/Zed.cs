using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Utility;

namespace OKTRAIO.Champions
{
    internal class Zed : AIOChampion
    {
        #region Initalize and Declare

        private static readonly List<Obj_AI_Base> ActiveShadows = new List<Obj_AI_Base>();

        private static Obj_AI_Base WShadow;
        private static Obj_AI_Base RShadow;

        private static Spell.Skillshot Q, W;
        private static Spell.Active E;
        private static Spell.Targeted R;

        private static bool WIsSwitch;
        private static bool RIsSwitch;

        private static bool IsDoingCombo;

        private static bool IsValidNotNull(Obj_AI_Base target)
        {
            if (target != null && target.IsValid)
                return true;
            return false;
        }

        public override void Init()
        {
            try
            {
                try
                {
                    Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1650, 45);
                    W = new Spell.Skillshot(SpellSlot.W, 650, SkillShotType.Linear, 250, 1400, 60);
                    E = new Spell.Active(SpellSlot.E, 280);
                    R = new Spell.Targeted(SpellSlot.R, 625);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code SPELLS)</font>");
                }

                try
                {
                    // combo
                    MainMenu.ComboKeys();
                    MainMenu.Combo.AddSlider("combo.mode", "Combo Mode", 2, 1, 3, true);
                    MainMenu.Combo["combo.mode"].Cast<Slider>().OnValueChange += ComboStringList;
                    MainMenu.Combo.AddSlider("combo.distant.health", "Distant Mode Health: ", 25, 0, 100, true);
                    MainMenu.Combo.AddCheckBox("combo.switch", "Switch Shadows After combo", false, true);

                    // draw
                    MainMenu.DamageIndicator();
                    MainMenu.DrawKeys();
                    MainMenu.Draw.AddGroupLabel("W Shadow Settings", "draw.w.settings", true);
                    MainMenu.Draw.AddCheckBox("draw.w.q", "Draw Q", true, true);
                    MainMenu.Draw.AddCheckBox("draw.w.e", "Draw E", true, true);
                    MainMenu.Draw.AddGroupLabel("R Shadow Settings", "draw.r.settings", true);
                    MainMenu.Draw.AddCheckBox("draw.r.q", "Draw Q", true, true);
                    MainMenu.Draw.AddCheckBox("draw.r.e", "Draw E", true, true);

                    // flee
                    MainMenu.FleeKeys(false, useE: false, useR: false);
                    MainMenu.Flee.AddSlider("flee.w.delay", "Switch Delay", 5, 0, 100, true);

                    // lane
                    MainMenu.LaneKeys(useR: false);
                    MainMenu.LaneManaManager(true, true, true, false, 40, 50, 40, 100);

                    // ks
                    MainMenu.KsKeys(useR: false);
                    MainMenu.Ks.AddSlider("killsteal.q.hitchance", "Q Hitchance", 75, 1, 100, true);

                    // last hit
                    MainMenu.LastHitKeys(true, true, false, defaultE: true);
                    MainMenu.Lasthit.AddCheckBox("lasthit.notkillablebyaa",
                        "Only use Spells if Minion is not Killable by AA", false, true);

                    // harass
                    MainMenu.HarassKeys(useR: false);
                    MainMenu.Harass.AddCheckBox("harass.autoe", "Auto E", true);
                    MainMenu.Harass.AddCheckBox("harass.autoe.combodisable", "Disable When Combo", true, true);
                    MainMenu.Harass.AddCheckBox("harass.autoe.w", "Auto E for W Shadow", true, true);
                    MainMenu.Harass.AddCheckBox("harass.autoe.r", "Auto E for R Shadow", true, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 2)</font>");
                }

                try
                {
                    // events
                    Value.Init();
                    Game.OnUpdate += Game_OnUpdate;
                    Obj_AI_Base.OnBuffGain += delegate(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
                    {
                        var shadow = sender as Obj_AI_Minion;
                        if (shadow == null || !shadow.IsValid || !args.Buff.Caster.IsMe || !args.Buff.IsValid)
                            return;

                        if (shadow.IsAlly && shadow.BaseSkinName.ToLower() == "zedshadow")
                        {
                            if (args.Buff.Name == "zedwshadowbuff")
                            {
                                WShadow = shadow;
                                if (!ActiveShadows.Contains(shadow))
                                    ActiveShadows.Add(shadow);
                            }
                            if (args.Buff.Name == "zedrshadowbuff")
                            {
                                RShadow = shadow;
                                if (!ActiveShadows.Contains(shadow))
                                    ActiveShadows.Add(shadow);
                            }
                        }
                    };
                    Obj_AI_Base.OnBuffLose += delegate(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
                    {
                        var shadow = sender as Obj_AI_Minion;
                        if (shadow == null || !shadow.IsValid || !args.Buff.Caster.IsMe || !args.Buff.IsValid)
                            return;

                        if (shadow.IsAlly && shadow.BaseSkinName.ToLower() == "zedshadow")
                        {
                            if (args.Buff.Name == "zedwshadowbuff")
                            {
                                WShadow = null;
                                if (ActiveShadows.Contains(shadow))
                                    ActiveShadows.Remove(shadow);
                            }
                            if (args.Buff.Name == "zedrshadowbuff")
                            {
                                RShadow = null;
                                if (ActiveShadows.Contains(shadow))
                                    ActiveShadows.Remove(shadow);
                            }
                        }
                    };

                    GameObject.OnCreate += delegate(GameObject sender, EventArgs args)
                    {
                        var shadow = sender as Obj_AI_Minion;
                        if (shadow != null && shadow.IsValid && shadow.CharData.BaseSkinName.ToLower() == "zedshadow" &&
                            shadow.IsAlly && shadow.IsVisible)
                        {
                            if (!ActiveShadows.Contains(shadow))
                                ActiveShadows.Add(shadow);
                        }
                    };
                    GameObject.OnDelete += delegate(GameObject sender, EventArgs args)
                    {
                        var shadow = sender as Obj_AI_Minion;
                        if (shadow != null && shadow.IsValid && shadow.CharData.BaseSkinName.ToLower() == "zedshadow" &&
                            shadow.IsAlly && shadow.IsVisible)
                        {
                            if (ActiveShadows.Contains(shadow))
                                ActiveShadows.Remove(shadow);
                        }
                    };

                    DamageIndicator.DamageToUnit = GetRawComboDamage;
                    Drawing.OnDraw += Drawing_OnDraw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Chat.Print(
                        "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 5)</font>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code 503)</font>");
            }
        }

        private static void ComboStringList(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            UpdateSlider(1);
        }

        private static void UpdateSlider(int id)
        {
            try
            {
                var displayName = "";
                switch (id)
                {
                    case 1:
                        displayName = "Combo Mode: ";
                        if (Value.Get("combo.mode") == 1)
                        {
                            displayName += "Line";
                        }
                        else if (Value.Get("combo.mode") == 2)
                        {
                            displayName += "Default";
                        }
                        else if (Value.Get("combo.mode") == 3)
                        {
                            displayName += "Distant";
                        }
                        MainMenu.Combo["combo.mode"].Cast<Slider>().DisplayName = displayName;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code UPDATESLIDER)</font>");
            }
        }

        #endregion

        #region Game-Related Logic

        #region Draw

        private void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Value.Use("draw.disable"))
                    return;

                if (Value.Use("draw.q"))
                {
                    if (Value.Use("draw.ready"))
                    {
                        if (Q.IsReady())
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                Color = MainMenu.Draw.GetColor("color.q"),
                                Radius = Q.Range
                            }.Draw(Player.Instance.Position);
                            if (WShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.w.q"))
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                    Color = MainMenu.Draw.GetColor("color.q"),
                                    Radius = Q.Range
                                }.Draw(WShadow.Position);
                            if (RShadow != null && WShadow != null && WShadow.IsValid && WShadow.IsVisible &&
                                Value.Use("draw.r.q"))
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                    Color = MainMenu.Draw.GetColor("color.q"),
                                    Radius = Q.Range
                                }.Draw(RShadow.Position);
                        }
                    }
                    else
                    {
                        new Circle
                        {
                            BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                            Color = MainMenu.Draw.GetColor("color.q"),
                            Radius = Q.Range
                        }.Draw(Player.Instance.Position);
                        if (WShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.w.q"))
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                Color = MainMenu.Draw.GetColor("color.q"),
                                Radius = Q.Range
                            }.Draw(WShadow.Position);
                        if (RShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.r.q"))
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.q"),
                                Color = MainMenu.Draw.GetColor("color.q"),
                                Radius = Q.Range
                            }.Draw(RShadow.Position);
                    }
                }
                if (Value.Use("draw.w"))
                {
                    if (Value.Use("draw.ready"))
                    {
                        if (W.IsReady())
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.w"),
                                Color = MainMenu.Draw.GetColor("color.w"),
                                Radius = W.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }
                    else
                    {
                        new Circle
                        {
                            BorderWidth = MainMenu.Draw.GetWidth("width.w"),
                            Color = MainMenu.Draw.GetColor("color.w"),
                            Radius = W.Range
                        }.Draw(Player.Instance.Position);
                    }
                }
                if (Value.Use("draw.e"))
                {
                    if (Value.Use("draw.ready"))
                    {
                        if (E.IsReady())
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                Color = MainMenu.Draw.GetColor("color.e"),
                                Radius = E.Range
                            }.Draw(Player.Instance.Position);
                            if (WShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.w.e"))
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                    Color = MainMenu.Draw.GetColor("color.e"),
                                    Radius = E.Range
                                }.Draw(WShadow.Position);
                            if (RShadow != null && WShadow != null && WShadow.IsValid && WShadow.IsVisible &&
                                Value.Use("draw.r.e"))
                                new Circle
                                {
                                    BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                    Color = MainMenu.Draw.GetColor("color.e"),
                                    Radius = E.Range
                                }.Draw(RShadow.Position);
                        }
                    }
                    else
                    {
                        new Circle
                        {
                            BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                            Color = MainMenu.Draw.GetColor("color.e"),
                            Radius = E.Range
                        }.Draw(Player.Instance.Position);
                        if (WShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.w.e"))
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                Color = MainMenu.Draw.GetColor("color.e"),
                                Radius = E.Range
                            }.Draw(WShadow.Position);
                        if (RShadow != null && WShadow.IsValid && WShadow.IsVisible && Value.Use("draw.r.e"))
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.e"),
                                Color = MainMenu.Draw.GetColor("color.e"),
                                Radius = E.Range
                            }.Draw(RShadow.Position);
                    }
                }
                if (Value.Use("draw.r"))
                {
                    if (Value.Use("draw.ready"))
                    {
                        if (R.IsReady())
                        {
                            new Circle
                            {
                                BorderWidth = MainMenu.Draw.GetWidth("width.r"),
                                Color = MainMenu.Draw.GetColor("color.r"),
                                Radius = R.Range
                            }.Draw(Player.Instance.Position);
                        }
                    }
                    else
                    {
                        new Circle
                        {
                            BorderWidth = MainMenu.Draw.GetWidth("width.r"),
                            Color = MainMenu.Draw.GetColor("color.r"),
                            Radius = R.Range
                        }.Draw(Player.Instance.Position);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code DRAWING_.ONDRAW)</font>");
            }
        }

        #endregion

        #region Combo

        public override void Combo()
        {
            try
            {
                var Target = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (Target == null || !Target.IsValid)
                    return;

                var r = W.GetPrediction(Target);

                switch (Value.Get("combo.mode"))
                {
                    //Line
                    case 1:
                        try
                        {
                            if (R.IsReady() && R.IsInRange(Target) && !RIsSwitch)
                            {
                                R.Cast(Target);
                            }
                            if (W.IsReady() && W.IsInRange(Target) && !WIsSwitch)
                            {
                                var pos = Player.Instance.Position.Extend(Target.ServerPosition, W.Range);
                                if (!WIsSwitch)
                                    W.Cast(pos.To3D());
                            }
                            if (Q.IsReady() && Q.IsInRange(Target))
                            {
                                Core.DelayAction(() => Q.Cast(Target), 6);
                            }
                            if (Value.Use("combo.switch"))
                            {
                                if (RIsSwitch && RShadow != null && RShadow.IsValid)
                                {
                                    Player.Instance.Spellbook.CastSpell(SpellSlot.R);
                                }
                            }
                            if (E.IsReady())
                            {
                                Core.DelayAction(() => E.Cast(), 6);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Chat.Print(
                                "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code COMBO.1)</font>");
                        }
                        break;
                    case 2:
                        try
                        {
                            //Default
                            if (R.IsReady() && R.IsInRange(Target) && !RIsSwitch)
                            {
                                R.Cast(Target);
                            }
                            if (W.IsReady() && !WIsSwitch)
                            {
                                if (RShadow == null || !RShadow.IsValid)
                                    return;
                                if (WShadow == null || !WShadow.IsValid)
                                    return;

                                var pos =
                                    RShadow.ServerPosition.Extend(WShadow, RShadow.Distance(WShadow)/2)
                                        .Perpendicular()
                                        .Extend(Player.Instance, E.Range/1.75f);
                                W.Cast(pos.To3D());
                            }
                            if (Q.IsReady())
                            {
                                if ((WShadow.IsInRange(Target, Q.Range) && WShadow != null && WShadow.IsValid) ||
                                    (RShadow.IsInRange(Target, Q.Range) && RShadow != null && RShadow.IsValid) ||
                                    Q.IsInRange(Target))
                                {
                                    Core.DelayAction(() => Q.Cast(Target), 6);
                                }
                            }
                            if (E.IsReady())
                            {
                                if (WShadow.IsInRange(Target, E.Range) || RShadow.IsInRange(Target, E.Range) ||
                                    E.IsInRange(Target))
                                {
                                    Core.DelayAction(() => E.Cast(), 6);
                                }
                            }
                            if (Value.Use("combo.switch"))
                            {
                                if (RIsSwitch)
                                {
                                    Player.Instance.Spellbook.CastSpell(SpellSlot.R);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Chat.Print(
                                "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code COMBO.2)</font>");
                        }
                        break;

                    //Distant
                    case 3:
                        try
                        {
                            if (R.IsReady() && R.IsInRange(Target))
                            {
                                if (!RIsSwitch)
                                {
                                    R.Cast(Target);
                                }
                            }
                            if (W.IsReady())
                            {
                                if (!WIsSwitch)
                                {
                                    if (RShadow != null && RShadow.IsValid)
                                    {
                                        var pos = RShadow.ServerPosition.Extend(Target, RShadow.Distance(Target)*2);
                                        W.Cast(pos.To3D());
                                    }
                                }
                            }
                            if (WIsSwitch && W.IsReady())
                            {
                                Player.Instance.Spellbook.CastSpell(SpellSlot.W);
                            }
                            if ((WShadow != null && WShadow.IsValid && WShadow.IsInRange(Target, Q.Range)) ||
                                (WShadow != null && WShadow.IsValid && RShadow.IsInRange(Target, Q.Range)) ||
                                Q.IsInRange(Target))
                            {
                                if (Q.IsReady())
                                {
                                    Q.Cast(Target);
                                }
                            }
                            if ((WShadow != null && WShadow.IsValid && WShadow.IsInRange(Target, E.Range)) ||
                                (WShadow != null && WShadow.IsValid && RShadow.IsInRange(Target, E.Range)) ||
                                E.IsInRange(Target))
                            {
                                if (E.IsReady())
                                {
                                    E.Cast();
                                }
                            }
                            if (RIsSwitch)
                            {
                                Player.Instance.Spellbook.CastSpell(SpellSlot.R);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Chat.Print(
                                "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code COMBO.3)</font>");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code COMBO)</font>");
            }
        }

        #endregion

        #region Harass

        public override void Harass()
        {
            try
            {
                var Target = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (Target == null || !Target.IsValid)
                    return;

                if (W.IsReady() && Player.Instance.IsInRange(Target.ServerPosition, W.Range - E.Range - 50) &&
                    !WIsSwitch && Value.Use("harass.w"))
                {
                    W.Cast(W.GetPrediction(Target).UnitPosition);
                }
                if (Q.IsReady() && Value.Use("harass.q") && WShadow != null && WShadow.IsValid &&
                    WShadow.IsInRange(Target, Q.Range) || Q.IsInRange(Target))
                {
                    Q.Cast(Q.GetPrediction(Target).UnitPosition);
                }
                if (E.IsReady() && Value.Use("harass.e") && WShadow.IsValid && WShadow != null &&
                    WShadow.IsInRange(Target, E.Range) || E.IsInRange(Target))
                {
                    E.Cast();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code HARASS)</font>");
            }
        }

        #endregion

        #region LaneClear

        public override void Laneclear()
        {
            try
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions)
                {
                    if (minion == null || minion.IsValid)
                        return;

                    if (W.IsReady() && !WIsSwitch && Value.Use("lane.w") &&
                        Value.Get("lane.w.mana") <= Player.Instance.ManaPercent)
                    {
                        W.Cast(minion.ServerPosition.Extend(Player.Instance, Player.Instance.Distance(minion)*2).To3D());
                    }
                    if (E.IsReady() && Value.Use("lane.e") && Value.Get("lane.e.mana") <= Player.Instance.ManaPercent &&
                        WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, E.Range) ||
                        E.IsInRange(minion))
                    {
                        E.Cast();
                    }
                    if (Q.IsReady() && Value.Use("lane.q") && Value.Get("lane.q.mana") <= Player.Instance.ManaPercent &&
                        WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, Q.Range) ||
                        Q.IsInRange(minion))
                    {
                        Q.Cast(minion);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LANECLEAR)</font>");
            }
        }

        #endregion

        #region Flee

        public override void Flee()
        {
            try
            {
                if (Value.Use("flee.w") && W.IsReady())
                {
                    Core.DelayAction(() => W.Cast(Player.Instance.Position.Extend(Game.CursorPos, W.Range).To3D()),
                        Value.Get("flee.w.delay"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code FLEE)</font>");
            }
        }

        #endregion

        #region LastHit

        public override void LastHit()
        {
            try
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions)
                {
                    if (Value.Use("lasthit.notkillablebyaa"))
                    {
                        if (Orbwalker.UnKillableMinionsList.Contains(minion))
                        {
                            if ((Prediction.Health.GetPrediction(minion, W.CastDelay + Q.CastDelay) <=
                                 Player.Instance.GetSpellDamage(minion, SpellSlot.Q)*1.5 && Q.IsReady() &&
                                 Value.Use("lasthit.q")) ||
                                Prediction.Health.GetPrediction(minion, W.CastDelay + E.CastDelay) <=
                                Player.Instance.GetSpellDamage(minion, SpellSlot.E)*1.5 && E.IsReady() &&
                                Value.Use("lasthit.e") &&
                                Value.Use("lasthit.w") && !WIsSwitch)
                            {
                                if (WIsSwitch)
                                    return;

                                W.Cast(minion);
                            }
                            if (Prediction.Health.GetPrediction(minion, Q.CastDelay) <=
                                Player.Instance.GetSpellDamage(minion, SpellSlot.Q)*
                                ActiveShadows.Where(s => s != null && s.IsValid && s.IsInRange(minion, Q.Range)).Count() +
                                1 &&
                                Prediction.Health.GetPrediction(minion, Q.CastDelay) >= 1 &&
                                Value.Use("lasthit.q"))
                            {
                                if (Q.IsInRange(minion) ||
                                    (WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, Q.Range)) ||
                                    (RShadow != null && RShadow.IsValid && RShadow.IsInRange(minion, Q.Range)))
                                {
                                    Q.Cast(minion);
                                }
                            }
                            if (Prediction.Health.GetPrediction(minion, E.CastDelay) <=
                                Player.Instance.GetSpellDamage(minion, SpellSlot.E)*
                                ActiveShadows.Where(s => s != null && s.IsValid && s.IsInRange(minion, Q.Range)).Count() +
                                1 &&
                                Prediction.Health.GetPrediction(minion, E.CastDelay) >= 1 &&
                                Value.Use("lasthit.e") && !WIsSwitch)
                            {
                                if (E.IsInRange(minion) ||
                                    (WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, E.Range)) ||
                                    (RShadow != null && RShadow.IsValid && RShadow.IsInRange(minion, E.Range)))
                                {
                                    E.Cast();
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((Prediction.Health.GetPrediction(minion, W.CastDelay + Q.CastDelay) <=
                             Player.Instance.GetSpellDamage(minion, SpellSlot.Q)*1.5 && Q.IsReady() &&
                             Value.Use("lasthit.q")) ||
                            Prediction.Health.GetPrediction(minion, W.CastDelay + E.CastDelay) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.E)*1.5 && E.IsReady() &&
                            Value.Use("lasthit.e") &&
                            Value.Use("lasthit.w") && !WIsSwitch)
                        {
                            if (WIsSwitch)
                                return;

                            W.Cast(minion);
                        }
                        if (Prediction.Health.GetPrediction(minion, Q.CastDelay) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.Q)*
                            ActiveShadows.Where(s => s != null && s.IsValid && s.IsInRange(minion, Q.Range)).Count() + 1 &&
                            Prediction.Health.GetPrediction(minion, Q.CastDelay) >= 1 &&
                            Value.Use("lasthit.q"))
                        {
                            if (Q.IsInRange(minion) ||
                                (WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, Q.Range)) ||
                                (RShadow != null && RShadow.IsValid && RShadow.IsInRange(minion, Q.Range)))
                            {
                                Q.Cast(minion);
                            }
                        }
                        if (Prediction.Health.GetPrediction(minion, E.CastDelay) <=
                            Player.Instance.GetSpellDamage(minion, SpellSlot.E)*
                            ActiveShadows.Where(s => s != null && s.IsValid && s.IsInRange(minion, Q.Range)).Count() + 1 &&
                            Prediction.Health.GetPrediction(minion, E.CastDelay) >= 1 &&
                            Value.Use("lasthit.e"))
                        {
                            if (E.IsInRange(minion) ||
                                (WShadow != null && WShadow.IsValid && WShadow.IsInRange(minion, E.Range)) ||
                                (RShadow != null && RShadow.IsValid && RShadow.IsInRange(minion, E.Range)))
                            {
                                E.Cast();
                            }
                        }

                        if (Value.Use("lasthit.e") &&
                            minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.E) &&
                            IsValidNotNull(WShadow) && WShadow.IsInRange(minion, E.Range) ||
                            (IsValidNotNull(RShadow) && RShadow.IsInRange(minion, E.Range)))
                        {
                            if (E.IsReady())
                            {
                                Chat.Print("EEEEE");
                                Player.Instance.Spellbook.CastSpell(SpellSlot.E);
                            }
                        }
                        if (Value.Use("lasthit.q") &&
                            minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q) &&
                            IsValidNotNull(WShadow) && WShadow.IsInRange(minion, Q.Range) ||
                            (IsValidNotNull(RShadow) && RShadow.IsInRange(minion, Q.Range)))
                        {
                            if (Q.IsReady())
                            {
                                Player.Instance.Spellbook.CastSpell(SpellSlot.Q, minion);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code LASTHIT)</font>");
            }
        }

        #endregion

        #endregion

        #region Utils

        #region GameOnUpdate

        private static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                IsDoingCombo = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
                WIsSwitch = W.Name.ToLower() == "zedw2";
                RIsSwitch = R.Name.ToLower() == "zedr2";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code ONUPDATE.SHADOWMANAGER)</font>");
            }
            AutoE();
        }

        #endregion

        #region AutoE

        private static void AutoE()
        {
            try
            {
                if (!Value.Use("harass.autoe"))
                    return;


                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValid && !e.IsDead && e.IsTargetable))
                {
                    if (E.IsInRange(enemy))
                    {
                        E.Cast();
                    }
                    if (WShadow != null && WShadow.IsValid)
                    {
                        if (enemy.IsInRange(WShadow, E.Range) && Value.Use("harass.autoe.w"))
                        {
                            E.Cast();
                        }
                    }
                    if (RShadow != null && RShadow.IsValid)
                    {
                        if (enemy.IsInRange(RShadow, E.Range) && Value.Use("harass.autoe.r"))
                        {
                            E.Cast();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code AUTOE)</font>");
            }
        }

        #endregion

        #region KillSteal

        private static void KillSteal()
        {
            try
            {
                var e = EntityManager.Heroes.Enemies.Where(ee => !ee.IsDead && ee.IsValid);

                foreach (var enemy in e)
                {
                    var damage = Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Physical,
                        GetRawComboDamage(enemy));
                    if (enemy.Health <= damage)
                    {
                        if (Q.IsReady() && (Q.IsInRange(enemy) || enemy.IsInRange(WShadow, Q.Range)) &&
                            Value.Use("killsteal.q"))
                        {
                            if (Q.GetPrediction(enemy).HitChancePercent >= Value.Get("killsteal.q.hitchance"))
                            {
                                Q.Cast(Q.GetPrediction(enemy).UnitPosition);
                            }
                        }
                        if (W.IsReady() && W.IsInRange(enemy) && Value.Use("killsteal.w") && !WIsSwitch)
                        {
                            W.Cast(enemy);
                        }
                        if (E.IsReady() && (E.IsInRange(enemy) || enemy.IsInRange(WShadow, E.Range)) &&
                            Value.Use("killsteal.e"))
                        {
                            E.Cast();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code KILLSTEAL)</font>");
            }
        }

        #endregion

        #region Damage

        private static readonly float[] QBaseDamage = new float[6] {0, 75, 115, 155, 195, 235};
        private static readonly float[] QFirstHitBonusDamage = new float[6] {0, 45, 69, 93, 117, 141};
        private static readonly float[] EBaseDamage = new float[6] {0, 60, 90, 120, 150, 180};
        private static readonly float[] RBasePercentDamage = new float[4] {0, 30, 40, 50};

        private static float GetRawSpellDamage(SpellSlot slot, Obj_AI_Base target = null)
        {
            try
            {
                var damage = 0f;

                if (slot == SpellSlot.Q)
                {
                    damage += QBaseDamage[Q.Level] + 115f/199f*Player.Instance.FlatPhysicalDamageMod;
                    if (target != null)
                    {
                        if (Q.GetPrediction(target).Collision)
                        {
                            damage += QBaseDamage[Q.Level] + 69f/199f*Player.Instance.FlatPhysicalDamageMod;
                        }
                    }
                }
                if (slot == SpellSlot.E)
                {
                    damage += EBaseDamage[Q.Level] + 92f/199f*Player.Instance.FlatPhysicalDamageMod;
                }
                if (slot == SpellSlot.R)
                {
                    damage += Player.Instance.FlatPhysicalDamageMod;
                }
                if (slot == GetIgniteSpellSlot())
                {
                    damage += 50 + 20*Player.Instance.Level;
                }
                return damage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETRAWSPELLDAMAGE)</font>");
                return 0f;
            }
        }

        private static float GetRawComboDamage(Obj_AI_Base enemy)
        {
            try
            {
                var damage = 0f;

                var spells = new List<SpellSlot>();
                spells.Add(SpellSlot.Q);
                spells.Add(SpellSlot.W);
                spells.Add(SpellSlot.E);
                spells.Add(SpellSlot.R);
                spells.Add(GetIgniteSpellSlot());

                foreach (
                    var spell in
                        spells.Where(
                            s => Player.Instance.Spellbook.CanUseSpell(s) == SpellState.Ready && s != SpellSlot.R))
                {
                    if (Player.Instance.Spellbook.CanUseSpell(spell) == SpellState.Ready)
                        damage += GetRawSpellDamage(spell, enemy);
                }

                if (Player.Instance.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready)
                {
                    damage *= 2f;
                    if (R.IsReady())
                    {
                        damage -= Player.Instance.FlatPhysicalDamageMod;
                    }
                }
                if (Player.Instance.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready)
                    damage += RBasePercentDamage[R.Level]/100f*damage;
                return damage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETRAWCOMBODAMAGE)</font>");
                return 0f;
            }
        }

        private static SpellSlot GetIgniteSpellSlot()
        {
            try
            {
                if (Player.GetSpell(SpellSlot.Summoner1).Name.ToLower() == "summonerignite")
                    return SpellSlot.Summoner1;
                if (Player.GetSpell(SpellSlot.Summoner2).Name.ToLower() == "summonerignite")
                    return SpellSlot.Summoner2;
                return SpellSlot.Unknown;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code GETIGNITESPELLSLOT)</font>");
                return SpellSlot.Unknown;
            }
        }

        #endregion

        #endregion
    }
}