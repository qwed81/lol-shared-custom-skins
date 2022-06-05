using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ClientStore.Models
{

    public delegate void CollectionChangedHandler<T>(SynchronizedCollection<T> collection);

    public class SynchronizedCollection<T> : IEnumerable<T>
    {

        private ConcurrentBag<T> _elements; // for concurrent enumeration
        private object _changeLock; // to lock change so an empty bag is never enumerated on

        public event CollectionChangedHandler<T>? CollectionChanged;

        public SynchronizedCollection()
        {
            _elements = new ConcurrentBag<T>();
            _changeLock = new object();
        }

        internal void ChangeCollection(List<T> newElements)
        {
            lock (_changeLock)
            {
                _elements.Clear();
                foreach (var element in newElements)
                {
                    _elements.Add(element);
                }
            }

            CollectionChanged?.Invoke(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_changeLock)
            {
                return _elements.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_changeLock)
            {
                return _elements.GetEnumerator();
            }
        }
    }
}
