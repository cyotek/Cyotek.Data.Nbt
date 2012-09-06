using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [DebuggerStepThrough]
  internal class TestBase
  {
    protected string AnvilRegionFileName
    { get { return Path.Combine(this.DataPath, "r.0.0.mca"); } }

    protected string ComplexDataFileName
    { get { return Path.Combine(this.DataPath, "bigtest.nbt"); } }

    protected string ComplexXmlDataFileName
    { get { return Path.Combine(this.DataPath, "complextest.xml"); } }

    protected string DataPath
    { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"); } }

    protected string OutputFileName { get; set; }

    protected string SimpleDataFileName
    { get { return Path.Combine(this.DataPath, "test.nbt"); } }

    protected string UncompressedComplexDataFileName
    { get { return Path.Combine(this.DataPath, "bigtest.raw"); } }

    protected void CompareTags(ITag expected, ITag actual)
    {
      Assert.AreEqual(expected.Type, actual.Type);
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(expected.FullPath, actual.FullPath);

      if (expected.Parent == null)
        Assert.IsNull(actual.Parent);
      else
        Assert.AreEqual(expected.Parent.Name, actual.Parent.Name);

      Assert.AreEqual(expected.CanRemove, actual.CanRemove);

      if (expected is ICollectionTag)
      {
        ICollectionTag expectedChildren;
        ICollectionTag actualChildren;

        Assert.IsInstanceOf<ICollectionTag>(actual);

        expectedChildren = (ICollectionTag)expected;
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

    protected TagCompound GetComplexData()
    {
      return new NbtDocument(this.ComplexDataFileName).DocumentRoot;
    }

    protected TagCompound GetSimpleData()
    {
      return new NbtDocument(this.SimpleDataFileName).DocumentRoot;
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
  }
}