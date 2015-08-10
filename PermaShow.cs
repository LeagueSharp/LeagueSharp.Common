#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 PermaShow.cs is part of LeagueSharp.Common.
 
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

using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D9;

namespace LeagueSharp.Common
{

    /// <summary>
    ///     The PermaShow class allows you to add important items to permashow easily.
    /// </summary>


    public static class PermaShow
    {

        private static bool subbed;
        /// <summary>
        ///    List of items for PermaShow
        /// </summary>

        private static List<PermaShowItem> PermaShowItems = new List<PermaShowItem>();

        public class PermaShowItem
        {
            public string DisplayName { get; set; }
            public MenuItem Item { get; set; }
            internal MenuValueType ItemType { get; set; }
            public Color Color { get; set; }

            /// <summary>
            ///    This is a bool MenuItem used to control the item's visibility from your an assembly.
            /// </summary>
            public MenuItem Controller { get; set; }
        }

        /// <summary>
        ///    Positioning
        /// </summary>

        private static Vector2 DefaultPosition = new Vector2(Drawing.Width - (0.79f * Drawing.Width), Drawing.Height - (0.98f * Drawing.Height));
        private static Vector2 BoxPosition;

        private static float XFactor = Drawing.Height / 768f;
        private static float YFactor = Drawing.Width / 1366f;

        private static float PermaShowWidth = 200f;

        /// <summary>
        ///   Drawing tools
        /// </summary>
        /// 
        private static Font Text;
        private static Line BoxLine;

        /// <summary>
        ///    Menu to save position and bool for moving the menu
        /// </summary>

        private static Menu placetosave;
        private static bool Dragging;


        /// <summary>
        ///    Constructor
        /// </summary>

        static PermaShow()
        {
            CreateMenu();
            BoxPosition = GetPosition();
            PrepareDrawing();
        }


        private static void Sub()
        {
            subbed = true;
            Drawing.OnPreReset += DrawingOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
            Game.OnWndProc += OnWndProc;
        }

