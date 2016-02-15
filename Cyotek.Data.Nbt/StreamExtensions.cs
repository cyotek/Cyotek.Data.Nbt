using System.IO;

namespace Cyotek.Data.Nbt
{
  internal static class StreamExtensions
  {
    #region Static Methods

    public static bool IsGzipCompressed(this Stream stream)
    {
      int bytesRead;
      long position;
      byte[] buffer;
      bool result;

      // http://www.gzip.org/zlib/rfc-gzip.html#file-format

      position = stream.Position;

      buffer = new byte[4];

      bytesRead = stream.Read(buffer, 0, 4);
      result = bytesRead == 4;

      if (result)
      {
        result = buffer[0] == 31 && buffer[1] == 139 && buffer[2] == 8;
      }

      stream.Position = position;

      return result;
    }

    #endregion
  }
}
