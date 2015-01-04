using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSharp.Network.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    class SerializeAttribute : Attribute
    {
        public int BitIndex { get; private set; }
        public int Bits { get; private set; }
        public uint[] Dict { get; private set; }
        public bool ReverseByteOrder { get; private set; }

        public SerializeAttribute(int bitIndex, int bits, uint[] dict = null, bool reverseByteOrder = false)
        {
            this.BitIndex = bitIndex;
            this.Bits = bits;
            this.Dict = dict;
            this.ReverseByteOrder = reverseByteOrder;
        }

        public SerializeAttribute(int bitIndex, int bits, uint cryptoRoutine, bool reverseByteOrder = false)
        {
            this.BitIndex = bitIndex;
            this.Bits = bits;
            this.Dict = new[] {cryptoRoutine};
            this.ReverseByteOrder = reverseByteOrder;
        }
    }
}
