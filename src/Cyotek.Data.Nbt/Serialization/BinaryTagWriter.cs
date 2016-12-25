using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class BinaryTagWriter : TagWriter
  {
    #region Constants

    private readonly Stream _stream;

    #endregion

    #region Fields

    private Stack<TagState> _openContainers;

    private Stack<TagType> _openTags;

    #endregion

    #region Constructors

    public BinaryTagWriter(Stream stream)
    {
      _stream = stream;
    }

    #endregion

    #region Methods

    public override void Close()
    {
      _stream.Flush();
      _stream.Close();
    }

    public override void Flush()
    {
      _stream.Flush();
    }

    public override void WriteEndDocument()
    {
      if (_openTags == null)
      {
        throw new InvalidOperationException("No document is currently open");
      }

      _openTags = null;
      _openContainers = null;
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

        if (type == TagType.Compound)
        {
          this.WriteEnd();
        }
        else if (state.ChildCount != state.ExpectedCount)
        {
          throw new InvalidDataException($"Expected {state.ExpectedCount} children, but {state.ChildCount} were written.");
        }
      }
    }

    public override void WriteStartDocument()
    {
      if (_openTags != null)
      {
        throw new InvalidOperationException("Document is already open.");
      }

      _openTags = new Stack<TagType>();
      _openContainers = new Stack<TagState>();
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

      if (type != TagType.End && (currentState == null || currentState.Type != TagType.List))
      {
        this.WriteValue((byte)type);
        this.WriteValue(name);
      }
    }

    public override void WriteStartTag(TagType type, string name, TagType listType, int count)
    {
      // HACK: This is messy, rethink

      this.WriteStartTag(type, name);

      TagState state;

      state = _openContainers.Peek();
      state.ChildType = listType;
      state.ExpectedCount = count;

      _stream.WriteByte((byte)listType);
      this.WriteValue(count);
    }

    protected override void WriteEnd()
    {
      _stream.WriteByte((byte)TagType.End);
    }

    protected override void WriteValue(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        this.WriteValue((short)0);
      }
      else
      {
        byte[] buffer;

        buffer = Encoding.UTF8.GetBytes(value);

        if (buffer.Length > short.MaxValue)
        {
          throw new ArgumentException("String data would be truncated.");
        }

        this.WriteValue((short)buffer.Length);
        _stream.Write(buffer, 0, buffer.Length);
      }
    }

    protected override void WriteValue(short value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.ShortSize);
      }

      _stream.Write(buffer, 0, BitHelper.ShortSize);
    }

    protected override void WriteValue(long value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.LongSize);
      }

      _stream.Write(buffer, 0, BitHelper.LongSize);
    }

    protected override void WriteValue(int[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.WriteValue(value.Length);
        foreach (int item in value)
        {
          this.WriteValue(item);
        }
      }
      else
      {
        this.WriteValue(0);
      }
    }

    protected override void WriteValue(int value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.IntSize);
      }

      _stream.Write(buffer, 0, BitHelper.IntSize);
    }

    protected override void WriteValue(float value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.FloatSize);
      }

      _stream.Write(buffer, 0, BitHelper.FloatSize);
    }

    protected override void WriteValue(double value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.DoubleSize);
      }

      _stream.Write(buffer, 0, BitHelper.DoubleSize);
    }

    protected override void WriteValue(byte value)
    {
      _stream.WriteByte(value);
    }

    protected override void WriteValue(byte[] value)
    {
      if (value != null && value.Length != 0)
      {
        this.WriteValue(value.Length);
        _stream.Write(value, 0, value.Length);
      }
      else
      {
        this.WriteValue(0);
      }
    }

    protected override void WriteValue(TagCollection value)
    {
      TagState state;

      state = _openContainers.Peek();
      state.ChildType = value.LimitType;
      state.ExpectedCount = value.Count;

      _stream.WriteByte((byte)value.LimitType);

      this.WriteValue(value.Count);

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
