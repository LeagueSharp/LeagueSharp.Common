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
using Font = SharpDX.Direct3D9.Font;

namespace LeagueSharp.Common
{

    /// <summary>
    ///     The PermaShow class allows you to add important things to permashow easily.
    /// </summary>


    public static class PermaShow
    {

        /// <summary>
        ///    List of items for PermaShow
        /// </summary>
      
        public static List<PermaShowItem> PermaShowItems = new List<PermaShowItem>();
        

        public class PermaShowItem
        {
            public String DisplayName { get; set; }
            public MenuItem Item { get; set; }
            internal MenuValueType ItemType { get; set; }
            public SharpDX.Color Color { get; set; }
        }

        /// <summary>
        ///    Position where the text will start from  
        /// </summary>

        private static Vector2 DefaultPosition = new Vector2(Drawing.Width - (0.79f * Drawing.Width), Drawing.Height - (0.98f * Drawing.Height));
        private static Vector2 BoxPosition;

        /// <summary>
        ///   Drawing tools
        /// </summary>
        /// 
        public static Font Text;
        private static Line BoxLine;

        /// <summary>
        ///    Menu to save position and bool for moving the menu
        /// </summary>

        private static Menu placetosave;
        private static bool Dragging;

        /// <summary>
        ///    Constructor.
        /// </summary>

        static PermaShow()
        {
            CreateMenu();
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        /// <summary>
        ///    Initialization and subscribe to events
        /// </summary>
        
        static void OnLoad(EventArgs args)
        {
            BoxPosition = GetPosition();
            PrepareDrawing();
            Drawing.OnPreReset += DrawingOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
            Game.OnWndProc += OnWndProc;
        }


        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            PermaArea();

            foreach (var permaitem in PermaShowItems)
            {
                var index = PermaShowItems.IndexOf(permaitem);
                var baseposition = new Vector2(BoxPosition.X - 80, BoxPosition.Y);
                var itempos = new Vector2(baseposition.X, baseposition.Y + (Text.Description.Height * 1.3f * index));
                switch (permaitem.Item.ValueType)
                {
                    case MenuValueType.Boolean:
                        DrawRect(itempos, permaitem.Item.GetValue<bool>(), (int) (permaitem.Item.NeededWidth * 0.9f), Text.Description.Height);
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<bool>(), (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.Slider:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<Slider>().Value, (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.KeyBind:
                        DrawRect(itempos, permaitem.Item.GetValue<KeyBind>().Active, (int)(permaitem.Item.NeededWidth * 0.7f), Text.Description.Height);
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<KeyBind>().Active + " - " + (permaitem.Item.GetValue<KeyBind>().Type), (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.StringList:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<StringList>().SelectedValue, (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.Color:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<System.Drawing.Color>(), (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.Circle:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<Circle>().Color, (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.Integer:
                        Text.DrawText(null, permaitem.DisplayName + ": " + permaitem.Item.GetValue<int>(), (int)itempos.X, (int)itempos.Y, permaitem.Color);
                        break;
                    case MenuValueType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        ///    Adds a menuitem to PermaShow, can be used without any arguements or with if you want to customize. The bool can be set to false to remove the item from permashow. 
        ///    When removing, you can simply set the bool parameter to false and everything else can be null. The default color is White.
        /// </summary>

        public static void Permashow(this MenuItem item, bool enabled = true, String customdisplayname = null, SharpDX.Color? col = null)
        {

            if (enabled && !PermaShowItems.Any(x => x.Item == item))
            {
                String DispName = customdisplayname != null ? customdisplayname : item.DisplayName;
                SharpDX.Color? color = col != null ? col : new ColorBGRA(255, 255, 255, 255);
                PermaShowItems.Add(new PermaShowItem { DisplayName = DispName, Item = item, ItemType = item.ValueType, Color = (SharpDX.Color)color });
            }

            else if (!enabled)
            {
                var itemtoremove = PermaShowItems.FirstOrDefault(x => x.Item == item);
                if (itemtoremove != null)
                {
                    PermaShowItems.Remove(itemtoremove);
                }
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
            return ((pos.X >= BoxPosition.X - 100) && pos.X <= (BoxPosition.X + 100) &&
                pos.Y >= BoxPosition.Y && pos.Y <= (BoxPosition.Y + PermaShowItems.Count() * (Text.Description.Height * 1.3f)));
        }

        /// <summary>
        ///   Create the menu
        /// </summary>
        private static void CreateMenu()
        {
            placetosave = new Menu("PermaShow", "Permashow", true);
            var xvalue = new MenuItem("X", "X").SetValue(new Slider((int)DefaultPosition.X, 0, Drawing.Width));
            var yvalue = new MenuItem("Y", "Y").SetValue(new Slider((int)DefaultPosition.Y, 0, Drawing.Height));
            placetosave.AddItem(xvalue);
            placetosave.AddItem(yvalue);
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
            return new Vector2((float) placetosave.Item("X").GetValue<Slider>().Value, (float) placetosave.Item("Y").GetValue<Slider>().Value);
        }

        /// <summary>
        ///     Draw area where text will be drawn
        /// </summary>
        private static void PermaArea()
        {

            BoxLine.Width = 200;

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
        ///    Draw red/green rectangle based on bool value
        /// </summary>
        private static void DrawRect(Vector2 pos, bool on, int width, int height)
        {
            BoxLine.Width = 1;
            var position = new Vector2(pos.X - 5, pos.Y);
            BoxLine.Begin();
            BoxLine.Draw(new[] {
            new Vector2(position.X, position.Y),
            new Vector2(position.X + width, position.Y),
            new Vector2(position.X + width, position.Y + height),
            new Vector2(position.X, position.Y + height),
            new Vector2(position.X, position.Y), }, on ? SharpDX.Color.Green : SharpDX.Color.Red);
            BoxLine.End();
        }

        /// <summary>
        ///    Initialize the Drawing tools
        /// </summary>
        private static void PrepareDrawing()
        {
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = (int) (15f / 768f * (Drawing.Height)),
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });

            BoxLine = new Line(Drawing.Direct3DDevice) { Width = 1 };
        }
    }
}
