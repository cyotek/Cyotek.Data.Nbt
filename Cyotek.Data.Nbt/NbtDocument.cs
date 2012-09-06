using System;

namespace Cyotek.Data.Nbt
{
  public class NbtDocument
  {
    private Type _readerType;

    private Type _writerType;

    public NbtDocument(Type reader, Type writer)
      : this()
    {
      this.ReaderType = reader;
      this.WriterType = writer;
    }

    public NbtDocument(NbtFormat format)
      : this()
    {
      switch (format)
      {
        case NbtFormat.Binary:
          this.WriterType = typeof(BinaryTagWriter);
          this.ReaderType = typeof(BinaryTagReader);
          break;
        default:
          throw new ArgumentException("Unrecognized or unsupported format.", "format");
      }
    }

    public NbtDocument()
    {
      this.DocumentRoot = new TagCompound();
      this.WriterType = typeof(BinaryTagWriter);
      this.ReaderType = typeof(BinaryTagReader);
    }

    public NbtDocument(string fileName)
      : this()
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      this.Load(fileName);
    }

    public NbtDocument(TagCompound document)
      : this()
    {
      if (document == null)
        throw new ArgumentNullException("document");

      this.DocumentRoot = document;
    }

    public TagCompound DocumentRoot { get; set; }

    public string FileName { get; set; }

    public virtual Type ReaderType
    {
      get { return _readerType; }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");
        else if (!typeof(ITagReader).IsAssignableFrom(value))
          throw new ArgumentException("Cannot assign ITagWriter from specified type.", "value");

        _readerType = value;
      }
    }

    public virtual Type WriterType
    {
      get { return _writerType; }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");
        else if (!typeof(ITagWriter).IsAssignableFrom(value))
          throw new ArgumentException("Cannot assign ITagWriter from specified type.", "value");

        _writerType = value;
      }
    }

    public void Load()
    {
      this.Load(this.FileName);
    }

    public void Load(string fileName)
    {
      ITagReader reader;

      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      reader = (ITagReader)Activator.CreateInstance(this.ReaderType);

      this.DocumentRoot = reader.Load(fileName, NbtOptions.Header);
      this.FileName = fileName;
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
  }
}