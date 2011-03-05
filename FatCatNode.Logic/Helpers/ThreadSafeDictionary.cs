using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FatCatNode.Logic.Helpers
{
    public class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly object _lockObj = new object();

        public ThreadSafeDictionary()
        {
            Collection = new Dictionary<TKey, TValue>();
        }

        private Dictionary<TKey, TValue> Collection { get; set; }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_lockObj)
            {
                Collection.Add(item.Key, item.Value);
            }
        }

        public void Clear()
        {
            lock (_lockObj)
            {
                Collection.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Collection.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_lockObj)
            {
                return Collection.Remove(item.Key);
            }
        }

        public int Count
        {
            get { return Collection.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(TKey key)
        {
            return Collection.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            lock (_lockObj)
            {
                Collection.Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_lockObj)
            {
                return Collection.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Collection.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return Collection[key]; }
            set { Collection[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return Collection.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return Collection.Values; }
        }
    }
}