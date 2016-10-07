// <copyright file="Circle.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     Circle drawing.
        /// </summary>
        public partial class Circle : RenderObject
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Circle" /> class.
            /// </summary>
            /// <param name="unit">
            ///     The unit.
            /// </param>
            /// <param name="radius">
            ///     The radius.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="zDeep">
            ///     A value indicating whether to enable depth.
            /// </param>
            public Circle(GameObject unit, float radius, Color color, int width = 1, bool zDeep = false)
                : this(radius, color, width, zDeep)
            {
                this.Unit = unit;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Circle" /> class.
            /// </summary>
            /// <param name="unit">
            ///     The unit.
            /// </param>
            /// <param name="offset">
            ///     The offset.
            /// </param>
            /// <param name="radius">
            ///     The radius.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="zDeep">
            ///     A value indicating whether to enable depth.
            /// </param>
            public Circle(GameObject unit, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
                : this(radius, color, width, zDeep)
            {
                this.Unit = unit;
                this.Offset = offset;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Circle" /> class.
            /// </summary>
            /// <param name="pos">
            ///     The position.
            /// </param>
            /// <param name="offset">
            ///     The offset.
            /// </param>
            /// <param name="radius">
            ///     The radius.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="zDeep">
            ///     A value indicating whether to enable depth.
            /// </param>
            public Circle(Vector3 pos, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
                : this(radius, color, width, zDeep)
            {
                this.Position = pos;
                this.Offset = offset;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Circle" /> class.
            /// </summary>
            /// <param name="pos">
            ///     The position.
            /// </param>
            /// <param name="radius">
            ///     The radius.
            /// </param>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            /// <param name="zDeep">
            ///     A value indicating whether to enable depth.
            /// </param>
            public Circle(Vector3 pos, float radius, Color color, int width = 1, bool zDeep = false)
                : this(radius, color, width, zDeep)
            {
                this.Position = pos;
            }

            private Circle(float radius, Color color, int width, bool zDeep)
            {
                this.Radius = radius;
                this.Color = color;
                this.Width = width;
                this.ZDeep = zDeep;
                this.SubscribeToResetEvents();
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            public Color Color { get; set; }

            /// <summary>
            ///     Gets or sets the offset.
            /// </summary>
            public Vector3 Offset { get; set; } = default(Vector3);

            /// <summary>
            ///     Gets or sets the position.
            /// </summary>
            public Vector3 Position { get; set; }

            /// <summary>
            ///     Gets or sets the radius.
            /// </summary>
            public float Radius { get; set; }

            /// <summary>
            ///     Gets or sets the unit.
            /// </summary>
            public GameObject Unit { get; set; }

            /// <summary>
            ///     Gets or sets the width.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether to enable depth.
            /// </summary>
            public bool ZDeep { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public override void OnDraw()
            {
                try
                {
                    var position = default(Vector3);
                    if (this.Unit?.IsValid ?? false)
                    {
                        position = this.Unit.Position + this.Offset;
                    }
                    else if (!(this.Position + this.Offset).To2D().IsZero)
                    {
                        position = this.Position + this.Offset;
                    }

                    if (!position.IsZero)
                    {
                        DrawCircle(position, this.Radius, this.Color, this.Width, this.ZDeep);
                    }
                }
                catch (Exception e)
                {
                    this.Log.Error($"Could not draw a circle.", e);
                }
            }

            #endregion
        }
    }
}