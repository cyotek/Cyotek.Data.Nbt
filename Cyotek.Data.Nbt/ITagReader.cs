using System.IO;

namespace Cyotek.Data.Nbt
{
  public interface ITagReader
  {
    Stream InputStream { get; set; }

    NbtOptions Options { get; set; }

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
  }
}