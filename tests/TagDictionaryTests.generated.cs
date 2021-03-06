//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public partial class TagDictionaryTests : TestBase
  {
    [Test]
    public void Add_adds_named_byte()
    {
      // arrange
      TagDictionary target;
      TagByte actual;
      byte expected;
      string expectedName;

      expectedName = "AlphaByte";
      expected = (byte)(byte.MaxValue >> 1);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_byte_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      byte expected;
      string expectedName;

      expectedName = "BetaByte";
      expected = (byte)(byte.MaxValue >> 1);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_short()
    {
      // arrange
      TagDictionary target;
      TagShort actual;
      short expected;
      string expectedName;

      expectedName = "AlphaShort";
      expected = (short)(short.MaxValue >> 1);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_short_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      short expected;
      string expectedName;

      expectedName = "BetaShort";
      expected = (short)(short.MaxValue >> 1);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_int()
    {
      // arrange
      TagDictionary target;
      TagInt actual;
      int expected;
      string expectedName;

      expectedName = "AlphaInt";
      expected = 1073741823;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_int_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      int expected;
      string expectedName;

      expectedName = "BetaInt";
      expected = 1073741823;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_long()
    {
      // arrange
      TagDictionary target;
      TagLong actual;
      long expected;
      string expectedName;

      expectedName = "AlphaLong";
      expected = 4611686018427387903;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_long_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      long expected;
      string expectedName;

      expectedName = "BetaLong";
      expected = 4611686018427387903;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_float()
    {
      // arrange
      TagDictionary target;
      TagFloat actual;
      float expected;
      string expectedName;

      expectedName = "AlphaFloat";
      expected = 1.701412E+38F;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_float_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      float expected;
      string expectedName;

      expectedName = "BetaFloat";
      expected = 1.701412E+38F;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_double()
    {
      // arrange
      TagDictionary target;
      TagDouble actual;
      double expected;
      string expectedName;

      expectedName = "AlphaDouble";
      expected = 8.98846567431158E+307;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_double_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      double expected;
      string expectedName;

      expectedName = "BetaDouble";
      expected = 8.98846567431158E+307;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_bytearray()
    {
      // arrange
      TagDictionary target;
      TagByteArray actual;
      byte[] expected;
      string expectedName;

      expectedName = "AlphaByteArray";
      expected = new byte[] { 2, 4, 8, 16, 32, 64, 128 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_bytearray_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      byte[] expected;
      string expectedName;

      expectedName = "BetaByteArray";
      expected = new byte[] { 2, 4, 8, 16, 32, 64, 128 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_string()
    {
      // arrange
      TagDictionary target;
      TagString actual;
      string expected;
      string expectedName;

      expectedName = "AlphaString";
      expected = "HELLO WORLD THIS IS A TEST STRING ���!";

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_string_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      string expected;
      string expectedName;

      expectedName = "BetaString";
      expected = "HELLO WORLD THIS IS A TEST STRING ���!";

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_list()
    {
      // arrange
      TagDictionary target;
      TagList actual;
      TagCollection expected;
      string expectedName;

      expectedName = "AlphaList";
      expected = new TagCollection(TagType.Int) { 2, 4, 8, 16, 32, 64, 128, 256 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_list_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      TagCollection expected;
      string expectedName;

      expectedName = "BetaList";
      expected = new TagCollection(TagType.Int) { 2, 4, 8, 16, 32, 64, 128, 256 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_compound()
    {
      // arrange
      TagDictionary target;
      TagCompound actual;
      TagDictionary expected;
      string expectedName;

      expectedName = "AlphaCompound";
      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_compound_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      TagDictionary expected;
      string expectedName;

      expectedName = "BetaCompound";
      expected = new TagDictionary { new TagByte("A", 2), new TagShort("B", 4), new TagInt("C", 8) };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_intarray()
    {
      // arrange
      TagDictionary target;
      TagIntArray actual;
      int[] expected;
      string expectedName;

      expectedName = "AlphaIntArray";
      expected = new[] { 2190, 2994, 3248, 4294394 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_intarray_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      int[] expected;
      string expectedName;

      expectedName = "BetaIntArray";
      expected = new[] { 2190, 2994, 3248, 4294394 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_longarray()
    {
      // arrange
      TagDictionary target;
      TagLongArray actual;
      long[] expected;
      string expectedName;

      expectedName = "AlphaLongArray";
      expected = new[] { long.MinValue / 2, 2994, long.MaxValue / 2, 4294394 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_longarray_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      long[] expected;
      string expectedName;

      expectedName = "BetaLongArray";
      expected = new[] { long.MinValue / 2, 2994, long.MaxValue / 2, 4294394 };

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)expected);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }


    [Test]
    public void Add_adds_named_guid()
    {
      // arrange
      TagDictionary target;
      TagByteArray actual;
      byte[] expected;
      Guid value;
      string expectedName;

      expectedName = "AlphaByteArray";
      expected = new byte[] {102, 249, 193, 82, 111, 73, 2, 72, 132, 29, 158, 85, 121, 200, 103, 6 };
      value = new Guid("{52C1F966-496F-4802-841D-9E5579C86706}");

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_guid_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      byte[] expected;
      string expectedName;
      Guid value;

      expectedName = "BetaByteArray";
      expected = new byte[] {102, 249, 193, 82, 111, 73, 2, 72, 132, 29, 158, 85, 121, 200, 103, 6 };
      value = new Guid("{52C1F966-496F-4802-841D-9E5579C86706}");

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_datetime()
    {
      // arrange
      TagDictionary target;
      TagString actual;
      string expected;
      DateTime value;
      string expectedName;

      expectedName = "AlphaString";
      expected = "2016-12-27 21:06:00Z";
      value = new DateTime(2016, 12, 27, 21, 06, 00);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_datetime_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      string expected;
      string expectedName;
      DateTime value;

      expectedName = "BetaString";
      expected = "2016-12-27 21:06:00Z";
      value = new DateTime(2016, 12, 27, 21, 06, 00);

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_boolean()
    {
      // arrange
      TagDictionary target;
      TagByte actual;
      byte expected;
      Boolean value;
      string expectedName;

      expectedName = "AlphaByte";
      expected = (byte)1;
      value = true;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_boolean_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      byte expected;
      string expectedName;
      Boolean value;

      expectedName = "BetaByte";
      expected = (byte)1;
      value = true;

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

    [Test]
    public void Add_adds_named_stringarray()
    {
      // arrange
      TagDictionary target;
      TagList actual;
      TagCollection expected;
      String[] value;
      string expectedName;

      expectedName = "AlphaList";
      expected = new TagCollection
      {
        new TagString(null, "alpha"),
        new TagString(null, "beta"),
        new TagString(null, "gamma")
      };
      value = new string[] {"alpha", "beta", "gamma"};

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.Value);
    }

    [Test]
    public void Add_adds_named_stringarray_object()
    {
      // arrange
      TagDictionary target;
      Tag actual;
      TagCollection expected;
      string expectedName;
      String[] value;

      expectedName = "BetaList";
      expected = new TagCollection
      {
        new TagString(null, "alpha"),
        new TagString(null, "beta"),
        new TagString(null, "gamma")
      };
      value = new string[] {"alpha", "beta", "gamma"};

      target = new TagDictionary();

      // act
      actual = target.Add(expectedName, (object)value);

      // assert
      Assert.IsNotNull(actual);
      Assert.IsTrue(target.Contains(expectedName));
      Assert.AreEqual(expectedName, actual.Name);
      Assert.AreEqual(expected, actual.GetValue());
    }

  }
}

