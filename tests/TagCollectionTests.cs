using System;
using System.Linq;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagCollectionTests : TestBase
  {
    #region  Tests

    [Test]
    public void Add_sets_limit_type()
    {
      // arrange
      TagCollection target;
      TagType expected;
      TagType actual;

      target = new TagCollection();

      expected = TagType.Byte;

      // act
      target.Add((byte)127);

      // assert
      actual = target.LimitType;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Add_sets_parent()
    {
      // arrange
      TagList owner;
      TagCollection target;
      Tag actual;

      actual = new TagByte(56);

      owner = new TagList();
      target = owner.Value;

      // act
      target.Add(actual);

      // assert
      Assert.AreSame(owner, actual.Parent);
    }

    [Test]
    public void Add_throws_exception_for_named_tags()
    {
      // arrange
      TagCollection target;

      target = new TagCollection(TagType.Byte);

      // act & assert
      Assert.Throws<ArgumentException>(() => target.Add(new TagByte("alpha", 120)));
    }

    [Test]
    public void Add_throws_exception_for_tags_not_matching_list_type()
    {
      // arrange
      TagCollection target;

      target = new TagCollection(TagType.Byte);

      // act & assert
      Assert.Throws<ArgumentException>(() => target.Add(int.MaxValue));
    }

    [Test]
    public void Add_throws_exception_for_unsupported_data_type()
    {
      // arrange
      TagCollection target;

      target = new TagCollection();

      // act & assert
      Assert.Throws<ArgumentException>(() => target.Add(TimeSpan.MinValue));
    }

    [Test]
    public void AddRange_adds_values()
    {
      // arrange
      TagCollection target;
      object[] expected;

      target = new TagCollection();

      expected = new object[]
                 {
                   8,
                   16,
                   32
                 };

      // act
      target.AddRange(expected);

      // assert
      Assert.AreEqual(expected.Length, target.Count);
      CollectionAssert.AreEqual(expected, target.Select(t => t.GetValue()).ToArray());
    }

    [Test]
    public void Clear_removes_parents()
    {
      // arrange
      TagList owner;
      TagCollection target;
      Tag actual1;
      Tag actual2;

      actual1 = new TagByte(56);
      actual2 = new TagByte(156);

      owner = new TagList();
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
    public void Constructor_sets_default_limit_type()
    {
      // arrange
      TagCollection target;
      TagType expected;
      TagType actual;

      expected = TagType.None;

      // act
      target = new TagCollection();

      // assert
      actual = target.LimitType;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Constructor_sets_limit_type()
    {
      // arrange
      TagCollection target;
      TagType expected;
      TagType actual;

      expected = TagType.Byte;

      // act
      target = new TagCollection(expected);

      // assert
      actual = target.LimitType;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Indexer_sets_parent()
    {
      // arrange
      TagList owner;
      TagCollection target;
      Tag actual;

      actual = new TagByte(56);

      owner = new TagList();
      target = owner.Value;

      target.Add(byte.MaxValue);

      // act
      target[0] = actual;

      // assert
      Assert.AreSame(owner, actual.Parent);
    }

    [Test]
    public void Indexer_throws_exception_for_tags_not_matching_list_type()
    {
      // arrange
      TagCollection target;

      target = new TagCollection(TagType.Byte)
      {
        byte.MaxValue
      };

      // act & assert
      Assert.Throws<ArgumentException>(() => target[0] = new TagInt());
    }

    [Test]
    public void Remove_clears_parent()
    {
      // arrange
      TagList owner;
      TagCollection target;
      Tag actual;

      owner = new TagList();
      target = owner.Value;

      actual = target.Add((byte)56);

      // act
      target.Remove(actual);

      // assert
      Assert.IsNull(actual.Parent);
    }

    #endregion
  }
}
