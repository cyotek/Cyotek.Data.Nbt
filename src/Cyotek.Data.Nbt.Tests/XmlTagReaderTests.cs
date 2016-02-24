using System.IO;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void Constructor_allows_external_reader()
    {
      // arrange
      ITagReader target;
      ITag expected;
      ITag actual;
      XmlReader reader;

      expected = this.CreateComplexData();

      reader = XmlReader.Create(this.ComplexXmlDataFileName);

      target = new XmlTagReader(reader);

      // act
      actual = target.ReadTag();

      // assert
      this.CompareTags(expected, actual);
    }

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
    public void ReadDocument_can_handle_xml_documents_with_self_closing_tags()
    {
      // arrange
      XmlTagReader target;
      ITag actual;
      ITag expected;

      target = new XmlTagReader();

      expected = this.CreateSimpleNesting();

      // act
      using (Stream stream = File.OpenRead(Path.Combine(this.DataPath, "project.xml")))
      {
        actual = target.ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    [Test]
    public void ReadDocument_can_handle_xml_documents_without_whitespace()
    {
      // arrange
      ITagReader target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();
      target = new XmlTagReader();

      // act
      using (Stream stream = File.OpenRead(this.ComplexXmlWithoutWhitespaceDataFileName))
      {
        actual = target.ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    #endregion
  }
}
