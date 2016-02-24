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
    public void WriteDocument_should_accept_auto_compression()
    {
      this.WriteDocumentTest<BinaryTagWriter, BinaryTagReader>(CompressionOption.Auto);
    }

    [Test]
    public void WriteDocument_should_accept_forced_compression()
    {
      this.WriteDocumentTest<BinaryTagWriter, BinaryTagReader>(CompressionOption.On);
    }

    [Test]
    public void WriteDocument_should_accept_no_compression()
    {
      this.WriteDocumentTest<BinaryTagWriter, BinaryTagReader>(CompressionOption.Off);
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
