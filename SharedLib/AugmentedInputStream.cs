using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace SharedLib
{
    public class AugmentedInputStream
    {

        private StreamReader _reader;
        private Stream _stream;

        public AugmentedInputStream(Stream stream)
        {
            _stream = stream;
            _reader = new StreamReader(stream);
        }
        public async Task<IOResult<T>> ReadObjectAsync<T>(Type? deserializeType = null)
        {
            string? json;
            try
            {
                json = await _reader.ReadLineAsync();
            }
            catch (IOException)
            {
                return IOResult.CreateFailure<T>(IOErrorType.IOError); 
            }
            
            if (json == null)
                return IOResult.CreateFailure<T>(IOErrorType.ImproperFormat);

            object? obj = JsonConvert.DeserializeObject(json, deserializeType ?? typeof(T));
            if (obj == null)
                return IOResult.CreateFailure<T>(IOErrorType.ImproperFormat);

            return IOResult.CreateSuccess((T)obj);
        }

        public async Task<IOResult> ReadFileAsync(FileStream outputFs, long fileSize, IProgress<double> progress)
        {
            byte[] buffer = new byte[1024];

            long totalAmtRead = 0;
            try
            {
                while (totalAmtRead < fileSize)
                {
                    int amtRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    totalAmtRead = amtRead + totalAmtRead;

                    await outputFs.WriteAsync(buffer, 0, amtRead);
                    await outputFs.FlushAsync();

                    progress.Report(totalAmtRead / (double)fileSize);
                }

                progress.Report(1);
            }
            catch(IOException)
            {
                return IOResult.CreateFailure(IOErrorType.IOError);
            }

            return IOResult.CreateSuccess();
        }

    }
}
