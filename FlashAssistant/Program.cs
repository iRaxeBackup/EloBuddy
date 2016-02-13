using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Notifications;

namespace FlashAssistant
{
    internal class Program
    {
        public static Menu Menu;
        public static Spell.Targeted Flash { get; private set; }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Init;
        }

        private static void Init(EventArgs args)
        {

            var slot = Player.Instance.GetSpellSlotFromName("summonerflash");

            switch (slot)
            {
                case SpellSlot.Summoner1:
                case SpellSlot.Summoner2:
                    Flash = new Spell.Targeted(slot, 425);
                    break;
            }

            if (Flash == null) return;

            Menu = MainMenu.AddMenu("Flash Assistant", "flash.assistant", Player.Instance.ChampionName);

            Menu.AddGroupLabel("Flash Assistant for " + Player.Instance.ChampionName);
            Menu.AddLabel("Spaghetti code done in 10 minutes >_< by iRaxe");
            Menu.AddSeparator();

            Menu.AddLabel("Set your desired KeyBind for the Flash Assistant");

            Menu.Add("flash.bind",
                new KeyBind("Flash!", false, KeyBind.BindTypes.HoldActive, 'G'))
                .OnValueChange += OnFlash;

            Notifications.Show(new SimpleNotification("Flash Assistant",
                "Hey nUUbZ ! Im helping you now, blame iRaxe!"));
        }

        private static void OnFlash(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (Flash.IsReady() && args.NewValue)
            {
                Notifications.Show(new SimpleNotification("Flash Assistant", "NOOOOOOOOOOOOB!"));
                var position = Player.Instance.ServerPosition.Extend(Game.CursorPos, Flash.Range);
                Flash.Cast(position.To3DWorld());
            }
        }
    }
}
