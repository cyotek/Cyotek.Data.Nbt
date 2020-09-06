using System;
using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public partial class BinaryTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void IsNbtDocument_handles_none_seekable_streams()
    {
      // arrange
      TagReader target;
      bool actual;

      target = this.CreateReader(new NoSeekStream(File.ReadAllBytes(this.SimpleDataFileName)));

      // act
      actual = target.IsNbtDocument();

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void ReadDocument_should_handle_deflate_compressed_files()
    {
      // arrange
      TagReader target;
      TagCompound expected;
      TagCompound actual;
      Stream stream;

      expected = this.CreateComplexData();
      stream = File.OpenRead(this.DeflateComplexDataFileName);
      target = new BinaryTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadDocument_should_handle_gzip_compressed_files()
    {
      // arrange
      TagReader target;
      TagCompound expected;
      TagCompound actual;
      Stream stream;

      expected = this.CreateComplexData();
      stream = File.OpenRead(this.ComplexDataFileName);
      target = new BinaryTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadDocument_should_handle_uncompressed_files()
    {
      // arrange
      TagReader target;
      TagCompound expected;
      TagCompound actual;
      Stream stream;

      expected = this.CreateComplexData();
      stream = File.OpenRead(this.UncompressedComplexDataFileName);
      target = new BinaryTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "Unexpected list type '182' found.")]
    public void ReadList_throws_exception_if_list_type_is_invalid_and_items_are_present()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        writer.WriteStartDocument();
        writer.WriteStartTag("list", TagType.List, (TagType)182, 1);
        writer.WriteStartTag((TagType)182);
        writer.WriteEndTag();
        writer.WriteEndTag();
        writer.WriteEndDocument();

        stream.Position = 0;

        reader.ReadTagType();
        reader.ReadTagName();

        // act
        reader.ReadList();
      }
    }

    [Test]
    public void ReadList_ignores_invalid_list_type_for_empty_list()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;
        TagCollection actual;
        TagType expected;

        expected = (TagType)182;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        writer.WriteStartDocument();
        writer.WriteStartTag("list", TagType.List, expected, 0);
        writer.WriteEndTag();
        writer.WriteEndDocument();

        stream.Position = 0;

        reader.ReadTagType();
        reader.ReadTagName();

        // act
        actual = reader.ReadList();

        // assert
        Assert.AreEqual(expected, actual.LimitType);
      }
    }

    #endregion

    #region Test Helpers

    private TagReader CreateReader(Stream stream)
    {
      return new BinaryTagReader(stream, false);
    }

    private TagWriter CreateWriter(Stream stream)
    {
      return new BinaryTagWriter(stream);
    }

    private void WriteValue(Stream stream, int value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.IntSize);
      }

      stream.Write(buffer, 0, BitHelper.IntSize);
    }

    private void WriteValue(Stream stream, short value)
    {
      byte[] buffer;

      buffer = BitConverter.GetBytes(value);

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, BitHelper.ShortSize);
      }

      stream.Write(buffer, 0, BitHelper.ShortSize);
    }

    #endregion

  }
}
