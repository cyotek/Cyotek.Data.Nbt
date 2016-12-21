using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagReader : ITagReader
  {
    #region Constants

    private static readonly bool _isLittleEndian;

    #endregion

    #region Fields

    private Stream _stream;

    #endregion

    #region Static Constructors

    static BinaryTagReader()
    {
      _isLittleEndian = BitConverter.IsLittleEndian;
    }

    #endregion

    #region Constructors

    public BinaryTagReader()
    { }

    public BinaryTagReader(Stream stream)
      : this()
    {
      _stream = stream;
    }

    #endregion

    #region ITagReader Interface

    public virtual bool IsNbtDocument(Stream stream)
    {
      bool result;
      long position;

      position = stream.Position;

      try
      {
        if (stream.IsGzipCompressed())
        {
          using (Stream decompressionStream = new GZipStream(stream, CompressionMode.Decompress, true))
          {
            result = decompressionStream.PeekNextByte() == (int)TagType.Compound;
          }
        }
        else if (stream.IsDeflateCompressed())
        {
          using (Stream decompressionStream = new DeflateStream(stream, CompressionMode.Decompress, true))
          {
            result = decompressionStream.PeekNextByte() == (int)TagType.Compound;
          }
        }
        else if (stream.PeekNextByte() == (int)TagType.Compound)
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

      stream.Position = position;

      return result;
    }

    public virtual byte ReadByte()
    {
      int data;

      data = _stream.ReadByte();
      if (data != (data & 0xFF))
      {
        throw new InvalidDataException();
      }

      return (byte)data;
    }

    public virtual byte[] ReadByteArray()
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

    public virtual TagCollection ReadCollection()
    {
      TagCollection tags;
      int length;
      TagType listType;

      listType = (TagType)this.ReadByte();
      tags = new TagCollection(listType);
      length = this.ReadInt();

      for (int i = 0; i < length; i++)
      {
        ITag tag;

        switch (listType)
        {
          case TagType.Byte:
            tag = TagFactory.CreateTag(this.ReadByte());
            break;

          case TagType.ByteArray:
            tag = TagFactory.CreateTag(this.ReadByteArray());
            break;

          case TagType.Compound:
            tag = TagFactory.CreateTag(this.ReadDictionary());
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
            tag = TagFactory.CreateTag(this.ReadCollection());
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

    public virtual TagDictionary ReadDictionary()
    {
      TagDictionary results;
      ITag tag;

      results = new TagDictionary();

      tag = this.ReadTag();
      while (tag.Type != TagType.End)
      {
        results.Add(tag);
        tag = this.ReadTag();
      }

      return results;
    }

    public virtual TagCompound ReadDocument(Stream stream)
    {
      return this.ReadDocument(stream, ReadTagOptions.None);
    }

    public virtual TagCompound ReadDocument(Stream stream, ReadTagOptions options)
    {
      TagCompound tag;

      if (stream.IsGzipCompressed())
      {
        using (Stream decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
        {
          _stream = decompressionStream;
          tag = (TagCompound)this.ReadTag(options);
        }
      }
      else if (stream.IsDeflateCompressed())
      {
        using (Stream decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
        {
          _stream = decompressionStream;
          tag = (TagCompound)this.ReadTag(options);
        }
      }
      else if (stream.PeekNextByte() == (int)TagType.Compound)
      {
        _stream = stream;
        tag = (TagCompound)this.ReadTag(options);
      }
      else
      {
        throw new InvalidDataException("Source stream does not contain a NBT document.");
      }

      return tag;
    }

    public virtual double ReadDouble()
    {
      byte[] data;

      data = new byte[BitHelper.DoubleSize];
      if (BitHelper.DoubleSize != _stream.Read(data, 0, BitHelper.DoubleSize))
      {
        throw new InvalidDataException();
      }

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.DoubleSize);
      }

      return BitConverter.ToDouble(data, 0);
    }

    public virtual float ReadFloat()
    {
      byte[] data;

      data = new byte[BitHelper.FloatSize];
      if (BitHelper.FloatSize != _stream.Read(data, 0, BitHelper.FloatSize))
      {
        throw new InvalidDataException();
      }

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.FloatSize);
      }

      return BitConverter.ToSingle(data, 0);
    }

    public virtual int ReadInt()
    {
      byte[] data;

      data = new byte[BitHelper.IntSize];
      if (BitHelper.IntSize != _stream.Read(data, 0, BitHelper.IntSize))
      {
        throw new InvalidDataException();
      }

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.IntSize);
      }

      return BitConverter.ToInt32(data, 0);
    }

    public virtual int[] ReadIntArray()
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
        if (_isLittleEndian)
        {
          BitHelper.SwapBytes(buffer, i * 4, 4);
        }

        ints[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      return ints;
    }

    public virtual long ReadLong()
    {
      byte[] data;

      data = new byte[BitHelper.LongSize];
      if (BitHelper.LongSize != _stream.Read(data, 0, BitHelper.LongSize))
      {
        throw new InvalidDataException();
      }

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.LongSize);
      }

      return BitConverter.ToInt64(data, 0);
    }

    public virtual short ReadShort()
    {
      byte[] data;

      data = new byte[BitHelper.ShortSize];
      if (BitHelper.ShortSize != _stream.Read(data, 0, BitHelper.ShortSize))
      {
        throw new InvalidDataException();
      }

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.ShortSize);
      }

      return BitConverter.ToInt16(data, 0);
    }

    public virtual string ReadString()
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

    public virtual ITag ReadTag(ReadTagOptions options)
    {
      int rawType;
      ITag result;
      TagType type;
      string name;

      rawType = _stream.ReadByte();
      type = (TagType)rawType;

      if (type > TagType.IntArray)
      {
        throw new InvalidDataException($"Unrecognized tag type: {rawType}.");
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
            result = TagFactory.CreateTag(name, this.ReadCollection());
            break;

          case TagType.Compound:
            result = TagFactory.CreateTag(name, this.ReadDictionary());
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

    public virtual ITag ReadTag()
    {
      return this.ReadTag(ReadTagOptions.None);
    }

    #endregion
  }
}
