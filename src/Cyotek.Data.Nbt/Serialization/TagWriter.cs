using System;
using System.ComponentModel;
using System.IO;

namespace Cyotek.Data.Nbt.Serialization
{
  public abstract class TagWriter : IDisposable
  {
    internal static readonly bool IsLittleEndian;


    public virtual void Close()
    {

    }

    public abstract void Flush();



    /// <summary>
    /// Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
    [Browsable(false)]
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);

      GC.SuppressFinalize(this);
    }



    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
      {
        this.IsDisposed = true;
      }
    }

    static TagWriter()
    {
      IsLittleEndian = BitConverter.IsLittleEndian;
    }

    public virtual void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteDocument(Stream stream, TagCompound tag)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteTag(ITag tag)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteTag(ITag tag, WriteTagOptions options)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(byte value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(byte[] value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(double value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(short value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(int value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(int[] value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(long value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(float value)
    {
      throw new NotImplementedException();
    }

    public virtual void WriteValue(string value)
    {
      throw new NotImplementedException();
    }
  }
}
