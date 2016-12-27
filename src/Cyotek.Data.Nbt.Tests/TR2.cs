using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cyotek.Data.Nbt.Serialization
{
  public class TR2
  {
    public TR2(Stream stream)
    {
      _stream = stream;
      _state = new TagState(FileAccess.Read);
    }

    private Stream _stream;

    public bool Read()
    {
      switch (_readState)
      {
        case ReadState.NotInitialized:
          int value;
          value = _stream.ReadByte();
          if (value != (int)TagType.Compound)
          {
            throw new InvalidDataException("Stream does not contain a NBT compound.");
          }
          _type = TagType.Compound;
          _readState = ReadState.Tag;
          _state.Start();
          _state.StartTag(_type);
          break;
        case ReadState.TagEnd:
        case ReadState.TagType:
          this.SetType(_stream.ReadByte());
          break;
        case ReadState.Tag:
        case ReadState.TagValue:
          this.MoveToNextElement();
          break;
        case ReadState.End:
          _state.SetComplete();
          break;
        default:
          throw new InvalidOperationException($"Unexpected read state '{_readState}'.");
      }

      return _readState != ReadState.End;
    }

    private void SetType(int value)
    {
      if (value != -1)
      {
        _type = (TagType)value;

        if (_type < TagType.None || _type > TagType.IntArray)
        {
          throw new InvalidDataException($"Unexpected type '{value}'.");
        }

        _state.StartTag(_type);
        _readState = _type != TagType.End ? ReadState.Tag : ReadState.TagEnd;
      }
      else
      {
        _readState = ReadState.End;
      }
    }

    public bool MoveToNextElement()
    {
      if (_readState == ReadState.NotInitialized)
      {
        this.Read();
      }

      if (_readState == ReadState.TagEnd || _readState == ReadState.TagType)
      {
        this.SetType(_stream.ReadByte());
      }

      if (_readState == ReadState.Tag)
      {
        // skip past the name
        this.ReadName();
        _readState = ReadState.TagValue;
      }

      if (_readState == ReadState.TagValue)
      {
        this.SkipValue();
      }

      return _readState != ReadState.End;
    }

    private static readonly byte[] _byteBuffer = new byte[1];
    private static readonly byte[] _shortBuffer = new byte[BitHelper.ShortSize];
    private static readonly byte[] _intBuffer = new byte[BitHelper.IntSize];
    private static readonly byte[] _longBuffer = new byte[BitHelper.LongSize];
    private static readonly byte[] _floatBuffer = new byte[BitHelper.FloatSize];
    private static readonly byte[] _doubleBuffer = new byte[BitHelper.DoubleSize];
    private static readonly byte[] _stringBuffer = new byte[short.MaxValue];

    private void SkipValue()
    {
      switch (_type)
      {
        case TagType.Byte:
          _stream.Read(_byteBuffer, 0, 1);
          _readState = ReadState.TagType;
          break;
        case TagType.Short:
          _stream.Read(_shortBuffer, 0, BitHelper.ShortSize);
          _readState = ReadState.TagType;
          break;
        case TagType.Int:
          _stream.Read(_intBuffer, 0, BitHelper.IntSize);
          _readState = ReadState.TagType;
          break;
        case TagType.Long:
          _stream.Read(_longBuffer, 0, BitHelper.LongSize);
          _readState = ReadState.TagType;
          break;
        case TagType.Float:
          _stream.Read(_floatBuffer, 0, BitHelper.FloatSize);
          _readState = ReadState.TagType;
          break;
        case TagType.Double:
          _stream.Read(_doubleBuffer, 0, BitHelper.DoubleSize);
          _readState = ReadState.TagType;
          break;
        case TagType.ByteArray:
          break;
        case TagType.String:
          this.ReadStringData();
          _readState = ReadState.TagType;
          break;
        case TagType.List:
          _stream.Read(_intBuffer, 0, BitHelper.IntSize);
          _readState = ReadState.TagType;
          break;
        case TagType.Compound:
          this.SetType(_stream.ReadByte());
          break;
        case TagType.IntArray:
          break;
        default:
          throw new NotImplementedException(_type.ToString());
      }
    }

    private void ReadStringData()
    {
      int length;
      length = this.ReadShortImpl();
      _stream.Read(_stringBuffer, 0, length);
    }

    public bool IsStartElement()
    {
      return _readState == ReadState.Tag && _type != TagType.End;
    }

    public bool IsEndElement()
    {
      return _type == TagType.End;
    }

    private enum ReadState
    {
      NotInitialized,
      TagType,
      Tag,
      TagValue,
      TagEnd,
      End
    }

    private ReadState _readState;

    private TagState _state;

    public string Name
    {
      get
      {
        this.ReadName();

        return _name;
      }
    }

    private void ReadName()
    {
      if (_readState == ReadState.Tag)
      {
        if (_type != TagType.End)
        {
          _name = this.ReadStringImpl();
        }
        _readState = ReadState.TagValue;
      }
    }

    public short ReadShort2()
    {
      short value;

      this.InitializeValueRead(TagType.Short);

      value = this.ReadShortImpl();

      _readState = ReadState.TagType;

      return value;
    }

    private void InitializeValueRead(TagType tagType)
    {
      if (_readState == ReadState.TagType)
      {
        this.SetType(_stream.ReadByte());
      }

      if (_readState == ReadState.Tag)
      {
        this.ReadName();
      }

      if (_readState != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != tagType)
      {
        throw new InvalidOperationException("Wrong value type.");
      }
    }

    public long ReadLong2()
    {
      long value;

      this.InitializeValueRead(TagType.Long);

      value = this.ReadLongImpl();

      _readState = ReadState.TagType;

      return value;
    }

    public int ReadInt2()
    {
      int value;

      this.InitializeValueRead(TagType.Int);

      value = this.ReadIntImpl();

      _readState = ReadState.TagType;

      return value;
    }

    public float ReadFloat2()
    {
      float value;

      this.InitializeValueRead(TagType.Float);

      value = this.ReadFloatImpl();

      _readState = ReadState.TagType;

      return value;
    }
    public double ReadDouble2()
    {
      double value;

      this.InitializeValueRead(TagType.Double);

      value = this.ReadDoubleImpl();

      _readState = ReadState.TagType;

      return value;
    }

    public string ReadString2()
    {
      string value;

      this.InitializeValueRead(TagType.String);

      value = this.ReadStringImpl();

      _readState = ReadState.TagType;

      return value;
    }
    public byte ReadByte2()
    {
      byte value;

      this.InitializeValueRead(TagType.Byte);

      value = this.ReadByteImpl();

      _readState = ReadState.TagType;

      return value;
    }

    public byte[] ReadByteArray2()
    {
      byte[] value;

      this.InitializeValueRead(TagType.ByteArray);


      value = this.ReadByteArrayImpl();

      _readState = ReadState.TagType;

      return value;
    }

    public int[] ReadIntArray2()
    {
      int[] value;

      this.InitializeValueRead(TagType.IntArray);


      value = this.ReadIntArrayImpl();

      _readState = ReadState.TagType;

      return value;
    }


    public TagType Type
    { get { return _type; } }

    private string _name;
    private TagType _type;
    private byte[] _data;
    private short _dataLength;

    private string ReadStringImpl()
    {
      short length;
      byte[] data;

      length = this.ReadShortImpl();
      data = new byte[length];

      if (length != _stream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data.Length != 0 ? Encoding.UTF8.GetString(data) : null;
    }

    private short ReadShortImpl()
    {
      byte[] data;

      data = new byte[BitHelper.ShortSize];
      if (BitHelper.ShortSize != _stream.Read(data, 0, BitHelper.ShortSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.ShortSize);
      }

      return BitConverter.ToInt16(data, 0);
    }

    private long ReadLongImpl()
    {
      byte[] data;

      data = new byte[BitHelper.LongSize];
      if (BitHelper.LongSize != _stream.Read(data, 0, BitHelper.LongSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.LongSize);
      }

      return BitConverter.ToInt64(data, 0);
    }

    private double ReadDoubleImpl()
    {
      byte[] data;

      data = new byte[BitHelper.DoubleSize];
      if (BitHelper.DoubleSize != _stream.Read(data, 0, BitHelper.DoubleSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.DoubleSize);
      }

      return BitConverter.ToDouble(data, 0);
    }

    private float ReadFloatImpl()
    {
      byte[] data;

      data = new byte[BitHelper.FloatSize];
      if (BitHelper.FloatSize != _stream.Read(data, 0, BitHelper.FloatSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.FloatSize);
      }

      return BitConverter.ToSingle(data, 0);
    }

    private int ReadIntImpl()
    {
      byte[] data;

      data = new byte[BitHelper.IntSize];
      if (BitHelper.IntSize != _stream.Read(data, 0, BitHelper.IntSize))
      {
        throw new InvalidDataException();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(data, 0, BitHelper.IntSize);
      }

      return BitConverter.ToInt32(data, 0);
    }

    private byte ReadByteImpl()
    {
      int data;

      data = _stream.ReadByte();
      if (data != (data & 0xFF))
      {
        throw new InvalidDataException();
      }

      return (byte)data;
    }

    private byte[] ReadByteArrayImpl()
    {
      int length;
      byte[] data;

      length = this.ReadIntImpl();

      data = new byte[length];
      if (length != _stream.Read(data, 0, length))
      {
        throw new InvalidDataException();
      }

      return data;
    }

    private int[] ReadIntArrayImpl()
    {
      int length;
      int bufferLength;
      byte[] buffer;
      int[] ints;
      bool isLittleEndian;

      isLittleEndian = BitConverter.IsLittleEndian;
      length = this.ReadIntImpl();
      bufferLength = length * BitHelper.IntSize;

      buffer = new byte[bufferLength];
      if (bufferLength != _stream.Read(buffer, 0, bufferLength))
      {
        throw new InvalidDataException();
      }

      ints = new int[length];
      for (int i = 0; i < length; i++)
      {
        if (isLittleEndian)
        {
          BitHelper.SwapBytes(buffer, i * 4, 4);
        }

        ints[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      return ints;
    }
  }
}
