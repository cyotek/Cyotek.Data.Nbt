using System.IO;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public class XmlTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void Constructor_allows_external_reader()
    {
      // arrange
      TagReader target;
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
      TagReader target;
      TagCompound expected;
      TagCompound actual;
      Stream stream;

      expected = this.CreateComplexData();
      stream = File.OpenRead(this.ComplexXmlDataFileName);
      target = new XmlTagReader(stream);

      // act
      actual = target.ReadDocument();

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
      Stream stream;

      expected = this.CreateSimpleNesting();
      stream = File.OpenRead(Path.Combine(this.DataPath, "project.xml"));
      target = new XmlTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      this.CompareTags(expected, actual);
    }

    [Test]
    public void ReadDocument_can_handle_xml_documents_without_whitespace()
    {
      // arrange
      TagReader target;
      TagCompound expected;
      TagCompound actual;
      Stream stream;

      expected = this.CreateComplexData();
      stream = File.OpenRead(this.ComplexXmlWithoutWhitespaceDataFileName);
      target = new XmlTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      this.CompareTags(expected, actual);
    }

    #endregion
  }
}
