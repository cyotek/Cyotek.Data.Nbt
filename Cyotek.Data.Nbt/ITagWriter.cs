using System.IO;

namespace Cyotek.Data.Nbt
{
  internal interface ITagWriter
  {
    #region Properties

    NbtOptions Options { get; set; }

    Stream OutputStream { get; set; }

    #endregion

    #region Methods

    void Close();

    void Open();

    void Write(TagCompound tag, string fileName);

    void Write(TagCompound tag, string fileName, NbtOptions options);

    void Write(ITag value);

    void Write(ITag value, NbtOptions options);

    void Write(byte value);

    void Write(byte[] value);

    void Write(double value);

    void Write(short value);

    void Write(int value);

    void Write(int[] value);

    void Write(long value);

    void Write(float value);

    void Write(string value);

    #endregion
  }
}
