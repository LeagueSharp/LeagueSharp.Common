#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace LeagueSharp.Common
{
    public enum WindowsMessages
    {
        WmLbuttondblclck = 0x203,
        WmRbuttondblclck = 0x206,
        WmMbuttondblclck = 0x209,
        WmMbuttondown = 0x207,
        WmMbuttonup = 0x208,
        WmMousemove = 0x200,
        WmLbuttondown = 0x201,
        WmLbuttonup = 0x202,
        WmRbuttondown = 0x204,
        WmRbuttonup = 0x205,
        WmKeydown = 0x0100,
        WmKeyup = 0x101
    }

    /// <summary>
    ///     Non game related utilities.
    /// </summary>
    public static class Utils
    {
        private const int StdInputHandle = -10;
        private const int EnableQuickEditMode = 0x40 | 0x80;

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
                return ((char) vKey).ToString();
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
            var bytes = new byte[str.Length*sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length/sizeof (char)];
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
                var windowHeight = Console.WindowHeight;
                Console.Clear();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Returns the directory where the assembly is located
        /// </summary>
        public static string GetLocation()
        {
            var fileLoc = Assembly.GetExecutingAssembly().Location;
            return fileLoc.Remove(fileLoc.LastIndexOf("\\", StringComparison.Ordinal));
        }

        public static string ToHexString(this byte bit)
        {
            return BitConverter.ToString(new[] {bit});
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
            int mode;
            var handle = GetStdHandle(StdInputHandle);
            GetConsoleMode(handle, out mode);
            mode |= EnableQuickEditMode;
            SetConsoleMode(handle, mode);
        }

        public static double NextDouble(this Random rng, double min, double max)
        {
            return min + (rng.NextDouble()*(max - min));
        }

        private static class CursorPosT
        {
            private static int _posX;
            private static int _posY;

            static CursorPosT()
            {
                Game.OnWndProc += Game_OnWndProc;
            }

            private static void Game_OnWndProc(WndEventArgs args)
            {
                if (args.Msg != (uint) WindowsMessages.WmMousemove) return;
                _posX = unchecked((short) args.LParam);
                _posY = unchecked((short) ((long) args.LParam >> 16));
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
        public static List<TSource> FindAll<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            return (source as List<TSource> ?? source.ToList()).FindAll(match);
        }

        public static T MaxOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo)
            where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                return default(T);

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) <= 0) continue;
                maxVal = currVal;
                maxElem = enumerator.Current;
            }

            return maxElem;
        }

        public static T MinOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo)
            where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                return default(T);

            var minElem = enumerator.Current;
            var minVal = valuingFoo(minElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(minVal) >= 0) continue;
                minVal = currVal;
                minElem = enumerator.Current;
            }

            return minElem;
        }
    }
}