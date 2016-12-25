using System.IO;
using System.Xml;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  partial class XmlTagWriterTests
  {
    #region  Tests

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
      this.CompareTags(expected, actual);
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
