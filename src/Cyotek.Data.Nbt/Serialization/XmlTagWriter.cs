using System;
using System.Collections.Generic;
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

    private readonly XmlWriter _writer;

    #endregion

    #region Fields

    private Stack<TagState> _openContainers;

    private Stack<TagType> _openTags;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor. <see cref="XmlWriter"/>
    /// </summary>
    /// <param name="writer">The writer.</param>
    public XmlTagWriter(XmlWriter writer)
    {
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

      _writer = XmlWriter.Create(stream, settings);
    }

    #endregion

    #region Methods

    public override void Close()
    {
      _writer.Flush();
      _writer.Close();
    }

    public override void Flush()
    {
      _writer.Flush();
    }

    public override void WriteEndDocument()
    {
      if (_openTags == null)
      {
        throw new InvalidOperationException("No document is currently open");
      }

      _openTags = null;
      _openContainers = null;

      _writer.WriteEndDocument();
      _writer.Flush();
    }

    public override void WriteEndTag()
    {
      TagType type;

      if (_openTags == null)
      {
        throw new InvalidOperationException("No document is currently open");
      }

      if (_openTags.Count == 0)
      {
        throw new InvalidOperationException("No tag is currently open");
      }

      type = _openTags.Pop();

      if (type == TagType.List || type == TagType.Compound)
      {
        TagState state;

        state = _openContainers.Pop();

        if (type == TagType.List && state.ChildCount != state.ExpectedCount)
        {
          throw new InvalidDataException($"Expected {state.ExpectedCount} children, but {state.ChildCount} were written.");
        }
      }
      _writer.WriteEndElement();
    }

    public override void WriteStartDocument()
    {
      if (_openTags != null)
      {
        throw new InvalidOperationException("Document is already open.");
      }

      _openTags = new Stack<TagType>();
      _openContainers = new Stack<TagState>();

      _writer.WriteStartDocument(true);
    }

    public override void WriteStartTag(TagType type, string name)
    {
      TagState currentState;

      if (_openTags == null)
      {
        throw new InvalidOperationException("No document is currently open");
      }

      if (_openTags.Count != 0)
      {
        currentState = _openContainers.Peek();

        if (currentState.Type == TagType.List && currentState.ChildType != TagType.End && type != currentState.ChildType)
        {
          throw new InvalidOperationException($"Attempted to add tag of type '{type}' to container that only accepts '{currentState.ChildType}'");
        }

        currentState.ChildCount++;
      }
      else
      {
        currentState = null;
      }

      _openTags.Push(type);

      if (type == TagType.Compound || type == TagType.List)
      {
        _openContainers.Push(new TagState
                             {
                               Type = type
                             });
      }

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

      if (type != TagType.End && (currentState == null || currentState.Type != TagType.List))
      {
        _writer.WriteAttributeString("type", type.ToString());
      }
    }

    public override void WriteStartTag(TagType type, string name, TagType listType, int count)
    {
      this.WriteStartTag(type, name);

      TagState state;

      state = _openContainers.Peek();
      state.ChildType = listType;
      state.ExpectedCount = count;

      _writer.WriteAttributeString("limitType", listType.ToString());
    }

    protected override void WriteEnd()
    {
      // no op
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
      TagState state;

      state = _openContainers.Peek();
      state.ChildType = value.LimitType;
      state.ExpectedCount = value.Count;

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
