using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagReader : ITagReader
  {
    private const int _doubleSize = 8;

    private const int _floatSize = 4;

    private const int _intSize = 4;

    private const int _longSize = 8;

    private const int _shortSize = 2;

    #region Public Constructors

    public BinaryTagReader()
    { }

    public BinaryTagReader(Stream stream)
      : this()
    {
      _stream = stream;
    }


    #endregion

    #region Overridden Methods

    public virtual TagCompound ReadDocument(Stream stream)
    {
      return this.ReadDocument(stream, ReadTagOptions.None);
    }
    public virtual TagCompound ReadDocument(Stream stream,ReadTagOptions options)
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
      else
      {
        _stream = stream;
        tag = (TagCompound)this.ReadTag(options);
      }

      //BinaryTagReader reader;


      ////Check if gzipped stream
      //try
      //{
      //    using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
      //    {
      //      reader = new BinaryTagReader2(gzipStream, options);
      //      tag = (TagCompound)reader.Read();
      //    }
      //  }
      //}
      //catch (Exception)
      //{
      //  tag = null;
      //}

      //if (tag != null)
      //{
      //  return tag;
      //}

      ////Try Deflate stream
      //try
      //{
      //  using (FileStream input = File.OpenRead(fileName))
      //  {
      //    using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
      //    {
      //      reader = new BinaryTagReader(deflateStream, options);
      //      tag = (TagCompound)reader.Read();
      //    }
      //  }
      //}
      //catch (Exception)
      //{
      //  tag = null;
      //}

      //if (tag != null)
      //{
      //  return tag;
      //}

      ////Assume uncompressed stream
      //using (FileStream input = File.OpenRead(fileName))
      //{
      //  reader = new BinaryTagReader(input, options);
      //  tag = (TagCompound)reader.Read();
      //}

      return tag;
    }

    private Stream _stream;

    public virtual ITag ReadTag(ReadTagOptions options)
    {
      int rawType;
      ITag result;

      rawType = _stream.ReadByte();
      result = TagFactory.CreateTag((TagType)rawType);

      if (result.Type != TagType.End && (options & ReadTagOptions.IgnoreName) == 0)
      {
        result.Name = this.ReadString();
      }

      if ((options & ReadTagOptions.IgnoreValue) == 0)
      {
        object value;

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
            throw new InvalidDataException($"Unrecognized tag type: {rawType}");
        }

        result.Value = value;
      }


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

    public virtual TagCollection ReadCollection(TagList owner)
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

    public virtual TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary results;
      ITag tag;

      results = new TagDictionary(owner);

      tag = this.ReadTag();
      while (tag.Type != TagType.End)
      {
        results.Add(tag);
        tag = this.ReadTag();
      }

      return results;
    }

    public virtual ITag ReadTag()
    {
      return this.ReadTag(ReadTagOptions.None);
    }

    public virtual double ReadDouble()
    {
      byte[] data;

      data = new byte[_doubleSize];
      if (_doubleSize != _stream.Read(data, 0, _doubleSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, _doubleSize);
      }

      return BitConverter.ToDouble(data, 0);
    }

    public virtual float ReadFloat()
    {
      byte[] data;

      data = new byte[_floatSize];
      if (_floatSize != _stream.Read(data, 0, _floatSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, _floatSize);
      }

      return BitConverter.ToSingle(data, 0);
    }

    public virtual int ReadInt()
    {
      byte[] data;

      data = new byte[_intSize];
      if (_intSize != _stream.Read(data, 0, _intSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, _intSize);
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
      bufferLength = length * _intSize;

      buffer = new byte[bufferLength];
      if (bufferLength != _stream.Read(buffer, 0, bufferLength))
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

    public virtual long ReadLong()
    {
      byte[] data;

      data = new byte[_longSize];
      if (_longSize != _stream.Read(data, 0, _longSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, _longSize);
      }

      return BitConverter.ToInt64(data, 0);
    }

    public virtual short ReadShort()
    {
      byte[] data;

      data = new byte[_shortSize];
      if (_shortSize != _stream.Read(data, 0, _shortSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, _shortSize);
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

    #endregion
  }
}
