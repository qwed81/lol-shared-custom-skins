using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoreModels
{

    public delegate void ListChangedHandler<T>(SynchronizedList<T> collection);


    public class SynchronizedList<T> : IEnumerable<T>
    {

        private List<T> _elements; // for concurrent enumeration
        private object _changeLock; // for adding elements from multiple thread

        public event ListChangedHandler<T>? CollectionChanged;

        public SynchronizedList()
        {
            _elements = new List<T>();
            _changeLock = new object();
        }

        public void TakeOwnershipOfCollection(List<T> newElements)
        {
            lock(_changeLock)
            {
                _elements = newElements;
            }

            CollectionChanged?.Invoke(this);
        }

        public void AddElement(T element)
        {
            lock(_changeLock)
            {
                _elements.Add(element);
            }
        }

        public void RemoveElement(T element)
        {
            lock(_changeLock)
            {
                _elements.Remove(element);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_changeLock)
            {
                return new SyncCollectionEnumerator<T>(_elements);
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_changeLock)
            {
                return new SyncCollectionEnumerator<T>(_elements);
            }
        }

        private class SyncCollectionEnumerator<T2> : IEnumerator<T2>
        {
            private T2[] _elements;
            private int _index;

            public SyncCollectionEnumerator(IReadOnlyList<T2> list)
            {
                _elements = list.ToArray();
                _index = 0;
            }

            public T2 Current => _elements[_index];

            object? IEnumerator.Current => _elements[_index];

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (++_index == _elements.Length)
                    return false;

                return true;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
    

}
