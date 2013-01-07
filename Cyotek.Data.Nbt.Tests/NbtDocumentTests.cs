using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class NbtDocumentTests : TestBase
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
      expected4 = NbtFormat.Xml;
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

    [Test, ExpectedException(typeof(ArgumentException))]
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
  }
}
