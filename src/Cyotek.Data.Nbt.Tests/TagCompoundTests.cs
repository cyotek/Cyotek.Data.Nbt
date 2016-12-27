using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  partial class TagCompoundTests
  {
    #region  Tests

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
    [ExpectedException(typeof(ArgumentNullException))]
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
