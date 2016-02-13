using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility.SkinManager
{
    public class SkinManagement : UtilityAddon
    {
        public XmlDocument InfoXml;
        internal Model[] Models;
        public int OriginalSkinIndex;

        public string[] ModelNames;

        internal Model GetModelByIndex(int index)
        {
            return Models[index];
        }

        internal Model GetModelByName(string name)
        {
            return
                Models.FirstOrDefault(
                    model => string.Equals(model.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "SkinManager", "skinmanager", "Unknown");
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - Skinmanager for " + Player.Instance.ChampionName,
                "skinmanager.grouplabel.utilitymenu");
            Menu.AddLabel("PSA: Changing your Model might in rare cases crash the game." + Environment.NewLine +
                                 "This does not apply to changing skin.");
            Menu.AddSeparator();
            Menu.Add("skinmanager.models", new Slider("Model - ", 0, 0, 0)).OnValueChange +=
                SkinManager_OnModelSliderChange;
            Menu.Add("skinmanager.skins", new Slider("Skin - Classic", 0, 0, 0)).OnValueChange +=
                SkinManager_OnSkinSliderChange;
            Menu.Add("skinmanager.resetModel", new CheckBox("Reset Model", false)).OnValueChange +=
                SkinManager_OnResetModel;
            Menu.Add("skinmanager.resetSkin", new CheckBox("Reset Skin", false)).OnValueChange +=
                SkinManager_OnResetSkin;
            Menu.AddSeparator();
        }

        public override void Initialize()
        {
            using (
                var infoStream =
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("OKTRAIO.Utility.SkinManager.SkinInfo.xml"))

                if (infoStream != null)

                    using (var infoReader = new StreamReader(infoStream))
                    {
                        InfoXml = new XmlDocument();
                        InfoXml.LoadXml(infoReader.ReadToEnd());
                    }

            if (InfoXml.DocumentElement != null)
                Models =
                    InfoXml.DocumentElement.ChildNodes.Cast<XmlElement>()
                        .Select(
                            model =>
                                new Model(model.Attributes["name"].Value,
                                    model.ChildNodes.Cast<XmlElement>()
                                        .Select(
                                            skin =>
                                                new ModelSkin(skin.Attributes["name"].Value,
                                                    skin.Attributes["index"].Value))
                                        .ToArray()))
                        .ToArray();
            ModelNames = Models.Select(model => model.Name).ToArray();

            OriginalSkinIndex = Player.Instance.SkinId;
            Menu["skinmanager.models"].Cast<Slider>().MaxValue = Models.Length - 1;
            Menu["skinmanager.models"].Cast<Slider>().CurrentValue = Array.IndexOf(ModelNames,
                Player.Instance.ChampionName);
        }

        public void SkinManager_OnResetSkin(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            Menu["skinmanager.skins"].Cast<Slider>().CurrentValue = OriginalSkinIndex;
            if (Menu["skinmanager.resetSkin"].Cast<CheckBox>().CurrentValue)
                Menu["skinmanager.resetSkin"].Cast<CheckBox>().CurrentValue = false;
        }

        public void SkinManager_OnResetModel(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            Menu["skinmanager.models"].Cast<Slider>().CurrentValue = Array.IndexOf(ModelNames,
                Player.Instance.ChampionName);

            if (Menu["skinmanager.resetModel"].Cast<CheckBox>().CurrentValue)
                Menu["skinmanager.resetModel"].Cast<CheckBox>().CurrentValue = false;
        }

        public void SkinManager_OnSkinSliderChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            var model = GetModelByIndex(Menu["skinmanager.models"].Cast<Slider>().CurrentValue);
            var skin = model.Skins[Menu["skinmanager.skins"].Cast<Slider>().CurrentValue];
            Menu["skinmanager.skins"].Cast<Slider>().DisplayName = "Skin - " + skin.Name;
            Player.SetSkinId(skin.Index);
        }

        public void SkinManager_OnModelSliderChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            var model = GetModelByIndex(Menu["skinmanager.models"].Cast<Slider>().CurrentValue);
            Menu["skinmanager.models"].Cast<Slider>().DisplayName = "Model - " + model.Name;
            Player.SetModel(model.Name);
            Menu["skinmanager.skins"].Cast<Slider>().CurrentValue = 0;
            Menu["skinmanager.skins"].Cast<Slider>().MaxValue = model.Skins.Length - 1;
        }

        internal struct Model
        {
            public readonly string Name;
            public readonly ModelSkin[] Skins;

            public Model(string name, ModelSkin[] skins)
            {
                Name = name;
                Skins = skins;
            }

            public string[] GetSkinNames()
            {
                return Skins.Select(skin => skin.Name).ToArray();
            }
        }

        internal struct ModelSkin
        {
            public readonly string Name;
            public readonly int Index;

            public ModelSkin(string name, string index)
            {
                Name = name;
                Index = int.Parse(index);
            }
        }

        public SkinManagement(Menu menu) : base(menu, null)
        {
        }
    }
}