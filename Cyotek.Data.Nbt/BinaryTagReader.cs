using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class BinaryTagReader : TagReader
  {
    #region Public Constructors

    public BinaryTagReader()
    { }

    public BinaryTagReader(Stream input, NbtOptions options)
      : base(input, options)
    { }

    #endregion

    #region Overridden Methods

    public override TagCompound Load(string fileName, NbtOptions options)
    {
      TagCompound tag;
      BinaryTagReader reader;

      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException("Cannot find source file.", fileName);
      }

      //Check if gzipped stream
      try
      {
        using (FileStream input = File.OpenRead(fileName))
        {
          using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
          {
            reader = new BinaryTagReader(gzipStream, options);
            tag = (TagCompound)reader.Read();
          }
        }
      }
      catch (Exception)
      {
        tag = null;
      }

      if (tag != null)
      {
        return tag;
      }

      //Try Deflate stream
      try
      {
        using (FileStream input = File.OpenRead(fileName))
        {
          using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
          {
            reader = new BinaryTagReader(deflateStream, options);
            tag = (TagCompound)reader.Read();
          }
        }
      }
      catch (Exception)
      {
        tag = null;
      }

      if (tag != null)
      {
        return tag;
      }

      //Assume uncompressed stream
      using (FileStream input = File.OpenRead(fileName))
      {
        reader = new BinaryTagReader(input, options);
        tag = (TagCompound)reader.Read();
      }

      return tag;
    }

    public override ITag Read(NbtOptions options)
    {
      int rawType;
      ITag result;
      object value;

      rawType = this.InputStream.ReadByte();
      result = TagFactory.CreateTag((TagType)rawType);

      if (result.Type != TagType.End && (options & NbtOptions.ReadHeader) != 0)
      {
        result.Name = this.ReadString();
      }

      if ((options & NbtOptions.HeaderOnly) == 0)
      {
        switch (result.Type)
        {
          case TagType.End:
            value = null;
            break;

          case TagType.Byte:
            value = this.ReadByte();
            break;

          case TagType.Short:
            value = this.ReadShort();
            break;

          case TagType.Int:
            value = this.ReadInt();
            break;

          case TagType.IntArray:
            value = this.ReadIntArray();
            break;

          case TagType.Long:
            value = this.ReadLong();
            break;

          case TagType.Float:
            value = this.ReadFloat();
            break;

          case TagType.Double:
            value = this.ReadDouble();
            break;

          case TagType.ByteArray:
            value = this.ReadByteArray();
            break;

          case TagType.String:
            value = this.ReadString();
            break;

          case TagType.List:
            value = this.ReadCollection((TagList)result);
            break;

          case TagType.Compound:
            value = this.ReadDictionary((TagCompound)result);
            break;

          default:
            throw new InvalidDataException(string.Format("Unrecognized tag type: {0}", rawType));
        }
      }
      else
      {
        value = null;
      }

      result.Value = value;

      return result;
    }

    public override byte ReadByte()
    {
      int data;

      data = this.InputStream.ReadByte();
      if (data != (data & 0xFF))
      {
        throw new InvalidDataException();
      }

      return (byte)data;
    }

    public override byte[] ReadByteArray()
    {
      int length;
      byte[] data;

      length = this.ReadInt();

      data = new byte[length];
      if (length != this.InputStream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data;
    }

    public override TagCollection ReadCollection(TagList owner)
    {
      TagCollection tags;
      int length;

      owner.ListType = (TagType)this.ReadByte();
      tags = new TagCollection(owner, owner.ListType);
      length = this.ReadInt();

      for (int i = 0; i < length; i++)
      {
        ITag tag;

        switch (owner.ListType)
        {
          case TagType.Byte:
            tag = TagFactory.CreateTag(TagType.Byte, this.ReadByte());
            break;

          case TagType.ByteArray:
            tag = TagFactory.CreateTag(TagType.ByteArray, this.ReadByteArray());
            break;

          case TagType.Compound:
            tag = TagFactory.CreateTag(TagType.Compound);
            tag.Value = this.ReadDictionary((TagCompound)tag);
            break;

          case TagType.Double:
            tag = TagFactory.CreateTag(TagType.Double, this.ReadDouble());
            break;

          case TagType.End:
            tag = new TagEnd();
            break;

          case TagType.Float:
            tag = TagFactory.CreateTag(TagType.Float, this.ReadFloat());
            break;

          case TagType.Int:
            tag = TagFactory.CreateTag(TagType.Int, this.ReadInt());
            break;

          case TagType.IntArray:
            tag = TagFactory.CreateTag(TagType.IntArray, this.ReadIntArray());
            break;

          case TagType.List:
            tag = TagFactory.CreateTag(TagType.List);
            tag.Value = this.ReadCollection((TagList)tag);
            break;

          case TagType.Long:
            tag = TagFactory.CreateTag(TagType.Long, this.ReadLong());
            break;

          case TagType.Short:
            tag = TagFactory.CreateTag(TagType.Short, this.ReadShort());
            break;

          case TagType.String:
            tag = TagFactory.CreateTag(TagType.String, this.ReadString());
            break;

          default:
            throw new InvalidDataException("Invalid list type.");
        }

        tags.Add(tag);
      }

      return tags;
    }

    public override TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary results;
      ITag tag;

      results = new TagDictionary(owner);

      tag = this.Read();
      while (tag.Type != TagType.End)
      {
        results.Add(tag);
        tag = this.Read();
      }

      return results;
    }

    public override double ReadDouble()
    {
      byte[] data;

      data = new byte[BinaryTagWriter.DoubleSize];
      if (BinaryTagWriter.DoubleSize != this.InputStream.Read(data, 0, BinaryTagWriter.DoubleSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.DoubleSize);
      }

      return BitConverter.ToDouble(data, 0);
    }

    public override float ReadFloat()
    {
      byte[] data;

      data = new byte[BinaryTagWriter.FloatSize];
      if (BinaryTagWriter.FloatSize != this.InputStream.Read(data, 0, BinaryTagWriter.FloatSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.FloatSize);
      }

      return BitConverter.ToSingle(data, 0);
    }

    public override int ReadInt()
    {
      byte[] data;

      data = new byte[BinaryTagWriter.IntSize];
      if (BinaryTagWriter.IntSize != this.InputStream.Read(data, 0, BinaryTagWriter.IntSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.IntSize);
      }

      return BitConverter.ToInt32(data, 0);
    }

    public override int[] ReadIntArray()
    {
      int length;
      int bufferLength;
      byte[] buffer;
      int[] ints;

      length = this.ReadInt();
      bufferLength = length * BinaryTagWriter.IntSize;

      buffer = new byte[bufferLength];
      if (bufferLength != this.InputStream.Read(buffer, 0, bufferLength))
      {
        throw new InvalidDataException();
      }

      ints = new int[length];
      for (int i = 0; i < length; i++)
      {
        if (BitConverter.IsLittleEndian)
        {
          BitHelper.SwapBytes(buffer, i * 4, 4);
        }

        ints[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      return ints;
    }

    public override long ReadLong()
    {
      byte[] data;

      data = new byte[BinaryTagWriter.LongSize];
      if (BinaryTagWriter.LongSize != this.InputStream.Read(data, 0, BinaryTagWriter.LongSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.LongSize);
      }

      return BitConverter.ToInt64(data, 0);
    }

    public override short ReadShort()
    {
      byte[] data;

      data = new byte[BinaryTagWriter.ShortSize];
      if (BinaryTagWriter.ShortSize != this.InputStream.Read(data, 0, BinaryTagWriter.ShortSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.ShortSize);
      }

      return BitConverter.ToInt16(data, 0);
    }

    public override string ReadString()
    {
      short length;
      byte[] data;

      length = this.ReadShort();
      data = new byte[length];

      if (length != this.InputStream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data.Length != 0 ? Encoding.UTF8.GetString(data) : null;
    }

    #endregion
  }
}
