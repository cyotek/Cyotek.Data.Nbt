using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  internal class TagCompoundTests
    : TestBase
  {
    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagCompound tag;

      // act
      tag = new TagCompound();

      // assert
      Assert.IsEmpty(tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
    }

    [Test]
    public void ConstructorWithNameTest()
    {
      // arrange
      TagCompound tag;
      string name;

      name = "creationDate";

      // act
      tag = new TagCompound(name);

      // assert
      Assert.AreEqual(name, tag.Name);
      Assert.IsNotNull(tag.Value);
      Assert.AreSame(tag, tag.Value.Owner);
    }

    [Test]
    public void NameTest()
    {
      // arrange
      TagCompound target;
      string expected;

      target = new TagCompound();
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
      TagCompound target;
      string expected;
      string actual;
      string name;

      name = "tagname";
      expected = string.Format("[Compound: {0}] (0 entries)", name);
      target = new TagCompound(name);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringEmptyWithIndentTest()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;
      string name;
      string prefix;

      prefix = "test";
      name = "tagname";
      expected = string.Format("{1}[Compound: {0}] (0 entries)", name, prefix);
      target = new TagCompound(name);

      // act
      actual = target.ToString(prefix);

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;
      string name;

      name = "tagname";
      expected = string.Format("[Compound: {0}] (2 entries)", name);
      target = new TagCompound(name);
      target.Value.Add("item 1", "value1");
      target.Value.Add("item 2", 2.0F);

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToStringWithIndentTest()
    {
      // arrange
      TagCompound target;
      string expected;
      string actual;
      string name;
      string prefix;

      prefix = "test";
      name = "tagname";
      expected = string.Format("{1}[Compound: {0}] (2 entries)", name, prefix);
      target = new TagCompound(name);
      target.Value.Add("item 1", "value1");
      target.Value.Add("item 2", 2.0F);

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

      expected = TagType.Compound;

      // act
      actual = new TagCompound().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ValueTest()
    {
      // arrange
      TagCompound target;
      TagDictionary expected;

      target = new TagCompound();
      expected = new TagDictionary();

      // act
      target.Value = expected;

      // assert
      Assert.AreEqual(expected, target.Value);
      Assert.AreSame(target, expected.Owner);
    }
  }
}