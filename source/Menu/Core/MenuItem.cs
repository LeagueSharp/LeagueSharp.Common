// <copyright file="MenuItem.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;

    using SharpDX;
    using SharpDX.Menu;

    using Color = SharpDX.Color;

    /// <summary>
    ///     The menu item.
    /// </summary>
    public class MenuItem
    {
        #region Fields

        private bool showItem;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuItem" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="displayName">
        ///     The display name.
        /// </param>
        /// <param name="championUnique">
        ///     Indicates whether the menu item is champion unique.
        /// </param>
        public MenuItem(string name, string displayName, bool championUnique = false)
        {
            if (championUnique)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            this.MenuItemReference = ComponentFactory.CreateItem(name, displayName);
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The value changed event.
        /// </summary>
        public event EventHandler<OnValueChangeEventArgs> ValueChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.MenuItemReference.DisplayName;
            }

            set
            {
                this.MenuItemReference.DisplayName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the font color.
        /// </summary>
        public ColorBGRA FontColor { get; set; }

        /// <summary>
        ///     Gets or sets the font style.
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        ///     Gets or sets the menu font size.
        /// </summary>
        [Obsolete]
        public int MenuFontSize { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.MenuItemReference.Name;
            }

            set
            {
                // TODO: Log?
            }
        }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        public Menu Parent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to show the item.
        /// </summary>
        public bool ShowItem
        {
            get
            {
                return this.showItem;
            }

            set
            {
                this.Show(value);
            }
        }

        /// <summary>
        ///     Gets or sets the tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        ///     Gets or sets the tooltip.
        /// </summary>
        [Obsolete]
        public string Tooltip { get; set; }

        /// <summary>
        ///     Gets or sets the tooltip color.
        /// </summary>
        [Obsolete]
        public Color TooltipColor { get; set; }

        /// <summary>
        ///     Gets the tooltip duration.
        /// </summary>
        [Obsolete]
        public int TooltipDuration => 0;

        /// <summary>
        ///     Gets a value indicating whether the value was set.
        /// </summary>
        public bool ValueSet { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the menu item reference.
        /// </summary>
        internal IMenuItem MenuItemReference { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Sets the menu item to not save.
        /// </summary>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem DontSave()
        {
            this.MenuItemReference.IsSaveable = false;
            return this;
        }

        /// <summary>
        ///     Gets the item value.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The value.
        /// </returns>
        public T GetValue<T>()
        {
            return ((IMenuItem<T>)this.MenuItemReference).Value;
        }

        /// <summary>
        ///     Gets a value indicating whether the item is active.
        /// </summary>
        /// <returns>
        ///     Value indicating whether the item is active.
        /// </returns>
        public bool IsActive()
        {
            var type = this.MenuItemReference.ValueType;
            if (type == typeof(bool))
            {
                return this.GetValue<bool>();
            }

            if (type == typeof(Circle))
            {
                return this.GetValue<Circle>().Active;
            }

            if (type == typeof(KeyBind))
            {
                return this.GetValue<KeyBind>().Active;
            }

            if (type == typeof(SliderBool))
            {
                return this.GetValue<SliderBool>().IsActive;
            }

            return false;
        }

        /// <summary>
        ///     Sets the font style.
        /// </summary>
        /// <param name="fontStyle">
        ///     The font style.
        /// </param>
        /// <param name="fontColor">
        ///     The font color.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetFontStyle(FontStyle fontStyle = FontStyle.Regular, Color? fontColor = null)
        {
            this.FontStyle = fontStyle;

            var view = this.MenuItemReference.View;
            if (view != null)
            {
                view.SetAttribute(typeof(FontStyle).ToString(), fontStyle);
                if (fontColor.HasValue)
                {
                    var color = fontColor.Value;
                    this.FontColor = color;

                    var brush = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                    view.SetAttribute(typeof(Brush).ToString(), brush);
                }
            }

            return this;
        }

        /// <summary>
        ///     Sets the menu item to be shared.
        /// </summary>
        /// <returns>
        ///     The item instance.
        /// </returns>
        [Obsolete]
        public MenuItem SetShared()
        {
            return this;
        }

        /// <summary>
        ///     Sets the menu item tag.
        /// </summary>
        /// <param name="tag">
        ///     The tag.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        [Obsolete("Use Tag Property.")]
        public MenuItem SetTag(int tag = 0)
        {
            this.Tag = tag;
            return this;
        }

        /// <summary>
        ///     Sets the tooltip.
        /// </summary>
        /// <param name="tooltip">
        ///     The tooltip string.
        /// </param>
        /// <param name="tooltipColor">
        ///     The tooltip color.
        /// </param>
        /// <returns>
        ///     The menu instance.
        /// </returns>
        [Obsolete]
        public MenuItem SetTooltip(string tooltip, Color? tooltipColor = null)
        {
            return this;
        }

        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="newValue">
        ///     The new value.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetValue<T>(T newValue)
        {
            if (!this.ValueSet)
            {
                this.ValueSet = true;
                this.MenuItemReference = ComponentFactory.CreateItem(this.Name, this.DisplayName, newValue);
                return this;
            }

            var oldObject = this.MenuItemReference as IMenuItem<T>;
            var oldValue = oldObject != null ? oldObject.Value : default(T);

            this.MenuItemReference = new MenuItem<T>(this.Name, this.DisplayName, newValue);
            this.ValueChanged?.Invoke(this, new OnValueChangeEventArgs(oldValue, newValue) { MenuItemReference = this });

            return this;
        }

        /// <summary>
        ///     Shows the item.
        /// </summary>
        /// <param name="flag">
        ///     Indicates whether to show the item.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem Show(bool flag = true)
        {
            this.MenuItemReference.IsVisible = flag;
            this.showItem = flag;
            return this;
        }

        /// <summary>
        ///     Shows the tooltip.
        /// </summary>
        /// <param name="hide">
        ///     A value indicating whether to hide or show the tooltip.
        /// </param>
        [Obsolete]
        public void ShowTooltip(bool hide = false)
        {
        }

        /// <summary>
        ///     Shows the tooltip notification.
        /// </summary>
        [Obsolete]
        public void ShowTooltipNotification()
        {
        }

        /// <summary>
        ///     Gets the item value, safely.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The value.
        /// </returns>
        public T TryGetValue<T>()
        {
            var item = this.MenuItemReference as IMenuItem<T>;
            return item != null ? item.Value : default(T);
        }

        /// <summary>
        ///     Gets a value indicating if the value is of item type.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="bool" /> indicating whether the value is of type.
        /// </returns>
        public bool TypeOf<T>()
        {
            return this.MenuItemReference is IMenuItem<T>;
        }

        #endregion
    }
}