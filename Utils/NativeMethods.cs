namespace LeagueSharp.Common
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     The native methods.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        ///     Move event function.
        /// </summary>
        /// <param name="dwFlags">
        ///     The event flags.
        /// </param>
        /// <param name="dx">
        ///     The X-axis position.
        /// </param>
        /// <param name="dy">
        ///     The Y-axis position.
        /// </param>
        /// <param name="dwData">
        ///     The data.
        /// </param>
        /// <param name="dwExtraInfo">
        ///     The extra info.
        /// </param>
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        ///     Key event function.
        /// </summary>
        /// <param name="vk">
        ///     The virutal key.
        /// </param>
        /// <param name="scan">
        ///     The scan code.
        /// </param>
        /// <param name="flags">
        ///     The flags.
        /// </param>
        /// <param name="extrainfo">
        ///     The extra info.
        /// </param>
        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        /// <summary>
        ///     Set console mode.
        /// </summary>
        /// <param name="hConsoleHandle">
        ///     The console handle pointer.
        /// </param>
        /// <param name="mode">
        ///     The mode.
        /// </param>
        /// <returns>
        ///     Indicates whether the console mode retrival was successful.
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        /// <summary>
        ///     Get the console mode.
        /// </summary>
        /// <param name="hConsoleHandle">
        ///     The console handle pointer.
        /// </param>
        /// <param name="mode">
        ///     The mode.
        /// </param>
        /// <returns>
        ///     Indicates whether the console mode retrival was successful.
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);

        /// <summary>
        ///     Get the STD handle.
        /// </summary>
        /// <param name="handle">
        ///     The handle.
        /// </param>
        /// <returns>
        ///     The handle pointer.
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int handle);

        /// <summary>
        ///     Gets the virutal key state.
        /// </summary>
        /// <param name="virtualKeyCode">
        ///     The virtual key.
        /// </param>
        /// <returns>
        ///     The state.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);
    }
}