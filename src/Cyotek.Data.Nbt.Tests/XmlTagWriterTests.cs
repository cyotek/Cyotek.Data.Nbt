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
    public void Serialization_deserialization_test()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(stream => new XmlTagWriter(stream));
    }

    #endregion
  }
}
