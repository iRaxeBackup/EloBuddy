using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu;
using OKTRAIO.Menu_Settings;

namespace OKTRAIO.Utility
{
    public abstract class UtilityAddon
    {
        /// <summary>
        /// The menu associated towards this Utility.
        /// </summary>
        public readonly Menu Menu;
        /// <summary>
        /// Gets the information of this Utility such as Name, ID, Author, etc.
        /// </summary>
        /// <returns></returns>
        public abstract UtilityInfo GetUtilityInfo();

        /// <summary>
        /// Constructs a <see cref="UtilityAddon"/> using the parent menu supplied. Will not call Initialization if the champion is not supported.
        /// </summary>
        /// <param name="menu">The parent menu that this Utility's menu will be added to. Pass 'null' if this utility does not need a menu.</param>
        /// <param name="champion">The players champion name (If needed)</param>
        protected UtilityAddon(Menu menu, Champion? champion = null)
        {
            if (champion != null && !CheckSupported(champion.Value)) return;

            if (menu != null)
            {
                Menu = MakeMenu(menu);
                Value.MenuList.Add(Menu);
            }
            Init();
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Game.OnTick += Game_OnTick;
            
        }

        private bool CheckSupported(Champion champion)
        {
            var ui = GetUtilityInfo();
            return ui.RequiredChampions.Any(c => c == champion);
        }

        /// <summary>
        /// Global Initialization, private so you can not reload the menu.
        /// </summary>
        private void Init()
        {
            InitializeMenu();
            Initialize();
        }
        /// <summary>
        /// Creates a sub-menu using the <see cref="UtilityInfo"/> associated to this <see cref="UtilityAddon"/>
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private Menu MakeMenu(Menu menu)
        {
            return menu.AddSubMenu(GetUtilityInfo().Name, GetUtilityInfo().Uid);
        }
        /// <summary>
        /// Initializes the menu, create your controls etc. here. If you do not need a menu, leave this blank.
        /// </summary>
        protected abstract void InitializeMenu();
        /// <summary>
        /// Call to reload the utility, will not reload the menu.
        /// </summary>
        public abstract void Initialize();

        protected virtual void Game_OnUpdate(EventArgs args)
        {
            
        }
        protected virtual void Drawing_OnDraw(EventArgs args)
        {

        }
        protected virtual void Drawing_OnEndScene(EventArgs args)
        {

        }
        protected virtual void Game_OnTick(EventArgs args)
        {
            
        }
        
    }

    public struct UtilityInfo
    {
        /// <summary>
        /// Returns the name of the <see cref="UtilityAddon"/>.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Returns the name of the <see cref="UtilityAddon"/>.
        /// </summary>
        public readonly string Uid;
        /// <summary>
        /// Returns the <see cref="UtilityAddon"/>'s Author.
        /// </summary>
        public readonly string Author;
        public readonly Champion[] RequiredChampions;

        /// <summary>
        /// Returns if the <see cref="UtilityAddon"/> supports all champions.
        /// </summary>
        public readonly bool SupportsAllChampions;

        public readonly UtilityAddon Owner;

        /// <summary>
        /// Creates a new instance of <see cref="UtilityInfo"/> and associates it to a <see cref="UtilityAddon"/>.
        /// </summary>
        /// <param name="owner">The owner of this object if creating within 'UtilityAddon.GetUtilityInfo()' use "this" to specify the current object.</param>
        /// <param name="name">The name</param>
        /// <param name="uid">A Unique id</param>
        /// <param name="author">The author (Who made this Utility)</param>
        /// <param name="requiredChampions">An array of supported champions, if every champion is supported pass "null" or no value</param>
        public UtilityInfo(UtilityAddon owner, string name, string uid, string author, params Champion[] requiredChampions)
        {
            Uid = uid;
            Owner = owner;
            Name = name;
            Author = author;
            SupportsAllChampions = requiredChampions.Length > 0;
            RequiredChampions = requiredChampions;
        }
    }
}