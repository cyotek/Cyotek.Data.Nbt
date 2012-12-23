using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class BinaryTagWriterTests
    : TestBase
  {
    #region  Public Methods

    [Test]
    public void SaveCompressedTest()
    {
      // arrange
      BinaryTagWriter writer;
      TagCompound tag;

      tag = this.GetComplexData();
      writer = new BinaryTagWriter();

      // act
      writer.Write(tag, this.OutputFileName, NbtOptions.Compress | NbtOptions.Header);

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
      writer.Write(tag, this.OutputFileName, NbtOptions.Header);

      // assert
      this.CompareTags(tag, new NbtDocument(this.OutputFileName).DocumentRoot);
      FileAssert.AreEqual(this.UncompressedComplexDataFileName, this.OutputFileName);
    }

    #endregion  Public Methods
  }
}