using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public class XmlTagWriter : TagWriter
  {
    #region Constants

    private static readonly char[] _cDataTriggers =
    {
      '<',
      '>',
      '&'
    };

    #endregion

    #region Fields

    private XmlWriter _writer;

    #endregion

    #region Constructors

    public XmlTagWriter()
    { }

    public XmlTagWriter(XmlWriter writer)
      : this()
    {
      _writer = writer;
    }

    #endregion

    #region Methods

    protected void WriteHeader(ITag value)
    {
      _writer.WriteAttributeString("type", value.Type.ToString());
    }

    protected  void WriteValue(TagCollection value)
    {
      _writer.WriteAttributeString("limitType", value.LimitType.ToString());

      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.IgnoreName);
      }
    }

    protected  void WriteValue(TagDictionary value)
    {
      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.None);
      }
    }

    #endregion

    #region ITagWriter Interface

    public override void WriteDocument(Stream stream, TagCompound tag)
    {
      this.WriteDocument(stream, tag, CompressionOption.Auto);
    }

    public override void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression)
    {
      if (compression == CompressionOption.On)
      {
        throw new NotSupportedException("Compression is not supported.");
      }

        XmlWriterSettings settings;

        settings = new XmlWriterSettings
                   {
                     Indent = true,
                     Encoding = Encoding.UTF8
                   };

        _writer = XmlWriter.Create(stream, settings);

      this.WriteStartDocument();
      this.WriteTag(tag, WriteTagOptions.None);
      this.WriteEndDocument();
    }



    public override void WriteTag(ITag tag)
    {
      this.WriteTag(tag, WriteTagOptions.None);
    }

    public override void WriteTag(ITag tag, WriteTagOptions options)
    {
      string name;

      name = tag.Name;
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

      if (tag.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteHeader(tag);
      }

      switch (tag.Type)
      {
        case TagType.End:
          // no op
          break;

        case TagType.Byte:
          this.WriteValue(((TagByte)tag).Value);
          break;

        case TagType.Short:
          this.WriteValue(((TagShort)tag).Value);
          break;

        case TagType.Int:
          this.WriteValue(((TagInt)tag).Value);
          break;

        case TagType.Long:
          this.WriteValue(((TagLong)tag).Value);
          break;

        case TagType.Float:
          this.WriteValue(((TagFloat)tag).Value);
          break;

        case TagType.Double:
          this.WriteValue(((TagDouble)tag).Value);
          break;

        case TagType.ByteArray:
          this.WriteValue(((TagByteArray)tag).Value);
          break;

        case TagType.String:
          this.WriteValue(((TagString)tag).Value);
          break;

        case TagType.List:
          this.WriteValue(((TagList)tag).Value);
          break;

        case TagType.Compound:
          this.WriteValue(((TagCompound)tag).Value);
          break;

        case TagType.IntArray:
          this.WriteValue(((TagIntArray)tag).Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", nameof(tag));
      }

      _writer.WriteEndElement();
    }

    public override void WriteValue(string value)
    {
      if (value != null)
      {
        if (value.IndexOfAny(_cDataTriggers) != -1)
        {
          _writer.WriteCData(value);
        }
        else
        {
          _writer.WriteValue(value);
        }
      }
    }

    public override void WriteValue(short value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(long value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(int[] value)
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

      this.WriteValue(output.ToString());
    }

    public override void WriteValue(int value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(float value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(double value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(byte value)
    {
      _writer.WriteValue(value);
    }

    public override void WriteValue(byte[] value)
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

      this.WriteValue(output.ToString());
    }


    public void WriteStartDocument()
    {
      _writer.WriteStartDocument(true);
    }

    public void WriteEndDocument()
    {
        _writer.WriteEndDocument();
        _writer.Flush();
    }

    public override void Flush()
    {
      _writer.Flush();
    }

    #endregion
  }
}
