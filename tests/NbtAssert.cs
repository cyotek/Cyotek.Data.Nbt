using System.Collections.Generic;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  public static class NbtAssert
  {
    #region Static Methods

    public static void AreEqual(TagCompound expected, TagCompound actual)
    {
      List<Tag> expectedChildValues;
      List<Tag> actualChildValues;
      ICollectionTag expectedChildren;
      ICollectionTag actualChildren;

      AreEqualBasic(expected, actual);

      expectedChildren = expected;
      actualChildren = actual;

      Assert.AreEqual(expectedChildren.IsList, actualChildren.IsList);
      Assert.AreEqual(expectedChildren.ListType, actualChildren.ListType);
      Assert.AreEqual(expectedChildren.Values.Count, actualChildren.Values.Count);

      expectedChildValues = new List<Tag>(expectedChildren.Values);
      actualChildValues = new List<Tag>(actualChildren.Values);

      for (int i = 0; i < expectedChildValues.Count; i++)
      {
        AreEqual(expectedChildValues[i], actualChildValues[i]);
      }
    }

    public static void AreEqual(Tag expected, Tag actual)
    {
      TagCompound expectedCompound;
      TagCompound actualCompound;

      AreEqualBasic(expected, actual);

      expectedCompound = expected as TagCompound;
      actualCompound = actual as TagCompound;

      if (expectedCompound != null && actualCompound != null)
      {
        AreEqual(expectedCompound, actualCompound);
      }
    }

    public static void AreEqual(ComplexData expected, ComplexData actual)
    {
      Assert.AreEqual(expected.LongValue, actual.LongValue);
      Assert.AreEqual(expected.ShortValue, actual.ShortValue);
      Assert.AreEqual(expected.StringValue, actual.StringValue);
      Assert.AreEqual(expected.FloatValue, actual.FloatValue);
      Assert.AreEqual(expected.IntegerValue, actual.IntegerValue);
      AreEqual(expected.CompoundValue, actual.CompoundValue);
      CollectionAssert.AreEqual(expected.LongList, actual.LongList);
      AreEqual(expected.CompoundList, actual.CompoundList);
      CollectionAssert.AreEqual(expected.ByteArrayValue, actual.ByteArrayValue);
      Assert.AreEqual(expected.ByteValue, actual.ByteValue);
      Assert.AreEqual(expected.DoubleValue, actual.DoubleValue);
    }

    internal static void AreEqual(NbtDocument expected, NbtDocument actual)
    {
      AreEqual(expected.DocumentRoot, actual.DocumentRoot);
    }

    private static void AreEqual(List<ComplexData.Compound2> expected, List<ComplexData.Compound2> actual)
    {
      Assert.AreEqual(expected.Count, actual.Count);

      for (int i = 0; i < expected.Count; i++)
      {
        AreEqual(expected[i], actual[i]);
      }
    }

    private static void AreEqual(ComplexData.Compound2 expected, ComplexData.Compound2 actual)
    {
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(expected.CreatedOn, actual.CreatedOn);
    }

    private static void AreEqual(Dictionary<string, ComplexData.Compound1> expected, Dictionary<string, ComplexData.Compound1> actual)
    {
      Assert.AreEqual(expected.Count, actual.Count);

      foreach (KeyValuePair<string, ComplexData.Compound1> pair in expected)
      {
        AreEqual(pair.Value, actual[pair.Key]);
      }
    }

    private static void AreEqual(ComplexData.Compound1 expected, ComplexData.Compound1 actual)
    {
      Assert.AreEqual(expected.Name, actual.Name);
      Assert.AreEqual(expected.Value, actual.Value);
    }

    private static void AreEqualBasic(Tag expected, Tag actual)
    {
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
    }

    #endregion
  }
}
