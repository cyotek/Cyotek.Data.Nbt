namespace Cyotek.Data.Nbt
{
  public interface ITagEditor
  {
    void Bind(ITag tag);

    void SaveChanges();
  }
}