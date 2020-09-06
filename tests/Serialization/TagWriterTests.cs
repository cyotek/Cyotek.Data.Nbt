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
    public void CreateWriter_throws_exception_for_null_stream()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => TagWriter.CreateWriter(NbtFormat.Xml, null));
    }

    [Test]
    public void CreateWriter_throws_exception_for_unknown_type()
    {
      // act & assert
      Assert.Throws<ArgumentOutOfRangeException>(() => TagWriter.CreateWriter(NbtFormat.Unknown, new MemoryStream()));
    }

    #endregion
  }
}
