using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public abstract class TagWriter
    : ITagWriter
  {
    #region Private Member Declarations

    private Stream _outputStream;

    #endregion Private Member Declarations

    #region Protected Constructors

    protected TagWriter()
    { }

    protected TagWriter(Stream output, NbtOptions options)
      : this()
    {
      if (output == null)
        throw new ArgumentNullException("output");

      this.OutputStream = output;
      this.Options = options;
    }

    #endregion Protected Constructors

    #region Events

    /// <summary>
    /// Occurs when the OutputStream property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler OutputStreamChanged;

    #endregion Events

    #region Public Abstract Methods

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

    #endregion Public Abstract Methods

    #region Public Methods

    [DebuggerStepThrough]
    public virtual void Write(ITag value)
    {
      this.Write(value, this.Options);
    }

    public virtual void Write(TagCompound tag, string fileName)
    {
      this.Write(tag, fileName, this.DefaultOptions);
    }

    #endregion Public Methods

    #region Public Properties

    public NbtOptions Options { get; set; }

    public virtual Stream OutputStream
    {
      get { return _outputStream; }
      set
      {
        if (this.OutputStream != value)
        {
          _outputStream = value;

          this.OnOutputStreamChanged(EventArgs.Empty);
        }
      }
    }

    #endregion Public Properties

    #region Protected Properties

    protected abstract NbtOptions DefaultOptions { get; }

    #endregion Protected Properties

    #region Protected Methods

    /// <summary>
    /// Raises the <see cref="E:OutputStreamChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnOutputStreamChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.OutputStreamChanged;

      if (handler != null)
        handler(this, e);
    }

    #endregion Protected Methods
  }
}