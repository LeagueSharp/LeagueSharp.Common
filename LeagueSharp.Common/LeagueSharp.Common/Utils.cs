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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public enum WindowsMessages
    {
        WM_LBUTTONDBLCLCK = 0x203,
        WM_RBUTTONDBLCLCK = 0x206,
        WM_MBUTTONDBLCLCK = 0x209,
        WM_MBUTTONDOWN = 0x207,
        WM_MBUTTONUP = 0x208,
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x101,
    }

    /// <summary>
    /// Non game related utilities.
    /// </summary>
    public static class Utils
    {
        private const int STD_INPUT_HANDLE = -10;
        private const int ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;

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


        /// <summary>
        /// Returns true if the point is under the rectangle
        /// </summary>
        public static bool IsUnderRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return (point.X > x && point.X < x + width && point.Y > y && point.Y < y + height);
        }

        public static string FormatTime(float time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static string FormatTime(double time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof (char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Searches in the haystack array for the given needle using the default equality operator and returns the index at which the needle starts.
        /// </summary>
        /// <typeparam name="T">Type of the arrays.</typeparam>
        /// <param name="haystack">Sequence to operate on.</param>
        /// <param name="needle">Sequence to search for.</param>
        /// <returns>Index of the needle within the haystack or -1 if the needle isn't contained.</returns>
        public static IEnumerable<int> IndexOf<T>(this T[] haystack, T[] needle)
        {
            if ((needle == null) || (haystack.Length < needle.Length))
            {
                yield break;
            }

            for (var l = 0; l < haystack.Length - needle.Length + 1; l++)
            {
                if (!needle.Where((data, index) => !haystack[l + index].Equals(data)).Any())
                {
                    yield return l;
                }
            }
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

        public static string ToHexString(this byte bit)
        {
            return BitConverter.ToString(new[] { bit });
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int handle);

        /// <summary>
        /// Allows text in the console to be selected and copied.
        /// <summary>
        public static void EnableConsoleEditMode()
        {
            int mode;
            var handle = GetStdHandle(STD_INPUT_HANDLE);
            GetConsoleMode(handle, out mode);
            mode |= ENABLE_QUICK_EDIT_MODE;
            SetConsoleMode(handle, mode);
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
    }
}