using System;
using System.IO;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void Constructor_allows_external_writer()
    {
      // arrange
      TagWriter target;
      ITag expected;
      ITag actual;
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
      target.WriteTag(expected);
      writer.Flush();

      using (TextReader textReader = new StringReader(textWriter.ToString()))
      {
        using (XmlReader reader = XmlReader.Create(textReader))
        {
          actual = new XmlTagReader(reader).ReadTag();
        }
      }

      // assert
      this.CompareTags(expected, actual);
    }

    [Test]
    public void WriteDocument_should_accept_auto_compression()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.Auto);
    }

    [Test]
    public void WriteDocument_should_accept_no_compression()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.Off);
    }

    [Test]
    [ExpectedException(typeof(NotSupportedException))]
    public void WriteDocument_should_throw_exception_if_compression_is_enabled()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.On);
    }

    #endregion
  }
}
