using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TR2Tests : TestBase
  {
    private enum ComplextDataLoadState
    {
      NotInitialized,
      Level,
      Compound,
      LongList,
      CompoundList
    }

    [Test]
    public void TestName()
    {
      using (Stream stream = File.OpenRead(this.ComplexDataFileName))
      {
        using (Stream decompressorStream = new GZipStream(stream, CompressionMode.Decompress)) {
          // arrange
          TR2 target;
          ComplexData actual;
          ComplexData expected;
          ComplextDataLoadState state;

          actual = new ComplexData();
          expected = ComplexData.Default;

          target = new TR2(decompressorStream);

          state = ComplextDataLoadState.NotInitialized;

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
                  if(state == ComplextDataLoadState.Compound)
                  {

                  }
                  else
                  {
                    throw new InvalidDataException($"Unexpected token '{target.Name}'");
                  }
                  break;
              }
            }
          }

          // assert
          NbtAssert.AreEqual(expected, actual);
        }
      } }

  }
}
