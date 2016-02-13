using EloBuddy;
using EloBuddy.SDK.Menu;
using OKTRAIO.Utility.SkinManager;

namespace OKTRAIO.Utility
{
    public static class UtilityManager
    {
        public static Menu UtilityMenu;
        public static Tracker.Tracker Tracker;
        public static Activator Activator;
        public static BaseUlt BaseUlt;
        public static RandomUlt RandomUlt;
        public static BushRevealer BushRevealer;
        public static SkinManagement SkinManagement;
        public static RecallTracker RecallTracker;
        public static FlashAssistant FlashAssistant;
        public static AutoLantern AutoLantern;
        public static void Initialize()
        {

            UtilityMenu = MainMenu.AddMenu("OKTR Utility", "marks.aio.utility.menu", Player.Instance.ChampionName);
            Activator = new Activator(UtilityMenu);
            Tracker = new Tracker.Tracker(UtilityMenu);

            BaseUlt = new BaseUlt(UtilityMenu, Player.Instance.Hero);
            RandomUlt = new RandomUlt(UtilityMenu, Player.Instance.Hero);

            BushRevealer = new BushRevealer(UtilityMenu);
            SkinManagement = new SkinManagement(UtilityMenu);
            RecallTracker = new RecallTracker(UtilityMenu);
            FlashAssistant = new FlashAssistant(UtilityMenu);
            AutoLantern = new AutoLantern(UtilityMenu);
           
        }

        
    }
}