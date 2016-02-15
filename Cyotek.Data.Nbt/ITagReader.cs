using System.IO;

namespace Cyotek.Data.Nbt
{
  public interface ITagReader
  {
    #region Methods

    byte ReadByte();

    byte[] ReadByteArray();

    TagCollection ReadCollection(TagList owner);

    TagDictionary ReadDictionary(TagCompound owner);

    TagCompound ReadDocument(Stream stream);
    TagCompound ReadDocument(Stream stream,ReadTagOptions options);

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
