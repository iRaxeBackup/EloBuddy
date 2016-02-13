using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Xml.Linq;
using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Newtonsoft.Json;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility
{
    public static class JsonSettings
    {
        internal static string SandBox = SandboxConfig.DataDirectory + @"\OKTR\";
        internal static Profile Profile = new Profile();

       public static string FileLocation
        {
            get
            {
                // ReSharper disable once ConvertPropertyToExpressionBody
                return Path.Combine(
                    SandBox, Player.Instance.ChampionName + ".json");
            }
        }

        public static void Init()
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
            var profile = JsonConvert.DeserializeObject<Profile>(json);
            foreach (var option in profile.Options)
            {
                LoadOption(Value.MenuList.Find(m => m.UniqueMenuId.Contains(Value.MenuSubString(option.UiD)))[option.UiD], option);
            }
            Chat.Print("Loaded Settings for " + Player.Instance.ChampionName);
        }

        private static void LoadOption(ValueBase valueBase, JsonSetting type)
        {
            if (type.Type == Setting.Checkbox)
            {
                valueBase.Cast<CheckBox>().CurrentValue = Convert.ToBoolean(type.Value);
            }
            if (type.Type == Setting.Slider)
            {
                valueBase.Cast<Slider>().CurrentValue = Convert.ToInt32(type.Value);
            }
            if (type.Type == Setting.Combobox)
            {
                valueBase.Cast<ComboBox>().CurrentValue = Convert.ToInt32(type.Value);
            }
        }

        private static void SaveOption(ValueBase valueBase, JsonSetting option)
        {
            if (option.Type == Setting.Checkbox)
            {
                Profile.Options[Profile.Options.FindIndex(o => o.UiD == option.UiD)].Value = valueBase.Cast<CheckBox>().CurrentValue.ToString();
            }
            if (option.Type == Setting.Slider)
            {
                Profile.Options[Profile.Options.FindIndex(o => o.UiD == option.UiD)].Value = valueBase.Cast<Slider>().CurrentValue.ToString();
            }
            if (option.Type == Setting.Combobox)
            {
                Profile.Options[Profile.Options.FindIndex(o => o.UiD == option.UiD)].Value = valueBase.Cast<ComboBox>().CurrentValue.ToString();
            }
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
                    Chat.Print("Idk don't ask me");
                    args.Process = false;
                    break;
            }
        }


        [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
        private static void SaveProfile()
        {
            foreach (var option in Profile.Options)
            {
                SaveOption(Value.MenuList.Find(m => m.UniqueMenuId.Contains(Value.MenuSubString(option.UiD)))[option.UiD], option);
            }
            var json = JsonConvert.SerializeObject(Profile);
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
    }

    public enum Setting
    {
        Checkbox,
        Combobox,
        Slider,
        Key
    }

    public class JsonSetting
    {
        [JsonProperty(PropertyName = "UiD")]
        internal readonly string UiD;
        [JsonProperty(PropertyName = "Type")]
        internal readonly Setting Type;
        [JsonProperty(PropertyName = "Value")]
        internal string Value;

        public JsonSetting(string uid, Setting type, string value)
        {
            UiD = uid;
            Type = type;
            Value = value;
        }
    }

    internal class Profile
    {
        [JsonProperty(PropertyName = "Options")] public List<JsonSetting> Options { get; set; }

        internal Profile()
        {
            Options = new List<JsonSetting>();
        }
    }
}