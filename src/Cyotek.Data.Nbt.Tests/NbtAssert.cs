using System.Collections.Generic;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  public static class NbtAssert
  {
    #region Static Methods

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

    #endregion
  }
}
