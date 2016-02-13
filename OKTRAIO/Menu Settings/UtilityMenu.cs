using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using MarksmanAIO.Menu_Settings;
using SharpDX;

namespace MarksmanAIO
{
    public static class UtilityMenu
    {
        public static Menu _menu, _activator, _baseult, _randomult;

        public static void Init()
        {
            try
            {
                _menu = EloBuddy.SDK.Menu.MainMenu.AddMenu("OKTR Utility", "marks.aio.utility.menu",
                    Player.Instance.ChampionName);
                _menu.AddGroupLabel("OKTR Utilities for " + Player.Instance.ChampionName);
                Activator.Init();
                BaseUltMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print(
                    "<font color='#23ADDB'>Marksman AIO:</font><font color='#E81A0C'> an error ocurred. (Code Utility Menu)</font>");
            }
        }

        public static void ActivatorMenu()
        {
            _activator = _menu.AddSubMenu("Activator", "activator");
            _activator.AddGroupLabel("OKTR AIO - Activator for " + Player.Instance.ChampionName,
                "activator.grouplabel.utilitymenu");
            _activator.AddCheckBox("activator.botrk", "Use BOTRK");
            _activator.AddCheckBox("activator.bilgewater", "Use BC");
            _activator.AddCheckBox("activator.youmus", "Use Youmus");
            _activator.AddCheckBox("activator.potions", "Use POTIONS");
            _activator.Add("activator.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange += Value.AdvancedModeChanged;
            _activator.AddLabel("Blade of The Ruined King Manager:", 25, "activator.label.utilitymenu.botrk", true);
            _activator.AddCheckBox("activator.botrk.combo", "Use BOTRK (COMBO Mode)", true, true);
            _activator.AddCheckBox("activator.botrk.ks", "Use BOTRK (KS Mode)", false, true);
            _activator.AddCheckBox("activator.botrk.lifesave", "Use BOTRK (LifeSave)", false, true);
            _activator.AddSlider("activator.botrk.hp", "Use BOTRK (LifeSave) if HP are under {0}", 20, 0, 100, true);
            _activator.AddSeparator();
            _activator.AddLabel("Bilgewater Cutlass Manager:", 25, "activator.label.utilitymenu.bilgewater", true);
            _activator.AddCheckBox("activator.bilgewater.combo", "Use BC (COMBO Mode)", true, true);
            _activator.AddCheckBox("activator.bilgewater.ks", "Use BC (KS Mode)", false, true);
            _activator.AddSeparator();
            _activator.AddLabel("Youmus Manager:", 25, "activator.label.utilitymenu.youmus", true);
            _activator.AddCheckBox("activator.youmusspellonly", "Use Youmus only on spell cast", false, true);
            _activator.AddSeparator();
            _activator.AddLabel("Potions Manager:", 25, "activator.label.utilitymenu.potions", true);
            _activator.AddSlider("activator.potions.hp", "Use POTIONS if HP are under {0}", 20, 0, 100, true);
            _activator.AddSlider("activator.potions.mana", "Use POTIONS if mana is under {0}", 20, 0, 100, true);
        }
        public static void BaseUltMenu()
        {
            _baseult = _menu.AddSubMenu("BaseUlt Menu", "baseult");
            _baseult.AddGroupLabel("OKTR AIO - BaseULT for " + Player.Instance.ChampionName,
                "baseult.grouplabel.utilitymenu");
            _baseult.AddCheckBox("baseult.use", "Use BaseUlt");
            _baseult.Add("baseult.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange += Value.AdvancedModeChanged;
            _baseult.AddCheckBox("baseult.recallsEnemy", "Show enemy recalls", true, true);
            _baseult.AddCheckBox("baseult.recallsAlly", "Show ally recalls", true, true);
            _baseult.AddSlider("baseult.x", "Recall location X", (int)(Drawing.Width * 0.4), 0, Drawing.Width, true);
            _baseult.AddSlider("baseult.y", "Recall location Y", (int)(Drawing.Height * 0.75), 0, Drawing.Height, true);
            _baseult.AddSlider("baseult.width", "Recall width", 300, 200, 500, true);
            _baseult.AddSeparator();
            _baseult.AddLabel("Use BaseULT for:", 25, "baseult.label", true);
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                _baseult.AddCheckBox("baseult." + enemy.ChampionName, enemy.ChampionName, true, true);
            }
        }
    }
}

