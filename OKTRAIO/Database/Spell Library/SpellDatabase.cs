using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace OKTRAIO.Database.Spell_Library
{

    #region Skillshot spells

    public static class SpellDatabase
    {
        public static List<Spell> Spells = new List<Spell>();

        static SpellDatabase()
        {
            #region Aatrox

            Spells.Add(
                new Spell
                {
                    ChampionName = "Aatrox",
                    SpellName = "AatroxQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 600,
                    Range = 650,
                    Radius = 250,
                    MissileSpeed = 2000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = ""
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Aatrox",
                    SpellName = "AatroxE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1075,
                    Radius = 35,
                    MissileSpeed = 1250,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "AatroxEConeMissile"
                });

            #endregion Aatrox

            #region Ahri

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriOrbofDeception",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 100,
                    MissileSpeed = 2500,
                    MissileAccel = -3200,
                    MissileMaxSpeed = 2500,
                    MissileMinSpeed = 400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "AhriOrbMissile",
                    CanBeRemoved = true,
                    ForceRemove = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriOrbReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 100,
                    MissileSpeed = 60,
                    MissileAccel = 1900,
                    MissileMinSpeed = 60,
                    MissileMaxSpeed = 2600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileFollowsUnit = true,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "AhriOrbReturn",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ahri",
                    SpellName = "AhriSeduce",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 60,
                    MissileSpeed = 1550,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "AhriSeduceMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Ahri

            #region Amumu

            Spells.Add(
                new Spell
                {
                    ChampionName = "Amumu",
                    SpellName = "BandageToss",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 90,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SadMummyBandageToss",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Amumu",
                    SpellName = "CurseoftheSadMummy",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 0,
                    Radius = 550,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = ""
                });

            #endregion Amumu

            #region Anivia

            Spells.Add(
                new Spell
                {
                    ChampionName = "Anivia",
                    SpellName = "FlashFrost",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 110,
                    MissileSpeed = 850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "FlashFrostSpell",
                    CanBeRemoved = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Anivia

            #region Annie

            Spells.Add(
                new Spell
                {
                    ChampionName = "Annie",
                    SpellName = "Incinerate",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Cone,
                    Delay = 250,
                    Range = 825,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = ""
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Annie",
                    SpellName = "InfernalGuardian",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 600,
                    Radius = 251,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = ""
                });

            #endregion Annie

            #region Ashe

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ashe",
                    SpellName = "Volley",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1250,
                    Radius = 60,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VolleyAttack",
                    MultipleNumber = 9,
                    MultipleAngle = 4.62f*(float) Math.PI/180,
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall,
                            Spell.CollisionObjectTypes.Minion
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ashe",
                    SpellName = "EnchantedCrystalArrow",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 20000,
                    Radius = 130,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "EnchantedCrystalArrow",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Ashe

            #region Bard

            Spells.Add(
                new Spell
                {
                    ChampionName = "Bard",
                    SpellName = "BardQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BardQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Bard",
                    SpellName = "BardR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 3400,
                    Radius = 350,
                    MissileSpeed = 2100,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "BardR"
                });

            #endregion

            #region Blatzcrink

            Spells.Add(
                new Spell
                {
                    ChampionName = "Blitzcrank",
                    SpellName = "RocketGrab",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 70,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "RocketGrabMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Blitzcrank",
                    SpellName = "StaticField",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 0,
                    Radius = 600,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = ""
                });

            #endregion Blatzcrink

            #region Brand

            Spells.Add(
                new Spell
                {
                    ChampionName = "Brand",
                    SpellName = "BrandBlaze",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 60,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BrandBlazeMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Brand",
                    SpellName = "BrandFissure",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 850,
                    Range = 900,
                    Radius = 240,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = ""
                });

            #endregion Brand

            #region Braum

            Spells.Add(
                new Spell
                {
                    ChampionName = "Braum",
                    SpellName = "BraumQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BraumQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Braum",
                    SpellName = "BraumRWrapper",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1200,
                    Radius = 115,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "braumrmissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Braum

            #region Caitlyn

            Spells.Add(
                new Spell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynPiltoverPeacemaker",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 625,
                    Range = 1300,
                    Radius = 90,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "CaitlynPiltoverPeacemaker",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynEntrapment",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 125,
                    Range = 1000,
                    Radius = 80,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 1,
                    IsDangerous = false,
                    MissileSpellName = "CaitlynEntrapmentMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynAceintheHole",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "CaitlynAceintheHole",
                    ExtraDuration = 600
                });

            #endregion Caitlyn

            #region Cassiopeia

            Spells.Add(
                new Spell
                {
                    ChampionName = "Cassiopeia",
                    SpellName = "CassiopeiaNoxiousBlast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 750,
                    Range = 850,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "CassiopeiaNoxiousBlast"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Cassiopeia",
                    SpellName = "CassiopeiaMiasma",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 850,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "CassiopeiaMiasma"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Cassiopeia",
                    SpellName = "CassiopeiaPetrifyingGaze",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Cone,
                    Delay = 600,
                    Range = 825,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "CassiopeiaPetrifyingGaze"
                });

            #endregion Cassiopeia

            #region Chogath

            Spells.Add(
                new Spell
                {
                    ChampionName = "Chogath",
                    SpellName = "Rupture",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 1200,
                    Range = 950,
                    Radius = 250,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "Rupture"
                });

            #endregion Chogath

            #region Corki

            Spells.Add(
                new Spell
                {
                    ChampionName = "Corki",
                    SpellName = "PhosphorusBomb",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 300,
                    Range = 825,
                    Radius = 250,
                    MissileSpeed = 1000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "PhosphorusBombMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Corki",
                    SpellName = "MissileBarrage",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 200,
                    Range = 1300,
                    Radius = 40,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "MissileBarrageMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Corki",
                    SpellName = "MissileBarrage2",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 200,
                    Range = 1500,
                    Radius = 40,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "MissileBarrageMissile2",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Corki

            #region Darius

            Spells.Add(
                new Spell
                {
                    ChampionName = "Darius",
                    SpellName = "DariusCleave",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 750,
                    Range = 0,
                    Radius = 425,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "DariusCleave",
                    FollowCaster = true,
                    DisabledByDefault = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Darius",
                    SpellName = "DariusAxeGrabCone",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Cone,
                    Delay = 250,
                    Range = 550,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DariusAxeGrabCone"
                });

            #endregion Darius

            #region DrMundo

            Spells.Add(
                new Spell
                {
                    ChampionName = "DrMundo",
                    SpellName = "InfectedCleaverMissileCast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "InfectedCleaverMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion DrMundo

            #region Draven

            Spells.Add(
                new Spell
                {
                    ChampionName = "Draven",
                    SpellName = "DravenDoubleShot",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 130,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DravenDoubleShotMissile",
                    CanBeRemoved = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Draven",
                    SpellName = "DravenRCast",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 400,
                    Range = 20000,
                    Radius = 160,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "DravenR",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Draven

            #region Ekko

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ekko",
                    SpellName = "EkkoQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1650,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "ekkoqmis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ekko",
                    SpellName = "EkkoW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 3750,
                    Range = 1600,
                    Radius = 375,
                    MissileSpeed = 1650,
                    FixedRange = false,
                    DisabledByDefault = true,
                    AddHitbox = false,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "EkkoW",
                    CanBeRemoved = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ekko",
                    SpellName = "EkkoR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1600,
                    Radius = 375,
                    MissileSpeed = 1650,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "EkkoR",
                    CanBeRemoved = true,
                    FromObjects = new[] {"Ekko_Base_R_TrailEnd.troy"}
                });

            #endregion Ekko

            #region Elise

            Spells.Add(
                new Spell
                {
                    ChampionName = "Elise",
                    SpellName = "EliseHumanE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 55,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = true,
                    MissileSpellName = "EliseHumanE",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Elise

            #region Evelynn

            Spells.Add(
                new Spell
                {
                    ChampionName = "Evelynn",
                    SpellName = "EvelynnR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 650,
                    Radius = 350,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "EvelynnR"
                });

            #endregion Evelynn

            #region Ezreal

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealMysticShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "EzrealMysticShotMissile",
                    ExtraMissileNames = new[] {"EzrealMysticShotPulseMissile"},
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        },
                    Id = 229
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealEssenceFlux",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 80,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "EzrealEssenceFluxMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ezreal",
                    SpellName = "EzrealTrueshotBarrage",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 1000,
                    Range = 20000,
                    Radius = 160,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "EzrealTrueshotBarrage",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall},
                    Id = 245
                });

            #endregion Ezreal

            #region Fiddlesticks

            Spells.Add(
                new Spell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "Drain"
                });

            //Max rank Drain had different buff name
            Spells.Add(
                new Spell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "fearmonger_marker"
                });

            #endregion

            #region Fiora

            Spells.Add(
                new Spell
                {
                    ChampionName = "Fiora",
                    SpellName = "FioraW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 700,
                    Range = 800,
                    Radius = 70,
                    MissileSpeed = 3200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "FioraWMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Fiora

            #region Fizz

            Spells.Add(
                new Spell
                {
                    ChampionName = "Fizz",
                    SpellName = "FizzMarinerDoom",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1300,
                    Radius = 120,
                    MissileSpeed = 1350,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "FizzMarinerDoomMissile",
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall},
                    CanBeRemoved = true
                });

            #endregion Fizz

            #region Galio

            Spells.Add(
                new Spell
                {
                    ChampionName = "Galio",
                    SpellName = "GalioResoluteSmite",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 900,
                    Radius = 200,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GalioResoluteSmite"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Galio",
                    SpellName = "GalioRighteousGust",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 120,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GalioRighteousGust",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Galio",
                    SpellName = "GalioIdolOfDurand",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 0,
                    Radius = 550,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High
                });

            #endregion Galio

            #region Gnar

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1125,
                    Radius = 60,
                    MissileSpeed = 2500,
                    MissileAccel = -3000,
                    MissileMaxSpeed = 2500,
                    MissileMinSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "gnarqmissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarQReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 2500,
                    Radius = 75,
                    MissileSpeed = 60,
                    MissileAccel = 800,
                    MissileMaxSpeed = 2600,
                    MissileMinSpeed = 60,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    CanBeRemoved = true,
                    ForceRemove = true,
                    MissileSpellName = "GnarQMissileReturn",
                    DisableFowDetection = false,
                    DisabledByDefault = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1150,
                    Radius = 90,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 600,
                    Range = 600,
                    Radius = 80,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigW"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 473,
                    Radius = 150,
                    MissileSpeed = 903,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarE"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarBigE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 475,
                    Radius = 200,
                    MissileSpeed = 1000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GnarBigE"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gnar",
                    SpellName = "GnarR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 0,
                    Radius = 500,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = ""
                });

            #endregion

            #region Gragas

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1100,
                    Radius = 275,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GragasQMissile",
                    ExtraDuration = 4500,
                    ToggleParticleName = "Gragas_.+_Q_(Enemy|Ally)",
                    DontCross = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 950,
                    Radius = 200,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GragasE",
                    CanBeRemoved = true,
                    ExtraRange = 300,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Gragas",
                    SpellName = "GragasR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1050,
                    Radius = 375,
                    MissileSpeed = 1800,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "GragasRBoom",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Gragas

            #region Graves

            Spells.Add(
                new Spell
                {
                    ChampionName = "Graves",
                    SpellName = "GravesClusterShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 50,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "GravesClusterShotAttack",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall},
                    MultipleNumber = 3,
                    MultipleAngle = 15*(float) Math.PI/180
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Graves",
                    SpellName = "GravesChargeShot",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 100,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "GravesChargeShotShot",
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Graves

            #region Heimerdinger

            Spells.Add(
                new Spell
                {
                    ChampionName = "Heimerdinger",
                    SpellName = "Heimerdingerwm",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1500,
                    Radius = 70,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "HeimerdingerWAttack2",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Heimerdinger",
                    SpellName = "HeimerdingerE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 925,
                    Radius = 100,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "heimerdingerespell",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Heimerdinger

            #region Irelia

            Spells.Add(
                new Spell
                {
                    ChampionName = "Irelia",
                    SpellName = "IreliaTranscendentBlades",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 1200,
                    Radius = 65,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "IreliaTranscendentBlades",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Irelia

            #region Janna

            Spells.Add(
                new Spell
                {
                    ChampionName = "Janna",
                    SpellName = "JannaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1700,
                    Radius = 120,
                    MissileSpeed = 900,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "HowlingGaleSpell",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            //Different cast stages calls for different spell names
            Spells.Add(
                new Spell
                {
                    ChampionName = "Janna",
                    SpellName = "ReapTheWhirlwind",
                    BuffName = "ReapTheWhirlwind",
                    Slot = SpellSlot.R,
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Low
                });

            #endregion Janna

            #region JarvanIV

            Spells.Add(
                new Spell
                {
                    ChampionName = "JarvanIV",
                    SpellName = "JarvanIVDragonStrike",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 600,
                    Range = 770,
                    Radius = 70,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "JarvanIV",
                    SpellName = "JarvanIVEQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 880,
                    Radius = 70,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "JarvanIV",
                    SpellName = "JarvanIVDemacianStandard",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 860,
                    Radius = 175,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JarvanIVDemacianStandard"
                });

            #endregion JarvanIV

            #region Jayce

            Spells.Add(
                new Spell
                {
                    ChampionName = "Jayce",
                    SpellName = "jayceshockblast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JayceShockBlastMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Jayce",
                    SpellName = "JayceQAccel",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 2350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "JayceShockBlastWallMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Jayce

            #region Jinx

            Spells.Add(
                new Spell
                {
                    ChampionName = "Jinx",
                    SpellName = "JinxW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 600,
                    Range = 1500,
                    Radius = 60,
                    MissileSpeed = 3300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "JinxWMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Jinx",
                    SpellName = "JinxR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 600,
                    Range = 20000,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "JinxR",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Jinx

            #region Kalista

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kalista",
                    SpellName = "KalistaMysticShot",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 40,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "kalistamysticshotmis",
                    ExtraMissileNames = new[] {"kalistamysticshotmistrue"},
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Kalista

            #region Karma

            Spells.Add(
                new Spell
                {
                    ChampionName = "Karma",
                    SpellName = "KarmaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KarmaQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Karma",
                    SpellName = "KarmaQMantra",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 80,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KarmaQMissileMantra",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Karma

            #region Karthus

            Spells.Add(
                new Spell
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusLayWasteA2",
                    ExtraSpellNames =
                        new[]
                        {
                            "karthuslaywastea3", "karthuslaywastea1", "karthuslaywastedeada1", "karthuslaywastedeada2",
                            "karthuslaywastedeada3"
                        },
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 625,
                    Range = 875,
                    Radius = 160,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = ""
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusFallenOne",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "karthusfallenonecastsound"
                });

            #endregion Karthus

            #region Kassadin

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kassadin",
                    SpellName = "RiftWalk",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 450,
                    Radius = 270,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RiftWalk"
                });

            #endregion Kassadin

            #region Katarina

            Spells.Add(
                new Spell
                {
                    ChampionName = "Katarina",
                    SpellName = "KatarinaR",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "katarinarsound"
                });

            #endregion

            #region Kennen

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kennen",
                    SpellName = "KennenShurikenHurlMissile1",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 125,
                    Range = 1050,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KennenShurikenHurlMissile1",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Kennen

            #region Khazix

            Spells.Add(
                new Spell
                {
                    ChampionName = "Khazix",
                    SpellName = "KhazixW",
                    ExtraSpellNames = new[] {"khazixwlong"},
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1025,
                    Radius = 73,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KhazixWMissile",
                    CanBeRemoved = true,
                    MultipleNumber = 3,
                    MultipleAngle = 22f*(float) Math.PI/180,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Khazix",
                    SpellName = "KhazixE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 600,
                    Radius = 300,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KhazixE"
                });

            #endregion Khazix

            #region Kogmaw

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 70,
                    MissileSpeed = 1650,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawQMis",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawVoidOoze",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1360,
                    Radius = 120,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawVoidOozeMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Kogmaw",
                    SpellName = "KogMawLivingArtillery",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 1200,
                    Range = 1800,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "KogMawLivingArtillery"
                });

            #endregion Kogmaw

            #region Leblanc

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSlide",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 600,
                    Radius = 220,
                    MissileSpeed = 1450,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LeblancSlide"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSlideM",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 600,
                    Radius = 220,
                    MissileSpeed = 1450,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LeblancSlideM"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSoulShackle",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LeblancSoulShackle",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leblanc",
                    SpellName = "LeblancSoulShackleM",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LeblancSoulShackleM",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Leblanc

            #region LeeSin

            Spells.Add(
                new Spell
                {
                    ChampionName = "LeeSin",
                    SpellName = "BlindMonkQOne",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 65,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "BlindMonkQOne",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion LeeSin

            #region Leona

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leona",
                    SpellName = "LeonaZenithBlade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 905,
                    Radius = 70,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    TakeClosestPath = true,
                    MissileSpellName = "LeonaZenithBladeMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Leona",
                    SpellName = "LeonaSolarFlare",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 1000,
                    Range = 1200,
                    Radius = 300,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "LeonaSolarFlare"
                });

            #endregion Leona

            #region Lissandra

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 700,
                    Radius = 75,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LissandraQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraQShards",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 700,
                    Radius = 90,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "lissandraqshards",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lissandra",
                    SpellName = "LissandraE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1025,
                    Radius = 125,
                    MissileSpeed = 850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LissandraEMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Lulu

            #region Lucian

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1300,
                    Radius = 65,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LucianQ"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 55,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "lucianwmissile"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianRMis",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1400,
                    Radius = 110,
                    MissileSpeed = 2800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "lucianrmissileoffhand",
                    ExtraMissileNames = new[] {"lucianrmissile"},
                    DontCheckForDuplicates = true,
                    DisabledByDefault = true
                });

            //This one is the r channel, where the top one detects only the missiles.
            Spells.Add(
                new Spell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianR",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "LucianR"
                });

            #endregion Lucian

            #region Lulu

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lulu",
                    SpellName = "LuluQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LuluQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lulu",
                    SpellName = "LuluQPix",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1450,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LuluQMissileTwo",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Lulu

            #region Lux

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lux",
                    SpellName = "LuxLightBinding",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1300,
                    Radius = 70,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "LuxLightBindingMis"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lux",
                    SpellName = "LuxLightStrikeKugel",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1100,
                    Radius = 275,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "LuxLightStrikeKugel",
                    ExtraDuration = 5500,
                    ToggleParticleName = "Lux_.+_E_tar_aoe_",
                    DontCross = true,
                    CanBeRemoved = true,
                    DisabledByDefault = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Lux",
                    SpellName = "LuxMaliceCannon",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 1000,
                    Range = 3500,
                    Radius = 190,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "LuxMaliceCannon"
                });

            #endregion Lux

            #region Malphite

            Spells.Add(
                new Spell
                {
                    ChampionName = "Malphite",
                    SpellName = "UFSlash",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 1000,
                    Radius = 270,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "UFSlash"
                });

            #endregion Malphite

            #region Malzahar

            Spells.Add(
                new Spell
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharCalloftheVoid",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 1000,
                    Range = 900,
                    Radius = 85,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    DontCross = true,
                    MissileSpellName = "AlZaharCalloftheVoid"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharNetherGrasp",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "alzaharnethergraspsound",
                    ExtraDuration = 2000
                });

            #endregion Malzahar

            #region MasterYi

            Spells.Add(
                new Spell
                {
                    ChampionName = "MasterYi",
                    SpellName = "Meditate",
                    BuffName = "Meditate",
                    Slot = SpellSlot.W,
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Low
                });

            #endregion

            #region MissFortune

            Spells.Add(
                new Spell
                {
                    ChampionName = "MissFortune",
                    SpellName = "MissFortuneBulletTime",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "missfortunebulletsound"
                });

            #endregion

            #region Morgana

            Spells.Add(
                new Spell
                {
                    ChampionName = "Morgana",
                    SpellName = "DarkBindingMissile",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1300,
                    Radius = 80,
                    MissileSpeed = 1200,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "DarkBindingMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Morgana",
                    SpellName = "TormentedSoil",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 900,
                    Radius = 400,
                    MissileSpeed = 2200,
                    FixedRange = true,
                    AddHitbox = false,
                    DangerValue = 1,
                    IsDangerous = false,
                    MissileSpellName = "TormentedSoil",
                    CanBeRemoved = false
                });

            #endregion Morgana

            #region Nami

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nami",
                    SpellName = "NamiQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 950,
                    Range = 1625,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "namiqmissile"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nami",
                    SpellName = "NamiR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 2750,
                    Radius = 260,
                    MissileSpeed = 850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "NamiRMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Nami

            #region Nautilus

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nautilus",
                    SpellName = "NautilusAnchorDrag",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1250,
                    Radius = 90,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "NautilusAnchorDragMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Nautilus

            #region Nocturne

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nocturne",
                    SpellName = "NocturneDuskbringer",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1125,
                    Radius = 60,
                    MissileSpeed = 1400,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "NocturneDuskbringer",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Nocturne

            #region Nidalee

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nidalee",
                    SpellName = "JavelinToss",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1500,
                    Radius = 40,
                    MissileSpeed = 1300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "JavelinToss",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Nidalee

            #region Nunu

            Spells.Add(
                new Spell
                {
                    ChampionName = "Nunu",
                    SpellName = "AbsoluteZero",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "AbsoluteZero"
                });

            #endregion

            #region Olaf

            Spells.Add(
                new Spell
                {
                    ChampionName = "Olaf",
                    SpellName = "OlafAxeThrowCast",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    ExtraRange = 150,
                    Radius = 105,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "olafaxethrow",
                    CanBeRemoved = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Olaf

            #region Orianna

            Spells.Add(
                new Spell
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannasQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 1500,
                    Radius = 80,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "orianaizuna",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannaQend",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 1500,
                    Radius = 90,
                    MissileSpeed = 1200,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Orianna",
                    SpellName = "OrianaDissonanceCommand",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 0,
                    Radius = 255,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "OrianaDissonanceCommand",
                    FromObject = "yomu_ring_"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Orianna",
                    SpellName = "OriannasE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 1500,
                    Radius = 85,
                    MissileSpeed = 1850,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "orianaredact",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Orianna",
                    SpellName = "OrianaDetonateCommand",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 700,
                    Range = 0,
                    Radius = 410,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "OrianaDetonateCommand",
                    FromObject = "yomu_ring_"
                });

            #endregion Orianna

            #region Pantheon

            Spells.Add(
                new Spell
                {
                    ChampionName = "Pantheon",
                    SpellName = "PantheonRJump",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "PantheonRJump"
                });

            #endregion

            #region Quinn

            Spells.Add(
                new Spell
                {
                    ChampionName = "Quinn",
                    SpellName = "QuinnQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1050,
                    Radius = 80,
                    MissileSpeed = 1550,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "QuinnQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Quinn

            #region Rengar

            Spells.Add(
                new Spell
                {
                    ChampionName = "Rengar",
                    SpellName = "RengarE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 70,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "RengarEFinal",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Rengar

            #region RekSai

            Spells.Add(
                new Spell
                {
                    ChampionName = "RekSai",
                    SpellName = "reksaiqburrowed",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1625,
                    Radius = 60,
                    MissileSpeed = 1950,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = false,
                    MissileSpellName = "RekSaiQBurrowedMis",
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion RekSai

            #region Riven

            Spells.Add(
                new Spell
                {
                    ChampionName = "Riven",
                    SpellName = "rivenizunablade",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 125,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 5,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 15*(float) Math.PI/180,
                    MissileSpellName = "RivenLightsaberMissile",
                    ExtraMissileNames = new[] {"RivenLightsaberMissileSide"}
                });

            #endregion Riven

            #region Rumble

            Spells.Add(
                new Spell
                {
                    ChampionName = "Rumble",
                    SpellName = "RumbleGrenade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RumbleGrenade",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Rumble",
                    SpellName = "RumbleCarpetBombM",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 400,
                    MissileDelayed = true,
                    Range = 1200,
                    Radius = 200,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 4,
                    IsDangerous = false,
                    MissileSpellName = "RumbleCarpetBombMissile",
                    CanBeRemoved = false,
                    CollisionObjects = new Spell.CollisionObjectTypes[] {}
                });

            #endregion Rumble

            #region Ryze

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ryze",
                    SpellName = "RyzeQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 900,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RyzeQ",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ryze",
                    SpellName = "ryzerq",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 900,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ryzerq",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion

            #region Sejuani

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sejuani",
                    SpellName = "SejuaniArcticAssault",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 900,
                    Radius = 70,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "",
                    ExtraRange = 200,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.Minion,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sejuani",
                    SpellName = "SejuaniGlacialPrisonStart",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 110,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "sejuaniglacialprison",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Sejuani

            #region Sion

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sion",
                    SpellName = "SionE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 1800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SionEMissile",
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sion",
                    SpellName = "SionR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 800,
                    Radius = 120,
                    MissileSpeed = 1000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions}
                });

            #endregion Sion

            #region Soraka

            Spells.Add(
                new Spell
                {
                    ChampionName = "Soraka",
                    SpellName = "SorakaQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 950,
                    Radius = 300,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Soraka

            #region Shen

            Spells.Add(
                new Spell
                {
                    ChampionName = "Shen",
                    SpellName = "ShenShadowDash",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 650,
                    Radius = 50,
                    MissileSpeed = 1600,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ShenShadowDash",
                    ExtraRange = 200,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Shen",
                    SpellName = "ShenStandUnited",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Low,
                    Slot = SpellSlot.R,
                    BuffName = "shenstandunitedlock"
                });

            #endregion Shen

            #region Shyvana

            Spells.Add(
                new Spell
                {
                    ChampionName = "Shyvana",
                    SpellName = "ShyvanaFireball",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 60,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ShyvanaFireballMissile",
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Shyvana",
                    SpellName = "ShyvanaTransformCast",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 150,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ShyvanaTransformCast",
                    ExtraRange = 200
                });

            #endregion Shyvana

            #region Sivir

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sivir",
                    SpellName = "SivirQReturn",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 1250,
                    Radius = 100,
                    MissileSpeed = 1350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SivirQMissileReturn",
                    DisableFowDetection = false,
                    MissileFollowsUnit = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sivir",
                    SpellName = "SivirQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1250,
                    Radius = 90,
                    MissileSpeed = 1350,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SivirQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Sivir

            #region Skarner

            Spells.Add(
                new Spell
                {
                    ChampionName = "Skarner",
                    SpellName = "SkarnerFracture",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 70,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SkarnerFractureMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Skarner

            #region Sona

            Spells.Add(
                new Spell
                {
                    ChampionName = "Sona",
                    SpellName = "SonaR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 140,
                    MissileSpeed = 2400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 5,
                    IsDangerous = true,
                    MissileSpellName = "SonaR",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Sona

            #region Swain

            Spells.Add(
                new Spell
                {
                    ChampionName = "Swain",
                    SpellName = "SwainShadowGrasp",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 1100,
                    Range = 900,
                    Radius = 180,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "SwainShadowGrasp"
                });

            #endregion Swain

            #region Syndra

            Spells.Add(
                new Spell
                {
                    ChampionName = "Syndra",
                    SpellName = "SyndraQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 600,
                    Range = 800,
                    Radius = 150,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SyndraQ"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Syndra",
                    SpellName = "syndrawcast",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 950,
                    Radius = 210,
                    MissileSpeed = 1450,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "syndrawcast"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Syndra",
                    SpellName = "syndrae5",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 300,
                    Range = 950,
                    Radius = 90,
                    MissileSpeed = 1601,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "syndrae5",
                    DisableFowDetection = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Syndra",
                    SpellName = "SyndraE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 300,
                    Range = 950,
                    Radius = 90,
                    MissileSpeed = 1601,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    DisableFowDetection = true,
                    MissileSpellName = "SyndraE"
                });

            #endregion Syndra

            #region Talon

            Spells.Add(
                new Spell
                {
                    ChampionName = "Talon",
                    SpellName = "TalonRake",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 2300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 20*(float) Math.PI/180,
                    MissileSpellName = "talonrakemissileone"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Talon",
                    SpellName = "TalonRakeReturn",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 800,
                    Radius = 80,
                    MissileSpeed = 1850,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MultipleNumber = 3,
                    MultipleAngle = 20*(float) Math.PI/180,
                    MissileSpellName = "talonrakemissiletwo"
                });

            #endregion Riven

            #region Tahm Kench

            Spells.Add(
                new Spell
                {
                    ChampionName = "TahmKench",
                    SpellName = "TahmKenchQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 951,
                    Radius = 90,
                    MissileSpeed = 2800,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "tahmkenchqmissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            #endregion Tahm Kench

            #region Thresh

            Spells.Add(
                new Spell
                {
                    ChampionName = "Thresh",
                    SpellName = "ThreshQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1100,
                    Radius = 70,
                    MissileSpeed = 1900,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ThreshQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Thresh",
                    SpellName = "ThreshEFlay",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 125,
                    Range = 1075,
                    Radius = 110,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    Centered = true,
                    MissileSpellName = "ThreshEMissile1"
                });

            #endregion Thresh

            #region Tristana

            Spells.Add(
                new Spell
                {
                    ChampionName = "Tristana",
                    SpellName = "RocketJump",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 900,
                    Radius = 270,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "RocketJump"
                });

            #endregion Tristana

            #region Tryndamere

            Spells.Add(
                new Spell
                {
                    ChampionName = "Tryndamere",
                    SpellName = "slashCast",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 0,
                    Range = 660,
                    Radius = 93,
                    MissileSpeed = 1300,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "slashCast"
                });

            #endregion Tryndamere

            #region TwistedFate

            Spells.Add(
                new Spell
                {
                    ChampionName = "TwistedFate",
                    SpellName = "WildCards",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1450,
                    Radius = 40,
                    MissileSpeed = 1000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "SealFateMissile",
                    MultipleNumber = 3,
                    MultipleAngle = 28*(float) Math.PI/180,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "TwistedFate",
                    SpellName = "Destiny",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.R,
                    BuffName = "Destiny"
                });

            #endregion TwistedFate

            #region Twitch

            Spells.Add(
                new Spell
                {
                    ChampionName = "Twitch",
                    SpellName = "TwitchVenomCask",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 900,
                    Radius = 275,
                    MissileSpeed = 1400,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "TwitchVenomCaskMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Twitch

            #region Urgot

            Spells.Add(
                new Spell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotHeatseekingLineMissile",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 125,
                    Range = 1000,
                    Radius = 60,
                    MissileSpeed = 1600,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "UrgotHeatseekingLineMissile",
                    CanBeRemoved = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotPlasmaGrenade",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1100,
                    Radius = 210,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "UrgotPlasmaGrenadeBoom"
                });
            Spells.Add(
                new Spell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotSwap2",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "UrgotSwap2"
                });

            #endregion Urgot

            #region Varus

            Spells.Add(
                new Spell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusQMissilee",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1800,
                    Radius = 70,
                    MissileSpeed = 1900,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VarusQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall},
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Low
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 1000,
                    Range = 925,
                    Radius = 235,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VarusE"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 120,
                    MissileSpeed = 1950,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "VarusRMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[] {Spell.CollisionObjectTypes.Champions, Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Varus

            #region Veigar

            Spells.Add(
                new Spell
                {
                    ChampionName = "Veigar",
                    SpellName = "VeigarBalefulStrike",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 950,
                    Radius = 70,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VeigarBalefulStrikeMis",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Veigar",
                    SpellName = "VeigarDarkMatter",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 1350,
                    Range = 900,
                    Radius = 225,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = ""
                });

            #endregion Veigar

            #region Velkoz

            Spells.Add(
                new Spell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 50,
                    MissileSpeed = 1300,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozQMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozR",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "VelkozR"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozQSplit",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1100,
                    Radius = 55,
                    MissileSpeed = 2100,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozQMissileSplit",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1200,
                    Radius = 88,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozWMissile"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 800,
                    Radius = 225,
                    MissileSpeed = 1500,
                    FixedRange = false,
                    AddHitbox = false,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "VelkozEMissile"
                });

            #endregion Velkoz

            #region Vi

            Spells.Add(
                new Spell
                {
                    ChampionName = "Vi",
                    SpellName = "Vi-q",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1000,
                    Radius = 90,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ViQMissile"
                });

            #endregion Vi

            #region Viktor

            Spells.Add(
                new Spell
                {
                    ChampionName = "Viktor",
                    SpellName = "Laser",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1500,
                    Radius = 80,
                    MissileSpeed = 780,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ViktorDeathRayMissile",
                    ExtraMissileNames = new[] {"viktoreaugmissile"},
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Viktor

            #region Vladimir

            Spells.Add(
                new Spell
                {
                    ChampionName = "Vladimir",
                    SpellName = "VladimirHemoplague",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 700,
                    Radius = 375,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true
                });

            #endregion Vladimir

            #region Warwick

            Spells.Add(
                new Spell
                {
                    ChampionName = "Warwick",
                    SpellName = "InfiniteDuress",
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "infiniteduresssound"
                });

            #endregion

            #region Xerath

            Spells.Add(
                new Spell
                {
                    ChampionName = "Xerath",
                    SpellName = "xeratharcanopulse2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 600,
                    Range = 1600,
                    Radius = 100,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "xeratharcanopulse2"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathArcaneBarrage2",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 700,
                    Range = 1000,
                    Radius = 200,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "XerathArcaneBarrage2"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathMageSpear",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 200,
                    Range = 1150,
                    Radius = 60,
                    MissileSpeed = 1400,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "XerathMageSpearMissile",
                    CanBeRemoved = true,
                    CollisionObjects =
                        new[]
                        {
                            Spell.CollisionObjectTypes.Minion, Spell.CollisionObjectTypes.Champions,
                            Spell.CollisionObjectTypes.YasuoWall
                        }
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Xerath",
                    SpellName = "xerathrmissilewrapper",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 700,
                    Range = 5600,
                    Radius = 120,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "xerathrmissilewrapper"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathLocusOfPower2",
                    BuffName = "XerathLocusOfPower2",
                    Slot = SpellSlot.R,
                    IsInterruptableSpell = true,
                    DangerLevel = Spell.InterruptableDangerLevel.Low
                });

            #endregion Xerath

            #region Yasuo

            Spells.Add(
                new Spell
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 400,
                    Range = 550,
                    Radius = 20,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq2",
                    Invert = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq3w",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1150,
                    Radius = 90,
                    MissileSpeed = 1500,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq3w",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Yasuo",
                    SpellName = "yasuoq",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 400,
                    Range = 550,
                    Radius = 20,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = true,
                    MissileSpellName = "yasuoq",
                    Invert = true
                });

            #endregion Yasuo

            #region Zac

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zac",
                    SpellName = "ZacQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 550,
                    Radius = 120,
                    MissileSpeed = int.MaxValue,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZacQ"
                });

            #endregion Zac

            #region Zed

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zed",
                    SpellName = "ZedQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 925,
                    Radius = 50,
                    MissileSpeed = 1700,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "zedshurikenmisone",
                    FromObjects = new[] {"Zed_Clone_idle.troy", "Zed_Clone_Idle.troy"},
                    ExtraMissileNames = new[] {"zedshurikenmistwo", "zedshurikenmisthree"},
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Zed

            #region Ziggs

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 850,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell",
                    CanBeRemoved = false,
                    DisableFowDetection = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQBounce1",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 850,
                    Radius = 140,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell2",
                    ExtraMissileNames = new[] {"ZiggsQSpell2"},
                    CanBeRemoved = false,
                    DisableFowDetection = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsQBounce2",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 850,
                    Radius = 160,
                    MissileSpeed = 1700,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsQSpell3",
                    ExtraMissileNames = new[] {"ZiggsQSpell3"},
                    CanBeRemoved = false,
                    DisableFowDetection = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsW",
                    Slot = SpellSlot.W,
                    Type = SkillShotType.Circular,
                    Delay = 250,
                    Range = 1000,
                    Radius = 275,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsW",
                    DisableFowDetection = true,
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsE",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Circular,
                    Delay = 500,
                    Range = 900,
                    Radius = 235,
                    MissileSpeed = 1750,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsE",
                    DisableFowDetection = true
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Ziggs",
                    SpellName = "ZiggsR",
                    Slot = SpellSlot.R,
                    Type = SkillShotType.Circular,
                    Delay = 0,
                    Range = 5300,
                    Radius = 500,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZiggsR",
                    DisableFowDetection = true
                });

            #endregion Ziggs

            #region Zilean

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zilean",
                    SpellName = "ZileanQ",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 300,
                    Range = 900,
                    Radius = 210,
                    MissileSpeed = 2000,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZileanQMissile",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Zilean

            #region Zyra

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zyra",
                    SpellName = "ZyraQFissure",
                    Slot = SpellSlot.Q,
                    Type = SkillShotType.Circular,
                    Delay = 850,
                    Range = 800,
                    Radius = 220,
                    MissileSpeed = int.MaxValue,
                    FixedRange = false,
                    AddHitbox = true,
                    DangerValue = 2,
                    IsDangerous = false,
                    MissileSpellName = "ZyraQFissure"
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zyra",
                    SpellName = "ZyraGraspingRoots",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 250,
                    Range = 1150,
                    Radius = 70,
                    MissileSpeed = 1150,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "ZyraGraspingRoots",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            Spells.Add(
                new Spell
                {
                    ChampionName = "Zyra",
                    SpellName = "zyrapassivedeathmanager",
                    Slot = SpellSlot.E,
                    Type = SkillShotType.Linear,
                    Delay = 500,
                    Range = 1474,
                    Radius = 70,
                    MissileSpeed = 2000,
                    FixedRange = true,
                    AddHitbox = true,
                    DangerValue = 3,
                    IsDangerous = true,
                    MissileSpellName = "zyrapassivedeathmanager",
                    CollisionObjects = new[] {Spell.CollisionObjectTypes.YasuoWall}
                });

            #endregion Zyra
        }

        public static Spell GetByName(string spellName)
        {
            spellName = spellName.ToLower();
            foreach (var spellData in Spells)
            {
                if (spellData.SpellName.ToLower() == spellName || spellData.ExtraSpellNames.Contains(spellName))
                {
                    return spellData;
                }
            }

            return null;
        }

        public static Spell GetByMissileName(string missileSpellName)
        {
            missileSpellName = missileSpellName.ToLower();
            foreach (var spellData in Spells)
            {
                if (spellData.MissileSpellName != null && spellData.MissileSpellName.ToLower() == missileSpellName ||
                    spellData.ExtraMissileNames.Contains(missileSpellName))
                {
                    return spellData;
                }
            }

            return null;
        }

        public static Spell GetBySpeed(string ChampionName, int speed, int id = -1)
        {
            foreach (var spellData in Spells)
            {
                if (spellData.ChampionName == ChampionName && spellData.MissileSpeed == speed &&
                    (spellData.Id == -1 || id == spellData.Id))
                {
                    return spellData;
                }
            }

            return null;
        }
    }

    #endregion

    #region CC Spells

    public class CCDataBase
    {
        private static readonly string[] _nonskillshots =
        {
            "ShenShadowDash", "Pulverize", "GragasE", "FizzPiercingStrike", "reksaiqburrowed",
            "RunePrison", "Fling", "NocturneUnspeakableHorror", "SejuaniArcticAssault", "ShyvanaTransformCast",
            "PantheonW", "Ice Blast", "Terrify", "GalioIdolOfDurand", "GnarR", "JaxCounterStrike", "BlindMonkRKick",
            "UFSlash", "JayceThunderingBlow", "ZacR", "StaticField", "GarenQ", "TalonCutthroat", "ViR", "LissandraR",
            "CurseoftheSadMummy", "OrianaDetonateCommand"
        };

        private static readonly string[] _skillShots =
        {
            "AhriSeduce", "AhriOrbofDeception", "SwainShadowGrasp", "syndrae5", "SyndraE", "TahmKenchQ", "VarusE",
            "VeigarBalefulStrike", "VeigarDarkMatter", "VeigarEventHorizon", "VelkozQ", "VelkozQSplit", "VelkozE",
            "Laser", "Vi-q", "xeratharcanopulse2", "XerathArcaneBarrage2", "XerathMageSpear",
            "xerathrmissilewrapper", "BraumQ", "RocketGrab", "JavelinToss", "BrandBlazeMissile", "Heimerdingerwm",
            "JannaQ", "JarvanIVEQ", "BandageToss", "CaitlynEntrapment", "PhosphorusBomb", "MissileBarrage2",
            "DariusAxeGrabCone", "DianaArc", "DianaArcArc", "InfectedCleaverMissileCast", "DravenDoubleShot",
            "EkkoQ", "EkkoW", "EkkoR", "EliseHumanE", "GalioResoluteSmite", "GalioRighteousGust",
            "FlashFrost", "EvelynnR", "QuinnQ", "yasuoq3w",
            "RengarEFinal", "ZiggsW", "ZyraGraspingRoots", "ZyraBrambleZone", "Dazzle", "FiddlesticksDarkWind",
            "FeralScream", "ZiggsW", "ViktorChaosStorm", "AlZaharCalloftheVoid",
            "RumbleCarpetBombMissile", "ThreshQ", "ThreshE", "NamiQ", "DarkBindingMissile",
            "NautilusAnchorDrag",
            "SejuaniGlacialPrisonCast", "SonaR", "VarusR", "rivenizunablade", "EnchantedCrystalArrow", "BardR",
            "InfernalGuardian",
            "CassiopeiaPetrifyingGaze",
            "BraumRWrapper", "FizzMarinerDoomMissile", "ViktorDeathRay", "ViktorDeathRay3", "XerathMageSpear",
            "GragasR", "HecarimUlt", "LeonaSolarFlare", "LuxLightBinding", "LuxMaliceCannon", "JinxW",
            "LuxLightStrikeKugel"
        };

        public static bool IsCC(string spell)
        {
            return _skillShots.Contains(spell) || _nonskillshots.Contains(spell);
        }

        public static bool IsCC_SkillShot(string spell)
        {
            return _skillShots.Contains(spell);
        }

        public static bool IsCC_NonSkillShot(string spell)
        {
            return _nonskillshots.Contains(spell);
        }
    }

    #endregion

    #region Targeted Spell

    public class TargetSpell
    {
        public int blockBelow = 0;
        public int created = 0;
        public bool hadPart = false;
        public string particleName;
        public GameObjectProcessSpellCastEventArgs spellArgs;
        public AIHeroClient Caster { get; set; }
        public Obj_AI_Base Target { get; set; }
        public TargetSpellData Spell { get; set; }
        public int StartTick { get; set; }
        public Vector2 StartPosition { get; set; }
        public GameObject particle { get; set; }

        public int EndTick
        {
            get
            {
                return (Spell == null)
                    ? 0
                    : (int) (StartTick + Spell.Delay + 1000*(StartPosition.Distance(EndPosition)/Spell.Speed));
            }
        }

        public Vector2 EndPosition
        {
            get { return Target.ServerPosition.To2D(); }
        }

        public Vector2 Direction
        {
            get { return (EndPosition - StartPosition).Normalized(); }
        }

        public double Damage
        {
            get { return Caster.GetSpellDamage(Target, Spell.Spellslot); }
        }

        public bool IsKillable
        {
            get { return Damage >= Target.Health; }
        }

        public CcType CrowdControl
        {
            get { return Spell.CcType; }
        }

        public SpellType Type
        {
            get { return Spell.Type; }
        }

        public bool IsActive
        {
            get { return Environment.TickCount <= EndTick || StartTick + 4000 < Environment.TickCount; }
        }

        public bool HasMissile
        {
            get { return Spell.Speed == float.MaxValue || Spell.Speed == 0; }
        }
    }

    public static class TargetSpellDatabase
    {
        public static List<TargetSpellData> Spells;

        static TargetSpellDatabase()
        {
            Spells = new List<TargetSpellData>
            {
                #region Aatrox
                new TargetSpellData("aatrox", "aatroxq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 650, 500, 20),
                new TargetSpellData("aatrox", "aatroxw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxw2", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxe", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1000, 500, 1200),
                new TargetSpellData("aatrox", "aatroxr", SpellSlot.R, SpellType.Self, CcType.No, 550, 0, 0),

                #endregion Aatrox

                #region Ahri
                new TargetSpellData("ahri", "ahriorbofdeception", SpellSlot.Q, SpellType.Skillshot, CcType.No, 880, 500,
                    1100),
                new TargetSpellData("ahri", "ahrifoxfire", SpellSlot.W, SpellType.Self, CcType.No, 800, 0, 1800),
                new TargetSpellData("ahri", "ahriseduce", SpellSlot.E, SpellType.Skillshot, CcType.Charm, 975, 500, 1200),
                new TargetSpellData("ahri", "ahritumble", SpellSlot.R, SpellType.Skillshot, CcType.No, 450, 500, 2200),

                #endregion Ahri

                #region Akali
                new TargetSpellData("akali", "akalimota", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 650, 1000),
                new TargetSpellData("akali", "akalismokebomb", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 700, 500,
                    0),
                new TargetSpellData("akali", "akalishadowswipe", SpellSlot.E, SpellType.Self, CcType.No, 325, 0, 0),
                new TargetSpellData("akali", "akalishadowdance", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 0,
                    2200),

                #endregion Akali

                #region Alistar
                new TargetSpellData("alistar", "pulverize", SpellSlot.Q, SpellType.Self, CcType.Knockup, 365, 500, 20),
                new TargetSpellData("alistar", "headbutt", SpellSlot.W, SpellType.Targeted, CcType.Knockback, 650, 500,
                    0),
                new TargetSpellData("alistar", "triumphantroar", SpellSlot.E, SpellType.Self, CcType.No, 575, 0, 0),
                new TargetSpellData("alistar", "feroucioushowl", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 828),

                #endregion Alistar

                #region Amumu
                new TargetSpellData("amumu", "bandagetoss", SpellSlot.Q, SpellType.Skillshot, CcType.Stun, 1100, 500,
                    2000),
                new TargetSpellData("amumu", "auraofdespair", SpellSlot.W, SpellType.Self, CcType.No, 300, 470,
                    float.MaxValue),
                new TargetSpellData("amumu", "tantrum", SpellSlot.E, SpellType.Self, CcType.No, 350, 500, float.MaxValue),
                new TargetSpellData("amumu", "curseofthesadmummy", SpellSlot.R, SpellType.Self, CcType.Stun, 550, 500,
                    float.MaxValue),

                #endregion Amumu

                #region Anivia
                new TargetSpellData("anivia", "flashfrost", SpellSlot.Q, SpellType.Skillshot, CcType.Stun, 1200, 500,
                    850),
                new TargetSpellData("anivia", "crystalize", SpellSlot.W, SpellType.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("anivia", "frostbite", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 500, 1200),
                new TargetSpellData("anivia", "glacialstorm", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 675, 300,
                    float.MaxValue),

                #endregion Anivia

                #region Annie
                new TargetSpellData("annie", "disintegrate", SpellSlot.Q, SpellType.Targeted, CcType.No, 623, 500, 1400),
                new TargetSpellData("annie", "incinerate", SpellSlot.W, SpellType.Targeted, CcType.No, 623, 500, 0),
                new TargetSpellData("annie", "moltenshield", SpellSlot.E, SpellType.Self, CcType.No, 100, 0, 20),
                new TargetSpellData("annie", "infernalguardian", SpellSlot.R, SpellType.Skillshot, CcType.No, 600, 500,
                    float.MaxValue),

                #endregion Annie

                #region Ashe
                new TargetSpellData("ashe", "frostshot", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("ashe", "frostarrow", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 0, 0,
                    float.MaxValue),
                new TargetSpellData("ashe", "volley", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1200, 500, 902),
                new TargetSpellData("ashe", "ashespiritofthehawk", SpellSlot.E, SpellType.Skillshot, CcType.No, 2500,
                    500, 1400),
                new TargetSpellData("ashe", "enchantedcrystalarrow", SpellSlot.R, SpellType.Skillshot, CcType.Stun,
                    50000, 500, 1600),

                #endregion Ashe

                #region Blitzcrank
                new TargetSpellData("blitzcrank", "rocketgrabmissile", SpellSlot.Q, SpellType.Skillshot, CcType.Pull,
                    925, 220, 1800),
                new TargetSpellData("blitzcrank", "overdrive", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("blitzcrank", "powerfist", SpellSlot.E, SpellType.Self, CcType.Knockup, 0, 0, 0),
                new TargetSpellData("blitzcrank", "staticfield", SpellSlot.R, SpellType.Self, CcType.Silence, 600, 0, 0),

                #endregion Blitzcrank

                #region Brand
                new TargetSpellData("brand", "brandblaze", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1150, 500, 1200),
                new TargetSpellData("brand", "brandfissure", SpellSlot.W, SpellType.Skillshot, CcType.No, 240, 500, 20),
                new TargetSpellData("brand", "brandconflagration", SpellSlot.E, SpellType.Targeted, CcType.No, 0, 0,
                    1800),
                new TargetSpellData("brand", "brandwildfire", SpellSlot.R, SpellType.Targeted, CcType.No, 0, 0, 1000),

                #endregion Brand

                #region Braum
                new TargetSpellData("braum", "braumq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 500, 1200),
                new TargetSpellData("braum", "braumqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 500,
                    1200),
                new TargetSpellData("braum", "braumw", SpellSlot.W, SpellType.Targeted, CcType.No, 650, 500, 1500),
                new TargetSpellData("braum", "braume", SpellSlot.E, SpellType.Skillshot, CcType.No, 250, 0,
                    float.MaxValue),
                new TargetSpellData("braum", "braumr", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1250, 0, 1200),

                #endregion Braum

                #region Caitlyn
                new TargetSpellData("caitlyn", "caitlynpiltoverpeacemaker", SpellSlot.Q, SpellType.Skillshot, CcType.No,
                    2000, 250, 2200),
                new TargetSpellData("caitlyn", "caitlynyordletrap", SpellSlot.W, SpellType.Skillshot, CcType.Snare, 800,
                    0, 1400),
                new TargetSpellData("caitlyn", "caitlynentrapment", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 950,
                    250, 2000),
                new TargetSpellData("caitlyn", "caitlynaceinthehole", SpellSlot.R, SpellType.Targeted, CcType.No, 2500,
                    0, 1500),

                #endregion Caitlyn

                #region Cassiopeia
                new TargetSpellData("cassiopeia", "cassiopeianoxiousblast", SpellSlot.Q, SpellType.Skillshot, CcType.No,
                    925, 250, float.MaxValue),
                new TargetSpellData("cassiopeia", "cassiopeiamiasma", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 925,
                    500, 2500),
                new TargetSpellData("cassiopeia", "cassiopeiatwinfang", SpellSlot.E, SpellType.Targeted, CcType.No, 700,
                    0, 1900),
                new TargetSpellData("cassiopeia", "cassiopeiapetrifyinggaze", SpellSlot.R, SpellType.Skillshot,
                    CcType.Stun, 875, 500, float.MaxValue),

                #endregion Cassiopeia

                #region Chogath
                new TargetSpellData("chogath", "rupture", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1000, 500,
                    float.MaxValue),
                new TargetSpellData("chogath", "feralscream", SpellSlot.W, SpellType.Skillshot, CcType.Silence, 675, 250,
                    float.MaxValue),
                new TargetSpellData("chogath", "vorpalspikes", SpellSlot.E, SpellType.Targeted, CcType.No, 0, 0, 347),
                new TargetSpellData("chogath", "feast", SpellSlot.R, SpellType.Targeted, CcType.No, 230, 0, 500),

                #endregion Chogath

                #region Corki
                new TargetSpellData("corki", "phosphorusbomb", SpellSlot.Q, SpellType.Skillshot, CcType.No, 875, 0,
                    float.MaxValue),
                new TargetSpellData("corki", "carpetbomb", SpellSlot.W, SpellType.Skillshot, CcType.No, 875, 0, 700),
                new TargetSpellData("corki", "ggun", SpellSlot.E, SpellType.Skillshot, CcType.No, 750, 0, 902),
                new TargetSpellData("corki", "missilebarrage", SpellSlot.R, SpellType.Skillshot, CcType.No, 1225, 250,
                    828),

                #endregion Corki

                #region Darius
                new TargetSpellData("darius", "dariuscleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 425, 500, 0),
                new TargetSpellData("darius", "dariusnoxiantacticsonh", SpellSlot.W, SpellType.Self, CcType.Slow, 210, 0,
                    0),
                new TargetSpellData("darius", "dariusaxegrabcone", SpellSlot.E, SpellType.Skillshot, CcType.Pull, 540,
                    500, 1500),
                new TargetSpellData("darius", "dariusexecute", SpellSlot.R, SpellType.Targeted, CcType.No, 460, 500, 20),

                #endregion Darius

                #region Diana
                new TargetSpellData("diana", "dianaarc", SpellSlot.Q, SpellType.Skillshot, CcType.No, 900, 500, 1500),
                new TargetSpellData("diana", "dianaorbs", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("diana", "dianavortex", SpellSlot.E, SpellType.Self, CcType.Pull, 300, 500, 1500),
                new TargetSpellData("diana", "dianateleport", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 500, 1500),

                #endregion Diana

                #region Draven
                new TargetSpellData("draven", "dravenspinning", SpellSlot.Q, SpellType.Self, CcType.No, 0,
                    float.MaxValue, float.MaxValue),
                new TargetSpellData("draven", "dravenfury", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("draven", "dravendoubleshot", SpellSlot.E, SpellType.Skillshot, CcType.Knockback,
                    1050, 500, 1600),
                new TargetSpellData("draven", "dravenrcast", SpellSlot.R, SpellType.Skillshot, CcType.No, 20000, 500,
                    2000),

                #endregion Draven

                #region DrMundo
                new TargetSpellData("drmundo", "infectedcleavermissilecast", SpellSlot.Q, SpellType.Skillshot,
                    CcType.Slow, 1000, 500, 1500),
                new TargetSpellData("drmundo", "burningagony", SpellSlot.W, SpellType.Self, CcType.No, 225,
                    float.MaxValue, float.MaxValue),
                new TargetSpellData("drmundo", "masochism", SpellSlot.E, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("drmundo", "sadism", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),

                #endregion DrMundo

                #region Elise
                new TargetSpellData("elise", "elisehumanq", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 750, 2200),
                new TargetSpellData("elise", "elisespiderqcast", SpellSlot.Q, SpellType.Targeted, CcType.No, 475, 500,
                    float.MaxValue),
                new TargetSpellData("elise", "elisehumanw", SpellSlot.W, SpellType.Skillshot, CcType.No, 950, 750, 5000),
                new TargetSpellData("elise", "elisespiderw", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("elise", "elisehumane", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1075, 500,
                    1450),
                new TargetSpellData("elise", "elisespidereinitial", SpellSlot.E, SpellType.Targeted, CcType.No, 975,
                    float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisespideredescent", SpellSlot.E, SpellType.Targeted, CcType.No, 975,
                    float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "eliser", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("elise", "elisespiderr", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),

                #endregion Elise

                #region Evelynn
                new TargetSpellData("evelynn", "evelynnq", SpellSlot.Q, SpellType.Self, CcType.No, 500, 500,
                    float.MaxValue),
                new TargetSpellData("evelynn", "evelynnw", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("evelynn", "evelynne", SpellSlot.E, SpellType.Targeted, CcType.No, 290, 500, 900),
                new TargetSpellData("evelynn", "evelynnr", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 650, 500, 1300),

                #endregion Evelynn

                #region Ezreal
                new TargetSpellData("ezreal", "ezrealmysticshot", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1200, 250,
                    2000),
                new TargetSpellData("ezreal", "ezrealessenceflux", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050,
                    250, 1600),
                new TargetSpellData("ezreal", "ezrealessencemissle", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050,
                    250, 1600),
                new TargetSpellData("ezreal", "ezrealarcaneshift", SpellSlot.E, SpellType.Targeted, CcType.No, 475, 500,
                    float.MaxValue),
                new TargetSpellData("ezreal", "ezrealtruehotbarrage", SpellSlot.R, SpellType.Skillshot, CcType.No, 20000,
                    1000, 2000),

                #endregion Ezreal

                #region FiddleSticks
                new TargetSpellData("fiddlesticks", "terrify", SpellSlot.Q, SpellType.Targeted, CcType.Fear, 575, 500,
                    float.MaxValue),
                new TargetSpellData("fiddlesticks", "drain", SpellSlot.W, SpellType.Targeted, CcType.No, 575, 500,
                    float.MaxValue),
                new TargetSpellData("fiddlesticks", "fiddlesticksdarkwind", SpellSlot.E, SpellType.Skillshot,
                    CcType.Silence, 750, 500, 1100),
                new TargetSpellData("fiddlesticks", "crowstorm", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 500,
                    float.MaxValue),

                #endregion FiddleSticks

                #region Fiora
                new TargetSpellData("fiora", "fioraq", SpellSlot.Q, SpellType.Targeted, CcType.No, 300, 500, 2200),
                new TargetSpellData("fiora", "fiorariposte", SpellSlot.W, SpellType.Self, CcType.No, 100, 0, 0),
                new TargetSpellData("fiora", "fioraflurry", SpellSlot.E, SpellType.Self, CcType.No, 210, 0, 0),
                new TargetSpellData("fiora", "fioradance", SpellSlot.R, SpellType.Targeted, CcType.No, 210, 500, 0),

                #endregion Fiora

                #region Fizz
                new TargetSpellData("fizz", "fizzpiercingstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 550, 500,
                    float.MaxValue),
                new TargetSpellData("fizz", "fizzseastonepassive", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("fizz", "fizzjump", SpellSlot.E, SpellType.Self, CcType.No, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzjumptwo", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzmarinerdoom", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1275,
                    500, 1200),

                #endregion Fizz

                #region Galio
                new TargetSpellData("galio", "galioresolutesmite", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 940,
                    500, 1300),
                new TargetSpellData("galio", "galiobulwark", SpellSlot.W, SpellType.Targeted, CcType.No, 800, 500,
                    float.MaxValue),
                new TargetSpellData("galio", "galiorighteousgust", SpellSlot.E, SpellType.Skillshot, CcType.No, 1180,
                    500, 1200),
                new TargetSpellData("galio", "galioidolofdurand", SpellSlot.R, SpellType.Self, CcType.Taunt, 560, 500,
                    float.MaxValue),

                #endregion Galio

                #region Gangplank
                new TargetSpellData("gangplank", "parley", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, 2000),
                new TargetSpellData("gangplank", "removescurvy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("gangplank", "raisemorale", SpellSlot.E, SpellType.Self, CcType.No, 1300, 500,
                    float.MaxValue),
                new TargetSpellData("gangplank", "cannonbarrage", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 20000,
                    500, 500),

                #endregion Gangplank

                #region Garen
                new TargetSpellData("garen", "garenq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 200, float.MaxValue),
                new TargetSpellData("garen", "garenw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("garen", "garene", SpellSlot.E, SpellType.Self, CcType.No, 325, 0, 700),
                new TargetSpellData("garen", "garenr", SpellSlot.R, SpellType.Targeted, CcType.No, 400, 120,
                    float.MaxValue),

                #endregion Garen

                #region Gragas
                new TargetSpellData("gragas", "gragasq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasqtoggle", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100, 300,
                    1000),
                new TargetSpellData("gragas", "gragasw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("gragas", "gragase", SpellSlot.E, SpellType.Skillshot, CcType.Knockback, 1100, 300,
                    1000),
                new TargetSpellData("gragas", "gragasr", SpellSlot.R, SpellType.Skillshot, CcType.Knockback, 1100, 300,
                    1000),

                #endregion Gragas

                #region Graves
                new TargetSpellData("graves", "gravesclustershot", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100,
                    300, 902),
                new TargetSpellData("graves", "gravessmokegrenade", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1100,
                    300, 1650),
                new TargetSpellData("graves", "gravessmokegrenadeboom", SpellSlot.W, SpellType.Skillshot, CcType.Slow,
                    1100, 300, 1650),
                new TargetSpellData("graves", "gravesmove", SpellSlot.E, SpellType.Skillshot, CcType.No, 425, 300, 1000),
                new TargetSpellData("graves", "graveschargeshot", SpellSlot.R, SpellType.Skillshot, CcType.No, 1000, 500,
                    1200),

                #endregion Graves

                #region Hecarim
                new TargetSpellData("hecarim", "hecarimrapidslash", SpellSlot.Q, SpellType.Self, CcType.No, 350, 300,
                    1450),
                new TargetSpellData("hecarim", "hecarimw", SpellSlot.W, SpellType.Self, CcType.No, 525, 120, 828),
                new TargetSpellData("hecarim", "hecarimramp", SpellSlot.E, SpellType.Self, CcType.No, 0, float.MaxValue,
                    float.MaxValue),
                new TargetSpellData("hecarim", "hecarimult", SpellSlot.R, SpellType.Skillshot, CcType.Fear, 1350, 500,
                    1200),

                #endregion Hecarim

                #region Heimerdinger
                new TargetSpellData("heimerdinger", "heimerdingerq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 350,
                    500, float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingerw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1525,
                    500, 902),
                new TargetSpellData("heimerdinger", "heimerdingere", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 970,
                    500, 2500),
                new TargetSpellData("heimerdinger", "heimerdingerr", SpellSlot.R, SpellType.Self, CcType.No, 0, 230,
                    float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingereult", SpellSlot.R, SpellType.Skillshot, CcType.Stun,
                    970, 230, float.MaxValue),

                #endregion Heimerdinger

                #region Irelia
                new TargetSpellData("irelia", "ireliagatotsu", SpellSlot.Q, SpellType.Targeted, CcType.No, 650, 0, 2200),
                new TargetSpellData("irelia", "ireliahitenstyle", SpellSlot.W, SpellType.Self, CcType.No, 0, 230, 347),
                new TargetSpellData("irelia", "ireliaequilibriumstrike", SpellSlot.E, SpellType.Targeted, CcType.Stun,
                    325, 500, float.MaxValue),
                new TargetSpellData("irelia", "ireliatranscendentblades", SpellSlot.R, SpellType.Skillshot, CcType.No,
                    1200, 500, 779),

                #endregion Irelia

                #region Janna
                new TargetSpellData("janna", "howlinggale", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1800, 0,
                    float.MaxValue),
                new TargetSpellData("janna", "sowthewind", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600, 500, 1600),
                new TargetSpellData("janna", "eyeofthestorm", SpellSlot.E, SpellType.Targeted, CcType.No, 800, 500,
                    float.MaxValue),
                new TargetSpellData("janna", "reapthewhirlwind", SpellSlot.R, SpellType.Self, CcType.Knockback, 725, 500,
                    828),

                #endregion Janna

                #region JarvanIV
                new TargetSpellData("jarvaniv", "jarvanivdragonstrike", SpellSlot.Q, SpellType.Skillshot, CcType.No, 700,
                    500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivgoldenaegis", SpellSlot.W, SpellType.Self, CcType.Slow, 300,
                    500, 0),
                new TargetSpellData("jarvaniv", "jarvanivdemacianstandard", SpellSlot.E, SpellType.Skillshot, CcType.No,
                    830, 500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivcataclysm", SpellSlot.R, SpellType.Skillshot, CcType.No, 650,
                    500, 0),

                #endregion JarvanIV

                #region Jax
                new TargetSpellData("jax", "jaxleapstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 210, 500, 0),
                new TargetSpellData("jax", "jaxempowertwo", SpellSlot.W, SpellType.Targeted, CcType.No, 0, 500, 0),
                new TargetSpellData("jax", "jaxcounterstrike", SpellSlot.E, SpellType.Self, CcType.Stun, 425, 500, 1450),
                new TargetSpellData("jax", "jaxrelentlessasssault", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 0),

                #endregion Jax

                #region Jayce
                new TargetSpellData("jayce", "jaycetotheskies", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 600, 500,
                    float.MaxValue),
                new TargetSpellData("jayce", "jayceshockblast", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1050, 500,
                    1200),
                new TargetSpellData("jayce", "jaycestaticfield", SpellSlot.W, SpellType.Self, CcType.No, 285, 500, 1500),
                new TargetSpellData("jayce", "jaycehypercharge", SpellSlot.W, SpellType.Self, CcType.No, 0, 750,
                    float.MaxValue),
                new TargetSpellData("jayce", "jaycethunderingblow", SpellSlot.E, SpellType.Targeted, CcType.Knockback,
                    300, 0, float.MaxValue),
                new TargetSpellData("jayce", "jayceaccelerationgate", SpellSlot.E, SpellType.Skillshot, CcType.No, 685,
                    500, 1600),
                new TargetSpellData("jayce", "jaycestancehtg", SpellSlot.R, SpellType.Self, CcType.No, 0, 750,
                    float.MaxValue),
                new TargetSpellData("jayce", "jaycestancegth", SpellSlot.R, SpellType.Self, CcType.No, 0, 750,
                    float.MaxValue),

                #endregion Jayce

                #region Jinx
                new TargetSpellData("jinx", "jinxq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("jinx", "jinxw", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1550, 500, 1200),
                new TargetSpellData("jinx", "jinxwmissle", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1550, 500,
                    1200),
                new TargetSpellData("jinx", "jinxe", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 900, 500, 1000),
                new TargetSpellData("jinx", "jinxr", SpellSlot.R, SpellType.Skillshot, CcType.No, 25000, 0,
                    float.MaxValue),
                new TargetSpellData("jinx", "jinxrwrapper", SpellSlot.R, SpellType.Skillshot, CcType.No, 25000, 0,
                    float.MaxValue),

                #endregion Jinx

                #region Karma
                new TargetSpellData("karma", "karmaq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 950, 500, 902),
                new TargetSpellData("karma", "karmaspiritbind", SpellSlot.W, SpellType.Targeted, CcType.Snare, 700, 500,
                    2000),
                new TargetSpellData("karma", "karmasolkimshield", SpellSlot.E, SpellType.Targeted, CcType.No, 800, 500,
                    float.MaxValue),
                new TargetSpellData("karma", "karmamantra", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, 1300),

                #endregion Karma

                #region Karthus
                new TargetSpellData("karthus", "laywaste", SpellSlot.Q, SpellType.Skillshot, CcType.No, 875, 500,
                    float.MaxValue),
                new TargetSpellData("karthus", "wallofpain", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1090, 500,
                    1600),
                new TargetSpellData("karthus", "defile", SpellSlot.E, SpellType.Self, CcType.No, 550, 500, 1000),
                new TargetSpellData("karthus", "fallenone", SpellSlot.R, SpellType.Self, CcType.No, 20000, 0,
                    float.MaxValue),

                #endregion Karthus

                #region Kassadin
                new TargetSpellData("kassadin", "nulllance", SpellSlot.Q, SpellType.Targeted, CcType.Silence, 650, 500,
                    1400),
                new TargetSpellData("kassadin", "netherblade", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("kassadin", "forcepulse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 700, 500,
                    float.MaxValue),
                new TargetSpellData("kassadin", "riftwalk", SpellSlot.R, SpellType.Skillshot, CcType.No, 675, 500,
                    float.MaxValue),

                #endregion Kassadin

                #region Katarina
                new TargetSpellData("katarina", "katarinaq", SpellSlot.Q, SpellType.Targeted, CcType.No, 675, 500, 1800),
                new TargetSpellData("katarina", "katarinaw", SpellSlot.W, SpellType.Self, CcType.No, 400, 500, 1800),
                new TargetSpellData("katarina", "katarinae", SpellSlot.E, SpellType.Targeted, CcType.No, 700, 500, 0),
                new TargetSpellData("katarina", "katarinar", SpellSlot.R, SpellType.Self, CcType.No, 550, 500, 1450),

                #endregion Katarina

                #region Kayle
                new TargetSpellData("kayle", "judicatorreckoning", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 650,
                    500, 1500),
                new TargetSpellData("kayle", "judicatordevineblessing", SpellSlot.W, SpellType.Targeted, CcType.No, 900,
                    220, float.MaxValue),
                new TargetSpellData("kayle", "judicatorrighteousfury", SpellSlot.E, SpellType.Self, CcType.No, 0, 500,
                    779),
                new TargetSpellData("kayle", "judicatorintervention", SpellSlot.R, SpellType.Targeted, CcType.No, 900,
                    500, float.MaxValue),

                #endregion Kayle

                #region Kennen
                new TargetSpellData("kennen", "kennenshurikenhurlmissile1", SpellSlot.Q, SpellType.Skillshot, CcType.No,
                    1000, 690, 1700),
                new TargetSpellData("kennen", "kennenbringthelight", SpellSlot.W, SpellType.Self, CcType.No, 900, 500,
                    float.MaxValue),
                new TargetSpellData("kennen", "kennenlightningrush", SpellSlot.E, SpellType.Self, CcType.No, 0, 0,
                    float.MaxValue),
                new TargetSpellData("kennen", "kennenshurikenstorm", SpellSlot.R, SpellType.Self, CcType.No, 550, 500,
                    779),

                #endregion Kennen

                #region Khazix
                new TargetSpellData("khazix", "khazixq", SpellSlot.Q, SpellType.Targeted, CcType.No, 325, 500,
                    float.MaxValue),
                new TargetSpellData("khazix", "khazixqlong", SpellSlot.Q, SpellType.Targeted, CcType.No, 375, 500,
                    float.MaxValue),
                new TargetSpellData("khazix", "khazixw", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1000, 500, 828),
                new TargetSpellData("khazix", "khazixwlong", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1000, 500,
                    828),
                new TargetSpellData("khazix", "khazixe", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 500,
                    float.MaxValue),
                new TargetSpellData("khazix", "khazixelong", SpellSlot.E, SpellType.Skillshot, CcType.No, 900, 500,
                    float.MaxValue),
                new TargetSpellData("khazix", "khazixr", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("khazix", "khazixrlong", SpellSlot.R, SpellType.Self, CcType.No, 0, 0,
                    float.MaxValue),

                #endregion Khazix

                #region KogMaw
                new TargetSpellData("kogmaw", "kogmawcausticspittle", SpellSlot.Q, SpellType.Targeted, CcType.No, 625,
                    500, float.MaxValue),
                new TargetSpellData("kogmaw", "kogmawbioarcanbarrage", SpellSlot.W, SpellType.Self, CcType.No, 130, 500,
                    2000),
                new TargetSpellData("kogmaw", "kogmawvoidooze", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000, 500,
                    1200),
                new TargetSpellData("kogmaw", "kogmawlivingartillery", SpellSlot.R, SpellType.Skillshot, CcType.No, 1400,
                    600, 2000),

                #endregion KogMaw

                #region Leblanc
                new TargetSpellData("leblanc", "leblancchaosorb", SpellSlot.Q, SpellType.Targeted, CcType.No, 700, 500,
                    2000),
                new TargetSpellData("leblanc", "leblancslide", SpellSlot.W, SpellType.Skillshot, CcType.No, 600, 500,
                    float.MaxValue),
                new TargetSpellData("leblanc", "leblacslidereturn", SpellSlot.W, SpellType.Skillshot, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshackle", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 925,
                    500, 1600),
                new TargetSpellData("leblanc", "leblancchaosorbm", SpellSlot.R, SpellType.Targeted, CcType.No, 700, 500,
                    2000),
                new TargetSpellData("leblanc", "leblancslidem", SpellSlot.R, SpellType.Skillshot, CcType.No, 600, 500,
                    float.MaxValue),
                new TargetSpellData("leblanc", "leblancslidereturnm", SpellSlot.R, SpellType.Skillshot, CcType.No, 0,
                    500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshacklem", SpellSlot.R, SpellType.Skillshot, CcType.No, 925,
                    500, 1600),

                #endregion Leblanc

                #region LeeSin
                new TargetSpellData("leesin", "blindmonkqone", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1000, 500,
                    1800),
                new TargetSpellData("leesin", "blindmonkqtwo", SpellSlot.Q, SpellType.Targeted, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("leesin", "blindmonkwone", SpellSlot.W, SpellType.Targeted, CcType.No, 700, 0, 1500),
                new TargetSpellData("leesin", "blindmonkwtwo", SpellSlot.W, SpellType.Self, CcType.No, 700, 0,
                    float.MaxValue),
                new TargetSpellData("leesin", "blindmonkeone", SpellSlot.E, SpellType.Self, CcType.No, 425, 500,
                    float.MaxValue),
                new TargetSpellData("leesin", "blindmonketwo", SpellSlot.E, SpellType.Self, CcType.Slow, 425, 500,
                    float.MaxValue),
                new TargetSpellData("leesin", "blindmonkrkick", SpellSlot.R, SpellType.Targeted, CcType.Knockback, 375,
                    500, 1500),

                #endregion LeeSin

                #region Leona
                new TargetSpellData("leona", "leonashieldofdaybreak", SpellSlot.Q, SpellType.Self, CcType.Stun, 215, 0,
                    0),
                new TargetSpellData("leona", "leonasolarbarrier", SpellSlot.W, SpellType.Self, CcType.No, 500, 3000, 0),
                new TargetSpellData("leona", "leonazenithblade", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 900, 0,
                    2000),
                new TargetSpellData("leona", "leonazenithblademissle", SpellSlot.E, SpellType.Skillshot, CcType.Stun,
                    900, 0, 2000),
                new TargetSpellData("leona", "leonasolarflare", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 1200, 700,
                    float.MaxValue),

                #endregion Leona

                #region Lissandra
                new TargetSpellData("lissandra", "lissandraq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 725, 500,
                    1200),
                new TargetSpellData("lissandra", "lissandraw", SpellSlot.W, SpellType.Self, CcType.Snare, 450, 500,
                    float.MaxValue),
                new TargetSpellData("lissandra", "lissandrae", SpellSlot.E, SpellType.Skillshot, CcType.No, 1050, 500,
                    850),
                new TargetSpellData("lissandra", "lissandrar", SpellSlot.R, SpellType.Targeted, CcType.Stun, 550, 0,
                    float.MaxValue),

                #endregion Lissandra

                #region Lucian
                new TargetSpellData("lucian", "lucianpassiveshot", SpellSlot.Unknown, SpellType.Targeted, CcType.No, 550,
                    500, 500),
                new TargetSpellData("lucian", "lucianq", SpellSlot.Q, SpellType.Targeted, CcType.No, 550, 500, 500),
                new TargetSpellData("lucian", "lucianw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1000, 500, 500),
                new TargetSpellData("lucian", "luciane", SpellSlot.E, SpellType.Skillshot, CcType.No, 650, 500,
                    float.MaxValue),
                new TargetSpellData("lucian", "lucianr", SpellSlot.R, SpellType.Targeted, CcType.No, 1400, 500,
                    float.MaxValue),

                #endregion Lucian

                #region Lulu
                new TargetSpellData("lulu", "luluq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluw", SpellSlot.W, SpellType.Targeted, CcType.Polymorph, 650, 640, 2000),
                new TargetSpellData("lulu", "lulue", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 640,
                    float.MaxValue),
                new TargetSpellData("lulu", "lulur", SpellSlot.R, SpellType.Targeted, CcType.Knockup, 900, 500,
                    float.MaxValue),

                #endregion Lulu

                #region Lux
                new TargetSpellData("lux", "luxlightbinding", SpellSlot.Q, SpellType.Skillshot, CcType.Snare, 1300, 500,
                    1200),
                new TargetSpellData("lux", "luxprismaticwave", SpellSlot.W, SpellType.Skillshot, CcType.No, 1075, 500,
                    1200),
                new TargetSpellData("lux", "luxlightstrikekugel", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100,
                    500, 1300),
                new TargetSpellData("lux", "luxlightstriketoggle", SpellSlot.E, SpellType.Skillshot, CcType.No, 1100,
                    500, 1300),
                new TargetSpellData("lux", "luxmalicecannon", SpellSlot.R, SpellType.Skillshot, CcType.No, 3340, 1750,
                    3000),
                new TargetSpellData("lux", "luxmalicecannonmis", SpellSlot.R, SpellType.Skillshot, CcType.No, 3340, 1750,
                    3000),

                #endregion Lux

                #region Malphite
                new TargetSpellData("malphite", "seismicshard", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 625, 500,
                    1200),
                new TargetSpellData("malphite", "obduracy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("malphite", "landslide", SpellSlot.E, SpellType.Self, CcType.No, 400, 500,
                    float.MaxValue),
                new TargetSpellData("malphite", "ufslash", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1000, 0,
                    700),

                #endregion Malphite

                #region Malzahar
                new TargetSpellData("malzahar", "alzaharcallofthevoid", SpellSlot.Q, SpellType.Skillshot, CcType.Silence,
                    900, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnullzone", SpellSlot.W, SpellType.Skillshot, CcType.No, 800, 500,
                    float.MaxValue),
                new TargetSpellData("malzahar", "alzaharmaleficvisions", SpellSlot.E, SpellType.Targeted, CcType.No, 650,
                    500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnethergrasp", SpellSlot.R, SpellType.Targeted,
                    CcType.Suppression, 700, 500, float.MaxValue),

                #endregion Malzahar

                #region Maokai
                new TargetSpellData("maokai", "maokaitrunkline", SpellSlot.Q, SpellType.Skillshot, CcType.Knockback, 600,
                    500, 1200),
                new TargetSpellData("maokai", "maokaiunstablegrowth", SpellSlot.W, SpellType.Targeted, CcType.Snare, 650,
                    500, float.MaxValue),
                new TargetSpellData("maokai", "maokaisapling2", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100, 500,
                    1750),
                new TargetSpellData("maokai", "maokaidrain3", SpellSlot.R, SpellType.Targeted, CcType.No, 625, 500,
                    float.MaxValue),

                #endregion Maokai

                #region MasterYi
                new TargetSpellData("masteryi", "alphastrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500,
                    4000),
                new TargetSpellData("masteryi", "meditate", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("masteryi", "wujustyle", SpellSlot.E, SpellType.Self, CcType.No, 0, 230,
                    float.MaxValue),
                new TargetSpellData("masteryi", "highlander", SpellSlot.R, SpellType.Self, CcType.No, 0, 370,
                    float.MaxValue),

                #endregion MasterYi

                #region MissFortune
                new TargetSpellData("missfortune", "missfortunericochetshot", SpellSlot.Q, SpellType.Targeted, CcType.No,
                    650, 500, 1400),
                new TargetSpellData("missfortune", "missfortuneviciousstrikes", SpellSlot.W, SpellType.Self, CcType.No,
                    0, 0, float.MaxValue),
                new TargetSpellData("missfortune", "missfortunescattershot", SpellSlot.E, SpellType.Skillshot,
                    CcType.Slow, 1000, 500, 500),
                new TargetSpellData("missfortune", "missfortunebullettime", SpellSlot.R, SpellType.Skillshot, CcType.No,
                    1400, 500, 775),

                #endregion MissFortune

                #region MonkeyKing
                new TargetSpellData("monkeyking", "monkeykingdoubleattack", SpellSlot.Q, SpellType.Self, CcType.No, 300,
                    500, 20),
                new TargetSpellData("monkeyking", "monkeykingdecoy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("monkeyking", "monkeykingdecoyswipe", SpellSlot.W, SpellType.Self, CcType.No, 325,
                    500, 0),
                new TargetSpellData("monkeyking", "monkeykingnimbus", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 0,
                    2200),
                new TargetSpellData("monkeyking", "monkeykingspintowin", SpellSlot.R, SpellType.Self, CcType.Knockup,
                    315, 0, 700),
                new TargetSpellData("monkeyking", "monkeykingspintowinleave", SpellSlot.R, SpellType.Self, CcType.No, 0,
                    0, 700),

                #endregion MonkeyKing

                #region Mordekaiser
                new TargetSpellData("mordekaiser", "mordekaisermaceofspades", SpellSlot.Q, SpellType.Self, CcType.No,
                    600, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaisercreepindeathcast", SpellSlot.W, SpellType.Targeted,
                    CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("mordekaiser", "mordekaisersyphoneofdestruction", SpellSlot.E, SpellType.Skillshot,
                    CcType.No, 700, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaiserchildrenofthegrave", SpellSlot.R, SpellType.Targeted,
                    CcType.No, 850, 500, 1500),

                #endregion Mordekaiser

                #region Morgana
                new TargetSpellData("morgana", "darkbindingmissile", SpellSlot.Q, SpellType.Skillshot, CcType.Snare,
                    1300, 500, 1200),
                new TargetSpellData("morgana", "tormentedsoil", SpellSlot.W, SpellType.Skillshot, CcType.No, 1075, 500,
                    float.MaxValue),
                new TargetSpellData("morgana", "blackshield", SpellSlot.E, SpellType.Targeted, CcType.No, 750, 500,
                    float.MaxValue),
                new TargetSpellData("morgana", "soulshackles", SpellSlot.R, SpellType.Self, CcType.Stun, 600, 500,
                    float.MaxValue),

                #endregion Morgana

                #region Nami
                new TargetSpellData("nami", "namiq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 875, 500, 1750),
                new TargetSpellData("nami", "namiw", SpellSlot.W, SpellType.Targeted, CcType.No, 725, 500, 1100),
                new TargetSpellData("nami", "namie", SpellSlot.E, SpellType.Targeted, CcType.Slow, 800, 500,
                    float.MaxValue),
                new TargetSpellData("nami", "namir", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 2550, 500, 1200),

                #endregion Nami

                #region Nasus
                new TargetSpellData("nasus", "nasusq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasusw", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600, 500,
                    float.MaxValue),
                new TargetSpellData("nasus", "nasuse", SpellSlot.E, SpellType.Skillshot, CcType.No, 850, 500,
                    float.MaxValue),
                new TargetSpellData("nasus", "nasusr", SpellSlot.R, SpellType.Skillshot, CcType.No, 1, 500,
                    float.MaxValue),

                #endregion Nasus

                #region Nautilus
                new TargetSpellData("nautilus", "nautilusanchordrag", SpellSlot.Q, SpellType.Skillshot, CcType.Pull, 950,
                    500, 1200),
                new TargetSpellData("nautilus", "nautiluspiercinggaze", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("nautilus", "nautilussplashzone", SpellSlot.E, SpellType.Self, CcType.Slow, 600, 500,
                    1300),
                new TargetSpellData("nautilus", "nautilusgandline", SpellSlot.R, SpellType.Targeted, CcType.Knockup,
                    1500, 500, 1400),

                #endregion Nautilus

                #region Nidalee
                new TargetSpellData("nidalee", "javelintoss", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1500, 500,
                    1300),
                new TargetSpellData("nidalee", "takedown", SpellSlot.Q, SpellType.Self, CcType.No, 50, 0, 500),
                new TargetSpellData("nidalee", "bushwhack", SpellSlot.W, SpellType.Skillshot, CcType.No, 900, 500, 1450),
                new TargetSpellData("nidalee", "pounce", SpellSlot.W, SpellType.Skillshot, CcType.No, 375, 500, 1500),
                new TargetSpellData("nidalee", "primalsurge", SpellSlot.E, SpellType.Targeted, CcType.No, 600, 0,
                    float.MaxValue),
                new TargetSpellData("nidalee", "swipe", SpellSlot.E, SpellType.Skillshot, CcType.No, 300, 500,
                    float.MaxValue),
                new TargetSpellData("nidalee", "aspectofthecougar", SpellSlot.R, SpellType.Self, CcType.No, 0, 0,
                    float.MaxValue),

                #endregion Nidalee

                #region Nocturne
                new TargetSpellData("nocturne", "nocturneduskbringer", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1125,
                    500, 1600),
                new TargetSpellData("nocturne", "nocturneshroudofdarkness", SpellSlot.W, SpellType.Self, CcType.No, 0,
                    500, 500),
                new TargetSpellData("nocturne", "nocturneunspeakablehorror", SpellSlot.E, SpellType.Targeted,
                    CcType.Fear, 500, 500, 0),
                new TargetSpellData("nocturne", "nocturneparanoia", SpellSlot.R, SpellType.Targeted, CcType.No, 2000,
                    500, 500),

                #endregion Nocturne

                #region Nunu
                new TargetSpellData("nunu", "consume", SpellSlot.Q, SpellType.Targeted, CcType.No, 125, 500, 1400),
                new TargetSpellData("nunu", "bloodboil", SpellSlot.W, SpellType.Targeted, CcType.No, 700, 500,
                    float.MaxValue),
                new TargetSpellData("nunu", "iceblast", SpellSlot.E, SpellType.Targeted, CcType.Slow, 550, 500, 1000),
                new TargetSpellData("nunu", "absolutezero", SpellSlot.R, SpellType.Self, CcType.Slow, 650, 500,
                    float.MaxValue),

                #endregion Nunu

                #region Olaf
                new TargetSpellData("olaf", "olafaxethrowcast", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1000, 500,
                    1600),
                new TargetSpellData("olaf", "olaffrenziedstrikes", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("olaf", "olafrecklessstrike", SpellSlot.E, SpellType.Targeted, CcType.No, 325, 500,
                    float.MaxValue),
                new TargetSpellData("olaf", "olafragnarok", SpellSlot.R, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),

                #endregion Olaf

                #region Orianna
                new TargetSpellData("orianna", "orianaizunacommand", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100,
                    500, 1200),
                new TargetSpellData("orianna", "orianadissonancecommand", SpellSlot.W, SpellType.Skillshot, CcType.Slow,
                    0, 500, 1200),
                new TargetSpellData("orianna", "orianaredactcommand", SpellSlot.E, SpellType.Targeted, CcType.No, 1095,
                    500, 1200),
                new TargetSpellData("orianna", "orianadetonatecommand", SpellSlot.R, SpellType.Skillshot, CcType.Pull, 0,
                    500, 1200),

                #endregion Orianna

                #region Pantheon
                new TargetSpellData("pantheon", "pantheonq", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1500),
                new TargetSpellData("pantheon", "pantheonw", SpellSlot.W, SpellType.Targeted, CcType.Stun, 600, 500,
                    float.MaxValue),
                new TargetSpellData("pantheon", "pantheone", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 500, 775),
                new TargetSpellData("pantheon", "pantheonrjump", SpellSlot.R, SpellType.Skillshot, CcType.No, 5500, 1000,
                    3000),
                new TargetSpellData("pantheon", "pantheonrfall", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 5500,
                    1000, 3000),

                #endregion Pantheon

                #region Poppy
                new TargetSpellData("poppy", "poppydevastatingblow", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("poppy", "poppyparagonofdemacia", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("poppy", "poppyheroiccharge", SpellSlot.E, SpellType.Targeted, CcType.Stun, 525, 500,
                    1450),
                new TargetSpellData("poppy", "poppydiplomaticimmunity", SpellSlot.R, SpellType.Targeted, CcType.No, 900,
                    500, float.MaxValue),

                #endregion Poppy

                #region Quinn
                new TargetSpellData("quinn", "quinnq", SpellSlot.Q, SpellType.Skillshot, CcType.Blind, 1025, 500, 1200),
                new TargetSpellData("quinn", "quinnw", SpellSlot.W, SpellType.Self, CcType.No, 2100, 0, 0),
                new TargetSpellData("quinn", "quinne", SpellSlot.E, SpellType.Targeted, CcType.Knockback, 700, 500, 775),
                new TargetSpellData("quinn", "quinnr", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("quinn", "quinnrfinale", SpellSlot.R, SpellType.Self, CcType.No, 700, 0, 0),

                #endregion Quinn

                #region Rammus
                new TargetSpellData("rammus", "powerball", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, 775),
                new TargetSpellData("rammus", "defensiveballcurl", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("rammus", "puncturingtaunt", SpellSlot.E, SpellType.Targeted, CcType.Taunt, 325, 500,
                    float.MaxValue),
                new TargetSpellData("rammus", "tremors2", SpellSlot.R, SpellType.Self, CcType.No, 300, 500,
                    float.MaxValue),

                #endregion Rammus

                #region Renekton
                new TargetSpellData("renekton", "renektoncleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1, 500,
                    float.MaxValue),
                new TargetSpellData("renekton", "renektonpreexecute", SpellSlot.W, SpellType.Self, CcType.Stun, 0, 500,
                    float.MaxValue),
                new TargetSpellData("renekton", "renektonsliceanddice", SpellSlot.E, SpellType.Skillshot, CcType.No, 450,
                    500, 1400),
                new TargetSpellData("renekton", "renektonreignofthetyrant", SpellSlot.R, SpellType.Skillshot, CcType.No,
                    1, 500, 775),

                #endregion Renekton

                #region Rengar
                new TargetSpellData("rengar", "rengarq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("rengar", "rengarw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1, 500,
                    float.MaxValue),
                new TargetSpellData("rengar", "rengare", SpellSlot.E, SpellType.Targeted, CcType.Snare, 1000, 500, 1500),
                new TargetSpellData("rengar", "rengarr", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Rengar

                #region Riven
                new TargetSpellData("riven", "riventricleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 250, 500, 0),
                new TargetSpellData("riven", "riventricleave_03", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 250,
                    500, 0),
                new TargetSpellData("riven", "rivenmartyr", SpellSlot.W, SpellType.Self, CcType.Stun, 260, 250, 1500),
                new TargetSpellData("riven", "rivenfeint", SpellSlot.E, SpellType.Skillshot, CcType.No, 325, 0, 1450),
                new TargetSpellData("riven", "rivenfengshuiengine", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, 1200),
                new TargetSpellData("riven", "rivenizunablade", SpellSlot.R, SpellType.Skillshot, CcType.No, 900, 300,
                    1450),

                #endregion Riven

                #region Rumble
                new TargetSpellData("rumble", "rumbleflamethrower", SpellSlot.Q, SpellType.Skillshot, CcType.No, 600,
                    500, float.MaxValue),
                new TargetSpellData("rumble", "rumbleshield", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("rumble", "rumbegrenade", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500,
                    1200),
                new TargetSpellData("rumble", "rumblecarpetbomb", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 1700,
                    500, 1400),

                #endregion Rumble

                #region Ryze
                new TargetSpellData("ryze", "overload", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("ryze", "runeprison", SpellSlot.W, SpellType.Targeted, CcType.Snare, 600, 500,
                    float.MaxValue),
                new TargetSpellData("ryze", "spellflux", SpellSlot.E, SpellType.Targeted, CcType.No, 600, 500, 1000),
                new TargetSpellData("ryze", "desperatepower", SpellSlot.R, SpellType.Targeted, CcType.No, 625, 500, 1400),

                #endregion Ryze

                #region Sejuani
                new TargetSpellData("sejuani", "sejuaniarcticassault", SpellSlot.Q, SpellType.Skillshot,
                    CcType.Knockback, 650, 500, 1450),
                new TargetSpellData("sejuani", "sejuaninorthernwinds", SpellSlot.W, SpellType.Skillshot, CcType.No, 1,
                    500, 1500),
                new TargetSpellData("sejuani", "sejuaniwintersclaw", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1,
                    500, 1450),
                new TargetSpellData("sejuani", "sejuaniglacialprisonstart", SpellSlot.R, SpellType.Skillshot,
                    CcType.Stun, 1175, 500, 1400),

                #endregion Sejuani

                #region Shaco
                new TargetSpellData("shaco", "deceive", SpellSlot.Q, SpellType.Skillshot, CcType.No, 400, 500,
                    float.MaxValue),
                new TargetSpellData("shaco", "jackinthebox", SpellSlot.W, SpellType.Skillshot, CcType.Fear, 425, 500,
                    1450),
                new TargetSpellData("shaco", "twoshivpoisen", SpellSlot.E, SpellType.Targeted, CcType.Slow, 625, 500,
                    1500),
                new TargetSpellData("shaco", "hallucinatefull", SpellSlot.R, SpellType.Skillshot, CcType.No, 1125, 500,
                    395),

                #endregion Shaco

                #region Shen
                new TargetSpellData("shen", "shenvorpalstar", SpellSlot.Q, SpellType.Targeted, CcType.No, 475, 500, 1500),
                new TargetSpellData("shen", "shenfeint", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shen", "shenshadowdash", SpellSlot.E, SpellType.Skillshot, CcType.Taunt, 600, 500,
                    1000),
                new TargetSpellData("shen", "shenstandunited", SpellSlot.R, SpellType.Targeted, CcType.No, 25000, 500,
                    float.MaxValue),

                #endregion Shen

                #region Shyvana
                new TargetSpellData("shyvana", "shyvanadoubleattack", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("shyvana", "shyvanadoubleattackdragon", SpellSlot.Q, SpellType.Self, CcType.No, 0,
                    500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolationauraqw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolateddragon", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("shyvana", "shyvanafireball", SpellSlot.E, SpellType.Skillshot, CcType.No, 925, 500,
                    1200),
                new TargetSpellData("shyvana", "shyvanafireballdragon2", SpellSlot.E, SpellType.Skillshot, CcType.No,
                    925, 500, 1200),
                new TargetSpellData("shyvana", "shyvanatransformcast", SpellSlot.R, SpellType.Skillshot, CcType.No, 1000,
                    500, 700),
                new TargetSpellData("shyvana", "shyvanatransformleap", SpellSlot.R, SpellType.Skillshot,
                    CcType.Knockback, 1000, 500, 700),

                #endregion Shyvana

                #region Singed
                new TargetSpellData("singed", "poisentrail", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("singed", "megaadhesive", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1175, 500,
                    700),
                new TargetSpellData("singed", "fling", SpellSlot.E, SpellType.Targeted, CcType.Pull, 125, 500,
                    float.MaxValue),
                new TargetSpellData("singed", "insanitypotion", SpellSlot.R, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),

                #endregion Singed

                #region Sion
                new TargetSpellData("sion", "crypticgaze", SpellSlot.Q, SpellType.Targeted, CcType.Stun, 550, 500, 1600),
                new TargetSpellData("sion", "deathscaressfull", SpellSlot.W, SpellType.Self, CcType.No, 550, 500,
                    float.MaxValue),
                new TargetSpellData("sion", "deathscaress", SpellSlot.W, SpellType.Self, CcType.No, 550, 500,
                    float.MaxValue),
                new TargetSpellData("sion", "enrage", SpellSlot.E, SpellType.Self, CcType.Slow, 0, 500, float.MaxValue),
                new TargetSpellData("sion", "cannibalism", SpellSlot.R, SpellType.Self, CcType.Stun, 0, 500, 500),

                #endregion Sion

                #region Sivir
                new TargetSpellData("sivir", "sivirq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1165, 500, 1350),
                new TargetSpellData("sivir", "sivirw", SpellSlot.W, SpellType.Targeted, CcType.No, 565, 500,
                    float.MaxValue),
                new TargetSpellData("sivir", "sivire", SpellSlot.E, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("sivir", "sivirr", SpellSlot.R, SpellType.Self, CcType.No, 1000, 500, float.MaxValue),

                #endregion Sivir

                #region Skarner
                new TargetSpellData("skarner", "skarnervirulentslash", SpellSlot.Q, SpellType.Self, CcType.No, 350, 0,
                    float.MaxValue),
                new TargetSpellData("skarner", "skarnerexoskeleton", SpellSlot.W, SpellType.Self, CcType.No, 0, 0,
                    float.MaxValue),
                new TargetSpellData("skarner", "skarnerfracture", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000,
                    500, 1200),
                new TargetSpellData("skarner", "skarnerfracturemissilespell", SpellSlot.E, SpellType.Skillshot,
                    CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("skarner", "skarnerimpale", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 350,
                    0, float.MaxValue),

                #endregion Skarner

                #region Sona
                new TargetSpellData("sona", "sonahymnofvalor", SpellSlot.Q, SpellType.Self, CcType.No, 700, 500, 1500),
                new TargetSpellData("sona", "sonaariaofperseverance", SpellSlot.W, SpellType.Self, CcType.No, 1000, 500,
                    1500),
                new TargetSpellData("sona", "sonasongofdiscord", SpellSlot.E, SpellType.Self, CcType.No, 1000, 500, 1500),
                new TargetSpellData("sona", "sonacrescendo", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 900, 500,
                    2400),

                #endregion Sona

                #region Soraka
                new TargetSpellData("soraka", "starcall", SpellSlot.Q, SpellType.Self, CcType.No, 675, 500,
                    float.MaxValue),
                new TargetSpellData("soraka", "astralblessing", SpellSlot.W, SpellType.Targeted, CcType.No, 750, 500,
                    float.MaxValue),
                new TargetSpellData("soraka", "infusewrapper", SpellSlot.E, SpellType.Targeted, CcType.No, 725, 500,
                    float.MaxValue),
                new TargetSpellData("soraka", "wish", SpellSlot.R, SpellType.Self, CcType.No, 25000, 500, float.MaxValue),

                #endregion Soraka

                #region Swain
                new TargetSpellData("swain", "swaindecrepify", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 625, 500,
                    float.MaxValue),
                new TargetSpellData("swain", "swainshadowgrasp", SpellSlot.W, SpellType.Skillshot, CcType.Snare, 1040,
                    500, 1250),
                new TargetSpellData("swain", "swaintorment", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("swain", "swainmetamorphism", SpellSlot.R, SpellType.Self, CcType.No, 700, 500, 950),

                #endregion Swain

                #region Syndra
                new TargetSpellData("syndra", "syndraq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 250, 1750),
                new TargetSpellData("syndra", "syndraw", SpellSlot.W, SpellType.Targeted, CcType.No, 925, 500, 1450),
                new TargetSpellData("syndra", "syndrawcast", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 950, 500,
                    1450),
                new TargetSpellData("syndra", "syndrae", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 700, 500, 902),
                new TargetSpellData("syndra", "syndrar", SpellSlot.R, SpellType.Targeted, CcType.No, 675, 500, 1100),

                #endregion Syndra

                #region Talon
                new TargetSpellData("talon", "talonnoxiandiplomacy", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("talon", "talonrake", SpellSlot.W, SpellType.Skillshot, CcType.No, 750, 500, 1200),
                new TargetSpellData("talon", "taloncutthroat", SpellSlot.E, SpellType.Targeted, CcType.Slow, 750, 0,
                    1200),
                new TargetSpellData("talon", "talonshadowassault", SpellSlot.R, SpellType.Self, CcType.No, 750, 0, 0),

                #endregion Talon

                #region Taric
                new TargetSpellData("taric", "imbue", SpellSlot.Q, SpellType.Targeted, CcType.No, 750, 500, 1200),
                new TargetSpellData("taric", "shatter", SpellSlot.W, SpellType.Self, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("taric", "dazzle", SpellSlot.E, SpellType.Targeted, CcType.Stun, 625, 500, 1400),
                new TargetSpellData("taric", "tarichammersmash", SpellSlot.R, SpellType.Self, CcType.No, 400, 500,
                    float.MaxValue),

                #endregion Taric

                #region Teemo
                new TargetSpellData("teemo", "blindingdart", SpellSlot.Q, SpellType.Targeted, CcType.Blind, 580, 500,
                    1500),
                new TargetSpellData("teemo", "movequick", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 943),
                new TargetSpellData("teemo", "toxicshot", SpellSlot.E, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("teemo", "bantamtrap", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 230, 0, 1500),

                #endregion Teemo

                #region Thresh
                new TargetSpellData("thresh", "threshq", SpellSlot.Q, SpellType.Skillshot, CcType.Pull, 1175, 500, 1200),
                new TargetSpellData("thresh", "threshw", SpellSlot.W, SpellType.Skillshot, CcType.No, 950, 500,
                    float.MaxValue),
                new TargetSpellData("thresh", "threshe", SpellSlot.E, SpellType.Skillshot, CcType.Knockback, 515, 300,
                    float.MaxValue),
                new TargetSpellData("thresh", "threshrpenta", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 420, 300,
                    float.MaxValue),

                #endregion Thresh

                #region Tristana
                new TargetSpellData("tristana", "rapidfire", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("tristana", "rocketjump", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 900, 500,
                    1150),
                new TargetSpellData("tristana", "detonatingshot", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 500,
                    1400),
                new TargetSpellData("tristana", "bustershot", SpellSlot.R, SpellType.Targeted, CcType.Knockback, 700,
                    500, 1600),

                #endregion Tristana

                #region Trundle
                new TargetSpellData("trundle", "trundletrollsmash", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 0, 500,
                    float.MaxValue),
                new TargetSpellData("trundle", "trundledesecrate", SpellSlot.W, SpellType.Skillshot, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("trundle", "trundlecircle", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100, 500,
                    1600),
                new TargetSpellData("trundle", "trundlepain", SpellSlot.R, SpellType.Targeted, CcType.No, 700, 500, 1400),

                #endregion Trundle

                #region Tryndamere
                new TargetSpellData("tryndamere", "bloodlust", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("tryndamere", "mockingshout", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 400,
                    500, 500),
                new TargetSpellData("tryndamere", "slashcast", SpellSlot.E, SpellType.Skillshot, CcType.No, 660, 500,
                    700),
                new TargetSpellData("tryndamere", "undyingrage", SpellSlot.R, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),

                #endregion Tryndamere

                #region Twich
                new TargetSpellData("twich", "hideinshadows", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("twich", "twitchvenomcask", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 800, 500,
                    1750),
                new TargetSpellData("twich", "twitchvenomcaskmissle", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 800,
                    500, 1750),
                new TargetSpellData("twich", "expunge", SpellSlot.E, SpellType.Targeted, CcType.No, 1200, 500,
                    float.MaxValue),
                new TargetSpellData("twich", "fullautomatic", SpellSlot.R, SpellType.Targeted, CcType.No, 850, 500, 500),

                #endregion Twich

                #region TwistedFate
                new TargetSpellData("twistedfate", "wildcards", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1450, 500,
                    1450),
                new TargetSpellData("twistedfate", "pickacard", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("twistedfate", "goldcardpreattack", SpellSlot.W, SpellType.Targeted, CcType.Stun,
                    600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "redcardpreattack", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600,
                    500, float.MaxValue),
                new TargetSpellData("twistedfate", "bluecardpreattack", SpellSlot.W, SpellType.Targeted, CcType.No, 600,
                    500, float.MaxValue),
                new TargetSpellData("twistedfate", "cardmasterstack", SpellSlot.E, SpellType.Self, CcType.No, 525, 500,
                    1200),
                new TargetSpellData("twistedfate", "destiny", SpellSlot.R, SpellType.Skillshot, CcType.No, 5500, 500,
                    float.MaxValue),

                #endregion TwistedFate

                #region Udyr
                new TargetSpellData("udyr", "udyrtigerstance", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("udyr", "udyrturtlestance", SpellSlot.W, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("udyr", "udyrbearstance", SpellSlot.E, SpellType.Self, CcType.Stun, 0, 500,
                    float.MaxValue),
                new TargetSpellData("udyr", "udyrphoenixstance", SpellSlot.R, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),

                #endregion Udyr

                #region Urgot
                new TargetSpellData("urgot", "urgotheatseekinglineqqmissile", SpellSlot.Q, SpellType.Skillshot,
                    CcType.No, 1000, 500, 1600),
                new TargetSpellData("urgot", "urgotheatseekingmissile", SpellSlot.Q, SpellType.Skillshot, CcType.No,
                    1000, 500, 1600),
                new TargetSpellData("urgot", "urgotterrorcapacitoractive2", SpellSlot.W, SpellType.Self, CcType.No, 0,
                    500, float.MaxValue),
                new TargetSpellData("urgot", "urgotplasmagrenade", SpellSlot.E, SpellType.Skillshot, CcType.No, 950, 500,
                    1750),
                new TargetSpellData("urgot", "urgotplasmagrenadeboom", SpellSlot.E, SpellType.Skillshot, CcType.No, 950,
                    500, 1750),
                new TargetSpellData("urgot", "urgotswap2", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 850, 500,
                    1800),

                #endregion Urgot

                #region Varus
                new TargetSpellData("varus", "varusq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1500, 500, 1500),
                new TargetSpellData("varus", "varusw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("varus", "varuse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 925, 500, 1500),
                new TargetSpellData("varus", "varusr", SpellSlot.R, SpellType.Skillshot, CcType.Snare, 1300, 500, 1500),

                #endregion Varus

                #region Vayne
                new TargetSpellData("vayne", "vaynetumble", SpellSlot.Q, SpellType.Skillshot, CcType.No, 250, 500,
                    float.MaxValue),
                new TargetSpellData("vayne", "vaynesilverbolts", SpellSlot.W, SpellType.Self, CcType.No, 0, 0,
                    float.MaxValue),
                new TargetSpellData("vayne", "vaynecondemm", SpellSlot.E, SpellType.Targeted, CcType.Stun, 450, 500,
                    1200),
                new TargetSpellData("vayne", "vayneinquisition", SpellSlot.R, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),

                #endregion Vayne

                #region Veigar
                new TargetSpellData("veigar", "veigarbalefulstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 650,
                    500, 1500),
                new TargetSpellData("veigar", "veigardarkmatter", SpellSlot.W, SpellType.Skillshot, CcType.No, 900, 1200,
                    1500),
                new TargetSpellData("veigar", "veigareventhorizon", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 650,
                    float.MaxValue, 1500),
                new TargetSpellData("veigar", "veigarprimordialburst", SpellSlot.R, SpellType.Targeted, CcType.No, 650,
                    500, 1400),

                #endregion Veigar

                #region Velkoz
                new TargetSpellData("velkoz", "velkozq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050, 300, 1200),
                new TargetSpellData("velkoz", "velkozqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050, 0,
                    1200),
                new TargetSpellData("velkoz", "velkozqplitactive", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050,
                    800, 1200),
                new TargetSpellData("velkoz", "velkozw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050, 0, 1200),
                new TargetSpellData("velkoz", "velkoze", SpellSlot.E, SpellType.Targeted, CcType.Knockup, 850, 0, 500),
                new TargetSpellData("velkoz", "velkozr", SpellSlot.R, SpellType.Skillshot, CcType.No, 1575, 0, 1500),

                #endregion Velkoz

                #region Vi
                new TargetSpellData("vi", "viq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 500, 1500),
                new TargetSpellData("vi", "viw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("vi", "vie", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 0, 0),
                new TargetSpellData("vi", "vir", SpellSlot.R, SpellType.Targeted, CcType.Stun, 800, 500, 0),

                #endregion Vi

                #region Viktor
                new TargetSpellData("viktor", "viktorpowertransfer", SpellSlot.Q, SpellType.Targeted, CcType.No, 600,
                    500, 1400),
                new TargetSpellData("viktor", "viktorgravitonfield", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 815,
                    500, 1750),
                new TargetSpellData("viktor", "viktordeathray", SpellSlot.E, SpellType.Skillshot, CcType.No, 700, 500,
                    1210),
                new TargetSpellData("viktor", "viktorchaosstorm", SpellSlot.R, SpellType.Skillshot, CcType.Silence, 700,
                    500, 1210),

                #endregion Viktor

                #region Vladimir
                new TargetSpellData("vladimir", "vladimirtransfusion", SpellSlot.Q, SpellType.Targeted, CcType.No, 600,
                    500, 1400),
                new TargetSpellData("vladimir", "vladimirsanguinepool", SpellSlot.W, SpellType.Self, CcType.Slow, 350,
                    500, 1600),
                new TargetSpellData("vladimir", "vladimirtidesofblood", SpellSlot.E, SpellType.Self, CcType.No, 610, 500,
                    1100),
                new TargetSpellData("vladimir", "vladimirhemoplague", SpellSlot.R, SpellType.Skillshot, CcType.No, 875,
                    500, 1200),

                #endregion Vladimir

                #region Volibear
                new TargetSpellData("volibear", "volibearq", SpellSlot.Q, SpellType.Self, CcType.No, 300, 500,
                    float.MaxValue),
                new TargetSpellData("volibear", "volibearw", SpellSlot.W, SpellType.Targeted, CcType.No, 400, 500, 1450),
                new TargetSpellData("volibear", "volibeare", SpellSlot.E, SpellType.Self, CcType.Slow, 425, 500, 825),
                new TargetSpellData("volibear", "volibearr", SpellSlot.R, SpellType.Self, CcType.No, 425, 0, 825),

                #endregion Volibear

                #region Warwick
                new TargetSpellData("warwick", "hungeringstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 400, 0,
                    float.MaxValue),
                new TargetSpellData("warwick", "hunterscall", SpellSlot.W, SpellType.Self, CcType.No, 1000, 0,
                    float.MaxValue),
                new TargetSpellData("warwick", "bloodscent", SpellSlot.E, SpellType.Self, CcType.No, 1500, 0,
                    float.MaxValue),
                new TargetSpellData("warwick", "infiniteduress", SpellSlot.R, SpellType.Targeted, CcType.Suppression,
                    700, 500, float.MaxValue),

                #endregion Warwick

                #region Xerath
                new TargetSpellData("xerath", "xeratharcanopulsechargeup", SpellSlot.Q, SpellType.Skillshot, CcType.No,
                    750, 750, 500),
                new TargetSpellData("xerath", "xeratharcanebarrage2", SpellSlot.W, SpellType.Skillshot, CcType.Slow,
                    1100, 500, 20),
                new TargetSpellData("xerath", "xerathmagespear", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1050,
                    500, 1600),
                new TargetSpellData("xerath", "xerathlocusofpower2", SpellSlot.R, SpellType.Skillshot, CcType.No, 5600,
                    750, 500),

                #endregion Xerath

                #region Xin Zhao
                new TargetSpellData("xin zhao", "xenzhaocombotarget", SpellSlot.Q, SpellType.Self, CcType.No, 200, 0,
                    2000),
                new TargetSpellData("xin zhao", "xenzhaobattlecry", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 2000),
                new TargetSpellData("xin zhao", "xenzhaosweep", SpellSlot.E, SpellType.Targeted, CcType.Slow, 600, 500,
                    1750),
                new TargetSpellData("xin zhao", "xenzhaoparry", SpellSlot.R, SpellType.Self, CcType.Knockback, 375, 0,
                    1750),

                #endregion Xin Zhao

                #region Yasuo
                new TargetSpellData("yasuo", "yasuoqw", SpellSlot.Q, SpellType.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq2w", SpellSlot.Q, SpellType.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq3w", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1000, 750,
                    1500),
                new TargetSpellData("yasuo", "yasuowmovingwall", SpellSlot.W, SpellType.Skillshot, CcType.No, 400, 500,
                    500),
                new TargetSpellData("yasuo", "yasuodashwrapper", SpellSlot.E, SpellType.Targeted, CcType.No, 475, 500,
                    20),
                new TargetSpellData("yasuo", "yasuorknockupcombow", SpellSlot.R, SpellType.Self, CcType.No, 1200, 500,
                    20),

                #endregion Yasuo

                #region Yorick
                new TargetSpellData("yorick", "yorickspectral", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500,
                    float.MaxValue),
                new TargetSpellData("yorick", "yorickdecayed", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 600, 500,
                    float.MaxValue),
                new TargetSpellData("yorick", "yorickravenous", SpellSlot.E, SpellType.Targeted, CcType.Slow, 550, 500,
                    float.MaxValue),
                new TargetSpellData("yorick", "yorickreviveally", SpellSlot.R, SpellType.Targeted, CcType.No, 900, 500,
                    1500),

                #endregion Yorick

                #region Zac
                new TargetSpellData("zac", "zacq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 550, 500, 902),
                new TargetSpellData("zac", "zacw", SpellSlot.W, SpellType.Self, CcType.No, 350, 500, 1600),
                new TargetSpellData("zac", "zace", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1550, 500, 1500),
                new TargetSpellData("zac", "zacr", SpellSlot.R, SpellType.Self, CcType.Knockback, 850, 500, 1800),

                #endregion Zac

                #region Zed
                new TargetSpellData("zed", "zedshuriken", SpellSlot.Q, SpellType.Skillshot, CcType.No, 900, 500, 902),
                new TargetSpellData("zed", "zedshdaowdash", SpellSlot.W, SpellType.Skillshot, CcType.No, 550, 500, 1600),
                new TargetSpellData("zed", "zedpbaoedummy", SpellSlot.E, SpellType.Self, CcType.Slow, 300, 0, 0),
                new TargetSpellData("zed", "zedult", SpellSlot.R, SpellType.Targeted, CcType.No, 850, 500, 0),

                #endregion Zed

                #region Ziggs
                new TargetSpellData("ziggs", "ziggsq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsqspell", SpellSlot.Q, SpellType.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsw", SpellSlot.W, SpellType.Skillshot, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggswtoggle", SpellSlot.W, SpellType.Self, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse2", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsr", SpellSlot.R, SpellType.Skillshot, CcType.No, 850, 500, 1750),

                #endregion Ziggs

                #region Zilean
                new TargetSpellData("zilean", "timebomb", SpellSlot.Q, SpellType.Targeted, CcType.No, 700, 0, 1100),
                new TargetSpellData("zilean", "rewind", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("zilean", "timewarp", SpellSlot.E, SpellType.Targeted, CcType.Slow, 700, 500, 1100),
                new TargetSpellData("zilean", "chronoshift", SpellSlot.R, SpellType.Targeted, CcType.No, 780, 500,
                    float.MaxValue),

                #endregion Zilean

                #region Zyra
                new TargetSpellData("zyra", "zyraqfissure", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 500, 1400),
                new TargetSpellData("zyra", "zyraseed", SpellSlot.W, SpellType.Skillshot, CcType.No, 800, 500, 2200),
                new TargetSpellData("zyra", "zyragraspingroots", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 1100,
                    500, 1400),
                new TargetSpellData("zyra", "zyrabramblezone", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 700,
                    500, 20)

                #endregion Zyra
            };
        }
    }

    public class TargetSpellData
    {
        public TargetSpellData(string champion, string name, SpellSlot slot, SpellType type, CcType cc, float range,
            float delay, float speed)
        {
            ChampionName = champion;
            Name = name;
            Spellslot = slot;
            Type = type;
            CcType = cc;
            Range = range;
            Speed = speed;
            Delay = delay;
        }

        public string ChampionName { get; set; }
        public SpellSlot Spellslot { get; set; }
        public SpellType Type { get; set; }
        public CcType CcType { get; set; }
        public string Name { get; set; }
        public float Range { get; set; }
        public double Delay { get; set; }
        public double Speed { get; set; }
    }

    public enum SpellType
    {
        Skillshot,
        Targeted,
        Self,
        AutoAttack
    }


    public enum CcType
    {
        No,
        Stun,
        Silence,
        Taunt,
        Polymorph,
        Slow,
        Snare,
        Fear,
        Charm,
        Suppression,
        Blind,
        Flee,
        Knockup,
        Knockback,
        Pull
    }

    #endregion
}