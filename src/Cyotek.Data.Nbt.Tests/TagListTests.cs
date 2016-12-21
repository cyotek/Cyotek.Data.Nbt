using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagListTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagList tag;
      TagType defaultType;

      defaultType = TagType.None;

      // act
      tag = new TagList();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
      Assert.AreEqual(defaultType, tag.ListType);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagList tag;
      string name;
      TagType value;

      name = "creationDate";
      value = TagType.String;

      // act
      tag = new TagList(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
      Assert.AreEqual(value, tag.ListType);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagList tag;
      string name;
      TagType defaultType;

      name = "creationDate";
      defaultType = TagType.None;

      // act
      tag = new TagList(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
      Assert.AreEqual(defaultType, tag.ListType);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagList tag;
      TagType value;

      value = TagType.String;

      // act
      tag = new TagList(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
      Assert.AreEqual(value, tag.ListType);
    }

    [Test]
    public void CountTest()
    {
      // arrange
      TagList target;
      int expectedCount;

      target = new TagList(TagType.Long);

      target.Value.Add((long)int.MinValue);
      target.Value.Add((long)int.MaxValue);
      target.Value.Add(long.MinValue);
      target.Value.Add(long.MaxValue);

      expectedCount = 3;

      // act
      target.Value.RemoveAt(3);

      // assert
      Assert.AreEqual(expectedCount, target.Count);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagList target;
      string expected;

      target = new TagList();
      expected = "newvalue";

      // act
      target.Name = expected;

      // assert
      Assert.AreEqual(expected, target.Name);
    }

    [Test]
    public void ToStringEmptyTest()
    {
      // arrange
      TagList target;
      string expected;
      string actual;
      string name;
      TagType itemType;

      name = "tagname";
      itemType = TagType.String;
      expected = $"[List: {name}] (0 items)";
      target = new TagList(name, itemType);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringEmptyWithIndentTest()
    {
      // arrange
      TagList target;
      string expected;
      string actual;
      string name;
      TagType itemType;
      string prefix;

      prefix = "test";
      name = "tagname";
      itemType = TagType.String;
      expected = string.Format("{1}[List: {0}] (0 items)", name, prefix);
      target = new TagList(name, itemType);

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      TagList target;
      string expected;
      string actual;
      string name;
      TagType itemType;

      name = "tagname";
      itemType = TagType.String;
      expected = $"[List: {name}] (2 items)";
      target = new TagList(name, itemType);
      target.Value.Add("item 1", "value1");
      target.Value.Add("item 2", "value2");

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagList target;
      string expected;
      string actual;
      string name;
      TagType itemType;
      string prefix;

      prefix = "test";
      name = "tagname";
      itemType = TagType.String;
      expected = string.Format("{1}[List: {0}] (2 items)", name, prefix);
      target = new TagList(name, itemType);
      target.Value.Add("item 1", "value1");
      target.Value.Add("item 2", "value2");

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToValueStringTest()
    {
      // arrange
      TagList target;
      string expected;
      string actual;
      string name;
      TagType itemType;

      name = "tagname";
      itemType = TagType.String;
      expected = "[value1, value2]";
      target = new TagList(name, itemType);
      target.Value.Add("item 1", "value1");
      target.Value.Add("item 2", "value2");

      // act
      actual = target.ToValueString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TypeTest()
    {
      // arrange
      TagType expected;
      TagType actual;

      expected = TagType.List;

      // act
      actual = new TagList().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ValueExceptionTest()
    {
      // arrange
      TagList target;

      target = new TagList();

      // act
      target.Value = null;

      // assert
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagList target;
      TagCollection expected;
      TagType defaultType;

      target = new TagList();
      expected = new TagCollection();
      defaultType = TagType.None;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
      Assert.IsNotNull(target.Value);
      Assert.AreSame(target, target.Value.Owner);
      Assert.AreEqual(defaultType, target.ListType);
    }

    #endregion
  }
}
