// <copyright file="StringListView.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Drawing;

    using SharpDX;
    using SharpDX.Menu;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The component view.
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Service", typeof(MenuItem<StringList>))]
    public class StringListView : View
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringListView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public StringListView(IMenuComponent component)
        {
            this.Component = component as MenuItem<StringList>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringListView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal StringListView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="StringListView" /> class.
        /// </summary>
        ~StringListView()
        {
            foreach (var sprite in this.Sprites.Values)
            {
                sprite?.Bitmap?.Dispose();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public MenuItem<StringList> Component { get; }

        #endregion

        #region Properties

        private IDictionary<int, Render.Sprite> Sprites { get; } = new Dictionary<int, Render.Sprite>();

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new StringListView(component);
        }

        /// <inheritdoc />
        public override void OnAttributeChange()
        {
            this.CreateContext();
        }

        /// <inheritdoc />
        public override void OnDraw()
        {
            if (this.Component == null)
            {
                return;
            }

            this.Sprites[this.Component.Value.SelectedIndex].Position = this.Component.Position;
            this.Sprites[this.Component.Value.SelectedIndex].OnEndScene();
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
            foreach (var sprite in this.Sprites.Values)
            {
                sprite.Dispose();
            }

            this.Sprites.Clear();

            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var viewAttributes = new ViewAttributes(this);

            for (var i = 0; i < this.Component.Value.Items.Length; ++i)
            {
                var bitmap = new Bitmap((int)Math.Round(width) + 1, (int)Math.Round(height) + 1);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var boxSize = (height / 2) + 5;
                    var title = $"{this.Component.DisplayName}: {this.Component.Value.Items[i]}";
                    var color = Color.FromArgb(255, 20, 90, 175);

                    SharedView.CreateBackgroundView(graphics, 0, 0, width, height);
                    SharedView.CreateTitle(graphics, boxSize, 0, width - boxSize, height, title, viewAttributes);
                    SharedView.CreateBox(graphics, 5, 5, height / 2, height - 10, color, "<");
                    SharedView.CreateBox(graphics, width - boxSize, 5, height / 2, height - 10, color, ">");
                }

                this.Sprites[i] = new Render.Sprite(bitmap, default(Vector2));
            }
        }

        #endregion
    }
}