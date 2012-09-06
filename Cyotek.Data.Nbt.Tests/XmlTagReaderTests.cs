using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class XmlTagReaderTests
    : TestBase
  {
    [Test]
    public void LoadTest()
    {
      // arrange
      XmlTagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.GetComplexData();
      target = new XmlTagReader();

      // act
      actual = target.Load(this.ComplexXmlDataFileName);

      // assert
      this.CompareTags(expected, actual);
    }
  }
}