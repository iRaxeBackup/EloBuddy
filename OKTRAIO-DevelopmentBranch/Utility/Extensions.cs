using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace OKTRAIO.Utility
{
    internal static class Extensions
    {
        public static bool CanUseItem(string name)
        {
            return
                ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name)
                    .Select(
                        slot =>
                            ObjectManager.Player.Spellbook.Spells.FirstOrDefault(
                                spell => (int) spell.Slot == slot.Slot + (int) SpellSlot.Item1))
                    .Select(inst => inst != null && inst.State == SpellState.Ready)
                    .FirstOrDefault();
        }

        public static bool CanUseItem(int id)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id))
            {
                var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell =>
                    (int) spell.Slot == slot.Slot + (int) SpellSlot.Item1);
                return inst != null && inst.State == SpellState.Ready;
            }

            return false;
        }

        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[]
            {2045, 2049, 2050, 2301, 2302, 2303, 3340, 3361, 3362, 3711, 1408, 1409, 1410, 1411, 2043};
            return (from wardId in wardIds
                where CanUseItem(wardId)
                select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId) wardId))
                .FirstOrDefault();
        }
    }

    /// <summary>
    ///     Represents a last casted spell.
    /// </summary>
    public class LastCastedSpellEntry
    {
        /// <summary>
        ///     The name
        /// </summary>
        public string Name;

        /// <summary>
        ///     The target
        /// </summary>
        public Obj_AI_Base Target;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastedSpellEntry" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="target">The target.</param>
        public LastCastedSpellEntry(string name, int tick, Obj_AI_Base target)
        {
            Name = name;
            Tick = tick;
            Target = target;
        }
    }

    /// <summary>
    ///     Represents the last cast packet sent.
    /// </summary>
    public class LastCastPacketSentEntry
    {
        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The target network identifier
        /// </summary>
        public int TargetNetworkId;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastPacketSentEntry" /> class.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="targetNetworkId">The target network identifier.</param>
        public LastCastPacketSentEntry(SpellSlot slot, int tick, int targetNetworkId)
        {
            Slot = slot;
            Tick = tick;
            TargetNetworkId = targetNetworkId;
        }
    }

    /// <summary>
    ///     Gets the last casted spell of the unit.
    /// </summary>
    public static class LastCastedSpell
    {
        /// <summary>
        ///     The casted spells
        /// </summary>
        internal static readonly Dictionary<int, LastCastedSpellEntry> CastedSpells =
            new Dictionary<int, LastCastedSpellEntry>();

        /// <summary>
        ///     The last cast packet sent
        /// </summary>
        public static LastCastPacketSentEntry LastCastPacketSent;

        /// <summary>
        ///     Initializes static members of the <see cref="LastCastedSpell" /> class.
        /// </summary>
        static LastCastedSpell()
        {
            Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Spellbook.OnCastSpell += SpellbookOnCastSpell;
        }

        /// <summary>
        ///     Fired then a spell is casted.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs" /> instance containing the event data.</param>
        private static void SpellbookOnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (spellbook.Owner.IsMe)
            {
                LastCastPacketSent = new LastCastPacketSentEntry(
                    args.Slot, Core.GameTickCount, args.Target is Obj_AI_Base ? args.Target.NetworkId : 0);
            }
        }

        /// <summary>
        ///     Fired when the game processes the spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is AIHeroClient)
            {
                var entry = new LastCastedSpellEntry(args.SData.Name, Core.GameTickCount, ObjectManager.Player);
                if (CastedSpells.ContainsKey(sender.NetworkId))
                {
                    CastedSpells[sender.NetworkId] = entry;
                }
                else
                {
                    CastedSpells.Add(sender.NetworkId, entry);
                }
            }
        }

        /// <summary>
        ///     Gets the last casted spell tick.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static int LastCastedSpellT(this AIHeroClient unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId)
                ? CastedSpells[unit.NetworkId].Tick
                : (Core.GameTickCount > 0 ? 0 : int.MinValue);
        }

        /// <summary>
        ///     Gets the last casted spell name.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static string LastCastedSpellName(this AIHeroClient unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Name : string.Empty;
        }

        /// <summary>
        ///     Gets the last casted spell's target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Obj_AI_Base LastCastedSpellTarget(this AIHeroClient unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Target : null;
        }

        /// <summary>
        ///     Gets the last casted spell.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static LastCastedSpellEntry LastCastedspell(this AIHeroClient unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId] : null;
        }
    }
}