#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Notification.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.IO;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Basic Notification
    /// </summary>
    public class Notification : INotification, IDisposable
    {
        #region Other

        public enum NotificationState
        {
            Idle,
            AnimationMove,
            AnimationShowShrink,
            AnimationShowMove,
            AnimationShowGrow
        }

        #endregion

        /// <summary>
        ///     Notification Constructor
        /// </summary>
        /// <param name="text">Display Text</param>
        /// <param name="duration">Duration (-1 for Infinite)</param>
        /// <param name="dispose">Auto Dispose after notification duration end</param>
        public Notification(string text, int duration = -0x1, bool dispose = false)
        {
            // Setting GUID
            id = Guid.NewGuid().ToString("N");

            // Setting main values
            Text = text;
            state = NotificationState.Idle;
            border = true;
            autoDispose = dispose;

            // Preload Text
            Font.PreloadText(text);

            // Calling Show
            Show(duration);
        }

        #region Functions

        /// <summary>
        ///     Show an inactive Notification, returns boolean if successful or not.
        /// </summary>
        /// <param name="newDuration">Duration (-1 for Infinite)</param>
        /// <returns></returns>
        public bool Show(int newDuration = -0x1)
        {
            if (Draw || Update)
            {
                state = NotificationState.AnimationShowShrink;
                return false;
            }

            handler = Notifications.Reserve(GetId(), handler);
            if (handler != null)
            {
                duration = newDuration;

                TextColor.A = 0xFF;
                BoxColor.A = 0xFF;
                BorderColor.A = 0xFF;

                position = new Vector2(Drawing.Width - 200f, Notifications.GetLocation(handler));

                decreasementTick = GetNextDecreasementTick();

                return Draw = Update = true;
            }

            return false;
        }

        /// <summary>
        ///     Enters Notification's flashing mode
        /// </summary>
        /// <param name="interval">Flash Interval</param>
        public Notification Flash(int interval = 0xFA)
        {
            flashing = !flashing;
            if (flashing)
            {
                flashInterval = interval;
            }

            return this;
        }

        /// <summary>
        ///     Toggles the notification border
        /// </summary>
        public Notification Border()
        {
            border = !border;

            return this;
        }

        /// <summary>
        ///     Sets the notification border toggle value
        /// </summary>
        /// <param name="value">bool value</param>
        public Notification Border(bool value)
        {
            border = value;

            return this;
        }

        #region Color Set

        /// <summary>
        ///     Sets the notification text color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetTextColor(Color color)
        {
            TextColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        /// <summary>
        ///     Sets the notification box color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetBoxColor(Color color)
        {
            BoxColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        /// <summary>
        ///     Sets the notification border color
        /// </summary>
        /// <param name="color">System Drawing Color</param>
        public Notification SetBorderColor(Color color)
        {
            BorderColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            return this;
        }

        #endregion

        /// <summary>
        ///     Calculate the next decreasement tick.
        /// </summary>
        /// <returns>Decreasement Tick</returns>
        private int GetNextDecreasementTick()
        {
            return Utils.TickCount + ((duration / 0xFF));
        }

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

        #endregion

        #region Public Fields

        /// <summary>
        ///     Notification's Text
        /// </summary>
        public string Text;

        #region Colors

        /// <summary>
        ///     Notification's Text Color
        /// </summary>
        public ColorBGRA TextColor = new ColorBGRA(255f, 255f, 255f, 255f);

        /// <summary>
        ///     Notification's Box Color
        /// </summary>
        public ColorBGRA BoxColor = new ColorBGRA(0f, 0f, 0f, 255f);

        /// <summary>
        ///     Notification's Border Color
        /// </summary>
        public ColorBGRA BorderColor = new ColorBGRA(255f, 255f, 255f, 255f);

        /// <summary>
        ///     Notification's Font
        /// </summary>
        public Font Font = new Font(
            Drawing.Direct3DDevice, 0xE, 0x0, FontWeight.DoNotCare, 0x0, false, FontCharacterSet.Default,
            FontPrecision.Default, FontQuality.Antialiased, FontPitchAndFamily.DontCare | FontPitchAndFamily.Decorative,
            "Tahoma");

        /// <summary>
        ///     Indicates if notification should be drawn
        /// </summary>
        public bool Draw { get; set; }

        /// <summary>
        ///     Indicates if notification should be updated
        /// </summary>
        public bool Update { get; set; }

        #endregion

        #endregion

        #region Private Fields

        /// <summary>
        ///     Locally saved Global Unique Identification (GUID)
        /// </summary>
        private readonly string id;

        /// <summary>
        ///     Locally saved Notification's Duration
        /// </summary>
        private int duration;

        /// <summary>
        ///     Locally saved handler for FileStream.
        /// </summary>
        private FileStream handler;

        /// <summary>
        ///     Locally saved position
        /// </summary>
        private Vector2 position;

        /// <summary>
        ///     Locally saved update position
        /// </summary>
        private Vector2 updatePosition;

        /// <summary>
        ///     Locally saved Notification State
        /// </summary>
        private NotificationState state;

        /// <summary>
        ///     Locally saved value, indicating when next decreasment tick should happen.
        /// </summary>
        private int decreasementTick;

        /// <summary>
        ///     Locally saved Line
        /// </summary>
        private readonly Line line = new Line(Drawing.Direct3DDevice)
        {
            Antialias = false,
            GLLines = true,
            Width = 190f
        };

        /// <summary>
        ///     Locally saved Sprite
        /// </summary>
        private readonly Sprite sprite = new Sprite(Drawing.Direct3DDevice);

        /// <summary>
        ///     Locally saved boolean for Text Fix
        /// </summary>
        private Vector2 textFix;

        /// <summary>
        ///     Locally saved bool which indicates if flashing mode is on or off.
        /// </summary>
        private bool flashing;

        /// <summary>
        ///     Locally saved bytes which contain old ALPHA values
        /// </summary>
        private readonly byte[] flashingBytes = new byte[3];

        /// <summary>
        ///     Locally saved int which contains an internval for flash mode
        /// </summary>
        private int flashInterval;

        /// <summary>
        ///     Locally saved int which contains next flash mode tick
        /// </summary>
        private int flashTick;

        /// <summary>
        ///     Locally saved int which contains data of the last tick.
        /// </summary>
        private int clickTick;

        /// <summary>
        ///     Locally saved bool which indicates if border should be drawn
        /// </summary>
        private bool border;

        /// <summary>
        ///     Locally saved bool which indicates if notification will be disposed after finishing
        /// </summary>
        private readonly bool autoDispose;

        #endregion

        #region Required Functions

        /// <summary>
        ///     Called for Drawing onto screen
        /// </summary>
        public void OnDraw()
        {
            if (!Draw)
            {
                return;
            }

            #region Box

            line.Begin();

            var vertices = new[]
            {
                new Vector2(position.X + line.Width / 0x2, position.Y),
                new Vector2(position.X + line.Width / 0x2, position.Y + 25f)
            };

            line.Draw(vertices, BoxColor);
            line.End();

            #endregion

            #region Outline

            if (border)
            {
                var x = position.X;
                var y = position.Y;
                var w = line.Width;
                const float h = 25f;
                const float px = 1f;

                line.Begin();
                line.Draw(GetBorder(x, y, w, px), BorderColor); // TOP
                line.End();

                var oWidth = line.Width;
                line.Width = px;

                line.Begin();
                line.Draw(GetBorder(x, y, px, h), BorderColor); // LEFT
                line.Draw(GetBorder(x + w, y, 1, h), BorderColor); // RIGHT
                line.End();

                line.Width = oWidth;

                line.Begin();
                line.Draw(GetBorder(x, y + h, w, 1), BorderColor); // BOTTOM
                line.End();
            }

            #endregion

            #region Text

            sprite.Begin();

            var textDimension = Font.MeasureText(sprite, Text);
            var finalText = Text;

            if (textDimension.Width + 0x5 > line.Width)
            {
                for (var i = Text.Length; i > 0x0; --i)
                {
                    var text = Text.Substring(0x0, i);
                    var textWidth = Font.MeasureText(sprite, text).Width;

                    if (textWidth + 0x5 > line.Width)
                    {
                        continue;
                    }

                    finalText = (text == Text) ? text : text.Substring(0x0, text.Length - 0x3) + "...";
                    break;
                }
            }

            textDimension = Font.MeasureText(sprite, finalText);

            var rectangle = new Rectangle((int) position.X, (int) position.Y, (int) line.Width, 0x19);
           
            Font.DrawText(
                sprite, finalText, rectangle.TopLeft.X + (rectangle.Width - textDimension.Width) / 0x2,
                rectangle.TopLeft.Y + (rectangle.Height - textDimension.Height) / 0x2, TextColor);

            sprite.End();

            #endregion
        }

        public void OnPreReset()
        {
            Font.OnLostDevice();
            line.OnLostDevice();
            sprite.OnLostDevice();
        }

        public void OnPostReset()
        {
            line.OnResetDevice();
            Font.OnResetDevice();
            sprite.OnResetDevice();
        }

        /// <summary>
        ///     Called per game tick for update
        /// </summary>
        public void OnUpdate()
        {
            if (!Update)
            {
                return;
            }

            switch (state)
            {
                case NotificationState.Idle:
                {
                    #region Duration End Handler

                    if (!flashing && duration > 0x0 && TextColor.A == 0x0 && BoxColor.A == 0x0 && BorderColor.A == 0x0)
                    {
                        Update = Draw = false;
                        if (autoDispose)
                        {
                            Dispose();
                        }

                        Notifications.Free(handler);

                        return;
                    }

                    #endregion

                    #region Decreasement Tick

                    if (!flashing && duration > 0x0 && Utils.TickCount - decreasementTick > 0x0)
                    {
                        if (TextColor.A > 0x0)
                        {
                            TextColor.A--;
                        }
                        if (BoxColor.A > 0x0)
                        {
                            BoxColor.A--;
                        }
                        if (BorderColor.A > 0x0)
                        {
                            BorderColor.A--;
                        }

                        decreasementTick = GetNextDecreasementTick();
                    }

                    #endregion

                    #region Flashing

                    if (flashing)
                    {
                        if (Utils.TickCount - flashTick > 0x0)
                        {
                            if (TextColor.A > 0x0 && BoxColor.A > 0x0 && BorderColor.A > 0x0)
                            {
                                if (duration > 0x0)
                                {
                                    if (TextColor.A == 0x0 && BoxColor.A == 0x0 && BorderColor.A == 0x0)
                                    {
                                        Update = Draw = false;
                                        if (autoDispose)
                                        {
                                            Dispose();
                                        }

                                        Notifications.Free(handler);

                                        return;
                                    }
                                }

                                flashingBytes[0x0] = --TextColor.A;
                                flashingBytes[0x1] = --BoxColor.A;
                                flashingBytes[0x2] = --BorderColor.A;

                                TextColor.A = 0x0;
                                BoxColor.A = 0x0;
                                BorderColor.A = 0x0;
                            }
                            else
                            {
                                TextColor.A = flashingBytes[0x0];
                                BoxColor.A = flashingBytes[0x1];
                                BorderColor.A = flashingBytes[0x2];

                                if (TextColor.A > 0x0)
                                {
                                    TextColor.A--;
                                }
                                if (BoxColor.A > 0x0)
                                {
                                    BoxColor.A--;
                                }
                                if (BorderColor.A > 0x0)
                                {
                                    BorderColor.A--;
                                }

                                if (duration > 0x0)
                                {
                                    if (TextColor.A == 0x0 && BoxColor.A == 0x0 && BorderColor.A == 0x0)
                                    {
                                        Update = Draw = false;
                                        if (autoDispose)
                                        {
                                            Dispose();
                                        }

                                        Notifications.Free(handler);

                                        return;
                                    }
                                }
                            }
                            flashTick = Utils.TickCount + flashInterval;
                        }
                    }

                    #endregion

                    #region Mouse

                    var mouseLocation = Drawing.WorldToScreen(Game.CursorPos);
                    if (Utils.IsUnderRectangle(mouseLocation, position.X, position.Y, line.Width, 25f))
                    {
                        TextColor.A = 0xFF;
                        BoxColor.A = 0xFF;
                        BorderColor.A = 0xFF;

                        var textDimension = Font.MeasureText(sprite, Text);
                        if (textDimension.Width + 0x10 > line.Width)
                        {
                            var extra = textDimension.Width - 0xB4;
                            if (updatePosition == Vector2.Zero)
                            {
                                textFix = new Vector2(position.X, position.Y);
                                updatePosition = new Vector2(position.X - extra, position.Y);
                            }
                            if (updatePosition != Vector2.Zero && position.X > updatePosition.X)
                            {
                                position.X -= 1f;
                                line.Width += 1f;
                            }
                        }
                    }
                    else if (updatePosition != Vector2.Zero)
                    {
                        if (position.X < textFix.X)
                        {
                            position.X += 1f;
                            line.Width -= 1f;
                        }
                        else
                        {
                            textFix = Vector2.Zero;
                            updatePosition = Vector2.Zero;
                        }
                    }

                    #endregion

                    #region Movement

                    var location = Notifications.GetLocation();
                    if (location != -0x1 && position.Y > location)
                    {
                        if (Notifications.IsFirst((int) position.Y))
                        {
                            handler = Notifications.Reserve(GetId(), handler);
                            if (handler != null)
                            {
                                if (updatePosition != Vector2.Zero && textFix != Vector2.Zero)
                                {
                                    position.X = textFix.X;
                                    textFix = Vector2.Zero;
                                    line.Width = 190f;
                                }

                                updatePosition = new Vector2(position.X, Notifications.GetLocation(handler));
                                state = NotificationState.AnimationMove;
                            }
                        }
                    }

                    #endregion

                    break;
                }
                case NotificationState.AnimationMove:
                {
                    #region Movement

                    if (Math.Abs(position.Y - updatePosition.Y) > float.Epsilon)
                    {
                        var value = (updatePosition.Distance(new Vector2(position.X, position.Y - 0x1)) <
                                     updatePosition.Distance(new Vector2(position.X, position.Y + 0x1)))
                            ? -0x1
                            : 0x1;
                        position.Y += value;
                    }
                    else
                    {
                        updatePosition = Vector2.Zero;
                        state = NotificationState.Idle;
                    }

                    #endregion

                    break;
                }
                case NotificationState.AnimationShowShrink:
                {
                    #region Shrink

                    if (Math.Abs(line.Width - 0xB9) < float.Epsilon)
                    {
                        handler = Notifications.Reserve(GetId(), handler);
                        if (handler != null)
                        {
                            state = NotificationState.AnimationShowMove;
                            updatePosition = new Vector2(position.X, Notifications.GetLocation(handler));
                        }
                        return;
                    }
                    line.Width--;
                    position.X++;

                    #endregion

                    break;
                }
                case NotificationState.AnimationShowMove:
                {
                    #region Movement

                    if (Math.Abs(Notifications.GetLocation() + 0x1E - updatePosition.Y) < float.Epsilon)
                    {
                        updatePosition.Y = Notifications.GetLocation();
                    }

                    if (Math.Abs(position.Y - updatePosition.Y) > float.Epsilon)
                    {
                        var value = (updatePosition.Distance(new Vector2(position.X, position.Y - 0.5f)) <
                                     updatePosition.Distance(new Vector2(position.X, position.Y + 0.5f)))
                            ? -0.5f
                            : 0.5f;
                        position.Y += value;
                    }
                    else
                    {
                        updatePosition = Vector2.Zero;
                        state = NotificationState.AnimationShowGrow;
                    }

                    #endregion

                    break;
                }
                case NotificationState.AnimationShowGrow:
                {
                    #region Growth

                    if (Math.Abs(line.Width - 0xBE) < float.Epsilon)
                    {
                        state = NotificationState.Idle;
                        return;
                    }
                    line.Width++;
                    position.X--;

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
            if (Utils.IsUnderRectangle(Drawing.WorldToScreen(Game.CursorPos), position.X, position.Y, line.Width, 25f))
            {
                #region Mouse

                var message = (WindowsMessages) args.Msg;
                if (message == WindowsMessages.WM_LBUTTONDOWN)
                {
                    if (Utils.TickCount - clickTick < 0x5DC)
                    {
                        clickTick = Utils.TickCount;

                        Notifications.Free(handler);

                        Draw = Update = false;
                        if (autoDispose)
                        {
                            Dispose();
                        }
                        return;
                    }
                    clickTick = Utils.TickCount;
                }

                #endregion
            }
        }

        /// <summary>
        ///     Returns the notification's global unique identification (GUID)
        /// </summary>
        /// <returns>GUID</returns>
        public string GetId()
        {
            return id;
        }

        #endregion

        #region Disposal

        /// <summary>
        ///     IDisposable callback
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                Text = null;

                TextColor = new ColorBGRA();
                BoxColor = new ColorBGRA();
                BorderColor = new ColorBGRA();

                Font.Dispose();
                Font = null;

                line.Dispose();
                sprite.Dispose();
                Draw = false;
                Update = false;

                duration = 0;

                if (handler != null)
                {
                    Notifications.Free(handler);
                }

                position = Vector2.Zero;
                updatePosition = Vector2.Zero;

                state = 0;
                decreasementTick = 0;

                textFix = Vector2.Zero;

                flashing = false;
                flashInterval = 0;
                flashTick = 0;
                clickTick = 0;

                border = false;
            }
        }

        /// <summary>
        ///     Finalization
        /// </summary>
        ~Notification()
        {
            Dispose(false);
        }

        #endregion
    }
}