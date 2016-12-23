using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Data.Nbt.Serialization
{
  public abstract partial class TagWriter : IDisposable
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

    public abstract void WriteEndDocument();

    public abstract void WriteEndTag();

    public abstract void WriteStartDocument();

    public void WriteStartTag(Tag tag, WriteTagOptions options)
    {
      this.WriteStartTag(tag.Type, tag.Name, options);
    }

    public void WriteStartTag(TagType type, string name)
    {
      this.WriteStartTag(type, name, WriteTagOptions.None);
    }

    public abstract void WriteStartTag(TagType type, string name, WriteTagOptions options);

    public abstract void WriteStartTag(TagType type, string name, TagType listType, int count);

    public void WriteTag(Tag tag)
    {
      this.WriteTag(tag, WriteTagOptions.None);
    }

    public void WriteTag(Tag tag, WriteTagOptions options)
    {
      this.WriteStartTag(tag, options);
      this.WriteValue(tag);
      this.WriteEndTag();
    }

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

    protected abstract void WriteEnd();

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
