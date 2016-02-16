using System.Globalization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagShortTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagShort tag;
      short expected;

      expected = 0;

      // act
      tag = new TagShort();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagShort tag;
      string name;
      short value;

      name = "creationDate";
      value = short.MaxValue;

      // act
      tag = new TagShort(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagShort tag;
      string name;
      short expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagShort(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagShort tag;
      short value;

      value = short.MaxValue;

      // act
      tag = new TagShort(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagShort target;
      string expected;

      target = new TagShort();
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
      TagShort target;
      string expected;
      string actual;
      string name;
      short value;

      name = "tagname";
      value = short.MaxValue;
      expected = string.Format("[Short: {0}={1}]", name, value);
      target = new TagShort(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagShort target;
      string expected;
      string actual;
      string name;
      short value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = short.MaxValue;
      expected = string.Format("{2}[Short: {0}={1}]", name, value, prefix);
      target = new TagShort(name, value);

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
      short value;

      value = short.MaxValue;
      expected = value.ToString(CultureInfo.InvariantCulture);
      target = new TagShort(value);

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

      expected = TagType.Short;

      // act
      actual = new TagShort().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagShort target;
      short expected;

      target = new TagShort();
      expected = short.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