        private static void Unsub()
        {
            subbed = false;
            Drawing.OnPreReset -= DrawingOnPreReset;
            Drawing.OnPostReset -= DrawingOnOnPostReset;
            Drawing.OnDraw -= Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomainOnDomainUnload;
            Game.OnWndProc -= OnWndProc;
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            PermaArea();

            var halfwidth = 0.9f * (PermaShowWidth / 2);

            foreach (var permaitem in PermaShowItems)
            {
                if (permaitem.Controller != null && !permaitem.Controller.GetValue<bool>())
                {
                    PermaShowItems.Remove(permaitem);
                    if (!PermaShowItems.Any())
                    {
                        Unsub();
                        return;
                    }
                    continue;
                }
                    var index = PermaShowItems.IndexOf(permaitem);
                    var baseposition = new Vector2(BoxPosition.X - ScaleValue(halfwidth, Direction.X),
                        BoxPosition.Y + ScaleValue(2, Direction.Y));
                    var endpos = new Vector2(BoxPosition.X + ScaleValue(halfwidth, Direction.X),
                        baseposition.Y + (Text.Description.Height * 1.3f * index));
                    var itempos = new Vector2(baseposition.X, baseposition.Y + (Text.Description.Height * 1.3f * index));
                    switch (permaitem.Item.ValueType)
                    {
                        case MenuValueType.Boolean:
                            DrawBox(endpos, permaitem.Item.GetValue<bool>());
                            Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<bool>(),
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.Slider:
                            Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<Slider>().Value,
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.KeyBind:
                            DrawBox(endpos, permaitem.Item.GetValue<KeyBind>().Active);
                            Text.DrawText(null,
                                permaitem.DisplayName + ": " + permaitem.Item.GetValue<KeyBind>().Active + " - " +
                                (permaitem.Item.GetValue<KeyBind>().Type), (int)itempos.X, (int)itempos.Y,
                                permaitem.Color);
                            break;
                        case MenuValueType.StringList:
                            Text.DrawText(null,
                                permaitem.DisplayName + ": " + permaitem.Item.GetValue<StringList>().SelectedValue,
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.Color:
                            Text.DrawText(null,
                                permaitem.DisplayName + ": " + permaitem.Item.GetValue<System.Drawing.Color>(),
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.Circle:
                            Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<Circle>().Color,
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.Integer:
                            Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<int>(),
                                (int)itempos.X, (int)itempos.Y, permaitem.Color);
                            break;
                        case MenuValueType.None:
                            break;
                    
                }
            }
        }

        /// <summary>
        ///    Adds a menuitem to PermaShow, can be used without any arguements or with if you want to customize. The bool can be set to false to remove the item from permashow. 
        ///    When removing, you can simply set the bool parameter to false and everything else can be null. The default color is White.
        /// </summary>

        public static void Permashow(this MenuItem item, bool enabled = true, String customdisplayname = null, Color? col = null)
        {
            if (enabled && PermaShowItems.All(x => x.Item != item))
            {
                if (!PermaShowItems.Any())
                {
                    Sub();
                }
                string dispName = customdisplayname ?? item.DisplayName;
                Color? color = col ?? new ColorBGRA(255, 255, 255, 255);
                PermaShowItems.Add(new PermaShowItem { DisplayName = dispName, Item = item, ItemType = item.ValueType, Color = (Color)color });
            }

            else if (!enabled)
            {
                var itemtoremove = PermaShowItems.FirstOrDefault(x => x.Item == item);
                if (itemtoremove != null)
                {
                    PermaShowItems.Remove(itemtoremove);
                    if (!PermaShowItems.Any())
                    {
                        Unsub();
                    }
                }
            }
        }

        /// <summary>
        ///    Adds a menuitem to PermaShow along with a controller MenuItem to control the visibility of this item.
        /// </summary>
        /// 
        public static MenuItem Permashow(this MenuItem item, MenuItem controlleritem, string customdisplayname = null, Color? col = null)
        {
            bool enabled = item.GetValue<bool>();
            if (enabled && PermaShowItems.All(x => x.Item != item))
            {
                if (!PermaShowItems.Any() && !subbed)
                {
                    Sub();
                }
                string dispName = customdisplayname ?? item.DisplayName;
                Color? color = col ?? new ColorBGRA(255, 255, 255, 255);
                PermaShowItems.Add(new PermaShowItem { DisplayName = dispName, Item = item, ItemType = item.ValueType, Color = (Color)color, Controller = controlleritem });
            }
            return item;
        }

        public static void Add(PermaShowItem permaitem)
        {
            if (!PermaShowItems.Contains(permaitem))
            {
                PermaShowItems.Add(permaitem);
            }
            else
            {
                throw new Exception("PermaShow: This item already exists");
            }
        }

        public static void Remove(PermaShowItem permaitem)
        {
            if (PermaShowItems.Contains(permaitem))
            {
                PermaShowItems.Remove(permaitem);
            }
            else
            {
                throw new Exception("PermaShow: The item you are trying to remove is non-existent.");
            }
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            Text.Dispose();
            BoxLine.Dispose();
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            Text.OnResetDevice();
            BoxLine.OnResetDevice();
        }

        private static void DrawingOnPreReset(EventArgs args)
        {
            Text.OnLostDevice();
            BoxLine.OnLostDevice();
        }

        /// <summary>
        ///    Dragging / Moving the menu
        /// </summary>

        private static void OnWndProc(WndEventArgs args)
        {
            if (Dragging)
            {
                BoxPosition = Utils.GetCursorPos();
            }

            if (MouseOverArea() && args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                if (!Dragging)
                {
                    Dragging = true;
                }
            }
            else if (Dragging && args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
            {
                Dragging = false;
                BoxPosition = Utils.GetCursorPos();
                SavePosition();
            }
        }

        private static bool MouseOverArea()
        {
            Vector2 pos = Utils.GetCursorPos();
            return ((pos.X >= BoxPosition.X - (PermaShowWidth / 2f)) && pos.X <= (BoxPosition.X + (PermaShowWidth / 2f)) &&
                pos.Y >= BoxPosition.Y && pos.Y <= (BoxPosition.Y + PermaShowItems.Count() * (Text.Description.Height)));
        }

        /// <summary>
        ///   Create the menu
        /// </summary>
        private static void CreateMenu()
        {
            placetosave = new Menu("PermaShow", "Permashow");
            MenuItem enablepermashow = new MenuItem("disablepermashow", "Enable PermaShow").SetValue(true);
            placetosave.AddItem(enablepermashow);
            var xvalue = new MenuItem("X", "X").SetValue(new Slider((int)DefaultPosition.X, 0, Drawing.Width));
            var yvalue = new MenuItem("Y", "Y").SetValue(new Slider((int)DefaultPosition.Y, 0, Drawing.Height));
            placetosave.AddItem(xvalue);
            placetosave.AddItem(yvalue);
            CommonMenu.Config.AddSubMenu(placetosave);

            enablepermashow.ValueChanged += (sender, args) =>
            {
                if (args.GetNewValue<bool>() && !subbed)
                {
                    Sub();
                }
                else if (!args.GetNewValue<bool>())
                {
                    Unsub();
                }
            };
        }

        /// <summary>
        ///   Save current position
        /// </summary>
        private static void SavePosition()
        {
            placetosave.Item("X")._valueSet = true;
            placetosave.Item("Y")._valueSet = true;
            placetosave.Item("X").SetValue(new Slider((int)BoxPosition.X, 0, Drawing.Width));
            placetosave.Item("Y").SetValue(new Slider((int)BoxPosition.Y, 0, Drawing.Height));
        }

        /// <summary>
        ///    Return current positions from file
        /// </summary>
        private static Vector2 GetPosition()
        {
            return new Vector2(placetosave.Item("X").GetValue<Slider>().Value, placetosave.Item("Y").GetValue<Slider>().Value);
        }

        /// <summary>
        ///     Draw area where text will be drawn
        /// </summary>
        private static void PermaArea()
        {

            BoxLine.Width = ScaleValue(PermaShowWidth, Direction.X);

            BoxLine.Begin();

            var pos = BoxPosition;

            var positions = new[]
            {
                new Vector2(pos.X, pos.Y),
                new Vector2(pos.X, pos.Y + PermaShowItems.Count() * (Text.Description.Height * 1.3f))
            };

            var col = Color.Black;

            BoxLine.Draw(positions, new ColorBGRA(col.B, col.G, col.R, 0.4f));

            BoxLine.End();
        }

        /// <summary>
        ///     Colored box to indicate booleans true or false with green and red respectively
        /// </summary>
        private static void DrawBox(Vector2 pos, bool ison)
        {

            BoxLine.Width = ScaleValue(20, Direction.X);

            BoxLine.Begin();

            var positions = new[]
            {
                new Vector2(pos.X, pos.Y),
                new Vector2(pos.X, pos.Y + (Text.Description.Height * 1.2f))
            };

            var col = ison ? Color.DarkGreen : Color.Red;

            BoxLine.Draw(positions, col);

            BoxLine.End();
        }


        private enum Direction
        {
            X,
            Y
        }

        private static float ScaleValue(float value, Direction direction)
        {
            var returnvalue = direction == Direction.X ? value * XFactor : value * YFactor;
            return returnvalue;
        }

        /// <summary>
        ///    Initialize the Drawing tools
        /// </summary>
        private static void PrepareDrawing()
        {
            int fontsize = (int)(ScaleValue(15, Direction.Y));

            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = fontsize < 15 ? 15 : fontsize,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });

            BoxLine = new Line(Drawing.Direct3DDevice) { Width = 1 };
        }
    }
}
