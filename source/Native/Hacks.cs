// <copyright file="Hacks.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     Core hacks.
    /// </summary>
    internal class Hacks
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the anti afk hack is enabled.
        /// </summary>
        public static bool AntiAfk
        {
            get
            {
                return LeagueSharp.Hacks.AntiAFK;
            }

            set
            {
                if (AntiAfk != value)
                {
                    LeagueSharp.Hacks.AntiAFK = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to disable the drawings.
        /// </summary>
        public static bool DisableDrawings
        {
            get
            {
                return LeagueSharp.Hacks.DisableDrawings;
            }

            set
            {
                if (DisableDrawings != value)
                {
                    LeagueSharp.Hacks.DisableDrawings = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the say function is disabled.
        /// </summary>
        public static bool DisableSay
        {
            get
            {
                return LeagueSharp.Hacks.DisableSay;
            }

            set
            {
                if (DisableSay != value)
                {
                    LeagueSharp.Hacks.DisableSay = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to show tower ranges.
        /// </summary>
        public static bool TowerRanges
        {
            get
            {
                return LeagueSharp.Hacks.TowerRanges;
            }

            set
            {
                if (TowerRanges != value)
                {
                    LeagueSharp.Hacks.TowerRanges = value;
                }
            }
        }

        #endregion

        #region Properties

        private static Menu Menu { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the hacks.
        /// </summary>
        /// <param name="menu">
        ///     The common menu.
        /// </param>
        public static void Initialize(Menu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu));
            }

            Menu = new Menu("Hacks", "Hacks");

            Menu.AddItem(CreateHackItem("antiafk", "Anti-AFK"));
            Menu.AddItem(CreateHackItem("disable_drawing", "Disable Drawing"));
            Menu.AddItem(CreateHackItem("disable_chat", "Disable L# Send Chat"));
            Menu.AddItem(CreateHackItem("show_tower_range", "Show Tower Range"));

            menu.AddSubMenu(Menu);
        }

        #endregion

        #region Methods

        private static MenuItem CreateHackItem(string name, string displayName)
        {
            var item = new MenuItem(name, displayName).SetValue(false);
            item.ValueChanged += OnValueChanged;
            return item;
        }

        private static void OnValueChanged(object sender, OnValueChangeEventArgs onValueChangeEventArgs)
        {
            var item = sender as MenuItem;
            if (item == null)
            {
                return;
            }

            switch (item.Name)
            {
                case "antiafk":
                    AntiAfk = onValueChangeEventArgs.GetNewValue<bool>();
                    break;
                case "disable_drawing":
                    DisableDrawings = onValueChangeEventArgs.GetNewValue<bool>();
                    break;
                case "disable_chat":
                    DisableSay = onValueChangeEventArgs.GetNewValue<bool>();
                    break;
                case "show_tower_range":
                    TowerRanges = onValueChangeEventArgs.GetNewValue<bool>();
                    break;
            }
        }

        #endregion
    }
}