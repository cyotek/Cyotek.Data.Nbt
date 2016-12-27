using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public class BinaryTagReaderTests : TestBase
  {
    #region  Tests

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

    #endregion
  }
}
