namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;

    /// <summary>
    ///     The HSL Color class.
    /// </summary>
    public class HSLColor
    {
        #region Constants

        /// <summary>
        ///     The Scale.
        /// </summary>
        private const double Scale = 100.0;

        #endregion

        #region Fields

        /// <summary>
        ///     The hue.
        /// </summary>
        private double hue = 1.0;

        /// <summary>
        ///     The luminosity.
        /// </summary>
        private double luminosity = 1.0;

        /// <summary>
        ///     The saturation.
        /// </summary>
        private double saturation = 1.0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HSLColor" /> class.
        /// </summary>
        public HSLColor()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HSLColor" /> class.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        public HSLColor(Color color)
        {
            this.SetRGB(color.R, color.G, color.B);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HSLColor" /> class.
        /// </summary>
        /// <param name="red">
        ///     The red component.
        /// </param>
        /// <param name="green">
        ///     The green component.
        /// </param>
        /// <param name="blue">
        ///     The blue component.
        /// </param>
        public HSLColor(int red, int green, int blue)
        {
            this.SetRGB(red, green, blue);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HSLColor" /> class.
        /// </summary>
        /// <param name="hue">
        ///     The hue.
        /// </param>
        /// <param name="saturation">
        ///     The saturation.
        /// </param>
        /// <param name="luminosity">
        ///     The luminosity.
        /// </param>
        public HSLColor(double hue, double saturation, double luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the hue.
        /// </summary>
        public double Hue
        {
            get
            {
                return this.hue * Scale;
            }

            set
            {
                this.hue = CheckRange(value / Scale);
            }
        }

        /// <summary>
        ///     Gets or sets the luminosity.
        /// </summary>
        public double Luminosity
        {
            get
            {
                return this.luminosity * Scale;
            }

            set
            {
                this.luminosity = CheckRange(value / Scale);
            }
        }

        /// <summary>
        ///     Gets or sets the saturation.
        /// </summary>
        public double Saturation
        {
            get
            {
                return this.saturation * Scale;
            }

            set
            {
                this.saturation = CheckRange(value / Scale);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs an implicit conversion from <see cref="HSLColor" /> to <see cref="Color" />.
        /// </summary>
        /// <param name="hslColor">
        ///     Color of the HSL.
        /// </param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (Math.Abs(hslColor.luminosity) > float.Epsilon)
            {
                if (Math.Abs(hslColor.saturation) < float.Epsilon)
                {
                    r = g = b = hslColor.luminosity;
                }
                else
                {
                    var temp2 = GetTemp2(hslColor);
                    var temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }

            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Color" /> to <see cref="HSLColor" />.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator HSLColor(Color color)
        {
            return new HSLColor
                       {
                           hue = color.GetHue() / 360.0, luminosity = color.GetBrightness(),
                           saturation = color.GetSaturation()
                       };
        }

        /// <summary>
        ///     Sets the RGB.
        /// </summary>
        /// <param name="red">
        ///     The red component.
        /// </param>
        /// <param name="green">
        ///     The green component.
        /// </param>
        /// <param name="blue">
        ///     The blue component.
        /// </param>
        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        /// <summary>
        ///     Returns a string that represents the current object in RGB.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object in RGB.
        /// </returns>
        public string ToRGBString()
        {
            Color color = this;
            return string.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", this.Hue, this.Saturation, this.Luminosity);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks the range.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The range.
        /// </returns>
        private static double CheckRange(double value)
        {
            if (value < 0.0)
            {
                value = 0.0;
            }
            else if (value > 1.0)
            {
                value = 1.0;
            }
            return value;
        }

        /// <summary>
        ///     Gets the color component.
        /// </summary>
        /// <param name="temp1">
        ///     The temp1.
        /// </param>
        /// <param name="temp2">
        ///     The temp2.
        /// </param>
        /// <param name="temp3">
        ///     The temp3.
        /// </param>
        /// <returns>
        ///     The color component.
        /// </returns>
        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);

            return (temp3 < 1.0 / 6.0)
                       ? temp1 + (temp2 - temp1) * 6.0 * temp3
                       : (temp3 < 0.5)
                             ? temp2
                             : (temp3 < 2.0 / 3.0) ? temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0) : temp1;
        }

        /// <summary>
        ///     Gets the temp2.
        /// </summary>
        /// <param name="hslColor">
        ///     Color of the HSL.
        /// </param>
        /// <returns>
        ///     The temp2.
        /// </returns>
        private static double GetTemp2(HSLColor hslColor)
        {
            return (hslColor.luminosity < 0.5)
                       ? hslColor.luminosity * (1.0 + hslColor.saturation)
                       : hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
        }

        /// <summary>
        ///     Moves the into range.
        /// </summary>
        /// <param name="temp3">
        ///     The temp3.
        /// </param>
        /// <returns>
        ///     The in range temp3.
        /// </returns>
        private static double MoveIntoRange(double temp3)
        {
            return temp3 < 0.0 ? temp3 + 1.0 : temp3 > 1.0 ? temp3 - 1.0 : temp3;
        }

        #endregion
    }
}