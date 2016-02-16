using System.Globalization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagFloatTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagFloat tag;
      float expected;

      expected = 0;

      // act
      tag = new TagFloat();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagFloat tag;
      string name;
      float value;

      name = "creationDate";
      value = float.MaxValue;

      // act
      tag = new TagFloat(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagFloat tag;
      string name;
      float expected;

      name = "creationDate";
      expected = 0;

      // act
      tag = new TagFloat(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagFloat tag;
      float value;

      value = float.MaxValue;

      // act
      tag = new TagFloat(value);

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagFloat target;
      string expected;

      target = new TagFloat();
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
      TagFloat target;
      string expected;
      string actual;
      string name;
      float value;

      name = "tagname";
      value = float.MaxValue;
      expected = string.Format("[Float: {0}={1}]", name, value);
      target = new TagFloat(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagFloat target;
      string expected;
      string actual;
      string name;
      float value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = float.MaxValue;
      expected = string.Format("{2}[Float: {0}={1}]", name, value, prefix);
      target = new TagFloat(name, value);

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
      float value;

      value = float.MaxValue;
      expected = value.ToString(CultureInfo.InvariantCulture);
      target = new TagFloat(value);

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

      expected = TagType.Float;

      // act
      actual = new TagFloat().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagFloat target;
      float expected;

      target = new TagFloat();
      expected = float.MaxValue;

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
    }

    #endregion
  }
}
