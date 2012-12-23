using System;
using System.IO;
using System.IO.Compression;

namespace Cyotek.Data.Nbt
{
  public class NbtDocument
  {
    #region Private Member Declarations

    private NbtFormat _format;
    private Type _readerType;
    private Type _writerType;

    #endregion Private Member Declarations

    #region Static Constructors

    static NbtDocument()
    {
      NbtDocument.DefaultFormat = NbtFormat.Binary;
    }

    #endregion Static Constructors

    #region Public Constructors

    public NbtDocument()
      : this(NbtDocument.DefaultFormat)
    { }

    public NbtDocument(NbtFormat format)
    {
      this.DocumentRoot = new TagCompound();
      this.Format = format;
    }

    public NbtDocument(string fileName)
      : this(fileName, NbtDocument.GetDocumentFormat(fileName))
    { }

    public NbtDocument(TagCompound document)
      : this()
    {
      if (document == null)
        throw new ArgumentNullException("document");

      this.DocumentRoot = document;
    }

    public NbtDocument(Type reader, Type writer)
      : this(NbtFormat.Custom)
    {
      this.ReaderType = reader;
      this.WriterType = writer;
    }

    public NbtDocument(string fileName, NbtFormat format)
      : this(format)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      else if (format == NbtFormat.Custom)
        throw new ArgumentException("Invalid or unsupported file format.", "format");

      this.Load(fileName);
    }

    #endregion Public Constructors

    #region Public Class Methods

    public static NbtFormat GetDocumentFormat(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      return new NbtDocument(NbtFormat.Custom).GetFormat(fileName);
    }

    public static NbtDocument LoadDocument(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      return new NbtDocument(fileName);
    }

    #endregion Public Class Methods

    #region Public Class Properties

    public static NbtFormat DefaultFormat { get; set; }

    #endregion Public Class Properties

    #region Public Methods

    public virtual NbtFormat GetFormat(string fileName)
    {
      NbtFormat format;

      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      else if (!File.Exists(fileName))
        throw new FileNotFoundException("Cannot find file.", fileName);

      if (this.IsGzipDocument(fileName) || this.IsDeflateDocument(fileName) || this.IsRawDocument(fileName))
        format = NbtFormat.Binary;
      else if (this.IsXmlDocument(fileName))
        format = NbtFormat.XML;
      else
        format = NbtFormat.Custom;

      return format;
    }

    public void Load()
    {
      this.Load(this.FileName);
    }

    public void Load(string fileName)
    {
      ITagReader reader;
      NbtFormat format;

      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      format = this.GetFormat(fileName);
      if (format == NbtFormat.Custom && this.ReaderType == null)
        throw new ArgumentNullException("Cannot load custom formatted documents when appropriate reader not specified.");

      this.Format = format;
      reader = (ITagReader)Activator.CreateInstance(this.ReaderType);

      this.DocumentRoot = reader.Load(fileName, NbtOptions.Header);
      this.FileName = fileName;
    }

    public ITag Query(string query)
    {
      return this.Query<ITag>(query);
    }

    public T Query<T>(string query) where T : ITag
    {
      return this.DocumentRoot.Query<T>(query);
    }

    public void Save()
    {
      this.Save(this.FileName);
    }

    public void Save(string fileName)
    {
      this.Save(fileName, NbtOptions.Compress | NbtOptions.Header);
    }

    public void Save(string fileName, NbtOptions options)
    {
      ITagWriter writer;

      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      writer = (ITagWriter)Activator.CreateInstance(this.WriterType);

      writer.Write(this.DocumentRoot, fileName, options);
      this.FileName = fileName;
    }

    #endregion Public Methods

    #region Public Properties

    public TagCompound DocumentRoot { get; set; }

    public string FileName { get; set; }

    public virtual NbtFormat Format
    {
      get { return _format; }
      set
      {
        if (_format != value)
        {
          _format = value;

          switch (_format)
          {
            case NbtFormat.Binary:
              this.WriterType = typeof(BinaryTagWriter);
              this.ReaderType = typeof(BinaryTagReader);
              break;

            case NbtFormat.XML:
              this.WriterType = typeof(XmlTagWriter);
              this.ReaderType = typeof(XmlTagReader);
              break;

            case NbtFormat.Custom:
              this.WriterType = null;
              this.ReaderType = null;
              break;

            default:
              throw new ArgumentException("Invalid format.", "value");
          }
        }
      }
    }

    public virtual Type ReaderType
    {
      get { return _readerType; }
      set
      {
        if (value != null && !typeof(ITagReader).IsAssignableFrom(value))
          throw new ArgumentException("Cannot assign ITagReader from specified type.", "value");

        _readerType = value;
      }
    }

    public virtual Type WriterType
    {
      get { return _writerType; }
      set
      {
        if (value != null && !typeof(ITagWriter).IsAssignableFrom(value))
          throw new ArgumentException("Cannot assign ITagWriter from specified type.", "value");

        _writerType = value;
      }
    }

    #endregion Public Properties

    #region Private Methods

    private bool IsDeflateDocument(string fileName)
    {
      bool result;

      try
      {
        using (Stream stream = File.OpenRead(fileName))
        {
          using (Stream decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
            result = ((TagType)decompressionStream.ReadByte() == TagType.Compound);
        }
      }
      catch
      {
        result = false;
      }

      return result;
    }

    private bool IsGzipDocument(string fileName)
    {
      bool result;

      try
      {
        using (Stream stream = File.OpenRead(fileName))
        {
          using (Stream decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
            result = ((TagType)decompressionStream.ReadByte() == TagType.Compound);
        }
      }
      catch
      {
        result = false;
      }

      return result;
    }

    private bool IsRawDocument(string fileName)
    {
      bool result;

      try
      {
        using (Stream stream = File.OpenRead(fileName))
          result = ((TagType)stream.ReadByte() == TagType.Compound);
      }
      catch
      {
        result = false;
      }

      return result;
    }

    private bool IsXmlDocument(string fileName)
    {
      bool result;

      try
      {
        using (Stream stream = File.OpenRead(fileName))
        {
          using (StreamReader reader = new StreamReader(stream))
          {
            char[] buffer;

            buffer = new char[1];
            reader.Read(buffer, 0, 1);

            result = buffer[0] == '<';
          }
        }
      }
      catch
      {
        result = false;
      }

      return result;
    }

    #endregion Private Methods

    public static string GetDocumentName(string fileName)
    {
      string result;

      result = null;

      if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
      {
        try
        {
          NbtDocument document;

          document = NbtDocument.LoadDocument(fileName);

          result = document.DocumentRoot.Name;
        }
        catch
        {
          // ignore errors
        }
      }

      return result;
    }
  }
}