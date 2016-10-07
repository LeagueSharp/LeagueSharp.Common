// <copyright file="VirtualMouse.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The virtual mouse.
    /// </summary>
    public static class VirtualMouse
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the X-axis coord.
        /// </summary>
        public static int CoordX { get; set; }

        /// <summary>
        ///     Gets or sets the Y-axis coord.
        /// </summary>
        public static int CoordY { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Virtual right click.
        /// </summary>
        public static void RightClick()
        {
            RightClick(new Vector2(CoordX, CoordY));
        }

        /// <summary>
        ///     Virtual right click.
        /// </summary>
        /// <param name="position">
        ///     The screen position.
        /// </param>
        public static void RightClick(Vector2 position)
        {
            NativeMethods.mouse_event(0, (int)position.X, (int)position.Y, 0, 0);
            NativeMethods.mouse_event(0, (int)position.X, (int)position.Y, 0, 0);
        }

        /// <summary>
        ///     Virtual right click.
        /// </summary>
        /// <param name="position">
        ///     The game position.
        /// </param>
        public static void RightClick(Vector3 position)
        {
            RightClick(Drawing.WorldToScreen(position));
        }

        /// <summary>
        ///     Virtual shift click.
        /// </summary>
        public static void ShiftClick()
        {
            ShiftClick(new Vector2(CoordX, CoordY));
        }

        /// <summary>
        ///     Virtual shift click.
        /// </summary>
        /// <param name="position">
        ///     The screen position.
        /// </param>
        public static void ShiftClick(Vector2 position)
        {
            NativeMethods.keybd_event(
                (int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL,
                (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE,
                (int)KeyboardEvents.KEYBDEVENTF_KEYDOWN,
                0);
            RightClick(position);
            Utility.DelayAction.Add(
                200,
                () =>
                    {
                        NativeMethods.keybd_event(
                            (int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL,
                            (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE,
                            (int)KeyboardEvents.KEYBDEVENTF_KEYUP,
                            0);
                    });
        }

        /// <summary>
        ///     Virtual shift click.
        /// </summary>
        /// <param name="position">
        ///     The game position.
        /// </param>
        public static void ShiftClick(Vector3 position)
        {
            ShiftClick(Drawing.WorldToScreen(position));
        }

        #endregion
    }
}