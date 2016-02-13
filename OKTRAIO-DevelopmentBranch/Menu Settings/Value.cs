using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Utility;

namespace OKTRAIO.Menu_Settings
{
    public static class Value
    {
        public static bool AddNameToEnd { get; set; }
        public static void Init()
        {
            if (MainMenu.Combo != null) MenuList.Add(MainMenu.Combo);
            if (MainMenu.Lane != null) MenuList.Add(MainMenu.Lane);
            if (MainMenu.Jungle != null) MenuList.Add(MainMenu.Jungle);
            if (MainMenu.Lasthit != null) MenuList.Add(MainMenu.Lasthit);
            if (MainMenu.Harass != null) MenuList.Add(MainMenu.Harass);
            if (MainMenu.Flee != null) MenuList.Add(MainMenu.Flee);
            if (MainMenu.Misc != null) MenuList.Add(MainMenu.Misc);
            if (MainMenu.Ks != null) MenuList.Add(MainMenu.Ks);
            if (MainMenu.Draw != null) MenuList.Add(MainMenu.Draw);

            //Utilities are now added as the menu is created (in UtilityAddon.Constructor())
        }

        public static readonly string[] MenuStrings = { "combo", "lane", "jungle", "lasthit", "harass", "flee", "misc", "killsteal", "draw", "activator", "baseult", "randomult", "recall", "bushreveal", "tracker", "flashassistant", "autolantern" };
        public static List<string> AdvancedMenuItemUiDs = new List<string>();
        public static List<Menu> MenuList = new List<Menu>();

        public static bool Use(string id)
        {
            return MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(id)))[id].Cast<CheckBox>().CurrentValue;
        }

        public static string ComboString(string id)
        {
            return MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(id)))[id].Cast<ComboBox>().SelectedText;
        }

        public static int ComboGet(string id)
        {
            return MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(id)))[id].Cast<ComboBox>().CurrentValue;
        }

        public static string MenuSubString(string id)
        {
            return id.Substring(0, id.IndexOf(".", StringComparison.OrdinalIgnoreCase));
        }

        public static bool Mode(Orbwalker.ActiveModes id)
        {
            return Orbwalker.ActiveModesFlags.HasFlag(id);
        }

        public static int Get(string id)
        {
            return MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(id)))[id].Cast<Slider>().CurrentValue;
        }
        public static bool Active(string id)
        {
            return MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(id)))[id].Cast<KeyBind>().CurrentValue;
        }

        public static void AddComboBox(this Menu menu, string uid, string displayname, IEnumerable<string> textValues, int defaultIndex = 0,
            bool advanced = false)
        {
            uid = AddNameToEnd ? uid.AddName() : uid;
            menu.Add(uid, new ComboBox(displayname, textValues, defaultIndex));
            JsonSettings.Profile.Options.Add(new JsonSetting(uid, Setting.Combobox, menu[uid].Cast<ComboBox>().CurrentValue.ToString()));
            if (!advanced) return;
            AdvancedMenuItemUiDs.Add(uid);
            menu[uid].IsVisible = menu[GetMenuString(menu) + ".advanced"].Cast<CheckBox>().CurrentValue;
        }

        public static void AddCheckBox(this Menu menu, string uid, string displayname, bool defaultvalue = true,
            bool advanced = false)
        {
            uid = AddNameToEnd ? uid.AddName() : uid;
            menu.Add(uid, new CheckBox(displayname, defaultvalue));
            JsonSettings.Profile.Options.Add(new JsonSetting(uid, Setting.Checkbox, menu[uid].Cast<CheckBox>().CurrentValue.ToString()));
            if (!advanced) return;
            AdvancedMenuItemUiDs.Add(uid);
            menu[uid].IsVisible = menu[GetMenuString(menu) + ".advanced"].Cast<CheckBox>().CurrentValue;
        }

        public static void AddSlider(this Menu menu, string uid, string displayName, int defaultValue = 0,
            int minValue = 0, int maxValue = 100, bool advanced = false)
        {
            uid = AddNameToEnd ? uid.AddName() : uid;
            menu.Add(uid, new Slider(displayName, defaultValue, minValue, maxValue));
            JsonSettings.Profile.Options.Add(new JsonSetting(uid, Setting.Slider, menu[uid].Cast<Slider>().CurrentValue.ToString()));
            if (!advanced) return;
            AdvancedMenuItemUiDs.Add(uid);
            menu[uid].IsVisible = menu[GetMenuString(menu) + ".advanced"].Cast<CheckBox>().CurrentValue;
        }

        public static void AddLabel(this Menu menu, string text, int size = 25, string uid = null, bool advanced = false)
        {
            if (uid != null)
            {
                menu.Add(uid, new Label(text));
                if (!advanced) return;
                AdvancedMenuItemUiDs.Add(uid);
                menu[uid].IsVisible =
                    menu[GetMenuString(menu) + ".advanced"].Cast<CheckBox>().CurrentValue;
            }
            else
            {
                menu.AddLabel(text, size);
            }
        }

        public static void AddGroupLabel(this Menu menu, string text, string uid = null, bool advanced = false)
        {
            if (uid != null)
            {
                menu.Add(uid, new GroupLabel(text));
                if (!advanced) return;
                AdvancedMenuItemUiDs.Add(uid);
                menu[uid].IsVisible =
                    menu[GetMenuString(menu) + ".advanced"].Cast<CheckBox>().CurrentValue;
            }
            else
            {
                menu.AddGroupLabel(text);
            }
        }

        private static string GetMenuString(Menu menu)
        {
            return MenuStrings.First(m => menu.UniqueMenuId.Contains(m));
        }

        public static void AdvancedModeChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (AdvancedMenuItemUiDs.All(uid => MenuSubString(sender.SerializationId) != MenuSubString(uid))) return;
            foreach (
                var box in
                    AdvancedMenuItemUiDs.Where(uid => MenuSubString(sender.SerializationId) == MenuSubString(uid)))
            {
                MenuList.Find(m => m.UniqueMenuId.Contains(MenuSubString(box)))[box].IsVisible = args.NewValue;
            }
        }

        public static string AddName(this string data)
        {
            return data + "." + Player.Instance.ChampionName;
        }
    }
}