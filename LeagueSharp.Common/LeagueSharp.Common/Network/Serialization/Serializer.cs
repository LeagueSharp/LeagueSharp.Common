using System;
using System.IO;
using System.Linq;
using LeagueSharp.Network.Cryptography;
using SharpDX;

namespace LeagueSharp.Network.Serialization
{
    public static class Serializer
    {
        public static bool Encode(object data, Type type, BinaryWriter writer, Operations encryptOperations, bool reverseByteOrder = false)
        {
            if (type == typeof (Int32)
                || type == typeof(Int16))
            {
                int _data = (int) data;

                while (_data > 0x7F)
                {
                    writer.Write(encryptOperations.Encrypt((byte) (_data | 0x80)));
                    _data >>= 7;
                }
                writer.Write(encryptOperations.Encrypt((byte) _data));
                return true;
            }
            else if (type == typeof(Byte))
            {
                byte _data = (byte) data;
                writer.Write(encryptOperations.Encrypt(_data));
                return true;
            }
            else if (type == typeof(Single))
            {
                float _data = (float) data;
                var bytes = BitConverter.GetBytes(_data);
                
                foreach (var b in !reverseByteOrder ? bytes.Reverse() : bytes)
                {
                    writer.Write(encryptOperations.Encrypt(b));
                }
                return true;
            }
            else if (type == typeof(Vector3))
            {
                Vector3 _data = (Vector3) data;

                return Encode(_data.X, typeof(Single), writer, encryptOperations, reverseByteOrder)
                    && Encode(_data.Y, typeof(Single), writer, encryptOperations, reverseByteOrder)
                    && Encode(_data.Z, typeof(Single), writer, encryptOperations, reverseByteOrder);
            }
            else if (type == typeof(Vector2))
            {
                Vector2 _data = (Vector2) data;

                return Encode(_data.X, typeof(Single), writer, encryptOperations, reverseByteOrder)
                    && Encode(_data.Y, typeof(Single), writer, encryptOperations, reverseByteOrder);
            }

            return false;
        }

        public static bool Decode(out object result, Type type, BinaryReader reader, Operations encryptOperations, bool reverseByteOrder = false)
        {
            result = null;

            if (type == typeof (Int32)
                || type == typeof(Int16))
            {
                var bitcounter = 0;
                var data = 0;
                do
                {
                    var encryptedByte = encryptOperations.Decrypt(reader.ReadByte());
                    data |= (encryptedByte & 0x7F) << bitcounter;
                    if ((sbyte) encryptedByte >= 0)
                    {
                        result = (dynamic) data;
                        return true;
                    }
                    bitcounter += 7;
                } while (reader.BaseStream.Position < reader.BaseStream.Length);
            }
            else if (type == typeof(Byte))
            {
                result = (dynamic) encryptOperations.Decrypt(reader.ReadByte());
                return true;
            }
            else if (type == typeof(Single))
            {
                byte[] buffer = new byte[4];

                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = encryptOperations.Decrypt(reader.ReadByte());
                }

                result = BitConverter.ToSingle(reverseByteOrder ? buffer : buffer.Reverse().ToArray(), 0);

                return true;
            }
            else if (type == typeof(Vector3))
            {
                object x = null;
                object y = null;
                object z = null;

                bool success = Decode(out x, typeof(Single), reader, encryptOperations, reverseByteOrder)
                    && Decode(out y, typeof(Single), reader, encryptOperations, reverseByteOrder)
                    && Decode(out z, typeof(Single), reader, encryptOperations, reverseByteOrder);

                result = new Vector3((float)x, (float)y, (float)z);

                return success;
            }
            else if (type == typeof(Vector2))
            {
                object x = null;
                object y = null;

                bool success = Decode(out x, typeof(Single), reader, encryptOperations, reverseByteOrder)
                    && Decode(out y, typeof(Single), reader, encryptOperations, reverseByteOrder);
                
                result = (dynamic)new Vector2((float)x, (float)y);

                return success;
            }

            return false;
        }
    }
}