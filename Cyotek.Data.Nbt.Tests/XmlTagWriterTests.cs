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
      ITagWriter writer;
      TagCompound target;

      target = this.CreateComplexData();
      writer = new XmlTagWriter();

      // act
      using (Stream stream = File.Create(this.OutputFileName))
      {
        writer.WriteDocument(stream, target);
      }

      // assert
      FileAssert.AreEqual(this.ComplexXmlDataFileName, this.OutputFileName);
    }

    #endregion
  }
}
