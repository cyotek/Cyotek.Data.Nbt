using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public abstract class TagWriter
    : ITagWriter
  {
    private Stream _outputStream;

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

    /// <summary>
    /// Occurs when the OutputStream property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler OutputStreamChanged;

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

    protected abstract NbtOptions DefaultOptions { get; }

    [DebuggerStepThrough]
    public virtual void Write(ITag value)
    {
      this.Write(value, this.Options);
    }

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

    public virtual void Write(TagCompound tag, string fileName)
    {
      this.Write(tag, fileName, this.DefaultOptions);
    }

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
  }
}