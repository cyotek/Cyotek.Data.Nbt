using System.IO;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class XmlTagWriterTests : TestBase
  {
    #region  Tests

    [Test]
    public void SaveTest()
    {
      // arrange
      ITagWriter target;
      TagCompound expected;
      TagCompound actual;

      expected = this.CreateComplexData();

      target = new XmlTagWriter();

      // act
      using (Stream stream = new MemoryStream())
      {
        target.WriteDocument(stream, expected);

        stream.Seek(0, SeekOrigin.Begin);

        actual = new XmlTagReader().ReadDocument(stream);
      }

      // assert
      this.CompareTags(expected, actual);
    }

    #endregion
  }
}
