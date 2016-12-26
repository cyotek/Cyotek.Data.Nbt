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
    }

    private Stream _stream;

    public bool Read()
    {
      int value;

      switch (_state)
      {
        case ReadState.NotInitialized:
          value = _stream.ReadByte();
          if (value != (int)TagType.Compound)
          {
            throw new InvalidDataException("Stream does not contain a NBT compound.");
          }
          _type = TagType.Compound;
          _state = ReadState.Tag;
          break;
        case ReadState.TagType:
          value = _stream.ReadByte();
          this.SetType(value);
          break;
        case ReadState.Tag:
        case ReadState.TagName:
        case ReadState.TagValue:
          this.MoveToNextElement();
          value = 0;
          break;
        //  case ReadState.TagEnd:
        //      break;
        case ReadState.End:
          value = -1; // Done reading, nothing left to do
          break;
        default:
          throw new InvalidOperationException($"Unexpected read state '{_state}'.");
      }

      if (value == -1)
      {
        _state = ReadState.End;
      }

      return value != -1;
    }

    private void SetType(int value)
    {
      _type = (TagType)value;

      if (_type < TagType.None || _type > TagType.IntArray)
      {
        throw new InvalidDataException($"Unexpected type '{value}'.");
      }

      _state = ReadState.Tag;
    }

    private void MoveToNextElement()
    {
      if (_state == ReadState.Tag)
      {
        // skip past the name
      }

      if (_state == ReadState.TagName)
      {
        // skip past the value
      }

      if (_state == ReadState.TagValue)
      {
        this.SkipValue();
      }
    }

    private static readonly byte[] _byteBuffer=new byte[1];
    private static readonly byte[] _shortBuffer = new byte[BitHelper.ShortSize];
    private static readonly byte[] _intBuffer = new byte[BitHelper.IntSize];
    private static readonly byte[] _longBuffer = new byte[BitHelper.LongSize];
    private static readonly byte[] _floatBuffer = new byte[BitHelper.FloatSize];
    private static readonly byte[] _doubleBuffer = new byte[BitHelper.DoubleSize];
    private static readonly byte[] _stringBuffer=new byte[short.MaxValue];

    private void SkipValue()
    {
      switch (_type)
      {
//        case TagType.None:
  //        break;
    //    case TagType.End:
      //    break;
        case TagType.Byte:
          _stream.Read(_byteBuffer, 0, 1);
          break;
        case TagType.Short:
          _stream.Read(_shortBuffer, 0, BitHelper.ShortSize);
          break;
        case TagType.Int:
          _stream.Read(_intBuffer, 0, BitHelper.IntSize);
          break;
        case TagType.Long:
          _stream.Read(_longBuffer, 0, BitHelper.LongSize);
          break;
        case TagType.Float:
          _stream.Read(_floatBuffer, 0, BitHelper.FloatSize);
          break;
        case TagType.Double:
          _stream.Read(_doubleBuffer, 0, BitHelper.DoubleSize);
          break;
        case TagType.ByteArray:
          break;
        case TagType.String:
          int length;
          length = this.ReadShortImpl();
          _stream.Read(_stringBuffer, 0, length);
          _state = ReadState.TagType;
          break;
        case TagType.List:
          break;
        case TagType.Compound:
          int nextByte;
          // peek the next byte?
          nextByte = _stream.ReadByte();

          if (nextByte == -1)
          {
            _state = ReadState.End;
          }
          else
          {
            this.SetType(nextByte);
          }
          break;
        case TagType.IntArray:
          break;
        default:
          throw new NotImplementedException(_type.ToString());
      }
    }

    public bool IsStartElement()
    {
      return _state == ReadState.Tag;
    }

    private enum ReadState
    {
      NotInitialized,
      TagType,
      Tag,
      TagName,
      TagValue,
      TagEnd,
      End
    }

    private ReadState _state;

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
      if (_state == ReadState.Tag)
      {
        _name = this.ReadStringImpl();
        _state = ReadState.TagValue;
      }
    }

    public short ReadShort2()
    {
      short value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Short)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadShortImpl();

      _state = ReadState.TagType;

      return value;
    }

    public long ReadLong2()
    {
      long value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Long)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadLongImpl();

      _state = ReadState.TagType;

      return value;
    }

    public int ReadInt2()
    {
      int value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Int)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadIntImpl();

      _state = ReadState.TagType;

      return value;
    }

    public float ReadFloat2()
    {
      float value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Float)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadFloatImpl();

      _state = ReadState.TagType;

      return value;
    }
    public double ReadDouble2()
    {
      double value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Double)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadDoubleImpl();

      _state = ReadState.TagType;

      return value;
    }

    public string ReadString2()
    {
      string value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.String)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadStringImpl();

      _state = ReadState.TagType;

      return value;
    }
    public byte ReadByte2()
    {
      byte value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.Byte)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadByteImpl();

      _state = ReadState.TagType;

      return value;
    }

    public byte[] ReadByteArray2()
    {
      byte[] value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.ByteArray)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadByteArrayImpl();

      _state = ReadState.TagType;

      return value;
    }

    public int[] ReadIntArray2()
    {
      int[] value;

      if (_state != ReadState.TagValue)
      {
        throw new InvalidOperationException("Wrong state.");
      }

      if (_type != TagType.IntArray)
      {
        throw new InvalidOperationException("Wrong value type.");
      }

      value = this.ReadIntArrayImpl();

      _state = ReadState.TagType;

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
