using System.Globalization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagByteTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagByte tag;
      byte expected;

      expected = 0;

      // act
      tag = new TagByte();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagByte tag;
      string name;
      byte value;

      name = "creationDate";
      value = byte.MaxValue;

      // act
      tag = new TagByte(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagByte tag;
      string name;
      byte expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagByte(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagByte tag;
      byte value;

      value = byte.MaxValue;

      // act
      tag = new TagByte(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagByte target;
      string expected;

      target = new TagByte();
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
      TagByte target;
      string expected;
      string actual;
      string name;
      byte value;

      name = "tagname";
      value = byte.MaxValue;
      expected = $"[Byte: {name}={value}]";
      target = new TagByte(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagByte target;
      string expected;
      string actual;
      string name;
      byte value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = byte.MaxValue;
      expected = string.Format("{2}[Byte: {0}={1}]", name, value, prefix);
      target = new TagByte(name, value);

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToValueStringTest()
    {
      // arrange
      Tag target;
      string expected;
      string actual;
      byte value;

      value = byte.MaxValue;
      expected = value.ToString(CultureInfo.InvariantCulture);
      target = new TagByte(value);

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

      expected = TagType.Byte;

      // act
      actual = new TagByte().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagByte target;
      byte expected;

      target = new TagByte();
      expected = byte.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
