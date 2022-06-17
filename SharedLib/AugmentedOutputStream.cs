using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    public class AugmentedOutputStream
    {

        private StreamWriter _writer;
        private Stream _stream;

        public AugmentedOutputStream(Stream stream)
        {
            _stream = stream;
            _writer = new StreamWriter(stream);
        }

        public async Task<IOResult> WriteObjectsAsync(params object[] objs)
        {
            try
            {
                foreach (object obj in objs)
                {
                    string objJson = JsonConvert.SerializeObject(obj);
                    await _writer.WriteLineAsync(objJson);
                }

                await _writer.FlushAsync();
            }
            catch (IOException)
            {
                return IOResult.CreateFailure(IOErrorType.IOError);
            }

            return IOResult.CreateSuccess();
        }

        public async Task<IOResult> WriteFileAsync(FileStream inputFs, long fileSize, IProgress<double> progress)
        {
            byte[] buffer = new byte[1024];
            int amtRead = 1;
            long totalAmtRead = 0;

            try
            {
                while (amtRead != 0)
                {
                    amtRead = await inputFs.ReadAsync(buffer, 0, buffer.Length);
                    if (amtRead == 0)
                        break;

                    totalAmtRead += amtRead;

                    await _stream.WriteAsync(buffer, 0, amtRead);
                    await _stream.FlushAsync();
                    progress.Report(amtRead / (double)totalAmtRead);
                }
            }
            catch (IOException)
            {
                return IOResult.CreateFailure(IOErrorType.IOError);
            }
            
            progress.Report(1);

            return IOResult.CreateSuccess();
        }

    }
}
