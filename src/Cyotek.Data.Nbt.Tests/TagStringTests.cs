using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagStringTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagString tag;

      // act
      tag = new TagString();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.IsEmpty(tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagString tag;
      string name;
      string value;

      name = "creationDate";
      value = "notadate";

      // act
      tag = new TagString(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagString tag;
      string name;

      name = "creationDate";

      // act
      tag = new TagString(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.IsEmpty(tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagString target;
      string expected;

      target = new TagString();
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
      TagString target;
      string expected;
      string actual;
      string name;
      string value;

      name = "tagname";
      value = "tagvalue";
      expected = $"[String: {name}=\"{value}\"]";
      target = new TagString(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagString target;
      string expected;
      string actual;
      string name;
      string value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = "somerandomvalue";
      expected = string.Format("{2}[String: {0}=\"{1}\"]", name, value, prefix);
      target = new TagString(name, value);

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
      string value;

      value = "Alpha";
      expected = value;
      target = new TagString(string.Empty, value);

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

      expected = TagType.String;

      // act
      actual = new TagString().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagString target;
      string expected;

      target = new TagString();
      expected = "newvalue";

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
