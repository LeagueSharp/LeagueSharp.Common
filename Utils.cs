#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Utils.cs is part of LeagueSharp.Common.
 
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
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
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
        WM_KEYUP = 0x101
    }

    public enum MouseEvents
    {
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
    }

    public enum KeyboardEvents
    {
        KEYBDEVENTF_SHIFTVIRTUAL = 0x10,
        KEYBDEVENTF_SHIFTSCANCODE = 0x2A,
        KEYBDEVENTF_KEYDOWN = 0,
        KEYBDEVENTF_KEYUP = 2
    }

    /// <summary>
    ///     This class offers real mouse clicks.
    /// </summary>
    public static class VirtualMouse
    {
        public static int clickdelay;
        public static int attkdelay;
        public static bool disableOrbClick = false; //if set to true, orbwalker won't send right clicks - for other scripts
        public static int coordX;
        public static int coordY;

        // mouse event
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        // keyboard event
        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);
        // simulates a click-and-release action of the right mouse button at its current position
        public static void RightClick()
        {
                mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, coordX, coordY, 0, 0);
                mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, coordX, coordY, 0, 0);
        }

        public static void RightClick(Vector2 position)
        {
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, (int)position.X, (int)position.Y, 0, 0);
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, (int)position.X, (int)position.Y, 0, 0);
        }

        public static void RightClick(Vector3 gamePosition)
        {
            RightClick(Drawing.WorldToScreen(gamePosition));
        }

        public static void ShiftClick()
        {
            keybd_event((int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL, (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE, (int)KeyboardEvents.KEYBDEVENTF_KEYDOWN, 0);
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, coordX, coordY, 0, 0);
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, coordX, coordY, 0, 0);
            Utility.DelayAction.Add(200, () => { keybd_event((int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL, (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE, (int)KeyboardEvents.KEYBDEVENTF_KEYUP, 0); });
        }

        public static void ShiftClick(Vector2 position)
        {
            keybd_event((int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL, (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE, (int)KeyboardEvents.KEYBDEVENTF_KEYDOWN, 0);
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, (int)position.X, (int)position.Y, 0, 0);
            mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, (int)position.X, (int)position.Y, 0, 0);
            Utility.DelayAction.Add(200, () => { keybd_event((int)KeyboardEvents.KEYBDEVENTF_SHIFTVIRTUAL, (int)KeyboardEvents.KEYBDEVENTF_SHIFTSCANCODE, (int)KeyboardEvents.KEYBDEVENTF_KEYUP, 0); });
        }

        public static void ShiftClick(Vector3 gamePosition)
        {
            ShiftClick(Drawing.WorldToScreen(gamePosition));
        }
    }

    /// <summary>
    ///     Non game related utilities.
    /// </summary>
    public static class Utils
    {
        private const int STD_INPUT_HANDLE = -10;
        private const int ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;

        // Convert an object to a byte array
        internal static byte[] Serialize(Object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var bf = new BinaryFormatter();
            var ms = new System.IO.MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        internal static T Deserialize<T>(byte[] arrBytes)
        {
            var memStream = new System.IO.MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            return (T)binForm.Deserialize(memStream);
        }

        public static byte FixVirtualKey(byte key)
        {
            switch (key)
            {
                case 160:
                    return 0x10;
                case 161:
                    return 0x10;
                case 162:
                    return 0x11;
                case 163:
                    return 0x11;
            }

            return key;
        }

        public static int TickCount
        {
            get { return Environment.TickCount & int.MaxValue; }
        }

        /// <summary>
        ///     Returns the cursor position on the screen.
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
                return ((char)vKey).ToString();
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
        ///     Returns the md5 hash from a string.
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
        ///     Returns true if the point is under the rectangle
        /// </summary>
        public static bool IsUnderRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return (point.X > x && point.X < x + width && point.Y > y && point.Y < y + height);
        }

        public static string FormatTime(double time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        ///     Searches in the haystack array for the given needle using the default equality operator and returns the index at
        ///     which the needle starts.
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

        public static void ClearConsole()
        {
            try
            {
                var window_height = Console.WindowHeight;
                Console.Clear();
            }
            catch { }
        }

        /// <summary>
        ///     Returns the directory where the assembly is located
        /// </summary>
        public static string GetLocation()
        {
            var FileLoc = Assembly.GetExecutingAssembly().Location;
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
        ///     Allows text in the console to be selected and copied.
        /// </summary>
        public static void EnableConsoleEditMode()
        {
            /*
            int mode;
            var handle = GetStdHandle(STD_INPUT_HANDLE);
            GetConsoleMode(handle, out mode);
            mode |= ENABLE_QUICK_EDIT_MODE;
            SetConsoleMode(handle, mode);*/
        }

        public static double NextDouble(this Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
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
                if (args.Msg == (uint)WindowsMessages.WM_MOUSEMOVE)
                {
                    _posX = unchecked((short)args.LParam);
                    _posY = unchecked((short)((long)args.LParam >> 16));
                }
            }

            internal static Vector2 GetCursorPos()
            {
                return new Vector2(_posX, _posY);
            }
        }
    }

    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Searches for an element that matches the conditions defined by the specified predicate, and returns the first
        ///     occurrence within the entire IEnumerable.
        /// </summary>
        public static TSource Find<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            return (source as List<TSource> ?? source.ToList()).Find(match);
        }

        /// <summary>
        ///     Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        [Obsolete("Use IEnumerable<TSource>.Where() instead", false)]
        public static List<TSource> FindAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where(predicate).ToList();
        }

        public static T MaxOrDefault<T, R>(this IEnumerable<T> container, Func<T, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) > 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }

        public static T MinOrDefault<T, R>(this IEnumerable<T> container, Func<T, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var minElem = enumerator.Current;
            var minVal = valuingFoo(minElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(minVal) < 0)
                {
                    minVal = currVal;
                    minElem = enumerator.Current;
                }
            }

            return minElem;
        }
    }

    public static class WeightedRandom
    {
        public static Random Random = new Random(Utils.TickCount);

        public static int Next(int min, int max)
        {
            var list = new List<int>();
            list.AddRange(Enumerable.Range(min, max));

            var mean = list.Average();
            var stdDev = list.StandardDeviation();

            var v1 = Random.NextDouble();
            var v2 = Random.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(v1)) *
                         Math.Sin(2.0 * Math.PI * v2);
            return (int)(mean + stdDev * randStdNormal);
        }

        public static double StandardDeviation(this IEnumerable<int> values)
        {
            var enumerable = values as int[] ?? values.ToArray();
            var avg = enumerable.Average();
            return Math.Sqrt(enumerable.Average(v => Math.Pow(v - avg, 2)));
        }
    }
}
