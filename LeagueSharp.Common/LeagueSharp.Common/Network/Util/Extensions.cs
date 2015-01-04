using System;

namespace LeagueSharp.Network.Util
{
    public static class Extensions
    {
        public static int GetBits(this int aInt, int position, int length)
        {
            return aInt >> position & ((1 << length) - 1);
        }

        public static int GetBits(this ushort aInt, int position, int length)
        {
            return aInt >> position & ((1 << length) - 1);
        }

        public static ushort SetRange(this ushort valueToMod, int startIndex, int range, ushort rangeValueToAssign)
        {
            var endIndex = startIndex + range;
            // Determine max value
            var max_value = Convert.ToUInt16(Math.Pow(2.0, (endIndex - startIndex) + 1.0) - 1);
            if (rangeValueToAssign > max_value) throw new Exception("Value To Large For Range");
            // Clear our bits where we want to "Set" the value for
            for (var i = startIndex; i < endIndex; i++)
                valueToMod &= (ushort) ~(1 << i);
            // Shift the value and add it to the orignal (effect of setting range?)
            var value_to_add = (ushort) (rangeValueToAssign << startIndex);
            return (ushort) (valueToMod + value_to_add);
        }

        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static int RotateLeft(this int value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static ushort RotateLeft(this ushort value, int count)
        {
            return (ushort) ((value << count) | (value >> (16 - count)));
        }

        public static byte RotateLeft(this byte value, int count)
        {
            return (byte) ((value << count) | (value >> (8 - count)));
        }

        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public static int RotateRight(this int value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public static ushort RotateRight(this ushort value, int count)
        {
            return (ushort) ((value >> count) | (value << (16 - count)));
        }

        public static byte RotateRight(this byte value, int count)
        {
            return (byte) ((value >> count) | (value << (8 - count)));
        }
    }
}