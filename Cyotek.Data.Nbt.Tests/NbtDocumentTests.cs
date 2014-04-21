using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class NbtDocumentTests : TestBase
  {
    #region Tests

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
      target = new NbtDocument(NbtFormat.Xml);

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
    public void EmptyListXmlTest()
    {
      // arrange
      NbtDocument target;
      NbtDocument reloaded;
      string fileName;

      fileName = this.GetWorkFile();
      target = new NbtDocument(NbtFormat.Xml);
      target.DocumentRoot.Name = "Test";
      target.DocumentRoot.Value.Add("EmptyList", TagType.List, TagType.Compound);

      // act
      try
      {
        target.Save(fileName);
        reloaded = NbtDocument.LoadDocument(fileName);
      }
      finally
      {
        this.DeleteFile(fileName);
      }

      // assert
      // this test is essentially ensuring that an infinite loop when reloading an XML document is no longer present
      this.CompareTags(target.DocumentRoot, reloaded.DocumentRoot);
    }

    [Test]
    public void FormatTest()
    {
      // arrange
      NbtDocument source;
      NbtDocument target1;
      NbtDocument target2;
      string fileName1;
      string fileName2;
      bool file1IsBinary;
      bool file2IsXml;

      fileName1 = this.GetWorkFile();
      fileName2 = this.GetWorkFile();
      source = new NbtDocument(this.CreateComplexData());

      // act
      try
      {
        source.Format = NbtFormat.Binary;
        source.Save(fileName1);
        source.Format = NbtFormat.Xml;
        source.Save(fileName2);

        target1 = NbtDocument.LoadDocument(fileName1);
        target2 = NbtDocument.LoadDocument(fileName2);

        file1IsBinary = (target1.Format == NbtFormat.Binary);
        file2IsXml = (target2.Format == NbtFormat.Xml);
      }
      finally
      {
        this.DeleteFile(fileName1);
        this.DeleteFile(fileName2);
      }

      // assert
      Assert.IsTrue(file1IsBinary);
      Assert.IsTrue(file2IsXml);
      this.CompareTags(target1.DocumentRoot, target2.DocumentRoot);
    }

    [Test]
    public void GetDocumentNameBadFileTest()
    {
      // arrange
      string actual;
      string fileName;

      fileName = this.BadFileName;

      // act
      actual = NbtDocument.GetDocumentName(fileName);

      // assert
      Assert.IsNull(actual);
    }

    [Test]
    public void GetDocumentNameMissingFileTest()
    {
      // arrange
      string actual;
      string fileName;

      fileName = Guid.NewGuid().ToString("N");

      // act
      actual = NbtDocument.GetDocumentName(fileName);

      // assert
      Assert.IsNull(actual);
    }

    [Test]
    public void GetDocumentNameNullArgumentTest()
    {
      // arrange
      string actual;

      // act
      actual = NbtDocument.GetDocumentName(null);

      // assert
      Assert.IsNull(actual);
    }

    [Test]
    public void GetDocumentNameTest()
    {
      // arrange
      string actual;
      string expected;
      string fileName;

      expected = "hello world";
      fileName = this.SimpleDataFileName;

      // act
      actual = NbtDocument.GetDocumentName(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatBinaryTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.UncompressedComplexDataFileName;
      expected = NbtFormat.Binary;
      target = new NbtDocument();

      // act
      actual = target.GetFormat(fileName);

      // assert
      actual.Should().Be(expected);
    }

    [Test]
    public void GetFormatDeflateBinaryTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.DeflateComplexDataFileName;
      expected = NbtFormat.Binary;
      target = new NbtDocument();

      // act
      actual = target.GetFormat(fileName);

      // assert
      actual.Should().Be(expected);
    }

    [Test]
    public void GetFormatGzipBinaryTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.ComplexDataFileName;
      expected = NbtFormat.Binary;
      target = new NbtDocument();

      // act
      actual = target.GetFormat(fileName);

      // assert
      actual.Should().Be(expected);
    }

    [Test]
    public void GetFormatInvalidTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.BadFileName;
      expected = NbtFormat.Custom;
      target = new NbtDocument();

      // act
      actual = target.GetFormat(fileName);

      // assert
      actual.Should().Be(expected);
    }

    [Test]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetFormatMissingFileTest()
    {
      // arrange
      NbtDocument target;
      string fileName;

      target = new NbtDocument();
      fileName = Guid.NewGuid().ToString();

      // act
      target.GetFormat(fileName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetFormatNullTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act
      target.GetFormat(null);

      // assert
    }

    [Test]
    public void GetFormatXmlTest()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.ComplexXmlDataFileName;
      expected = NbtFormat.Xml;
      target = new NbtDocument();

      // act
      actual = target.GetFormat(fileName);

      // assert
      actual.Should().Be(expected);
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
    public void IsNbtDocumentBinaryTest()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.UncompressedComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeTrue();
    }

    [Test]
    public void IsNbtDocumentDeflateBinaryTest()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.DeflateComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeTrue();
    }

    [Test]
    public void IsNbtDocumentGzipBinaryTest()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.ComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeTrue();
    }

    [Test]
    public void IsNbtDocumentInvalidTest()
    {
      // arrange
      bool actual;
      string fileName;

      fileName = this.BadFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeFalse();
    }

    [Test]
    public void IsNbtDocumentMissingFileTest()
    {
      // arrange
      bool actual;
      string fileName;

      fileName = Guid.NewGuid().ToString();

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeFalse();
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IsNbtDocumentNullTest()
    {
      // arrange

      // act
      NbtDocument.IsNbtDocument(null);

      // assert
    }

    [Test]
    public void IsNbtDocumentXmlTest()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.ComplexXmlDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      actual.Should().BeTrue();
    }

    [Test]
    public void LoadTest()
    {
      // arrange
      NbtDocument target1;
      NbtDocument target2;
      string fileName;

      fileName = this.ComplexDataFileName;
      target1 = new NbtDocument(this.CreateComplexData());
      target2 = new NbtDocument();
      target2.FileName = fileName;

      // act
      target2.Load();

      // assert
      this.CompareTags(target1.DocumentRoot, target2.DocumentRoot);
    }

    [Test]
    public void LoadWithFileTest()
    {
      // arrange
      NbtDocument target1;
      NbtDocument target2;
      string fileName;

      fileName = this.ComplexDataFileName;
      target1 = new NbtDocument(this.CreateComplexData());
      target2 = new NbtDocument();

      // act
      target2.Load(fileName);

      // assert
      this.CompareTags(target1.DocumentRoot, target2.DocumentRoot);
    }

    [Test]
    public void SaveTest()
    {
      // arrange
      NbtDocument target1;
      NbtDocument target2;
      string fileName;

      fileName = this.GetWorkFile();
      target1 = new NbtDocument(this.CreateComplexData());
      target1.FileName = fileName;

      // act
      try
      {
        target1.Save();
        target2 = NbtDocument.LoadDocument(fileName);
      }
      finally
      {
        this.DeleteFile(fileName);
      }

      // assert
      this.CompareTags(target1.DocumentRoot, target2.DocumentRoot);
    }

    [Test]
    public void SaveWithFileTest()
    {
      // arrange
      NbtDocument target1;
      NbtDocument target2;
      string fileName;

      fileName = this.GetWorkFile();
      target1 = new NbtDocument(this.CreateComplexData());

      // act
      try
      {
        target1.Save(fileName);
        target2 = NbtDocument.LoadDocument(fileName);
      }
      finally
      {
        this.DeleteFile(fileName);
      }

      // assert
      this.CompareTags(target1.DocumentRoot, target2.DocumentRoot);
    }

    [Test]
    public void SetBinaryFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument(NbtFormat.Xml);

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
      target.Format = NbtFormat.Xml;

      // assert
      Assert.AreEqual(typeof(XmlTagReader), target.ReaderType);
      Assert.AreEqual(typeof(XmlTagWriter), target.WriterType);
    }

    #endregion
  }
}
