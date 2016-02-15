using System;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;

namespace Cyotek.Data.Nbt.Tests
{
  [TestFixture]
  public class AnvilRegionTests : TestBase
  {
    #region Tests

    [Test]
    public void TestAnvilRegion()
    {
      string filename = this.AnvilRegionFileName;
      FileStream input = File.OpenRead(filename);
      int[] locations = new int[1024];
      byte[] buffer = new byte[4096];
      input.Read(buffer, 0, 4096);
      for (int i = 0; i < 1024; i++)
      {
        locations[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      int[] timestamps = new int[1024];
      input.Read(buffer, 0, 4096);
      for (int i = 0; i < 1024; i++)
      {
        timestamps[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      input.Read(buffer, 0, 4);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(buffer, 0, 4);
      }
      int sizeOfChunkData = BitConverter.ToInt32(buffer, 0) - 1;

      int compressionType = input.ReadByte();
      buffer = new byte[sizeOfChunkData];
      input.Read(buffer, 0, sizeOfChunkData);

      Stream inputStream = null;

      if (compressionType == 1)
      {
        inputStream = new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress);
      }
      else if (compressionType == 2)
      {
        inputStream = new DeflateStream(new MemoryStream(buffer, 2, buffer.Length - 6), CompressionMode.Decompress);
      }

      ITagReader reader;
      reader = new BinaryTagReader(inputStream);
      TagCompound tag = (TagCompound)reader.ReadTag();
      string strTag = tag.ToString();

      Assert.IsNotNull(tag);

      Assert.AreEqual(TagType.Compound, tag.GetTag("Level").Type);
      TagCompound levelTag = tag.GetCompound("Level");

      ITag aTag = levelTag.GetTag("Entities");
      Assert.AreEqual(TagType.List, aTag.Type);
      TagList entitiesTag = aTag as TagList;
      Assert.AreEqual(0, entitiesTag.Value.Count);

      aTag = levelTag.GetTag("Biomes");
      Assert.AreEqual(TagType.ByteArray, aTag.Type);
      TagByteArray biomesTag = aTag as TagByteArray;
      Assert.AreEqual(256, biomesTag.Value.Length);

      aTag = levelTag.GetTag("LastUpdate");
      Assert.AreEqual(TagType.Long, aTag.Type);
      TagLong lastUpdateTag = aTag as TagLong;
      Assert.AreEqual(2861877, lastUpdateTag.Value);

      aTag = levelTag.GetTag("xPos");
      Assert.AreEqual(TagType.Int, aTag.Type);
      TagInt xPosTag = aTag as TagInt;
      Assert.AreEqual(10, xPosTag.Value);

      aTag = levelTag.GetTag("zPos");
      Assert.AreEqual(TagType.Int, aTag.Type);
      TagInt zPosTag = aTag as TagInt;
      Assert.AreEqual(0, zPosTag.Value);

      aTag = levelTag.GetTag("TileEntities");
      Assert.AreEqual(TagType.List, aTag.Type);
      TagList tileEntitiesTag = aTag as TagList;
      Assert.AreEqual(0, tileEntitiesTag.Value.Count);

      aTag = levelTag.GetTag("TerrainPopulated");
      Assert.AreEqual(TagType.Byte, aTag.Type);
      TagByte terrainPopulatedTag = aTag as TagByte;
      Assert.AreEqual(1, terrainPopulatedTag.Value);

      aTag = levelTag.GetTag("HeightMap");
      Assert.AreEqual(TagType.IntArray, aTag.Type);
      TagIntArray heightmapTag = aTag as TagIntArray;
      Assert.AreEqual(256, heightmapTag.Value.Length);

      aTag = levelTag.GetTag("Sections");
      Assert.AreEqual(TagType.List, aTag.Type);
      TagList sectionsTag = aTag as TagList;
      Assert.AreEqual(4, sectionsTag.Value.Count);

      TagCompound section_0 = sectionsTag.Value[0] as TagCompound;
      Assert.IsNotNull(section_0);
      TagByteArray section_0_data = section_0.GetByteArray("Data");
      Assert.IsNotNull(section_0_data);
      Assert.AreEqual(2048, section_0_data.Value.Length);
      TagByteArray section_0_skyLight = section_0.GetByteArray("SkyLight");
      Assert.IsNotNull(section_0_skyLight);
      Assert.AreEqual(2048, section_0_skyLight.Value.Length);
      TagByteArray section_0_blockLight = section_0.GetByteArray("BlockLight");
      Assert.IsNotNull(section_0_blockLight);
      Assert.AreEqual(2048, section_0_blockLight.Value.Length);
      TagByte section_0_y = section_0.GetByte("Y");
      Assert.IsNotNull(section_0_y);
      Assert.AreEqual(0, section_0_y.Value);
      TagByteArray section_0_blocks = section_0.GetByteArray("Blocks");
      Assert.IsNotNull(section_0_blocks);
      Assert.AreEqual(4096, section_0_blocks.Value.Length);

      TagCompound section_1 = sectionsTag.Value[1] as TagCompound;
      Assert.IsNotNull(section_1);
      TagByteArray section_1_data = section_1.GetByteArray("Data");
      Assert.IsNotNull(section_1_data);
      Assert.AreEqual(2048, section_1_data.Value.Length);
      TagByteArray section_1_skyLight = section_1.GetByteArray("SkyLight");
      Assert.IsNotNull(section_1_skyLight);
      Assert.AreEqual(2048, section_1_skyLight.Value.Length);
      TagByteArray section_1_blockLight = section_1.GetByteArray("BlockLight");
      Assert.IsNotNull(section_1_blockLight);
      Assert.AreEqual(2048, section_1_blockLight.Value.Length);
      TagByte section_1_y = section_1.GetByte("Y");
      Assert.IsNotNull(section_1_y);
      Assert.AreEqual(1, section_1_y.Value);
      TagByteArray section_1_blocks = section_1.GetByteArray("Blocks");
      Assert.IsNotNull(section_1_blocks);
      Assert.AreEqual(4096, section_1_blocks.Value.Length);

      TagCompound section_2 = sectionsTag.Value[2] as TagCompound;
      Assert.IsNotNull(section_2);
      TagByteArray section_2_data = section_2.GetByteArray("Data");
      Assert.IsNotNull(section_2_data);
      Assert.AreEqual(2048, section_2_data.Value.Length);
      TagByteArray section_2_skyLight = section_2.GetByteArray("SkyLight");
      Assert.IsNotNull(section_2_skyLight);
      Assert.AreEqual(2048, section_2_skyLight.Value.Length);
      TagByteArray section_2_blockLight = section_2.GetByteArray("BlockLight");
      Assert.IsNotNull(section_2_blockLight);
      Assert.AreEqual(2048, section_2_blockLight.Value.Length);
      TagByte section_2_y = section_2.GetByte("Y");
      Assert.IsNotNull(section_2_y);
      Assert.AreEqual(2, section_2_y.Value);
      TagByteArray section_2_blocks = section_2.GetByteArray("Blocks");
      Assert.IsNotNull(section_2_blocks);
      Assert.AreEqual(4096, section_2_blocks.Value.Length);

      TagCompound section_3 = sectionsTag.Value[3] as TagCompound;
      Assert.IsNotNull(section_3);
      TagByteArray section_3_data = section_3.GetByteArray("Data");
      Assert.IsNotNull(section_3_data);
      Assert.AreEqual(2048, section_3_data.Value.Length);
      TagByteArray section_3_skyLight = section_3.GetByteArray("SkyLight");
      Assert.IsNotNull(section_3_skyLight);
      Assert.AreEqual(2048, section_3_skyLight.Value.Length);
      TagByteArray section_3_blockLight = section_3.GetByteArray("BlockLight");
      Assert.IsNotNull(section_3_blockLight);
      Assert.AreEqual(2048, section_3_blockLight.Value.Length);
      TagByte section_3_y = section_3.GetByte("Y");
      Assert.IsNotNull(section_3_y);
      Assert.AreEqual(3, section_3_y.Value);
      TagByteArray section_3_blocks = section_3.GetByteArray("Blocks");
      Assert.IsNotNull(section_3_blocks);
      Assert.AreEqual(4096, section_3_blocks.Value.Length);
    }

    #endregion
  }
}
