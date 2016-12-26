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
      using (Stream stream = new MemoryStream())
      {
        using (TagWriter writer = new BinaryTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteStartTag(TagType.Compound, "Level");
          writer.WriteTag("longTest", 9223372036854775807);
          writer.WriteTag("shortTest", (short)32767);
          writer.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
          writer.WriteTag("floatTest", (float)0.498231471);
          writer.WriteTag("intTest", 2147483647);
          writer.WriteStartTag(TagType.Compound, "nested compound test");
          writer.WriteStartTag(TagType.Compound, "ham");
          writer.WriteTag("name", "Hampus");
          writer.WriteTag("value", 0.75F);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.Compound, "egg");
          writer.WriteTag("name", "Eggbert");
          writer.WriteTag("value", 0.5F);
          writer.WriteEndTag();
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
          writer.WriteTag((long)11);
          writer.WriteTag((long)12);
          writer.WriteTag((long)13);
          writer.WriteTag((long)14);
          writer.WriteTag((long)15);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
          writer.WriteStartTag(TagType.Compound);
          writer.WriteTag("name", "Compound tag #0");
          writer.WriteTag("created-on", 1264099775885);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.Compound);
          writer.WriteTag("name", "Compound tag #1");
          writer.WriteTag("created-on", 1264099775885);
          writer.WriteEndTag();
          writer.WriteEndTag();
          writer.WriteTag("byteTest", (byte)127);
          writer.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
          writer.WriteTag("doubleTest", 0.49312871321823148);
          writer.WriteEndTag();
          writer.WriteEndDocument();
        }
      }
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

      using (Stream stream = new MemoryStream())
      {
        using (BinaryTagWriter writer = new BinaryTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteTag(root);
          writer.WriteEndDocument();
        }
      }
    }

    [Benchmark]
    public void WriteBinaryDocumentViaNbtDocument()
    {
      NbtDocument document;
      TagCompound root;
      TagCompound compound;
      TagCompound child;
      TagList list;

      document = new NbtDocument();

      root = document.DocumentRoot;
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

      using (Stream stream = new MemoryStream())
      {
        document.Format = NbtFormat.Binary;
        document.Save(stream);
      }
    }

    [Benchmark]
    public void WritePredefinedBinaryDocument()
    {
      using (Stream stream = new MemoryStream())
      {
        using (BinaryTagWriter writer = new BinaryTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteTag(_predefined);
          writer.WriteEndDocument();
        }
      }
    }

    [Benchmark]
    public void WritePredefinedXmlDocument()
    {
      using (Stream stream = new MemoryStream())
      {
        using (XmlTagWriter writer = new XmlTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteTag(_predefined);
          writer.WriteEndDocument();
        }
      }
    }

    [Benchmark]
    public void WriteXmlDirect()
    {
      using (Stream stream = new MemoryStream())
      {
        using (XmlTagWriter writer = new XmlTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteStartTag(TagType.Compound, "Level");
          writer.WriteTag("longTest", 9223372036854775807);
          writer.WriteTag("shortTest", (short)32767);
          writer.WriteTag("stringTest", "HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
          writer.WriteTag("floatTest", (float)0.498231471);
          writer.WriteTag("intTest", 2147483647);
          writer.WriteStartTag(TagType.Compound, "nested compound test");
          writer.WriteStartTag(TagType.Compound, "ham");
          writer.WriteTag("name", "Hampus");
          writer.WriteTag("value", 0.75F);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.Compound, "egg");
          writer.WriteTag("name", "Eggbert");
          writer.WriteTag("value", 0.5F);
          writer.WriteEndTag();
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.List, "listTest (long)", TagType.Long, 5);
          writer.WriteTag((long)11);
          writer.WriteTag((long)12);
          writer.WriteTag((long)13);
          writer.WriteTag((long)14);
          writer.WriteTag((long)15);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.List, "listTest (compound)", TagType.Compound, 2);
          writer.WriteStartTag(TagType.Compound);
          writer.WriteTag("name", "Compound tag #0");
          writer.WriteTag("created-on", 1264099775885);
          writer.WriteEndTag();
          writer.WriteStartTag(TagType.Compound);
          writer.WriteTag("name", "Compound tag #1");
          writer.WriteTag("created-on", 1264099775885);
          writer.WriteEndTag();
          writer.WriteEndTag();
          writer.WriteTag("byteTest", (byte)127);
          writer.WriteTag("byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))", SampleByteArray);
          writer.WriteTag("doubleTest", 0.49312871321823148);
          writer.WriteEndTag();
          writer.WriteEndDocument();
        }
      }
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

      using (Stream stream = new MemoryStream())
      {
        using (XmlTagWriter writer = new XmlTagWriter(stream))
        {
          writer.WriteStartDocument();
          writer.WriteTag(root);
          writer.WriteEndDocument();
        }
      }
    }

    [Benchmark]
    public void WriteXmlDocumentViaNbtDocument()
    {
      NbtDocument document;
      TagCompound root;
      TagCompound compound;
      TagCompound child;
      TagList list;

      document = new NbtDocument();

      root = document.DocumentRoot;
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

      using (Stream stream = new MemoryStream())
      {
        document.Format = NbtFormat.Xml;
        document.Save(stream);
      }
    }

    #endregion
  }
}
