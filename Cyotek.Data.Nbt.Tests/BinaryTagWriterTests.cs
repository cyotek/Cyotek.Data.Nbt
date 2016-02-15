using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class BinaryTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void SaveCompressedTest()
    {
      // arrange
      ITagWriter writer;
      TagCompound tag;

      tag = this.CreateComplexData();
      writer = new BinaryTagWriter();

      // act
      using (Stream stream = File.Create(this.OutputFileName))
      {
        writer.WriteDocument(stream, tag, CompressionOption.On);
      }

      // assert
      this.CompareTags(tag, NbtDocument.LoadFromFile(this.OutputFileName).
                                        DocumentRoot);
    }

    [Test]
    public void SaveUncompressedTest()
    {
      // arrange
      ITagWriter writer;
      TagCompound tag;

      tag = this.CreateComplexData();
      writer = new BinaryTagWriter();

      // act
      using (Stream stream = File.Create(this.OutputFileName))
      {
        writer.WriteDocument(stream, tag, CompressionOption.Off);
      }

      // assert
      this.CompareTags(tag, NbtDocument.LoadFromFile(this.OutputFileName).
                                        DocumentRoot);
      FileAssert.AreEqual(this.UncompressedComplexDataFileName, this.OutputFileName);
    }

    [Test]
    public void WriteEmptyByteArrayTest()
    {
      // arrange
      ITagWriter target;
      NbtDocument expected;
      MemoryStream stream;
      ITagReader reader;

      expected = new NbtDocument();
      expected.DocumentRoot.Name = "WriteEmptyByteArrayTest";
      expected.DocumentRoot.Value.Add("ByteArray", new byte[0]);
      expected.DocumentRoot.Value.Add("Byte", 255);

      stream = new MemoryStream();

      target = new BinaryTagWriter(stream);

      // act
      target.WriteTag(expected.DocumentRoot, WriteTagOptions.None);

      // assert
      stream.Seek(0, SeekOrigin.Begin);
      reader = new BinaryTagReader(stream);
      this.CompareTags(expected.DocumentRoot, reader.ReadTag());
    }

    #endregion
  }
}
