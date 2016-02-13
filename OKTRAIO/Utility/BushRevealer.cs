using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility
{
    public class BushRevealer : UtilityAddon
    {
        private int lastWarded;

        public ItemId[] WardIds =
        {
            UtilityManager.Activator.WardingTotem.Id,
            UtilityManager.Activator.GreaterStealthTotem.Id,
            UtilityManager.Activator.GreaterVisionTotem.Id,
            UtilityManager.Activator.PinkVision.Id,
            UtilityManager.Activator.FarsightAlteration.Id
        };

        public InventorySlot GetWardSlot()
        {
            return
                WardIds.Select(wardId => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == wardId))
                    .FirstOrDefault(slot => slot != null && slot.CanUseItem());
        }

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "BushRevealer", "bushrevealer", "Unknown");
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - Bush Revealer for " + Player.Instance.ChampionName,
                "bushreveal.grouplabel.utilitymenu");
            Menu.AddCheckBox("bushreveal.use", "Use Bush Revealer");
            Menu.AddCheckBox("bushreveal.humanize", "Humanize the Bush Reveal");
        }

        public override void Initialize()
        {
            
        }

        protected override void Game_OnUpdate(EventArgs args)
        {
            if (Value.Use("bushreveal.use"))
            {
                var rnd = new Random();
                var random = Value.Use("bushreveal.humanize") ? rnd.Next(200, 500) : 0;
                if (Value.Mode(Orbwalker.ActiveModes.Combo))
                {
                    foreach (
                        var heros in
                            EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1000))
                    {
                        var path = heros.Path.LastOrDefault();

                        if (NavMesh.IsWallOfGrass(path, 1))
                        {
                            if (NavMesh.IsWallOfGrass(Player.Instance.Position, 1) &&
                                Player.Instance.Distance(path) < 200 || heros.Distance(path) > 200)
                                return;

                            if (Player.Instance.Distance(path) < 500)
                            {
                                foreach (
                                    var obj in
                                        ObjectManager.Get<AIHeroClient>()
                                            .Where(
                                                x =>
                                                    x.Name.ToLower().Contains("ward") && x.IsAlly &&
                                                    x.Distance(path) < 300))
                                {
                                    if (NavMesh.IsWallOfGrass(obj.Position, 1)) return;
                                }

                                var wardslot = GetWardSlot();
                                if (wardslot != null && Environment.TickCount - lastWarded > 1000)
                                {
                                    Core.DelayAction(() => wardslot.Cast(path), random);
                                    lastWarded = Environment.TickCount;
                                }
                            }
                        }
                    }
                }
            }
        }

        public BushRevealer(Menu menu) : base(menu, null)
        {
        }
    }
}