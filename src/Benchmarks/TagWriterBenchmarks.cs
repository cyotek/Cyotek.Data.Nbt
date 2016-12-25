using System.IO;
using BenchmarkDotNet.Attributes;
using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;

namespace Benchmarks
{
  [MemoryDiagnoser]
  //[InliningDiagnoser]
  public class TagWriterBenchmarks : BenchmarksBase
  {
    #region Constants

    private readonly TagCompound _predefined;

    #endregion

    #region Constructors

    public TagWriterBenchmarks()
    {
      _predefined = this.CreateComplexData();
    }

    #endregion

    #region Methods

    [Benchmark]
    public void WriteBinaryDirect()
    {
      BinaryTagWriter binaryWriter = new BinaryTagWriter(new MemoryStream());

      binaryWriter.WriteStartDocument();
      binaryWriter.WriteStartTag(TagType.Compound, "Level");
      binaryWriter.WriteTag("longTest", 9223372036854775807);
      binaryWriter.WriteTag("shortTest", (short)32767);
      binaryWriter.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      binaryWriter.WriteTag("floatTest", (float)0.498231471);
      binaryWriter.WriteTag("intTest", 2147483647);
      binaryWriter.WriteStartTag(TagType.Compound, "nested compound test");
      binaryWriter.WriteStartTag(TagType.Compound, "ham");
      binaryWriter.WriteTag("name", "Hampus");
      binaryWriter.WriteTag("value", 0.75F);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteStartTag(TagType.Compound, "egg");
      binaryWriter.WriteTag("name", "Eggbert");
      binaryWriter.WriteTag("value", 0.5F);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteEndTag();
      binaryWriter.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
      binaryWriter.WriteTag((long)11);
      binaryWriter.WriteTag((long)12);
      binaryWriter.WriteTag((long)13);
      binaryWriter.WriteTag((long)14);
      binaryWriter.WriteTag((long)15);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
      binaryWriter.WriteStartTag(TagType.Compound);
      binaryWriter.WriteTag("name", "Compound tag #0");
      binaryWriter.WriteTag("created-on", 1264099775885);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteStartTag(TagType.Compound);
      binaryWriter.WriteTag("name", "Compound tag #1");
      binaryWriter.WriteTag("created-on", 1264099775885);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteEndTag();
      binaryWriter.WriteTag("byteTest", (byte)127);
      binaryWriter.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      binaryWriter.WriteTag("doubleTest", 0.49312871321823148);
      binaryWriter.WriteEndTag();
      binaryWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WriteBinaryDocument()
    {
      TagCompound root;
      TagCompound compound;
      TagCompound child;
      TagList list;

      root = new TagCompound();
      root.Name = "Level";
      root.Value.Add("longTest", 9223372036854775807);
      root.Value.Add("shortTest", (short)32767);
      root.Value.Add("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      root.Value.Add("floatTest", (float)0.498231471);
      root.Value.Add("intTest", 2147483647);

      compound = (TagCompound)root.Value.Add("nested compound test", TagType.Compound);
      child = (TagCompound)compound.Value.Add("ham", TagType.Compound);
      child.Value.Add("name", "Hampus");
      child.Value.Add("value", (float)0.75);
      child = (TagCompound)compound.Value.Add("egg", TagType.Compound);
      child.Value.Add("name", "Eggbert");
      child.Value.Add("value", (float)0.5);

      list = (TagList)root.Value.Add("listTest (long)", TagType.List, TagType.Long);
      list.Value.Add((long)11);
      list.Value.Add((long)12);
      list.Value.Add((long)13);
      list.Value.Add((long)14);
      list.Value.Add((long)15);

      list = (TagList)root.Value.Add("listTest (compound)", TagType.List, TagType.Compound);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #0");
      child.Value.Add("created-on", 1264099775885);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #1");
      child.Value.Add("created-on", 1264099775885);

      root.Value.Add("byteTest", (byte)127);
      root.Value.Add("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      root.Value.Add("doubleTest", 0.49312871321823148);

      BinaryTagWriter binaryWriter = new BinaryTagWriter(new MemoryStream());

      binaryWriter.WriteStartDocument();
      binaryWriter.WriteTag(root);
      binaryWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WritePredefinedBinaryDocument()
    {
      BinaryTagWriter binaryWriter = new BinaryTagWriter(new MemoryStream());

      binaryWriter.WriteStartDocument();
      binaryWriter.WriteTag(_predefined);
      binaryWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WritePredefinedXmlDocument()
    {
      XmlTagWriter xmlWriter = new XmlTagWriter(new MemoryStream());

      xmlWriter.WriteStartDocument();
      xmlWriter.WriteTag(_predefined);
      xmlWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WriteXmlDirect()
    {
      XmlTagWriter xmlWriter = new XmlTagWriter(new MemoryStream());

      xmlWriter.WriteStartDocument();
      xmlWriter.WriteStartTag(TagType.Compound, "Level");
      xmlWriter.WriteTag("longTest", 9223372036854775807);
      xmlWriter.WriteTag("shortTest", (short)32767);
      xmlWriter.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      xmlWriter.WriteTag("floatTest", (float)0.498231471);
      xmlWriter.WriteTag("intTest", 2147483647);
      xmlWriter.WriteStartTag(TagType.Compound, "nested compound test");
      xmlWriter.WriteStartTag(TagType.Compound, "ham");
      xmlWriter.WriteTag("name", "Hampus");
      xmlWriter.WriteTag("value", 0.75F);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteStartTag(TagType.Compound, "egg");
      xmlWriter.WriteTag("name", "Eggbert");
      xmlWriter.WriteTag("value", 0.5F);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteEndTag();
      xmlWriter.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
      xmlWriter.WriteTag((long)11);
      xmlWriter.WriteTag((long)12);
      xmlWriter.WriteTag((long)13);
      xmlWriter.WriteTag((long)14);
      xmlWriter.WriteTag((long)15);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
      xmlWriter.WriteStartTag(TagType.Compound);
      xmlWriter.WriteTag("name", "Compound tag #0");
      xmlWriter.WriteTag("created-on", 1264099775885);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteStartTag(TagType.Compound);
      xmlWriter.WriteTag("name", "Compound tag #1");
      xmlWriter.WriteTag("created-on", 1264099775885);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteEndTag();
      xmlWriter.WriteTag("byteTest", (byte)127);
      xmlWriter.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      xmlWriter.WriteTag("doubleTest", 0.49312871321823148);
      xmlWriter.WriteEndTag();
      xmlWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WriteXmlDocument()
    {
      TagCompound root;
      TagCompound compound;
      TagCompound child;
      TagList list;

      root = new TagCompound();
      root.Name = "Level";
      root.Value.Add("longTest", 9223372036854775807);
      root.Value.Add("shortTest", (short)32767);
      root.Value.Add("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      root.Value.Add("floatTest", (float)0.498231471);
      root.Value.Add("intTest", 2147483647);

      compound = (TagCompound)root.Value.Add("nested compound test", TagType.Compound);
      child = (TagCompound)compound.Value.Add("ham", TagType.Compound);
      child.Value.Add("name", "Hampus");
      child.Value.Add("value", (float)0.75);
      child = (TagCompound)compound.Value.Add("egg", TagType.Compound);
      child.Value.Add("name", "Eggbert");
      child.Value.Add("value", (float)0.5);

      list = (TagList)root.Value.Add("listTest (long)", TagType.List, TagType.Long);
      list.Value.Add((long)11);
      list.Value.Add((long)12);
      list.Value.Add((long)13);
      list.Value.Add((long)14);
      list.Value.Add((long)15);

      list = (TagList)root.Value.Add("listTest (compound)", TagType.List, TagType.Compound);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #0");
      child.Value.Add("created-on", 1264099775885);
      child = (TagCompound)list.Value.Add(TagType.Compound);
      child.Value.Add("name", "Compound tag #1");
      child.Value.Add("created-on", 1264099775885);

      root.Value.Add("byteTest", (byte)127);
      root.Value.Add("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      root.Value.Add("doubleTest", 0.49312871321823148);

      XmlTagWriter xmlWriter = new XmlTagWriter(new MemoryStream());

      xmlWriter.WriteStartDocument();
      xmlWriter.WriteTag(root);
      xmlWriter.WriteEndDocument();
    }

    #endregion
  }
}
