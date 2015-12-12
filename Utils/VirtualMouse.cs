namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    public static class VirtualMouse
    {
        #region Static Fields

        /// <summary>
        ///     The X-axis coord.
        /// </summary>
        public static int CoordX;

        /// <summary>
        ///     The Y-axis coord.
        /// </summary>
        public static int CoordY;

        #endregion

        #region Public Properties

        [Obsolete("Alias and marked to removal, use CoordX.")]
        public static int coordX
        {
            get
            {
                return CoordX;
            }

            set
            {
                CoordX = value;
            }
        }

        [Obsolete("Alias and marked to removal, use CoordX.")]
        public static int coordY
        {
            get
            {
                return CoordY;
            }

            set
            {
                CoordY = value;
            }
        }

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