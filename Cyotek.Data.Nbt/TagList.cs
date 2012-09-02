using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.NtbNullEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagList : Tag, ITagCollection
  {
    private TagType _listType;

    public TagList()
    {
      this.Name = string.Empty;
      this.Value = new TagCollection(this);
      this.ListType = TagType.None;
    }

    public TagList(string name)
      : this(name, TagType.None)
    { }

    public TagList(TagType listType)
      : this(string.Empty, listType)
    { }

    public TagList(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadTagList(this, input);
    }

    public TagList(string name, TagType listType)
      : this()
    {
      this.Name = name;
      this.ListType = listType;
    }

    bool ITagCollection.IsList
    { get { return true; } }

    TagType ITagCollection.LimitToType
    {
      get { return this.ListType; }
      set { this.ListType = value; }
    }

    IList<ITag> ITagCollection.Values
    {
      get { return this.Value; }
    }

    public int Count
    { get { return this.Value.Count; } }

    public virtual TagType ListType
    {
      get { return _listType; }
      set
      {
        _listType = value;
        this.Value = new TagCollection(this, value);
      }
    }

    public override TagType Type
    {
      get { return TagType.List; }
    }

    public new TagCollection Value
    {
      get { return (TagCollection)base.Value; }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");

        base.Value = value;
      }
    }

    public override string ToString(string indentString)
    {
      StringBuilder results;

      results = new StringBuilder();

      if (this.Value.Count == 0)
        results.AppendFormat("{0}[List: {1}]", indentString, Name);
      else
      {
        results.AppendLine(string.Format("{0}[List: {1}", indentString, Name));

        foreach (ITag item in this.Value)
          results.AppendLine(string.Format("{0}  {1}", indentString, item.ToString(indentString + "  ").Trim()));

        results.AppendLine(string.Format("{0}]", indentString));
      }

      return results.ToString();
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      this.Value.WriteList(output);
    }

    public override void WriteUnnamed(Stream output)
    {
      this.Value.WriteList(output);
    }

    internal static TagCollection ReadTagList(TagList owner, Stream input)
    {
      TagCollection tags;
      int length;

      owner.ListType = (TagType)TagByte.ReadByte(input);
      tags = new TagCollection(owner, owner.ListType);

      length = TagInt.ReadInt(input);

      for (int i = 0; i < length; i++)
      {
        ITag tag;

        switch (owner.ListType)
        {
          case TagType.Byte:
            tag = TagByte.ReadUnnamedTagByte(input);
            break;

          case TagType.ByteArray:
            tag = TagByteArray.ReadUnnamedTagByteArray(input);
            break;

          case TagType.Compound:
            tag = TagCompound.ReadUnnamedTagCompound(input);
            break;

          case TagType.Double:
            tag = TagDouble.ReadUnnamedTagDouble(input);
            break;

          case TagType.End:
            tag = new TagEnd();
            break;

          case TagType.Float:
            tag = TagFloat.ReadUnnamedTagFloat(input);
            break;

          case TagType.Int:
            tag = TagInt.ReadUnnamedTagInt(input);
            break;

          case TagType.IntArray:
            tag = TagIntArray.ReadUnnamedTagIntArray(input);
            break;

          case TagType.List:
            tag = TagList.ReadUnnamedTagList(input);
            break;

          case TagType.Long:
            tag = TagLong.ReadUnnamedTagLong(input);
            break;

          case TagType.Short:
            tag = TagShort.ReadUnnamedTagShort(input);
            break;

          case TagType.String:
            tag = TagString.ReadUnnamedTagString(input);
            break;
          default:
            tag = null;
            break;
        }

        if (tag != null)
        {
          tags.Add(tag);
        }
      }

      return tags;
    }

    internal static ITag ReadUnnamedTagList(Stream input)
    {
      TagList result;

      result = new TagList();
      result.Value = TagList.ReadTagList(result, input);

      return result;
    }
  }
}