using System.Collections.Generic;
using EloBuddy;

namespace OKTRAIO.Database.Spell_Library
{
    public class SpellDamage
    {
        public static List<SpellDb> Spells = new List<SpellDb>();

        private static readonly AIHeroClient Player = EloBuddy.Player.Instance;

        private static readonly float AD = Player.TotalAttackDamage;
        private static readonly float BonusAD = AD - Player.FlatPhysicalDamageMod;
        private static readonly float AP = Player.TotalMagicalDamage;


        internal SpellDamage()
        {
            #region Ashe

            Spells.Add(
                new SpellDb
                {
                    charName = "Ashe",
                    spellKey = "Q",
                    damage = AD*(23 + (GetSpellLevel(SpellSlot.Q) - 1))/100
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Ashe",
                    spellKey = "W",
                    damage =
                        20 + (GetSpellLevel(SpellSlot.W) - 1)*15 +
                        AD
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Ashe",
                    spellKey = "R",
                    damage =
                        250 + (GetSpellLevel(SpellSlot.R) - 1)*175 +
                        AP
                });

            #endregion

            #region Caitlyn

            Spells.Add(
                new SpellDb
                {
                    charName = "Caitlyn",
                    spellKey = "Q",
                    damage =
                        25 + (GetSpellLevel(SpellSlot.Q) - 1)*75 +
                        AD*
                        (130 + (GetSpellLevel(SpellSlot.Q) - 1)*10)/100
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Caitlyn",
                    spellKey = "E",
                    damage =
                        25 + (GetSpellLevel(SpellSlot.E) - 1)*40 +
                        AP*80/100
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Caitlyn",
                    spellKey = "R",
                    damage = 250 + 225*(GetSpellLevel(SpellSlot.R) - 1) + AD*2
                });

            #endregion

            #region Corki

            Spells.Add(
                new SpellDb
                {
                    charName = "Corki",
                    spellKey = "Q",
                    damage = 80 + 50*(GetSpellLevel(SpellSlot.Q) - 1) + AD*0.5f + AP*0.5f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Corki",
                    spellKey = "W",
                    damage = 30 + 15*(GetSpellLevel(SpellSlot.W) - 1) + AP*0.2f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Corki",
                    spellKey = "W2",
                    damage = 0 //TODO: W2 Special Delivery
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Corki",
                    spellKey = "E",
                    damage = 10 + 6*(GetSpellLevel(SpellSlot.E) - 1) + (AD - Player.BaseAttackDamage)*0.2f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Corki",
                    spellKey = "R",
                    damage =
                        100 + 30*(GetSpellLevel(SpellSlot.R) - 1) + AD*((GetSpellLevel(SpellSlot.R) - 1)*30)/100 +
                        AP*0.3f
                });

            #endregion

            #region Draven

            Spells.Add(
                new SpellDb
                {
                    charName = "Draven",
                    spellKey = "E",
                    damage = 70 + 35*(GetSpellLevel(SpellSlot.E) - 1) + BonusAD*0.4f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Draven",
                    spellKey = "R",
                    damage = 175 + 100*(GetSpellLevel(SpellSlot.R) - 1) + BonusAD*1.1f
                });

            #endregion

            #region Ezreal

            Spells.Add(
                new SpellDb
                {
                    charName = "Ezreal",
                    spellKey = "Q",
                    damage = 35 + 20*(GetSpellLevel(SpellSlot.Q) - 1) + AD*1.1f + AP*0.4f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Ezreal",
                    spellKey = "W",
                    damage = 70 + 45*(GetSpellLevel(SpellSlot.W) - 1) + AP*0.8f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Ezreal",
                    spellKey = "E",
                    damage = 0
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Ezreal",
                    spellKey = "R",
                    damage = 350 + 150*(GetSpellLevel(SpellSlot.R) - 1) + AD
                });

            #endregion

            #region Graves

            Spells.Add(
                new SpellDb
                {
                    charName = "Graves",
                    spellKey = "Q",
                    damage =
                        55 + 15*(GetSpellLevel(SpellSlot.Q) - 1) + BonusAD*0.75f +
                        (85 + 60*(GetSpellLevel(SpellSlot.Q) - 1) +
                         BonusAD*(40 + 20*(GetSpellLevel(SpellSlot.Q) - 1))/100)
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Graves",
                    spellKey = "W",
                    damage = 60 + 50*(GetSpellLevel(SpellSlot.W) - 1) + AP*0.6f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Graves",
                    spellKey = "R",
                    damage = 250 + 150*(GetSpellLevel(SpellSlot.R) - 1) + BonusAD*1.5f
                });

            #endregion

            #region Jinx

            Spells.Add(
                new SpellDb
                {
                    charName = "Jinx",
                    spellKey = "W",
                    damage = 10 + 50*(GetSpellLevel(SpellSlot.W) - 1) + AD*1.4f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Jinx",
                    spellKey = "E",
                    damage = 80 + 55*(GetSpellLevel(SpellSlot.E) - 1) + AP
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Jinx",
                    spellKey = "R",
                    damage = 0
                });

            #endregion

            #region Kalista

            Spells.Add(
                new SpellDb
                {
                    charName = "Kalista",
                    spellKey = "Q",
                    damage = new float[] {10, 70, 130, 190, 250}[GetSpellLevel(SpellSlot.Q)] + AD
                });

            #endregion

            #region Kindred

            Spells.Add(
                new SpellDb
                {
                    charName = "Kindred",
                    spellKey = "Q",
                    damage = new float[] {60, 90, 120, 150, 180}[GetSpellLevel(SpellSlot.Q)] + AD*0.2f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Kindred",
                    spellKey = "E",
                    damage = new float[] {80, 110, 140, 170, 200}[GetSpellLevel(SpellSlot.Q)] + AD*0.2f
                });

            #endregion

            #region Kog'Maw

            Spells.Add(
                new SpellDb
                {
                    charName = "Kog'Maw",
                    spellKey = "Q",
                    damage = new float[] {80, 130, 180, 230, 280}[GetSpellLevel(SpellSlot.Q)] + AP*0.5f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Kog'Maw",
                    spellKey = "E",
                    damage = new float[] {60, 110, 160, 210, 260}[GetSpellLevel(SpellSlot.Q)] + AP*0.7f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Kog'Maw",
                    spellKey = "R",
                    damage = new float[] {70, 110, 150}[GetSpellLevel(SpellSlot.Q)] + AP*0.25f + AD*0.65f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Kog'Maw",
                    spellKey = "R2",
                    damage = new float[] {140, 220, 300}[GetSpellLevel(SpellSlot.Q)] + AP*0.5f + AD*1.3f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Kog'Maw",
                    spellKey = "R3",
                    damage = new float[] {210, 330, 450}[GetSpellLevel(SpellSlot.Q)] + AP*1f + AD*1.95f
                });

            #endregion

            #region Lucian

            Spells.Add(
                new SpellDb
                {
                    charName = "Lucian",
                    spellKey = "Passive",
                    damage = LucianPassive()
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Lucian",
                    spellKey = "Q",
                    damage =
                        80 + 30*(GetSpellLevel(SpellSlot.Q) - 1) +
                        BonusAD*(60 + 15*(GetSpellLevel(SpellSlot.Q) - 1))/100
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Lucian",
                    spellKey = "W",
                    damage = 80 + 30*(GetSpellLevel(SpellSlot.W) - 1) + AP*0.9f
                });
            Spells.Add(
                new SpellDb
                {
                    charName = "Lucian",
                    spellKey = "R",
                    damage =
                        (20 + 15*(GetSpellLevel(SpellSlot.R) - 1) + AD*0.2f + AP*0.1f)*20 +
                        5*(GetSpellLevel(SpellSlot.R) - 1)
                });

            #endregion

            #region MF

            Spells.Add(
                new SpellDb
                {
                    charName = "Miss Fortune",
                    spellKey = "E",
                    damage =
                        (new[] {(float) 11.25, (float) 18.125, 25, (float) 31.875, (float) 38.75}[
                            GetSpellLevel(SpellSlot.Q)] + AP*0.20f)*4
                });

            #endregion
        }


        private static int GetSpellLevel(SpellSlot slot)
        {
            return Player.Spellbook.GetSpell(slot).Level;
            ;
        }

        public static float LucianPassive()
        {
            if (Player.Level >= 1 && Player.Level < 6)
            {
                return AD*0.3f;
            }
            if (Player.Level >= 6 && Player.Level < 11)
            {
                return AD*0.4f;
            }
            if (Player.Level >= 11 && Player.Level < 16)
            {
                return AD*0.5f;
            }
            if (Player.Level >= 16)
            {
                return AD*0.6f;
            }
            return 0;
        }
    }
}