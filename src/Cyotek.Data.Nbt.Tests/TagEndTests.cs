using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagEndTests : TestBase
  {
    #region  Tests

    [Test]
    public void ConstructorTest()
    {
      // arrange
      TagEnd target;

      // act
      target = new TagEnd();

      // assert
      Assert.IsEmpty(target.Name);
    }

    [Test]
    public void ConstructorWithValueTest()
    {
      // arrange
      TagEnd tag;

      // act
      tag = new TagEnd();

      // assert
      Assert.IsEmpty(tag.Name);
    }

    [Test]
    public void ToStringTest()
    {
      // arrange
      TagEnd target;
      string expected;
      string actual;

      expected = "[End]";
      target = new TagEnd();

      // act
      actual = target.ToString();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TypeTest()
    {
      // arrange
      TagType expected;
      TagType actual;

      expected = TagType.End;

      // act
      actual = new TagEnd().Type;

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}
