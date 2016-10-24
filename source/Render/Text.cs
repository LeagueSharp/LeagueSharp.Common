// <copyright file="Text.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using SharpDX;
    using SharpDX.Direct3D9;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     Text Render Object, used to draw text onto the screen.
        /// </summary>
        public class Text : RenderObject
        {
            #region Fields

            private string content;

            private int x;

            private int xCalcualted;

            private int y;

            private int yCalculated;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Text" /> class.
            /// </summary>
            /// <param name="text">
            ///     The text.
            /// </param>
            /// <param name="x">
            ///     The X-axis.
            /// </param>
            /// <param name="y">
            ///     The Y-axis.
            /// </param>
            /// <param name="size">
            ///     The size.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="fontName">
            ///     The font name.
            /// </param>
            public Text(string text, int x, int y, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                this.x = x;
                this.y = y;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Text" /> class.
            /// </summary>
            /// <param name="text">
            ///     The text.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            /// <param name="size">
            ///     The size.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="fontName">
            ///     The font name.
            /// </param>
            public Text(string text, Vector2 position, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                this.x = (int)position.X;
                this.y = (int)position.Y;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Text" /> class.
            /// </summary>
            /// <param name="text">
            ///     The text.
            /// </param>
            /// <param name="unit">
            ///     The unit.
            /// </param>
            /// <param name="offset">
            ///     The offset.
            /// </param>
            /// <param name="size">
            ///     The size.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="fontName">
            ///     The font name.
            /// </param>
            public Text(
                string text,
                Obj_AI_Base unit,
                Vector2 offset,
                int size,
                ColorBGRA color,
                string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                this.Unit = unit;
                this.Offset = offset;

                var pos = unit.HPBarPosition + offset;
                this.x = (int)pos.X;
                this.y = (int)pos.Y;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Text" /> class.
            /// </summary>
            /// <param name="x">
            ///     The X-axis.
            /// </param>
            /// <param name="y">
            ///     The Y-axis.
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
            /// <param name="fontName">
            ///     The font name.
            /// </param>
            public Text(int x, int y, string text, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                this.x = x;
                this.y = y;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Text" /> class.
            /// </summary>
            /// <param name="position">
            ///     The position.
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
            /// <param name="fontName">
            ///     The font name.
            /// </param>
            public Text(Vector2 position, string text, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                this.x = (int)position.X;
                this.y = (int)position.Y;
            }

            private Text(string text, string fontName, int size, ColorBGRA color)
            {
                const FontPrecision OpDefault = FontPrecision.Default;
                const FontQuality QDefault = FontQuality.Default;

                var fontDesc = new FontDescription
                                   {
                                       FaceName = fontName, Height = size, OutputPrecision = OpDefault,
                                       Quality = QDefault
                                   };
                this.Font = new Font(Device, fontDesc);
                this.Color = color;
                this.Content = text;

                Game.OnUpdate += this.OnUpdate;
                this.SubscribeToResetEvents();
            }

            #endregion

            #region Delegates

            /// <summary>
            ///     The position delegate.
            /// </summary>
            /// <returns>
            ///     The <see cref="Vector2" />.
            /// </returns>
            public delegate Vector2 PositionDelegate();

            /// <summary>
            ///     The text delegate.
            /// </summary>
            /// <returns>
            ///     The <see cref="string" />.
            /// </returns>
            public delegate string TextDelegate();

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets a value indicating whether the text is centered.
            /// </summary>
            public bool Centered { get; set; }

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            public ColorBGRA Color { get; set; }

            /// <summary>
            ///     Gets or sets the content.
            /// </summary>
            public string Content
            {
                get
                {
                    return this.content;
                }

                set
                {
                    if (value != this.content && (!this.Font?.IsDisposed ?? false) && !string.IsNullOrEmpty(value))
                    {
                        var size = this.Font?.MeasureText(null, value, 0) ?? default(SharpDX.Rectangle);
                        this.Width = size.Width;
                        this.Height = size.Height;
                        this.Font?.PreloadText(value);
                    }

                    this.content = value;
                }
            }

            /// <summary>
            ///     Gets the height.
            /// </summary>
            public int Height { get; private set; }

            /// <summary>
            ///     Gets or sets the offset.
            /// </summary>
            public Vector2 Offset { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the text is outlined.
            /// </summary>
            public bool OutLined { get; set; }

            /// <summary>
            ///     Gets or sets the position update.
            /// </summary>
            public PositionDelegate PositionUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the text.
            /// </summary>
            [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Old API Compability.")]
            [Obsolete("Use Content property.")]
#pragma warning disable SA1300 // Element must begin with upper-case letter
            public string text
#pragma warning restore SA1300 // Element must begin with upper-case letter
            {
                get
                {
                    return this.Content;
                }

                set
                {
                    this.Content = value;
                }
            }

            /// <summary>
            ///     Gets or sets the text font description.
            /// </summary>
            public FontDescription TextFontDescription
            {
                get
                {
                    return this.Font.Description;
                }

                set
                {
                    this.Font.Dispose();
                    this.Font = new Font(Device, value);
                }
            }

            /// <summary>
            ///     Gets or sets the text update.
            /// </summary>
            public TextDelegate TextUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the unit.
            /// </summary>
            public Obj_AI_Base Unit { get; set; }

            /// <summary>
            ///     Gets the width.
            /// </summary>
            public int Width { get; private set; }

            /// <summary>
            ///     Gets or sets the X-Axis of the postiion.
            /// </summary>
            public int X
            {
                get
                {
                    return this.PositionUpdate != null ? this.xCalcualted : this.x + this.XOffset;
                }

                set
                {
                    this.x = value;
                }
            }

            /// <summary>
            ///     Gets or sets the Y-Axis of the position.
            /// </summary>
            public int Y
            {
                get
                {
                    return this.PositionUpdate != null ? this.yCalculated : this.y + this.YOffset;
                }

                set
                {
                    this.y = value;
                }
            }

            #endregion

            #region Properties

            private Font Font { get; set; }

            private int XOffset => this.Centered ? -this.Width / 2 : 0;

            private int YOffset => this.Centered ? -this.Height / 2 : 0;

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public override void OnEndScene()
            {
                try
                {
                    if ((this.Font == null || this.Font.IsDisposed) || string.IsNullOrEmpty(this.content))
                    {
                        return;
                    }

                    if (this.Unit != null && this.Unit.IsValid)
                    {
                        var pos = this.Unit.HPBarPosition + this.Offset;
                        this.X = (int)pos.X;
                        this.Y = (int)pos.Y;
                    }

                    var xP = this.X;
                    var yP = this.Y;

                    if (this.OutLined)
                    {
                        var outlineColor = new ColorBGRA(0, 0, 0, 255);
                        this.Font?.DrawText(null, this.Content, xP - 1, yP - 1, outlineColor);
                        this.Font?.DrawText(null, this.Content, xP + 1, yP + 1, outlineColor);
                        this.Font?.DrawText(null, this.Content, xP - 1, yP, outlineColor);
                        this.Font?.DrawText(null, this.Content, xP + 1, yP, outlineColor);
                    }

                    this.Font?.DrawText(null, this.Content, xP, yP, this.Color);
                }
                catch (Exception e)
                {
                    this.Log.Error("Render Text Error.", e);
                }
            }

            /// <inheritdoc />
            public override void OnPostReset()
            {
                this.Font.OnResetDevice();
            }

            /// <inheritdoc />
            public override void OnPreReset()
            {
                this.Font.OnLostDevice();
            }

            #endregion

            #region Methods

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    this.Font?.Dispose();
                }

                Game.OnUpdate -= this.OnUpdate;
            }

            private void OnUpdate(EventArgs args)
            {
                if (this.Visible)
                {
                    if (this.TextUpdate != null)
                    {
                        this.Content = this.TextUpdate();
                    }

                    if (this.PositionUpdate != null && !string.IsNullOrEmpty(this.Content))
                    {
                        var pos = this.PositionUpdate();
                        this.xCalcualted = (int)pos.X + this.XOffset;
                        this.yCalculated = (int)pos.Y + this.YOffset;
                    }
                }
            }

            #endregion
        }
    }
}