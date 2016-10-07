namespace LeagueSharp.Common
{
    using System;

    using LeagueSharp.Common.Properties;

    using SharpDX;

    /// <summary>
    ///     The color picker slider.
    /// </summary>
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

        /// <summary>
        ///     The active sprite.
        /// </summary>
        internal Render.Sprite ActiveSprite;

        /// <summary>
        ///     The inactive sprite.
        /// </summary>
        internal Render.Sprite InactiveSprite;

        /// <summary>
        ///     The X-axis position.
        /// </summary>
        private readonly int xPos;

        /// <summary>
        ///     The Y-axis position.
        /// </summary>
        private readonly int yPos;

        /// <summary>
        ///     Indicates whether the slider is visible.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        ///     The percent.
        /// </summary>
        private float percent;

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
            this.xPos = x;
            this.yPos = y;
            this.Height = height - Resources.CPActiveSlider.Height;
            this.percent = percent;

            this.ActiveSprite = new Render.Sprite(Resources.CPActiveSlider, new Vector2(this.X, this.Y));
            this.InactiveSprite = new Render.Sprite(Resources.CPInactiveSlider, new Vector2(this.X, this.Y));

            this.ActiveSprite.Add(2);
            this.InactiveSprite.Add(2);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the percent.
        /// </summary>
        public float Percent
        {
            get
            {
                return this.percent;
            }

            set
            {
                this.percent = Math.Max(0, Math.Min(1, value));
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the slider is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.ActiveSprite.Visible = this.InactiveSprite.Visible = this.isVisible = value;
            }
        }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        public int Width
        {
            get
            {
                return Resources.CPActiveSlider.Width;
            }
        }

        /// <summary>
        ///     Gets or sets the X.
        /// </summary>
        public int X
        {
            get
            {
                return this.xPos + ColorPicker.X;
            }

            set
            {
                this.ActiveSprite.X = this.InactiveSprite.X = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Y.
        /// </summary>
        public int Y
        {
            get
            {
                return this.yPos + ColorPicker.Y;
            }

            set
            {
                this.ActiveSprite.Y = this.InactiveSprite.Y = value + (int)(this.percent * this.Height);
            }
        }

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
            switch (args.Msg)
            {
                case WindowsMessages.WM_LBUTTONDOWN:
                    if (Utils.IsUnderRectangle(
                        Utils.GetCursorPos(),
                        this.X,
                        this.Y,
                        this.Width,
                        this.Height + Resources.CPActiveSlider.Height))
                    {
                        this.ActiveSprite.Visible = this.Moving = true;
                        this.InactiveSprite.Visible = false;
                        this.UpdatePercent();
                    }
                    break;
                case WindowsMessages.WM_MOUSEMOVE:
                    if (this.Moving)
                    {
                        this.UpdatePercent();
                    }
                    break;
                case WindowsMessages.WM_LBUTTONUP:
                    this.ActiveSprite.Visible = this.Moving = false;
                    this.InactiveSprite.Visible = true;
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates the percent.
        /// </summary>
        private void UpdatePercent()
        {
            this.Percent = (Utils.GetCursorPos().Y - (Resources.CPActiveSlider.Height / 2f) - this.Y) / this.Height;
            ColorPicker.UpdateColor();
            this.ActiveSprite.Y = this.InactiveSprite.Y = this.Y + (int)(this.percent * this.Height);
        }

        #endregion
    }
}