using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public abstract class TagReader
    : ITagReader
  {
    #region Private Member Declarations

    private Stream _inputStream;

    #endregion Private Member Declarations

    #region Protected Constructors

    protected TagReader()
    { }

    protected TagReader(Stream input, NbtOptions options)
      : this()
    {
      if (input == null)
        throw new ArgumentNullException("input");

      this.InputStream = input;
      this.Options = options;
    }

    #endregion Protected Constructors

    #region Events

    /// <summary>
    /// Occurs when the InputStream property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler InputStreamChanged;

    #endregion Events

    #region Public Abstract Methods

    public abstract TagCompound Load(string fileName, NbtOptions options);

    public abstract ITag Read(NbtOptions options);

    public abstract byte ReadByte();

    public abstract byte[] ReadByteArray();

    public abstract TagCollection ReadCollection(TagList owner);

    public abstract TagDictionary ReadDictionary(TagCompound owner);

    public abstract double ReadDouble();

    public abstract float ReadFloat();

    public abstract int ReadInt();

    public abstract int[] ReadIntArray();

    public abstract long ReadLong();

    public abstract short ReadShort();

    public abstract string ReadString();

    #endregion Public Abstract Methods

    #region Public Methods

    public virtual TagCompound Load(string fileName)
    {
      return this.Load(fileName, this.DefaultOptions);
    }

    [DebuggerStepThrough]
    public virtual ITag Read()
    {
      return this.Read(this.Options);
    }

    #endregion Public Methods

    #region Public Properties

    public virtual Stream InputStream
    {
      get { return _inputStream; }
      set
      {
        if (this.InputStream != value)
        {
          _inputStream = value;

          this.OnInputStreamChanged(EventArgs.Empty);
        }
      }
    }

    public NbtOptions Options { get; set; }

    #endregion Public Properties

    #region Protected Properties

    protected virtual NbtOptions DefaultOptions
    { get { return NbtOptions.Header; } }

    #endregion Protected Properties

    #region Protected Methods

    /// <summary>
    /// Raises the <see cref="E:InputStreamChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnInputStreamChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.InputStreamChanged;

      if (handler != null)
        handler(this, e);
    }

    #endregion Protected Methods
  }
}