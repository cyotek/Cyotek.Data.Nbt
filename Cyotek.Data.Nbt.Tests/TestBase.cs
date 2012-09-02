using System;
using System.IO;

namespace Cyotek.Data.Nbt.Tests
{
  internal class TestBase
  {
    protected string AnvilRegionFileName
    { get { return Path.Combine(this.DataPath, "r.0.0.mca"); } }

    protected string ComplexDataFileName
    { get { return Path.Combine(this.DataPath, "bigtest.nbt"); } }

    protected string DataPath
    { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"); } }

    protected string OutputFileName
    { get { return Path.Combine(this.DataPath, "savetest.raw"); } }

    protected string SimpleDataFileName
    { get { return Path.Combine(this.DataPath, "test.nbt"); } }

    protected TagCompound GetComplexData()
    {
      return (TagCompound)Tag.ReadFromFile(this.ComplexDataFileName);
    }

    protected TagCompound GetSimpleData()
    {
      return (TagCompound)Tag.ReadFromFile(this.SimpleDataFileName);
    }
  }
}