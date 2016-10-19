// <copyright file="Slider.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Runtime.Serialization;

    using SharpDX.Menu;

    /// <summary>
    ///     The menu slider.
    /// </summary>
    [DataContract]
    public class Slider : IUpdateableValue<Slider>
    {
        #region Fields

        /// <summary>
        ///     The value.
        /// </summary>
        private int value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Slider" /> class.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="minValue">
        ///     The minimum value.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum value.
        /// </param>
        public Slider(int value = 0, int minValue = 0, int maxValue = 100)
        {
            this.value = value;
            this.MaxValue = Math.Max(maxValue, minValue);
            this.MinValue = Math.Min(maxValue, minValue);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the maximum value.
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the minimum value.
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        [DataMember]
        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = Math.Min(Math.Max(value, this.MinValue), this.MaxValue);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void Update(Slider newValue)
        {
            this.Value = newValue.Value;
        }

        #endregion
    }
}