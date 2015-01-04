using System;
using System.IO;
using System.Linq;
using LeagueSharp.Network.Cryptography;
using SharpDX;

namespace LeagueSharp.Network.Serialization
{
    public static class Serializer
    {
        public static bool Encode<T>(T data, BinaryWriter writer, Operations encryptOperations)
        {
            if (typeof (T) == typeof (Int32)
                || typeof (T) == typeof (Int16))
            {
                int _data = (dynamic) data;

                while (_data > 0x7F)
                {
                    writer.Write(encryptOperations.Encrypt((byte) (_data | 0x80)));
                    _data >>= 7;
                }
                writer.Write(encryptOperations.Encrypt((byte) _data));
                return true;
            }
            else if (typeof (T) == typeof (Byte))
            {
                byte _data = (dynamic) data;
                writer.Write(encryptOperations.Encrypt(_data));
                return true;
            }
            else if (typeof (T) == typeof (Single))
            {
                float _data = (dynamic) data;
                foreach (var b in BitConverter.GetBytes(_data).Reverse())
                {
                    writer.Write(encryptOperations.Encrypt(b));
                }
                return true;
            }
            else if (typeof (T) == typeof (Vector3))
            {
                Vector3 _data = (dynamic) data;

                return Encode<Single>(_data.X, writer, encryptOperations) 
                    && Encode<Single>(_data.Y, writer, encryptOperations) 
                    && Encode<Single>(_data.Z, writer, encryptOperations);
            }

            return false;
        }

        public static bool Decode<T>(out T result, BinaryReader reader, Operations encryptOperations)
        {
            result = default(T);

            if (typeof (T) == typeof (Int32)
                || typeof (T) == typeof (Int16))
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
            else if (typeof (T) == typeof (Byte))
            {
                result = (dynamic) encryptOperations.Decrypt(reader.ReadByte());
                return true;
            }
            else if (typeof (T) == typeof (Single))
            {
                byte[] buffer = new byte[4];

                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = encryptOperations.Decrypt(reader.ReadByte());
                }

                result = (dynamic) BitConverter.ToSingle(buffer.Reverse().ToArray(), 0);

                return true;
            }
            else if (typeof (T) == typeof (Vector3))
            {
                Vector3 _result = new Vector3(0f, 0f, 0f);

                bool success = Decode<Single>(out _result.X, reader, encryptOperations) 
                    && Decode<Single>(out _result.Y, reader, encryptOperations) 
                    && Decode<Single>(out _result.Z, reader, encryptOperations);

                result = (dynamic)_result;

                return success;
            }

            return false;
        }
    }
}