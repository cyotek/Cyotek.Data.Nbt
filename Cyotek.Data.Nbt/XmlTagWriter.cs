using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt
{
  public class XmlTagWriter
    : TagWriter
  {
    private XmlWriterSettings _settings;

    private XmlWriter _writer;

    public XmlTagWriter()
      : base()
    { }

    public XmlTagWriter(Stream stream, NbtOptions options)
      : base(stream, options)
    { }

    protected override NbtOptions DefaultOptions
    { get { return NbtOptions.Header; } }

    public override void Write(ITag value, NbtOptions options)
    {
      string name;

      name = value.Name;
      if (string.IsNullOrEmpty(name))
        name = "tag";

      if (XmlConvert.EncodeName(name) == name)
        _writer.WriteStartElement(name);
      else
      {
        _writer.WriteStartElement("tag");
        _writer.WriteAttributeString("name", name);
      }

      if (options.HasFlag(NbtOptions.Header) && value.Type != TagType.End)
        this.WriteHeader(value);

      switch (value.Type)
      {
        case TagType.End:
          this.WriteEnd();
          break;

        case TagType.Byte:
          this.Write((byte)value.Value);
          break;

        case TagType.Short:
          this.Write((short)value.Value);
          break;

        case TagType.Int:
          this.Write((int)value.Value);
          break;

        case TagType.Long:
          this.Write((long)value.Value);
          break;

        case TagType.Float:
          this.Write((float)value.Value);
          break;

        case TagType.Double:
          this.Write((double)value.Value);
          break;

        case TagType.ByteArray:
          this.Write((byte[])value.Value);
          break;

        case TagType.String:
          this.Write((string)value.Value);
          break;

        case TagType.List:
          this.Write((TagCollection)value.Value);
          break;

        case TagType.Compound:
          this.Write((TagDictionary)value.Value);
          break;

        case TagType.IntArray:
          this.Write((int[])value.Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", "value");
      }

      _writer.WriteEndElement();
    }

    public virtual void Write(TagCollection value)
    {
      if (value.LimitType == TagType.None || value.LimitType == TagType.End)
        throw new TagException("Limit type not set.");

      _writer.WriteAttributeString("limitType", value.LimitType.ToString());

      foreach (ITag item in value)
        this.Write(item, NbtOptions.None);
    }

    public override void Write(string value)
    {
      if (value.Contains("<") || value.Contains(">") || value.Contains("&"))
        _writer.WriteCData(value);
      else
        _writer.WriteValue(value);
    }

    public override void Write(short value)
    {
      _writer.WriteValue(value);
    }

    public override void Write(long value)
    {
      _writer.WriteValue(value);
    }

    public override void Write(int[] value)
    {
      StringBuilder output;

      output = new StringBuilder();
      foreach (int i in value)
      {
        if (output.Length != 0)
          output.Append(" ");

        output.Append(i);
      }

      this.Write(output.ToString());
    }

    public override void Write(int value)
    {
      _writer.WriteValue(value);
    }

    public override void Write(float value)
    {
      _writer.WriteValue(value);
    }

    public override void Write(double value)
    {
      _writer.WriteValue(value);
    }

    public override void Write(byte value)
    {
      _writer.WriteValue(value);
    }

    public virtual void Write(TagDictionary value)
    {
      foreach (ITag item in value)
        this.Write(item, NbtOptions.Header);
    }

    public override void Write(byte[] value)
    {
      StringBuilder output;

      output = new StringBuilder();
      foreach (int i in value)
      {
        if (output.Length != 0)
          output.Append(" ");

        output.Append(i);
      }

      this.Write(output.ToString());
    }

    public override void Write(TagCompound tag, string fileName, NbtOptions options)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      else if (tag == null)
        throw new ArgumentNullException("tag");

      this.Options = options;

      using (Stream fileStream = File.Create(fileName))
      {
        this.OutputStream = fileStream;
        this.Write(tag, options);
        _writer.WriteEndDocument();
        _writer.Flush();
      }
    }

    public virtual void WriteEnd()
    {
    }

    protected override void OnOutputStreamChanged(EventArgs e)
    {
      base.OnOutputStreamChanged(e);

      _settings = new XmlWriterSettings()
      {
        Indent = true,
        Encoding = Encoding.UTF8
      };

      _writer = XmlWriter.Create(this.OutputStream, _settings);
      _writer.WriteStartDocument(true);
    }

    protected void WriteHeader(ITag value)
    {
      _writer.WriteAttributeString("type", value.Type.ToString());
    }
  }
}