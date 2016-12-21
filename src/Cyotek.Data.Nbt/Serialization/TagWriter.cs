using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Data.Nbt.Serialization
{
  public abstract class TagWriter : IDisposable
  {
    #region Constants

    internal static readonly bool IsLittleEndian;

    #endregion

    #region Static Constructors

    static TagWriter()
    {
      IsLittleEndian = BitConverter.IsLittleEndian;
    }

    #endregion

    #region Static Methods

    public static TagWriter CreateWriter(NbtFormat format, Stream stream)
    {
      TagWriter writer;

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      switch (format)
      {
        case NbtFormat.Binary:
          writer = new BinaryTagWriter(stream);
          break;
        case NbtFormat.Xml:
          writer = new XmlTagWriter(stream);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(format), format, "Invalid format.");
      }

      return writer;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
    [Browsable(false)]
    public bool IsDisposed { get; private set; }

    #endregion

    #region Methods

    public virtual void Close()
    { }

    public abstract void Flush();

    public abstract void WriteEnd();

    public abstract void WriteEndDocument();

    public abstract void WriteEndTag();

    public abstract void WriteStartDocument();

    public abstract void WriteStartTag(ITag tag, WriteTagOptions options);

    public void WriteTag(ITag tag)
    {
      this.WriteTag(tag, WriteTagOptions.None);
    }

    public void WriteTag(ITag tag, WriteTagOptions options)
    {
      this.WriteStartTag(tag, options);

      this.WriteTagValue(tag);

      this.WriteEndTag();
    }

    private void WriteTagValue(ITag tag)
    {
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

    public abstract void WriteValue(byte value);

    public abstract void WriteValue(byte[] value);

    public abstract void WriteValue(double value);

    public abstract void WriteValue(short value);

    public abstract void WriteValue(int value);

    public abstract void WriteValue(int[] value);

    public abstract void WriteValue(long value);

    public abstract void WriteValue(float value);

    public abstract void WriteValue(string value);

    public abstract void WriteValue(TagCollection value);

    public abstract void WriteValue(TagDictionary value);

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
      {
        this.IsDisposed = true;
      }
    }

    #endregion

    #region IDisposable Interface

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);

      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
