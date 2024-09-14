using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Landscapes;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000FD RID: 253
	public class FoliageSystem : DevkitHierarchyItemBase
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00018474 File Offset: 0x00016674
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0001847B File Offset: 0x0001667B
		public static FoliageSystem instance { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00018483 File Offset: 0x00016683
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0001848A File Offset: 0x0001668A
		public static List<IFoliageSurface> surfaces { get; private set; } = new List<IFoliageSurface>();

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600064C RID: 1612 RVA: 0x00018494 File Offset: 0x00016694
		// (remove) Token: 0x0600064D RID: 1613 RVA: 0x000184C8 File Offset: 0x000166C8
		public static event FoliageSystemPreBakeHandler preBake;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600064E RID: 1614 RVA: 0x000184FC File Offset: 0x000166FC
		// (remove) Token: 0x0600064F RID: 1615 RVA: 0x00018530 File Offset: 0x00016730
		public static event FoliageSystemPreBakeTileHandler preBakeTile;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000650 RID: 1616 RVA: 0x00018564 File Offset: 0x00016764
		// (remove) Token: 0x06000651 RID: 1617 RVA: 0x00018598 File Offset: 0x00016798
		public static event FoliageSystemPostBakeTileHandler postBakeTile;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000652 RID: 1618 RVA: 0x000185CC File Offset: 0x000167CC
		// (remove) Token: 0x06000653 RID: 1619 RVA: 0x00018600 File Offset: 0x00016800
		public static event FoliageSystemGlobalBakeHandler globalBake;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000654 RID: 1620 RVA: 0x00018634 File Offset: 0x00016834
		// (remove) Token: 0x06000655 RID: 1621 RVA: 0x00018668 File Offset: 0x00016868
		public static event FoliageSystemLocalBakeHandler localBake;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000656 RID: 1622 RVA: 0x0001869C File Offset: 0x0001689C
		// (remove) Token: 0x06000657 RID: 1623 RVA: 0x000186D0 File Offset: 0x000168D0
		public static event FoliageSystemPostBakeHandler postBake;

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00018703 File Offset: 0x00016903
		public static int bakeQueueProgress
		{
			get
			{
				return FoliageSystem.bakeQueueTotal - FoliageSystem.bakeQueue.Count;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00018715 File Offset: 0x00016915
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x0001871C File Offset: 0x0001691C
		public static int bakeQueueTotal { get; private set; }

		/// <summary>
		/// Settings configured when starting the bake.
		/// </summary>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00018724 File Offset: 0x00016924
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0001872B File Offset: 0x0001692B
		public static FoliageBakeSettings bakeSettings { get; private set; }

		// Token: 0x0600065D RID: 1629 RVA: 0x00018734 File Offset: 0x00016934
		public static void CreateInLevelIfMissing()
		{
			if (FoliageSystem.instance == null)
			{
				UnturnedLog.info("Adding default foliage system to level");
				LevelHierarchy.AssignInstanceIdAndMarkDirty(new GameObject().AddComponent<FoliageSystem>());
				if (VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().additiveVolumes.Count < 1)
				{
					UnturnedLog.info("Adding default additive foliage volume to level");
					FoliageVolume foliageVolume = new GameObject
					{
						transform = 
						{
							position = Vector3.zero,
							rotation = Quaternion.identity,
							localScale = new Vector3((float)Level.size, Landscape.TILE_HEIGHT, (float)Level.size)
						}
					}.AddComponent<FoliageVolume>();
					LevelHierarchy.AssignInstanceIdAndMarkDirty(foliageVolume);
					foliageVolume.mode = FoliageVolume.EFoliageVolumeMode.ADDITIVE;
				}
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000187DD File Offset: 0x000169DD
		public static void addSurface(IFoliageSurface surface)
		{
			FoliageSystem.surfaces.Add(surface);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000187EA File Offset: 0x000169EA
		public static void removeSurface(IFoliageSurface surface)
		{
			FoliageSystem.surfaces.Remove(surface);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000187F8 File Offset: 0x000169F8
		[Obsolete]
		public static void addCut(IShapeVolume cut)
		{
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000187FC File Offset: 0x000169FC
		internal static void AddCut(FoliageCut cut)
		{
			for (int i = cut.foliageBounds.min.x; i <= cut.foliageBounds.max.x; i++)
			{
				for (int j = cut.foliageBounds.min.y; j <= cut.foliageBounds.max.y; j++)
				{
					FoliageSystem.getOrAddTile(new FoliageCoord(i, j)).AddCut(cut);
				}
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00018870 File Offset: 0x00016A70
		internal static void RemoveCut(FoliageCut cut)
		{
			for (int i = cut.foliageBounds.min.x; i <= cut.foliageBounds.max.x; i++)
			{
				for (int j = cut.foliageBounds.min.y; j <= cut.foliageBounds.max.y; j++)
				{
					FoliageSystem.getOrAddTile(new FoliageCoord(i, j)).RemoveCut(cut);
				}
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000188E4 File Offset: 0x00016AE4
		private static Dictionary<FoliageTile, List<IFoliageSurface>> getTileSurfacePairs()
		{
			Dictionary<FoliageTile, List<IFoliageSurface>> dictionary = new Dictionary<FoliageTile, List<IFoliageSurface>>();
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				FoliageTile value = keyValuePair.Value;
				if (VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsTileBakeable(value))
				{
					dictionary.Add(value, new List<IFoliageSurface>());
				}
			}
			foreach (IFoliageSurface foliageSurface in FoliageSystem.surfaces)
			{
				FoliageBounds foliageSurfaceBounds = foliageSurface.getFoliageSurfaceBounds();
				for (int i = foliageSurfaceBounds.min.x; i <= foliageSurfaceBounds.max.x; i++)
				{
					for (int j = foliageSurfaceBounds.min.y; j <= foliageSurfaceBounds.max.y; j++)
					{
						FoliageTile orAddTile = FoliageSystem.getOrAddTile(new FoliageCoord(i, j));
						if (VolumeManager<FoliageVolume, FoliageVolumeManager>.Get().IsTileBakeable(orAddTile))
						{
							List<IFoliageSurface> list;
							if (!dictionary.TryGetValue(orAddTile, ref list))
							{
								list = new List<IFoliageSurface>();
								dictionary.Add(orAddTile, list);
							}
							list.Add(foliageSurface);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00018A34 File Offset: 0x00016C34
		private static void bakePre()
		{
			FoliageSystemPreBakeHandler foliageSystemPreBakeHandler = FoliageSystem.preBake;
			if (foliageSystemPreBakeHandler != null)
			{
				foliageSystemPreBakeHandler();
			}
			FoliageSystem.bakeQueue.Clear();
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00018A50 File Offset: 0x00016C50
		public static void bakeGlobal(FoliageBakeSettings bakeSettings)
		{
			FoliageSystem.CreateInLevelIfMissing();
			FoliageSystem.bakeSettings = bakeSettings;
			FoliageSystem.bakePre();
			FoliageSystem.bakeGlobalBegin();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00018A68 File Offset: 0x00016C68
		private static void bakeGlobalBegin()
		{
			foreach (KeyValuePair<FoliageTile, List<IFoliageSurface>> keyValuePair in FoliageSystem.getTileSurfacePairs())
			{
				FoliageSystem.bakeQueue.Enqueue(keyValuePair);
			}
			FoliageSystem.bakeQueueTotal = FoliageSystem.bakeQueue.Count;
			FoliageSystem.bakeEnd = new FoliageSystemPostBakeHandler(FoliageSystem.bakeGlobalEnd);
			FoliageSystem.bakeEnd();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00018AE8 File Offset: 0x00016CE8
		private static void bakeGlobalEnd()
		{
			FoliageSystemGlobalBakeHandler foliageSystemGlobalBakeHandler = FoliageSystem.globalBake;
			if (foliageSystemGlobalBakeHandler != null)
			{
				foliageSystemGlobalBakeHandler();
			}
			FoliageSystem.bakePost();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00018AFF File Offset: 0x00016CFF
		public static void bakeLocal(FoliageBakeSettings bakeSettings)
		{
			FoliageSystem.CreateInLevelIfMissing();
			FoliageSystem.bakeSettings = bakeSettings;
			FoliageSystem.bakePre();
			FoliageSystem.bakeLocalBegin();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00018B18 File Offset: 0x00016D18
		private static void bakeLocalBegin()
		{
			FoliageSystem.bakeLocalPosition = MainCamera.instance.transform.position;
			int num = 6;
			int num2 = num * num;
			FoliageCoord foliageCoord = new FoliageCoord(FoliageSystem.bakeLocalPosition);
			Dictionary<FoliageTile, List<IFoliageSurface>> tileSurfacePairs = FoliageSystem.getTileSurfacePairs();
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					if (i * i + j * j <= num2)
					{
						FoliageTile tile = FoliageSystem.getTile(new FoliageCoord(foliageCoord.x + i, foliageCoord.y + j));
						List<IFoliageSurface> list;
						if (tile != null && tileSurfacePairs.TryGetValue(tile, ref list))
						{
							KeyValuePair<FoliageTile, List<IFoliageSurface>> keyValuePair = new KeyValuePair<FoliageTile, List<IFoliageSurface>>(tile, list);
							FoliageSystem.bakeQueue.Enqueue(keyValuePair);
						}
					}
				}
			}
			FoliageSystem.bakeQueueTotal = FoliageSystem.bakeQueue.Count;
			FoliageSystem.bakeEnd = new FoliageSystemPostBakeHandler(FoliageSystem.bakeLocalEnd);
			FoliageSystem.bakeEnd();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00018BF0 File Offset: 0x00016DF0
		private static void bakeLocalEnd()
		{
			FoliageSystemLocalBakeHandler foliageSystemLocalBakeHandler = FoliageSystem.localBake;
			if (foliageSystemLocalBakeHandler != null)
			{
				foliageSystemLocalBakeHandler(FoliageSystem.bakeLocalPosition);
			}
			FoliageSystem.bakePost();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00018C0C File Offset: 0x00016E0C
		public static void bakeCancel()
		{
			if (FoliageSystem.bakeQueue.Count == 0)
			{
				return;
			}
			FoliageSystem.bakeQueue.Clear();
			FoliageSystem.bakeEnd();
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00018C2F File Offset: 0x00016E2F
		private static void bakePreTile(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			if (!bakeSettings.bakeInstancesMeshes)
			{
				return;
			}
			if (bakeSettings.bakeApplyScale)
			{
				foliageTile.applyScale();
				return;
			}
			foliageTile.clearGeneratedInstances();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00018C50 File Offset: 0x00016E50
		private static void bake(FoliageTile tile, List<IFoliageSurface> list)
		{
			FoliageSystem.bakePreTile(FoliageSystem.bakeSettings, tile);
			FoliageSystemPreBakeTileHandler foliageSystemPreBakeTileHandler = FoliageSystem.preBakeTile;
			if (foliageSystemPreBakeTileHandler != null)
			{
				foliageSystemPreBakeTileHandler(FoliageSystem.bakeSettings, tile);
			}
			if (!FoliageSystem.bakeSettings.bakeApplyScale)
			{
				foreach (IFoliageSurface foliageSurface in list)
				{
					foliageSurface.bakeFoliageSurface(FoliageSystem.bakeSettings, tile);
				}
			}
			FoliageSystemPostBakeTileHandler foliageSystemPostBakeTileHandler = FoliageSystem.postBakeTile;
			if (foliageSystemPostBakeTileHandler == null)
			{
				return;
			}
			foliageSystemPostBakeTileHandler(FoliageSystem.bakeSettings, tile);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00018CE4 File Offset: 0x00016EE4
		private static void bakePost()
		{
			if (LevelHierarchy.instance != null)
			{
				LevelHierarchy.instance.isDirty = true;
			}
			FoliageSystemPostBakeHandler foliageSystemPostBakeHandler = FoliageSystem.postBake;
			if (foliageSystemPostBakeHandler == null)
			{
				return;
			}
			foliageSystemPostBakeHandler();
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00018D08 File Offset: 0x00016F08
		public static void addInstance(AssetReference<FoliageInstancedMeshInfoAsset> assetReference, Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			FoliageTile orAddTile = FoliageSystem.getOrAddTile(position);
			Matrix4x4 newMatrix = Matrix4x4.TRS(position, rotation, scale);
			orAddTile.addInstance(new FoliageInstanceGroup(assetReference, newMatrix, clearWhenBaked));
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00018D32 File Offset: 0x00016F32
		protected static void shutdownStorage()
		{
			if (FoliageSystem.storage != null)
			{
				FoliageSystem.storage.Shutdown();
				FoliageSystem.storage = null;
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00018D4C File Offset: 0x00016F4C
		protected static void clearAndReleaseTiles()
		{
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				keyValuePair.Value.clearAndReleaseInstances();
			}
			FoliageSystem.tiles.Clear();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00018DB0 File Offset: 0x00016FB0
		public static FoliageTile getOrAddTile(Vector3 worldPosition)
		{
			return FoliageSystem.getOrAddTile(new FoliageCoord(worldPosition));
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00018DBD File Offset: 0x00016FBD
		public static FoliageTile getTile(Vector3 worldPosition)
		{
			return FoliageSystem.getTile(new FoliageCoord(worldPosition));
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00018DCC File Offset: 0x00016FCC
		public static FoliageTile getOrAddTile(FoliageCoord tileCoord)
		{
			FoliageTile foliageTile;
			if (!FoliageSystem.tiles.TryGetValue(tileCoord, ref foliageTile))
			{
				foliageTile = new FoliageTile(tileCoord);
				FoliageSystem.tiles.Add(tileCoord, foliageTile);
			}
			return foliageTile;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00018DFC File Offset: 0x00016FFC
		public static FoliageTile getTile(FoliageCoord tileCoord)
		{
			FoliageTile result;
			FoliageSystem.tiles.TryGetValue(tileCoord, ref result);
			return result;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00018E18 File Offset: 0x00017018
		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader.containsKey("Version"))
			{
				this.version = reader.readValue<uint>("Version");
			}
			else
			{
				this.version = 1U;
			}
			int num = reader.readArrayLength("Tiles");
			if (FoliageSystem.instance != this)
			{
				UnturnedLog.warn("Level contains multiple FoliageSystems. Ignoring {0} tile(s) with instance ID: {1}", new object[]
				{
					num,
					this.instanceID
				});
				return;
			}
			if (this.version == 2U)
			{
				FoliageSystem.storage = new FoliageStorageV2();
			}
			else
			{
				FoliageSystem.storage = new FoliageStorageV1();
			}
			FoliageSystem.storage.Initialize();
			FoliageSystem.shutdownStorage();
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00018EC4 File Offset: 0x000170C4
		public override void write(IFormattedFileWriter writer)
		{
			if (FoliageSystem.storage == null || this.version < 2U)
			{
				new FoliageStorageV2().EditorSaveAllTiles(FoliageSystem.tiles.Values);
				this.version = 2U;
			}
			else
			{
				FoliageSystem.storage.EditorSaveAllTiles(FoliageSystem.tiles.Values);
			}
			writer.beginObject();
			writer.writeValue<uint>("Version", this.version);
			writer.beginArray("Tiles");
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				FoliageTile value = keyValuePair.Value;
				writer.writeValue<FoliageTile>(value);
			}
			writer.endArray();
			writer.endObject();
		}

		/// <summary>
		/// Automatically placing foliage onto tiles in editor.
		/// </summary>
		// Token: 0x06000678 RID: 1656 RVA: 0x00018F90 File Offset: 0x00017190
		protected void tickBakeQueue()
		{
			KeyValuePair<FoliageTile, List<IFoliageSurface>> keyValuePair = FoliageSystem.bakeQueue.Dequeue();
			FoliageSystem.bake(keyValuePair.Key, keyValuePair.Value);
			if (FoliageSystem.bakeQueue.Count == 0)
			{
				FoliageSystem.bakeEnd();
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00018FD1 File Offset: 0x000171D1
		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00018FD9 File Offset: 0x000171D9
		protected void OnDisable()
		{
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00018FE4 File Offset: 0x000171E4
		protected void Awake()
		{
			base.name = "Foliage_System";
			base.gameObject.layer = 20;
			if (FoliageSystem.instance == null)
			{
				FoliageSystem.instance = this;
				FoliageSystem.prevTiles.Clear();
				FoliageSystem.activeTiles.Clear();
				FoliageSystem.bakeQueue.Clear();
				FoliageSystem.shutdownStorage();
				FoliageSystem.clearAndReleaseTiles();
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00019044 File Offset: 0x00017244
		protected void OnDestroy()
		{
			if (FoliageSystem.instance == this)
			{
				FoliageSystem.instance = null;
				FoliageSystem.prevTiles.Clear();
				FoliageSystem.activeTiles.Clear();
				FoliageSystem.bakeQueue.Clear();
				FoliageSystem.shutdownStorage();
				FoliageSystem.clearAndReleaseTiles();
			}
		}

		// Token: 0x0400025D RID: 605
		public static float TILE_SIZE = 32f;

		// Token: 0x0400025E RID: 606
		public static int TILE_SIZE_INT = 32;

		// Token: 0x0400025F RID: 607
		public static int SPLATMAP_RESOLUTION_PER_TILE = 8;

		// Token: 0x0400026A RID: 618
		protected static Dictionary<FoliageCoord, FoliageTile> prevTiles = new Dictionary<FoliageCoord, FoliageTile>();

		// Token: 0x0400026B RID: 619
		protected static Dictionary<FoliageCoord, FoliageTile> activeTiles = new Dictionary<FoliageCoord, FoliageTile>();

		// Token: 0x0400026C RID: 620
		protected static Dictionary<FoliageCoord, FoliageTile> tiles = new Dictionary<FoliageCoord, FoliageTile>();

		/// <summary>
		/// Implementation of tile data storage.
		/// </summary>
		// Token: 0x0400026D RID: 621
		protected static IFoliageStorage storage = null;

		// Token: 0x0400026E RID: 622
		protected static Queue<KeyValuePair<FoliageTile, List<IFoliageSurface>>> bakeQueue = new Queue<KeyValuePair<FoliageTile, List<IFoliageSurface>>>();

		// Token: 0x0400026F RID: 623
		protected static FoliageSystemPostBakeHandler bakeEnd;

		// Token: 0x04000270 RID: 624
		protected static Vector3 bakeLocalPosition;

		// Token: 0x04000271 RID: 625
		private static Plane[] mainCameraFrustumPlanes = new Plane[6];

		// Token: 0x04000272 RID: 626
		private static Plane[] focusCameraFrustumPlanes = new Plane[6];

		// Token: 0x04000273 RID: 627
		public static Vector3 focusPosition;

		// Token: 0x04000274 RID: 628
		public static bool isFocused;

		// Token: 0x04000275 RID: 629
		public static Camera focusCamera;

		// Token: 0x04000276 RID: 630
		public bool hiddenByHeightEditor;

		// Token: 0x04000277 RID: 631
		public bool hiddenByMaterialEditor;

		/// <summary>
		/// Version number associated with this particular system instance.
		/// </summary>
		// Token: 0x04000278 RID: 632
		protected uint version;

		/// <summary>
		/// 2022-04-26: this used to be environment layer, but "scope focus foliage" can draw outside that render distance
		/// so we now use the sky layer which is visible up to the far clip plane.
		/// </summary>
		// Token: 0x04000279 RID: 633
		private const int foliageRenderLayer = 18;
	}
}
