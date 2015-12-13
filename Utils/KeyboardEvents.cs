namespace LeagueSharp.Common
{
    /// <summary>
    ///     The keyboard events.
    /// </summary>
    public enum KeyboardEvents
    {
        /// <summary>
        ///     The key down event.
        /// </summary>
        KEYBDEVENTF_KEYDOWN = 0x0,

        /// <summary>
        ///     The key up event.
        /// </summary>
        KEYBDEVENTF_KEYUP = 0x2,

        /// <summary>
        ///     The shift virtual event.
        /// </summary>
        KEYBDEVENTF_SHIFTVIRTUAL = 0x10,

        /// <summary>
        ///     The shift scan code event.
        /// </summary>
        KEYBDEVENTF_SHIFTSCANCODE = 0x2a,
    }
}