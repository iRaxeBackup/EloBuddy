using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using OKTRAIO.Utility;

namespace OKTRAIO.Database.Spell_Library
{

    #region Spelldamage values

    public class SpellDb
    {
        public string CharName;
        public float Damage;
        public string SpellKey;

        public SpellDb()
        {
        }

        public SpellDb(
            string charName,
            string spellKey,
            float damage
            )
        {
            CharName = charName;
            SpellKey = spellKey;
            Damage = damage;
        }

        public string charName { get; set; }
        public string spellKey { get; set; }
        public float damage { get; set; }
    }

    #endregion

    #region SkillShot values

    public class Spell
    {
        public enum CollisionObjectTypes
        {
            Minion,
            Champions,
            YasuoWall
        }

        public enum InterruptableDangerLevel
        {
            Low,
            Medium,
            High
        }

        public bool AddHitbox;
        public string BuffName;
        public bool CanBeRemoved = false;
        public bool Centered;
        public string ChampionName;

        public CollisionObjectTypes[] CollisionObjects = {};
        public InterruptableDangerLevel DangerLevel;
        public int DangerValue;
        public int Delay;
        public bool DisabledByDefault = false;
        public bool DisableFowDetection = false;
        public bool DontAddExtraDuration;
        public bool DontCheckForDuplicates = false;
        public bool DontCross = false;
        public bool DontRemove = false;
        public int ExtraDuration;
        public string[] ExtraMissileNames = {};
        public int ExtraRange = -1;
        public string[] ExtraSpellNames = {};
        public bool FixedRange;
        public bool FollowCaster = false;
        public bool ForceRemove = false;
        public string FromObject = "";
        public string[] FromObjects = {};
        public int Id = -1;
        public bool Invert;
        public bool IsDangerous = false;
        public bool IsInterruptableSpell;
        public int MissileAccel = 0;
        public bool MissileDelayed;
        public bool MissileFollowsUnit;
        public int MissileMaxSpeed;
        public int MissileMinSpeed;
        public int MissileSpeed;
        public string MissileSpellName = "";
        public float MultipleAngle;
        public int MultipleNumber = -1;
        public int RingRadius;
        public SpellSlot Slot;
        public string SpellName;
        public bool TakeClosestPath = false;
        public string ToggleParticleName = "";
        public SkillShotType Type;

        public Spell()
        {
        }

        public Spell(string championName,
            string spellName,
            SpellSlot slot,
            SkillShotType type,
            int delay,
            int range,
            int radius,
            int missileSpeed,
            bool addHitbox,
            bool fixedRange,
            int defaultDangerValue)
        {
            ChampionName = championName;
            SpellName = spellName;
            Slot = slot;
            Type = type;
            Delay = delay;
            Range = range;
            Radius = radius;
            MissileSpeed = missileSpeed;
            AddHitbox = addHitbox;
            FixedRange = fixedRange;
            DangerValue = defaultDangerValue;
        }

        public string MenuItemName
        {
            get { return ChampionName + " - " + SpellName; }
        }

        public int Radius { get; set; }

        public int RawRadius
        {
            get { return Radius; }
        }

        public int RawRange { get; private set; }

        public int Range
        {
            get { return RawRange; }
            set { RawRange = value; }
        }

        public bool Collisionable
        {
            get
            {
                for (var i = 0; i < CollisionObjects.Length; i++)
                {
                    if (CollisionObjects[i] == CollisionObjectTypes.Champions ||
                        CollisionObjects[i] == CollisionObjectTypes.Minion)
                        return true;
                }
                return false;
            }
        }
    }

    internal class InterrupterExtensions
    {
        private readonly List<Spell> Spells = new List<Spell>();

        public bool IsChannelingImportantSpell(AIHeroClient unit)
        {
            return
                Spells.Any(
                    spell =>
                        spell.ChampionName == unit.ChampionName &&
                        spell.IsInterruptableSpell &&
                        ((unit.LastCastedspell() != null &&
                          string.Equals(
                              unit.LastCastedspell().Name, spell.SpellName, StringComparison.CurrentCultureIgnoreCase) &&
                          Core.GameTickCount - unit.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                         (spell.BuffName != null && unit.HasBuff(spell.BuffName)) ||
                         (unit.IsMe &&
                          LastCastedSpell.LastCastPacketSent != null &&
                          LastCastedSpell.LastCastPacketSent.Slot == spell.Slot &&
                          Core.GameTickCount - LastCastedSpell.LastCastPacketSent.Tick < 150 + Game.Ping)));
        }
    }

    #endregion
}