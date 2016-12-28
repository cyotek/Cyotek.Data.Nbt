using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public sealed class XmlTagWriter : TagWriter
  {
    #region Constants

    private static readonly char[] _cDataTriggers =
    {
      '<',
      '>',
      '&'
    };

    private readonly TagState _state;

    private readonly XmlWriter _writer;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor. <see cref="XmlWriter"/>
    /// </summary>
    /// <param name="writer">The writer.</param>
    public XmlTagWriter(XmlWriter writer)
    {
      _state = new TagState(FileAccess.Write);
      _writer = writer;
    }

    public XmlTagWriter(Stream stream)
    {
      XmlWriterSettings settings;

      settings = new XmlWriterSettings
                 {
                   Indent = true,
                   Encoding = Encoding.UTF8
                 };

      _state = new TagState(FileAccess.Write);
      _writer = XmlWriter.Create(stream, settings);
    }

    #endregion

    #region Methods

    public override void Close()
    {
      base.Close();

      _writer.Flush();
      _writer.Close();
    }

    public override void Flush()
    {
      _writer.Flush();
    }

    public override void WriteEndDocument()
    {
      _state.SetComplete();

      _writer.WriteEndDocument();
      _writer.Flush();
    }

    public override void WriteEndTag()
    {
      _state.EndTag();

      _writer.WriteEndElement();
    }

    public override void WriteStartDocument()
    {
      _state.Start();

      _writer.WriteStartDocument(true);
    }

    public override void WriteStartTag(TagType type, string name)
    {
      TagContainerState currentState;

      currentState = _state.StartTag(type);

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

      if (type != TagType.End && (currentState == null || currentState.ContainerType != TagType.List))
      {
        _writer.WriteAttributeString("type", type.ToString());
      }
    }

    public override void WriteStartTag(TagType type, string name, TagType listType, int count)
    {
      this.WriteStartTag(type, name);

      _state.StartList(listType, count);

      _writer.WriteAttributeString("limitType", listType.ToString());
    }

    protected override void WriteValue(string value)
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

    protected override void WriteValue(short value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(long value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(int[] value)
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

    protected override void WriteValue(int value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(float value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(double value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(byte value)
    {
      _writer.WriteValue(value);
    }

    protected override void WriteValue(byte[] value)
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

    protected override void WriteValue(TagCollection value)
    {
      _state.StartList(value.LimitType, value.Count);

      _writer.WriteAttributeString("limitType", value.LimitType.ToString());

      foreach (Tag item in value)
      {
        this.WriteTag(item);
      }
    }

    protected override void WriteValue(TagDictionary value)
    {
      foreach (Tag item in value)
      {
        this.WriteTag(item);
      }
    }

    #endregion
  }
}
