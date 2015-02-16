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

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Basic Notification
    /// </summary>
    public class Notification : INotification
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
        public Notification(string text, int duration = -0x1)
        {
            // Setting GUID
            id = Guid.NewGuid().ToString("N");

            // Setting main values
            Text = text;
            state = NotificationState.Idle;

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
            if (draw || update)
            {
                state = NotificationState.AnimationShowShrink;
                return false;
            }

            var yAxis = Notifications.GetLocation();
            if (yAxis != -0x1)
            {
                handler = Notifications.Reserve(GetId(), handler);
                if (handler != null)
                {
                    duration = newDuration;

                    TextColor.A = 0xFF;
                    BoxColor.A = 0xFF;
                    BorderColor.A = 0xFF;

                    position = new Vector2(Drawing.Width - 200f, yAxis);

                    decreasementTick = GetNextDecreasementTick();

                    draw = update = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Enters Notification's flashing mode
        /// </summary>
        /// <param name="interval">Flash Interval</param>
        public void Flash(int interval = 0xFA)
        {
            flashing = !flashing;
            if (flashing)
            {
                flashInterval = interval;
            }
        }

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
            Drawing.Direct3DDevice, 0xE, 0x0, FontWeight.Bold, 0x0, false, FontCharacterSet.Default,
            FontPrecision.Default, FontQuality.Antialiased, FontPitchAndFamily.DontCare | FontPitchAndFamily.Decorative,
            "Tahoma");

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
        ///     Locally saved bool, indicating if OnDraw should be executed/processed.
        /// </summary>
        private bool draw;

        /// <summary>
        ///     Locally saved bool, indicating if OnUpdate should be executed/processed.
        /// </summary>
        private bool update;

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
        ///     Locally saved float which indicates how much text overflow is allowed.
        /// </summary>
        private int overflowText;

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

        #endregion

        #region Required Functions

        /// <summary>
        ///     Called for Drawing onto screen
        /// </summary>
        public void OnDraw()
        {
            if (!draw)
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

            #endregion

            #region Text

            var text = (Text.Length > 0x1B)
                ? Text.Substring(0x0, 0x18 + overflowText) + ((Text.Length - 0x1B != overflowText) ? "..." : "")
                : Text;

            sprite.Begin();

            var textDimension = Font.MeasureText(sprite, text, 0x0);
            var rectangle = new Rectangle((int) position.X, (int) position.Y, (int) line.Width, 0x19);

            Font.DrawText(
                sprite, text, rectangle.TopLeft.X + (rectangle.Width - textDimension.Width) / 0x2,
                rectangle.TopLeft.Y + (rectangle.Height - textDimension.Height) / 0x2, TextColor);

            sprite.End();

            #endregion
        }

        /// <summary>
        ///     Called per game tick for update
        /// </summary>
        public void OnUpdate()
        {
            if (!update)
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
                        update = false;
                        draw = false;
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
                                        update = false;
                                        draw = false;
                                        Notifications.Free(handler);
                                        return;
                                    }
                                }

                                flashingBytes[0x0] = TextColor.A;
                                flashingBytes[0x1] = BoxColor.A;
                                flashingBytes[0x2] = BorderColor.A;

                                --flashingBytes[0x0];
                                --flashingBytes[0x1];
                                --flashingBytes[0x2];

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
                                        update = false;
                                        draw = false;
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

                        if (Text.Length > 0x1B)
                        {
                            var textDimension = Font.MeasureText(sprite, Text, 0x0);
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
                            if (Math.Abs(position.X - updatePosition.X) < float.Epsilon && overflowText < Text.Length - 0x1B)
                            {
                                ++overflowText;
                            }
                        }
                    }
                    else if (updatePosition != Vector2.Zero)
                    {
                        if (position.X < textFix.X)
                        {
                            position.X += 1f;
                            line.Width -= 1f;
                            if (overflowText > 0x0)
                            {
                                --overflowText;
                            }
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
                                    overflowText = 0x0;
                                }
                                updatePosition = new Vector2(position.X, location);
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
                        var yAxis = Notifications.GetLocation();
                        if (yAxis != -0x1)
                        {
                            handler = Notifications.Reserve(GetId(), handler);
                            if (handler != null)
                            {
                                state = NotificationState.AnimationShowMove;
                                updatePosition = new Vector2(position.X, yAxis);
                            }
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
            // Unused Currently.
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
    }
}