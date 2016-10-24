namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     The color picker slider.
    /// </summary>
    [Obsolete]
    public class CPSlider
    {
        #region Fields

        /// <summary>
        ///     The height.
        /// </summary>
        public int Height;

        /// <summary>
        ///     Indicates whether the slider is moving.
        /// </summary>
        public bool Moving;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CPSlider" /> class.
        /// </summary>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="height">
        ///     The Height.
        /// </param>
        /// <param name="percent">
        ///     The Percent.
        /// </param>
        public CPSlider(int x, int y, int height, float percent = 1)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the percent.
        /// </summary>
        public float Percent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the slider is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        public int Width => 0;

        /// <summary>
        ///     Gets or sets the X.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets the Y.
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The windows event process message event.
        /// </summary>
        /// <param name="args">
        ///     The arguments.
        /// </param>
        public void OnWndProc(WndEventComposition args)
        {
        }

        #endregion
    }
}