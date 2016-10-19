// <copyright file="MenuController.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Configuration
{
    using System.ComponentModel.Composition;

    using SharpDX;
    using SharpDX.Menu;

    /// <summary>
    ///     The menu controller.
    /// </summary>
    [Export(typeof(IMenuController))]
    public class MenuController : IMenuController
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuController" /> class.
        /// </summary>
        public MenuController()
        {
            Game.OnWndProc += this.OnWndProc;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public float ComponentHeight { get; } = 32;

        /// <inheritdoc />
        public float ComponentWidth { get; } = 180;

        /// <inheritdoc />
        public bool IsVisible { get; private set; } = true;

        /// <inheritdoc />
        public Vector2 Position { get; } = new Vector2(10, 10);

        #endregion

        #region Methods

        private void OnWndProc(WndEventArgs args)
        {
            if ((args.Msg == (uint)WindowsMessages.WM_KEYUP || args.Msg == (uint)WindowsMessages.WM_KEYDOWN)
                && args.WParam == Config.ShowMenuPressKey)
            {
                this.IsVisible = args.Msg == (uint)WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == (uint)WindowsMessages.WM_KEYUP && args.WParam == Config.ShowMenuToggleKey)
            {
                this.IsVisible = !this.IsVisible;
            }
        }

        #endregion
    }
}