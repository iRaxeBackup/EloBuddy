using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using SharpDX;

namespace OKTRAIO.Utility
{
    public static class Humanizer
    {
        public static List<IssueOrder> LastMove = new List<IssueOrder>();
        public static List<SpellCast> SpellCasts = new List<SpellCast>();

        public static void Init()
        {   if (Player.Instance.ChampionName == "Vayne" || Player.Instance.ChampionName == "Lucian") return;
            Player.OnIssueOrder += Player_OnIssueOrder;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        private static void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (
                LastMove.Any(
                    m =>
                        m.Order == args.Order && args.TargetPosition == m.Pos &&
                        DateTime.Now - m.Time < TimeSpan.FromSeconds(0.05)))
            {
                args.Process = false;
            }
            else if (LastMove.Any(m => m.Order == args.Order))
            {
                LastMove.Remove(LastMove.Find(m => m.Order == args.Order));
                LastMove.Add(new IssueOrder(DateTime.Now, args.TargetPosition, args.Order));
            }
            else
            {
                LastMove.Add(new IssueOrder(DateTime.Now, args.TargetPosition, args.Order));
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!sender.Owner.IsMe) return;
            if (!Player.Instance.Spellbook.GetSpell(args.Slot).IsReady) args.Process = false;
            if (SpellCasts.All(s => s.Slot != args.Slot))
            {
                SpellCasts.Add(new SpellCast(DateTime.Now, args.Slot));
            }
            else
            {
                var spellCast = SpellCasts.Find(s => s.Slot == args.Slot);
                if (DateTime.Now - spellCast.Time < TimeSpan.FromSeconds(0.1)) args.Process = false;
                SpellCasts.Remove(spellCast);
                SpellCasts.Add(new SpellCast(DateTime.Now, args.Slot));
            }
        }

        public struct SpellCast
        {
            internal DateTime Time;
            internal SpellSlot Slot;

            public SpellCast(DateTime time, SpellSlot slot)
            {
                Time = time;
                Slot = slot;
            }
        }

        public struct IssueOrder
        {
            internal GameObjectOrder Order;
            internal Vector3 Pos;
            internal DateTime Time;

            public IssueOrder(DateTime time, Vector3 pos, GameObjectOrder order)
            {
                Order = order;
                Time = time;
                Pos = pos;
            }
        }
    }
}