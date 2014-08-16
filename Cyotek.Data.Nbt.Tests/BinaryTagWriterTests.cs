using System.IO;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class BinaryTagWriterTests : TestBase
  {
    #region Tests

    [Test]
    public void SaveCompressedTest()
    {
      // arrange
      BinaryTagWriter writer;
      TagCompound tag;

      tag = this.GetComplexData();
      writer = new BinaryTagWriter();

      // act
      writer.Write(tag, this.OutputFileName, NbtOptions.Compress | NbtOptions.ReadHeader);

      // assert
      this.CompareTags(tag, new NbtDocument(this.OutputFileName).DocumentRoot);
    }

    [Test]
    public void SaveUncompressedTest()
    {
      // arrange
      BinaryTagWriter writer;
      TagCompound tag;

      tag = this.GetComplexData();
      writer = new BinaryTagWriter();

      // act
      writer.Write(tag, this.OutputFileName, NbtOptions.ReadHeader);

      // assert
      this.CompareTags(tag, new NbtDocument(this.OutputFileName).DocumentRoot);
      FileAssert.AreEqual(this.UncompressedComplexDataFileName, this.OutputFileName);
    }

    [Test]
    public void WriteEmptyByteArrayTest()
    {
      // arrange
      BinaryTagWriter target;
      NbtDocument expected;
      MemoryStream stream;
      BinaryTagReader reader;

      expected = new NbtDocument();
      expected.DocumentRoot.Name = "WriteEmptyByteArrayTest";
      expected.DocumentRoot.Value.Add("ByteArray", new byte[0]);
      expected.DocumentRoot.Value.Add("Byte", 255);

      stream = new MemoryStream();

      target = new BinaryTagWriter(stream);

      // act
      target.Write(expected.DocumentRoot);

      // assert
      stream.Seek(0, SeekOrigin.Begin);
      reader = new BinaryTagReader(stream, NbtOptions.ReadHeader | NbtOptions.Compress);
      this.CompareTags(expected.DocumentRoot, reader.Read());
    }

    #endregion
  }
}
