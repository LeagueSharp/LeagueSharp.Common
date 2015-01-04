using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using LeagueSharp.Network.Cryptography;
using LeagueSharp.Network.Util;
using SharpDX;

namespace LeagueSharp.Network.Serialization
{
    public class SerializedData<T>
    {
        private static readonly Random random = new Random();
        private readonly int bitIndex;
        private readonly int bits;
        private readonly List<uint> dict;

        public SerializedData(int bitIndex, int bits, List<uint> dict)
        {
            this.Data = default(T);
            this.bitIndex = bitIndex;
            this.bits = bits;
            this.dict = dict;
        }
        public SerializedData(int bitIndex, int bits)
        {
            this.Data = default(T);
            this.bitIndex = bitIndex;
            this.bits = bits;
        }

        public T Data { get; set; }

        public T Decode(ushort bitmask, BinaryReader reader)
        {
            var result = default(T);


            if (typeof (T) == typeof (Int32)
                || typeof (T) == typeof (Int16)
                || typeof (T) == typeof (Byte))
            {
                if (dict.Count == 8)
                {
                    var entry = (int) dict[bitmask.GetBits(bitIndex, bits)];

                    if (entry < -1 || entry > 7)
                    {
                        Serializer.Decode(out result, reader, Operations.GetOperations((uint) entry));
                    }
                    else
                    {
                        result = (T) (dynamic) entry;
                    }

                    return (Data = result);
                }
                else if(dict.Count == 1)
                {
                    if (bitmask.GetBits(bitIndex, bits) == 0)
                    {
                        Serializer.Decode(out result, reader, Operations.GetOperations((uint) dict[0]));
                        return (Data = result);
                    }
                    else
                    {
                        return (Data = (dynamic) 0);
                    }
                }
            }
            else if (typeof(T) == typeof(Vector3))
            {
                if (bitmask.GetBits(this.bitIndex, this.bits) == 0)
                {
                    Serializer.Decode(out result, reader, Operations.GetOperations(dict[0]));
                    return (Data = result);
                }
            }
            else if (typeof (T) == typeof (Boolean))
            {
                return Data = (dynamic) (bitmask.GetBits(this.bitIndex, this.bits) == 1 ? true : false);                
            }
            return result;
        }

        public bool Encode(ref ushort bitmask, BinaryWriter writer)
        {
            if (typeof (T) == typeof (Int32)
                || typeof (T) == typeof (Int16)
                || typeof (T) == typeof (Byte))
            {
                var _data = (Int32) (dynamic) Data;

                if (dict.Count == 8)
                {

                    switch (_data)
                    {
                        case -1:
                        case 0:
                        case 1:
                        case 2:
                            bitmask = bitmask.SetRange(bitIndex, bits, (ushort) dict.IndexOf((uint) _data));
                            return true;
                        default:
                            break;
                    }

                    var cryptOperationHashes = dict.Where(x => x > 7 && x != unchecked ((uint) -1)).ToList();
                    var cryptOperation = cryptOperationHashes[random.Next()%cryptOperationHashes.Count];
                    bitmask = bitmask.SetRange(bitIndex, bits, (ushort) dict.IndexOf(cryptOperation));
                    return Serializer.Encode(Data, writer, Operations.GetOperations(cryptOperation));
                }
                else if(dict.Count == 1)
                {
                    if (_data == 0)
                    {
                        bitmask = bitmask.SetRange(bitIndex, bits, 1);
                    }
                    else
                    {
                        bitmask = bitmask.SetRange(bitIndex, bits, 0);
                        return Serializer.Encode(Data, writer, Operations.GetOperations(dict[0]));
                    }
                }
            }
            else if (typeof (T) == typeof (Vector3))
            {
                var _data = (Vector3) (dynamic) Data;

                if (_data.X != 0.0f
                    || _data.Y != 0.0f
                    || _data.Z != 0.0f)
                {
                    bitmask = bitmask.SetRange(bitIndex, bits, 0);
                    return Serializer.Encode(Data, writer, Operations.GetOperations(dict[0]));
                }
                else
                {
                    bitmask = bitmask.SetRange(bitIndex, bits, 1);
                    return true;
                }
            }
            else if (typeof (T) == typeof (Boolean))
            {
                bitmask = bitmask.SetRange(bitIndex, bits, (ushort) ((bool)(dynamic) Data == true ? 1 : 0));
            }

            return false;
        }
    }
}