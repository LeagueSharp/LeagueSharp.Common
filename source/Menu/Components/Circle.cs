// <copyright file="Circle.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Drawing;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The circle color spectrum (picker), with the toggle feature.
    /// </summary>
    [DataContract]
    public class Circle
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Circle" /> class.
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="Circle" /> class.
        /// </summary>
        public Circle()
            : this(false, default(Color))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the circle is enabled.
        /// </summary>
        [DataMember]
        public bool Active { get; set; }

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or sets the radius.
        /// </summary>
        public float Radius { get; set; }

        #endregion
    }
}