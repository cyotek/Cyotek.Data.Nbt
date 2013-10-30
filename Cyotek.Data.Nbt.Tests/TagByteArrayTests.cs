using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagByteArrayTests : TestBase
  {
    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagByteArray tag;
      byte[] expected;

      expected = new byte[0];

      // act
      tag = new TagByteArray();

      // assert
      Assert.IsEmpty(tag.Name);
      CollectionAssert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithNameAndValueTest()
    {
      // arrange
      TagByteArray tag;
      string name;
      byte[] value;

      name = "creationDate";
      value = new[]
      {
        byte.MinValue, byte.MaxValue
      };

      // act
      tag = new TagByteArray(name, value);

      // assert
      Assert.AreEqual(name, tag.Name);
      CollectionAssert.AreEqual(value, tag.Value);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagByteArray tag;
      string name;
      byte[] expected;

      name = "creationDate";
      expected = new byte[0];

      // act
      tag = new TagByteArray(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      CollectionAssert.AreEqual(expected, tag.Value);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagByteArray tag;
      byte[] value;

      value = new[]
      {
        byte.MinValue, byte.MaxValue
      };

      // act
      tag = new TagByteArray(value);

      // assert
      Assert.IsEmpty(tag.Name);
      CollectionAssert.AreEqual(value, tag.Value);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagByteArray target;
      string expected;

      target = new TagByteArray();
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
      TagByteArray target;
      string expected;
      string actual;
      string name;
      byte[] value;

      name = "tagname";
      value = new[]
      {
        byte.MinValue, byte.MaxValue
      };
      expected = string.Format("[ByteArray: {0}={1} values]", name, value.Length);
      target = new TagByteArray(name, value);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagByteArray target;
      string expected;
      string actual;
      string name;
      byte[] value;
      string prefix;

      prefix = "test";
      name = "tagname";
      value = new[]
      {
        byte.MinValue, byte.MaxValue
      };
      expected = string.Format("{2}[ByteArray: {0}={1} values]", name, value.Length, prefix);
      target = new TagByteArray(name, value);

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
      byte[] value;

      value = new[]
      {
        byte.MinValue, byte.MaxValue
      };
      expected = "00, FF";
      target = new TagByteArray(value);

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

      expected = TagType.ByteArray;

      // act
      actual = new TagByteArray().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagByteArray target;
      byte[] expected;

      target = new TagByteArray();
      expected = new[]
      {
        byte.MinValue, byte.MaxValue
      };

      // act
      target.Value = expected;

      // assert
      CollectionAssert.AreEqual(expected, target.Value);
    }
  }
}
