using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagCompoundTests : TestBase
  {
    #region  Tests

    [Test]
    public void Contains_returns_false_if_not_found()
    {
      // arrange
      TagCompound target;
      bool actual;

      target = new TagCompound();

      target.Value.Add("Beta", 10);
      target.Value.Add("Alpha", 11);
      target.Value.Add("Gamma", 12);

      // act
      actual = target.Contains("Delta");

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Contains_returns_true_if_found()
    {
      // arrange
      TagCompound target;
      bool actual;

      target = new TagCompound();

      target.Value.Add("Beta", 10);
      target.Value.Add("Alpha", 11);
      target.Value.Add("Gamma", 12);

      // act
      actual = target.Contains("Alpha");

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void Count_returns_child_count()
    {
      // arrange
      TagCompound target;
      int actual;
      int expected;

      target = new TagCompound();

      target.Value.Add("Beta", 10);
      target.Value.Add("Alpha", 11);
      target.Value.Add("Gamma", 12);

      expected = 3;

      // act
      actual = target.Count;

      // assert
      Assert.AreEqual(actual, expected);
    }

    [Test]
    public void Count_returns_zero_for_new_compound()
    {
      // arrange
      TagCompound target;
      int expected;
      int actual;

      target = new TagCompound();

      expected = 0;

      // act
      actual = target.Count;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetEnumValue_returns_default_value()
    {
      // arrange
      TagCompound target;
      AppDomainManagerInitializationOptions expected;
      AppDomainManagerInitializationOptions actual;
      string name;

      expected = AppDomainManagerInitializationOptions.RegisterWithHost;
      name = "alpha";

      target = new TagCompound();

      // act
      actual = target.GetEnumValue(name, expected);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetEnumValue_returns_existing_value()
    {
      // arrange
      TagCompound target;
      AppDomainManagerInitializationOptions expected;
      AppDomainManagerInitializationOptions actual;
      string name;

      expected = AppDomainManagerInitializationOptions.RegisterWithHost;
      name = "alpha";

      target = new TagCompound();
      target.Value.Add(name, (int)expected);

      // act
      actual = target.GetEnumValue<AppDomainManagerInitializationOptions>(name);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Indexer_returns_item_by_index()
    {
      // arrange
      TagCompound target;
      Tag actual;
      Tag expected;

      target = new TagCompound();
      expected = new TagByte("Alpha", 10);

      target.Value.Add(new TagByte("Beta", 10));
      target.Value.Add(expected);
      target.Value.Add(new TagInt("Gamma"));

      // act
      actual = target[1];

      // assert
      Assert.AreSame(actual, expected);
    }

    [Test]
    public void Indexer_returns_item_by_name()
    {
      // arrange
      TagCompound target;
      Tag actual;
      Tag expected;

      target = new TagCompound();
      expected = new TagByte("Alpha", 10);

      target.Value.Add(new TagByte("Beta", 10));
      target.Value.Add(expected);
      target.Value.Add(new TagInt("Gamma"));

      // act
      actual = target["Alpha"];

      // assert
      Assert.AreSame(actual, expected);
    }

    [Test]
    [ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Compounds cannot be restricted to a single type.")]
    public void ListType_throws_exception_if_set()
    {
      // arrange
      TagCompound target;

      target = new TagCompound();

      // act
      ((ICollectionTag)target).ListType = TagType.Byte;
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: value")]
    public void Value_throws_exception_if_set_to_null_value()
    {
      // arrange
      TagCompound target;

      target = new TagCompound();

      // act
      target.Value = null;
    }

    #endregion
  }
}
