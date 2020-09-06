using System.IO;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public partial class XmlTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void Close_should_close_writer()
    {
      // arrange
      TagWriter target;
      XmlWriter writer;
      MemoryStream stream;
      WriteState expected;

      stream = new MemoryStream();
      writer = XmlWriter.Create(stream);

      target = new XmlTagWriter(writer);

      expected = WriteState.Closed;

      // act
      target.Close();

      // assert
      Assert.AreEqual(expected, writer.WriteState);
    }

    [Test]
    public void Constructor_allows_external_writer()
    {
      // arrange
      TagWriter target;
      Tag expected;
      Tag actual;
      XmlWriter writer;
      TextWriter textWriter;

      expected = this.CreateComplexData();

      textWriter = new StringWriter();
      writer = XmlWriter.Create(textWriter, new XmlWriterSettings
                                            {
                                              Indent = true
                                            });

      target = new XmlTagWriter(writer);

      // act
      target.WriteStartDocument();
      target.WriteTag(expected);
      target.WriteEndDocument();

      using (TextReader textReader = new StringReader(textWriter.ToString()))
      {
        using (XmlReader reader = XmlReader.Create(textReader))
        {
          actual = new XmlTagReader(reader).ReadTag();
        }
      }

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void WriteValue_should_use_cdata_if_required()
    {
      // arrange
      XmlTagWriter target;
      TextWriter writer;
      XmlWriter xmlWriter;
      string actual;
      string expected;

      writer = new StringWriter();
      xmlWriter = XmlWriter.Create(writer);

      expected = "<?xml version=\"1.0\" encoding=\"utf-16\" standalone=\"yes\"?><tag type=\"Compound\"><alpha type=\"String\"><![CDATA[<BAD>]]></alpha></tag>";

      target = new XmlTagWriter(xmlWriter);
      target.WriteStartDocument();
      target.WriteStartTag(TagType.Compound);

      // act
      target.WriteTag("alpha", "<BAD>");

      // assert
      target.WriteEndTag();
      target.WriteEndDocument();
      actual = writer.ToString();
      Assert.AreEqual(expected, actual);
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

    #endregion
  }
}
