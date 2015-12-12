namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;

    /// <summary>
    ///     The circle color spectrum (picker), with the toggle feature.
    /// </summary>
    [Serializable]
    public struct Circle
    {
        #region Fields

        /// <summary>
        ///     Indicates whether the circle is enabled.
        /// </summary>
        public bool Active;

        /// <summary>
        ///     The color.
        /// </summary>
        public Color Color;

        /// <summary>
        ///     The radius.
        /// </summary>
        public float Radius;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Circle" /> struct.
        /// </summary>
        /// <param name="active">
        ///     Indicates whether the circle is active.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="radius">
        ///     The radius.
        /// </param>
        public Circle(bool active, Color color, float radius = 100)
        {
            this.Active = active;
            this.Color = color;
            this.Radius = radius;
        }

        #endregion
    }
}