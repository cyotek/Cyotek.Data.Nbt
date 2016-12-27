using System.IO;
using System.Text;
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
      Tag expected;
      Tag actual;
      XmlReader reader;

      expected = this.CreateComplexData();

      reader = XmlReader.Create(this.ComplexXmlDataFileName);

      target = new XmlTagReader(reader);

      // act
      actual = target.ReadTag();

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void IsNbtDocument_returns_false_for_non_compound_type()
    {
      // arrange
      XmlTagReader target;
      MemoryStream stream;
      bool actual;

      stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<Level type=""Int"" />
"));

      target = new XmlTagReader(stream);

      // act
      actual = target.IsNbtDocument();

      // assert
      Assert.IsFalse(actual);
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
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadDocument_can_handle_xml_documents_with_self_closing_tags()
    {
      // arrange
      XmlTagReader target;
      Tag actual;
      Tag expected;
      Stream stream;

      expected = this.CreateSimpleNesting();
      stream = File.OpenRead(Path.Combine(this.DataPath, "project.xml"));
      target = new XmlTagReader(stream);

      // act
      actual = target.ReadDocument();

      // assert
      NbtAssert.AreEqual(expected, actual);
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
      NbtAssert.AreEqual(expected, actual);
    }

    #endregion
  }
}
