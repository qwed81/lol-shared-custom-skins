using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedLib
{
    public class LockedList<T>
    {

        private List<T> _list;

        public LockedList()
        {
            _list = new List<T>();
        }

        public T this[int index] 
        { 
            get
            { 
                lock (_list)
                { 
                    return _list[index];
                } 
            }
        }

        public void Add(T value)
        {
            lock(_list)
            {
                _list.Add(value);
            }
        }

        public void Insert(int index, T value)
        {
            lock(_list)
            {
                _list.Insert(index, value);
            }
        }

        public void RemoveAt(int index)
        {
            lock(_list)
            {
                _list.RemoveAt(index);
            }
        }

        public void ForEach(Action<T, int> action)
        {
            lock(_list)
            {
                for(int i = 0; i < _list.Count; i++)
                {
                    action(_list[i], i);
                }
            }
        }

        public List<T> Copy()
        {
            lock(_list)
            {
                return _list.ToList();
            }
        }

    }
}
