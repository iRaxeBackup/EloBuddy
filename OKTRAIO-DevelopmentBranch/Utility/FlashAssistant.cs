using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility
{
    public class FlashAssistant : UtilityAddon
    {
        public FlashAssistant(Menu menu) : base(menu)
        {
        }

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "Flash Assistant", "flashassistant", "iRaxe");
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - Flash Assistant for " + Player.Instance.ChampionName,
                "flashassistant.grouplabel.utilitymenu");

            if (UtilityManager.Activator.Flash != null)
            {
                Menu.AddLabel("Set your desired KeyBind for the Flash Assistant");
                Menu.Add("flashassistant.bind",
                    new KeyBind("Flash!", false, KeyBind.BindTypes.HoldActive, 'G'))
                    .OnValueChange += OnFlash;
            }
            else
            {
                Menu.AddLabel("Sorry, you havent Flash Spell - UTILITY DISABLED");
            }
        }

        public override void Initialize()
        {
            // ReSharper disable once RedundantJumpStatement
            if (UtilityManager.Activator.Flash == null) return;
        }

        private static void OnFlash(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (UtilityManager.Activator.Flash.IsReady() && args.NewValue)
            {
                var position = Player.Instance.ServerPosition.Extend(Game.CursorPos,
                    UtilityManager.Activator.Flash.Range);
                UtilityManager.Activator.Flash.Cast(position.To3DWorld());
            }
        }
    }
}