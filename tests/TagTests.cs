using System.IO;
using System.Linq;
using Cyotek.Data.Nbt.Serialization;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class TagTests : TestBase
  {
    #region Tests

    [Test]
    public void Attempting_to_set_name_to_null_sets_empty_string()
    {
      // arrange
      Tag target;

      target = new TagByte("alpha");

      // act
      target.Name = null;

      // assert
      Assert.IsEmpty(target.Name);
    }

    [Test]
    public void Equals_returns_false_for_null_object()
    {
      // arrange
      Tag target;
      bool actual;

      target = new TagByte();

      // act
      actual = target.Equals((object)null);

      // assert
      Assert.IsFalse(actual);
    }

    [Test]
    public void Equals_returns_true_for_same_reference()
    {
      // arrange
      Tag target;
      bool actual;

      target = new TagByte();

      // act
      actual = target.Equals((object)target);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void Equals_returns_true_for_same_tag()
    {
      // arrange
      Tag target;
      Tag other;
      bool actual;

      target = new TagByte(127);
      other = new TagByte(127);

      // act
      actual = target.Equals((object)other);

      // assert
      Assert.IsTrue(actual);
    }

    [Test]
    public void Flatten_returns_tag_and_all_descendants()
    {
      // arrange
      TagCompound target;
      Tag[] tags;
      string[] expectedNames;
      string[] actualNames;
      int expectedCount;
      int actualCount;

      target = this.CreateComplexData();
      expectedCount = 29;
      expectedNames = new[]
      {
        "Level",
        "longTest",
        "shortTest",
        "stringTest",
        "floatTest",
        "intTest",
        "nested compound test",
        "ham",
        "name",
        "value",
        "egg",
        "listTest (long)",
        "",
        "listTest (compound)",
        "created-on",
        "byteTest",
        "byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))",
        "doubleTest"
      };

      // act
      tags = target.Flatten();
      actualCount = tags.Length;
      actualNames = tags.Select(t => t.Name).Distinct().ToArray();

      // assert
      Assert.AreEqual(expectedCount, actualCount);
      CollectionAssert.AreEqual(expectedNames, actualNames);
    }

    [Test]
    public void Flattern_returns_tab()
    {
      // arrange
      Tag target;
      Tag[] actual;

      target = new TagByte();

      // act
      actual = target.Flatten();

      // assert
      Assert.AreEqual(1, actual.Length);
      Assert.AreSame(target, actual[0]);
    }

    [Test]
    public void FullPathTest()
    {
      // arrange
      TagCompound data;
      Tag target;
      string expected;
      string actual;

      data = this.CreateComplexData();
      expected = @"Level\listTest (compound)\0\name";
      target = data.Query(@"listTest (compound)\0\name");

      // act
      actual = target.FullPath;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetAncestorsTest()
    {
      // arrange
      TagCompound data;
      Tag target;
      Tag[] actual;
      Tag[] expected;

      data = this.CreateComplexData();
      target = data.Query(@"listTest (compound)\0\name");
      expected = new[]
      {
        data,
        data.Value["listTest (compound)"],
        data.Query(@"listTest (compound)\0")
      };

      // act
      actual = target.GetAncestors();

      // assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadExceptionTest()
    {
      // arrange
      MemoryStream stream;
      TagReader reader;

      stream = new MemoryStream();
      stream.WriteByte(255);
      stream.Seek(0, SeekOrigin.Begin);
      reader = new BinaryTagReader(stream);

      // act & assert
      Assert.Throws<InvalidDataException>(() => reader.ReadTag());
    }

    [Test]
    public void TestLoadComplexNbt()
    {
      Tag tag;

      tag = this.CreateComplexData();

      Assert.IsNotNull(tag);
      Assert.IsInstanceOf<TagCompound>(tag);
      TagCompound level = tag as TagCompound;
      Assert.AreEqual("Level", level.Name);

      TagShort shortTest = level.GetShort("shortTest");
      Assert.IsNotNull(shortTest);
      Assert.AreEqual("shortTest", shortTest.Name);
      Assert.AreEqual(32767, shortTest.Value);

      TagLong longTest = level.GetLong("longTest");
      Assert.IsNotNull(longTest);
      Assert.AreEqual("longTest", longTest.Name);
      Assert.AreEqual(9223372036854775807, longTest.Value);

      TagFloat floatTest = level.GetFloat("floatTest");
      Assert.IsNotNull(floatTest);
      Assert.AreEqual("floatTest", floatTest.Name);
      Assert.AreEqual(0.49823147f, floatTest.Value);

      TagString stringTest = level.GetString("stringTest");
      Assert.IsNotNull(stringTest);
      Assert.AreEqual("stringTest", stringTest.Name);
      Assert.AreEqual("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!", stringTest.Value);

      TagInt intTest = level.GetInt("intTest");
      Assert.IsNotNull(intTest);
      Assert.AreEqual("intTest", intTest.Name);
      Assert.AreEqual(2147483647, intTest.Value);

      TagCompound nestedCompoundTest = level.GetCompound("nested compound test");
      Assert.IsNotNull(nestedCompoundTest);
      Assert.AreEqual("nested compound test", nestedCompoundTest.Name);

      TagCompound ham = nestedCompoundTest.GetCompound("ham");
      Assert.IsNotNull(ham);
      Assert.AreEqual("ham", ham.Name);

      TagString ham_name = ham.GetString("name");
      Assert.IsNotNull(ham_name);
      Assert.AreEqual("name", ham_name.Name);
      Assert.AreEqual("Hampus", ham_name.Value);

      TagFloat ham_value = ham.GetFloat("value");
      Assert.IsNotNull(ham_value);
      Assert.AreEqual("value", ham_value.Name);
      Assert.AreEqual(0.75f, ham_value.Value);

      TagCompound egg = nestedCompoundTest.GetCompound("egg");
      Assert.IsNotNull(egg);
      Assert.AreEqual("egg", egg.Name);

      TagString egg_name = egg.GetString("name");
      Assert.IsNotNull(egg_name);
      Assert.AreEqual("name", egg_name.Name);
      Assert.AreEqual("Eggbert", egg_name.Value);

      TagFloat egg_value = egg.GetFloat("value");
      Assert.IsNotNull(egg_value);
      Assert.AreEqual("value", egg_value.Name);
      Assert.AreEqual(0.5f, egg_value.Value);

      TagByte byteTest = level.GetByte("byteTest");
      Assert.IsNotNull(byteTest);
      Assert.AreEqual("byteTest", byteTest.Name);
      Assert.AreEqual(0x7f, byteTest.Value);

      TagDouble doubleTest = level.GetDouble("doubleTest");
      Assert.IsNotNull(doubleTest);
      Assert.AreEqual("doubleTest", doubleTest.Name);
      Assert.AreEqual(0.4931287132182315, doubleTest.Value);

      TagList listTest_long = level.GetList("listTest (long)");
      Assert.IsNotNull(listTest_long);
      Assert.AreEqual("listTest (long)", listTest_long.Name);
      Assert.IsNotNull(listTest_long.Value);
      Assert.AreEqual(5, listTest_long.Value.Count);
      Assert.AreEqual(11, (listTest_long.Value[0] as TagLong).Value);
      Assert.AreEqual(12, (listTest_long.Value[1] as TagLong).Value);
      Assert.AreEqual(13, (listTest_long.Value[2] as TagLong).Value);
      Assert.AreEqual(14, (listTest_long.Value[3] as TagLong).Value);
      Assert.AreEqual(15, (listTest_long.Value[4] as TagLong).Value);

      TagList listTest_compound = level.GetList("listTest (compound)");
      Assert.IsNotNull(listTest_compound);
      Assert.AreEqual("listTest (compound)", listTest_compound.Name);
      Assert.IsNotNull(listTest_compound.Value);
      Assert.AreEqual(2, listTest_compound.Value.Count);
      TagCompound listTest_compound_tag0 = listTest_compound.Value[0] as TagCompound;
      Assert.IsNotNull(listTest_compound_tag0);
      TagString listTest_compound_tag0_name = listTest_compound_tag0.GetString("name");
      Assert.IsNotNull(listTest_compound_tag0_name);
      Assert.AreEqual("name", listTest_compound_tag0_name.Name);
      Assert.AreEqual("Compound tag #0", listTest_compound_tag0_name.Value);
      TagLong listTest_compound_tag0_createdOn = listTest_compound_tag0.GetLong("created-on");
      Assert.IsNotNull(listTest_compound_tag0_createdOn);
      Assert.AreEqual("created-on", listTest_compound_tag0_createdOn.Name);
      Assert.AreEqual(1264099775885, listTest_compound_tag0_createdOn.Value);

      TagCompound listTest_compound_tag1 = listTest_compound.Value[1] as TagCompound;
      Assert.IsNotNull(listTest_compound_tag1);
      TagString listTest_compound_tag1_name = listTest_compound_tag1.GetString("name");
      Assert.IsNotNull(listTest_compound_tag1_name);
      Assert.AreEqual("name", listTest_compound_tag1_name.Name);
      Assert.AreEqual("Compound tag #1", listTest_compound_tag1_name.Value);
      TagLong listTest_compound_tag1_createdOn = listTest_compound_tag1.GetLong("created-on");
      Assert.IsNotNull(listTest_compound_tag1_createdOn);
      Assert.AreEqual("created-on", listTest_compound_tag1_createdOn.Name);
      Assert.AreEqual(1264099775885, listTest_compound_tag1_createdOn.Value);

      TagByteArray byteArrayTest = level.GetByteArray("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))");
      Assert.IsNotNull(byteArrayTest);
      Assert.AreEqual("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", byteArrayTest.Name);
      Assert.IsNotNull(byteArrayTest.Value);
      Assert.AreEqual(1000, byteArrayTest.Value.Length);
    }

    [Test]
    public void TestLoadSimpleNbt()
    {
      Tag tag;

      tag = this.GetSimpleData();

      Assert.IsNotNull(tag);
      Assert.IsInstanceOf<TagCompound>(tag);
      TagCompound root = tag as TagCompound;
      Assert.AreEqual("hello world", root.Name);

      TagString tagStr = root.GetString("name");
      Assert.AreEqual("name", tagStr.Name);

      Assert.AreEqual("Bananrama", tagStr.Value);
    }

    [Test]
    public void TestReadNonExistantTagFromCompound()
    {
      TagCompound newTag = new TagCompound();

      Tag aTag = newTag.GetTag("nope");

      Assert.IsNull(aTag);

      TagCompound fileTag = this.GetSimpleData();

      aTag = fileTag.GetTag("Entities");

      Assert.IsNull(aTag);
    }

    [Test]
    public void ChildCount_returns_zero_for_primitive_tag()
    {
      // arrange
      Tag target;
      int expected;
      int actual;

      target = new TagByte(127);
      expected = 0;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_zero_for_empty_compound()
    {
      // arrange
      TagCompound target;
      int expected;
      int actual;

      target = new TagCompound("empty");
      expected = 0;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_zero_for_empty_list()
    {
      // arrange
      TagList target;
      int expected;
      int actual;

      target = new TagList("empty", TagType.Int);
      expected = 0;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_child_count_for_flat_compound()
    {
      // arrange
      TagCompound target;
      int expected;
      int actual;

      target = new TagCompound("test");
      target.Value.Add("a", 1);
      target.Value.Add("b", 2);
      target.Value.Add("c", 3);
      expected = 3;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_child_count_for_flat_list()
    {
      // arrange
      TagList target;
      int expected;
      int actual;

      target = new TagList("test", TagType.Int);
      target.Value.Add(1);
      target.Value.Add(2);
      target.Value.Add(3);
      target.Value.Add(4);
      target.Value.Add(5);
      expected = 5;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_total_descendants_for_nested_compound()
    {
      // arrange
      TagCompound target;
      TagCompound child;
      int expected;
      int actual;

      target = new TagCompound("root");
      child = (TagCompound)target.Value.Add("child", TagType.Compound);
      child.Value.Add("a", 1);
      child.Value.Add("b", 2);
      // Structure: root -> child -> (a, b)
      // ChildCount = 3 (child + a + b)
      expected = 3;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_total_descendants_for_nested_list()
    {
      // arrange
      TagList target;
      TagCompound child;
      int expected;
      int actual;

      target = new TagList("root", TagType.Compound);
      child = (TagCompound)target.Value.Add(TagType.Compound);
      child.Value.Add("x", 10);
      child = (TagCompound)target.Value.Add(TagType.Compound);
      child.Value.Add("y", 20);
      child.Value.Add("z", 30);
      // Structure: root -> (compound1 -> x), (compound2 -> y, z)
      // ChildCount = 5 (compound1 + x + compound2 + y + z)
      expected = 5;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_correct_count_for_complex_data()
    {
      // arrange
      TagCompound target;
      int expected;
      int actual;

      target = this.CreateComplexData();
      // Flatten returns 29 tags including self, so ChildCount should be 28
      expected = 28;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_returns_correct_count_for_deeply_nested_structure()
    {
      // arrange
      TagCompound target;
      TagCompound current;
      int depth;
      int expected;
      int actual;

      depth = 10;
      target = new TagCompound("level0");
      current = target;

      for (int i = 1; i <= depth; i++)
      {
        current = (TagCompound)current.Value.Add("level" + i, TagType.Compound);
      }

      // Structure: level0 -> level1 -> level2 -> ... -> level10
      // ChildCount = 10 (not counting self)
      expected = depth;

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChildCount_is_consistent_with_Flatten()
    {
      // arrange
      TagCompound target;
      int ChildCountResult;
      int flattenCount;

      target = this.CreateComplexData();

      // act
      ChildCountResult = target.ChildCount;
      flattenCount = target.Flatten().Length - 1; // Flatten includes self

      // assert
      Assert.AreEqual(flattenCount, ChildCountResult);
    }

    [Test]
    public void ChildCount_returns_zero_for_string_tag()
    {
      // arrange
      Tag target;
      int actual;

      target = new TagString("test", "value");

      // act
      actual = target.ChildCount;

      // assert
      Assert.AreEqual(0, actual);
    }

    [Test]
    public void ChildCount_returns_zero_for_array_tags()
    {
      // arrange
      TagByteArray byteArrayTag;
      TagIntArray intArrayTag;
      TagLongArray longArrayTag;

      byteArrayTag = new TagByteArray("bytes", new byte[] { 1, 2, 3 });
      intArrayTag = new TagIntArray("ints", new int[] { 1, 2, 3 });
      longArrayTag = new TagLongArray("longs", new long[] { 1, 2, 3 });

      // act & assert
      Assert.AreEqual(0, byteArrayTag.ChildCount);
      Assert.AreEqual(0, intArrayTag.ChildCount);
      Assert.AreEqual(0, longArrayTag.ChildCount);
    }

    [Test]
    public void Depth_returns_zero_for_root_tag()
    {
      // arrange
      Tag target;
      int expected;
      int actual;

      target = new TagCompound("root");
      expected = 0;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_zero_for_standalone_primitive_tag()
    {
      // arrange
      Tag target;
      int expected;
      int actual;

      target = new TagByte(127);
      expected = 0;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_one_for_direct_child()
    {
      // arrange
      TagCompound root;
      Tag target;
      int expected;
      int actual;

      root = new TagCompound("root");
      root.Value.Add("child", 42);
      target = root.Value["child"];
      expected = 1;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_two_for_grandchild()
    {
      // arrange
      TagCompound root;
      TagCompound child;
      Tag target;
      int expected;
      int actual;

      root = new TagCompound("root");
      child = (TagCompound)root.Value.Add("child", TagType.Compound);
      child.Value.Add("grandchild", 42);
      target = child.Value["grandchild"];
      expected = 2;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_correct_value_for_deeply_nested_tag()
    {
      // arrange
      TagCompound root;
      TagCompound current;
      Tag target;
      int depth;
      int expected;
      int actual;

      depth = 10;
      root = new TagCompound("level0");
      current = root;

      for (int i = 1; i <= depth; i++)
      {
        current = (TagCompound)current.Value.Add("level" + i, TagType.Compound);
      }

      target = current; // level10
      expected = depth;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_correct_value_for_tag_in_list()
    {
      // arrange
      TagCompound root;
      TagList list;
      Tag target;
      int expected;
      int actual;

      root = new TagCompound("root");
      list = (TagList)root.Value.Add("list", TagType.List, TagType.Int);
      list.Value.Add(1);
      list.Value.Add(2);
      list.Value.Add(3);
      target = list.Value[1]; // second item in list
      expected = 2; // root -> list -> item

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_correct_value_for_complex_data()
    {
      // arrange
      TagCompound data;
      Tag target;
      int expected;
      int actual;

      data = this.CreateComplexData();
      target = data.Query(@"listTest (compound)\0\name");
      // Level -> listTest (compound) -> [0] -> name
      expected = 3;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_is_consistent_with_GetAncestors_length()
    {
      // arrange
      TagCompound data;
      Tag target;
      int depthResult;
      int ancestorsLength;

      data = this.CreateComplexData();
      target = data.Query(@"nested compound test\ham\name");

      // act
      depthResult = target.Depth;
      ancestorsLength = target.GetAncestors().Length;

      // assert
      Assert.AreEqual(ancestorsLength, depthResult);
    }

    [Test]
    public void Depth_returns_correct_value_for_nested_compound_in_list()
    {
      // arrange
      TagCompound root;
      TagList list;
      TagCompound compound;
      Tag target;
      int expected;
      int actual;

      root = new TagCompound("root");
      list = (TagList)root.Value.Add("compounds", TagType.List, TagType.Compound);
      compound = (TagCompound)list.Value.Add(TagType.Compound);
      compound.Value.Add("value", 123);
      target = compound.Value["value"];
      // root -> compounds -> [0] -> value
      expected = 3;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_one_for_list_child()
    {
      // arrange
      TagCompound root;
      TagList target;
      int expected;
      int actual;

      root = new TagCompound("root");
      target = (TagList)root.Value.Add("list", TagType.List, TagType.Int);
      expected = 1;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Depth_returns_zero_for_orphaned_tag()
    {
      // arrange
      TagInt target;
      int expected;
      int actual;

      // Create a tag that was never added to a parent
      target = new TagInt("orphan", 42);
      expected = 0;

      // act
      actual = target.Depth;

      // assert
      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}
