using System;
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
    public void Equals_returns_true_for_any_end_tag()
    {
      // arrange
      TagEnd target;
      TagEnd other;
      bool actual;

      target = new TagEnd();
      other = new TagEnd();

      // act
      actual = target.Equals(other);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void GetHashCode_returns_same_value()
    {
      // arrange
      TagEnd target;
      int expected;
      int actual;

      target = new TagEnd();

      expected = new TagEnd().GetHashCode();

      // act
      actual = target.GetHashCode();

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Tag does not support values.")]
    public void GetValue_throws_exception()
    {
      // arrange
      TagEnd target;

      target = new TagEnd();

      // act
      target.GetValue();
    }

    [Test]
    [ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Tag does not support values.")]
    public void SetValue_throws_exception()
    {
      // arrange
      TagEnd target;

      target = new TagEnd();

      // act
      target.SetValue("TEST");
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
    public void ToValueString_returns_empty_value()
    {
      // arrange
      TagEnd target;
      string actual;

      target = new TagEnd();

      // act
      actual = target.ToValueString();

      // assert
      Assert.IsEmpty(actual);
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
