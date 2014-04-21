using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public abstract class TagReader : ITagReader
  {
    #region Instance Fields

    private Stream _inputStream;

    #endregion

    #region Protected Constructors

    protected TagReader()
    { }

    protected TagReader(Stream input, NbtOptions options)
      : this()
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      this.InputStream = input;
      this.Options = options;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Occurs when the InputStream property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler InputStreamChanged;

    #endregion

    #region Public Properties

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

    #endregion

    #region Protected Properties

    protected virtual NbtOptions DefaultOptions
    {
      get { return NbtOptions.ReadHeader; }
    }

    #endregion

    #region Public Members

    public abstract TagCompound Load(string fileName, NbtOptions options);

    public virtual TagCompound Load(string fileName)
    {
      return this.Load(fileName, this.DefaultOptions);
    }

    public abstract ITag Read(NbtOptions options);

    [DebuggerStepThrough]
    public virtual ITag Read()
    {
      return this.Read(this.Options);
    }

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

    #endregion

    #region Protected Members

    /// <summary>
    ///   Raises the <see cref="InputStreamChanged" /> event.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="EventArgs" /> instance containing the event data.
    /// </param>
    protected virtual void OnInputStreamChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.InputStreamChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    #endregion

    #region ITagReader Members

    Stream ITagReader.InputStream
    {
      get { return this.InputStream; }
      set { this.InputStream = value; }
    }

    NbtOptions ITagReader.Options
    {
      get { return this.Options; }
      set { this.Options = value; }
    }

    TagCompound ITagReader.Load(string fileName, NbtOptions options)
    {
      return this.Load(fileName, options);
    }

    ITag ITagReader.Read()
    {
      return this.Read();
    }

    ITag ITagReader.Read(NbtOptions options)
    {
      return this.Read(options);
    }

    byte ITagReader.ReadByte()
    {
      return this.ReadByte();
    }

    byte[] ITagReader.ReadByteArray()
    {
      return this.ReadByteArray();
    }

    TagCollection ITagReader.ReadCollection(TagList owner)
    {
      return this.ReadCollection(owner);
    }

    TagDictionary ITagReader.ReadDictionary(TagCompound owner)
    {
      return this.ReadDictionary(owner);
    }

    double ITagReader.ReadDouble()
    {
      return this.ReadDouble();
    }

    float ITagReader.ReadFloat()
    {
      return this.ReadFloat();
    }

    int ITagReader.ReadInt()
    {
      return this.ReadInt();
    }

    int[] ITagReader.ReadIntArray()
    {
      return this.ReadIntArray();
    }

    long ITagReader.ReadLong()
    {
      return this.ReadLong();
    }

    short ITagReader.ReadShort()
    {
      return this.ReadShort();
    }

    string ITagReader.ReadString()
    {
      return this.ReadString();
    }

    #endregion
  }
}
