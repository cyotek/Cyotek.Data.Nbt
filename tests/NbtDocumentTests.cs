using System;
using System.IO;
using System.IO.Compression;
using System.Text;
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
    public void Constructor_throws_exception_if_compound_is_null()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => new NbtDocument(null));
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
      NbtAssert.AreEqual(target.DocumentRoot, reloaded.DocumentRoot);
    }

    [Test]
    public void FormatTest()
    {
      // arrange
      NbtDocument source;
      NbtDocument expected;
      NbtDocument target;
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

        expected = NbtDocument.LoadDocument(fileName1);
        target = NbtDocument.LoadDocument(fileName2);

        file1IsBinary = expected.Format == NbtFormat.Binary;
        file2IsXml = target.Format == NbtFormat.Xml;
      }
      finally
      {
        this.DeleteFile(fileName1);
        this.DeleteFile(fileName2);
      }

      // assert
      Assert.IsTrue(file1IsBinary);
      Assert.IsTrue(file2IsXml);
      NbtAssert.AreEqual(expected, target);
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
    public void GetDocumentFormat_should_throw_exception_if_file_not_found()
    {
      // arrange
      string fileName;

      fileName = "9A2763952B7A4627AAE8668F1631628F.nbt";

      // act & assert
      Assert.Throws<FileNotFoundException>(() => NbtDocument.GetDocumentFormat(fileName));
    }

    [Test]
    public void GetDocumentFormat_should_throw_exception_with_null_filename()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.GetDocumentFormat((string)null));
    }

    [Test]
    public void GetDocumentFormat_should_throw_exception_with_null_stream()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.GetDocumentFormat((Stream)null));
    }

    [Test]
    public void GetDocumentFormat_throws_exception_for_non_seekable_streams()
    {
      // arrange
      Stream stream;

      stream = new DeflateStream(Stream.Null, CompressionMode.Decompress);

      // act & assert
      Assert.Throws<ArgumentException>(() => NbtDocument.GetDocumentFormat(stream));
    }

    [Test]
    public void GetDocumentName_should_throw_exception_if_file_not_found()
    {
      // arrange
      string fileName;

      fileName = "9A2763952B7A4627AAE8668F1631628F.nbt";

      // act & assert
      Assert.Throws<FileNotFoundException>(() => NbtDocument.GetDocumentName(fileName));
    }

    [Test]
    public void GetDocumentName_should_throw_exception_if_filename_is_empty()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.GetDocumentName(string.Empty));
    }

    [Test]
    public void GetDocumentName_should_throw_exception_if_filename_is_null()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.GetDocumentName(null));
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
    public void InvalidFormatTest()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentOutOfRangeException>(() => target.Format = (NbtFormat)(-1));
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
    public void IsNbtDocument_should_throw_exception_for_null_filename()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.IsNbtDocument((string)null));
    }

    [Test]
    public void IsNbtDocument_should_throw_exception_for_null_stream()
    {
      // act & assert
      Assert.Throws<ArgumentNullException>(() => NbtDocument.IsNbtDocument((string)null));
    }

    [Test]
    public void IsNbtDocument_should_throw_exception_if_file_is_missing()
    {
      // arrange
      string fileName;

      fileName = "9A2763952B7A4627AAE8668F1631628F.nbt";

      // act & assert
      Assert.Throws<FileNotFoundException>(() => NbtDocument.IsNbtDocument(fileName));

      // assert
    }

    [Test]
    public void Load_should_throw_exception_for_invalid_file()
    {
      // arrange
      NbtDocument target;
      string fileName;

      fileName = this.BadFileName;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<InvalidDataException>(() => target.Load(fileName));
    }

    [Test]
    public void Load_should_throw_exception_for_missing_file()
    {
      // arrange
      NbtDocument target;
      string fileName;

      fileName = "9A2763952B7A4627AAE8668F1631628F.nbt";

      target = new NbtDocument();

      // act & assert
      Assert.Throws<FileNotFoundException>(() => target.Load(fileName));
    }

    [Test]
    public void Load_should_throw_exception_for_null_filename()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.Load((string)null));
    }

    [Test]
    public void Load_should_throw_exception_for_null_stream()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.Load((Stream)null));
    }

    [Test]
    public void Load_updates_filename_property()
    {
      // arrange
      NbtDocument target;
      string expected;
      string actual;

      expected = this.ComplexDataFileName;

      target = new NbtDocument();

      // act
      target.Load(expected);

      // assert
      actual = target.FileName;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void LoadDocument_loads_data_from_stream()
    {
      // arrange
      NbtDocument expected;
      NbtDocument actual;
      string fileName;

      fileName = this.ComplexDataFileName;
      expected = new NbtDocument(this.CreateComplexData());

      // act
      using (Stream stream = File.OpenRead(fileName))
      {
        actual = NbtDocument.LoadDocument(stream);
      }

      // assert
      NbtAssert.AreEqual(expected, actual);
    }

    [Test]
    public void LoadTest()
    {
      // arrange
      NbtDocument expected;
      NbtDocument target;
      string fileName;

      fileName = this.ComplexDataFileName;
      expected = new NbtDocument(this.CreateComplexData());
      target = new NbtDocument();
      target.FileName = fileName;

      // act
      target.Load();

      // assert
      NbtAssert.AreEqual(expected, target);
    }

    [Test]
    public void LoadWithFileTest()
    {
      // arrange
      NbtDocument expected;
      NbtDocument target;
      string fileName;

      fileName = this.ComplexDataFileName;
      expected = new NbtDocument(this.CreateComplexData());
      target = new NbtDocument();

      // act
      target.Load(fileName);

      // assert
      NbtAssert.AreEqual(expected, target);
    }

    [Test]
    public void Query_returns_tag()
    {
      // arrange
      NbtDocument target;
      Tag expected;
      Tag actual;

      target = new NbtDocument(this.CreateComplexData());

      expected = ((TagCompound)((TagList)target.DocumentRoot["listTest (compound)"]).Value[1])["created-on"];

      // act
      actual = target.Query(@"listTest (compound)\1\created-on");

      // assert
      Assert.AreSame(expected, actual);
    }

    [Test]
    public void Query_returns_typed_tag()
    {
      // arrange
      NbtDocument target;
      TagLong expected;
      TagLong actual;

      target = new NbtDocument(this.CreateComplexData());

      expected = (TagLong)((TagCompound)((TagList)target.DocumentRoot["listTest (compound)"]).Value[1])["created-on"];

      // act
      actual = target.Query<TagLong>(@"listTest (compound)\1\created-on");

      // assert
      Assert.AreSame(expected, actual);
    }

    [Test]
    public void Save_throws_exception_if_filename_is_empty()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.Save(string.Empty));
    }

    [Test]
    public void Save_throws_exception_if_filename_is_null()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.Save((string)null));
    }

    [Test]
    public void Save_throws_exception_if_stream_is_null()
    {
      // arrange
      NbtDocument target;

      target = new NbtDocument();

      // act & assert
      Assert.Throws<ArgumentNullException>(() => target.Save((Stream)null));
    }

    [Test]
    public void Save_updates_filename_property()
    {
      // arrange
      NbtDocument target;
      string expected;
      string actual;

      expected = this.GetWorkFile();

      target = new NbtDocument(this.CreateComplexData());

      // act
      try
      {
        target.Save(expected);
      }
      finally
      {
        this.DeleteFile(expected);
      }

      // assert
      actual = target.FileName;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void SaveTest()
    {
      // arrange
      NbtDocument expected;
      NbtDocument target;
      string fileName;

      fileName = this.GetWorkFile();
      expected = new NbtDocument(this.CreateComplexData());
      expected.FileName = fileName;

      // act
      try
      {
        expected.Save();
        target = NbtDocument.LoadDocument(fileName);
      }
      finally
      {
        this.DeleteFile(fileName);
      }

      // assert
      NbtAssert.AreEqual(expected, target);
    }

    [Test]
    public void SaveWithFileTest()
    {
      // arrange
      NbtDocument expected;
      NbtDocument target;
      string fileName;

      fileName = this.GetWorkFile();
      expected = new NbtDocument(this.CreateComplexData());

      // act
      try
      {
        expected.Save(fileName);
        target = NbtDocument.LoadDocument(fileName);
      }
      finally
      {
        this.DeleteFile(fileName);
      }

      // assert
      NbtAssert.AreEqual(expected, target);
    }

    [Test]
    public void ToString_returns_tag_hieararchy()
    {
      // arrange
      NbtDocument target;
      string expected;
      string actual;
      StringBuilder sb;

      target = new NbtDocument(this.CreateComplexData());

      sb = new StringBuilder();
      sb.AppendLine("compound:Level");
      sb.AppendLine("  long:longTest [9223372036854775807]");
      sb.AppendLine("  short:shortTest [32767]");
      sb.AppendLine("  string:stringTest [HELLO WORLD THIS IS A TEST STRING ÅÄÖ!]");
      sb.AppendLine("  float:floatTest [0.4982315]");
      sb.AppendLine("  int:intTest [2147483647]");
      sb.AppendLine("  compound:nested compound test");
      sb.AppendLine("    compound:ham");
      sb.AppendLine("      string:name [Hampus]");
      sb.AppendLine("      float:value [0.75]");
      sb.AppendLine("    compound:egg");
      sb.AppendLine("      string:name [Eggbert]");
      sb.AppendLine("      float:value [0.5]");
      sb.AppendLine("  list:listTest (long)");
      sb.AppendLine("    long#0 [11]");
      sb.AppendLine("    long#1 [12]");
      sb.AppendLine("    long#2 [13]");
      sb.AppendLine("    long#3 [14]");
      sb.AppendLine("    long#4 [15]");
      sb.AppendLine("  list:listTest (compound)");
      sb.AppendLine("    compound#0");
      sb.AppendLine("      string:name [Compound tag #0]");
      sb.AppendLine("      long:created-on [1264099775885]");
      sb.AppendLine("    compound#1");
      sb.AppendLine("      string:name [Compound tag #1]");
      sb.AppendLine("      long:created-on [1264099775885]");
      sb.AppendLine("  byte:byteTest [127]");
      sb.AppendLine("  bytearray:byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...)) [00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30, 00, 3E, 22, 10, 08, 0A, 16, 2C, 4C, 12, 46, 20, 04, 56, 4E, 50, 5C, 0E, 2E, 58, 28, 02, 4A, 38, 30, 32, 3E, 54, 10, 3A, 0A, 48, 2C, 1A, 12, 14, 20, 36, 56, 1C, 50, 2A, 0E, 60, 58, 5A, 02, 18, 38, 62, 32, 0C, 54, 42, 3A, 3C, 48, 5E, 1A, 44, 14, 52, 36, 24, 1C, 1E, 2A, 40, 60, 26, 5A, 34, 18, 06, 62, 00, 0C, 22, 42, 08, 3C, 16, 5E, 4C, 44, 46, 52, 04, 24, 4E, 1E, 5C, 40, 2E, 26, 28, 34, 4A, 06, 30]");
      sb.AppendLine("  double:doubleTest [0.493128713218231]");

      expected = sb.ToString();

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}
