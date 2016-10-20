// <copyright file="StringListView.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System;
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
        #region Fields

        private bool isPicking;

        private Render.Sprite sprite;

        private bool updateContext;

        #endregion

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
            this.sprite?.Bitmap?.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public MenuItem<StringList> Component { get; }

        #endregion

        #region Properties

        private float BorderWidth { get; set; }

        private float TitleWidth { get; set; }

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
            if (this.Component == null || !this.Component.IsVisible)
            {
                return;
            }

            if (this.updateContext)
            {
                this.CreateContext();
                this.updateContext = false;
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
            if (this.Component == null || !this.Component.IsVisible)
            {
                return;
            }

            var cursor = Cursor.GetCursorPos();
            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            switch (message)
            {
                case (uint)WindowsMessages.WM_LBUTTONDOWN:
                    if (Utils.IsUnderRectangle(
                        cursor,
                        this.Component.Position.X + this.TitleWidth,
                        this.Component.Position.Y,
                        width - this.TitleWidth,
                        height))
                    {
                        this.isPicking = !this.isPicking;
                        this.updateContext = true;
                    }

                    if (this.isPicking)
                    {
                        if (Utils.IsUnderRectangle(
                            cursor,
                            this.Component.Position.X + width,
                            this.Component.Position.Y,
                            width,
                            height * this.Component.Value.Items.Length))
                        {
                            this.Component.Value.SelectedIndex = (int)((cursor.Y - this.Component.Position.Y) / height);
                            this.updateContext = true;
                        }
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        private void CreateContext()
        {
            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var viewAttributes = new ViewAttributes(this);

            var title = $"{this.Component.DisplayName}:";
            var value = this.Component.Value.SelectedValue;

            var bitmap = new Bitmap(
                             (int)Math.Round(width * 2) + 1,
                             (int)Math.Round(height * this.Component.Value.Items.Length) + 1);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                float arrowWidth;
                using (var font = new Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                {
                    this.BorderWidth = graphics.MeasureString(value, font).Width;
                    this.TitleWidth = graphics.MeasureString(title, font).Width + 10;
                    arrowWidth = graphics.MeasureString("»", font).Width;
                }

                SharedView.CreateBackgroundView(graphics, 0, 0, width, height);
                SharedView.CreateTitle(graphics, 0, 0, width - (this.BorderWidth / 2), height, title, viewAttributes);
                SharedView.CreateText(
                    graphics,
                    this.TitleWidth,
                    0,
                    width - this.TitleWidth - arrowWidth,
                    height,
                    value,
                    viewAttributes);
                SharedView.CreateText(graphics, width - arrowWidth, 0, arrowWidth, height, "»", viewAttributes);
                SharedView.CreateBorder(
                    graphics,
                    this.TitleWidth,
                    0,
                    width - this.TitleWidth,
                    height - 1,
                    Color.DarkGray);

                if (this.isPicking)
                {
                    SharedView.CreateBackgroundView(
                        graphics,
                        width + 1,
                        0,
                        width - 1,
                        height * this.Component.Value.Items.Length);
                    for (var i = 0; i < this.Component.Value.Items.Length; ++i)
                    {
                        SharedView.CreateBorder(graphics, width + 1, height * i, width - 1, height, Color.Black);
                        SharedView.CreateText(
                            graphics,
                            width + 1,
                            height * i,
                            width - 1,
                            height,
                            this.Component.Value.Items[i],
                            viewAttributes);
                    }
                }
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