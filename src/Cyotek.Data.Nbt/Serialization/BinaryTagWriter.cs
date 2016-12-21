using System;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagWriter : TagWriter
  {
    #region Constants

    private readonly Stream _stream;

    #endregion

    #region Constructors

    public BinaryTagWriter(Stream stream)
    {
      _stream = stream;
    }

    #endregion

    #region Methods

    public override void Flush()
    {
      _stream.Flush();
    }

    public override void WriteEnd()
    {
      _stream.WriteByte((byte)TagType.End);
    }

    public override void WriteEndDocument()
    {
      // no-op
    }

    public override void WriteEndTag()
    {
      // no-op
    }

    public override void WriteStartDocument()
    {
      // no-op
    }

    public override void WriteStartTag(ITag tag, WriteTagOptions options)
    {
      if (tag.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteValue((byte)tag.Type);
        this.WriteValue(tag.Name);
      }
    }

    public override void WriteValue(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        this.WriteValue((short)0);
      }
      else
      {
        byte[] buffer;

        buffer = Encoding.UTF8.GetBytes(value);

        if (buffer.Length > short.MaxValue)
        {
          throw new ArgumentException("String data would be truncated.");
        }

        this.WriteValue((short)buffer.Length);
        _stream.Write(buffer, 0, buffer.Length);
      }
    }

    public override void WriteValue(short value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.ShortSize);
      }

      _stream.Write(buffer, 0, BitHelper.ShortSize);
    }

    public override void WriteValue(long value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.LongSize);
      }

      _stream.Write(buffer, 0, BitHelper.LongSize);
    }

    public override void WriteValue(int[] value)
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

    public override void WriteValue(int value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.IntSize);
      }

      _stream.Write(buffer, 0, BitHelper.IntSize);
    }

    public override void WriteValue(float value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.FloatSize);
      }

      _stream.Write(buffer, 0, BitHelper.FloatSize);
    }

    public override void WriteValue(double value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.DoubleSize);
      }

      _stream.Write(buffer, 0, BitHelper.DoubleSize);
    }

    public override void WriteValue(byte value)
    {
      _stream.WriteByte(value);
    }

    public override void WriteValue(byte[] value)
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

    public override void WriteValue(TagCollection value)
    {
      _stream.WriteByte((byte)value.LimitType);

      this.WriteValue(value.Count);

      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.IgnoreName);
      }
    }

    public override void WriteValue(TagDictionary value)
    {
      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.None);
      }

      this.WriteEnd();
    }

    #endregion
  }
}
