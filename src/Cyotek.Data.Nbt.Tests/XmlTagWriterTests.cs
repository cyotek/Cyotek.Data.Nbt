using System;
using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    [ExpectedException(typeof(NotSupportedException))]
    public void WriteDocument_should_throw_exception_if_compression_is_enabled()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.On);
    }

    [Test]
    public void WriteDocument_should_accept_no_compression()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.Off);
    }

    [Test]
    public void WriteDocument_should_accept_auto_compression()
    {
      this.WriteDocumentTest<XmlTagWriter, XmlTagReader>(CompressionOption.Auto);
    }

    #endregion
  }
}
