Cyotek.Data.Nbt
===============

[![Build status](https://ci.appveyor.com/api/projects/status/d2l6xj7mbv5rkc92?svg=true)](https://ci.appveyor.com/project/cyotek/cyotek-data-nbt)

NBT (Named Binary Tag) is a tag based binary format designed to carry large amounts of binary data with smaller amounts of additional data. This is currently the format that Minecraft uses for player and region data.

Cyotek.Data.Nbt is a library for reading and writing NBT format files used by Minecraft. However, the format is versatile enough to use for many other applications and purposes.

It was originally based on [LibNBT](http://libnbt.codeplex.com/) found on CodePlex, but I've made a lot of changes to it. The API has substantially changed, although it should be easier to use than the original.

In addition to support for the native binary NBT format, this library also offers the ability to read and write NBT tags to and from XML... not quite as "binary", but certainly more readable!

Features
---------

* Support for all tags in the specification
* Reads and writes binary files compatible with existing NBT libraries and tools
* Supports reading and writing to XML based files for human readable output (deprecated)
* Ability to provide custom readers/writers if required
* Extended API for working with NBT documents
* Query support

Saving a document
-----------------

Similar to `XmlDocument` and `XmlWriter`, you can either directly write NBT documents, or you can construct a document then save it. The former approach is the fastest, the latter approach may be simpler.

### Using the BinaryTagWriter to create a document

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
      
### Using NbtDocument to construct and save a document

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

Once you have a document or root `TagCompound`, you can either use the `NbtDocument` class to save the document, or use a `TagWriter` class directly.

#### Using a TagWriter class

      using (BinaryTagWriter writer = new BinaryTagWriter(stream)) // Or XmlTagWriter
      {
        writer.WriteStartDocument();
        writer.WriteTag(document.DocumentRoot);
        writer.WriteEndDocument();
      }

#### Using NbtDocument

      document.Save(stream);

See the Benchmarks project, or the test suite for examples of the different ways of serializing document.s

Using Binary or XML Formats
---------------------------

The library supports both the use of the binary NBT format, and a variant that uses XML. While the XML version is much more readable, it is also much slower to serialize and isn't really recommended for production use.

The `Load` methods of `NbtDocument` class will automatically detect if the source is binary or XML, allowing seamless use. However, if you use serialization classes directly you will need to perform your own detection and construct an `XmlTagReader` or `BinaryTagReader` object accordingly. The static `NbtDocument.GetDocumentFormat` method can help with format detection.  

The following table was generated by running the write benchmarks using [BenchmarkDotNet ](http://benchmarkdotnet.org/) and clearly show the difference between writing XML and writing binary.

|                             Method |        Mean |    StdErr |     StdDev |      Median |   Gen 0 | Allocated |
| ---------------------------------- |------------ |---------- |----------- |------------ |-------- |---------- |
|                  WriteBinaryDirect |   8.4448 us | 0.0836 us |  0.5733 us |   8.1153 us |  3.4424 |   6.67 kB |
|                WriteBinaryDocument |  16.6214 us | 0.1641 us |  0.7337 us |  16.1428 us |  5.4867 |  10.12 kB |
|  WriteBinaryDocumentViaNbtDocument |  16.8629 us | 0.1667 us |  1.2696 us |  16.0460 us |  5.6095 |  10.14 kB |
|      WritePredefinedBinaryDocument |   9.3312 us | 0.0808 us |  0.3129 us |   9.1901 us |  3.6070 |   6.86 kB |
|         WritePredefinedXmlDocument | 180.8340 us | 1.8036 us | 10.8217 us | 177.5857 us | 23.9909 |  50.56 kB |
|                     WriteXmlDirect | 172.9066 us | 1.8421 us | 10.2566 us | 167.6837 us | 24.0885 |  50.37 kB |
|                   WriteXmlDocument | 180.4061 us | 0.6515 us |  2.2568 us | 179.7206 us | 29.5038 |   53.8 kB |
|     WriteXmlDocumentViaNbtDocument | 187.5790 us | 1.8653 us | 12.7877 us | 182.9234 us | 28.5127 |  53.82 kB |

In closing, XML support will probably be removed in the next major version of the library and is deprecated in the current.

License
-------

As per the original library, this source is licensed under the [GNU Lesser General Public License, version 2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html). For more information, see [COPYING.txt](COPYING.txt)
