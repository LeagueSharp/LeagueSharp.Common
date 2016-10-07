namespace LeagueSharp.Common
{
    using System;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     Basic Notification
    /// </summary>
    public class Notification : INotification, IDisposable
    {
        #region Fields

        /// <summary>
        ///     Notification's Border Color
        /// </summary>
        public ColorBGRA BorderColor = new ColorBGRA(255f, 255f, 255f, 255f);

        /// <summary>
        ///     Notification's Box Color
        /// </summary>
        public ColorBGRA BoxColor = new ColorBGRA(0f, 0f, 0f, 255f);

        /// <summary>
        ///     Notification's Font
        /// </summary>
        public Font Font = new Font(
            Drawing.Direct3DDevice,
            0xE,
            0x0,
            FontWeight.DoNotCare,
            0x0,
            false,
            FontCharacterSet.Default,
            FontPrecision.Default,
            FontQuality.Antialiased,
            FontPitchAndFamily.DontCare | FontPitchAndFamily.Decorative,
            "Tahoma");

        /// <summary>
        ///     Notification's Text
        /// </summary>
        public string Text;

        /// <summary>
        ///     Notification's Text Color
        /// </summary>
        public ColorBGRA TextColor = new ColorBGRA(255f, 255f, 255f, 255f);

        /// <summary>
        ///     Locally saved bool which indicates if notification will be disposed after finishing
        /// </summary>
        private readonly bool autoDispose;

        /// <summary>
        ///     Locally saved bytes which contain old ALPHA values
        /// </summary>
        private readonly byte[] flashingBytes = new byte[3];

        /// <summary>
        ///     Locally saved Global Unique Identification (GUID)
        /// </summary>
        private readonly string id;

        /// <summary>
        ///     Locally saved Line
        /// </summary>
        private readonly Line line = new Line(Drawing.Direct3DDevice)
                                         { Antialias = false, GLLines = true, Width = 190f };

        /// <summary>
        ///     Locally saved Sprite
        /// </summary>
        private readonly Sprite sprite = new Sprite(Drawing.Direct3DDevice);

        /// <summary>
        ///     Locally saved bool which indicates if border should be drawn
        /// </summary>
        private bool border;

        /// <summary>
        ///     Locally saved int which contains data of the last tick.
        /// </summary>
        private int clickTick;

        /// <summary>
        ///     Locally saved Notification's Duration
        /// </summary>
        private int duration;

        /// <summary>
        ///     Locally saved bool which indicates if flashing mode is on or off.
        /// </summary>
        private bool flashing;

        /// <summary>
        ///     Locally saved int which contains an internval for flash mode
        /// </summary>
        private int flashInterval;

        /// <summary>
        ///     Locally saved int which contains next flash mode tick
        /// </summary>
        private int flashTick;

        /// <summary>
        ///     Locally moved saved position
        /// </summary>
        private Vector2 moveStartPosition;

        /// <summary>
        ///     Locally saved Notification's Movement Start Tick
        /// </summary>
        private int moveStartT;

        /// <summary>
        ///     Locally saved position
        /// </summary>
        private Vector2 position;

        /// <summary>
        ///     Locally saved Notification's Start Tick
        /// </summary>
        private int startT;

        /// <summary>
        ///     Locally saved Notification State
        /// </summary>
        private NotificationState state;

        /// <summary>
        ///     Locally saved boolean for Text Fix
        /// </summary>
        private Vector2 textFix;

        /// <summary>
        ///     Locally saved update position
        /// </summary>
        private Vector2 updatePosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Notification Constructor
        /// </summary>
        /// <param name="text">Display Text</param>
        /// <param name="duration">Duration (-1 for Infinite)</param>
        /// <param name="dispose">Auto Dispose after notification duration end</param>
        public Notification(string text, int duration = -0x1, bool dispose = false)
        {
            // Setting GUID
            this.id = Guid.NewGuid().ToString("N");

            // Setting main values
            this.Text = text;
            this.state = NotificationState.Idle;
            this.border = true;
            this.autoDispose = dispose;

            // Preload Text
            this.Font.PreloadText(text);

            // Calling Show
            this.Show(duration);
        }

        /// <summary>
        ///     Finalization
        /// </summary>
        ~Notification()
        {
            this.Dispose(false);
        }

        #endregion

        #region Enums

        public enum NotificationState
        {
            Idle,

            AnimationMove,

            AnimationShowShrink,

            AnimationShowMove,

            AnimationShowGrow
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Indicates if notification should be drawn
        /// </summary>
        public bool Draw { get; set; }

        /// <summary>
        ///     Indicates if notification should be updated
        /// </summary>
        public bool Update { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Toggles the notification border
        /// </summary>
        public Notification Border()
        {
            this.border = !this.border;

            return this;
        }

        /// <summary>
        ///     Sets the notification border toggle value
        /// </summary>
        /// <param name="value">bool value</param>
        public Notification Border(bool value)
        {
            this.border = value;

            return this;
        }

        /// <summary>
        ///     IDisposable callback
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        ///     Enters Notification's flashing mode
        /// </summary>
        /// <param name="interval">Flash Interval</param>
        public Notification Flash(int interval = 0xFA)
        {
            this.flashing = !this.flashing;
            if (this.flashing)
            {
                this.flashInterval = interval;
            }

            return this;
        }

        /// <summary>
        ///     Returns the notification's global unique identification (GUID)
        /// </summary>
        /// <returns>GUID</returns>
        public string GetId()
        {
            return this.id;
        }

        public void OnDomainUnload()
        {
            this.Font.OnLostDevice();
            this.line.OnLostDevice();
            this.sprite.OnLostDevice();
        }

        /// <summary>
        ///     Called for Drawing onto screen
        /// </summary>
        public void OnDraw()
        {
            if (!this.Draw)
            {
                return;
            }

            Vector2[] vertices;

            #region Outline

            if (this.border)
            {
                this.line.Begin();

                vertices = new[]
                               {
                                   new Vector2(
                                       this.position.X + (int)Math.Floor(this.line.Width / 0x2),
                                       this.position.Y - 0x01),
                                   new Vector2(
                                       this.position.X + (int)Math.Floor(this.line.Width / 0x2) + 0x01,
                                       this.position.Y + 25f + 0x01)
                               };

                this.line.Draw(vertices, this.BorderColor);
                this.line.End();
            }

            #endregion

            #region Box

            this.line.Begin();

            vertices = new[]
                           {
                               new Vector2(this.position.X + (int)Math.Floor(this.line.Width / 0x2), this.position.Y),
                               new Vector2(
                                   this.position.X + (int)Math.Floor(this.line.Width / 0x2),
                                   this.position.Y + 25f)
                           };

            this.line.Draw(vertices, this.BoxColor);
            this.line.End();

            #endregion

            #region Text

            this.sprite.Begin();

            var textDimension = this.Font.MeasureText(this.sprite, this.Text);
            var finalText = this.Text;

            if (textDimension.Width + 0x5 > this.line.Width)
            {
                for (var i = this.Text.Length; i > 0x0; --i)
                {
                    var text = this.Text.Substring(0x0, i);
                    var textWidth = this.Font.MeasureText(this.sprite, text).Width;

                    if (textWidth + 0x5 > this.line.Width)
                    {
                        continue;
                    }

                    finalText = (text == this.Text) ? text : text.Substring(0x0, text.Length - 0x3) + "...";
                    break;
                }
            }

            textDimension = this.Font.MeasureText(this.sprite, finalText);

            var rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.line.Width, 0x19);

            this.Font.DrawText(
                this.sprite,
                finalText,
                rectangle.TopLeft.X + (rectangle.Width - textDimension.Width) / 0x2,
                rectangle.TopLeft.Y + (rectangle.Height - textDimension.Height) / 0x2,
                this.TextColor);

            this.sprite.End();

            #endregion
        }

        public void OnPostReset()
        {
            this.line.OnResetDevice();
            this.Font.OnResetDevice();
            this.sprite.OnResetDevice();
        }

        public void OnPreReset()
        {
            this.Font.OnLostDevice();
            this.line.OnLostDevice();
            this.sprite.OnLostDevice();
        }

        /// <summary>
        ///     Called per game tick for update
        /// </summary>
        public void OnUpdate()
        {
            if (!this.Update)
            {
                return;
            }

            switch (this.state)
            {
                case NotificationState.Idle:
                    {
                        #region Duration End Handler

                        if (!this.flashing && this.duration > 0x0 && this.TextColor.A == 0x0 && this.BoxColor.A == 0x0
                            && this.BorderColor.A == 0x0)
                        {
                            this.Update = this.Draw = false;
                            if (this.autoDispose)
                            {
                                this.Dispose();
                            }

                            Notifications.RemoveNotification(this);

                            return;
                        }

                        #endregion

                        #region Decreasement Tick

                        var t = Math.Max(0, this.startT + this.duration - Utils.GameTimeTickCount + 500);
                        if (!this.flashing && this.duration > 0x0 && t < 500)
                        {
                            var alpha = (byte)(255 * ((float)t / 500));
                            this.TextColor.A = alpha;
                            this.BoxColor.A = alpha;
                            this.BorderColor.A = alpha;
                        }

                        #endregion

                        #region Flashing

                        if (this.flashing)
                        {
                            if (Utils.TickCount - this.flashTick > 0x0)
                            {
                                if (this.TextColor.A > 0x0 && this.BoxColor.A > 0x0 && this.BorderColor.A > 0x0)
                                {
                                    if (this.duration > 0x0)
                                    {
                                        if (this.TextColor.A == 0x0 && this.BoxColor.A == 0x0
                                            && this.BorderColor.A == 0x0)
                                        {
                                            this.Update = this.Draw = false;
                                            if (this.autoDispose)
                                            {
                                                this.Dispose();
                                            }

                                            Notifications.RemoveNotification(this);

                                            return;
                                        }
                                    }

                                    this.flashingBytes[0x0] = --this.TextColor.A;
                                    this.flashingBytes[0x1] = --this.BoxColor.A;
                                    this.flashingBytes[0x2] = --this.BorderColor.A;

                                    this.TextColor.A = 0x0;
                                    this.BoxColor.A = 0x0;
                                    this.BorderColor.A = 0x0;
                                }
                                else
                                {
                                    this.TextColor.A = this.flashingBytes[0x0];
                                    this.BoxColor.A = this.flashingBytes[0x1];
                                    this.BorderColor.A = this.flashingBytes[0x2];

                                    if (this.TextColor.A > 0x0)
                                    {
                                        this.TextColor.A--;
                                    }
                                    if (this.BoxColor.A > 0x0)
                                    {
                                        this.BoxColor.A--;
                                    }
                                    if (this.BorderColor.A > 0x0)
                                    {
                                        this.BorderColor.A--;
                                    }

                                    if (this.duration > 0x0)
                                    {
                                        if (this.TextColor.A == 0x0 && this.BoxColor.A == 0x0
                                            && this.BorderColor.A == 0x0)
                                        {
                                            this.Update = this.Draw = false;
                                            if (this.autoDispose)
                                            {
                                                this.Dispose();
                                            }

                                            Notifications.RemoveNotification(this);

                                            return;
                                        }
                                    }
                                }
                                this.flashTick = Utils.TickCount + this.flashInterval;
                            }
                        }

                        #endregion

                        #region Mouse

                        var mouseLocation = Drawing.WorldToScreen(Game.CursorPos);
                        if (Utils.IsUnderRectangle(
                            mouseLocation,
                            this.position.X,
                            this.position.Y,
                            this.line.Width,
                            25f))
                        {
                            this.TextColor.A = 0xFF;
                            this.BoxColor.A = 0xFF;
                            this.BorderColor.A = 0xFF;

                            var textDimension = this.Font.MeasureText(this.sprite, this.Text);
                            if (textDimension.Width + 0x10 > this.line.Width)
                            {
                                var extra = textDimension.Width - 0xB4;
                                if (this.updatePosition == Vector2.Zero)
                                {
                                    this.textFix = new Vector2(this.position.X, this.position.Y);
                                    this.updatePosition = new Vector2(this.position.X - extra, this.position.Y);
                                }
                                if (this.updatePosition != Vector2.Zero && this.position.X > this.updatePosition.X)
                                {
                                    this.position.X -= 1f;
                                    this.line.Width += 1f;
                                }
                            }
                        }
                        else if (this.updatePosition != Vector2.Zero)
                        {
                            if (this.position.X < this.textFix.X)
                            {
                                this.position.X += 1f;
                                this.line.Width -= 1f;
                            }
                            else
                            {
                                this.textFix = Vector2.Zero;
                                this.updatePosition = Vector2.Zero;
                            }
                        }

                        #endregion

                        #region Movement

                        var location = Notifications.GetLocation();
                        if (location != -0x1 && this.position.Y > location)
                        {
                            if (this.updatePosition != Vector2.Zero && this.textFix != Vector2.Zero)
                            {
                                this.position.X = this.textFix.X;
                                this.textFix = Vector2.Zero;
                                this.line.Width = 190f;
                            }

                            this.updatePosition = new Vector2(this.position.X, Notifications.GetLocation(this));
                            this.state = NotificationState.AnimationMove;
                            this.moveStartT = Utils.GameTimeTickCount;
                            this.moveStartPosition = this.position;
                        }

                        #endregion

                        break;
                    }
                case NotificationState.AnimationMove:
                    {
                        #region Movement

                        if (Math.Abs(this.position.Y - this.updatePosition.Y) > float.Epsilon)
                        {
                            var percentT = Math.Min(1, ((float)Utils.GameTimeTickCount - this.moveStartT) / 500);

                            this.position.Y = this.moveStartPosition.Y
                                              + (this.updatePosition.Y - this.moveStartPosition.Y) * percentT;
                        }
                        else
                        {
                            this.updatePosition = Vector2.Zero;
                            this.state = NotificationState.Idle;
                        }

                        #endregion

                        break;
                    }
                case NotificationState.AnimationShowShrink:
                    {
                        #region Shrink

                        if (Math.Abs(this.line.Width - 0xB9) < float.Epsilon)
                        {
                            this.state = NotificationState.AnimationShowMove;
                            this.updatePosition = new Vector2(this.position.X, Notifications.GetLocation(this));
                            return;
                        }
                        this.line.Width--;
                        this.position.X++;

                        #endregion

                        break;
                    }
                case NotificationState.AnimationShowMove:
                    {
                        #region Movement

                        if (Math.Abs(Notifications.GetLocation() + 0x1E - this.updatePosition.Y) < float.Epsilon)
                        {
                            this.updatePosition.Y = Notifications.GetLocation();
                        }

                        if (Math.Abs(this.position.Y - this.updatePosition.Y) > float.Epsilon)
                        {
                            var value =
                                (this.updatePosition.Distance(new Vector2(this.position.X, this.position.Y - 0.5f))
                                 < this.updatePosition.Distance(new Vector2(this.position.X, this.position.Y + 0.5f)))
                                    ? -0.5f
                                    : 0.5f;
                            this.position.Y += value;
                        }
                        else
                        {
                            this.updatePosition = Vector2.Zero;
                            this.state = NotificationState.AnimationShowGrow;
                        }

                        #endregion

                        break;
                    }
                case NotificationState.AnimationShowGrow:
                    {
                        #region Growth

                        if (Math.Abs(this.line.Width - 0xBE) < float.Epsilon)
                        {
                            this.state = NotificationState.Idle;
                            return;
                        }
                        this.line.Width++;
                        this.position.X--;

                        #endregion

                        break;
                    }
            }
        }

        /// <summary>
        ///     Called per Windows Message.
        /// </summary>
        /// <param name="args">WndEventArgs</param>
        public void OnWndProc(WndEventArgs args)
        {
            if (Utils.IsUnderRectangle(Utils.GetCursorPos(), this.position.X, this.position.Y, this.line.Width, 25f))
            {
                #region Mouse

                var message = (WindowsMessages)args.Msg;
                if (message == WindowsMessages.WM_LBUTTONDOWN)
                {
                    if (Utils.TickCount - this.clickTick < 0x5DC)
                    {
                        this.clickTick = Utils.TickCount;

                        Notifications.RemoveNotification(this);

                        this.Draw = this.Update = false;
                        if (this.autoDispose)
                        {
                            this.Dispose();
                        }
                        return;
                    }
                    this.clickTick = Utils.TickCount;
                }

                #endregion
            }
        }

        /// <summary>
        ///     Sets the notification border color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetBorderColor(Color color)
        {
            this.BorderColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        /// <summary>
        ///     Sets the notification box color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetBoxColor(Color color)
        {
            this.BoxColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        /// <summary>
        ///     Sets the notification text color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetTextColor(Color color)
        {
            this.TextColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        /// <summary>
        ///     Show an inactive Notification, returns boolean if successful or not.
        /// </summary>
        /// <param name="newDuration">Duration (-1 for Infinite)</param>
        /// <returns></returns>
        public bool Show(int newDuration = -0x1)
        {
            if (this.Draw || this.Update)
            {
                this.state = NotificationState.AnimationShowShrink;
                return false;
            }

            this.startT = Utils.GameTimeTickCount;
            this.duration = newDuration;

            this.TextColor.A = 0xFF;
            this.BoxColor.A = 0xFF;
            this.BorderColor.A = 0xFF;

            this.position = new Vector2(Drawing.Width - 200f, Notifications.GetLocation(this));

            return this.Draw = this.Update = true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Calculate the border into vertices
        /// </summary>
        /// <param name="x">X axis</param>
        /// <param name="y">Y axis</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <returns>Vector2 Array</returns>
        private static Vector2[] GetBorder(float x, float y, float w, float h)
        {
            return new[] { new Vector2(x + w / 0x2, y), new Vector2(x + w / 0x2, y + h) };
        }

        /// <summary>
        ///     Safe disposal callback
        /// </summary>
        /// <param name="safe">Is Pre-Finailized / Safe (values not cleared by GC)</param>
        private void Dispose(bool safe)
        {
            if (Notifications.IsValidNotification(this))
            {
                Notifications.RemoveNotification(this);
            }

            if (safe)
            {
                this.Text = null;

                this.TextColor = new ColorBGRA();
                this.BoxColor = new ColorBGRA();
                this.BorderColor = new ColorBGRA();

                this.Font.Dispose();
                this.Font = null;

                this.line.Dispose();
                this.sprite.Dispose();
                this.Draw = false;
                this.Update = false;

                this.duration = 0;

                Notifications.RemoveNotification(this);

                this.position = Vector2.Zero;
                this.updatePosition = Vector2.Zero;

                this.state = 0;

                this.textFix = Vector2.Zero;

                this.flashing = false;
                this.flashInterval = 0;
                this.flashTick = 0;
                this.clickTick = 0;

                this.border = false;
            }
        }

        #endregion
    }
}