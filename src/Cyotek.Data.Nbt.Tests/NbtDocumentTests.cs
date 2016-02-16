using System;
using System.IO;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class NbtDocumentTests : TestBase
  {
    #region  Tests

    [Test]
    public void Constructor_should_have_default_format()
    {
      // arrange
      NbtDocument target;
      NbtFormat expected;

      expected = NbtFormat.Binary;

      // act
      target = new NbtDocument();

      // assert
      Assert.AreEqual(expected, target.Format);
    }

    [Test]
    public void Constructor_should_have_have_empty_root()
    {
      // arrange
      NbtDocument target;

      // act
      target = new NbtDocument();

      // assert
      Assert.IsNotNull(target.DocumentRoot);
    }

    [Test]
    public void EmptyListXmlTest()
    {
      // arrange
      NbtDocument target;
      NbtDocument reloaded;
      string fileName;

      fileName = this.GetWorkFile();
      target = new NbtDocument
               {
                 Format = NbtFormat.Xml
               };
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
    public void Get_document_name_should_return_name_from_deflate_binary_file()
    {
      // arrange
      string expected;
      string actual;

      expected = "Level";

      // act
      actual = NbtDocument.GetDocumentName(this.DeflateComplexDataFileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Get_document_name_should_return_name_from_gzip_binary_file()
    {
      // arrange
      string expected;
      string actual;

      expected = "Level";

      // act
      actual = NbtDocument.GetDocumentName(this.ComplexDataFileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Get_document_name_should_return_name_from_uncompressed_binary_file()
    {
      // arrange
      string expected;
      string actual;

      expected = "Level";

      // act
      actual = NbtDocument.GetDocumentName(this.UncompressedComplexDataFileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Get_document_name_should_return_name_from_xml_file()
    {
      // arrange
      string expected;
      string actual;

      expected = "Level";

      // act
      actual = NbtDocument.GetDocumentName(this.ComplexXmlDataFileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetDocumentFormat_should_throw_exception_if_file_not_found()
    {
      // arrange
      string fileName;

      fileName = Guid.NewGuid().
                      ToString();

      // act
      NbtDocument.GetDocumentFormat(fileName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentFormat_should_throw_exception_with_null_filename()
    {
      // arrange

      // act
      NbtDocument.GetDocumentFormat((string)null);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentFormat_should_throw_exception_with_null_stream()
    {
      // arrange

      // act
      NbtDocument.GetDocumentFormat((Stream)null);

      // assert
    }

    [Test]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetDocumentName_should_throw_exception_if_file_not_found()
    {
      // arrange
      string fileName;

      fileName = Guid.NewGuid().
                      ToString("N");

      // act
      NbtDocument.GetDocumentName(fileName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentName_should_throw_exception_if_filename_is_empty()
    {
      // arrange

      // act
      NbtDocument.GetDocumentName(string.Empty);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetDocumentName_should_throw_exception_if_filename_is_null()
    {
      // arrange

      // act
      NbtDocument.GetDocumentName(null);

      // assert
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
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.UncompressedComplexDataFileName;
      expected = NbtFormat.Binary;

      // act
      actual = NbtDocument.GetDocumentFormat(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatDeflateBinaryTest()
    {
      // arrange
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.DeflateComplexDataFileName;
      expected = NbtFormat.Binary;

      // act
      actual = NbtDocument.GetDocumentFormat(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatGzipBinaryTest()
    {
      // arrange
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.ComplexDataFileName;
      expected = NbtFormat.Binary;

      // act
      actual = NbtDocument.GetDocumentFormat(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatInvalidTest()
    {
      // arrange
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.BadFileName;
      expected = NbtFormat.Unknown;

      // act
      actual = NbtDocument.GetDocumentFormat(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFormatXmlTest()
    {
      // arrange
      NbtFormat expected;
      NbtFormat actual;
      string fileName;

      fileName = this.ComplexXmlDataFileName;
      expected = NbtFormat.Xml;

      // act
      actual = NbtDocument.GetDocumentFormat(fileName);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
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
    public void IsNbtDocument_should_return_false_for_unknown_file()
    {
      // arrange
      bool actual;
      string fileName;

      fileName = this.BadFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void IsNbtDocument_should_return_true_for_deflate_binary_file()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.DeflateComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void IsNbtDocument_should_return_true_for_gzip_binary_file()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.ComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void IsNbtDocument_should_return_true_for_gzip_binary_stream()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.ComplexDataFileName;

      // act
      using (Stream stream = File.OpenRead(fileName))
      {
        actual = NbtDocument.IsNbtDocument(stream);
      }

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void IsNbtDocument_should_return_true_for_uncompressed_binary_file()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.UncompressedComplexDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void IsNbtDocument_should_return_true_for_xml_file()
    {
      // arrange
      string fileName;
      bool actual;

      fileName = this.ComplexXmlDataFileName;

      // act
      actual = NbtDocument.IsNbtDocument(fileName);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IsNbtDocument_should_throw_exception_for_null_filename()
    {
      // arrange

      // act
      NbtDocument.IsNbtDocument((string)null);

      // assert
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IsNbtDocument_should_throw_exception_for_null_stream()
    {
      // arrange

      // act
      NbtDocument.IsNbtDocument((string)null);

      // assert
    }

    [Test]
    [ExpectedException(typeof(FileNotFoundException))]
    public void IsNbtDocument_should_throw_exception_if_file_is_missing()
    {
      // arrange
      string fileName;

      fileName = Guid.NewGuid().
                      ToString();

      // act
      NbtDocument.IsNbtDocument(fileName);

      // assert
    }

    [Test]
    [ExpectedException(typeof(InvalidDataException))]
    public void Load_should_throw_exception_for_invalid_file()
    {
      // arrange
      NbtDocument target;
      string fileName;

      fileName = this.BadFileName;

      target = new NbtDocument();

      // act
      target.Load(fileName);
    }

    [Test]
    [ExpectedException(typeof(FileNotFoundException))]
    public void Load_should_throw_exception_for_missing_file()
    {
      // arrange
      NbtDocument target;
      string fileName;

      fileName = Guid.NewGuid().
                      ToString("N");

      target = new NbtDocument();

      // act
      target.Load(fileName);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Load_should_throw_exception_for_null_filename()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act
      target.Load((string)null);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Load_should_throw_exception_for_null_stream()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act
      target.Load((Stream)null);
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

    #endregion
  }
}
