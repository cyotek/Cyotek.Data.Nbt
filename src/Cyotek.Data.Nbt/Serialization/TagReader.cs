using System;
using System.ComponentModel;

namespace Cyotek.Data.Nbt.Serialization
{
  public abstract partial class TagReader : IDisposable
  {
    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
    [Browsable(false)]
    public bool IsDisposed { get; private set; }

    #endregion

    #region Methods

    public virtual void Close()
    { }

    public abstract bool IsNbtDocument();

    public abstract TagCompound ReadDocument();

    public abstract TagCompound ReadDocument(ReadTagOptions options);

    public Tag ReadTag()
    {
      return this.ReadTag(ReadTagOptions.None);
    }

    public abstract Tag ReadTag(ReadTagOptions options);

    public abstract string ReadTagName();

    public abstract TagType ReadTagType();

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

    #endregion

    #region IDisposable Interface

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);

      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
