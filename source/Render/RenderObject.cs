// <copyright file="RenderObject.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Reflection;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using IDisposable = PlaySharp.Toolkit.AppDomain.IDisposable;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     The render object.
        /// </summary>
        public class RenderObject : IDisposable
        {
            #region Fields

            private bool visible = true;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Finalizes an instance of the <see cref="RenderObject" /> class.
            /// </summary>
            ~RenderObject()
            {
                this.Dispose(false);
            }

            #endregion

            #region Delegates

            /// <summary>
            ///     The visible condition delegate.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public delegate bool VisibleConditionDelegate(RenderObject sender);

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets a value indicating whether the render object was dispoed.
            /// </summary>
            public bool IsDisposed { get; private set; }

            /// <summary>
            ///     Gets or sets the layer.
            /// </summary>
            public float Layer { get; set; } = 0.0f;

            /// <summary>
            ///     Gets or sets a value indicating whether the render object is visible.
            /// </summary>
            public bool Visible
            {
                get
                {
                    return this.VisibleCondition?.Invoke(this) ?? this.visible;
                }

                set
                {
                    this.visible = value;
                }
            }

            /// <summary>
            ///     Gets or sets the visible condition.
            /// </summary>
            public VisibleConditionDelegate VisibleCondition { get; set; }

            #endregion

            #region Properties

            /// <summary>
            ///     Gets the log.
            /// </summary>
            protected ILog Log { get; } = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.OnPreReset();
                this.Dispose(true);
            }

            /// <summary>
            ///     Determines if the render object has a valid layer.
            /// </summary>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool HasValidLayer()
            {
                return this.Layer >= -5 && this.Layer <= 5;
            }

            /// <summary>
            ///     The draw event callback.
            /// </summary>
            public virtual void OnDraw()
            {
            }

            /// <summary>
            ///     The endscene event callback.
            /// </summary>
            public virtual void OnEndScene()
            {
            }

            /// <summary>
            ///     The post-reset event callback.
            /// </summary>
            public virtual void OnPostReset()
            {
            }

            /// <summary>
            ///     The pre-reset event callback.
            /// </summary>
            public virtual void OnPreReset()
            {
            }

            #endregion

            #region Methods

            /// <summary>
            ///     Subscribers to D3D9 reset event.
            /// </summary>
            internal void SubscribeToResetEvents()
            {
                /*Drawing.OnPreReset += this.DrawingOnOnPreReset;
                Drawing.OnPostReset += this.DrawingOnOnPostReset;
                AppDomain.CurrentDomain.DomainUnload += this.CurrentDomainOnDomainUnload;*/
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">
            ///     A value indicating whether the call is disposing managed resources.
            /// </param>
            protected virtual void Dispose(bool disposing)
            {
                if (this.IsDisposed)
                {
                    return;
                }

                /*Drawing.OnPreReset -= this.DrawingOnOnPreReset;
                Drawing.OnPostReset -= this.DrawingOnOnPostReset;
                AppDomain.CurrentDomain.DomainUnload -= this.CurrentDomainOnDomainUnload;*/

                this.IsDisposed = true;
            }

            private void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
            {
                this.OnPostReset();
            }

            private void DrawingOnOnPostReset(EventArgs args)
            {
                this.OnPostReset();
            }

            private void DrawingOnOnPreReset(EventArgs args)
            {
                this.OnPreReset();
            }

            #endregion
        }
    }
}