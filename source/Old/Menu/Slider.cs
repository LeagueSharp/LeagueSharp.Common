namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     The menu slider.
    /// </summary>
    [Serializable]
    public struct Slider
    {
        #region Fields

        /// <summary>
        ///     The maximum value.
        /// </summary>
        public int MaxValue;

        /// <summary>
        ///     The minimum value.
        /// </summary>
        public int MinValue;

        /// <summary>
        ///     The value.
        /// </summary>
        private int value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Slider" /> struct.
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
        ///     Gets or sets the value.
        /// </summary>
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
    }
}