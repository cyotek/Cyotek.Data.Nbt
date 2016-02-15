using System.IO;

namespace Cyotek.Data.Nbt
{
  public interface ITagWriter
  {
    #region Methods

    void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression);

    void WriteDocument(Stream stream, TagCompound tag);

    void WriteTag(ITag value);

    void WriteTag(ITag value, WriteOptions options);

    void WriteValue(byte value);

    void WriteValue(byte[] value);

    void WriteValue(double value);

    void WriteValue(short value);

    void WriteValue(int value);

    void WriteValue(int[] value);

    void WriteValue(long value);

    void WriteValue(float value);

    void WriteValue(string value);

    #endregion
  }
}
