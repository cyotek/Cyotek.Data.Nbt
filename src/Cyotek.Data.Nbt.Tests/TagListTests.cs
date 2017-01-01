using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagListTests : TestBase
  {
    #region  Tests

    [Test]
    public void Count_returns_number_of_children()
    {
      // arrange
      TagList target;
      int expected;
      int actual;

      target = new TagList(TagType.Int);
      target.Value.Add(256);
      target.Value.Add(512);
      target.Value.Add(1024);

      expected = 3;

      // act
      actual = target.Count;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Value_throws_exception_if_set_to_null_value()
    {
      // arrange
      TagList target;

      target = new TagList();

      // act
      target.Value = null;

      // assert
    }

    #endregion
  }
}
