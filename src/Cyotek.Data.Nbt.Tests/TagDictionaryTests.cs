using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  partial class TagDictionaryTests
  {
    #region  Tests

    [Test]
    public void Add_sets_parent()
    {
      // arrange
      TagCompound owner;
      TagDictionary target;
      Tag actual;

      actual = new TagByte("alpha", 56);

      owner = new TagCompound();
      target = owner.Value;

      // act
      target.Add(actual);

      // assert
      Assert.AreSame(owner, actual.Parent);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid value type.\r\nParameter name: value")]
    public void Add_throws_exception_for_unsupported_data_type()
    {
      // arrange
      TagDictionary target;

      target = new TagDictionary();

      // act
      target.Add("alpha", TimeSpan.MinValue);
    }

    [Test]
    public void AddRange_adds_dictionary_contents()
    {
      // arrange
      TagDictionary target;
      Dictionary<string, object> expected;

      target = new TagDictionary();

      expected = new Dictionary<string, object>
                 {
                   {
                     "alpha", (byte)1
                   },
                   {
                     "beta", (short)short.MaxValue
                   },
                   {
                     "gamma", int.MaxValue
                   }
                 };

      // act
      target.AddRange(expected);

      // assert
      Assert.AreEqual(expected.Count, target.Count);
      Assert.IsInstanceOf<TagByte>(target["alpha"]);
      Assert.AreEqual(1, target["alpha"].GetValue());
      Assert.IsInstanceOf<TagShort>(target["beta"]);
      Assert.AreEqual(short.MaxValue, target["beta"].GetValue());
      Assert.IsInstanceOf<TagInt>(target["gamma"]);
      Assert.AreEqual(int.MaxValue, target["gamma"].GetValue());
    }

    [Test]
    public void AddRange_adds_multiple__key_value_pairs()
    {
      // arrange
      TagDictionary target;
      KeyValuePair<string, object>[] expected;

      target = new TagDictionary();

      expected = new[]
                 {
                   new KeyValuePair<string, object>("alpha", (byte)1),
                   new KeyValuePair<string, object>("beta", (short)short.MaxValue),
                   new KeyValuePair<string, object>("gamma", int.MaxValue)
                 };

      // act
      target.AddRange(expected);

      // assert
      Assert.AreEqual(expected.Length, target.Count);
      Assert.IsInstanceOf<TagByte>(target["alpha"]);
      Assert.AreEqual(1, target["alpha"].GetValue());
      Assert.IsInstanceOf<TagShort>(target["beta"]);
      Assert.AreEqual(short.MaxValue, target["beta"].GetValue());
      Assert.IsInstanceOf<TagInt>(target["gamma"]);
      Assert.AreEqual(int.MaxValue, target["gamma"].GetValue());
    }

    [Test]
    public void AddRange_adds_tags()
    {
      // arrange
      TagDictionary target;
      Tag[] expected;

      target = new TagDictionary();

      expected = new Tag[]
                 {
                   new TagByte("alpha", (byte)1),
                   new TagShort("beta", (short)short.MaxValue),
                   new TagInt("gamma", int.MaxValue)
                 };

      // act
      target.AddRange(expected);

      // assert
      Assert.AreEqual(expected.Length, target.Count);
      Assert.AreSame(target["alpha"], expected[0]);
      Assert.AreSame(target["beta"], expected[1]);
      Assert.AreSame(target["gamma"], expected[2]);
    }

    [Test]
    public void Changing_tag_name_updates_key()
    {
      // arrange
      TagCompound owner;
      TagDictionary target;
      Tag actual;

      actual = new TagByte("alpha", 56);

      owner = new TagCompound();
      target = owner.Value;

      target.Add(actual);

      // act
      actual.Name = "beta";

      // assert
      Assert.IsFalse(target.Contains("alpha"));
      Assert.IsTrue(target.Contains("beta"));
      Assert.AreSame(actual, target["beta"]);
    }

    [Test]
    public void Clear_removes_parents()
    {
      // arrange
      TagCompound owner;
      TagDictionary target;
      Tag actual1;
      Tag actual2;

      actual1 = new TagByte("alpha", 56);
      actual2 = new TagShort("beta", 56);

      owner = new TagCompound();
      target = owner.Value;

      target.Add(actual1);
      target.Add(actual2);

      // act
      target.Clear();

      // assert
      Assert.IsNull(actual1.Parent);
      Assert.IsNull(actual2.Parent);
    }

    [Test]
    public void Remove_clears_parent()
    {
      // arrange
      TagCompound owner;
      TagDictionary target;
      Tag actual;

      owner = new TagCompound();
      target = owner.Value;

      actual = target.Add("alpha", (byte)56);

      // act
      target.Remove(actual);

      // assert
      Assert.IsNull(actual.Parent);
    }

    #endregion
  }
}
