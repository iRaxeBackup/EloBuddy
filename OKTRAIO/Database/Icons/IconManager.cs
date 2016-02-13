using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Rendering;

namespace OKTRAIO.Database.Icons
{
    public static class IconManager
    {
        public static IconGenerator IconGenerator { get; set; }
        private static Dictionary<Cache, Bitmap> BitmapCache { get; set; }
        private static Dictionary<Cache, Sprite> SpriteCache { get; set; }
        static IconManager()
        {
            IconGenerator = new IconGenerator(IconGenerator.IconType.Circle, 32, 32, IconGenerator.DefaultGoldColor, 2);
            BitmapCache = new Dictionary<Cache, Bitmap>();
            SpriteCache = new Dictionary<Cache, Sprite>();
        }
        public static void Init() { } // Used so constructor is called. That is all ;)

        private static Cache CreateCache(Cache.CacheType cacheType,  string name)
        {
            return new Cache(cacheType, name, IconGenerator.Type, IconGenerator.Width, IconGenerator.BorderColor, IconGenerator.BorderWidth);
        }
        private static void SetIconGeneratorSettings(IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            IconGenerator.Type = type;
            IconGenerator.Width = size;
            IconGenerator.Height = size;
            IconGenerator.BorderColor = borderColor;
            IconGenerator.BorderWidth = borderWidth;
        }

        private static Bitmap GetBitmap(Cache.CacheType cacheType, string name, IconGenerator.IconType type, 
            int size, Color borderColor, float borderWidth, out Cache cache)
        {
            SetIconGeneratorSettings(type, size, borderColor, borderWidth);
            cache = CreateCache(cacheType, name);
            if (BitmapCache.ContainsKey(cache)) return BitmapCache[cache]; //Bitmap has been created already, return it.

            //Bitmap has been created yet, so lets create it and cache it.
            return BitmapCache[cache] = IconGenerator.GetIcon(name); // All done, returning sprite.
        }

        private static Sprite GetSprite(Cache.CacheType cacheType, string name, IconGenerator.IconType type,
            int size, Color borderColor, float borderWidth, out Cache cache)
        {
            var bitmap = GetBitmap(cacheType, name, type, size, borderColor, borderWidth,  out cache);
            if (SpriteCache.ContainsKey(cache)) return SpriteCache[cache];
            return SpriteCache[cache] = new Sprite(TextureLoader.BitmapToTexture(bitmap)); // All done, returning sprite.
        }

        #region Champions
        public static Bitmap GetChampionBitmap(string hero, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetBitmap(Cache.CacheType.Champion, hero, type, size, borderColor, borderWidth, out cache);
        }
        public static Sprite GetChampionSprite(string hero, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetSprite(Cache.CacheType.Champion, hero, type, size, borderColor, borderWidth, out cache);
        }
        public static Bitmap GetChampionBitmap(AIHeroClient hero, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetBitmap(Cache.CacheType.Champion, hero.ChampionName, type, size, borderColor, borderWidth, out cache);
        }
        public static Sprite GetChampionSprite(AIHeroClient hero, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetSprite(Cache.CacheType.Champion, hero.ChampionName, type, size, borderColor, borderWidth, out cache);
        }
        #endregion

        #region Spells
        public static Bitmap GetSpellBitmap(SpellDataInst spell, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetBitmap(Cache.CacheType.Spell, spell.Name, type, size, borderColor, borderWidth, out cache);
        }
        public static Sprite GetSpellSprite(SpellDataInst spell, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetSprite(Cache.CacheType.Spell, spell.Name, type, size, borderColor, borderWidth, out cache);
        }
        public static Bitmap GetSpellBitmap(string spellName, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetBitmap(Cache.CacheType.Spell, spellName, type, size, borderColor, borderWidth, out cache);
        }
        public static Sprite GetSpellSprite(string spellName, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
        {
            Cache cache;
            return GetSprite(Cache.CacheType.Spell, spellName, type, size, borderColor, borderWidth, out cache);
        }
        #endregion

        private struct Cache
        {

            public readonly string Name;
            public readonly CacheType Type;
            public readonly IconGenerator.IconType IconType;
            public readonly int Size;
            public readonly Color BorderColor;
            public readonly float BorderWidth;

            public Cache(CacheType cacheType, string name, IconGenerator.IconType type, int size, Color borderColor, float borderWidth)
            {
                Type = cacheType;
                Name = name;
                IconType = type;
                Size = size;
                BorderColor = borderColor;
                BorderWidth = borderWidth;
            }
            public enum CacheType
            {
                Champion,
                Spell
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Cache)) return false;
                return Equals((Cache)obj);
            }
            public bool Equals(Cache other)
            {
                return string.Equals(Name, other.Name) && Type == other.Type && IconType == other.IconType && Size == other.Size && BorderColor.Equals(other.BorderColor) && Math.Abs(BorderWidth - other.Size) < 1;
            }

            public static bool operator ==(Cache left, Cache right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Cache left, Cache right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            /// Please don't use, just here to remove compiler warning. (Massive CPU usage)
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (Name != null ? Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int)Type;
                    hashCode = (hashCode * 397) ^ (int)IconType;
                    hashCode = (hashCode * 397) ^ Size;
                    hashCode = (hashCode * 397) ^ BorderColor.GetHashCode();
                    hashCode = (hashCode * 397) ^ (int) BorderWidth;
                    return hashCode;
                }
            }
        }
    }
}