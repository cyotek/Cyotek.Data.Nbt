using System.IO;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagReaderTests : TestBase
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

    [Test]
    public void SelfClosingTagBugTest()
    {
      // arrange
      NbtDocument document;

      // act
      document = NbtDocument.LoadDocument(Path.Combine(this.DataPath, "project.xml"));

      // assert
      Assert.AreEqual(NbtFormat.Xml, document.Format);
    }
  }
}
