using System.IO;

namespace Cyotek.Data.Nbt.Serialization
{
  public interface ITagReader
  {
    #region Methods

    bool IsNbtDocument(Stream stream);

    byte ReadByte();

    byte[] ReadByteArray();

    TagCollection ReadCollection();

    TagDictionary ReadDictionary();

    TagCompound ReadDocument(Stream stream);

    TagCompound ReadDocument(Stream stream, ReadTagOptions options);

    double ReadDouble();

    float ReadFloat();

    int ReadInt();

    int[] ReadIntArray();

    long ReadLong();

    short ReadShort();

    string ReadString();

    ITag ReadTag();

    ITag ReadTag(ReadTagOptions options);

    #endregion
  }
}
