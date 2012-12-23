using System.IO;

namespace Cyotek.Data.Nbt
{
  public interface ITagReader
  {
  #region Private Properties 

    Stream InputStream { get; set; }

    NbtOptions Options { get; set; }

  #endregion Private Properties 

  #region Private Methods 

    TagCompound Load(string fileName, NbtOptions options);

    ITag Read();

    ITag Read(NbtOptions options);

    byte ReadByte();

    byte[] ReadByteArray();

    TagCollection ReadCollection(TagList owner);

    TagDictionary ReadDictionary(TagCompound owner);

    double ReadDouble();

    float ReadFloat();

    int ReadInt();

    int[] ReadIntArray();

    long ReadLong();

    short ReadShort();

    string ReadString();

  #endregion Private Methods 
  }
}