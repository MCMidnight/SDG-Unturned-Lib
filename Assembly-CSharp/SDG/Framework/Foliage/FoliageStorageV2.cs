using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SDG.Framework.Devkit;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	/// <summary>
	/// Replacement foliage storage with all tiles in a single file.
	///
	/// In the level editor all tiles are loaded into memory, whereas during gameplay the relevant tiles
	/// are loaded as-needed by a worker thread.
	/// </summary>
	// Token: 0x020000F5 RID: 245
	public class FoliageStorageV2 : IFoliageStorage
	{
		// Token: 0x0600061B RID: 1563 RVA: 0x00017148 File Offset: 0x00015348
		public void Initialize()
		{
			this.tileBlobOffsets.Clear();
			this.tileBlobHeaderOffset = 0L;
			this.assetsHeader.Clear();
			this.loadedFileVersion = 0;
			string text = Level.info.path + "/Foliage.blob";
			if (File.Exists(text))
			{
				this.readerStream = new FileStream(text, 3, 1, 3);
				this.reader = new BinaryReader(this.readerStream);
				using (SHA1Stream sha1Stream = new SHA1Stream(this.readerStream))
				{
					using (BinaryReader binaryReader = new BinaryReader(sha1Stream))
					{
						this.loadedFileVersion = binaryReader.ReadInt32();
						int num = binaryReader.ReadInt32();
						UnturnedLog.info("Found {0} foliage v2 tiles", new object[]
						{
							num
						});
						for (int i = 0; i < num; i++)
						{
							int new_x = binaryReader.ReadInt32();
							int new_y = binaryReader.ReadInt32();
							long num2 = binaryReader.ReadInt64();
							this.tileBlobOffsets.Add(new FoliageCoord(new_x, new_y), num2);
						}
						if (this.loadedFileVersion >= 2)
						{
							int num3 = binaryReader.ReadInt32();
							UnturnedLog.info("Found {0} foliage used assets in header", new object[]
							{
								num3
							});
							this.assetsHeader.Capacity = num3;
							for (int j = 0; j < num3; j++)
							{
								GuidBuffer guidBuffer = default(GuidBuffer);
								binaryReader.Read(this.GUID_BUFFER, 0, 16);
								guidBuffer.Read(this.GUID_BUFFER, 0);
								AssetReference<FoliageInstancedMeshInfoAsset> assetReference = new AssetReference<FoliageInstancedMeshInfoAsset>(guidBuffer.GUID);
								this.assetsHeader.Add(assetReference);
								if (assetReference.Find() == null)
								{
									ClientAssetIntegrity.ServerAddKnownMissingAsset(assetReference.GUID, string.Format("Foliage asset {0} of {1}", j + 1, num3));
								}
							}
						}
						this.tileBlobHeaderOffset = this.readerStream.Position;
						Level.includeHash("Foliage", sha1Stream.Hash);
					}
				}
				if (Level.isEditor && this.loadedFileVersion < 2)
				{
					LevelHierarchy.MarkDirty();
				}
				bool isEditor = Level.isEditor;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001737C File Offset: 0x0001557C
		public void Shutdown()
		{
			if (this.workerThread != null)
			{
				this.shouldWorkerThreadContinue = false;
				return;
			}
			this.CloseReader();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00017394 File Offset: 0x00015594
		public void TileBecameRelevantToViewer(FoliageTile tile)
		{
			if (!this.hasAllTilesInMemory && !this.mainThreadTilesWithRelevancyChanges.Contains(tile))
			{
				this.mainThreadTilesWithRelevancyChanges.Add(tile);
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000173B8 File Offset: 0x000155B8
		public void TileNoLongerRelevantToViewer(FoliageTile tile)
		{
			if (!this.hasAllTilesInMemory)
			{
				tile.clearAndReleaseInstances();
				if (!this.mainThreadTilesWithRelevancyChanges.Contains(tile))
				{
					this.mainThreadTilesWithRelevancyChanges.Add(tile);
				}
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000173E4 File Offset: 0x000155E4
		public void Update()
		{
			if (this.workerThread == null)
			{
				return;
			}
			FoliageStorageV2.TileData tileData = null;
			object obj = this.lockObject;
			lock (obj)
			{
				foreach (FoliageTile foliageTile in this.mainThreadTilesWithRelevancyChanges)
				{
					if (foliageTile.isRelevantToViewer)
					{
						if (!this.workerThreadTileQueue.Contains(foliageTile.coord))
						{
							this.workerThreadTileQueue.AddLast(foliageTile.coord);
						}
					}
					else
					{
						this.workerThreadTileQueue.Remove(foliageTile.coord);
					}
				}
				if (this.tileDataFromWorkerThread.Count > 0)
				{
					tileData = this.tileDataFromWorkerThread.Dequeue();
				}
				if (this.mainThreadTileDataFromPreviousUpdate != null)
				{
					this.tileDataFromMainThread.Add(this.mainThreadTileDataFromPreviousUpdate);
					this.mainThreadTileDataFromPreviousUpdate = null;
				}
			}
			this.mainThreadTilesWithRelevancyChanges.Clear();
			if (tileData != null)
			{
				FoliageTile tile = FoliageSystem.getTile(tileData.coord);
				if (tile != null && tile.isRelevantToViewer)
				{
					tile.clearAndReleaseInstances();
					this.DeserializeTileOnMainThreadUsingDataFromWorkerThread(tile, tileData);
				}
				this.mainThreadTileDataFromPreviousUpdate = tileData;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00017524 File Offset: 0x00015724
		public void EditorLoadAllTiles(IEnumerable<FoliageTile> tiles)
		{
			this.hasAllTilesInMemory = true;
			foreach (FoliageTile tile in tiles)
			{
				this.DeserializeTileOnMainThread(tile);
			}
			this.CloseReader();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001757C File Offset: 0x0001577C
		public void EditorSaveAllTiles(IEnumerable<FoliageTile> tiles)
		{
			string text = Level.info.path + "/Foliage.blob";
			if (File.Exists(text) && this.loadedFileVersion >= 2)
			{
				bool flag = false;
				using (IEnumerator<FoliageTile> enumerator = tiles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.hasUnsavedChanges)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			List<byte[]> list = new List<byte[]>();
			Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, int> assetRefToIndex = new Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, int>();
			this.tileBlobOffsets.Clear();
			this.assetsHeader.Clear();
			long num = 0L;
			foreach (FoliageTile foliageTile in tiles)
			{
				if (!foliageTile.isEmpty())
				{
					byte[] array = this.SerializeTileOnMainThread(foliageTile, assetRefToIndex);
					if (array != null && array.Length != 0)
					{
						list.Add(array);
						this.tileBlobOffsets.Add(foliageTile.coord, num);
						num += (long)array.Length;
					}
				}
			}
			if (list.Count != this.tileBlobOffsets.Count)
			{
				UnturnedLog.error("Foliage blob count ({0}) does not match offset count ({1})", new object[]
				{
					list.Count,
					this.tileBlobOffsets.Count
				});
				return;
			}
			using (FileStream fileStream = new FileStream(text, 4, 2, 3))
			{
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(2);
				binaryWriter.Write(this.tileBlobOffsets.Count);
				foreach (KeyValuePair<FoliageCoord, long> keyValuePair in this.tileBlobOffsets)
				{
					binaryWriter.Write(keyValuePair.Key.x);
					binaryWriter.Write(keyValuePair.Key.y);
					binaryWriter.Write(keyValuePair.Value);
				}
				UnturnedLog.info(string.Format("Foliage saving with {0} assets in header", this.assetsHeader.Count));
				binaryWriter.Write(this.assetsHeader.Count);
				foreach (AssetReference<FoliageInstancedMeshInfoAsset> assetReference in this.assetsHeader)
				{
					GuidBuffer guidBuffer = new GuidBuffer(assetReference.GUID);
					guidBuffer.Write(this.GUID_BUFFER, 0);
					binaryWriter.Write(this.GUID_BUFFER, 0, 16);
				}
				foreach (byte[] array2 in list)
				{
					binaryWriter.Write(array2);
				}
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000178B4 File Offset: 0x00015AB4
		private FoliageStorageV2.TileData GetTileDataOnWorkerThread(FoliageCoord coord)
		{
			FoliageStorageV2.TileData tileData;
			if (this.tileDataPool.Count > 0)
			{
				tileData = this.tileDataPool.Pop();
			}
			else
			{
				tileData = new FoliageStorageV2.TileData();
				tileData.perAssetData = new List<FoliageStorageV2.TilePerAssetData>();
			}
			tileData.coord = coord;
			long num;
			if (this.tileBlobOffsets.TryGetValue(coord, ref num))
			{
				this.readerStream.Position = this.tileBlobHeaderOffset + num;
				int num2 = this.reader.ReadInt32();
				tileData.perAssetData.Capacity = Mathf.Max(tileData.perAssetData.Capacity, num2);
				for (int i = 0; i < num2; i++)
				{
					AssetReference<FoliageInstancedMeshInfoAsset> assetRef;
					if (this.loadedFileVersion >= 2)
					{
						int num3 = this.reader.ReadInt32();
						if (num3 >= 0 && num3 < this.assetsHeader.Count)
						{
							assetRef = this.assetsHeader[num3];
						}
						else
						{
							assetRef = AssetReference<FoliageInstancedMeshInfoAsset>.invalid;
						}
					}
					else
					{
						GuidBuffer guidBuffer = default(GuidBuffer);
						this.readerStream.Read(this.GUID_BUFFER, 0, 16);
						guidBuffer.Read(this.GUID_BUFFER, 0);
						assetRef = new AssetReference<FoliageInstancedMeshInfoAsset>(guidBuffer.GUID);
					}
					int num4 = this.reader.ReadInt32();
					FoliageStorageV2.TilePerAssetData tilePerAssetData;
					if (this.perAssetDataPool.Count > 0)
					{
						tilePerAssetData = this.perAssetDataPool.Pop();
						tilePerAssetData.matrices.Capacity = Mathf.Max(tilePerAssetData.matrices.Capacity, num4);
						tilePerAssetData.clearWhenBaked.Capacity = Mathf.Max(tilePerAssetData.clearWhenBaked.Capacity, num4);
					}
					else
					{
						tilePerAssetData = new FoliageStorageV2.TilePerAssetData();
						tilePerAssetData.matrices = new List<Matrix4x4>(num4);
						tilePerAssetData.clearWhenBaked = new List<bool>(num4);
					}
					tilePerAssetData.assetRef = assetRef;
					for (int j = 0; j < num4; j++)
					{
						Matrix4x4 matrix4x = default(Matrix4x4);
						for (int k = 0; k < 16; k++)
						{
							matrix4x[k] = this.reader.ReadSingle();
						}
						tilePerAssetData.matrices.Add(matrix4x);
						bool flag = this.reader.ReadBoolean();
						tilePerAssetData.clearWhenBaked.Add(flag);
					}
					if (num4 > 0)
					{
						tileData.perAssetData.Add(tilePerAssetData);
					}
				}
			}
			return tileData;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00017AE0 File Offset: 0x00015CE0
		private void DeserializeTileOnMainThreadUsingDataFromWorkerThread(FoliageTile tile, FoliageStorageV2.TileData tileData)
		{
			foreach (FoliageStorageV2.TilePerAssetData tilePerAssetData in tileData.perAssetData)
			{
				if (tilePerAssetData.assetRef.isNull)
				{
					UnturnedLog.error(string.Format("Foliage loaded invalid asset ref for tile {0}", tile.coord));
				}
				else
				{
					FoliageInstanceList orAddList = tile.getOrAddList(tilePerAssetData.assetRef);
					for (int i = 0; i < tilePerAssetData.matrices.Count; i++)
					{
						Matrix4x4 newMatrix = tilePerAssetData.matrices[i];
						bool newClearWhenBaked = tilePerAssetData.clearWhenBaked[i];
						if (!tile.isInstanceCut(newMatrix.GetPosition()))
						{
							orAddList.addInstanceAppend(new FoliageInstanceGroup(tilePerAssetData.assetRef, newMatrix, newClearWhenBaked));
						}
					}
				}
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00017BC0 File Offset: 0x00015DC0
		private void DeserializeTileOnMainThread(FoliageTile tile)
		{
			long num;
			if (this.tileBlobOffsets.TryGetValue(tile.coord, ref num))
			{
				this.readerStream.Position = this.tileBlobHeaderOffset + num;
				int num2 = this.reader.ReadInt32();
				for (int i = 0; i < num2; i++)
				{
					AssetReference<FoliageInstancedMeshInfoAsset> assetReference;
					bool flag;
					if (this.loadedFileVersion >= 2)
					{
						int num3 = this.reader.ReadInt32();
						if (num3 >= 0 && num3 < this.assetsHeader.Count)
						{
							assetReference = this.assetsHeader[num3];
							flag = !assetReference.isNull;
						}
						else
						{
							assetReference = AssetReference<FoliageInstancedMeshInfoAsset>.invalid;
							UnturnedLog.error(string.Format("Foliage loaded invalid asset index {0} for tile {1}", num3, tile.coord));
							flag = false;
						}
					}
					else
					{
						GuidBuffer guidBuffer = default(GuidBuffer);
						this.readerStream.Read(this.GUID_BUFFER, 0, 16);
						guidBuffer.Read(this.GUID_BUFFER, 0);
						assetReference = new AssetReference<FoliageInstancedMeshInfoAsset>(guidBuffer.GUID);
						flag = !assetReference.isNull;
					}
					FoliageInstanceList orAddList = tile.getOrAddList(assetReference);
					int num4 = this.reader.ReadInt32();
					for (int j = 0; j < num4; j++)
					{
						Matrix4x4 newMatrix = default(Matrix4x4);
						for (int k = 0; k < 16; k++)
						{
							newMatrix[k] = this.reader.ReadSingle();
						}
						bool newClearWhenBaked = this.reader.ReadBoolean();
						if (flag && !tile.isInstanceCut(newMatrix.GetPosition()))
						{
							orAddList.addInstanceAppend(new FoliageInstanceGroup(assetReference, newMatrix, newClearWhenBaked));
						}
					}
				}
			}
			tile.updateBounds();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00017D58 File Offset: 0x00015F58
		private int GetOrAddAssetIndex(AssetReference<FoliageInstancedMeshInfoAsset> assetRef, Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, int> assetRefToIndex)
		{
			int result;
			if (assetRefToIndex.TryGetValue(assetRef, ref result))
			{
				return result;
			}
			int count = this.assetsHeader.Count;
			this.assetsHeader.Add(assetRef);
			assetRefToIndex.Add(assetRef, count);
			return count;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00017D94 File Offset: 0x00015F94
		private byte[] SerializeTileOnMainThread(FoliageTile tile, Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, int> assetRefToIndex)
		{
			this.tileInstanceListsToSave.Clear();
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in tile.instances)
			{
				if (!keyValuePair.Key.isNull && !keyValuePair.Value.IsListEmpty())
				{
					if (!LevelObjects.preserveMissingAssets && keyValuePair.Key.Find() == null)
					{
						UnturnedLog.info(string.Format("Discarding missing foliage asset {0} from tile {1}", keyValuePair.Key, tile.coord));
					}
					else
					{
						this.tileInstanceListsToSave.Add(keyValuePair);
					}
				}
			}
			if (this.tileInstanceListsToSave.Count < 1)
			{
				return null;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(this.tileInstanceListsToSave.Count);
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair2 in this.tileInstanceListsToSave)
				{
					int orAddAssetIndex = this.GetOrAddAssetIndex(keyValuePair2.Key, assetRefToIndex);
					binaryWriter.Write(orAddAssetIndex);
					int num = 0;
					foreach (List<Matrix4x4> list in keyValuePair2.Value.matrices)
					{
						num += list.Count;
					}
					binaryWriter.Write(num);
					for (int i = 0; i < keyValuePair2.Value.matrices.Count; i++)
					{
						List<Matrix4x4> list2 = keyValuePair2.Value.matrices[i];
						List<bool> list3 = keyValuePair2.Value.clearWhenBaked[i];
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
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00018040 File Offset: 0x00016240
		private void CloseReader()
		{
			if (this.reader != null)
			{
				this.reader.Close();
				this.reader.Dispose();
				this.reader = null;
			}
			if (this.readerStream != null)
			{
				this.readerStream.Close();
				this.readerStream.Dispose();
				this.readerStream = null;
			}
		}

		/// <summary>
		/// Entry point for worker thread loop.
		/// </summary>
		// Token: 0x06000628 RID: 1576 RVA: 0x00018098 File Offset: 0x00016298
		private void WorkerThreadMain()
		{
			this.perAssetDataPool = new Stack<FoliageStorageV2.TilePerAssetData>();
			this.tileDataPool = new Stack<FoliageStorageV2.TileData>();
			FoliageStorageV2.TileData tileData = null;
			while (this.shouldWorkerThreadContinue)
			{
				FoliageCoord coord = default(FoliageCoord);
				bool flag = false;
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.workerThreadTileQueue.Count > 0)
					{
						coord = this.workerThreadTileQueue.First.Value;
						this.workerThreadTileQueue.RemoveFirst();
						flag = true;
					}
					if (tileData != null)
					{
						this.tileDataFromWorkerThread.Enqueue(tileData);
						tileData = null;
					}
					foreach (FoliageStorageV2.TileData tileData2 in this.tileDataFromMainThread)
					{
						foreach (FoliageStorageV2.TilePerAssetData tilePerAssetData in tileData2.perAssetData)
						{
							tilePerAssetData.matrices.Clear();
							tilePerAssetData.clearWhenBaked.Clear();
							this.perAssetDataPool.Push(tilePerAssetData);
						}
						tileData2.perAssetData.Clear();
						this.tileDataPool.Push(tileData2);
					}
					this.tileDataFromMainThread.Clear();
				}
				if (flag)
				{
					tileData = this.GetTileDataOnWorkerThread(coord);
				}
				Thread.Sleep(10);
			}
			this.CloseReader();
		}

		// Token: 0x04000244 RID: 580
		private List<KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList>> tileInstanceListsToSave = new List<KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList>>();

		// Token: 0x04000245 RID: 581
		private const int FOLIAGE_FILE_VERSION_INITIAL = 1;

		// Token: 0x04000246 RID: 582
		private const int FOLIAGE_FILE_VERSION_ADDED_ASSET_LIST_HEADER = 2;

		// Token: 0x04000247 RID: 583
		private const int FOLIAGE_FILE_VERSION_NEWEST = 2;

		// Token: 0x04000248 RID: 584
		private byte[] GUID_BUFFER = new byte[16];

		// Token: 0x04000249 RID: 585
		private FileStream readerStream;

		// Token: 0x0400024A RID: 586
		private BinaryReader reader;

		// Token: 0x0400024B RID: 587
		private bool hasAllTilesInMemory;

		/// <summary>
		/// Order is important because TileBecameRelevant is called from the closest tile outward.
		/// </summary>
		// Token: 0x0400024C RID: 588
		private List<FoliageTile> mainThreadTilesWithRelevancyChanges = new List<FoliageTile>();

		/// <summary>
		/// Offsets into blob for per-tile data.
		/// </summary>
		// Token: 0x0400024D RID: 589
		private Dictionary<FoliageCoord, long> tileBlobOffsets = new Dictionary<FoliageCoord, long>();

		/// <summary>
		/// Tiles save an index into this list rather than guid.
		/// </summary>
		// Token: 0x0400024E RID: 590
		private List<AssetReference<FoliageInstancedMeshInfoAsset>> assetsHeader = new List<AssetReference<FoliageInstancedMeshInfoAsset>>();

		// Token: 0x0400024F RID: 591
		private int loadedFileVersion;

		/// <summary>
		/// Offset from header data.
		/// </summary>
		// Token: 0x04000250 RID: 592
		private long tileBlobHeaderOffset;

		// Token: 0x04000251 RID: 593
		private Thread workerThread;

		// Token: 0x04000252 RID: 594
		private bool shouldWorkerThreadContinue;

		/// <summary>
		/// Ready to be released to the worker thread during the next lock.
		/// </summary>
		// Token: 0x04000253 RID: 595
		private FoliageStorageV2.TileData mainThreadTileDataFromPreviousUpdate;

		/// <summary>
		/// Mutex lock. Only used in the main thread Update loop and worker thread loop.
		/// </summary>
		// Token: 0x04000254 RID: 596
		private object lockObject;

		/// <summary>
		/// SHARED BY BOTH THREADS!
		/// Coordinates requested by main thread for worker thread to read.
		/// This is a list because while main thread is busy the worker thread can continue reading.
		/// </summary>
		// Token: 0x04000255 RID: 597
		private LinkedList<FoliageCoord> workerThreadTileQueue = new LinkedList<FoliageCoord>();

		/// <summary>
		/// SHARED BY BOTH THREADS!
		/// Tiles read by worker thread ready to be copied into actual foliage tiles on main thread.
		/// </summary>
		// Token: 0x04000256 RID: 598
		private Queue<FoliageStorageV2.TileData> tileDataFromWorkerThread = new Queue<FoliageStorageV2.TileData>();

		/// <summary>
		/// SHARED BY BOTH THREADS!
		/// Main thread has finished using this tile data and it can be released back to the pool on the worker thread.
		/// This is a list because main thread could have populated multiple foliage tiles while the worker thread was busy reading.
		/// </summary>
		// Token: 0x04000257 RID: 599
		private List<FoliageStorageV2.TileData> tileDataFromMainThread = new List<FoliageStorageV2.TileData>();

		/// <summary>
		/// Lifecycle:
		/// 1. Worker thread claims or allocates data.
		/// 2. Worker thread passes data to main thread.
		/// 3. Main thread copies data over to actual foliage tile.
		/// 4. Main thread passes data back to worker thread.
		/// 5. Worker thread releases data back to pool.
		/// </summary>
		// Token: 0x04000258 RID: 600
		private Stack<FoliageStorageV2.TilePerAssetData> perAssetDataPool;

		// Token: 0x04000259 RID: 601
		private Stack<FoliageStorageV2.TileData> tileDataPool;

		/// <summary>
		/// Data-only FoliageInstanceList shared between threads.
		/// </summary>
		// Token: 0x02000865 RID: 2149
		private class TilePerAssetData
		{
			// Token: 0x04003169 RID: 12649
			public AssetReference<FoliageInstancedMeshInfoAsset> assetRef;

			// Token: 0x0400316A RID: 12650
			public List<Matrix4x4> matrices;

			// Token: 0x0400316B RID: 12651
			public List<bool> clearWhenBaked;
		}

		/// <summary>
		/// Data-only FoliageTile shared between threads.
		/// </summary>
		// Token: 0x02000866 RID: 2150
		private class TileData
		{
			// Token: 0x0400316C RID: 12652
			public FoliageCoord coord;

			// Token: 0x0400316D RID: 12653
			public List<FoliageStorageV2.TilePerAssetData> perAssetData;
		}
	}
}
