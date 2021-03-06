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
using System.IO;
using NUnit.Framework;
using Cyotek.Data.Nbt.Serialization;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  partial class BinaryTagReaderTests
  {
    [Test]
    public void ReadByte_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadByte());
      }
    }

    [Test]
    public void ReadShort_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadShort());
      }
    }

    [Test]
    public void ReadInt_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadInt());
      }
    }

    [Test]
    public void ReadLong_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadLong());
      }
    }

    [Test]
    public void ReadFloat_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadFloat());
      }
    }

    [Test]
    public void ReadDouble_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadDouble());
      }
    }

    [Test]
    public void ReadByteArray_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        // TODO: WriteValue is currently protected
        //writer.WriteValue(100);
        this.WriteValue(stream, 100);

        stream.Position = 0;

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadByteArray());
      }
    }

        [Test]
    public void ReadString_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        // TODO: WriteValue is currently protected
        //writer.WriteValue((short)100);
        this.WriteValue(stream, (short)100);

        stream.Position = 0;

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadString());
      }
    }

        [Test]
    public void ReadList_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadList());
      }
    }

    [Test]
    public void ReadCompound_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;

        reader = this.CreateReader(stream);

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadCompound());
      }
    }

    [Test]
    public void ReadIntArray_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        // TODO: WriteValue is currently protected
        //writer.WriteValue(100);
        this.WriteValue(stream, 100);

        stream.Position = 0;

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadIntArray());
      }
    }

        [Test]
    public void ReadLongArray_throws_exception_if_data_invalid()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        // arrange
        TagReader reader;
        TagWriter writer;

        reader = this.CreateReader(stream);
        writer = new BinaryTagWriter(stream);

        // TODO: WriteValue is currently protected
        //writer.WriteValue(100);
        this.WriteValue(stream, 100);

        stream.Position = 0;

        // act & assert
        Assert.Throws<InvalidDataException>(() => reader.ReadLongArray());
      }
    }

      }
}
