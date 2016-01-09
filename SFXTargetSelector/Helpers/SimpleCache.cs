#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 SimpleCache.cs is part of SFXTargetSelector.

 SFXTargetSelector is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXTargetSelector is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXTargetSelector. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System;
using System.Collections.Concurrent;

#endregion

namespace SFXTargetSelector
{
    internal sealed class SimpleCache<T> where T : struct
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache;

        internal SimpleCache()
        {
            _cache = new ConcurrentDictionary<string, CacheItem>();
        }

        internal int MaxAge { get; set; }

        internal T? this[string key]
        {
            get { return Get(key); }
        }

        internal T? Get(string key)
        {
            try
            {
                if (_cache.ContainsKey(key))
                {
                    CacheItem item;
                    _cache.TryGetValue(key, out item);
                    if (item != null)
                    {
                        if (item.LastUpdate + MaxAge <= Environment.TickCount)
                        {
                            return item.Value;
                        }
                        CacheItem oldItem;
                        _cache.TryRemove(key, out oldItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        internal T? GetOrAdd(string key, Func<T?> valueFunction)
        {
            var value = Get(key);
            if (value != null)
            {
                return (T) value;
            }

            value = GetValue(valueFunction);

            if (value != null)
            {
                Add(key, (T) value);
                return (T) value;
            }
            return null;
        }

        private T? GetValue(Func<T?> valueFunction)
        {
            try
            {
                return valueFunction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        internal bool Add(string key, T value)
        {
            try
            {
                if (!_cache.ContainsKey(key))
                {
                    _cache.TryAdd(key, new CacheItem { Value = value, LastUpdate = Environment.TickCount });
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        internal void AddOrUpdate(string key, T value)
        {
            try
            {
                if (!Add(key, value))
                {
                    CacheItem item;
                    _cache.TryGetValue(key, out item);
                    if (item != null)
                    {
                        item.Value = value;
                        item.LastUpdate = Environment.TickCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal void Update(string key, T value)
        {
            try
            {
                if (_cache.ContainsKey(key))
                {
                    CacheItem item;
                    _cache.TryGetValue(key, out item);
                    if (item != null)
                    {
                        item.Value = value;
                        item.LastUpdate = Environment.TickCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal sealed class CacheItem
        {
            public T Value { get; set; }
            public int LastUpdate { get; set; }
        }
    }
}