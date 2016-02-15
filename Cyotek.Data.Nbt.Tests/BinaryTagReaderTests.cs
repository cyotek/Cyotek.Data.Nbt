using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class BinaryTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void ReadDocument_should_handle_deflate_compressed_files()
    {
      // arrange
      ITagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();
      target = new BinaryTagReader();

      // act
      using (Stream stream = File.OpenRead(this.DeflateComplexDataFileName))
      {
        actual = target.ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    [Test]
    public void ReadDocument_should_handle_gzip_compressed_files()
    {
      // arrange
      ITagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();
      target = new BinaryTagReader();

      // act
      using (Stream stream = File.OpenRead(this.ComplexDataFileName))
      {
        actual = target.ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    [Test]
    public void ReadDocument_should_handle_uncompressed_files()
    {
      // arrange
      ITagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();
      target = new BinaryTagReader();

      // act
      using (Stream stream = File.OpenRead(this.UncompressedComplexDataFileName))
      {
        actual = target.ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    #endregion
  }
}
