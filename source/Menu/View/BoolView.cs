// <copyright file="BoolView.cs" company="LeagueSharp">
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
    [ExportMetadata("Service", typeof(MenuItem<bool>))]
    public class BoolView : View
    {
        #region Fields

        private Render.Sprite sprite;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public BoolView(IMenuComponent component)
        {
            this.Component = component as MenuItem<bool>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal BoolView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="BoolView" /> class.
        /// </summary>
        ~BoolView()
        {
            this.sprite?.Bitmap?.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public MenuItem<bool> Component { get; }

        #endregion

        #region Properties

        private static Render.Sprite FalseSprite { get; set; }

        private static Render.Sprite TrueSprite { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new BoolView(component);
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

            this.sprite.Position = this.Component.Position;
            this.sprite.OnEndScene();

            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var position = new Vector2(this.Component.Position.X + width - height, this.Component.Position.Y);
            if (this.Component.Value)
            {
                TrueSprite.Position = position;
                TrueSprite.OnEndScene();
            }
            else
            {
                FalseSprite.Position = position;
                FalseSprite.OnEndScene();
            }
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
        }

        /// <inheritdoc />
        public override void OnWindowProc(uint message, uint wParam, long lParam)
        {
            if (this.Component == null)
            {
                return;
            }

            switch (message)
            {
                case (uint)WindowsMessages.WM_LBUTTONDOWN:
                    var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
                    var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
                    var cursor = Cursor.GetCursorPos();
                    if (Utils.IsUnderRectangle(
                        cursor,
                        this.Component.Position.X + width - height,
                        this.Component.Position.Y,
                        height,
                        height))
                    {
                        this.Component.Value = !this.Component.Value;
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        private static void CreateGlobalContext(int height)
        {
            if (FalseSprite == null)
            {
                var bitmap = new Bitmap(height, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    SharedView.CreateBox(graphics, 0, 0, height, height, Color.DarkRed, "OFF");
                }

                FalseSprite = new Render.Sprite(bitmap, default(Vector2));
            }

            if (TrueSprite == null)
            {
                var bitmap = new Bitmap(height, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    SharedView.CreateBox(graphics, 0, 0, height, height, Color.DarkGreen, "ON");
                }

                TrueSprite = new Render.Sprite(bitmap, default(Vector2));
            }
        }

        private void CreateContext()
        {
            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var title = this.Component.DisplayName;
            var viewAttributes = new ViewAttributes(this);

            CreateGlobalContext((int)Math.Round(height));

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