using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagWriter : ITagWriter
  {
    #region Constants

    private static readonly bool _isLittleEndian;

    #endregion

    #region Fields

    private Stream _stream;

    #endregion

    #region Static Constructors

    static BinaryTagWriter()
    {
      _isLittleEndian = BitConverter.IsLittleEndian;
    }

    #endregion

    #region Constructors

    public BinaryTagWriter()
    { }

    public BinaryTagWriter(Stream stream)
      : this()
    {
      _stream = stream;
    }

    #endregion

    #region Methods

    protected virtual void WriteEnd()
    {
      _stream.WriteByte((byte)TagType.End);
    }

    protected void WriteHeader(ITag value)
    {
      this.WriteValue((byte)value.Type);
      this.WriteValue(value.Name);
    }

    protected virtual void WriteValue(TagCollection value)
    {
      _stream.WriteByte((byte)value.LimitType);

      this.WriteValue(value.Count);

      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.IgnoreName);
      }
    }

    protected virtual void WriteValue(TagDictionary value)
    {
      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.None);
      }

      this.WriteEnd();
    }

    #endregion

    #region ITagWriter Interface

    public virtual void WriteDocument(Stream stream, TagCompound tag)
    {
      this.WriteDocument(stream, tag, CompressionOption.Auto);
    }

    public virtual void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression)
    {
      if (compression != CompressionOption.Off)
      {
        using (Stream compressedStream = new GZipStream(stream, CompressionMode.Compress, true))
        {
          _stream = compressedStream;
          this.WriteTag(tag, WriteTagOptions.None);
        }
      }
      else
      {
        _stream = stream;
        this.WriteTag(tag, WriteTagOptions.None);
      }
    }

    public virtual void WriteTag(ITag tag)
    {
      this.WriteTag(tag, WriteTagOptions.None);
    }

    public virtual void WriteTag(ITag tag, WriteTagOptions options)
    {
      if (tag.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteHeader(tag);
      }

      switch (tag.Type)
      {
        case TagType.End:
          this.WriteEnd();
          break;

        case TagType.Byte:
          this.WriteValue(((TagByte)tag).Value);
          break;

        case TagType.Short:
          this.WriteValue(((TagShort)tag).Value);
          break;

        case TagType.Int:
          this.WriteValue(((TagInt)tag).Value);
          break;

        case TagType.Long:
          this.WriteValue(((TagLong)tag).Value);
          break;

        case TagType.Float:
          this.WriteValue(((TagFloat)tag).Value);
          break;

        case TagType.Double:
          this.WriteValue(((TagDouble)tag).Value);
          break;

        case TagType.ByteArray:
          this.WriteValue(((TagByteArray)tag).Value);
          break;

        case TagType.String:
          this.WriteValue(((TagString)tag).Value);
          break;

        case TagType.List:
          this.WriteValue(((TagList)tag).Value);
          break;

        case TagType.Compound:
          this.WriteValue(((TagCompound)tag).Value);
          break;

        case TagType.IntArray:
          this.WriteValue(((TagIntArray)tag).Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", nameof(tag));
      }
    }

    public virtual void WriteValue(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        this.WriteValue((short)0);
      }
      else
      {
        byte[] buffer;

        buffer = Encoding.UTF8.GetBytes(value);

        this.WriteValue((short)buffer.Length);
        _stream.Write(buffer, 0, buffer.Length);
      }
    }

    public virtual void WriteValue(short value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.ShortSize);
      }

      _stream.Write(buffer, 0, BitHelper.ShortSize);
    }

    public virtual void WriteValue(long value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.LongSize);
      }

      _stream.Write(buffer, 0, BitHelper.LongSize);
    }

    public virtual void WriteValue(int[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.WriteValue(value.Length);
        foreach (int item in value)
        {
          this.WriteValue(item);
        }
      }
      else
      {
        this.WriteValue(0);
      }
    }

    public virtual void WriteValue(int value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.IntSize);
      }

      _stream.Write(buffer, 0, BitHelper.IntSize);
    }

    public virtual void WriteValue(float value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.FloatSize);
      }

      _stream.Write(buffer, 0, BitHelper.FloatSize);
    }

    public virtual void WriteValue(double value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (_isLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.DoubleSize);
      }

      _stream.Write(buffer, 0, BitHelper.DoubleSize);
    }

    public virtual void WriteValue(byte value)
    {
      _stream.WriteByte(value);
    }

    public virtual void WriteValue(byte[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.WriteValue(value.Length);
        _stream.Write(value, 0, value.Length);
      }
      else
      {
        this.WriteValue(0);
      }
    }

    #endregion
  }
}
