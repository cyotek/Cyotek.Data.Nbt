using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt
{
  public class XmlTagWriter : TagWriter
  {
    #region Instance Fields

    private XmlWriterSettings _settings;

    private XmlWriter _writer;

    #endregion

    #region Public Constructors

    public XmlTagWriter()
    { }

    public XmlTagWriter(Stream stream)
      : this(stream, NbtOptions.ReadHeader)
    { }

    public XmlTagWriter(Stream stream, NbtOptions options)
      : base(stream, options)
    { }

    #endregion

    #region Overridden Properties

    protected override NbtOptions DefaultOptions
    {
      get { return NbtOptions.ReadHeader; }
    }

    #endregion

    #region Overridden Methods

    public override void Close()
    {
      base.Close();

      _writer.WriteEndDocument();
      _writer.Flush();
    }

    public override void Open()
    {
      base.Open();

      _settings = new XmlWriterSettings
                  {
                    Indent = true,
                    Encoding = Encoding.UTF8
                  };

      _writer = XmlWriter.Create(this.OutputStream, _settings);
      _writer.WriteStartDocument(true);
    }

    public override void Write(ITag value, NbtOptions options)
    {
      string name;

      if ((options & NbtOptions.SingleUse) != 0)
      {
        this.Open();
      }

      name = value.Name;
      if (string.IsNullOrEmpty(name))
      {
        name = "tag";
      }

      if (XmlConvert.EncodeName(name) == name)
      {
        _writer.WriteStartElement(name);
      }
      else
      {
        _writer.WriteStartElement("tag");
        _writer.WriteAttributeString("name", name);
      }

      if ((options & NbtOptions.ReadHeader) != 0 && value.Type != TagType.End)
      {
        this.WriteHeader(value);
      }

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

      if ((options & NbtOptions.SingleUse) != 0)
      {
        this.Close();
      }
    }

    public override void Write(string value)
    {
      if (value != null)
      {
        if (value.Contains("<") || value.Contains(">") || value.Contains("&"))
        {
          _writer.WriteCData(value);
        }
        else
        {
          _writer.WriteValue(value);
        }
      }
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
        {
          output.Append(" ");
        }

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

    public override void Write(byte[] value)
    {
      StringBuilder output;

      output = new StringBuilder();
      foreach (byte i in value)
      {
        if (output.Length != 0)
        {
          output.Append(" ");
        }

        output.Append(i);
      }

      this.Write(output.ToString());
    }

    public override void Write(TagCompound tag, string fileName, NbtOptions options)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      if (tag == null)
      {
        throw new ArgumentNullException("tag");
      }

      this.Options = options;

      using (Stream fileStream = File.Create(fileName))
      {
        this.OutputStream = fileStream;
        this.Open();
        this.Write(tag, options);
        this.Close();
      }
    }

    #endregion

    #region Public Members

    public virtual void Write(TagCollection value)
    {
      _writer.WriteAttributeString("limitType", value.LimitType.ToString());

      foreach (ITag item in value)
      {
        this.Write(item, NbtOptions.None);
      }
    }

    public virtual void Write(IEnumerable<ITag> value)
    {
      foreach (ITag item in value)
      {
        this.Write(item, NbtOptions.ReadHeader);
      }
    }

    public virtual void WriteEnd()
    {
      // not supported in XML documents as the tags close themselves
    }

    #endregion

    #region Protected Members

    protected void WriteHeader(ITag value)
    {
      _writer.WriteAttributeString("type", value.Type.ToString());
    }

    #endregion
  }
}
