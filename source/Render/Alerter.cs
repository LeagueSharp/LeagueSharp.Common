// <copyright file="Alerter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    /// <summary>
    ///     The alerter, shows text for an amount of time.
    /// </summary>
    public class Alerter : Render.Text
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Alerter" /> class.
        /// </summary>
        /// <param name="x">
        ///     The X-axis of the position.
        /// </param>
        /// <param name="y">
        ///     The Y-axis of the position.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="size">
        ///     The size.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="faceName">
        ///     The face name.
        /// </param>
        /// <param name="duration">
        ///     The duration.
        /// </param>
        public Alerter(
            int x,
            int y,
            string text,
            int size,
            ColorBGRA color,
            string faceName = "Calibri",
            float duration = 1f)
            : base(x, y, text, size, color, faceName)
        {
            this.Duration = duration;
            this.StartTime = Utils.GameTimeTickCount;
            this.EndTime = this.StartTime + duration;

            Game.OnUpdate += this.OnUpdate;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the duration.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        ///     Gets the ending time.
        /// </summary>
        public float EndTime { get; }

        /// <summary>
        ///     Gets the start time.
        /// </summary>
        public float StartTime { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Removes the alerter.
        /// </summary>
        public void Remove()
        {
            this.Visible = false;
            this.Dispose();
        }

        #endregion

        #region Methods

        private void OnUpdate(EventArgs args)
        {
            if (Utils.GameTimeTickCount < this.EndTime)
            {
                return;
            }

            this.Remove();
        }

        #endregion
    }
}