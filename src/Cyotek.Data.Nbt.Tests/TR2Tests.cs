using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TR2Tests : TestBase
  {
    #region  Tests

    [Test]
    public void ReadByte_test()
    {
      this.ReadTest<byte>(byte.MaxValue >> 1, (writer, value) => writer.WriteTag(value), reader => reader.ReadByte2());
    }

    [Test]
    public void ReadByteArray_test()
    {
      this.ReadTest(new byte[]
                    {
                      0, 128, 255
                    }, (writer, value) => writer.WriteTag(value), reader => reader.ReadByteArray2());
    }

    [Test]
    public void ReadDouble_test()
    {
      this.ReadTest<double>(double.MaxValue / 2, (writer, value) => writer.WriteTag(value), reader => reader.ReadDouble2());
    }

    [Test]
    public void ReadFloat_test()
    {
      this.ReadTest<float>(float.MaxValue / 2, (writer, value) => writer.WriteTag(value), reader => reader.ReadFloat2());
    }

    [Test]
    public void ReadInt_test()
    {
      this.ReadTest<int>(int.MaxValue >> 1, (writer, value) => writer.WriteTag(value), reader => reader.ReadInt2());
    }

    [Test]
    public void ReadIntArray_test()
    {
      this.ReadTest(new int[]
                    {
                      int.MinValue, short.MinValue, short.MaxValue, int.MaxValue
                    }, (writer, value) => writer.WriteTag(value), reader => reader.ReadIntArray2());
    }

    [Test]
    public void ReadLong_test()
    {
      this.ReadTest<long>(long.MaxValue >> 1, (writer, value) => writer.WriteTag(value), reader => reader.ReadLong2());
    }

    [Test]
    public void ReadShort_test()
    {
      this.ReadTest<short>(short.MaxValue >> 1, (writer, value) => writer.WriteTag(value), reader => reader.ReadShort2());
    }

    [Test]
    public void ReadString_test()
    {
      this.ReadTest<string>("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!", (writer, value) => writer.WriteTag(value), reader => reader.ReadString2());
    }

    [Test]
    [Ignore]
    public void TestName()
    {
      using (Stream stream = File.OpenRead(this.ComplexDataFileName))
      {
        using (Stream decompressorStream = new GZipStream(stream, CompressionMode.Decompress))
        {
          // arrange
          TR2 target;
          ComplexData actual;
          ComplexData expected;
          ComplextDataLoadState state;
          ComplexData.Compound1 currentCompound1;
          ComplexData.Compound2 currentCompound2;

          actual = new ComplexData();
          expected = ComplexData.Default;

          target = new TR2(decompressorStream);

          state = ComplextDataLoadState.NotInitialized;
          currentCompound1 = null;
          currentCompound2 = null;

          // act
          while (target.Read())
          {
            if (target.IsStartElement())
            {
              Trace.WriteLine(target.Type);
              Trace.WriteLine(target.Name);
              //Trace.WriteLine(target.Value)

              switch (target.Name)
              {
                case "Level":
                  state = ComplextDataLoadState.Level;
                  break;
                case "longTest":
                  if (state == ComplextDataLoadState.Level)
                  {
                    actual.LongValue = target.ReadLong2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "shortTest":
                  if (state == ComplextDataLoadState.Level)
                  {
                    actual.ShortValue = target.ReadShort2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "stringTest":
                  if (state == ComplextDataLoadState.Level)
                  {
                    actual.StringValue = target.ReadString2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "floatTest":
                  if (state == ComplextDataLoadState.Level)
                  {
                    actual.FloatValue = target.ReadFloat2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "intTest":
                  if (state == ComplextDataLoadState.Level)
                  {
                    actual.IntegerValue = target.ReadInt2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "nested compound test":
                  state = ComplextDataLoadState.Compound;
                  break;
                case "ham":
                case "egg":
                  if (state == ComplextDataLoadState.Compound)
                  {
                    currentCompound1 = new ComplexData.Compound1();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "name":
                  if (state == ComplextDataLoadState.Compound)
                  {
                    state = ComplextDataLoadState.CompoundItem;
                    currentCompound1.Name = target.ReadString2();
                    actual.CompoundValue.Add(currentCompound1.Name, currentCompound1);
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "value":
                  if (state == ComplextDataLoadState.CompoundItem)
                  {
                    currentCompound1.Value = target.ReadFloat2();
                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
                case "listTest (long)":
                  state = ComplextDataLoadState.LongList;
                  break;
              }
            }
            else if (target.IsEndElement())
            {
              switch (state)
              {
                case ComplextDataLoadState.NotInitialized:
                  break;
                case ComplextDataLoadState.Level:
                  break;
                case ComplextDataLoadState.CompoundItem:
                  state = ComplextDataLoadState.Compound;
                  break;
                case ComplextDataLoadState.Compound:
                  state = ComplextDataLoadState.Level;
                  break;
                case ComplextDataLoadState.LongList:
                  break;
                case ComplextDataLoadState.CompoundList:
                  break;
              }
            }
          }

          // assert
          NbtAssert.AreEqual(expected, actual);
        }
      }
    }

    [Test]
    [Ignore]
    public void Types_load_in_correct_order()
    {
      using (Stream stream = File.OpenRead(this.ComplexDataFileName))
      {
        using (Stream decompressorStream = new GZipStream(stream, CompressionMode.Decompress))
        {
          // arrange
          TR2 target;
          List<TagType> actual;
          TagType[] expected;

          expected = new TagType[0];
          actual = new List<TagType>();

          target = new TR2(decompressorStream);

          // act
          while (target.MoveToNextElement())
          {
            Debug.WriteLine(target.Name);
            actual.Add(target.Type);
          }

          // assert
          Assert.AreEqual(expected, actual);
        }
      }
    }

    #endregion

    #region Test Helpers

    private enum ComplextDataLoadState
    {
      NotInitialized,

      Level,

      Compound,

      CompoundItem,

      LongList,

      CompoundList
    }

    private void ReadTest<T>(T expected, Action<TagWriter, T> write, Func<TR2, T> read)
    {
      using (Stream stream = new MemoryStream())
      {
        // arrange
        TR2 target;
        T actual;

        using (BinaryTagWriter writer = new BinaryTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteStartTag(TagType.Compound);
          write(writer, expected);
          writer.WriteEndTag();
          writer.WriteEndDocument();
        }

        stream.Position = 0;

        target = new TR2(stream);
        target.MoveToNextElement(); // compound

        // act
        actual = read(target);

        // assert
        Assert.AreEqual(expected, actual);
      }
    }

    #endregion
  }
}
