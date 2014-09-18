#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public enum WindowsMessages
    {
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_RBUTTONDOWN = 0x203,
        WM_RBUTTONUP = 0x202,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x101,
    }

    /// <summary>
    /// Non game related utilities.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns the cursor position on the screen.
        /// </summary>
        public static Vector2 GetCursorPos()
        {
            return CursorPosT.GetCursorPos();
        }


        public static string KeyToText(uint vKey)
        {
            /*A-Z */
            if (vKey >= 65 && vKey <= 90)
            {
                return ("" + (char) vKey);
            }

            /*F1-F12*/
            if (vKey >= 112 && vKey <= 123)
            {
                return ("F" + (vKey - 111));
            }

            switch (vKey)
            {
                case 9:
                    return "Tab";
                case 16:
                    return "Shift";
                case 17:
                    return "Ctrl";
                case 20:
                    return "CAPS";
                case 27:
                    return "ESC";
                case 32:
                    return "Space";
                case 45:
                    return "Insert";
                case 220:
                    return "º";
                default:
                    return vKey.ToString();
            }
        }

        /// <summary>
        /// Returns the md5 hash from a string.
        /// </summary>
        public static string Md5Hash(string s)
        {
            var sb = new StringBuilder();
            HashAlgorithm algorithm = MD5.Create();
            var h = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            foreach (var b in h)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }


        internal static class CursorPosT
        {
            private static int _posX;
            private static int _posY;

            static CursorPosT()
            {
                Game.OnWndProc += Game_OnWndProc;
            }

            private static void Game_OnWndProc(WndEventArgs args)
            {
                if (args.Msg == (uint) WindowsMessages.WM_MOUSEMOVE)
                {
                    _posX = unchecked((short) args.LParam);
                    _posY = unchecked((short) ((long) args.LParam >> 16));
                }
            }

            internal static Vector2 GetCursorPos()
            {
                return new Vector2(_posX, _posY);
            }
        }

        /// <summary>
        /// Returns true if the point is under the rectangle
        /// </summary>
        public static bool IsUnderRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return (point.X > x && point.X < x + width && point.Y > y && point.Y < y + height);
        }

        /// <summary>
        /// Returns the directory where the assembly is located
        /// </summary>
        public static string GetLocation()
        {
            string FileLoc;
            FileLoc = Assembly.GetExecutingAssembly().Location;
            return FileLoc.Remove(FileLoc.LastIndexOf("\\", StringComparison.Ordinal));
        }
    }
}