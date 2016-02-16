using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void LoadTest()
    {
      // arrange
      ITagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();
      target = new XmlTagReader();

      // act
      using (Stream stream = File.OpenRead(this.ComplexXmlDataFileName))
      {
        actual = target.ReadDocument(stream);
      }

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

    #endregion
  }
}
