using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class BinaryTagWriter
    : TagWriter
  {
    internal static readonly int DoubleSize = 8;

    internal static readonly int FloatSize = 4;

    internal static readonly int IntSize = 4;

    internal static readonly int LongSize = 8;

    internal static readonly int ShortSize = 2;

    public BinaryTagWriter()
      : base()
    { }

    public BinaryTagWriter(Stream stream, NbtOptions options)
      : base(stream, options)
    { }

    protected override NbtOptions DefaultOptions
    { get { return NbtOptions.Header | NbtOptions.Compress; } }

    public override void Write(ITag value, NbtOptions options)
    {
      if (options.HasFlag(NbtOptions.Header) && value.Type != TagType.End)
        this.WriteHeader(value);

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

    public virtual void Write(TagCollection value)
    {
      if (value.LimitType == TagType.None || value.LimitType == TagType.End)
        throw new TagException("Limit type not set.");

      this.OutputStream.WriteByte((byte)value.LimitType);

      this.Write(value.Count);

      foreach (ITag item in value)
        this.Write(item, NbtOptions.None);
    }

    public override void Write(string value)
    {
      byte[] data;

      if (string.IsNullOrEmpty(value))
        data = new byte[0];
      else
        data = Encoding.UTF8.GetBytes(value);

      this.Write((short)data.Length);
      this.OutputStream.Write(data, 0, data.Length);
    }

    public override void Write(short value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.ShortSize);

      this.OutputStream.Write(data, 0, BinaryTagWriter.ShortSize);
    }

    public override void Write(long value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.LongSize);

      this.OutputStream.Write(data, 0, BinaryTagWriter.LongSize);
    }

    public override void Write(int[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.Write(value.Length);
        for (int i = 0; i < value.Length; i++)
          this.Write(value[i]);
      }
      else
        this.Write(0);
    }

    public override void Write(int value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.IntSize);

      this.OutputStream.Write(data, 0, BinaryTagWriter.IntSize);
    }

    public override void Write(float value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.FloatSize);

      this.OutputStream.Write(data, 0, BinaryTagWriter.FloatSize);
    }

    public override void Write(double value)
    {
      byte[] data;

      data = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
        BitHelper.SwapBytes(data, 0, BinaryTagWriter.DoubleSize);

      this.OutputStream.Write(data, 0, BinaryTagWriter.DoubleSize);
    }

    public override void Write(byte value)
    {
      this.OutputStream.WriteByte(value);
    }

    public virtual void Write(TagDictionary value)
    {
      foreach (ITag item in value)
        this.Write(item, NbtOptions.Header);

      this.WriteEnd();
    }

    public override void Write(byte[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.Write(value.Length);
        this.OutputStream.Write(value, 0, value.Length);
      }
      else
        this.Write((byte)0);
    }

    public override void Write(TagCompound tag, string fileName, NbtOptions options)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      else if (tag == null)
        throw new ArgumentNullException("tag");

      this.Options = options;

      if (options.HasFlag(NbtOptions.Compress))
        this.WriteCompressed(tag, fileName);
      else
        this.WriteUncompressed(tag, fileName);
    }

    public virtual void WriteEnd()
    {
      this.OutputStream.WriteByte((byte)TagType.End);
    }

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
  }
}