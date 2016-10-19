// <copyright file="KeyBind.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Runtime.Serialization;

    /// <summary>
    ///     The menu keybind.
    /// </summary>
    [DataContract]
    public class KeyBind
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyBind" /> class.
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
            this.SecondaryKey = 0;
            this.Type = type;
            this.Active = defaultValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyBind" /> class.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="secondaryKey">
        ///     The secondary key.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="defaultValue">
        ///     The default value.
        /// </param>
        public KeyBind(uint key, uint secondaryKey, KeyBindType type, bool defaultValue = false)
        {
            this.Key = key;
            this.SecondaryKey = secondaryKey;
            this.Type = type;
            this.Active = defaultValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyBind" /> class.
        /// </summary>
        public KeyBind()
            : this(0, KeyBindType.Press)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the keybind is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        [DataMember]
        public uint Key { get; set; }

        /// <summary>
        ///     Gets or sets the secondary key.
        /// </summary>
        [DataMember]
        public uint SecondaryKey { get; set; }

        /// <summary>
        ///     Gets or sets the key bind type.
        /// </summary>
        public KeyBindType Type { get; set; }

        #endregion
    }
}