// <copyright file="SliderBoolView.cs" company="LeagueSharp">
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
    [ExportMetadata("Service", typeof(MenuItem<SliderBool>))]
    public class SliderBoolView : View
    {
        #region Fields

        private bool isSettingValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderBoolView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public SliderBoolView(IMenuComponent component)
        {
            this.Component = component as MenuItem<SliderBool>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderBoolView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal SliderBoolView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="SliderBoolView" /> class.
        /// </summary>
        ~SliderBoolView()
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
        public MenuItem<SliderBool> Component { get; }

        #endregion

        #region Properties

        private IDictionary<int, Render.Sprite> Sprites { get; } = new Dictionary<int, Render.Sprite>();

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new SliderBoolView(component);
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
            if (this.Component == null || !this.Component.IsVisible)
            {
                return;
            }

            switch (message)
            {
                case (uint)WindowsMessages.WM_LBUTTONDOWN:
                    if (!this.isSettingValue)
                    {
                        var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
                        var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
                        var cursor = Cursor.GetCursorPos();
                        if (Utils.IsUnderRectangle(
                            cursor,
                            this.Component.Position.X + width - height,
                            this.Component.Position.Y,
                            height,
                            height))
                        {
                            this.Component.Value.IsActive = !this.Component.Value.IsActive;
                            break;
                        }

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

                    var active = this.Component.Value.IsActive;
                    var boxColor = active ? Color.DarkGreen : Color.DarkRed;
                    var boxContent = this.Component.Value.Value.ToString();
                    var boxX = width - height;

                    SharedView.CreateBox(graphics, boxX, 0, height, height, boxColor, boxContent);
                    SharedView.CreateSlider(graphics, 0, 0, width - height, height, i, v.MinValue, v.MaxValue);
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
            var value = (int)Math.Round(cursorValue);
            if (value != this.Component.Value.Value)
            {
                this.Component.Value.Value = (int)Math.Round(cursorValue);
            }
        }

        #endregion
    }
}