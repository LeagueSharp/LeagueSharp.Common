// <copyright file="CircleEffect.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Text;

    using LeagueSharp.Common.Properties;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     Circle drawing.
        /// </summary>
        public partial class Circle
        {
            #region Properties

            private static Effect Effect { get; set; }

            private static bool Initialized { get; set; }

            private static VertexBuffer VertexBuffer { get; set; }

            private static VertexDeclaration VertexDeclaration { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Creates the circle vertices.
            /// </summary>
            public static void CreateVertexes()
            {
                const Usage Usage = Usage.WriteOnly;
                const VertexFormat Format = VertexFormat.None;
                const Pool Pool = Pool.Managed;

                var sizeInBytes = Utilities.SizeOf<Vector4>() * 2 * 6;

                VertexBuffer = new VertexBuffer(Device, sizeInBytes, Usage, Format, Pool);
                SatisfyBuffer(VertexBuffer.Lock(0, 0, LockFlags.None));
                VertexBuffer.Unlock();

                var vertexElements = CreateVertexElements();
                VertexDeclaration = new VertexDeclaration(Device, vertexElements);

                try
                {
                    var effect = Encoding.UTF8.GetString(Resources.CircleEffect);
                    Effect = Effect.FromString(Device, effect, ShaderFlags.None);
                }
                catch (Exception e)
                {
                    Logger.Fatal("Failed to compile circle effect.", e);
                }

                if (!Initialized)
                {
                    Initialized = true;
                    Drawing.OnPreReset += OnPreReset;
                    Drawing.OnPostReset += OnPostReset;
                }
            }

            /// <summary>
            ///     Draws a circle.
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
            public static void DrawCircle(Vector3 pos, float radius, Color color, int width = 5, bool zDeep = false)
            {
                if (Device == null || Device.IsDisposed)
                {
                    return;
                }

                if (VertexBuffer == null)
                {
                    CreateVertexes();
                }

                if ((VertexBuffer?.IsDisposed ?? false) || VertexDeclaration.IsDisposed || Effect.IsDisposed)
                {
                    return;
                }

                try
                {
                    var vertexDeclaration = Device.VertexDeclaration;

                    Effect.Begin();
                    Effect.BeginPass(0);

                    Effect.SetValue(
                        "ProjectionMatrix",
                        Matrix.Translation(pos.SwitchYZ()) * Drawing.View * Drawing.Projection);
                    Effect.SetValue(
                        "CircleColor",
                        new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
                    Effect.SetValue("Radius", radius);
                    Effect.SetValue("Border", 2f + width);
                    Effect.SetValue("zEnabled", zDeep);

                    Device.SetStreamSource(0, VertexBuffer, 0, Utilities.SizeOf<Vector4>() * 2);
                    Device.VertexDeclaration = VertexDeclaration;

                    Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                    Effect.EndPass();
                    Effect.End();

                    Device.VertexDeclaration = vertexDeclaration;
                }
                catch (Exception e)
                {
                    Dispose(null, EventArgs.Empty);
                    Logger.Error("Unable to draw circle, flushing resources..", e);
                }
            }

            #endregion

            #region Methods

            /// <summary>
            ///     Disposes the circle effect unmanaged resources.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <param name="e">
            ///     The event args.
            /// </param>
            internal static void Dispose(object sender, EventArgs e)
            {
                Initialized = false;
                OnPreReset(EventArgs.Empty);

                if (Effect != null && !Effect.IsDisposed)
                {
                    Effect.Dispose();
                }

                if (VertexBuffer != null && !VertexBuffer.IsDisposed)
                {
                    VertexBuffer.Dispose();
                }

                if (VertexDeclaration != null && !VertexDeclaration.IsDisposed)
                {
                    VertexDeclaration.Dispose();
                }
            }

            private static VertexElement[] CreateVertexElements()
                =>
                new[]
                    {
                        new VertexElement(
                            0,
                            0,
                            DeclarationType.Float4,
                            DeclarationMethod.Default,
                            DeclarationUsage.Position,
                            0),
                        new VertexElement(
                            0,
                            16,
                            DeclarationType.Float4,
                            DeclarationMethod.Default,
                            DeclarationUsage.Color,
                            0),
                        VertexElement.VertexDeclarationEnd
                    };

            private static void OnPostReset(EventArgs args)
            {
                if (Effect != null && !Effect.IsDisposed)
                {
                    Effect.OnResetDevice();
                }
            }

            private static void OnPreReset(EventArgs args)
            {
                if (Effect != null && !Effect.IsDisposed)
                {
                    Effect.OnLostDevice();
                }
            }

            private static void SatisfyBuffer(DataStream dataStream)
            {
                const float X = 6000f;
                var range = new Vector4[12];

                for (var i = 1; i < range.Length; i += 2)
                {
                    range[i] = Vector4.Zero;
                }

                // T1
                range[0] = new Vector4(-X, 0f, -X, 1.0f);
                range[2] = new Vector4(-X, 0f, X, 1.0f);
                range[4] = new Vector4(X, 0f, -X, 1.0f);

                // T2
                range[6] = new Vector4(-X, 0f, X, 1.0f);
                range[8] = new Vector4(X, 0f, X, 1.0f);
                range[10] = new Vector4(X, 0f, -X, 1.0f);

                dataStream.WriteRange(range);
            }

            #endregion
        }
    }
}