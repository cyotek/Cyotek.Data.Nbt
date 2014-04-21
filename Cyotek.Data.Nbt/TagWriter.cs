using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public abstract class TagWriter : ITagWriter
  {
    #region Instance Fields

    private Stream _outputStream;

    #endregion

    #region Protected Constructors

    protected TagWriter()
    { }

    protected TagWriter(Stream output, NbtOptions options)
      : this()
    {
      if (output == null)
      {
        throw new ArgumentNullException("output");
      }

      this.OutputStream = output;
      this.Options = options;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Occurs when the OutputStream property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler OutputStreamChanged;

    #endregion

    #region Public Properties

    public NbtOptions Options { get; protected set; }

    public virtual Stream OutputStream
    {
      get { return _outputStream; }
      protected set
      {
        if (this.OutputStream != value)
        {
          _outputStream = value;

          this.OnOutputStreamChanged(EventArgs.Empty);
        }
      }
    }

    #endregion

    #region Protected Properties

    protected abstract NbtOptions DefaultOptions { get; }

    #endregion

    #region Public Members

    public virtual void Close()
    { }

    public virtual void Open()
    { }

    public abstract void Write(ITag value, NbtOptions options);

    public abstract void Write(byte value);

    public abstract void Write(byte[] value);

    public abstract void Write(double value);

    public abstract void Write(float value);

    public abstract void Write(int value);

    public abstract void Write(int[] value);

    public abstract void Write(long value);

    public abstract void Write(short value);

    public abstract void Write(string value);

    public abstract void Write(TagCompound tag, string fileName, NbtOptions options);

    [DebuggerStepThrough]
    public virtual void Write(ITag value)
    {
      this.Write(value, this.Options);
    }

    public virtual void Write(TagCompound tag, string fileName)
    {
      this.Write(tag, fileName, this.DefaultOptions);
    }

    #endregion

    #region Protected Members

    /// <summary>
    ///   Raises the <see cref="OutputStreamChanged" /> event.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="EventArgs" /> instance containing the event data.
    /// </param>
    protected virtual void OnOutputStreamChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.OutputStreamChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    #endregion

    #region ITagWriter Members

    NbtOptions ITagWriter.Options
    {
      get { return this.Options; }
      set { this.Options = value; }
    }

    Stream ITagWriter.OutputStream
    {
      get { return this.OutputStream; }
      set { this.OutputStream = value; }
    }

    void ITagWriter.Write(TagCompound tag, string fileName)
    {
      this.Write(tag, fileName);
    }

    void ITagWriter.Write(TagCompound tag, string fileName, NbtOptions options)
    {
      this.Write(tag, fileName, options);
    }

    void ITagWriter.Write(ITag value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(ITag value, NbtOptions options)
    {
      this.Write(value, options);
    }

    void ITagWriter.Write(byte value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(byte[] value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(double value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(short value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(int value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(int[] value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(long value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(float value)
    {
      this.Write(value);
    }

    void ITagWriter.Write(string value)
    {
      this.Write(value);
    }

    #endregion
  }
}
