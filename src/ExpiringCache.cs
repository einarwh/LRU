using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpiringCacheLib
{
    /// <summary>
    /// Simple LRU cache with self-expiring elements.
    /// </summary>
    public class ExpirationCache<TKey, TValue> where TValue : class
    {

        private readonly Dictionary<TKey, LinkedListNode<TimeStamped<TKey, TValue>>> _dict = new Dictionary<TKey, LinkedListNode<TimeStamped<TKey, TValue>>>();
        private readonly LinkedList<TimeStamped<TKey, TValue>> _list = new LinkedList<TimeStamped<TKey, TValue>>();
        private readonly TimeSpan _lifetime;

        public ExpirationCache(int lifetimeInSeconds)
        {
            _lifetime = new TimeSpan(0, 0, lifetimeInSeconds);
        }

        public TValue this[TKey key]
        {
            get { return Get(key); }
            set { Add(key, value); }
        }

        public TValue Get(TKey key)
        {
            Prune();
            return _dict.ContainsKey(key) ? _dict[key].Value.Value : null;
        }

        public void Add(TKey key, TValue value)
        {
            Prune();
            if (ContainsKey(key))
            {
                Remove(key);
            }
            var node = CreateNode(key, value);
            _list.AddFirst(node);
            _dict[key] = node;
        }

        public int Count
        {
            get
            {
                Prune();
                return _dict.Count;
            }
        }

        private static LinkedListNode<TimeStamped<TKey, TValue>> CreateNode(TKey key, TValue value)
        {
            return new LinkedListNode<TimeStamped<TKey, TValue>>(new TimeStamped<TKey, TValue>(key, value));
        }

        private void Prune()
        {
            DateTime expirationTime = DateTime.Now - _lifetime;
            while (true)
            {
                var node = _list.Last;
                if (node == null || node.Value.TimeStamp > expirationTime)
                {
                    return;
                }
                _list.RemoveLast();
                _dict.Remove(node.Value.Key);
            }
        }

        private bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        private void Remove(TKey key)
        {
            _list.Remove(_dict[key]);
            _dict.Remove(key);
        }
    }
}
