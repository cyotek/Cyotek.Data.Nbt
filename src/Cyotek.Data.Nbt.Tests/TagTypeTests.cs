using NUnit.Framework;

// sanity checks just in case the t4 generator data is screwed up

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagTypeTests
  {
    #region  Tests

    [Test]
    public void ByteArrayTest()
    {
      this.TestValue(7, TagType.ByteArray);
    }

    [Test]
    public void ByteTest()
    {
      this.TestValue(1, TagType.Byte);
    }

    [Test]
    public void CompoundTest()
    {
      this.TestValue(10, TagType.Compound);
    }

    [Test]
    public void DoubleTest()
    {
      this.TestValue(6, TagType.Double);
    }

    [Test]
    public void EndTest()
    {
      this.TestValue(0, TagType.End);
    }

    [Test]
    public void FloatTest()
    {
      this.TestValue(5, TagType.Float);
    }

    [Test]
    public void IntArrayTest()
    {
      this.TestValue(11, TagType.IntArray);
    }

    [Test]
    public void IntTest()
    {
      this.TestValue(3, TagType.Int);
    }

    [Test]
    public void ListTest()
    {
      this.TestValue(9, TagType.List);
    }

    [Test]
    public void LongTest()
    {
      this.TestValue(4, TagType.Long);
    }

    [Test]
    public void ShortTest()
    {
      this.TestValue(2, TagType.Short);
    }

    [Test]
    public void StringTest()
    {
      this.TestValue(8, TagType.String);
    }

    #endregion

    #region Test Helpers

    private void TestValue(int expected, TagType value)
    {
      // arrange
      int actual;

      // act
      actual = (int)value;

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}
