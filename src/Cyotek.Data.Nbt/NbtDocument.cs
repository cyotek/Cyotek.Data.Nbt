using System;
using System.IO;
using System.Text;
using Cyotek.Data.Nbt.Serialization;

namespace Cyotek.Data.Nbt
{
  public class NbtDocument
  {
    #region Fields

    private NbtFormat _format;

    #endregion

    #region Constructors

    public NbtDocument()
    {
      this.Format = NbtFormat.Binary;
      this.DocumentRoot = new TagCompound();
    }

    public NbtDocument(TagCompound document)
      : this()
    {
      if (document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }

      this.DocumentRoot = document;
    }

    #endregion

    #region Static Methods

    public static NbtFormat GetDocumentFormat(string fileName)
    {
      NbtFormat format;

      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException("Cannot find file.", fileName);
      }

      using (Stream stream = File.OpenRead(fileName))
      {
        format = GetDocumentFormat(stream);
      }

      return format;
    }

    public static NbtFormat GetDocumentFormat(Stream stream)
    {
      NbtFormat format;

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      if (!stream.CanSeek)
      {
        throw new InvalidDataException("Stream is not seekable.");
      }

      if (IsNbtDocument<BinaryTagReader>(stream))
      {
        format = NbtFormat.Binary;
      }
      else
      {
        stream.Seek(0, SeekOrigin.Begin);

        format = IsNbtDocument<XmlTagReader>(stream) ? NbtFormat.Xml : NbtFormat.Unknown;
      }

      return format;
    }

    public static string GetDocumentName(string fileName)
    {
      string result;

      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException("Cannot find file.", fileName);
      }

      using (Stream stream = File.OpenRead(fileName))
      {
        result = GetDocumentName<BinaryTagReader>(stream);

        // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
        if (result == null)
        {
          stream.Seek(0, SeekOrigin.Begin);
          result = GetDocumentName<XmlTagReader>(stream);
        }
      }

      return result;
    }

    public static bool IsNbtDocument(string fileName)
    {
      return GetDocumentFormat(fileName) != NbtFormat.Unknown;
    }

    public static bool IsNbtDocument(Stream stream)
    {
      return GetDocumentFormat(stream) != NbtFormat.Unknown;
    }

    public static NbtDocument LoadFromFile(string fileName)
    {
      NbtDocument document;

      document = new NbtDocument();
      document.Load(fileName);

      return document;
    }

    private static string GetDocumentName<T>(Stream stream) where T : ITagReader, new()
    {
      ITagReader reader;
      string result;

      reader = new T();

      if (reader.IsNbtDocument(stream))
      {
        TagCompound document;

        document = reader.ReadDocument(stream, ReadTagOptions.IgnoreValue);

        result = document?.Name;
      }
      else
      {
        result = null;
      }

      return result;
    }

    private static bool IsNbtDocument<T>(Stream stream) where T : ITagReader, new()
    {
      ITagReader reader;
      bool result;

      reader = new T();

      result = reader.IsNbtDocument(stream);

      return result;
    }

    #endregion

    #region Properties

    public TagCompound DocumentRoot { get; set; }

    public string FileName { get; set; }

    public NbtFormat Format
    {
      get { return _format; }
      set
      {
        if (_format != value)
        {
          switch (value)
          {
            case NbtFormat.Binary:
            case NbtFormat.Xml:
              _format = value;
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(value), value, null);
          }
        }
      }
    }

    #endregion

    #region Methods

    public void Load()
    {
      this.Load(this.FileName);
    }

    public void Load(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException("File not found.", fileName);
      }

      using (Stream stream = File.OpenRead(fileName))
      {
        this.Load(stream);
      }

      this.FileName = fileName;
    }

    public virtual void Load(Stream stream)
    {
      ITagReader reader;
      NbtFormat format;

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      format = GetDocumentFormat(stream);

      switch (format)
      {
        case NbtFormat.Binary:
          reader = new BinaryTagReader();
          break;
        case NbtFormat.Xml:
          reader = new XmlTagReader();
          break;
        default:
          throw new InvalidDataException("Unrecognized or unsupported file format.");
      }

      this.DocumentRoot = reader.ReadDocument(stream);
      this.Format = format;
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
      this.Save(fileName, CompressionOption.Auto);
    }

    public void Save(Stream stream)
    {
      this.Save(stream, CompressionOption.Auto);
    }

    public virtual void Save(string fileName, CompressionOption compression)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      using (Stream stream = File.Create(fileName))
      {
        this.Save(stream, compression);
      }

      this.FileName = fileName;
    }

    public virtual void Save(Stream stream, CompressionOption compression)
    {
      ITagWriter writer;

      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      switch (_format)
      {
        case NbtFormat.Binary:
          writer = new BinaryTagWriter();
          break;
        case NbtFormat.Xml:
          writer = new XmlTagWriter();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      writer.WriteDocument(stream, this.DocumentRoot, compression);
    }

    public override string ToString()
    {
      StringBuilder result;
      int indent;

      result = new StringBuilder();
      indent = -1;

      if (this.DocumentRoot != null)
      {
        this.WriteTagString(this.DocumentRoot, result, ref indent);
      }

      return result.ToString();
    }

    private void WriteTagString(ITag tag, StringBuilder result, ref int indent)
    {
      ICollectionTag collection;
      ICollectionTag parentCollection;

      indent++;

      result.Append(new string(' ', indent * 2));

      result.Append(tag.Type.ToString().
                        ToLowerInvariant());

      parentCollection = tag.Parent as ICollectionTag;
      if (parentCollection != null && parentCollection.IsList)
      {
        result.Append("#");
        result.Append(parentCollection.Values.IndexOf(tag));
      }
      else
      {
        result.Append(":");
        result.Append(tag.Name);
      }

      if (!(tag is ICollectionTag))
      {
        result.AppendFormat(" [{0}]", tag.ToValueString());
      }

      result.AppendLine();

      collection = tag as ICollectionTag;
      if (collection != null)
      {
        foreach (ITag child in collection.Values)
        {
          this.WriteTagString(child, result, ref indent);
        }
      }

      indent--;
    }

    #endregion
  }
}
