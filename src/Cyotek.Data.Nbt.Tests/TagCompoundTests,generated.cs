﻿//------------------------------------------------------------------------------
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
  public partial class TagCompoundTests : TestBase
  {
    [Test]
    public void Constructor_sets_name()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;

      expected = "Alphatag";

      // act
      target = new TagCompound(expected);

      // assert
      actual = target.Name;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_default_name()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;

      expected = string.Empty;

      // act
      target = new TagCompound();

      // assert
      actual = target.Name;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_default_value()
    {
      // arrange
      TagCompound target;
      TagDictionary expected;
      TagDictionary actual;

      expected = new TagDictionary();

      // act
      target = new TagCompound();

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_value()
    {
      // arrange
      TagCompound target;
      TagDictionary expected;
      TagDictionary actual;

      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      // act
      target = new TagCompound(string.Empty, expected);

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_value_without_name()
    {
      // arrange
      TagCompound target;
      TagDictionary expected;
      TagDictionary actual;

      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      // act
      target = new TagCompound(expected);

      // assert
      actual = target.Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void SetValue_updates_value()
    {
      // arrange
      Tag target;
      TagDictionary expected;
      TagDictionary actual;

      target = new TagCompound();

      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      // act
      target.SetValue(expected);

      // assert
      actual = ((TagCompound)target).Value;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetValue_retrieves_value()
    {
      // arrange
      Tag target;
      TagDictionary expected;
      object actual;

      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

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
      TagCompound target;
      TagCompound other;
      bool actual;

      target = new TagCompound("alpha", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });
      other = new TagCompound("alpha", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void Equals_returns_false_with_different_name()
    {
      // arrange
      TagCompound target;
      TagCompound other;
      bool actual;

      target = new TagCompound("Alpha", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });
      other = new TagCompound("Beta", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Equals_returns_false_with_different_value()
    {
      // arrange
      TagCompound target;
      TagCompound other;
      bool actual;

      target = new TagCompound(string.Empty, new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });
      other = new TagCompound(string.Empty, new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 16) });

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Type_returns_correct_value()
    {
      // arrange
      TagCompound target;
      TagType expected;
      TagType actual;

      target = new TagCompound();

      expected = TagType.Compound;

      // act
      actual = target.Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetHashCode_returns_same_value_for_matching_tags()
    {
      // arrange
      TagCompound target;
      int actual;
      int expected;

      target = new TagCompound("beta", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      expected = new TagCompound("beta", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) }).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetHashCode_returns_different_values_with_different_name()
    {
      // arrange
      TagCompound target;
      int actual;
      int notExpected;

      target = new TagCompound("Alpha", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      notExpected = new TagCompound("Beta", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) }).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreNotEqual(notExpected, actual);
    }

    [Test]
    public void GetHashCode_returns_different_values_with_different_value()
    {
      // arrange
      TagCompound target;
      int actual;
      int notExpected;

      target = new TagCompound(string.Empty, new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      notExpected = new TagCompound(string.Empty, new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 16) }).GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreNotEqual(notExpected, actual);
    }

    [Test]
    public void Value_can_be_set()
    {
      // arrange
      TagCompound target;
      TagDictionary expected;
      TagDictionary actual;

      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      target = new TagCompound();

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
      TagCompound target;
      string expected;
      string actual;

      expected = "[2, 4, 8]";

      target = new TagCompound(string.Empty, new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      // act
      actual = target.ToValueString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToString_returns_string_version_of_tag()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;

      expected = "[Compound: gamma] (3 items)";

      target = new TagCompound("gamma", new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) });

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }
  }
}

