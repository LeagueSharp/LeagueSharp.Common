// <copyright file="Utils.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    using SharpDX;

    /// <summary>
    ///     The non game related utilities.
    /// </summary>
    public static class Utils
    {
        #region Public Properties

        /// <summary>
        ///     Gets the game time tick count.
        /// </summary>
        public static int GameTimeTickCount => (int)(Game.Time * 1000);

        /// <summary>
        ///     Gets the tick count.
        /// </summary>
        public static int TickCount => Environment.TickCount & int.MaxValue;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Clears the console, (if available).
        /// </summary>
        public static void ClearConsole()
        {
            try
            {
                Console.WriteLine(Console.WindowHeight);
                Console.Clear();
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        ///     Enables the console edit mode, use with caution.
        /// </summary>
        public static void EnableConsoleEditMode()
        {
            const int EnableQuickEditMode = 0x40 | 0x80;
            const int StdInputHandle = -0xA;

            int mode;
            var handle = NativeMethods.GetStdHandle(StdInputHandle);
            NativeMethods.GetConsoleMode(handle, out mode);
            mode |= EnableQuickEditMode;
            NativeMethods.SetConsoleMode(handle, mode);
        }

        /// <summary>
        ///     Fixes the virtual key.
        /// </summary>
        /// <param name="key">
        ///     The virtual key.
        /// </param>
        /// <returns>
        ///     The fixed virtual key.
        /// </returns>
        public static byte FixVirtualKey(byte key)
        {
            switch (key)
            {
                case 160:
                case 161:
                    return 0x10;
                case 162:
                case 163:
                    return 0x11;
                default:
                    return key;
            }
        }

        /// <summary>
        ///     Formats the given time.
        /// </summary>
        /// <param name="time">
        ///     The time.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string FormatTime(double time)
        {
            var t = TimeSpan.FromSeconds(time);
            return $"{t.Minutes:D2}:{t.Seconds:D2}";
        }

        /// <summary>
        ///     Gets the <see cref="byte" /> array from the string.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <returns>
        ///     The <see cref="byte" /> array.
        /// </returns>
        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        ///     Returns the cursor position on the screen.
        /// </summary>
        /// <returns>
        ///     The <see cref="Vector2" />.
        /// </returns>
        public static Vector2 GetCursorPos()
        {
            return Cursor.GetCursorPos();
        }

        /// <summary>
        ///     Returns the directory where the assembly is located.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetLocation()
        {
            var fileLoc = Assembly.GetExecutingAssembly().Location;
            return fileLoc?.Remove(fileLoc.LastIndexOf("\\", StringComparison.Ordinal));
        }

        /// <summary>
        ///     Gets the string from the <see cref="byte" /> array.
        /// </summary>
        /// <param name="bytes">
        ///     The <see cref="byte" /> array.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
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
        /// <typeparam name="T">
        ///     Type of the arrays.
        /// </typeparam>
        /// <param name="haystack">
        ///     Sequence to operate on.
        /// </param>
        /// <param name="needle">
        ///     Sequence to search for.
        /// </param>
        /// <returns>
        ///     Index of the needle within the haystack or -1 if the needle isn't contained.
        /// </returns>
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
        ///     Indicates whether the given point is under the given rectangle.
        /// </summary>
        /// <param name="point">
        ///     The point.
        /// </param>
        /// <param name="x">
        ///     The rectangle X.
        /// </param>
        /// <param name="y">
        ///     The rectangle Y.
        /// </param>
        /// <param name="width">
        ///     The rectangle width.
        /// </param>
        /// <param name="height">
        ///     The rectangle height.
        /// </param>
        /// <returns>
        ///     A value indicating whether the point is under the rectangle.
        /// </returns>
        public static bool IsUnderRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return point.X > x && point.X < x + width && point.Y > y && point.Y < y + height;
        }

        /// <summary>
        ///     Transforms the virtual key to text.
        /// </summary>
        /// <param name="vKey">
        ///     The virtual key.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
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
                return "F" + (vKey - 111);
            }

            switch (vKey)
            {
                case 0:
                    return "None";
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
        ///     Creates a md5hash from the string.
        /// </summary>
        /// <param name="s">
        ///     The string.
        /// </param>
        /// <returns>
        ///     The hashed string.
        /// </returns>
        public static string Md5Hash(string s)
        {
            var sb = new StringBuilder();
            HashAlgorithm algorithm = MD5.Create();
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Generates a next random double.
        /// </summary>
        /// <param name="rng">
        ///     The random instance.
        /// </param>
        /// <param name="min">
        ///     The min double number.
        /// </param>
        /// <param name="max">
        ///     The max double number.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public static double NextDouble(this Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }

        /// <summary>
        ///     Converts the byte into a hex string.
        /// </summary>
        /// <param name="bit">
        ///     The byte.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string ToHexString(this byte bit)
        {
            return BitConverter.ToString(new[] { bit });
        }

        #endregion
    }
}