using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagFactoryTests
  {
    #region  Tests

    [Test]
    [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unrecognized or unsupported tag type.\r\nParameter name: tagType")]
    public void CreateTag_throws_exception_for_invalid_type()
    {
      // arrange
      TagType type;

      type = (TagType)(-1);

      // act
      TagFactory.CreateTag(type);
    }

    #endregion
  }
}
