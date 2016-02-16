using System.Globalization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagIntTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagInt tag;
      int expected;

      expected = 0;

      // act
      tag = new TagInt();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagInt tag;
      string name;
      int value;

      name = "creationDate";
      value = int.MaxValue;

      // act
      tag = new TagInt(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagInt tag;
      string name;
      int expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagInt(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagInt tag;
      int value;

      value = int.MaxValue;

      // act
      tag = new TagInt(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagInt target;
      string expected;

      target = new TagInt();
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
      TagInt target;
      string expected;
      string actual;
      string name;
      int value;

      name = "tagname";
      value = int.MaxValue;
      expected = string.Format("[Int: {0}={1}]", name, value);
      target = new TagInt(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagInt target;
      string expected;
      string actual;
      string name;
      int value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = int.MaxValue;
      expected = string.Format("{2}[Int: {0}={1}]", name, value, prefix);
      target = new TagInt(name, value);

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
      int value;

      value = int.MaxValue;
      expected = value.ToString(CultureInfo.InvariantCulture);
      target = new TagInt(value);

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

      expected = TagType.Int;

      // act
      actual = new TagInt().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagInt target;
      int expected;

      target = new TagInt();
      expected = int.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
