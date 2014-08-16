using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class BinaryTagWriter : TagWriter
  {
    #region Constants

    internal static readonly int DoubleSize = 8;

    internal static readonly int FloatSize = 4;

    internal static readonly int IntSize = 4;

    internal static readonly int LongSize = 8;

    internal static readonly int ShortSize = 2;

    #endregion

    #region Public Constructors

    public BinaryTagWriter()
    { }

    public BinaryTagWriter(Stream stream)
      : this(stream, NbtOptions.ReadHeader | NbtOptions.Compress)
    { }

    public BinaryTagWriter(Stream stream, NbtOptions options)
      : base(stream, options)
    { }

    #endregion

    #region Overridden Properties

    protected override NbtOptions DefaultOptions
    {
      get { return NbtOptions.ReadHeader | NbtOptions.Compress; }
    }

    #endregion

    #region Overridden Methods

    public override void Write(ITag value, NbtOptions options)
    {
      if ((options & NbtOptions.ReadHeader) != 0 && value.Type != TagType.End)
      {
        this.WriteHeader(value);
      }

      switch (value.Type)
      {
        case TagType.End:
          this.WriteEnd();
          break;

        case TagType.Byte:
          this.Write((byte)value.Value);
          break;

        case TagType.Short:
          this.Write((short)value.Value);
          break;

        case TagType.Int:
          this.Write((int)value.Value);
          break;

        case TagType.Long:
          this.Write((long)value.Value);
          break;

        case TagType.Float:
          this.Write((float)value.Value);
          break;

        case TagType.Double:
          this.Write((double)value.Value);
          break;

        case TagType.ByteArray:
          this.Write((byte[])value.Value);
          break;

        case TagType.String:
          this.Write((string)value.Value);
          break;

        case TagType.List:
          this.Write((TagCollection)value.Value);
          break;

        case TagType.Compound:
          this.Write((TagDictionary)value.Value);
          break;

        case TagType.IntArray:
          this.Write((int[])value.Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", "value");
      }
    }

    public override void Write(string value)
    {
      byte[] data;

      if (string.IsNullOrEmpty(value))
      {
        data = new byte[0];
      }
      else
      {
        data = Encoding.UTF8.GetBytes(value);
      }

      this.Write((short)data.Length);
      this.OutputStream.Write(data, 0, data.Length);
    }

    public override void Write(short value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, ShortSize);
      }

      this.OutputStream.Write(data, 0, ShortSize);
    }

    public override void Write(long value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, LongSize);
      }

      this.OutputStream.Write(data, 0, LongSize);
    }

    public override void Write(int[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.Write(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
          this.Write(value[i]);
        }
      }
      else
      {
        this.Write(0);
      }
    }

    public override void Write(int value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, IntSize);
      }

      this.OutputStream.Write(data, 0, IntSize);
    }

    public override void Write(float value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, FloatSize);
      }

      this.OutputStream.Write(data, 0, FloatSize);
    }

    public override void Write(double value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, DoubleSize);
      }

      this.OutputStream.Write(data, 0, DoubleSize);
    }

    public override void Write(byte value)
    {
      this.OutputStream.WriteByte(value);
    }

    public override void Write(byte[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.Write(value.Length);
        this.OutputStream.Write(value, 0, value.Length);
      }
      else
      {
        this.Write(0);
      }
    }

    public override void Write(TagCompound tag, string fileName, NbtOptions options)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      if (tag == null)
      {
        throw new ArgumentNullException("tag");
      }

      this.Options = options;

      if ((options & NbtOptions.Compress) != 0)
      {
        this.WriteCompressed(tag, fileName);
      }
      else
      {
        this.WriteUncompressed(tag, fileName);
      }
    }

    #endregion

    #region Public Members

    public virtual void Write(TagCollection value)
    {
      this.OutputStream.WriteByte((byte)value.LimitType);

      this.Write(value.Count);

      foreach (ITag item in value)
      {
        this.Write(item, NbtOptions.None);
      }
    }

    public virtual void Write(TagDictionary value)
    {
      foreach (ITag item in value)
      {
        this.Write(item, NbtOptions.ReadHeader);
      }

      this.WriteEnd();
    }

    public virtual void WriteEnd()
    {
      this.OutputStream.WriteByte((byte)TagType.End);
    }

    #endregion

    #region Protected Members

    protected void WriteCompressed(TagCompound tag, string fileName)
    {
      using (Stream fileStream = File.Open(fileName, FileMode.Create))
      {
        using (Stream output = new GZipStream(fileStream, CompressionMode.Compress))
        {
          this.OutputStream = output;
          this.Write(tag);
        }
      }
    }

    protected void WriteHeader(ITag value)
    {
      this.Write((byte)value.Type);
      this.Write(value.Name);
    }

    protected void WriteUncompressed(TagCompound tag, string fileName)
    {
      using (FileStream output = File.Open(fileName, FileMode.Create))
      {
        this.OutputStream = output;
        this.Write(tag);
      }
    }

    #endregion
  }
}
