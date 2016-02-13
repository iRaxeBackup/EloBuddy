using System.Drawing;
using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace OKTRAIO.Menu_Settings
{
    public static class ColorMenu
    {
        private static readonly Color[] Colors =
        {
            Color.AliceBlue, Color.LightSalmon, Color.AntiqueWhite, Color.LightSeaGreen, Color.Aqua, Color.LightSkyBlue,
            Color.Aquamarine, Color.LightSlateGray, Color.Azure, Color.LightSteelBlue, Color.Beige,
            Color.LightYellow, Color.Bisque, Color.Lime, Color.Black, Color.LimeGreen, Color.BlanchedAlmond, Color.Linen,
            Color.Blue, Color.Magenta, Color.BlueViolet, Color.Maroon, Color.Brown, Color.MediumAquamarine,
            Color.BurlyWood, Color.MediumBlue, Color.CadetBlue, Color.MediumOrchid, Color.Chartreuse, Color.MediumPurple,
            Color.Chocolate, Color.MediumSeaGreen, Color.Coral, Color.MediumSlateBlue, Color.CornflowerBlue,
            Color.MediumSpringGreen, Color.Cornsilk, Color.MediumTurquoise, Color.Crimson, Color.MediumVioletRed,
            Color.Cyan, Color.MidnightBlue, Color.DarkBlue, Color.MintCream, Color.DarkCyan, Color.MistyRose,
            Color.DarkGoldenrod, Color.Moccasin, Color.DarkGray, Color.NavajoWhite, Color.DarkGreen, Color.Navy,
            Color.DarkKhaki, Color.OldLace, Color.DarkMagenta, Color.Olive, Color.DarkOliveGreen, Color.OliveDrab,
            Color.DarkOrange, Color.Orange, Color.DarkOrchid, Color.OrangeRed, Color.DarkRed, Color.Orchid,
            Color.DarkSalmon, Color.PaleGoldenrod, Color.DarkSeaGreen, Color.PaleGreen, Color.DarkSlateBlue,
            Color.PaleTurquoise, Color.DarkSlateGray, Color.PaleVioletRed, Color.DarkTurquoise, Color.PapayaWhip,
            Color.DarkViolet, Color.PeachPuff, Color.DeepPink, Color.Peru, Color.DeepSkyBlue, Color.Pink,
            Color.DimGray, Color.Plum, Color.DodgerBlue, Color.PowderBlue, Color.Firebrick, Color.Purple,
            Color.FloralWhite, Color.Red, Color.ForestGreen, Color.RosyBrown, Color.Fuchsia, Color.RoyalBlue,
            Color.Gainsboro, Color.SaddleBrown, Color.GhostWhite, Color.Salmon, Color.Gold, Color.SandyBrown,
            Color.Goldenrod, Color.SeaGreen, Color.Gray, Color.SeaShell, Color.Green, Color.Sienna, Color.GreenYellow,
            Color.Silver, Color.Honeydew, Color.SkyBlue, Color.HotPink, Color.SlateBlue, Color.IndianRed,
            Color.SlateGray, Color.Indigo, Color.Snow, Color.Ivory, Color.SpringGreen, Color.Khaki, Color.SteelBlue,
            Color.Lavender, Color.Tan, Color.LavenderBlush, Color.Teal, Color.LawnGreen, Color.Thistle,
            Color.LemonChiffon, Color.Tomato, Color.LightBlue, Color.Turquoise, Color.LightCoral, Color.Violet,
            Color.LightCyan, Color.Wheat, Color.LightGoldenrodYellow, Color.White, Color.LightGreen, Color.WhiteSmoke,
            Color.LightGray, Color.Yellow, Color.LightPink, Color.YellowGreen
        };

        private static readonly string[] ColorsName =
        {
            "AliceBlue", "LightSalmon", "AntiqueWhite", "LightSeaGreen", "Aqua", "LightSkyBlue", "Aquamarine",
            "LightSlateGray", "Azure", "LightSteelBlue", "Beige", "LightYellow", "Bisque", "Lime", "Black",
            "LimeGreen", "BlanchedAlmond", "Linen", "Blue", "Magenta", "BlueViolet", "Maroon", "Brown",
            "MediumAquamarine", "BurlyWood", "MediumBlue", "CadetBlue", "MediumOrchid", "Chartreuse",
            "MediumPurple", "Chocolate", "MediumSeaGreen", "Coral", "MediumSlateBlue", "CornflowerBlue",
            "MediumSpringGreen", "Cornsilk", "MediumTurquoise", "Crimson", "MediumVioletRed", "Cyan", "MidnightBlue",
            "DarkBlue", "MintCream", "DarkCyan", "MistyRose", "DarkGoldenrod", "Moccasin", "DarkGray", "NavajoWhite",
            "DarkGreen", "Navy", "DarkKhaki", "OldLace", "DarkMagena", "Olive", "DarkOliveGreen", "OliveDrab",
            "DarkOrange", "Orange", "DarkOrchid", "OrangeRed", "DarkRed", "Orchid", "DarkSalmon", "PaleGoldenrod",
            "DarkSeaGreen", "PaleGreen", "DarkSlateBlue", "PaleTurquoise", "DarkSlateGray", "PaleVioletRed",
            "DarkTurquoise", "PapayaWhip", "DarkViolet", "PeachPuff", "DeepPink", "Peru", "DeepSkyBlue",
            "Pink", "DimGray", "Plum", "DodgerBlue", "PowderBlue", "Firebrick", "Purple", "FloralWhite", "Red",
            "ForestGreen", "RosyBrown", "Fuschia", "RoyalBlue", "Gainsboro", "SaddleBrown", "GhostWhite", "Salmon",
            "Gold", "SandyBrown", "Goldenrod", "SeaGreen", "Gray", "Seashell", "Green", "Sienna", "GreenYellow",
            "Silver", "Honeydew", "SkyBlue", "HotPink", "SlateBlue", "IndianRed", "SlateGray", "Indigo", "Snow",
            "Ivory", "SpringGreen", "Khaki", "SteelBlue", "Lavender", "Tan", "LavenderBlush", "Teal", "LawnGreen",
            "Thistle", "LemonChiffon", "Tomato", "LightBlue", "Turquoise", "LightCoral", "Violet", "LightCyan",
            "Wheat", "LightGoldenrodYellow", "White", "LightGreen", "WhiteSmoke", "LightGray", "Yellow", "LightPink",
            "YellowGreen"
        };

        public static void AddColorItem(this Menu menu, string uniqueId, int defaultColour = 0)
        {
            var a = menu.Add(uniqueId, new Slider("Color Picker: ", defaultColour, 0, Colors.Count() - 1));
            a.DisplayName = "Color Picker: " + ColorsName[a.CurrentValue];
            a.OnValueChange += delegate { a.DisplayName = "Colour Picker: " + ColorsName[a.CurrentValue]; };
        }

        public static Color GetColor(this Menu m, string id)
        {
            var number = m[id].Cast<Slider>();
            if (number != null)
            {
                return Colors[number.CurrentValue];
            }
            return Color.White;
        }
    }
}