using System;
using System.Collections.Generic;
using EloBuddy;

namespace OKTRAIO.Utility.Tracker
{
    public class SpellAvaliblity
    {
        public static readonly SpellSlot[] TrackedSpellSlots =
        {
            SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R,
            SpellSlot.Summoner1, SpellSlot.Summoner2
        };
        #region Spell: Q
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool Q
        {
            get { return !(QTimeLeft > 0); }
        }
        /// <summary>
        /// Returns True if the spell has been learned otherwise false.
        /// </summary>
        public bool QLearned
        {
            get { return QSpell.IsLearned; }
        }
        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst QSpell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.Q); }
        }
        /// <summary>
        /// Returns in Game.Time when the spell will be reset (available)
        /// </summary>
        public float QResetTime
        {
            get { return QSpell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float QTimeLeft
        {
            get { return QResetTime - Game.Time; }
        }

        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float QCooldownPercent
        {
            get
            {
                return (QTimeLeft > 0 && Math.Abs(QSpell.Cooldown) > float.Epsilon)
                    ? 1f - (QTimeLeft / QSpell.Cooldown)
                    : 1f;
            }
        }
        #endregion

        #region Spell: W
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool W
        {
            get { return !(WTimeLeft > 0); }
        }
        /// <summary>
        /// Returns True if the spell has been learned otherwise false.
        /// </summary>
        public bool WLearned
        {
            get { return WSpell.IsLearned; }
        }
        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst WSpell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.W); }
        }
        /// <summary>
        /// Returns in Game.Time when the spell will be reset (available)
        /// </summary>
        public float WResetTime
        {
            get { return WSpell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float WTimeLeft
        {
            get { return WResetTime - Game.Time; }
        }

        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float WCooldownPercent
        {
            get
            {
                return (WTimeLeft > 0 && Math.Abs(WSpell.Cooldown) > float.Epsilon)
                    ? 1f - (WTimeLeft / WSpell.Cooldown)
                    : 1f;
            }
        }
        #endregion

        #region Spell: E
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool E
        {
            get { return !(ETimeLeft > 0); }
        }
        /// <summary>
        /// Returns True if the spell has been learned otherwise false.
        /// </summary>
        public bool ELearned
        {
            get { return ESpell.IsLearned; }
        }
        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst ESpell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.E); }
        }

        public float EResetTime
        {
            get { return ESpell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float ETimeLeft
        {
            get { return EResetTime - Game.Time; }
        }

        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float ECooldownPercent
        {
            get
            {
                return (ETimeLeft > 0 && Math.Abs(ESpell.Cooldown) > float.Epsilon)
                    ? 1f - (ETimeLeft / ESpell.Cooldown)
                    : 1f;
            }
        }
        #endregion

        #region Spell: R
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool R
        {
            get { return !(RTimeLeft > 0); }
        }
        /// <summary>
        /// Returns True if the spell has been learned otherwise false.
        /// </summary>
        public bool RLearned
        {
            get { return RSpell.IsLearned; }
        }
        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst RSpell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.R); }
        }
        /// <summary>
        /// Returns in Game.Time when the spell will be reset (available)
        /// </summary>
        public float RResetTime
        {
            get { return RSpell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float RTimeLeft
        {
            get { return RResetTime - Game.Time; }
        }

        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float RCooldownPercent
        {
            get
            {
                return (RTimeLeft > 0 && Math.Abs(RSpell.Cooldown) > float.Epsilon)
                    ? 1f - (RTimeLeft / RSpell.Cooldown)
                    : 1f;
            }
        }
        #endregion

        #region Spell: Summoner1
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool Summoner1
        {
            get { return !(Summoner1TimeLeft > 0); }
        }

        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst Summoner1Spell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.Summoner1); }
        }
        /// <summary>
        /// Returns in Game.Time when the spell will be reset (available)
        /// </summary>
        public float Summoner1ResetTime
        {
            get { return Summoner1Spell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float Summoner1TimeLeft
        {
            get { return Summoner1ResetTime - Game.Time; }
        }

        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float Summoner1CooldownPercent
        {
            get
            {
                return (Summoner1TimeLeft > 0 && Math.Abs(Summoner1Spell.Cooldown) > float.Epsilon)
                    ? 1f - (Summoner1TimeLeft / Summoner1Spell.Cooldown)
                    : 1f;
            }
        }
        #endregion

        #region Spell: Summoner2
        /// <summary>
        /// Returns True if the spell is available otherwise false.
        /// </summary>
        public bool Summoner2
        {
            get { return !(Summoner2TimeLeft > 0); }
        }

        /// <summary>
        /// Returns the Spellbook instance.
        /// </summary>
        public SpellDataInst Summoner2Spell
        {
            get { return Hero.Spellbook.GetSpell(SpellSlot.Summoner2); }
        }
        /// <summary>
        /// Returns in Game.Time when the spell will be reset (available)
        /// </summary>
        public float Summoner2ResetTime
        {
            get { return Summoner2Spell.CooldownExpires; }
        }
        /// <summary>
        /// Returns when the spell will be reset (available) in seconds
        ///  = ResetTime - Game.Time
        /// </summary>
        public float Summoner2TimeLeft
        {
            get { return Summoner2ResetTime - Game.Time; }
        }
        /// <summary>
        /// Returns the cooldowns percent
        /// </summary>
        public float Summoner2CooldownPercent
        {
            get
            {
                return (Summoner2TimeLeft > 0 && Math.Abs(Summoner2Spell.Cooldown) > float.Epsilon)
                    ? 1f - (Summoner2TimeLeft / Summoner2Spell.Cooldown)
                    : 1f;
            }
        }
        #endregion


        public SpellDataInst GetSpell(SpellSlot slot)
        {
            return Hero.Spellbook.GetSpell(slot);
        }

        public bool IsAvailable(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return Q;
                case SpellSlot.W:
                    return W;
                case SpellSlot.E:
                    return E;
                case SpellSlot.R:
                    return R;
                case SpellSlot.Summoner1:
                    return Summoner1;
                case SpellSlot.Summoner2:
                    return Summoner2;
                default:
                    return false;
            }

        }

        public bool IsLearned(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return QLearned;
                case SpellSlot.W:
                    return WLearned;
                case SpellSlot.E:
                    return ELearned;
                case SpellSlot.R:
                    return RLearned;
                case SpellSlot.Summoner1:
                    return Summoner1;
                case SpellSlot.Summoner2:
                    return Summoner2;
                default: return false;
            }
        }
        public float CoolDownPercent(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return QCooldownPercent;
                case SpellSlot.W:
                    return WCooldownPercent;
                case SpellSlot.E:
                    return ECooldownPercent;
                case SpellSlot.R:
                    return RCooldownPercent;
                case SpellSlot.Summoner1:
                    return Summoner1CooldownPercent;
                case SpellSlot.Summoner2:
                    return Summoner2CooldownPercent;
                default: return 1f;
            }
        }

        public readonly AIHeroClient Hero;

        /// <summary>
        /// Constructs a <see cref="SpellAvaliblity"/> object using the specified <see cref="AIHeroClient"/>
        /// </summary>
        /// <param name="hero">The <see cref="AIHeroClient"/>'s cooldowns to track</param>
        public SpellAvaliblity(AIHeroClient hero)
        {
            Hero = hero;
        }

        public override string ToString()
        {
            return string.Format("Q: {0} W: {1} E: {2} R: {3} S1: {4} S2: {5}", Q, W, E, R, Summoner1, Summoner2);
        }
    }
}