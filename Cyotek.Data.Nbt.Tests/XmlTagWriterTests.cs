using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class XmlTagWriterTests : TestBase
  {
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
  }
}
