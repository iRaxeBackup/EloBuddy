using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace OKTRAIO.Utility
{
    public delegate void OnPossibleToInterruptH(AIHeroClient unit, InterruptableSpell spell);

    public enum InterruptableDangerLevel
    {
        Low,
        Medium,
        High,
    }

    public struct InterruptableSpell
    {

        public string BuffName;

        public string ChampionName;

        public InterruptableDangerLevel DangerLevel;

        public int ExtraDuration;

        public SpellSlot Slot;

        public string SpellName;
    }

    [Obsolete("Use Interrupter2", false)]
    public static class CustomInterrupter
    {

        public static List<InterruptableSpell> Spells = new List<InterruptableSpell>();

        static CustomInterrupter()
        {
            #region Varus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusQ",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.Q,
                    BuffName = "VarusQ"
                });

            #endregion

            #region Urgot

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotSwap2",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "UrgotSwap2"
                });

            #endregion

            #region Caitlyn

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynAceintheHole",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "CaitlynAceintheHole",
                    ExtraDuration = 600
                });

            #endregion

            #region Warwick

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Warwick",
                    SpellName = "InfiniteDuress",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "infiniteduresssound"
                });

            #endregion

            #region Shen

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Shen",
                    SpellName = "ShenStandUnited",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.R,
                    BuffName = "shenstandunitedlock"
                });

            #endregion

            #region Malzahar

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharNetherGrasp",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "alzaharnethergraspsound",
                    ExtraDuration = 2000
                });

            #endregion

            #region Nunu

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Nunu",
                    SpellName = "AbsoluteZero",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "AbsoluteZero",
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Pantheon",
                    SpellName = "PantheonRJump",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "PantheonRJump"
                });

            #endregion

            #region Karthus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusFallenOne",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "karthusfallenonecastsound"
                });

            #endregion

            #region Velkoz

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "VelkozR",
                });

            #endregion

            #region Galio

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Galio",
                    SpellName = "GalioIdolOfDurand",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "GalioIdolOfDurand",
                    ExtraDuration = 200,
                });

            #endregion

            #region MissFortune

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MissFortune",
                    SpellName = "MissFortuneBulletTime",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "missfortunebulletsound",
                });

            #endregion

            #region Fiddlesticks

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "Drain",
                });

            //Max rank Drain had different buff name
            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "fearmonger_marker",
                });

            /*  Crowstorm buffname only appears after finish casting.
        Spells.Add(
            new InterruptableSpell
            {
                ChampionName = "FiddleSticks",
                SpellName = "Crowstorm",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "Crowstorm",
            });*/

            #endregion

            #region Katarina

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Katarina",
                    SpellName = "KatarinaR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "katarinarsound"
                });

            #endregion

            #region MasterYi

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MasterYi",
                    SpellName = "Meditate",
                    BuffName = "Meditate",
                    Slot = SpellSlot.W,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Xerath

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathLocusOfPower2",
                    BuffName = "XerathLocusOfPower2",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Janna

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Janna",
                    SpellName = "ReapTheWhirlwind",
                    BuffName = "ReapTheWhirlwind",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Lucian

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "LucianR"
                });

            #endregion

            #region TwistedFate

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "TwistedFate",
                    SpellName = "Destiny",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.R,
                    BuffName = "Destiny"
                });

            #endregion

            Game.OnUpdate += Game_OnGameUpdate;
        }

        [Obsolete("Use Interrupter2.OnInterruptableTarget", false)]
        public static event OnPossibleToInterruptH OnPossibleToInterrupt;

        private static void FireOnInterruptable(AIHeroClient unit, InterruptableSpell spell)
        {
            if (OnPossibleToInterrupt != null)
            {
                OnPossibleToInterrupt(unit, spell);
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(e => e.IsValidTarget()))
            {
                foreach (var spell in
                    Spells.Where(
                        spell =>
                            (enemy.LastCastedspell() != null &&
                             String.Equals(
                                 enemy.LastCastedspell().Name, spell.SpellName,
                                 StringComparison.CurrentCultureIgnoreCase) &&
                             Core.GameTickCount - enemy.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                            (!string.IsNullOrEmpty(spell.BuffName) && enemy.HasBuff(spell.BuffName))))
                {
                    FireOnInterruptable(enemy, spell);
                }
            }
        }

        public static bool IsChannelingImportantSpell(this AIHeroClient unit)
        {
            return
                Spells.Any(
                    spell =>
                        spell.ChampionName == unit.ChampionName &&
                        ((unit.LastCastedspell() != null &&
                            String.Equals(
                                unit.LastCastedspell().Name, spell.SpellName, StringComparison.CurrentCultureIgnoreCase) &&
                            Core.GameTickCount - unit.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                        (spell.BuffName != null && unit.HasBuff(spell.BuffName)) ||
                        (unit.IsMe &&
                            LastCastedSpell.LastCastPacketSent != null &&
                            LastCastedSpell.LastCastPacketSent.Slot == spell.Slot &&
                            Core.GameTickCount - LastCastedSpell.LastCastPacketSent.Tick < 150 + Game.Ping)));
        }
    }
}