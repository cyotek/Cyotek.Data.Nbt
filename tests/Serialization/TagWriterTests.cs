using System;
using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public class TagWriterTests
  {
    #region  Tests

    [Test]
    public void CreateWriter_returns_binary_writer()
    {
      // arrange
      TagWriter target;

      // act
      target = TagWriter.CreateWriter(NbtFormat.Binary, new MemoryStream());

      // assert
      Assert.IsInstanceOf<BinaryTagWriter>(target);
    }

    [Test]
    public void CreateWriter_returns_xml_writer()
    {
      // arrange
      TagWriter target;

      // act
      target = TagWriter.CreateWriter(NbtFormat.Xml, new MemoryStream());

      // assert
      Assert.IsInstanceOf<XmlTagWriter>(target);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: stream")]
    public void CreateWriter_throws_exception_for_null_stream()
    {
      // act
      TagWriter.CreateWriter(NbtFormat.Xml, null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Invalid format.\r\nParameter name: format\r\nActual value was Unknown.")]
    public void CreateWriter_throws_exception_for_unknown_type()
    {
      // act
      TagWriter.CreateWriter(NbtFormat.Unknown, new MemoryStream());
    }

    #endregion
  }
}
