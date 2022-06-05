using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class StreamWrapper : IDisposable
    {

        private StreamWriter _writer;
        private StreamReader _reader;
        private Stream _stream;

        public StreamWrapper(Stream stream)
        {
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);
            _stream = stream;
        }

        public async Task WriteObjectAsync(object obj)
        {
            string objJson = SerializeUtil.SerializeObject(obj);
            await _writer.WriteLineAsync(objJson);
            await _writer.FlushAsync();
        }

        public async Task WriteObjectsAsync(params object[] objs)
        {
            foreach (object obj in objs)
            {
                string objJson = SerializeUtil.SerializeObject(obj);
                await _writer.WriteLineAsync(objJson);
            }

            await _writer.FlushAsync();
        }

        public async Task<object?> ReadObjectAsync(Type deserializeType)
        {
            string? json = await _reader.ReadLineAsync();
            return SerializeUtil.DeserializeObject(deserializeType, json);
        }

        public async Task<T> ExpectObjectAsync<T>(string errorMesage, Type? deserializeType = null)
        {
            string? json = await _reader.ReadLineAsync();
            if (json == null)
                throw new Exception(errorMesage);

            object? obj = SerializeUtil.DeserializeObject(deserializeType ?? typeof(T), json);
            if (obj == null)
                throw new Exception(errorMesage);

            return (T)obj;
        }

        public async Task WriteFileAsync(Stream outputFs, long fileSize, IProgress<double> progress)
        {
            byte[] buffer = new byte[1024];
            int amtRead = 1;
            long totalAmtRead = 0;
            while (amtRead != 0)
            {
                amtRead = await outputFs.ReadAsync(buffer, 0, buffer.Length);
                if (amtRead == 0)
                    break;

                totalAmtRead += amtRead;

                await _stream.WriteAsync(buffer, 0, amtRead);
                await _stream.FlushAsync();
                progress.Report(amtRead / (double)totalAmtRead);
            }

            progress.Report(1);
        }

        public async Task ReadFileAsync(Stream inputFs, long fileSize, IProgress<double> progress)
        {
            byte[] buffer = new byte[1024];

            long totalAmtRead = 0;
            while (totalAmtRead < fileSize)
            {
                int amtRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                totalAmtRead = amtRead + totalAmtRead;

                await inputFs.WriteAsync(buffer, 0, amtRead);
                await inputFs.FlushAsync();

                progress.Report(totalAmtRead / (double)fileSize);
            }

            progress.Report(1);
        }

        // does not dispose the underlying stream
        public void Dispose()
        {
            _writer.Dispose();
            _reader.Dispose();
        }

    }
}
