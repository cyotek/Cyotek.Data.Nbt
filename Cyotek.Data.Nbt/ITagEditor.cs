namespace Cyotek.Data.Nbt
{
  public interface ITagEditor
  {
    #region Methods

    void Bind(ITag tag);

    void SaveChanges();

    #endregion
  }
}
