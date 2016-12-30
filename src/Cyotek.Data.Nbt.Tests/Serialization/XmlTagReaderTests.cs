using System.IO;
using System.Text;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public partial class XmlTagReaderTests : TestBase
  {
    #region  Tests

    [Test]
    public void Close_should_close_reader()
    {
      // arrange
      TagReader target;
      XmlReader reader;
      MemoryStream stream;
      ReadState expected;

      stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<Level type=""Int"" />
"));
      reader = XmlReader.Create(stream);

      target = new XmlTagReader(reader);

      expected = ReadState.Closed;

      // act
      target.Close();

      // assert
      Assert.AreEqual(expected, reader.ReadState);
    }

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

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "Missing limitType attribute, unable to determine list contents type.")]
    public void ReadList_throws_exception_if_list_type_not_set()
    {
      // arrange
      TagReader target;
      XmlReader reader;
      MemoryStream stream;

      stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<Level type=""Compound"">
   <tag name=""listTest (long)"" type=""List"">
    <tag>11</tag>
    <tag>12</tag>
    <tag>13</tag>
    <tag>14</tag>
    <tag>15</tag>
  </tag>
</Level>"));
      reader = XmlReader.Create(stream);
      target = new XmlTagReader(reader);

      // act
      target.ReadDocument();
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "Missing type attribute, unable to determine tag type.")]
    public void ReadTagType_throws_exception_if_list_type_not_set()
    {
      // arrange
      TagReader target;
      XmlReader reader;
      MemoryStream stream;

      stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<Level type=""Compound"">
   <tag name=""listTest (long)"">
    <tag>11</tag>
    <tag>12</tag>
    <tag>13</tag>
    <tag>14</tag>
    <tag>15</tag>
  </tag>
</Level>"));
      reader = XmlReader.Create(stream);
      target = new XmlTagReader(reader);

      // act
      target.ReadDocument();
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "Unrecognized or unsupported tag type 'NOTATAG'.")]
    public void ReadTagType_throws_exception_tag_type_is_unknown()
    {
      // arrange
      TagReader target;
      XmlReader reader;
      MemoryStream stream;

      stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<Level type=""Compound"">
   <tag name=""listTest (long)"" type=""NOTATAG"">
    <tag>11</tag>
    <tag>12</tag>
    <tag>13</tag>
    <tag>14</tag>
    <tag>15</tag>
  </tag>
</Level>"));
      reader = XmlReader.Create(stream);
      target = new XmlTagReader(reader);

      // act
      target.ReadDocument();
    }

    #endregion

    #region Test Helpers

    private TagReader CreateReader(Stream stream)
    {
      return new XmlTagReader(stream);
    }

    private TagWriter CreateWriter(Stream stream)
    {
      return new XmlTagWriter(stream);
    }

    [Test]
    public void ReadDocument_stuck_in_infinite_loop_for_empty_root()
    {
      using (Stream stream = new MemoryStream())
      {
        // arrange
        TagWriter writer;
        TagReader target;
        Tag actual;

        writer = this.CreateWriter(stream);

        writer.WriteStartDocument();
        writer.WriteStartTag(TagType.Compound);
        writer.WriteEndTag();
        writer.WriteEndDocument();

        stream.Position = 0;

        target = this.CreateReader(stream);

        // act
        // if the root element was empty, the statement below
        // would get stuck in an infinite loop, causing the test
        // time out after one minute
        actual = target.ReadDocument();

        // assert
        Assert.IsNotNull(actual);
      }
    }
    #endregion
  }
}
