using System.IO;
using System.IO.Compression;
using System.Linq;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagTests : TestBase
  {
    #region  Tests

    [Test]
    public void FlattenTest()
    {
      // arrange
      TagCompound target;
      Tag[] tags;
      string[] expectedNames;
      string[] actualNames;
      int expectedCount;
      int actualCount;

      target = this.CreateComplexData();
      expectedCount = 29;
      expectedNames = new[]
                      {
                        "Level",
                        "longTest",
                        "shortTest",
                        "stringTest",
                        "floatTest",
                        "intTest",
                        "nested compound test",
                        "ham",
                        "name",
                        "value",
                        "egg",
                        "listTest (long)",
                        "",
                        "listTest (compound)",
                        "created-on",
                        "byteTest",
                        "byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))",
                        "doubleTest"
                      };

      // act
      tags = target.Flatten();
      actualCount = tags.Length;
      actualNames = tags.Select(t => t.Name).Distinct().ToArray();

      // assert
      Assert.AreEqual(expectedCount, actualCount);
      CollectionAssert.AreEqual(expectedNames, actualNames);
    }

    [Test]
    public void FullPathTest()
    {
      // arrange
      TagCompound data;
      Tag target;
      string expected;
      string actual;

      data = this.CreateComplexData();
      expected = @"Level\listTest (compound)\0\name";
      target = data.Query(@"listTest (compound)\0\name");

      // act
      actual = target.FullPath;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetAncestorsTest()
    {
      // arrange
      TagCompound data;
      Tag target;
      Tag[] actual;
      Tag[] expected;

      data = this.CreateComplexData();
      target = data.Query(@"listTest (compound)\0\name");
      expected = new[]
                 {
                   data,
                   data.Value["listTest (compound)"],
                   data.Query(@"listTest (compound)\0")
                 };

      // act
      actual = target.GetAncestors();

      // assert
      CollectionAssert.AreEqual(expected, actual);
    }

    //[Test]
    //public void GetValueTest()
    //{
    //  // arrange
    //  TagCompound target;
    //  byte[] expected;
    //  byte[] actual;

    //  expected = File.ReadAllBytes(this.UncompressedComplexDataFileName);
    //  target = this.CreateComplexData();

    //  // act
    //  actual = target.GetValue();

    //  // assert
    //  CollectionAssert.AreEqual(expected, actual);
    //}

    [Test]
    public void NameChangedEventTest()
    {
      // arrange
      Tag target;
      bool eventRaised;

      eventRaised = false;
      target = new TagString();
      target.NameChanged += delegate { eventRaised = true; };

      // act
      target.Name = "newvalue";

      // assert
      Assert.IsTrue(eventRaised);
    }

    [Test]
    [ExpectedException(ExpectedException = typeof(InvalidDataException), ExpectedMessage = "Unrecognized tag type: 255.")]
    public void ReadExceptionTest()
    {
      // arrange
      MemoryStream stream;
      TagReader reader;

      stream = new MemoryStream();
      stream.WriteByte(255);
      stream.Seek(0, SeekOrigin.Begin);
      reader = new BinaryTagReader(stream);

      // act
      reader.ReadTag();

      // assert
    }

    [Test]
    [ExpectedException(ExpectedException = typeof(TagException), ExpectedMessage = "Cannot remove this tag, parent not set or not supported.")]
    public void RemoveExceptionTest()
    {
      // arrange
      TagCompound target;

      target = this.CreateComplexData();

      // act
      target.Remove();

      // assert
    }

    [Test]
    public void RemoveTest()
    {
      // arrange
      TagCompound data;
      Tag target;
      string key;

      data = this.CreateComplexData();
      key = "listTest (compound)";
      target = data.Value[key];

      // act
      target.Remove();

      // assert
      Assert.IsFalse(data.Contains(key));
    }

    [Test]
    public void TestLoadComplexNbt()
    {
      Tag tag;

      tag = this.CreateComplexData();

      Assert.IsNotNull(tag);
      Assert.IsInstanceOf<TagCompound>(tag);
      TagCompound level = tag as TagCompound;
      Assert.AreEqual("Level", level.Name);

      TagShort shortTest = level.GetShort("shortTest");
      Assert.IsNotNull(shortTest);
      Assert.AreEqual("shortTest", shortTest.Name);
      Assert.AreEqual(32767, shortTest.Value);

      TagLong longTest = level.GetLong("longTest");
      Assert.IsNotNull(longTest);
      Assert.AreEqual("longTest", longTest.Name);
      Assert.AreEqual(9223372036854775807, longTest.Value);

      TagFloat floatTest = level.GetFloat("floatTest");
      Assert.IsNotNull(floatTest);
      Assert.AreEqual("floatTest", floatTest.Name);
      Assert.AreEqual(0.49823147f, floatTest.Value);

      TagString stringTest = level.GetString("stringTest");
      Assert.IsNotNull(stringTest);
      Assert.AreEqual("stringTest", stringTest.Name);
      Assert.AreEqual("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!", stringTest.Value);

      TagInt intTest = level.GetInt("intTest");
      Assert.IsNotNull(intTest);
      Assert.AreEqual("intTest", intTest.Name);
      Assert.AreEqual(2147483647, intTest.Value);

      TagCompound nestedCompoundTest = level.GetCompound("nested compound test");
      Assert.IsNotNull(nestedCompoundTest);
      Assert.AreEqual("nested compound test", nestedCompoundTest.Name);

      TagCompound ham = nestedCompoundTest.GetCompound("ham");
      Assert.IsNotNull(ham);
      Assert.AreEqual("ham", ham.Name);

      TagString ham_name = ham.GetString("name");
      Assert.IsNotNull(ham_name);
      Assert.AreEqual("name", ham_name.Name);
      Assert.AreEqual("Hampus", ham_name.Value);

      TagFloat ham_value = ham.GetFloat("value");
      Assert.IsNotNull(ham_value);
      Assert.AreEqual("value", ham_value.Name);
      Assert.AreEqual(0.75f, ham_value.Value);

      TagCompound egg = nestedCompoundTest.GetCompound("egg");
      Assert.IsNotNull(egg);
      Assert.AreEqual("egg", egg.Name);

      TagString egg_name = egg.GetString("name");
      Assert.IsNotNull(egg_name);
      Assert.AreEqual("name", egg_name.Name);
      Assert.AreEqual("Eggbert", egg_name.Value);

      TagFloat egg_value = egg.GetFloat("value");
      Assert.IsNotNull(egg_value);
      Assert.AreEqual("value", egg_value.Name);
      Assert.AreEqual(0.5f, egg_value.Value);

      TagByte byteTest = level.GetByte("byteTest");
      Assert.IsNotNull(byteTest);
      Assert.AreEqual("byteTest", byteTest.Name);
      Assert.AreEqual(0x7f, byteTest.Value);

      TagDouble doubleTest = level.GetDouble("doubleTest");
      Assert.IsNotNull(doubleTest);
      Assert.AreEqual("doubleTest", doubleTest.Name);
      Assert.AreEqual(0.4931287132182315, doubleTest.Value);

      TagList listTest_long = level.GetList("listTest (long)");
      Assert.IsNotNull(listTest_long);
      Assert.AreEqual("listTest (long)", listTest_long.Name);
      Assert.IsNotNull(listTest_long.Value);
      Assert.AreEqual(5, listTest_long.Value.Count);
      Assert.AreEqual(11, (listTest_long.Value[0] as TagLong).Value);
      Assert.AreEqual(12, (listTest_long.Value[1] as TagLong).Value);
      Assert.AreEqual(13, (listTest_long.Value[2] as TagLong).Value);
      Assert.AreEqual(14, (listTest_long.Value[3] as TagLong).Value);
      Assert.AreEqual(15, (listTest_long.Value[4] as TagLong).Value);

      TagList listTest_compound = level.GetList("listTest (compound)");
      Assert.IsNotNull(listTest_compound);
      Assert.AreEqual("listTest (compound)", listTest_compound.Name);
      Assert.IsNotNull(listTest_compound.Value);
      Assert.AreEqual(2, listTest_compound.Value.Count);
      TagCompound listTest_compound_tag0 = listTest_compound.Value[0] as TagCompound;
      Assert.IsNotNull(listTest_compound_tag0);
      TagString listTest_compound_tag0_name = listTest_compound_tag0.GetString("name");
      Assert.IsNotNull(listTest_compound_tag0_name);
      Assert.AreEqual("name", listTest_compound_tag0_name.Name);
      Assert.AreEqual("Compound tag #0", listTest_compound_tag0_name.Value);
      TagLong listTest_compound_tag0_createdOn = listTest_compound_tag0.GetLong("created-on");
      Assert.IsNotNull(listTest_compound_tag0_createdOn);
      Assert.AreEqual("created-on", listTest_compound_tag0_createdOn.Name);
      Assert.AreEqual(1264099775885, listTest_compound_tag0_createdOn.Value);

      TagCompound listTest_compound_tag1 = listTest_compound.Value[1] as TagCompound;
      Assert.IsNotNull(listTest_compound_tag1);
      TagString listTest_compound_tag1_name = listTest_compound_tag1.GetString("name");
      Assert.IsNotNull(listTest_compound_tag1_name);
      Assert.AreEqual("name", listTest_compound_tag1_name.Name);
      Assert.AreEqual("Compound tag #1", listTest_compound_tag1_name.Value);
      TagLong listTest_compound_tag1_createdOn = listTest_compound_tag1.GetLong("created-on");
      Assert.IsNotNull(listTest_compound_tag1_createdOn);
      Assert.AreEqual("created-on", listTest_compound_tag1_createdOn.Name);
      Assert.AreEqual(1264099775885, listTest_compound_tag1_createdOn.Value);

      TagByteArray byteArrayTest = level.GetByteArray("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))");
      Assert.IsNotNull(byteArrayTest);
      Assert.AreEqual("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", byteArrayTest.Name);
      Assert.IsNotNull(byteArrayTest.Value);
      Assert.AreEqual(1000, byteArrayTest.Value.Length);
    }

    [Test]
    public void TestLoadSimpleNbt()
    {
      Tag tag;

      tag = this.GetSimpleData();

      Assert.IsNotNull(tag);
      Assert.IsInstanceOf<TagCompound>(tag);
      TagCompound root = tag as TagCompound;
      Assert.AreEqual("hello world", root.Name);

      TagString tagStr = root.GetString("name");
      Assert.AreEqual("name", tagStr.Name);

      Assert.AreEqual("Bananrama", tagStr.Value);
    }

    [Test]
    public void TestReadNonExistantTagFromCompound()
    {
      TagCompound newTag = new TagCompound();

      Tag aTag = newTag.GetTag("nope");

      Assert.IsNull(aTag);

      TagCompound fileTag = this.GetSimpleData();

      aTag = fileTag.GetTag("Entities");

      Assert.IsNull(aTag);
    }

    [Test]
    public void ToValueStringTest()
    {
      // arrange
      Tag target1;
      Tag target2;
      string expected1;
      string actual1;
      string actual2;

      target1 = this.CreateComplexData().Query(@"nested compound test\ham\name");
      target2 = new TagString(null, null);
      expected1 = "Hampus";

      // act
      actual1 = target1.ToValueString();
      actual2 = target2.ToValueString();

      // assert
      Assert.AreEqual(expected1, actual1);
      Assert.IsEmpty(actual2);
    }

    [Test]
    public void ValueChangedEventTest()
    {
      // arrange
      TagString target;
      bool eventRaised;

      eventRaised = false;
      target = new TagString();
      target.ValueChanged += delegate { eventRaised = true; };

      // act
      target.Value = "newvalue";

      // assert
      Assert.IsTrue(eventRaised);
    }

    #endregion

    #region Test Helpers

    /// <summary>
    ///   Decompresses the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns></returns>
    private byte[] Decompress(Stream stream)
    {
      byte[] result;

      using (GZipStream decompressStream = new GZipStream(stream, CompressionMode.Decompress))
      {
        int bufferSize;
        byte[] buffer;

        bufferSize = 4096;
        buffer = new byte[bufferSize];

        using (MemoryStream memory = new MemoryStream())
        {
          int count;
          do
          {
            count = decompressStream.Read(buffer, 0, bufferSize);
            if (count > 0)
            {
              memory.Write(buffer, 0, count);
            }
          } while (count > 0);

          result = memory.ToArray();
        }
      }

      return result;
    }

    #endregion
  }
}
