// <copyright file="KeybindView.cs" company="LeagueSharp">
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
    [ExportMetadata("Service", typeof(MenuItem<KeyBind>))]
    public class KeybindView : View
    {
        #region Fields

        private bool firstKeyMemory;

        private bool secondKeyMemory;

        private Render.Sprite sprite;

        private bool updateView;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeybindView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public KeybindView(IMenuComponent component)
        {
            this.Component = component as MenuItem<KeyBind>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeybindView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal KeybindView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="KeybindView" /> class.
        /// </summary>
        ~KeybindView()
        {
            this.sprite?.Bitmap?.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public MenuItem<KeyBind> Component { get; }

        #endregion

        #region Properties

        private KeybindSetStage Stage { get; set; } = KeybindSetStage.NotSetting;

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new KeybindView(component);
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

            if (this.updateView)
            {
                this.CreateContext();
                this.updateView = false;
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
            if (this.Component == null)
            {
                return;
            }

            switch (message)
            {
                case (uint)WindowsMessages.WM_LBUTTONDOWN:
                    if (this.Component.IsHovered)
                    {
                        switch (this.Stage)
                        {
                            case KeybindSetStage.NotSetting:
                                this.Stage = KeybindSetStage.Keybind1;

                                break;
                            case KeybindSetStage.Keybind1:
                                this.Stage = KeybindSetStage.Keybind2;
                                if (this.Component.Value.SecondaryKey == 0)
                                {
                                    this.Component.Value.SecondaryKey = uint.MaxValue;
                                }

                                break;
                            case KeybindSetStage.Keybind2:
                                this.Stage = KeybindSetStage.NotSetting;
                                if (this.Component.Value.SecondaryKey == uint.MaxValue)
                                {
                                    this.Component.Value.SecondaryKey = 0;
                                }

                                break;
                        }

                        this.updateView = true;
                    }

                    break;

                case (uint)WindowsMessages.WM_KEYDOWN:
                    if (MenuGUI.IsChatOpen || MenuGUI.IsScoreboardOpen || MenuGUI.IsShopOpen)
                    {
                        break;
                    }

                    switch (this.Stage)
                    {
                        case KeybindSetStage.NotSetting:
                            if (wParam == this.Component.Value.Key || wParam == this.Component.Value.SecondaryKey)
                            {
                                this.Component.Value.Active = (this.Component.Value.Type != KeyBindType.Toggle)
                                                              || !this.Component.Value.Active;
                                this.firstKeyMemory = wParam == this.Component.Value.Key;
                                this.secondKeyMemory = wParam == this.Component.Value.SecondaryKey;
                                this.updateView = true;
                            }

                            break;

                        case KeybindSetStage.Keybind1:
                            if (wParam == (uint)Keys.Back)
                            {
                                if (this.Component.Value.SecondaryKey != 0)
                                {
                                    this.Component.Value.Key = this.Component.Value.SecondaryKey;
                                    this.Component.Value.SecondaryKey = 0;
                                }
                                else
                                {
                                    this.Component.Value.Key = 0;
                                }
                            }
                            else
                            {
                                this.Component.Value.Key = wParam;
                            }

                            this.Stage = KeybindSetStage.NotSetting;
                            this.updateView = true;

                            break;
                        case KeybindSetStage.Keybind2:
                            this.Component.Value.SecondaryKey = wParam == (uint)Keys.Back ? 0 : wParam;
                            this.Stage = KeybindSetStage.NotSetting;
                            this.updateView = true;

                            break;
                    }

                    break;

                case (uint)WindowsMessages.WM_KEYUP:
                    if (MenuGUI.IsChatOpen || MenuGUI.IsScoreboardOpen || MenuGUI.IsShopOpen)
                    {
                        break;
                    }

                    switch (this.Stage)
                    {
                        case KeybindSetStage.NotSetting:
                            if (wParam == this.Component.Value.Key || wParam == this.Component.Value.SecondaryKey)
                            {
                                if (this.Component.Value.Type == KeyBindType.Press)
                                {
                                    this.Component.Value.Active = false;
                                    this.firstKeyMemory = false;
                                    this.secondKeyMemory = false;
                                    this.updateView = true;
                                }
                            }

                            break;
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
            var title = this.Component.DisplayName;
            var viewAttributes = new ViewAttributes(this);
            var viewAttributesOn = new ViewAttributes(null) { FontBrush = new SolidBrush(Color.Aqua) };
            var viewAttributesOff = new ViewAttributes(null) { FontBrush = new SolidBrush(Color.DarkRed) };

            var bitmap = new Bitmap((int)Math.Round(width) + 1, (int)Math.Round(height) + 1);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                SharedView.CreateBackgroundView(graphics, 0, 0, width, height);

                var keyString = this.Stage == KeybindSetStage.Keybind1
                                    ? "[?]"
                                    : $"[{Utils.KeyToText(this.Component.Value.Key)}]";
                var secondKeyString = this.Stage == KeybindSetStage.Keybind2
                                          ? "[?]"
                                          : $"[{Utils.KeyToText(this.Component.Value.SecondaryKey)}]";
                var secondKeyValid = this.Component.Value.SecondaryKey != 0;
                var keyAttribute = this.firstKeyMemory ? viewAttributesOn : viewAttributesOff;
                var secondKeyAttribute = this.secondKeyMemory ? viewAttributesOn : viewAttributesOff;

                float keySize;
                float secondKeySize;
                float finalSize;
                using (var font = new Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                {
                    keySize = graphics.MeasureString(keyString, font).Width;
                    secondKeySize = secondKeyValid ? graphics.MeasureString(secondKeyString, font).Width : 0;
                    finalSize = keySize + secondKeySize;
                }

                SharedView.CreateTitle(graphics, 0, 0, width - finalSize, height, title, viewAttributes);
                SharedView.CreateText(graphics, width - finalSize, 0, keySize, height, keyString, keyAttribute);
                if (secondKeyValid)
                {
                    SharedView.CreateText(
                        graphics,
                        width - secondKeySize,
                        0,
                        secondKeySize,
                        height,
                        secondKeyString,
                        secondKeyAttribute);
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