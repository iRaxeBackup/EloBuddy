using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Newtonsoft.Json;

namespace OKTRAIO.Utility
{
    public class ChampionProfiles
    {
        internal static string SandBox = SandboxConfig.DataDirectory + @"\OKTR\";

        public enum OptionType
        {
            Checkbox,
            Slider,
            Key
        }

        public enum MenuType
        {
            Manamanager,
            Autoleveler,
            Castmanager
        }

        private struct ChampionProfile
        {
            [JsonProperty(PropertyName = "Version")]
            public readonly double Version;
            [JsonProperty(PropertyName = "Settings")]
            public readonly List<ProfileOption> Settings;

            public ChampionProfile(double version, List<ProfileOption> settings)
            {
                Version = version;
                Settings = settings;
            }
        }

        public struct ProfileOption
        {
            [JsonProperty(PropertyName = "MenuType")]
            internal readonly MenuType MenuType;
            [JsonProperty(PropertyName = "Id")]
            internal readonly string Id;
            [JsonProperty(PropertyName = "Type")]
            internal readonly OptionType Type;
            [JsonProperty(PropertyName = "Value")]
            internal readonly string Value;

            public ProfileOption(MenuType menuType, string id, OptionType type, string value)
            {
                MenuType = menuType;
                Id = id;
                Type = type;
                Value = value;
            }
        }

        public static string FileLocation
        {
            get
            {
                // ReSharper disable once ConvertPropertyToExpressionBody
                return Path.Combine(
                    SandBox, Player.Instance.ChampionName + ".json");
            }
        }

        public ChampionProfiles()
        {
            if (!Directory.Exists(SandBox))
                Directory.CreateDirectory(SandBox);

            if (!File.Exists(FileLocation))
                SaveProfile();
            else LoadProfile();

            Game.OnEnd += OnGameEnd;
            Game.OnDisconnect += OnDisconnect;
            Chat.OnInput += OnChatInput;
        }

        private static void OnDisconnect(EventArgs args)
        {
            SaveProfile();
        }

        private static void OnGameEnd(GameEndEventArgs args)
        {
            SaveProfile();
        }

        [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
        private static void LoadProfile()
        {
            var json = File.ReadAllText(FileLocation);
            var profile = JsonConvert.DeserializeObject<ChampionProfile>(json);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (profile.Version == 1.0)
            {
                foreach (var setting in profile.Settings)
                {
                    Menu settingMenu = MainMenu.GetMenu("volatilemenu.autoleveler");
                    if (setting.MenuType == MenuType.Manamanager)
                        settingMenu = MainMenu.GetMenu("volatilemenu.manamanager");
                    else if (setting.MenuType == MenuType.Castmanager) settingMenu = MainMenu.GetMenu("volatilemenu.castmenu");
                    switch (setting.Type)
                    {
                        case OptionType.Checkbox:
                            settingMenu[setting.Id].Cast<CheckBox>().CurrentValue = Convert.ToBoolean(setting.Value);
                            break;
                        case OptionType.Slider:
                            settingMenu[setting.Id].Cast<Slider>().CurrentValue = Convert.ToInt32(setting.Value);
                            break;
                    }
                }
            }
            Chat.Print("Loaded Settings for " + Player.Instance.ChampionName);
        }

        private static void OnChatInput(ChatInputEventArgs args)
        {
            switch (args.Input)
            {
                case "/save":
                    SaveProfile();
                    args.Process = false;
                    break;
                case "/load":
                    LoadProfile();
                    args.Process = false;
                    break;
                case "/help":
                    Chat.Print(Options.Count);
                    args.Process = false;
                    break;
            }
        }


        [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
        private static void SaveProfile()
        {
            var profile = new ChampionProfile(1.0, Options);
            var json = JsonConvert.SerializeObject(profile);
            if (File.Exists(FileLocation))
            {
                File.Delete(FileLocation);
                Chat.Print("Saved Settings for " + Player.Instance.ChampionName);
            }
            else
            {
                Chat.Print("Settings File created for " + Player.Instance.ChampionName);
            }
            using (var sw = new StreamWriter(FileLocation))
            {
                sw.Write(json);
            }
        }

        public static List<ProfileOption> Options;
    }
}