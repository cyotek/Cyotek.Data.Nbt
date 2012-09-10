using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class NbtDocumentTests
    : TestBase
  {
    [Test]
    public void ConstructorTest()
    {
      // arrange
      NbtDocument target;

      // act
      target = new NbtDocument();

      // assert
      Assert.IsNotNull(target.DocumentRoot);
      Assert.IsNotNull(target.WriterType);
      Assert.IsNotNull(target.ReaderType);
    }

    [Test]
    public void ConstructorWithBinaryFormatTest()
    {
      // arrange
      NbtDocument target;

      // act
      target = new NbtDocument(NbtFormat.Binary);

      // assert
      Assert.AreEqual(typeof(BinaryTagReader), target.ReaderType);
      Assert.AreEqual(typeof(BinaryTagWriter), target.WriterType);
    }

    [Test]
    public void ConstructorWithCustomFormatTest()
    {
      // arrange
      NbtDocument target;

      // act
      target = new NbtDocument(NbtFormat.Custom);

      // assert
      Assert.IsNull(target.ReaderType);
      Assert.IsNull(target.WriterType);
    }

    [Test]
    public void ConstructorWithXmlFormatTest()
    {
      // arrange
      NbtDocument target;

      // act
      target = new NbtDocument(NbtFormat.XML);

      // assert
      Assert.AreEqual(typeof(XmlTagReader), target.ReaderType);
      Assert.AreEqual(typeof(XmlTagWriter), target.WriterType);
    }

    [Test]
    public void DefaultFormatTest()
    {
      // arrange
      NbtFormat expected;
      NbtFormat actual;

      expected = NbtFormat.Binary;

      // act
      actual = NbtDocument.DefaultFormat;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected1;
      NbtFormat expected2;
      NbtFormat expected3;
      NbtFormat expected4;
      NbtFormat expected5;
      NbtFormat actual1;
      NbtFormat actual2;
      NbtFormat actual3;
      NbtFormat actual4;
      NbtFormat actual5;

      target = new NbtDocument();
      expected1 = NbtFormat.Binary;
      expected2 = NbtFormat.Binary;
      expected3 = NbtFormat.Binary;
      expected4 = NbtFormat.XML;
      expected5 = NbtFormat.Custom;

      // act
      actual1 = target.GetFormat(this.ComplexDataFileName); // gzip compressed binary
      actual2 = target.GetFormat(this.DeflateComplexDataFileName); // deflate compressed binary
      actual3 = target.GetFormat(this.UncompressedComplexDataFileName); // raw binary
      actual4 = target.GetFormat(this.ComplexXmlDataFileName); // xml
      actual5 = target.GetFormat(this.BadFileName); // invalid

      // assert
      Assert.AreEqual(expected1, actual1);
      Assert.AreEqual(expected2, actual2);
      Assert.AreEqual(expected3, actual3);
      Assert.AreEqual(expected4, actual4);
      Assert.AreEqual(expected5, actual5);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void InvalidFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act
      target.Format = (NbtFormat)(-1);

      // assert
    }

    [Test]
    public void SetBinaryFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument(NbtFormat.XML);

      // act
      target.Format = NbtFormat.Binary;

      // assert
      Assert.AreEqual(typeof(BinaryTagReader), target.ReaderType);
      Assert.AreEqual(typeof(BinaryTagWriter), target.WriterType);
    }

    [Test]
    public void SetCustomFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument(NbtFormat.Binary);

      // act
      target.Format = NbtFormat.Custom;

      // assert
      Assert.IsNull(target.ReaderType);
      Assert.IsNull(target.WriterType);
    }

    [Test]
    public void SetXmlFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument(NbtFormat.Binary);

      // act
      target.Format = NbtFormat.XML;

      // assert
      Assert.AreEqual(typeof(XmlTagReader), target.ReaderType);
      Assert.AreEqual(typeof(XmlTagWriter), target.WriterType);
    }
  }
}