using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Core
{
    public static class Decompressor
    {
        public static bool Decompress(byte[] input, out byte[] output)
        {
            output = null;

            try
            {
                Logger.Info("Decompressing...");

                using (var inputStream = new MemoryStream(input))
                using (var outputStream = new MemoryStream())
                {
                    byte[] header = new byte[8];
                    if (inputStream.Read(header, 0, 8) != 8)
                        throw new InvalidDataException();

                    using (var deflate = new DeflateStream(inputStream, CompressionMode.Decompress))
                    {
                        deflate.CopyTo(outputStream);
                    }

                    output = outputStream.ToArray();
                }

                if (output.Length > 2 && (output[0] != 'M' || output[1] != 'Z'))
                {
                    int idx = Array.IndexOf(output, (byte)'M');
                    if (idx > 0)
                        output = output.Skip(idx).ToArray();
                }

                Logger.Success("Decompressed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }
    }
}