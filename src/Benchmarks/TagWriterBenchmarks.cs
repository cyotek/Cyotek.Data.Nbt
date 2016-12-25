using System.IO;
using System.IO.MemoryMappedFiles;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;

namespace Benchmarks
{
  [MemoryDiagnoser]
  //[InliningDiagnoser]
  public class TagWriterBenchmarks : BenchmarksBase
  {
    #region Constants

//    private readonly TagWriter _binaryWriter;

    private readonly TagCompound _predefined;

//    private readonly MemoryStream _stream;

//    private readonly TagWriter _xmlWriter;

    #endregion

    #region Constructors

    public TagWriterBenchmarks()
    {
      //_stream = new MemoryStream();
      //_xmlWriter = new XmlTagWriter(_stream);
  //    _binaryWriter = new BinaryTagWriter(_stream);
      _predefined = this.CreateComplexData();
    }

    #endregion

    #region Methods

    [Benchmark]
    public void WriteBinaryDirect()
    {
      BinaryTagWriter _binaryWriter = new BinaryTagWriter(new MemoryStream());

      _binaryWriter.WriteStartDocument();
      _binaryWriter.WriteStartTag(TagType.Compound, "Level");
      _binaryWriter.WriteTag("longTest", 9223372036854775807);
      _binaryWriter.WriteTag("shortTest", (short)32767);
      _binaryWriter.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      _binaryWriter.WriteTag("floatTest", (float)0.498231471);
      _binaryWriter.WriteTag("intTest", 2147483647);
      _binaryWriter.WriteStartTag(TagType.Compound, "nested compound test");
      _binaryWriter.WriteStartTag(TagType.Compound, "ham");
      _binaryWriter.WriteTag("name", "Hampus");
      _binaryWriter.WriteTag("value", 0.75F);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteStartTag(TagType.Compound, "egg");
      _binaryWriter.WriteTag("name", "Eggbert");
      _binaryWriter.WriteTag("value", 0.5F);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
      _binaryWriter.WriteTag((long)11);
      _binaryWriter.WriteTag((long)12);
      _binaryWriter.WriteTag((long)13);
      _binaryWriter.WriteTag((long)14);
      _binaryWriter.WriteTag((long)15);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
      _binaryWriter.WriteStartTag(TagType.Compound, string.Empty, WriteTagOptions.IgnoreName);
      _binaryWriter.WriteTag("name", "Compound tag #0");
      _binaryWriter.WriteTag("created-on", 1264099775885);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteStartTag(TagType.Compound, string.Empty, WriteTagOptions.IgnoreName);
      _binaryWriter.WriteTag("name", "Compound tag #1");
      _binaryWriter.WriteTag("created-on", 1264099775885);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteTag("byteTest", (byte)127);
      _binaryWriter.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      _binaryWriter.WriteTag("doubleTest", 0.49312871321823148);
      _binaryWriter.WriteEndTag();
      _binaryWriter.WriteEndDocument();
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

      BinaryTagWriter _binaryWriter = new BinaryTagWriter(new MemoryStream());

      _binaryWriter.WriteStartDocument();
      _binaryWriter.WriteTag(root);
      _binaryWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WritePredefinedBinaryDocument()
    {
      BinaryTagWriter _binaryWriter = new BinaryTagWriter(new MemoryStream());

      _binaryWriter.WriteStartDocument();
      _binaryWriter.WriteTag(_predefined);
      _binaryWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WritePredefinedXmlDocument()
    {
      XmlTagWriter _xmlWriter = new XmlTagWriter(new MemoryStream());

      _xmlWriter.WriteStartDocument();
      _xmlWriter.WriteTag(_predefined);
      _xmlWriter.WriteEndDocument();
    }

    [Benchmark]
    public void WriteXmlDirect()
    {
      XmlTagWriter _xmlWriter = new XmlTagWriter(new MemoryStream());

      _xmlWriter.WriteStartDocument();
      _xmlWriter.WriteStartTag(TagType.Compound, "Level");
      _xmlWriter.WriteTag("longTest", 9223372036854775807);
      _xmlWriter.WriteTag("shortTest", (short)32767);
      _xmlWriter.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
      _xmlWriter.WriteTag("floatTest", (float)0.498231471);
      _xmlWriter.WriteTag("intTest", 2147483647);
      _xmlWriter.WriteStartTag(TagType.Compound, "nested compound test");
      _xmlWriter.WriteStartTag(TagType.Compound, "ham");
      _xmlWriter.WriteTag("name", "Hampus");
      _xmlWriter.WriteTag("value", 0.75F);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteStartTag(TagType.Compound, "egg");
      _xmlWriter.WriteTag("name", "Eggbert");
      _xmlWriter.WriteTag("value", 0.5F);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
      _xmlWriter.WriteTag((long)11);
      _xmlWriter.WriteTag((long)12);
      _xmlWriter.WriteTag((long)13);
      _xmlWriter.WriteTag((long)14);
      _xmlWriter.WriteTag((long)15);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
      _xmlWriter.WriteStartTag(TagType.Compound, string.Empty, WriteTagOptions.IgnoreName);
      _xmlWriter.WriteTag("name", "Compound tag #0");
      _xmlWriter.WriteTag("created-on", 1264099775885);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteStartTag(TagType.Compound, string.Empty, WriteTagOptions.IgnoreName);
      _xmlWriter.WriteTag("name", "Compound tag #1");
      _xmlWriter.WriteTag("created-on", 1264099775885);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteTag("byteTest", (byte)127);
      _xmlWriter.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
      _xmlWriter.WriteTag("doubleTest", 0.49312871321823148);
      _xmlWriter.WriteEndTag();
      _xmlWriter.WriteEndDocument();
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

      XmlTagWriter _xmlWriter = new XmlTagWriter(new MemoryStream());

      _xmlWriter.WriteStartDocument();
      _xmlWriter.WriteTag(root);
      _xmlWriter.WriteEndDocument();
    }

    #endregion
  }
}
