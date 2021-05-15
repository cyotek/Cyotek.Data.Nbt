using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public sealed class XmlTagWriter : TagWriter
  {
    #region Private Fields

    private static readonly char[] _cDataTriggers =
    {
      '<',
      '>',
      '&'
    };

    private readonly TagState _state;

    private readonly XmlWriter _writer;

    private StringBuilder _arraySb;

    #endregion Private Fields

    #region Public Constructors

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

    #endregion Public Constructors

    #region Public Methods

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

    public override void WriteArrayValue(byte value)
    {
      if (_arraySb.Length != 0)
      {
        _arraySb.Append(' ');
      }

      _arraySb.Append(value);
    }

    public override void WriteArrayValue(int value)
    {
      if (_arraySb.Length != 0)
      {
        _arraySb.Append(' ');
      }

      _arraySb.Append(value);
    }

    public override void WriteArrayValue(long value)
    {
      if (_arraySb.Length != 0)
      {
        _arraySb.Append(' ');
      }

      _arraySb.Append(value);
    }

    public override void WriteEndDocument()
    {
      _state.SetComplete();

      _writer.WriteEndDocument();
      _writer.Flush();
    }

    public override void WriteEndTag()
    {
      TagType currentTag;

      currentTag = _state.CurrentTag;

      if ((currentTag == TagType.ByteArray || currentTag == TagType.IntArray || currentTag == TagType.LongArray) && _arraySb != null && _arraySb.Length != 0)
      {
        _writer.WriteValue(_arraySb.ToString());
        _arraySb.Length = 0;
      }

      _state.EndTag();

      _writer.WriteEndElement();
    }

    public override void WriteStartArray(string name, TagType type, int count)
    {
      // ReSharper disable once ConvertIfStatementToSwitchStatement
      if (type == TagType.Byte)
      {
        type = TagType.ByteArray;
      }
      else if (type == TagType.Int)
      {
        type = TagType.IntArray;
      }
      else if (type == TagType.Long)
      {
        type = TagType.LongArray;
      }
      else if (type != TagType.ByteArray && type != TagType.IntArray && type != TagType.Long)
      {
        throw new ArgumentException("Only byte, 32bit integer or 64bit integer types are supported.", nameof(type));
      }

      if (_arraySb == null)
      {
        _arraySb = new StringBuilder();
      }

      this.WriteStartTag(name, type);
    }

    public override void WriteStartDocument()
    {
      _state.Start();

      _writer.WriteStartDocument(true);
    }

    public override void WriteStartTag(string name, TagType type)
    {
      TagContainerState currentState;

      currentState = _state.StartTag(type);

      if (XmlTagWriter.IsValidName(name))
      {
        _writer.WriteStartElement(name);
      }
      else
      {
        _writer.WriteStartElement("tag");

        if (!string.IsNullOrEmpty(name))
        {
          _writer.WriteAttributeString("name", name);
        }
      }

      if (type != TagType.End && (currentState == null || currentState.ContainerType != TagType.List))
      {
        _writer.WriteAttributeString("type", type.ToString());
      }
    }

    public override void WriteStartTag(string name, TagType type, TagType listType, int count)
    {
      this.WriteStartTag(name, type);

      _state.StartList(listType, count);

      _writer.WriteAttributeString("limitType", listType.ToString());
    }

    #endregion Public Methods

    #region Protected Methods

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
          output.Append(' ');
        }

        output.Append(i);
      }

      _writer.WriteValue(output.ToString());
    }

    protected override void WriteValue(long[] value)
    {
      StringBuilder output;

      output = new StringBuilder();

      foreach (long i in value)
      {
        if (output.Length != 0)
        {
          output.Append(' ');
        }

        output.Append(i);
      }

      _writer.WriteValue(output.ToString());
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
          output.Append(' ');
        }

        output.Append(i);
      }

      _writer.WriteValue(output.ToString());
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

    #endregion Protected Methods

    #region Private Methods

    private static bool IsValidName(string name)
    {
      bool result;

      if (!string.IsNullOrEmpty(name) && XmlTagWriter.IsValidNameStartingCharacter(name[0]))
      {
        result = true;

        for (int i = 1; i < name.Length; i++)
        {
          if (!XmlTagWriter.IsValidNameCharacter(name[i]))
          {
            result = false;
            break;
          }
        }
      }
      else
      {
        result = false;
      }

      return result;
    }

    private static bool IsValidNameCharacter(char c)
    {
      return XmlTagWriter.IsValidNameStartingCharacter(c) || c >= 48 && c <= 57 || c == '-' || c == '.';
    }

    private static bool IsValidNameStartingCharacter(char c)
    {
      // According to the spec (https://www.w3.org/TR/REC-xml/#NT-NameStartChar),
      // : is a valid starting character but .NET throws an exception regardless
      // also ignoring extended Unicode for now
      return c >= 65 && c <= 90 || c >= 97 && c <= 122 || c == '_';
    }

    #endregion Private Methods
  }
}
