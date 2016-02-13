using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using Newtonsoft.Json;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Properties;

/*namespace OKTRAIO.Utility.AutoLVLUP
{
    internal class ParseModule
    {
        private readonly JsonChampionAbilitySequence _abilitySequence;

        private ParseModule()
        {
            try
            {
                var sequence = Encoding.UTF8.GetString(Resources._6_1);
                _abilitySequence = JsonConvert.DeserializeObject<JsonChampionAbilitySequence>(sequence);
            }
            catch (Exception e)
            {
                Chat.Print("JSON parse FAILED", Color.Red);
                Console.Write(e);
            }
        }

        private static void InitializeComponent()
        {
            try
            {
                if (!Value.Use("autolvlup.use"))
                {
                    return;
                }

                var parseModule = new ParseModule();
                var sequence = parseModule._abilitySequence;

                if (sequence == null)
                    return;

                if (sequence.Version != Game.Version)
                {
                    if (!Value.Use("autolvlup.ignoreversion"))
                        return;
                }

                if (sequence.Data.FirstOrDefault() == null)
                    return;

                if (
                    sequence.Data.TrueForAll(
                        e =>
                            e.ChampionName == Player.Instance.ChampionName && e.Role == Value.Get("autolvlup.role") &&
                            e.DamageType == Value.Get("autolvlup.damagetype")))
                {
                    Obj_AI_Base.OnLevelUp += LevelUpComponent;
                }
                else
                {
                    Chat.Print("Your Autolvlup Configuration or Champion is not yet Supported!", Color.Red);
                }
            }
            catch (Exception e)
            {
                Chat.Print("JSON parse ERROR: Code INITIALIZECOMPONENT", Color.Red);
                Console.Write(e);
            }
        }

        private static void LevelUpComponent(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            try
            {
                var parseModule = new ParseModule();
                var sequence = parseModule._abilitySequence;

                if (sequence == null)
                    return;

                var seq = sequence.Data.Find(
                    e =>
                        e.ChampionName == Player.Instance.ChampionName && e.Role == Value.Get("autolvlup.role") &&
                        e.DamageType == Value.Get("autolvlup.damagetype")).Sequence;

                var currentLevel = seq[args.Level];

                switch (currentLevel)
                {
                    case 1:
                        Player.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        Player.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        Player.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        Player.LevelSpell(SpellSlot.R);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Chat.Print("JSON parse ERROR: Code LEVELUPCOMPONENT", Color.Red);
                Console.Write(e);
            }
        }

        // 1 = Laner, 2 = Jungler, 3 = Support
        // 1 = ap, 2 = ad
        internal class JsonChampionAbilitySequence
        {
            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("data")]
            public List<ChampionData> Data { get; set; }
        }

        internal class ChampionData
        {
            [JsonProperty("champion")]
            public string ChampionName { get; set; }

            [JsonProperty("role")]
            public int Role { get; set; }

            [JsonProperty("damagetype")]
            public int DamageType { get; set; }

            [JsonProperty("sequence")]
            public int[] Sequence { get; set; }
        }
    }

    internal class LevelerMisc
    {
        public static void DamageTypeSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            try
            {
                switch (args.NewValue)
                {
                    case 1:
                        sender.DisplayName += "AP";
                        break;
                    case 2:
                        sender.DisplayName += "AD";
                        break;
                }
            }
            catch (Exception e)
            {
                Chat.Print("JSON parse ERROR: Code AUTOLVLUP.DAMAGETYPESLIDER");
                Console.Write(e);
            }
        }

        public static void RoleSlider(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            try
            {
                switch (args.NewValue)
                {
                    case 1:
                        sender.DisplayName += "Laner";
                        break;
                    case 2:
                        sender.DisplayName += "Jungler";
                        break;
                    case 3:
                        sender.DisplayName += "Support";
                        break;
                }
            }
            catch (Exception e)
            {
                Chat.Print("JSON parse ERROR: Code AUTOLVLUP.ROLESLIDER");
                Console.Write(e);
            }
        }
         public static void LvlUpMenu()
        {
            Autolvlup = Menu.AddSubMenu("AutoLVLUP Menu", "autolvlup");
            Autolvlup.AddGroupLabel("OKTR AIO - AutoLVLUP for " + Player.Instance.ChampionName,
                "autolvlup.grouplabel.utilitymenu");
            Autolvlup.AddCheckBox("autolvlup.use", "Use AutoLVLUP");
            Autolvlup.Add("autolvlup.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange +=
                Value.AdvancedModeChanged;
            Autolvlup.AddSlider("autolvlup.mode", "Set Mode:", 1, 1, 2);
            Autolvlup["autolvlup.damagetype"].Cast<Slider>().OnValueChange += LevelerMisc.DamageTypeSlider;
            Autolvlup.AddSlider("autolvlup.role", "Set Role:", 1, 1, 3);
            Autolvlup["autolvlup.role"].Cast<Slider>().OnValueChange += LevelerMisc.RoleSlider;
            Autolvlup.AddCheckBox("autolvlup.ignorevers", "Ignore Version Differences", true, true);
        }
    }
}*/