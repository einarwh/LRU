using System;

namespace ExpiringCacheLib
{
    class TimeStamped<TKey, TValue>
    {
        private readonly TKey _key;
        private readonly TValue _value;
        private readonly DateTime _timeStamp;

        public TimeStamped(TKey key, TValue value)
        {
            _key = key;
            _value = value;
            _timeStamp = DateTime.Now;
        }

        public TKey Key { get { return _key; } }

        public TValue Value { get { return _value; } }

        public DateTime TimeStamp { get { return _timeStamp; } }
    }
}
