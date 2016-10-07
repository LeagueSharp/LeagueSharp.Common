// <copyright file="Render.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using SharpDX;
    using SharpDX.Direct3D9;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        #region Static Fields

        private static readonly ILog Logger = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<RenderObject> RenderObjects = new List<RenderObject>();

        private static readonly object RenderObjectsLock = new object();

        private static List<RenderObject> renderVisibleObjects = new List<RenderObject>();

        private static bool terminateThread;

        #endregion

        #region Constructors and Destructors

        static Render()
        {
            Drawing.OnEndScene += OnEndScne;
            Drawing.OnDraw += OnDraw;

            var thread = new Thread(PrepareObjects);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the device.
        /// </summary>
        public static Device Device => Drawing.Direct3DDevice;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the render object to be drawn by the rendering system.
        /// </summary>
        /// <param name="renderObject">
        ///     The render object.
        /// </param>
        /// <param name="layer">
        ///     The layer.
        /// </param>
        /// <returns>
        ///     The <see cref="RenderObject" />.
        /// </returns>
        public static RenderObject Add(this RenderObject renderObject, float layer = float.MaxValue)
        {
            renderObject.Layer = !layer.Equals(float.MaxValue) ? layer : renderObject.Layer;
            lock (RenderObjectsLock)
            {
                RenderObjects.Add(renderObject);
            }

            return renderObject;
        }

        /// <summary>
        ///     Determines if the point is on the screen.
        /// </summary>
        /// <param name="point">
        ///     The point.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool OnScreen(Vector2 point)
            => point.X > 0 && point.Y > 0 && point.X < Drawing.Width && point.Y < Drawing.Height;

        /// <summary>
        ///     Removes the render object from the engine.
        /// </summary>
        /// <param name="renderObject">
        ///     The render object.
        /// </param>
        public static void Remove(this RenderObject renderObject)
        {
            lock (RenderObjectsLock)
            {
                RenderObjects.Remove(renderObject);
            }
        }

        /// <summary>
        ///     Terminates the preparations thread.
        /// </summary>
        public static void Terminate() => terminateThread = true;

        #endregion

        #region Methods

        private static void OnDraw(EventArgs args)
        {
            if (Device == null || Device.IsDisposed)
            {
                return;
            }

            foreach (var renderObject in renderVisibleObjects)
            {
                renderObject.OnDraw();
            }
        }

        private static void OnEndScne(EventArgs args)
        {
            if (Device == null || Device.IsDisposed)
            {
                return;
            }

            Device.SetRenderState(RenderState.AlphaBlendEnable, true);

            foreach (var renderObject in renderVisibleObjects)
            {
                renderObject.OnEndScene();
            }
        }

        private static void PrepareObjects()
        {
            while (!terminateThread)
            {
                try
                {
                    Thread.Sleep(1);
                    lock (RenderObjectsLock)
                    {
                        renderVisibleObjects =
                            RenderObjects.Where(o => o != null && o.Visible && o.HasValidLayer())
                                .OrderBy(o => o.Layer)
                                .ToList();
                    }
                }
                catch (ThreadAbortException)
                {
                    // ignored
                }
                catch (Exception e)
                {
                    Logger.Error("Render Thread faulted", e);
                }
            }
        }

        #endregion
    }
}