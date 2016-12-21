using System.Globalization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagLongTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagLong tag;
      long expected;

      expected = 0;

      // act
      tag = new TagLong();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagLong tag;
      string name;
      long value;

      name = "creationDate";
      value = long.MaxValue;

      // act
      tag = new TagLong(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagLong tag;
      string name;
      long expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagLong(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagLong tag;
      long value;

      value = long.MaxValue;

      // act
      tag = new TagLong(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagLong target;
      string expected;

      target = new TagLong();
      expected = "newvalue";

      // act
      target.Name = expected;

      // assert
      Assert.AreEqual(expected, target.Name);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      TagLong target;
      string expected;
      string actual;
      string name;
      long value;

      name = "tagname";
      value = long.MaxValue;
      expected = $"[Long: {name}={value}]";
      target = new TagLong(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagLong target;
      string expected;
      string actual;
      string name;
      long value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = long.MaxValue;
      expected = string.Format("{2}[Long: {0}={1}]", name, value, prefix);
      target = new TagLong(name, value);

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToValueStringTest()
    {
      // arrange
      ITag target;
      string expected;
      string actual;
      long value;

      value = long.MaxValue;
      expected = value.ToString(CultureInfo.InvariantCulture);
      target = new TagLong(value);

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

      expected = TagType.Long;

      // act
      actual = new TagLong().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagLong target;
      long expected;

      target = new TagLong();
      expected = long.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
