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

    #endregion
  }
}
