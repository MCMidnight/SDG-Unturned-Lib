using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	/// <summary>
	/// Legacy implementation of foliage storage, with one file per tile.
	/// </summary>
	// Token: 0x020000F4 RID: 244
	public class FoliageStorageV1 : IFoliageStorage
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x00016C4D File Offset: 0x00014E4D
		public void Initialize()
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00016C4F File Offset: 0x00014E4F
		public void Shutdown()
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00016C51 File Offset: 0x00014E51
		public void TileBecameRelevantToViewer(FoliageTile tile)
		{
			if (!this.hasAllTilesInMemory)
			{
				this.pendingLoad.AddLast(tile);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00016C68 File Offset: 0x00014E68
		public void TileNoLongerRelevantToViewer(FoliageTile tile)
		{
			if (!this.hasAllTilesInMemory)
			{
				this.pendingLoad.Remove(tile);
				tile.clearAndReleaseInstances();
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016C88 File Offset: 0x00014E88
		public void Update()
		{
			if (this.pendingLoad.Count > 0)
			{
				FoliageTile value = this.pendingLoad.First.Value;
				this.pendingLoad.RemoveFirst();
				this.readInstances(value);
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016CC8 File Offset: 0x00014EC8
		public void EditorLoadAllTiles(IEnumerable<FoliageTile> tiles)
		{
			this.hasAllTilesInMemory = true;
			foreach (FoliageTile tile in tiles)
			{
				this.readInstances(tile);
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016D18 File Offset: 0x00014F18
		public void EditorSaveAllTiles(IEnumerable<FoliageTile> tiles)
		{
			foreach (FoliageTile foliageTile in tiles)
			{
				if (foliageTile.hasUnsavedChanges)
				{
					foliageTile.hasUnsavedChanges = false;
					this.writeInstances(foliageTile);
				}
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00016D70 File Offset: 0x00014F70
		protected string formatTilePath(FoliageTile tile)
		{
			FoliageCoord coord = tile.coord;
			string text = coord.x.ToString(CultureInfo.InvariantCulture);
			coord = tile.coord;
			string text2 = coord.y.ToString(CultureInfo.InvariantCulture);
			return string.Concat(new string[]
			{
				Level.info.path,
				"/Foliage/Tile_",
				text,
				"_",
				text2,
				".foliage"
			});
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016DE8 File Offset: 0x00014FE8
		protected void readInstances(FoliageTile tile)
		{
			string text = this.formatTilePath(tile);
			if (File.Exists(text))
			{
				using (FileStream fileStream = new FileStream(text, 3, 1, 3))
				{
					BinaryReader binaryReader = new BinaryReader(fileStream);
					int num = binaryReader.ReadInt32();
					int num2 = binaryReader.ReadInt32();
					for (int i = 0; i < num2; i++)
					{
						GuidBuffer guidBuffer = default(GuidBuffer);
						fileStream.Read(this.GUID_BUFFER, 0, 16);
						guidBuffer.Read(this.GUID_BUFFER, 0);
						AssetReference<FoliageInstancedMeshInfoAsset> assetReference = new AssetReference<FoliageInstancedMeshInfoAsset>(guidBuffer.GUID);
						FoliageInstanceList orAddList = tile.getOrAddList(assetReference);
						int num3 = binaryReader.ReadInt32();
						for (int j = 0; j < num3; j++)
						{
							Matrix4x4 newMatrix = default(Matrix4x4);
							for (int k = 0; k < 16; k++)
							{
								newMatrix[k] = binaryReader.ReadSingle();
							}
							bool newClearWhenBaked = num <= 2 || binaryReader.ReadBoolean();
							if (!tile.isInstanceCut(newMatrix.GetPosition()))
							{
								orAddList.addInstanceAppend(new FoliageInstanceGroup(assetReference, newMatrix, newClearWhenBaked));
							}
						}
					}
				}
			}
			tile.updateBounds();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00016F14 File Offset: 0x00015114
		public void writeInstances(FoliageTile tile)
		{
			string text = this.formatTilePath(tile);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(text, 4, 2, 3))
			{
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(this.FOLIAGE_FILE_VERSION);
				binaryWriter.Write(tile.instances.Count);
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in tile.instances)
				{
					GuidBuffer guidBuffer = new GuidBuffer(keyValuePair.Key.GUID);
					guidBuffer.Write(this.GUID_BUFFER, 0);
					fileStream.Write(this.GUID_BUFFER, 0, 16);
					int num = 0;
					foreach (List<Matrix4x4> list in keyValuePair.Value.matrices)
					{
						num += list.Count;
					}
					binaryWriter.Write(num);
					for (int i = 0; i < keyValuePair.Value.matrices.Count; i++)
					{
						List<Matrix4x4> list2 = keyValuePair.Value.matrices[i];
						List<bool> list3 = keyValuePair.Value.clearWhenBaked[i];
						for (int j = 0; j < list2.Count; j++)
						{
							Matrix4x4 matrix4x = list2[j];
							for (int k = 0; k < 16; k++)
							{
								binaryWriter.Write(matrix4x[k]);
							}
							bool flag = list3[j];
							binaryWriter.Write(flag);
						}
					}
				}
			}
		}

		// Token: 0x04000240 RID: 576
		protected bool hasAllTilesInMemory;

		// Token: 0x04000241 RID: 577
		protected LinkedList<FoliageTile> pendingLoad = new LinkedList<FoliageTile>();

		// Token: 0x04000242 RID: 578
		private readonly int FOLIAGE_FILE_VERSION = 3;

		// Token: 0x04000243 RID: 579
		private byte[] GUID_BUFFER = new byte[16];
	}
}
