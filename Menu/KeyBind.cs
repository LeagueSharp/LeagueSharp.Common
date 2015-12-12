namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     The menu keybind.
    /// </summary>
    [Serializable]
    public struct KeyBind
    {
        #region Fields

        /// <summary>
        ///     Indicates whether the keybind is active.
        /// </summary>
        public bool Active;

        /// <summary>
        ///     The key.
        /// </summary>
        public uint Key;

        /// <summary>
        ///     The key bind type.
        /// </summary>
        public KeyBindType Type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyBind" /> struct.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="defaultValue">
        ///     The default value.
        /// </param>
        public KeyBind(uint key, KeyBindType type, bool defaultValue = false)
        {
            this.Key = key;
            this.Type = type;
            this.Active = defaultValue;
        }

        #endregion
    }
}