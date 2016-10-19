// <copyright file="SliderView.cs" company="LeagueSharp">
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

    /// <summary>
    ///     The component view.
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Service", typeof(MenuItem<Slider>))]
    public class SliderView : View
    {
        #region Fields

        private bool isSettingValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public SliderView(IMenuComponent component)
        {
            this.Component = component as MenuItem<Slider>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal SliderView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="SliderView" /> class.
        /// </summary>
        ~SliderView()
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
        public MenuItem<Slider> Component { get; }

        #endregion

        #region Properties

        private IDictionary<int, Render.Sprite> Sprites { get; } = new Dictionary<int, Render.Sprite>();

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new SliderView(component);
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

            Render.Sprite value;
            if (this.Sprites.TryGetValue(this.Component.Value.Value, out value))
            {
                value.Position = this.Component.Position;
                value.OnEndScene();
            }
            else
            {
                this.CreateContext();
                this.Sprites[this.Component.Value.Value].Position = this.Component.Position;
                this.Sprites[this.Component.Value.Value].OnEndScene();
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
                    if (!this.isSettingValue)
                    {
                        this.isSettingValue = this.Component.IsHovered;
                        if (this.isSettingValue)
                        {
                            this.UpdateValue();
                        }
                    }

                    break;
                case (uint)WindowsMessages.WM_LBUTTONUP:
                    this.isSettingValue = false;

                    break;
                case (uint)WindowsMessages.WM_MOUSEMOVE:

                    if (this.isSettingValue)
                    {
                        this.UpdateValue();
                    }

                    break;
            }
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
            var title = this.Component.DisplayName;
            var viewAttributes = new ViewAttributes(this);

            for (var i = this.Component.Value.Value - 5; i < this.Component.Value.Value + 5; ++i)
            {
                var value = i.ToString();
                var v = this.Component.Value;
                var bitmap = new Bitmap((int)Math.Round(width) + 1, (int)Math.Round(height) + 1);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    float length;
                    using (var font = new Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                    {
                        length = graphics.MeasureString(value, font).Width + 5;
                    }

                    SharedView.CreateBackgroundView(graphics, 0, 0, width, height);
                    SharedView.CreateTitle(graphics, 0, 0, width - length, height, title, viewAttributes);
                    SharedView.CreateSlider(graphics, 0, 0, width, height, i, v.MinValue, v.MaxValue);
                    SharedView.CreateText(graphics, width - length, 0, length, height, value, viewAttributes);
                }

                this.Sprites[i] = new Render.Sprite(bitmap, default(Vector2));
            }
        }

        private void UpdateValue()
        {
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var min = this.Component.Value.MinValue;
            var max = this.Component.Value.MaxValue;
            var cursorValue = min + ((Cursor.GetCursorPos().X - this.Component.Position.X) * (max - min) / width);
            this.Component.Value.Value = (int)Math.Round(cursorValue);
        }

        #endregion
    }
}