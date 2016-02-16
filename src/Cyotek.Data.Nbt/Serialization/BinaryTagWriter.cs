using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagWriter : ITagWriter
  {
    #region Constants

    private const int _doubleSize = 8;

    private const int _floatSize = 4;

    private const int _intSize = 4;

    private const int _longSize = 8;

    private const int _shortSize = 2;

    #endregion

    #region Fields

    private Stream _stream;

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

    public virtual void WriteTag(ITag value)
    {
      this.WriteTag(value, WriteTagOptions.None);
    }

    public virtual void WriteTag(ITag value, WriteTagOptions options)
    {
      if (value.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteHeader(value);
      }

      switch (value.Type)
      {
        case TagType.End:
          this.WriteEnd();
          break;

        case TagType.Byte:
          this.WriteValue((byte)value.Value);
          break;

        case TagType.Short:
          this.WriteValue((short)value.Value);
          break;

        case TagType.Int:
          this.WriteValue((int)value.Value);
          break;

        case TagType.Long:
          this.WriteValue((long)value.Value);
          break;

        case TagType.Float:
          this.WriteValue((float)value.Value);
          break;

        case TagType.Double:
          this.WriteValue((double)value.Value);
          break;

        case TagType.ByteArray:
          this.WriteValue((byte[])value.Value);
          break;

        case TagType.String:
          this.WriteValue((string)value.Value);
          break;

        case TagType.List:
          this.WriteValue((TagCollection)value.Value);
          break;

        case TagType.Compound:
          this.WriteValue((TagDictionary)value.Value);
          break;

        case TagType.IntArray:
          this.WriteValue((int[])value.Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", nameof(value));
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

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, _shortSize);
      }

      _stream.Write(buffer, 0, _shortSize);
    }

    public virtual void WriteValue(long value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, _longSize);
      }

      _stream.Write(buffer, 0, _longSize);
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

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, _intSize);
      }

      _stream.Write(buffer, 0, _intSize);
    }

    public virtual void WriteValue(float value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, _floatSize);
      }

      _stream.Write(buffer, 0, _floatSize);
    }

    public virtual void WriteValue(double value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, _doubleSize);
      }

      _stream.Write(buffer, 0, _doubleSize);
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
