// <copyright file="Line.cs" company="LeagueSharp">
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
        ///     Draws a line.
        /// </summary>
        public class Line : RenderObject
        {
            #region Fields

            private int width;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Line" /> class.
            /// </summary>
            /// <param name="start">
            ///     The start.
            /// </param>
            /// <param name="end">
            ///     The end.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            public Line(Vector2 start, Vector2 end, int width, ColorBGRA color)
            {
                this.DeviceLine = new SharpDX.Direct3D9.Line(Device);

                this.Width = width;
                this.Color = color;
                this.Start = start;
                this.End = end;

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
            ///     Gets or sets the ending position.
            /// </summary>
            public Vector2 End { get; set; }

            /// <summary>
            ///     Gets or sets the end position update.
            /// </summary>
            public PositionDelegate EndPositionUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the starting position.
            /// </summary>
            public Vector2 Start { get; set; }

            /// <summary>
            ///     Gets or sets the start position update.
            /// </summary>
            public PositionDelegate StartPositionUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the line width.
            /// </summary>
            public int Width
            {
                get
                {
                    return this.width;
                }

                set
                {
                    this.DeviceLine.Width = value;
                    this.width = value;
                }
            }

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
                    this.DeviceLine.Draw(new[] { this.Start, this.End }, this.Color);
                    this.DeviceLine.End();
                }
                catch (Exception e)
                {
                    this.Log.Error("Unable to draw line.", e);
                }
            }

            /// <inheritdoc />
            public override void OnPostReset()
            {
                base.OnPostReset();
                this.DeviceLine?.OnResetDevice();
            }

            /// <inheritdoc />
            public override void OnPreReset()
            {
                base.OnPreReset();
                this.DeviceLine?.OnLostDevice();
            }

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
                if (this.StartPositionUpdate != null)
                {
                    this.Start = this.StartPositionUpdate();
                }

                if (this.EndPositionUpdate != null)
                {
                    this.End = this.EndPositionUpdate();
                }
            }

            #endregion
        }
    }
}