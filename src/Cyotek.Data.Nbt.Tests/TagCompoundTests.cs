using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  partial class TagCompoundTests
  {
    #region  Tests

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Value_throws_exception_if_set_to_null_value()
    {
      // arrange
      TagCompound target;

      target = new TagCompound();

      // act
      target.Value = null;

      // assert
    }

    #endregion
  }
}
