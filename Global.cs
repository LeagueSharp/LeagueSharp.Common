#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Global.cs is part of LeagueSharp.Common.
 
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
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Diagnostics;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public static class Global
    {
        internal static MemoryMappedFile MMFile;
        internal static int MemoryCapacity = 1024 * 1024;
        internal static int OffsetEntrySize = Marshal.SizeOf(typeof(OffsetEntry));

        static Global()
        {
            using (new CustomMutex(100))
            {
                MMFile = MemoryMappedFile.CreateOrOpen("LSharpShared" + Process.GetCurrentProcess().Id, MemoryCapacity);
            }
        }
		
        public static bool Evade
        {
            get { return Read<bool>("Evade"); }
            set { Write("Evade", value); }
        }

        public static bool IsEvading
        {
            get { return Read<bool>("IsEvading"); }
            set { Write("IsEvading", value); }
        }

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
            return (T) binForm.Deserialize(memStream);
        }

        public static T Read<T>(string key, bool defaultIfMissing = false)
        {
            try
            {
                using (new CustomMutex(350))
                {
                    using (var strm = MMFile.CreateViewAccessor())
                    {
                        var hash = CalculateHash(key);

                        var signature = strm.ReadInt32(0);
                        const int startingOffset = 2 * sizeof (int);
                        int currentOffset;
                        if (signature == 0x34CFABC0)
                        {
                            currentOffset = strm.ReadInt32(sizeof (int));
                        }
                        else
                        {
                            strm.Write(0, 0x34CFABC0);
                            strm.Write(sizeof (int), startingOffset);
                            currentOffset = startingOffset;
                        }
                        var thisOffset = startingOffset;
                        while (thisOffset != currentOffset)
                        {
                            var buff = new byte[OffsetEntrySize];
                            strm.ReadArray(thisOffset, buff, 0, OffsetEntrySize);
                            var entry = FromByteArray<OffsetEntry>(buff);
                            if (entry.Type != EntryType.Invalid && entry.KeyHash == hash)
                            {
                                if (typeof(T).IsValueType)
                                {
                                    var buff2 = new byte[Marshal.SizeOf(typeof(T))];
                                    strm.ReadArray(thisOffset + OffsetEntrySize, buff2, 0, Marshal.SizeOf(typeof(T)));
                                    return FromByteArray<T>(buff2);
                                }
                                else
                                {
                                    byte[] buff2;
                                    if (typeof(T) == typeof(string))
                                    {
                                        buff2 = new byte[entry.Capacity];
                                        strm.ReadArray(thisOffset + OffsetEntrySize, buff2, 0, entry.Capacity);
                                        var data = System.Text.Encoding.UTF8.GetString(buff2, 0, entry.Capacity);
                                        var end = data.IndexOf('\0');
                                        var result = data.Substring(0, end);
                                        return (T) (object) result;
                                    }
                                    if (typeof(T).IsSerializable)
                                    {
                                        var size = strm.ReadInt32(thisOffset + OffsetEntrySize);
                                        buff2 = new byte[size];
                                        strm.ReadArray(thisOffset + OffsetEntrySize + sizeof (int), buff2, 0, size);


                                        // it is a class, must serialize.
                                        return Deserialize<T>(buff2);
                                    }
                                    throw new Exception(
                                        String.Format("Type {0} is not serializable!  Cannot read.", typeof(T)));
                                }
                            }
                            thisOffset += OffsetEntrySize + entry.Capacity;
                        }
                        if (!defaultIfMissing)
                        {
                            throw new Exception(String.Format("Config key '{0}' not found!", key));
                        }
                        return default(T);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        public static int Load(string path)
        {
            var LoadedEntries = new Dictionary<ulong, byte[]>();
            var count = 0;
            using (var fl = new System.IO.BinaryReader(System.IO.File.Open(path, System.IO.FileMode.Open)))
            {
                long pos = 0;
                var len = fl.BaseStream.Length;
                while (pos != len)
                {
                    var thisEntry = FromByteArray<OffsetEntry>(fl.ReadBytes(OffsetEntrySize));
                    LoadedEntries[thisEntry.KeyHash] = fl.ReadBytes(thisEntry.Capacity);
                    pos += OffsetEntrySize + thisEntry.Capacity;
                    count++;
                }
            }
            using (new CustomMutex(650))
            {
                using (var strm = MMFile.CreateViewAccessor())
                {
                    var signature = strm.ReadInt32(0);
                    const int startingOffset = 2 * sizeof (int);
                    int currentOffset;
                    if (signature == 0x34CFABC0)
                    {
                        currentOffset = strm.ReadInt32(sizeof (int));
                    }
                    else
                    {
                        strm.Write(0, 0x34CFABC0);
                        strm.Write(sizeof (int), startingOffset);
                        currentOffset = startingOffset;
                    }
                    var thisOffset = startingOffset;
                    while (thisOffset != currentOffset)
                    {
                        var buff = new byte[OffsetEntrySize];
                        strm.ReadArray(thisOffset, buff, 0, OffsetEntrySize);
                        var entry = FromByteArray<OffsetEntry>(buff);
                        if (entry.Type != EntryType.Invalid && LoadedEntries.ContainsKey(entry.KeyHash))
                        {
                            if (LoadedEntries[entry.KeyHash].Length <= entry.Capacity)
                            {
                                strm.WriteArray(
                                    thisOffset + OffsetEntrySize, LoadedEntries[entry.KeyHash], 0,
                                    LoadedEntries[entry.KeyHash].Length <= entry.Capacity
                                        ? LoadedEntries[entry.KeyHash].Length
                                        : entry.Capacity);
                                LoadedEntries.Remove(entry.KeyHash);
                            }
                            else
                            {
                                var overwriteEntry = entry;
                                entry.Type = EntryType.Invalid;
                                strm.WriteArray(
                                    thisOffset, ToByteArray(overwriteEntry, OffsetEntrySize), 0, OffsetEntrySize);
                            }
                        }
                        thisOffset += OffsetEntrySize + entry.Capacity;
                    }

                    foreach (var needtoload in LoadedEntries)
                    {
                        OffsetEntry newEntry;
                        newEntry.KeyHash = needtoload.Key;
                        newEntry.Capacity = needtoload.Value.Length;
                        newEntry.Type = EntryType.Basic;
                        strm.WriteArray(currentOffset, ToByteArray(newEntry, OffsetEntrySize), 0, OffsetEntrySize);
                        strm.WriteArray(currentOffset + OffsetEntrySize, needtoload.Value, 0, newEntry.Capacity);
                        strm.Write(sizeof (int), currentOffset + OffsetEntrySize + newEntry.Capacity);
                    }
                }
            }
            return count;
        }

        public static int Save(string path, string[] keys)
        {
            using (new CustomMutex(1200))
            {
                using (var strm = MMFile.CreateViewAccessor())
                {
                    using (var fl = new System.IO.BinaryWriter(System.IO.File.Create(path)))
                    {
                        var signature = strm.ReadInt32(0);
                        const int startingOffset = 2 * sizeof (int);
                        int currentOffset;
                        if (signature == 0x34CFABC0)
                        {
                            currentOffset = strm.ReadInt32(sizeof (int));
                        }
                        else
                        {
                            strm.Write(0, 0x34CFABC0);
                            strm.Write(sizeof (int), startingOffset);
                            currentOffset = startingOffset;
                        }
                        var thisOffset = startingOffset;
                        var count = 0;
                        while (thisOffset != currentOffset)
                        {
                            var buff = new byte[OffsetEntrySize];
                            strm.ReadArray(thisOffset, buff, 0, OffsetEntrySize);
                            var entry = FromByteArray<OffsetEntry>(buff);
                            foreach (
                                var hash in
                                    keys.Select(CalculateHash)
                                        .Where(hash => entry.Type != EntryType.Invalid && entry.KeyHash == hash))
                            {
                                fl.Write(buff);
                                var buff2 = new byte[entry.Capacity];
                                strm.ReadArray(thisOffset + OffsetEntrySize, buff2, 0, entry.Capacity);
                                fl.Write(buff2);
                                count++;
                            }
                            thisOffset += OffsetEntrySize + entry.Capacity;
                        }
                        return count;
                    }
                }
            }
        }


        public static void Write<T>(string key, T val)
        {
            try
            {

                using (new CustomMutex(700))
                {
                    using (var strm = MMFile.CreateViewAccessor())
                    {
                        var hash = CalculateHash(key);
                        int requiredCapacity;
                        byte[] serialized = null;
                        if (typeof(T).IsValueType)
                        {
                            requiredCapacity = Marshal.SizeOf(typeof(T));
                        }
                        else if (typeof(T) == typeof(string))
                        {
                            requiredCapacity = val.ToString().Length + 1;
                        }
                        else if (typeof(T).IsSerializable)
                        {
                            // also store the sizeof the serialized object as what's ref'd by ptr
                            serialized = Serialize(val);
                            requiredCapacity = serialized.Length + sizeof (int);
                        }
                        else
                        {
                            throw new Exception(
                                String.Format("Type {0} is not serializable!  Cannot write.", typeof(T)));
                        }

                        var signature = strm.ReadInt32(0);
                        const int startingOffset = 2 * sizeof (int);
                        int currentOffset;
                        if (signature == 0x34CFABC0)
                        {
                            currentOffset = strm.ReadInt32(sizeof (int));
                        }
                        else
                        {
                            strm.Write(0, 0x34CFABC0);
                            strm.Write(sizeof (int), startingOffset);
                            currentOffset = startingOffset;
                        }
                        var thisOffset = startingOffset;
                        while (thisOffset != currentOffset)
                        {
                            var buff = new byte[OffsetEntrySize];
                            strm.ReadArray(thisOffset, buff, 0, OffsetEntrySize);
                            var entry = FromByteArray<OffsetEntry>(buff);
                            if (entry.Type != EntryType.Invalid && entry.KeyHash == hash)
                            {
                                if (requiredCapacity <= entry.Capacity)
                                {
                                    if (typeof(T).IsValueType)
                                    {
                                        var tobewritten = ToByteArray(val, entry.Capacity);
                                        strm.WriteArray(
                                            thisOffset + OffsetEntrySize, tobewritten, 0,
                                            buff.Length <= entry.Capacity ? buff.Length : entry.Capacity);
                                    }
                                    else if (typeof(T) != typeof(string))
                                    {
                                        strm.WriteArray(
                                            thisOffset + OffsetEntrySize, ToByteArray(serialized.Length, sizeof (int)),
                                            0, sizeof (int));
                                        strm.WriteArray(
                                            thisOffset + OffsetEntrySize + sizeof (int), serialized, 0,
                                            serialized.Length);
                                    }
                                    else
                                    {
                                        var strz = System.Text.Encoding.UTF8.GetBytes(val + "\0");
                                        strm.WriteArray(thisOffset + OffsetEntrySize, strz, 0, strz.Length);
                                    }
                                    return;
                                }
                                var overwriteEntry = entry;
                                entry.Type = EntryType.Invalid;
                                strm.WriteArray(
                                    thisOffset, ToByteArray(overwriteEntry, OffsetEntrySize), 0, OffsetEntrySize);
                            }
                            thisOffset += OffsetEntrySize + entry.Capacity;
                        }
                        OffsetEntry newEntry;
                        newEntry.KeyHash = hash;
                        newEntry.Capacity = (typeof(T).IsValueType ? 1 : 2) * requiredCapacity;
                        newEntry.Type = EntryType.Basic;
                        strm.WriteArray(currentOffset, ToByteArray(newEntry, OffsetEntrySize), 0, OffsetEntrySize);

                        if (typeof(T).IsValueType)
                        {
                            var buffr = ToByteArray(val, newEntry.Capacity);
                            strm.WriteArray(
                                currentOffset + OffsetEntrySize, buffr, 0,
                                buffr.Length <= newEntry.Capacity ? buffr.Length : newEntry.Capacity);
                        }
                        else if (typeof(T) != typeof(string))
                        {
                            strm.WriteArray(
                                thisOffset + OffsetEntrySize, ToByteArray(serialized.Length, sizeof (int)), 0,
                                sizeof (int));
                            strm.WriteArray(
                                thisOffset + OffsetEntrySize + sizeof (int), serialized, 0, serialized.Length);
                        }
                        else
                        {
                            var arr = System.Text.Encoding.UTF8.GetBytes(val + "\0");
                            strm.WriteArray(currentOffset + OffsetEntrySize, arr, 0, arr.Length);
                        }
                        // write new currentoffset
                        strm.Write(sizeof (int), currentOffset + OffsetEntrySize + newEntry.Capacity);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static T FromByteArray<T>(byte[] rawValue)
        {
            var handle = GCHandle.Alloc(rawValue, GCHandleType.Pinned);
            var structure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        public static byte[] ToByteArray(object value, int maxLength)
        {
            var rawsize = Marshal.SizeOf(value);
            var rawdata = new byte[rawsize];
            var handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
            handle.Free();
            if (maxLength < rawdata.Length)
            {
                var temp = new byte[maxLength];
                Array.Copy(rawdata, temp, maxLength);
                return temp;
            }
            return rawdata;
        }


        internal static ulong CalculateHash(string s)
        {
            return s.Aggregate<char, ulong>(5381, (current, c) => ((current << 5) + current) + c);
        }

        internal enum EntryType
        {
            Invalid,
            Basic,
            Allocated
        }


        internal struct OffsetEntry
        {
            internal int Capacity;
            internal ulong KeyHash;
            internal EntryType Type;
        }
    }

    internal class CustomMutex : IDisposable
    {
        private readonly bool hasHandle;
        private Mutex mutex;

        internal CustomMutex(int timeOut = -1)
        {
            InitMutex();
            try
            {
                // note, you may want to time out here instead of waiting forever
                // edited by acidzombie24
                // mutex.WaitOne(Timeout.Infinite, false);
                hasHandle = mutex.WaitOne(timeOut > 0 ? timeOut : Timeout.Infinite, false);
                if (hasHandle == false)
                {
                    throw new TimeoutException("Timeout waiting for exclusive access");
                }
            }
            catch (AbandonedMutexException)
            {
                // Log the fact the mutex was abandoned in another process, it will still get aquired
                hasHandle = true;
            }
        }


        public void Dispose()
        {
            if (mutex != null)
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
                mutex.Dispose();
            }
        }

        internal void InitMutex()
        {
            var appGuid =
                ((GuidAttribute)
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;
            var mutexId = string.Format("Global\\{{{0}}}", appGuid);
            mutex = new Mutex(false, mutexId);

            var allowEveryoneRule = new MutexAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl,
                AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
        }
    }
}