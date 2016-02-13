using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using OKTRAIO.Database.Icons;
using OKTRAIO.Menu_Settings;
using OKTRAIO.Properties;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Line = EloBuddy.SDK.Rendering.Line;
using MainMenu = EloBuddy.SDK.Menu.MainMenu;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace OKTRAIO.Utility.Tracker
{
    public class Tracker : UtilityAddon
    {
        public Dictionary<AIHeroClient, SpellAvaliblity> HeroSpellAvaliblitys { get; set; }
        private Dictionary<AIHeroClient, Dictionary<SpellSlot, Sprite>> HeroSpellSprites { get; set; }

        private static Vector2 _lineTrackerOffset = new Vector2(3, 10);
        private static float _spellTrackerWidth = 112 / 4f;
        private static Color _availableLineColor = Color.Green;
        private static Color _notAvailableLineColor = Color.Red;
        private static Color _notLearnedeLineColor = Color.OrangeRed;

        private Sprite TrackerHud { get; set; } 

        public Tracker(Menu menu) : base(menu)
        {

        }

        public override UtilityInfo GetUtilityInfo()
        {
            return new UtilityInfo(this, "Tracker", "tracker", "coman3");
        }

        protected override void InitializeMenu()
        {
            Menu.AddGroupLabel("OKTR AIO - Tracker", "tracker.grouplabel.utilitymenu");
            Menu.AddLabel("Developed By: Coman3");
            Menu.AddCheckBox("tracker.enable", "Enable", false);
            Menu.AddLabel("Please note that the tracker is still not complete but is working.");
            Menu.AddCheckBox("tracker.show.spells.summoner", "Show Summoner Spells");
            Menu.AddCheckBox("tracker.show.spells.normal", "Show Normal Spells");
            Menu.AddCheckBox("tracker.show.player.me", "Track Me");
            Menu.AddCheckBox("tracker.show.player.allies", "Track Allies");
            Menu.AddCheckBox("tracker.show.player.enemies", "Track Enemies");
            Menu.Add("tracker.reload", new CheckBox("Reload Tracker", false)).OnValueChange += Reload;

            Menu.AddSeparator();
            Menu.Add("tracker.advanced", new CheckBox("Show Advanced Menu", false)).OnValueChange += Value.AdvancedModeChanged;
            Menu.AddCheckBox("tracker.show.onlyhpbarrendered", "Only show Tracker on visible Enemies", false, true);
            Menu.AddLabel("This is currently buggy as the core does not fully support this option");

            Menu.AddSlider("tracker.visual.offset.x", "X Offset", 1, -100, 100, true);
            Menu.AddSlider("tracker.visual.offset.y", "Y Offset", 19, -100, 100, true);
            
        }

        public override void Initialize()
        {
            if (!Value.Use("tracker.enable"))
            {
                Logger.Warn("Tracker Disabled!");
                return;
            }
            HeroSpellAvaliblitys = new Dictionary<AIHeroClient, SpellAvaliblity>(EntityManager.Heroes.AllHeroes.Count);
            HeroSpellSprites = new Dictionary<AIHeroClient, Dictionary<SpellSlot, Sprite>>();
            TrackerHud = new Sprite(TextureLoader.BitmapToTexture(Resources.SpellLayout2));
            IconManager.IconGenerator.Padding = 0;
            using (new TimeMeasure("Tracker Sprite Generation"))
            {
                foreach (var hero in EntityManager.Heroes.AllHeroes)
                {
                    var spellAvaliblity = new SpellAvaliblity(hero);
                    HeroSpellAvaliblitys[hero] = spellAvaliblity;
                    var spriteDictonary = new Dictionary<SpellSlot, Sprite>();
                    foreach (SpellSlot slot in SpellAvaliblity.TrackedSpellSlots)
                    {
                        
                        if (slot != SpellSlot.Summoner1 && slot != SpellSlot.Summoner2) continue;
                        var spell = spellAvaliblity.GetSpell(slot);
                        spriteDictonary[slot] = IconManager.GetSpellSprite(spell, IconGenerator.IconType.Square, 8, Color.Empty, 1);
                    }
                    HeroSpellSprites[hero] = spriteDictonary;
                }
            }
        }

        public void Reload(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (!args.OldValue && args.NewValue)
            {
                Chat.Print("Reloaded Tracker!", Color.Green);
                
                sender.CurrentValue = false;
            }

        }

        private Vector2 GetDrawPos(AIHeroClient hero)
        {
            return new Vector2(hero.HPBarPosition.X + Value.Get("tracker.visual.offset.x"), hero.HPBarPosition.Y + Value.Get("tracker.visual.offset.y"));
        }

        private Vector2 GetChampionOffset(Vector2 value, Champion champion)
        {
            switch (champion)
            {
                case Champion.Darius:
                    return OffsetVector(value, -4, 0);
                
                default:
                    return value;
            }
        }
        private Vector2 OffsetVector(Vector2 value, Vector2 offset)
        {
            return OffsetVector(value, offset.X, offset.Y);
        }
        private Vector2 OffsetVector(Vector2 value, float xoffset, float yoffset)
        {
            return new Vector2(value.X + xoffset, value.Y + yoffset);
        }
        protected override void Drawing_OnEndScene(EventArgs args)
        {

            //PLEASE IGNORE HOW SHIT THIS CODE IS I AM TIRED AND WANT TO GO TO SLEEP! I WILL FIX IT WHEN I GET TO IT
            if (!Value.Use("tracker.enable")) return;
            if(MainMenu.IsOpen) return;
            if(Shop.IsOpen) return;
            foreach (var avaliblity in HeroSpellAvaliblitys)
            {
                var hero = avaliblity.Key;
                if(hero.IsDead || hero.IsNoRender) return;
                if (Value.Use("tracker.show.onlyhpbarrendered") && !hero.IsHPBarRendered) return;
                if(hero.IsMe && !Value.Use("tracker.show.player.me")) return;
                if ((!hero.IsEnemy || !Value.Use("tracker.show.player.enemies")) && (!hero.IsAlly || !Value.Use("tracker.show.player.allies"))) continue; // || hero.IsMe) continue;

                var drawPos = hero.IsMe ? OffsetVector(GetDrawPos(avaliblity.Key), 0, -8) : GetDrawPos(avaliblity.Key);
                drawPos = GetChampionOffset(drawPos, hero.Hero);
                var spells = avaliblity.Value;
                var startPos = OffsetVector(drawPos,  _lineTrackerOffset);
                var endPos = OffsetVector(drawPos, _lineTrackerOffset.X + _spellTrackerWidth * 4, _lineTrackerOffset.Y);
                Line.DrawLine(Color.Black, 4, startPos, endPos);
                for (int slotIndex = 0; slotIndex < SpellAvaliblity.TrackedSpellSlots.Length; slotIndex++)
                {
                    var slot = SpellAvaliblity.TrackedSpellSlots[slotIndex];
                    if (slot == SpellSlot.Summoner1 || slot == SpellSlot.Summoner2)
                    {
                        HeroSpellSprites[avaliblity.Key][slot].Draw(slot == SpellSlot.Summoner1 ? endPos : OffsetVector(endPos, 8, 0));

                        if(!spells.IsAvailable(slot)) Line.DrawLine(Color.FromArgb(180, Color.Black), 8, OffsetVector(endPos, slot == SpellSlot.Summoner1 ? 0 : 8, 2), OffsetVector(endPos, slot == SpellSlot.Summoner1 ? 8 : 16, 2));
                    }
                    else
                    {
                        var color = _availableLineColor;
                        if (!spells.IsLearned(slot))
                            color = _notLearnedeLineColor;
                        if (!spells.IsAvailable(slot))
                            color = _notAvailableLineColor;

                        Line.DrawLine(color, 4, OffsetVector(startPos, _spellTrackerWidth * slotIndex, 0),
                            OffsetVector(startPos, _spellTrackerWidth * slotIndex + _spellTrackerWidth * spells.CoolDownPercent(slot), 0));
                    }

                }
                
                TrackerHud.Draw(drawPos);
                if (hero.IsMe && (hero.Hero != Champion.Jhin)) Line.DrawLine(Color.FromArgb(74, 73, 74), 6, OffsetVector(startPos, 21, -6), OffsetVector(endPos, 15, -6));
            }

        }
        
    }
}