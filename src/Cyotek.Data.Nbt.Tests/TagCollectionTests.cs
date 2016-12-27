using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagCollectionTests
  {
    #region  Tests

    [Test]
    public void AddBoolWithNameTest()
    {
      this.AddTest<bool, TagByte>(Guid.NewGuid().ToString(), true, 1);
    }

    [Test]
    public void AddByteArrayTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.ByteArray);
    }

    [Test]
    public void AddByteArrayWithNameTest()
    {
      this.AddTest<byte[], TagByteArray>(Guid.NewGuid().ToString(), new[]
                                                                    {
                                                                      byte.MinValue,
                                                                      byte.MaxValue
                                                                    });
    }

    [Test]
    public void AddByteTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Byte);
    }

    [Test]
    public void AddByteWithNameTest()
    {
      this.AddTest<byte, TagByte>(Guid.NewGuid().ToString(), byte.MaxValue);
    }

    [Test]
    public void AddCompoundTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Compound);
    }

    [Test]
    public void AddDateTimeWithNameTest()
    {
      this.AddTest<DateTime, TagString>(Guid.NewGuid().ToString(), DateTime.MaxValue, DateTime.MaxValue.ToString("u"));
    }

    [Test]
    public void AddDoubleTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Double);
    }

    [Test]
    public void AddDoubleWithNameTest()
    {
      this.AddTest<double, TagDouble>(Guid.NewGuid().ToString(), double.MaxValue);
    }

    [Test]
    public void AddFloatTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Float);
    }

    [Test]
    public void AddFloatWithNameTest()
    {
      this.AddTest<float, TagFloat>(Guid.NewGuid().ToString(), float.MaxValue);
    }

    [Test]
    public void AddGuidWithNameTest()
    {
      Guid value;

      value = Guid.NewGuid();

      this.AddTest<Guid, TagByteArray>(Guid.NewGuid().ToString(), value, value.ToByteArray());
    }

    [Test]
    public void AddIntArrayTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.IntArray);
    }

    [Test]
    public void AddIntArrayWithNameTest()
    {
      this.AddTest<int[], TagIntArray>(Guid.NewGuid().ToString(), new[]
                                                                  {
                                                                    int.MinValue,
                                                                    int.MaxValue
                                                                  });
    }

    [Test]
    public void AddIntTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Int);
    }

    [Test]
    public void AddIntWithNameTest()
    {
      this.AddTest<int, TagInt>(Guid.NewGuid().ToString(), int.MaxValue);
    }

    [Test]
    public void AddListTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.List, TagType.String);
    }

    [Test]
    public void AddLongTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Long);
    }

    [Test]
    public void AddLongWithNameTest()
    {
      this.AddTest<long, TagLong>(Guid.NewGuid().ToString(), long.MaxValue);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void AddObjectWithNameTest()
    {
      this.AddTest<object, TagByte>(Guid.NewGuid().ToString(), 'c');
    }

    [Test]
    public void AddRangeTest()
    {
      // arrange
      TagCollection target;
      string key1;
      string key2;
      string value1;
      string value2;

      target = new TagCollection();
      key1 = Guid.NewGuid().ToString();
      key2 = Guid.NewGuid().ToString();
      value1 = Guid.NewGuid().ToString();
      value2 = Guid.NewGuid().ToString();

      // act
      target.AddRange(new[]
                      {
                        new KeyValuePair<string, object>(key1, value1),
                        new KeyValuePair<string, object>(key2, value2)
                      });

      // assert
      Assert.AreEqual(2, target.Count);
    }

    [Test]
    public void AddShortTypeTest()
    {
      this.AddTagByTypeTest(Guid.NewGuid().ToString(), TagType.Short);
    }

    [Test]
    public void AddShortWithNameTest()
    {
      this.AddTest<short, TagShort>(Guid.NewGuid().ToString(), short.MaxValue);
    }

    [Test]
    public void AddStringWithNameTest()
    {
      this.AddTest<string, TagString>(Guid.NewGuid().ToString(), "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
    }

    #endregion

    #region Test Helpers

    protected void AddTagByTypeTest(string name, TagType type)
    {
      // arrange
      TagCollection target;
      Tag tag;

      target = new TagCollection();

      // act
      tag = target.Add(name, type);

      // assert
      Assert.IsNotNull(tag);
      Assert.Contains(tag, target);
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(type, tag.Type);
    }

    protected void AddTagByTypeTest(string name, TagType type, TagType limitType)
    {
      // arrange
      TagCollection target;
      Tag tag;

      target = new TagCollection();

      // act
      tag = target.Add(name, type, limitType);

      // assert
      Assert.IsNotNull(tag);
      Assert.Contains(tag, target);
      Assert.AreEqual(name, tag.Name);
      Assert.AreEqual(type, tag.Type);
      Assert.IsInstanceOf<ICollectionTag>(tag);
      Assert.AreEqual(limitType, ((ICollectionTag)tag).LimitToType);
    }

    protected void AddTest<TValue, TTag>(string name, TValue value)
    {
      this.AddTest<TValue, TTag>(name, value, null);
    }

    protected void AddTest<TValue, TTag>(string name, TValue value, object alternateValue)
    {
      // arrange
      TagCollection target;
      Tag tag;

      target = new TagCollection();

      // act
      tag = target.Add(name, value);

      // assert
      Assert.IsNotNull(tag);
      Assert.Contains(tag, target);
      Assert.AreEqual(name, tag.Name);
      Assert.IsInstanceOf<TTag>(tag);
      Assert.AreEqual(alternateValue ?? value, tag.GetValue());
    }

    #endregion
  }
}
