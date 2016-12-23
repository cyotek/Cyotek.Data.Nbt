using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public class BinaryTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void Document_serialization_deserialization_test()
    {
      this.WriteDocumentTest<BinaryTagWriter, BinaryTagReader>(stream => new BinaryTagWriter(stream), stream => new BinaryTagReader(stream));
    }

    [Test]
    public void Serialization_deserialization_test()
    {
      this.WriteTest<BinaryTagWriter, BinaryTagReader>(stream => new BinaryTagWriter(stream), stream => new BinaryTagReader(stream));
    }

    [Test]
    public void WriteEmptyByteArrayTest()
    {
      // arrange
      TagWriter target;
      NbtDocument expected;
      MemoryStream stream;
      TagReader reader;

      expected = new NbtDocument();
      expected.DocumentRoot.Name = "WriteEmptyByteArrayTest";
      expected.DocumentRoot.Value.Add("ByteArray", new byte[0]);
      expected.DocumentRoot.Value.Add("Byte", 255);

      stream = new MemoryStream();

      target = new BinaryTagWriter(stream);

      // act
      target.WriteStartDocument();
      target.WriteTag(expected.DocumentRoot, WriteTagOptions.None);
      target.WriteEndDocument();

      // assert
      stream.Seek(0, SeekOrigin.Begin);
      reader = new BinaryTagReader(stream);
      this.CompareTags(expected.DocumentRoot, reader.ReadTag());
    }

    #endregion
  }
}
