using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class TagDoubleTests : TestBase
  {
    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagDouble tag;
      double expected;

      expected = 0;

      // act
      tag = new TagDouble();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagDouble tag;
      string name;
      double value;

      name = "creationDate";
      value = double.MaxValue;

      // act
      tag = new TagDouble(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagDouble tag;
      string name;
      double expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagDouble(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagDouble tag;
      double value;

      value = double.MaxValue;

      // act
      tag = new TagDouble(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagDouble target;
      string expected;

      target = new TagDouble();
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
      TagDouble target;
      string expected;
      string actual;
      string name;
      double value;

      name = "tagname";
      value = double.MaxValue;
      expected = string.Format("[Double: {0}={1}]", name, value);
      target = new TagDouble(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagDouble target;
      string expected;
      string actual;
      string name;
      double value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = double.MaxValue;
      expected = string.Format("{2}[Double: {0}={1}]", name, value, prefix);
      target = new TagDouble(name, value);

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TypeTest()
    {
      // arrange
      TagType expected;
      TagType actual;

      expected = TagType.Double;

      // act
      actual = new TagDouble().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagDouble target;
      double expected;

      target = new TagDouble();
      expected = double.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }
  }
}
