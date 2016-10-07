// <copyright file="Cursor.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The cursor tracker class.
    /// </summary>
    internal class Cursor
    {
        #region Static Fields

        /// <summary>
        ///     The cursor X-axis pos.
        /// </summary>
        private static int posX;

        /// <summary>
        ///     The cursor Y-axis pos.
        /// </summary>
        private static int posY;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Cursor" /> class.
        /// </summary>
        static Cursor()
        {
            Game.OnWndProc += args => OnWndProc(new WndEventComposition(args));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the cursor current position on screen.
        /// </summary>
        /// <returns>
        ///     The cursor current position.
        /// </returns>
        internal static Vector2 GetCursorPos()
        {
            return new Vector2(posX, posY);
        }

        /// <summary>
        ///     The windows process messages event.
        /// </summary>
        /// <param name="args">
        ///     The arguments.
        /// </param>
        private static void OnWndProc(WndEventComposition args)
        {
            if (args.Msg == WindowsMessages.WM_MOUSEMOVE)
            {
                unchecked
                {
                    posX = (short)args.LParam;
                    posY = (short)((long)args.LParam >> 16);
                }
            }
        }

        #endregion
    }
}