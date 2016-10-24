// <copyright file="ComponentView.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System;
    using System.ComponentModel.Composition;
    using System.Drawing;

    using SharpDX;
    using SharpDX.Menu;

    /// <summary>
    ///     The component view.
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Service", typeof(IMenuComponent))]
    public class ComponentView : View
    {
        #region Fields

        private Render.Sprite sprite;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ComponentView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public ComponentView(IMenuComponent component)
        {
            this.Component = component;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ComponentView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal ComponentView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="ComponentView" /> class.
        /// </summary>
        ~ComponentView()
        {
            this.sprite?.Bitmap?.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public IMenuComponent Component { get; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new ComponentView(component);
        }

        /// <inheritdoc />
        public override void OnAttributeChange()
        {
            this.CreateContext();
        }

        /// <inheritdoc />
        public override void OnDraw()
        {
            if (this.Component == null || !this.Component.IsVisible)
            {
                return;
            }

            this.sprite.Position = this.Component.Position;
            this.sprite.OnEndScene();
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
        }

        /// <inheritdoc />
        public override void OnWindowProc(uint message, uint wParam, long lParam)
        {
        }

        #endregion

        #region Methods

        private void CreateContext()
        {
            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var title = this.Component.DisplayName;
            var viewAttributes = new ViewAttributes(this);

            var bitmap = new Bitmap((int)Math.Round(width) + 1, (int)Math.Round(height) + 1);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                SharedView.CreateBackgroundView(graphics, 0, 0, width, height);
                SharedView.CreateTitle(graphics, 0, 0, width, height, title, viewAttributes);
            }

            if (this.sprite == null)
            {
                this.sprite = new Render.Sprite(bitmap, default(Vector2));
                return;
            }

            this.sprite.UpdateTextureBitmap(bitmap);
        }

        #endregion
    }
}