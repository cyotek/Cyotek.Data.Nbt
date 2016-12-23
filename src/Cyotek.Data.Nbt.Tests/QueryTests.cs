using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class QueryTests : TestBase
  {
    #region  Tests

    [Test]
    public void QueryValueTest()
    {
      // arrange
      TagCompound target;
      long result;
      string path;
      long expectedValue;

      target = this.CreateComplexData();
      path = @"listTest (compound)\1\created-on";
      expectedValue = 1264099775885;

      // act
      result = target.QueryValue<long>(path);

      // assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedValue, result);
    }

    [Test]
    public void QueryWithAttributeTest()
    {
      // arrange
      TagCompound target;
      TagCompound result;
      string path;
      string expectedValue;

      target = this.CreateComplexData();
      path = @"listTest (compound)/[name=Compound tag #0]";
      expectedValue = "Compound tag #0";

      // act
      result = target.Query<TagCompound>(path);

      // assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedValue, result.Value["name"].GetValue());
    }

    [Test]
    public void QueryWithExplicitTypeTest()
    {
      // arrange
      TagCompound target;
      TagString result;
      string path;
      string expectedValue;

      target = this.CreateComplexData();
      path = @"listTest (compound)\0\name";
      expectedValue = "Compound tag #0";

      // act
      result = target.Query<TagString>(path);

      // assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedValue, result.Value);
    }

    [Test]
    public void QueryWithoutExplicitTypeTest()
    {
      // arrange
      TagCompound target;
      Tag result;
      string path;
      Type expectedType;
      long expectedValue;

      target = this.CreateComplexData();
      path = @"listTest (compound)\1\created-on";
      expectedType = typeof(TagLong);
      expectedValue = 1264099775885;

      // act
      result = target.Query(path);

      // assert
      Assert.IsNotNull(result);
      Assert.IsAssignableFrom(expectedType, result);
      Assert.AreEqual(expectedValue, (long)result.GetValue());
    }

    #endregion
  }
}
