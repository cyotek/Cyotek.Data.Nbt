using System;

namespace Cyotek.Data.Nbt
{
  internal static class BitHelper
  {
    #region Internal Class Members

    internal static void SwapBytes(byte[] buffer, int offset, int length)
    {
      if (length < 1)
      {
        return;
      }

      if (offset + length > buffer.Length)
      {
        throw new ArgumentException("offset + length is larger than buffer");
      }

      byte temp;

      if (length == 2)
      {
        temp = buffer[offset];
        buffer[offset] = buffer[offset + 1];
        buffer[offset + 1] = temp;
        return;
      }

      int i2;

      if (offset == 0)
      {
        for (int i1 = (length + 1) / 2 - 1; i1 >= 0; i1--)
        {
          i2 = length - i1 - 1;

          temp = buffer[i1];
          buffer[i1] = buffer[i2];
          buffer[i2] = temp;
        }
      }
      else
      {
        for (int i1 = (length + 1) / 2 - 1; i1 >= 0; i1--)
        {
          i2 = length - i1 - 1;

          temp = buffer[offset + i1];
          buffer[offset + i1] = buffer[offset + i2];
          buffer[offset + i2] = temp;
        }
      }
    }

    #endregion
  }
}
