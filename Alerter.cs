namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    /// <summary>
    ///     Shows text for an amount of time.
    /// </summary>
    public class Alerter : Render.Text
    {
        #region Fields

        private readonly float _duration;

        private readonly float _endTime;

        private readonly float _startTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Alerter" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="text">The text.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        /// <param name="faceName">Name of the face.</param>
        /// <param name="duration">The duration.</param>
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
            this._duration = duration;
            this._startTime = Utils.TickCount;
            this._endTime = this._startTime + this._duration;

            Game.OnUpdate += this.Game_OnGameUpdate;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the duration.
        /// </summary>
        /// <value>
        ///     The duration.
        /// </value>
        public float Duration
        {
            get
            {
                return this._duration;
            }
        }

        /// <summary>
        ///     Gets the end time.
        /// </summary>
        /// <value>
        ///     The end time.
        /// </value>
        public float EndTime
        {
            get
            {
                return this._endTime;
            }
        }

        /// <summary>
        ///     Gets the start time.
        /// </summary>
        /// <value>
        ///     The start time.
        /// </value>
        public float StartTime
        {
            get
            {
                return this._startTime;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Removes this instance.
        /// </summary>
        public void Remove()
        {
            this.Visible = false;
            this.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!(Utils.TickCount > this.EndTime))
            {
                return;
            }

            this.Visible = false;
            this.Dispose();
        }

        #endregion
    }
}