using System.IO;

namespace Cyotek.Data.Nbt
{
  internal static class StreamExtensions
  {
    #region Static Methods

    public static bool IsDeflateCompressed(this Stream stream)
    {
      int buffer;
      long position;
      bool result;

      // http://www.gzip.org/zlib/rfc-deflate.html#spec

      position = stream.Position;
      buffer = stream.ReadByte();
      result = buffer != -1;

      if (result)
      {
        bool bit1Set;
        bool bit2Set;
        bool bit3Set;
        byte header;

        header = (byte)buffer;

        bit1Set = (header & (1 << 0)) != 0;
        bit2Set = (header & (1 << 1)) != 0;
        bit3Set = (header & (1 << 2)) != 0;

        result = bit1Set && (bit2Set || bit3Set) && !(bit2Set && bit3Set);
      }

      stream.Position = position;

      return result;
    }

    public static bool IsGzipCompressed(this Stream stream)
    {
      long position;
      bool result;

      // http://www.gzip.org/zlib/rfc-gzip.html#file-format

      position = stream.Position;

      int b0 = stream.ReadByte();
      int b1 = stream.ReadByte();
      int b2 = stream.ReadByte();
      int b3 = stream.ReadByte();

      result = b3 != -1;

      if (result)
      {
        result = b0 == 31 && b1 == 139 && b2 == 8;
      }

      stream.Position = position;

      return result;
    }

    #endregion
  }
}
