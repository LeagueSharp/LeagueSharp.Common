// <copyright file="Vector2Time.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     Vector2 time.
    /// </summary>
    public class Vector2Time
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2Time" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="time">
        ///     The time.
        /// </param>
        public Vector2Time(Vector2 position, float time)
        {
            this.Position = position;
            this.Time = time;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        public float Time { get; set; }

        #endregion
    }
}