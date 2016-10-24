// <copyright file="Rectangle.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     Draws a rectangle.
        /// </summary>
        public class Rectangle : RenderObject
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Rectangle" /> class.
            /// </summary>
            /// <param name="x">
            ///     The X-axis of the position.
            /// </param>
            /// <param name="y">
            ///     The Y-axis of the position.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="height">
            ///     The height.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            public Rectangle(int x, int y, int width, int height, ColorBGRA color)
            {
                this.DeviceLine = new SharpDX.Direct3D9.Line(Device) { Width = height };

                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
                this.Color = color;

                Game.OnUpdate += this.OnUpdate;
                this.SubscribeToResetEvents();
            }

            #endregion

            #region Delegates

            /// <summary>
            ///     The position update delegate.
            /// </summary>
            /// <returns>
            ///     The <see cref="Vector2" />.
            /// </returns>
            public delegate Vector2 PositionDelegate();

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            public ColorBGRA Color { get; set; }

            /// <summary>
            ///     Gets or sets the height.
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            ///     Gets or sets the position update.
            /// </summary>
            public PositionDelegate PositionUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the width.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            ///     Gets or sets the X-axis of the position.
            /// </summary>
            public int X { get; set; }

            /// <summary>
            ///     Gets or sets the Y-axis of the position.
            /// </summary>
            public int Y { get; set; }

            #endregion

            #region Properties

            private SharpDX.Direct3D9.Line DeviceLine { get; }

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public override void OnEndScene()
            {
                if (this.DeviceLine == null || this.DeviceLine.IsDisposed)
                {
                    return;
                }

                try
                {
                    this.DeviceLine.Begin();
                    this.DeviceLine.Draw(
                        new[]
                            {
                                new Vector2(this.X, this.Y + (this.Height / 2)),
                                new Vector2(this.X + this.Width, this.Y + (this.Height / 2))
                            },
                        this.Color);
                    this.DeviceLine.End();
                }
                catch (Exception e)
                {
                    this.Log.Error("Unable to draw a rectangle.", e);
                }
            }

            /// <inheritdoc />
            public override void OnPostReset() => this.DeviceLine.OnResetDevice();

            /// <inheritdoc />
            public override void OnPreReset() => this.DeviceLine.OnLostDevice();

            #endregion

            #region Methods

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (!this.DeviceLine.IsDisposed)
                {
                    this.DeviceLine.Dispose();
                }

                Game.OnUpdate -= this.OnUpdate;
            }

            private void OnUpdate(EventArgs args)
            {
                if (this.PositionUpdate != null)
                {
                    var pos = this.PositionUpdate();
                    this.X = (int)pos.X;
                    this.Y = (int)pos.Y;
                }
            }

            #endregion
        }
    }
}