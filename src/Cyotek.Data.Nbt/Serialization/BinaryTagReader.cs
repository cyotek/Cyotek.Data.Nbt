using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagReader : TagReader
  {
    #region Fields

    private Stream _stream;

    #endregion

    #region Constructors

    public BinaryTagReader()
    { }

    public BinaryTagReader(Stream stream)
    {
      _stream = stream;
    }

    #endregion

    #region Methods

    public override bool IsNbtDocument()
    {
      bool result;
      long position;

      position = _stream.Position;

      try
      {
        if (_stream.IsGzipCompressed())
        {
          using (Stream decompressionStream = new GZipStream(_stream, CompressionMode.Decompress, true))
          {
            result = decompressionStream.ReadByte() == (int)TagType.Compound;
          }
        }
        else if (_stream.IsDeflateCompressed())
        {
          using (Stream decompressionStream = new DeflateStream(_stream, CompressionMode.Decompress, true))
          {
            result = decompressionStream.ReadByte() == (int)TagType.Compound;
          }
        }
        else if (_stream.ReadByte() == (int)TagType.Compound)
        {
          result = true;
        }
        else
        {
          result = false;
        }
      }
      catch
      {
        result = false;
      }

      _stream.Position = position;

      return result;
    }

    public override byte ReadByte()
    {
      int data;

      data = _stream.ReadByte();
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
      if (length != _stream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data;
    }

    public override TagDictionary ReadCompound()
    {
      TagDictionary results;
      Tag tag;

      results = new TagDictionary();

      tag = this.ReadTag();
      while (tag.Type != TagType.End)
      {
        results.Add(tag);
        tag = this.ReadTag();
      }

      return results;
    }

    public override TagCompound ReadDocument()
    {
      return this.ReadDocument(ReadTagOptions.None);
    }

    public override TagCompound ReadDocument(ReadTagOptions options)
    {
      TagCompound tag;

      if (_stream.IsGzipCompressed())
      {
        using (Stream decompressionStream = new GZipStream(_stream, CompressionMode.Decompress))
        {
          _stream = decompressionStream;
          tag = (TagCompound)this.ReadTag(options);
        }
      }
      else if (_stream.IsDeflateCompressed())
      {
        using (Stream decompressionStream = new DeflateStream(_stream, CompressionMode.Decompress))
        {
          _stream = decompressionStream;
          tag = (TagCompound)this.ReadTag(options);
        }
      }
      else if (_stream.PeekNextByte() == (int)TagType.Compound)
      {
        tag = (TagCompound)this.ReadTag(options);
      }
      else
      {
        throw new InvalidDataException("Source stream does not contain a NBT document.");
      }

      return tag;
    }

    public override double ReadDouble()
    {
      byte[] data;

      data = new byte[BitHelper.DoubleSize];
      if (BitHelper.DoubleSize != _stream.Read(data, 0, BitHelper.DoubleSize))
      {
        throw new InvalidDataException();
      }

      if (TagWriter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.DoubleSize);
      }

      return BitConverter.ToDouble(data, 0);
    }

    public override float ReadFloat()
    {
      byte[] data;

      data = new byte[BitHelper.FloatSize];
      if (BitHelper.FloatSize != _stream.Read(data, 0, BitHelper.FloatSize))
      {
        throw new InvalidDataException();
      }

      if (TagWriter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.FloatSize);
      }

      return BitConverter.ToSingle(data, 0);
    }

    public override int ReadInt()
    {
      byte[] data;

      data = new byte[BitHelper.IntSize];
      if (BitHelper.IntSize != _stream.Read(data, 0, BitHelper.IntSize))
      {
        throw new InvalidDataException();
      }

      if (TagWriter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.IntSize);
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
      bufferLength = length * BitHelper.IntSize;

      buffer = new byte[bufferLength];
      if (bufferLength != _stream.Read(buffer, 0, bufferLength))
      {
        throw new InvalidDataException();
      }

      ints = new int[length];
      for (int i = 0; i < length; i++)
      {
        if (TagWriter.IsLittleEndian)
        {
          BitHelper.SwapBytes(buffer, i * 4, 4);
        }

        ints[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      return ints;
    }

    public override TagCollection ReadList()
    {
      TagCollection tags;
      int length;
      TagType listType;

      listType = (TagType)this.ReadByte();
      tags = new TagCollection(listType);
      length = this.ReadInt();

      for (int i = 0; i < length; i++)
      {
        Tag tag;

        switch (listType)
        {
          case TagType.Byte:
            tag = TagFactory.CreateTag(this.ReadByte());
            break;

          case TagType.ByteArray:
            tag = TagFactory.CreateTag(this.ReadByteArray());
            break;

          case TagType.Compound:
            tag = TagFactory.CreateTag(this.ReadCompound());
            break;

          case TagType.Double:
            tag = TagFactory.CreateTag(this.ReadDouble());
            break;

          case TagType.End:
            tag = TagFactory.CreateTag(TagType.End);
            break;

          case TagType.Float:
            tag = TagFactory.CreateTag(this.ReadFloat());
            break;

          case TagType.Int:
            tag = TagFactory.CreateTag(this.ReadInt());
            break;

          case TagType.IntArray:
            tag = TagFactory.CreateTag(this.ReadIntArray());
            break;

          case TagType.List:
            tag = TagFactory.CreateTag(this.ReadList());
            break;

          case TagType.Long:
            tag = TagFactory.CreateTag(this.ReadLong());
            break;

          case TagType.Short:
            tag = TagFactory.CreateTag(this.ReadShort());
            break;

          case TagType.String:
            tag = TagFactory.CreateTag(this.ReadString());
            break;

          default:
            throw new InvalidDataException("Invalid list type.");
        }

        tags.Add(tag);
      }

      return tags;
    }

    public override long ReadLong()
    {
      byte[] data;

      data = new byte[BitHelper.LongSize];
      if (BitHelper.LongSize != _stream.Read(data, 0, BitHelper.LongSize))
      {
        throw new InvalidDataException();
      }

      if (TagWriter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.LongSize);
      }

      return BitConverter.ToInt64(data, 0);
    }

    public override short ReadShort()
    {
      byte[] data;

      data = new byte[BitHelper.ShortSize];
      if (BitHelper.ShortSize != _stream.Read(data, 0, BitHelper.ShortSize))
      {
        throw new InvalidDataException();
      }

      if (TagWriter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.ShortSize);
      }

      return BitConverter.ToInt16(data, 0);
    }

    public override string ReadString()
    {
      short length;
      byte[] data;

      length = this.ReadShort();
      data = new byte[length];

      if (length != _stream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data.Length != 0 ? Encoding.UTF8.GetString(data) : null;
    }

    public override Tag ReadTag(ReadTagOptions options)
    {
      Tag result;
      TagType type;
      string name;

      type = this.ReadTagType();

      if (type > TagType.IntArray)
      {
        throw new InvalidDataException($"Unrecognized tag type: {type}.");
      }

      if (type != TagType.End && (options & ReadTagOptions.IgnoreName) == 0)
      {
        name = this.ReadString();
      }
      else
      {
        name = string.Empty;
      }

      if ((options & ReadTagOptions.IgnoreValue) == 0)
      {
        result = null;

        switch (type)
        {
          case TagType.End:
            result = TagFactory.CreateTag(TagType.End);
            break;

          case TagType.Byte:
            result = TagFactory.CreateTag(name, this.ReadByte());
            break;

          case TagType.Short:
            result = TagFactory.CreateTag(name, this.ReadShort());
            break;

          case TagType.Int:
            result = TagFactory.CreateTag(name, this.ReadInt());
            break;

          case TagType.IntArray:
            result = TagFactory.CreateTag(name, this.ReadIntArray());
            break;

          case TagType.Long:
            result = TagFactory.CreateTag(name, this.ReadLong());
            break;

          case TagType.Float:
            result = TagFactory.CreateTag(name, this.ReadFloat());
            break;

          case TagType.Double:
            result = TagFactory.CreateTag(name, this.ReadDouble());
            break;

          case TagType.ByteArray:
            result = TagFactory.CreateTag(name, this.ReadByteArray());
            break;

          case TagType.String:
            result = TagFactory.CreateTag(name, this.ReadString());
            break;

          case TagType.List:
            result = TagFactory.CreateTag(name, this.ReadList());
            break;

          case TagType.Compound:
            result = TagFactory.CreateTag(name, this.ReadCompound());
            break;
        }
      }
      else
      {
        // just create a tag with the right name
        result = TagFactory.CreateTag(type);
        result.Name = name;
      }

      return result;
    }

    public override TagType ReadTagType()
    {
      return (TagType)_stream.ReadByte();
    }

    #endregion
  }
}
