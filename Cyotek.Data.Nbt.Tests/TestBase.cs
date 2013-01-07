using System;
using System.IO;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  internal class TestBase
  {
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

    protected string DataPath
    {
      get { return Path.Combine(this.BasePath, "data"); }
    }

    protected string DeflateComplexDataFileName
    {
      get { return Path.Combine(this.DataPath, "complextest.def"); }
    }

    protected string OutputFileName { get; set; }

    protected string SimpleDataFileName
    {
      get { return Path.Combine(this.DataPath, "test.nbt"); }
    }

    protected string UncompressedComplexDataFileName
    {
      get { return Path.Combine(this.DataPath, "bigtest.raw"); }
    }

    #endregion

    #region Members

    protected void CompareTags(ITag expected, ITag actual)
    {
      ICollectionTag collection;

      Assert.AreEqual(expected.Type, actual.Type);
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(expected.FullPath, actual.FullPath);

      if (expected.Parent == null)
        Assert.IsNull(actual.Parent);
      else
        Assert.AreEqual(expected.Parent.Name, actual.Parent.Name);

      Assert.AreEqual(expected.CanRemove, actual.CanRemove);

      collection = expected as ICollectionTag;
      if (collection != null)
      {
        ICollectionTag expectedChildren;
        ICollectionTag actualChildren;

        Assert.IsInstanceOf<ICollectionTag>(actual);

        expectedChildren = collection;
        actualChildren = (ICollectionTag)actual;

        Assert.AreEqual(expectedChildren.IsList, actualChildren.IsList);
        Assert.AreEqual(expectedChildren.LimitToType, actualChildren.LimitToType);
        Assert.AreEqual(expectedChildren.Values.Count, actualChildren.Values.Count);

        for (int i = 0; i < expectedChildren.Values.Count; i++)
          this.CompareTags(expectedChildren.Values[i], actualChildren.Values[i]);
      }
      else
        Assert.IsNotInstanceOf<ICollectionTag>(actual);
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
      root.Value.Add("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", new byte[] { 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48, 0, 62, 34, 16, 8, 10, 22, 44, 76, 18, 70, 32, 4, 86, 78, 80, 92, 14, 46, 88, 40, 2, 74, 56, 48, 50, 62, 84, 16, 58, 10, 72, 44, 26, 18, 20, 32, 54, 86, 28, 80, 42, 14, 96, 88, 90, 2, 24, 56, 98, 50, 12, 84, 66, 58, 60, 72, 94, 26, 68, 20, 82, 54, 36, 28, 30, 42, 64, 96, 38, 90, 52, 24, 6, 98, 0, 12, 34, 66, 8, 60, 22, 94, 76, 68, 70, 82, 4, 36, 78, 30, 92, 64, 46, 38, 40, 52, 74, 6, 48 });
      root.Value.Add("doubleTest", 0.49312871321823148);

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
      return new NbtDocument(this.ComplexDataFileName).DocumentRoot;
    }

    protected TagCompound GetSimpleData()
    {
      return new NbtDocument(this.SimpleDataFileName).DocumentRoot;
    }

    protected string GetWorkFile()
    {
      string path;
      string fileName;

      fileName = string.Concat(Guid.NewGuid().ToString("N"), ".dat");
      path = this.BasePath;

      return Path.Combine(path, fileName);
    }

    [SetUp]
    protected void SetUp()
    {
      this.OutputFileName = Path.Combine(this.DataPath, Path.ChangeExtension(Guid.NewGuid().ToString("N"), ".tmp"));
    }

    [TearDown]
    protected void TearDown()
    {
      if (!string.IsNullOrEmpty(this.OutputFileName) && File.Exists(this.OutputFileName))
        File.Delete(this.OutputFileName);
    }

    #endregion
  }
}
