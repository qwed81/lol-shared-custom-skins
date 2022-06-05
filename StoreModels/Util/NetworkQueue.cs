using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class NetworkQueue<T>
    {

        private ConcurrentQueue<T> _requests;
        private Func<T, Task> _onAvailable;
        private bool _running;

        public NetworkQueue(Func<T, Task> onAvailable)
        {
            _requests = new ConcurrentQueue<T>();
            _onAvailable = onAvailable;
            _running = false;
        }


        public void Enqueue(T value)
        {
            _requests.Enqueue(value);
            onDataRecieved();
        }

        private void onDataRecieved()
        {
            if (_running)
                return;

            _running = true;
            Task.Run(async () =>
            {
                while (_requests.IsEmpty == false)
                {
                    T data;
                    if (_requests.TryDequeue(out data) == false)
                        continue;

                    await _onAvailable(data);
                }

                _running = false;
            });
        }

    }
}
