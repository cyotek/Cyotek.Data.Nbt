using System;
using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  [TestFixture]
  public partial class BinaryTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void WriteValue_throws_exception_for_long_strings()
    {
      // arrange
      TagWriter target;
      MemoryStream stream;

      stream = new MemoryStream();
      target = this.CreateWriter(stream);

      target.WriteStartDocument();
      target.WriteStartTag(TagType.Compound);

      // act & assert
      Assert.Throws<ArgumentException>(() => target.WriteTag(new string(' ', short.MaxValue + 1)));
    }

    #endregion

    #region Test Helpers

    private TagReader CreateReader(Stream stream)
    {
      return new BinaryTagReader(stream, false);
    }

    private TagWriter CreateWriter(Stream stream)
    {
      return new BinaryTagWriter(stream);
    }

    #endregion
  }
}
