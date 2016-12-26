using System;
using System.Collections.Generic;
using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  public class TestBase
  {
    #region Constants

    #endregion

    #region Constructors

    protected TestBase()
    {
      this.BasePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    #endregion

    #region Properties

    public string BasePath { get; set; }

    protected string AnvilRegionFileName
    {
      get { return Path.Combine(this.DataPath, "r.0.0.mca"); }
    }

    protected string BadFileName
    {
      get { return Path.Combine(this.DataPath, "badfile.txt"); }
    }

    protected string ComplexDataFileName
    {
      get { return Path.Combine(this.DataPath, "bigtest.nbt"); }
    }

    protected string ComplexXmlDataFileName
    {
      get { return Path.Combine(this.DataPath, "complextest.xml"); }
    }

    protected string ComplexXmlWithoutWhitespaceDataFileName
    {
      get { return Path.Combine(this.DataPath, "complextest-no-ws.xml"); }
    }

    protected string DataPath
    {
      get { return Path.Combine(this.BasePath, "data"); }
    }

    protected string DeflateComplexDataFileName
    {
      get { return Path.Combine(this.DataPath, "complextest.def"); }
    }

    protected string SimpleDataFileName
    {
      get { return Path.Combine(this.DataPath, "test.nbt"); }
    }

    protected string UncompressedComplexDataFileName
    {
      get { return Path.Combine(this.DataPath, "bigtest.raw"); }
    }

    #endregion

    #region Methods

    protected void CompareTags(Tag expected, Tag actual)
    {
      ICollectionTag collection;

      Assert.AreEqual(expected.Type, actual.Type);
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(expected.FullPath, actual.FullPath);

      if (expected.Parent == null)
      {
        Assert.IsNull(actual.Parent);
      }
      else
      {
        Assert.AreEqual(expected.Parent.Name, actual.Parent.Name);
      }

      collection = expected as ICollectionTag;
      if (collection != null)
      {
        ICollectionTag expectedChildren;
        ICollectionTag actualChildren;
        List<Tag> expectedChildValues;
        List<Tag> actualChildValues;

        Assert.IsInstanceOf<ICollectionTag>(actual);

        expectedChildren = collection;
        actualChildren = (ICollectionTag)actual;

        Assert.AreEqual(expectedChildren.IsList, actualChildren.IsList);
        Assert.AreEqual(expectedChildren.LimitToType, actualChildren.LimitToType);
        Assert.AreEqual(expectedChildren.Values.Count, actualChildren.Values.Count);

        expectedChildValues = new List<Tag>(expectedChildren.Values);
        actualChildValues = new List<Tag>(actualChildren.Values);

        for (int i = 0; i < expectedChildValues.Count; i++)
        {
          this.CompareTags(expectedChildValues[i], actualChildValues[i]);
        }
      }
      else
      {
        Assert.IsNotInstanceOf<ICollectionTag>(actual);
      }
    }

    protected TagCompound CreateComplexData()
    {
      TagCompound root;
      TagCompound compound;
      TagCompound child;
      TagList list;

      root = new TagCompound();
      root.Name = "Level";
      root.Value.Add("longTest", 9223372036854775807);
      root.Value.Add("shortTest", (short)32767);
      root.Value.Add("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      root.Value.Add("floatTest", (float)0.498231471);
      root.Value.Add("intTest", 2147483647);

      compound = (TagCompound)root.Value.Add("nested compound test", TagType.Compound);
      child = (TagCompound)compound.Value.Add("ham", TagType.Compound);
      child.Value.Add("name", "Hampus");
      child.Value.Add("value", (float)0.75);
      child = (TagCompound)compound.Value.Add("egg", TagType.Compound);
      child.Value.Add("name", "Eggbert");
      child.Value.Add("value", (float)0.5);

      list = (TagList)root.Value.Add("listTest (long)", TagType.List, TagType.Long);
      list.Value.Add((long)11);
      list.Value.Add((long)12);
      list.Value.Add((long)13);
      list.Value.Add((long)14);
      list.Value.Add((long)15);

      list = (TagList)root.Value.Add("listTest (compound)", TagType.List, TagType.Compound);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #0");
      child.Value.Add("created-on", 1264099775885);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #1");
      child.Value.Add("created-on", 1264099775885);

      root.Value.Add("byteTest", (byte)127);
      root.Value.Add("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", ComplexData.SampleByteArray);
      root.Value.Add("doubleTest", 0.49312871321823148);

      return root;
    }

    protected TagCompound CreateSimpleNesting()
    {
      TagCompound root;
      TagCompound compound;
      TagList list;

      root = new TagCompound("project");
      list = new TagList("slices", TagType.Compound);
      compound = new TagCompound();
      compound.Value.Add(new TagCompound("location"));
      list.Value.Add(compound);
      root.Value.Add(list);
      list = new TagList("regions", TagType.Compound);
      list.Value.Add(new TagCompound());
      list.Value.Add(new TagCompound());
      root.Value.Add(list);

      return root;
    }

    protected void DeleteFile(string fileName)
    {
      if (File.Exists(fileName))
      {
        File.SetAttributes(fileName, FileAttributes.Normal);
        File.Delete(fileName);
      }
    }

    protected TagCompound GetComplexData()
    {
      return NbtDocument.LoadDocument(this.ComplexDataFileName).DocumentRoot;
    }

    protected TagCompound GetSimpleData()
    {
      return NbtDocument.LoadDocument(this.SimpleDataFileName).DocumentRoot;
    }

    protected string GetWorkFile()
    {
      string path;
      string fileName;

      fileName = string.Concat(Guid.NewGuid().ToString("N"), ".dat");
      path = this.BasePath;

      return Path.Combine(path, fileName);
    }

    protected void WriteDocumentTest<T, T2>(Func<Stream, T> createWriter, Func<Stream, T2> createReader) where T : TagWriter where T2 : TagReader
    {
      // arrange
      TagWriter target;
      TagReader reader;
      TagCompound expected;
      TagCompound actual;
      Stream stream = new MemoryStream();

      expected = this.CreateComplexData();

      target = createWriter(stream);

      // act
      target.WriteStartDocument();
      target.WriteTag(expected);
      target.WriteEndDocument();

      // assert
      stream.Seek(0, SeekOrigin.Begin);
      reader = createReader(stream);
      actual = reader.ReadDocument();
      this.CompareTags(expected, actual);
    }

    protected void WriteTest<T, T2>(Func<Stream, T> createWriter, Func<Stream, T2> createReader) where T : TagWriter where T2 : TagReader
    {
      // arrange
      TagWriter target;
      TagReader reader;
      TagCompound expected;
      TagCompound actual;
      Stream stream = new MemoryStream();

      expected = this.CreateComplexData();

      target = createWriter(stream);

      // act
      target.WriteStartDocument();
      target.WriteStartTag(TagType.Compound, "Level");
      target.WriteTag("longTest", 9223372036854775807);
      target.WriteTag("shortTest", (short)32767);
      target.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      target.WriteTag("floatTest", (float)0.498231471);
      target.WriteTag("intTest", 2147483647);
      target.WriteStartTag(TagType.Compound, "nested compound test");
      target.WriteStartTag(TagType.Compound, "ham");
      target.WriteTag("name", "Hampus");
      target.WriteTag("value", 0.75F);
      target.WriteEndTag();
      target.WriteStartTag(TagType.Compound, "egg");
      target.WriteTag("name", "Eggbert");
      target.WriteTag("value", 0.5F);
      target.WriteEndTag();
      target.WriteEndTag();
      target.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
      target.WriteTag((long)11);
      target.WriteTag((long)12);
      target.WriteTag((long)13);
      target.WriteTag((long)14);
      target.WriteTag((long)15);
      target.WriteEndTag();
      target.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
      target.WriteStartTag(TagType.Compound);
      target.WriteTag("name", "Compound tag #0");
      target.WriteTag("created-on", 1264099775885);
      target.WriteEndTag();
      target.WriteStartTag(TagType.Compound);
      target.WriteTag("name", "Compound tag #1");
      target.WriteTag("created-on", 1264099775885);
      target.WriteEndTag();
      target.WriteEndTag();
      target.WriteTag("byteTest", (byte)127);
      target.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", ComplexData.SampleByteArray);
      target.WriteTag("doubleTest", 0.49312871321823148);
      target.WriteEndTag();
      target.WriteEndDocument();

      // assert
      stream.Seek(0, SeekOrigin.Begin);
      reader = createReader(stream);
      actual = reader.ReadDocument();
      this.CompareTags(expected, actual);
    }

    #endregion
  }
}
