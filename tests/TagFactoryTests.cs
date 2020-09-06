using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagFactoryTests
  {
    #region  Tests

    [Test]
    public void CreateTag_creates_list()
    {
      // arrange
      TagList actual;
      TagType type;
      TagType listType;

      type = TagType.List;
      listType = TagType.Byte;

      // act
      actual = TagFactory.CreateTag(type, listType);

      // assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(listType, actual.ListType);
    }

    [Test]
    public void CreateTag_throws_exception_for_invalid_type()
    {
      // arrange
      TagType type;

      type = (TagType)(-1);

      // act & assert
      Assert.Throws<ArgumentException>(() => TagFactory.CreateTag(type));
    }

    [Test]
    public void CreateTag_with_list_type_for_non_list_throws_exception()
    {
      // arrange
      TagType type;
      TagType listType;

      type = TagType.Byte;
      listType = TagType.ByteArray;

      // act & assert
      Assert.Throws<ArgumentException>(() => TagFactory.CreateTag(string.Empty, type, listType));
    }

    [Test]
    public void CreateTag_with_value_throws_exception_for_invalid_type()
    {
      // arrange
      TagType type;

      type = (TagType)(-1);

      // act & assert
      Assert.Throws<ArgumentException>(() => TagFactory.CreateTag(string.Empty, type, 13));
    }

    #endregion
  }
}
