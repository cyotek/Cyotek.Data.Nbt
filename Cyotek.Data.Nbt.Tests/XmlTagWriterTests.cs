using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagWriterTests : TestBase
  {
    #region Tests

    [Test]
    public void SaveTest()
    {
      // arrange
      XmlTagWriter writer;
      TagCompound target;

      target = this.GetComplexData();
      writer = new XmlTagWriter();

      // act
      writer.Write(target, this.OutputFileName);

      // assert
      FileAssert.AreEqual(this.ComplexXmlDataFileName, this.OutputFileName);
    }

    #endregion
  }
}
