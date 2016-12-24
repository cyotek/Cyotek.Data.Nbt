﻿ //------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagShortTests : TestBase
  {
    [Test]
    public void Constructor_sets_name()
    {
      // arrange
      TagShort target;
      string expected;
      string actual;

      expected = "Alphatag";

      // act
      target = new TagShort(expected);

      // assert
      actual = target.Name;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_default_name()
    {
      // arrange
      TagShort target;
      string expected;
      string actual;

      expected = string.Empty;

      // act
      target = new TagShort();

      // assert
      actual = target.Name;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_default_value()
    {
      // arrange
      TagShort target;
      short expected;
      short actual;

      expected = 0;

      // act
      target = new TagShort();

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_value()
    {
      // arrange
      TagShort target;
      short expected;
      short actual;

      expected = 16383;

      // act
      target = new TagShort(string.Empty, expected);

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_value_without_name()
    {
      // arrange
      TagShort target;
      short expected;
      short actual;

      expected = 16383;

      // act
      target = new TagShort(expected);

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void SetValue_updates_value()
    {
      // arrange
      Tag target;
      short expected;
      short actual;

      target = new TagShort();

      expected = 16383;

      // act
      target.SetValue(expected);

      // assert
      actual = ((TagShort)target).Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetValue_retrieves_value()
    {
      // arrange
      Tag target;
      short expected;
      object actual;

      expected = 16383;

      target = TagFactory.CreateTag(expected);

      // act
      actual = target.GetValue();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Equals_returns_true_for_matching_tag()
    {
      // arrange
      TagShort target;
      TagShort other;
      bool actual;

      target = new TagShort("alpha", 16383);
      other = new TagShort("alpha", 16383);

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void Equals_returns_false_with_different_name()
    {
      // arrange
      TagShort target;
      TagShort other;
      bool actual;

      target = new TagShort("Alpha", 16383);
      other = new TagShort("Beta", 16383);

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Equals_returns_false_with_different_value()
    {
      // arrange
      TagShort target;
      TagShort other;
      bool actual;

      target = new TagShort(string.Empty, 16383);
      other = new TagShort(string.Empty, 8191);

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Type_returns_correct_value()
    {
      // arrange
      TagShort target;
      TagType expected;
      TagType actual;

      target = new TagShort();

      expected = TagType.Short;

      // act
      actual = target.Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetHashCode_returns_same_value_for_matching_tags()
    {
      // arrange
      TagShort target;
      int actual;
      int expected;

      target = new TagShort("beta", 16383);

      expected = new TagShort("beta", 16383).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetHashCode_returns_different_values_with_different_name()
    {
      // arrange
      TagShort target;
      int actual;
      int notExpected;

      target = new TagShort("Alpha", 16383);

      notExpected = new TagShort("Beta", 16383).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreNotEqual(notExpected, actual);
    }

    [Test]
    public void GetHashCode_returns_different_values_with_different_value()
    {
      // arrange
      TagShort target;
      int actual;
      int notExpected;

      target = new TagShort(string.Empty, 16383);

      notExpected = new TagShort(string.Empty, 8191).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreNotEqual(notExpected, actual);
    }

    [Test]
    public void Value_can_be_set()
    {
      // arrange
      TagShort target;
      short expected;
      short actual;

      expected = 16383;

      target = new TagShort();

      // act
      target.Value = expected;

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToValueString_returns_string_version_of_value()
    {
      // arrange
      TagShort target;
      string expected;
      string actual;

      expected = "16383";

      target = new TagShort(string.Empty, 16383);

      // act
      actual = target.ToValueString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToString_returns_string_version_of_tag()
    {
      // arrange
      TagShort target;
      string expected;
      string actual;

      expected = "[Short: gamma=16383]";

      target = new TagShort("gamma", 16383);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }
  }
}

