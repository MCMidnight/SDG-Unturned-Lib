using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F0 RID: 1264
	public class LevelObjects : MonoBehaviour
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000A4C65 File Offset: 0x000A2E65
		private static uint generateUniqueInstanceID()
		{
			return LevelObjects.availableInstanceID++;
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000A4C74 File Offset: 0x000A2E74
		[Obsolete("Was the parent of all objects in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelObjects._models == null)
				{
					LevelObjects._models = new GameObject().transform;
					LevelObjects._models.name = "Objects";
					LevelObjects._models.parent = Level.level;
					LevelObjects._models.tag = "Logic";
					LevelObjects._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelObjects.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelObjects._models;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000A4CEE File Offset: 0x000A2EEE
		public static List<LevelObject>[,] objects
		{
			get
			{
				return LevelObjects._objects;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x000A4CF5 File Offset: 0x000A2EF5
		public static List<LevelBuildableObject>[,] buildables
		{
			get
			{
				return LevelObjects._buildables;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x000A4CFC File Offset: 0x000A2EFC
		public static int total
		{
			get
			{
				return LevelObjects._total;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000A4D03 File Offset: 0x000A2F03
		public static bool[,] regions
		{
			get
			{
				return LevelObjects._regions;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x000A4D0A File Offset: 0x000A2F0A
		public static int[,] loads
		{
			get
			{
				return LevelObjects._loads;
			}
		}

		/// <summary>
		/// Hash of Objects.dat
		/// </summary>
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x000A4D11 File Offset: 0x000A2F11
		// (set) Token: 0x06002784 RID: 10116 RVA: 0x000A4D18 File Offset: 0x000A2F18
		public static byte[] hash { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000A4D20 File Offset: 0x000A2F20
		// (set) Token: 0x06002786 RID: 10118 RVA: 0x000A4D27 File Offset: 0x000A2F27
		public static bool shouldInstantlyLoad { get; private set; }

		// Token: 0x06002787 RID: 10119 RVA: 0x000A4D30 File Offset: 0x000A2F30
		public static void undo()
		{
			while (LevelObjects.frame <= LevelObjects.reun.Length - 1)
			{
				if (LevelObjects.reun[LevelObjects.frame] != null)
				{
					LevelObjects.reun[LevelObjects.frame].undo();
				}
				if (LevelObjects.frame >= LevelObjects.reun.Length - 1 || LevelObjects.reun[LevelObjects.frame + 1] == null)
				{
					break;
				}
				LevelObjects.frame++;
				if (LevelObjects.reun[LevelObjects.frame].step != LevelObjects.step)
				{
					LevelObjects.step--;
					return;
				}
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000A4DBC File Offset: 0x000A2FBC
		public static void redo()
		{
			while (LevelObjects.frame >= 0)
			{
				if (LevelObjects.reun[LevelObjects.frame] != null)
				{
					LevelObjects.reun[LevelObjects.frame].redo();
				}
				if (LevelObjects.frame <= 0 || LevelObjects.reun[LevelObjects.frame - 1] == null)
				{
					break;
				}
				LevelObjects.frame--;
				if (LevelObjects.reun[LevelObjects.frame].step != LevelObjects.step)
				{
					LevelObjects.step++;
					return;
				}
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000A4E38 File Offset: 0x000A3038
		public static Transform register(IReun newReun)
		{
			if (LevelObjects.frame > 0)
			{
				LevelObjects.reun = new IReun[LevelObjects.reun.Length];
				LevelObjects.frame = 0;
			}
			for (int i = LevelObjects.reun.Length - 1; i > 0; i--)
			{
				LevelObjects.reun[i] = LevelObjects.reun[i - 1];
			}
			LevelObjects.reun[0] = newReun;
			return LevelObjects.reun[0].redo();
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000A4E9C File Offset: 0x000A309C
		public static void transformObject(Transform select, Vector3 toPosition, Quaternion toRotation, Vector3 toScale, Vector3 fromPosition, Quaternion fromRotation, Vector3 fromScale)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(fromPosition, out b, out b2))
			{
				byte b3;
				byte b4;
				if (!Regions.tryGetCoordinate(toPosition, out b3, out b4))
				{
					select.position = fromPosition;
					select.rotation = fromRotation;
					select.localScale = fromScale;
					return;
				}
				LevelObject levelObject = null;
				int num = -1;
				for (int i = 0; i < LevelObjects.objects[(int)b, (int)b2].Count; i++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][i].transform == select)
					{
						levelObject = LevelObjects.objects[(int)b, (int)b2][i];
						num = i;
						break;
					}
				}
				if (levelObject != null)
				{
					if (b != b3 || b2 != b4)
					{
						LevelObjects.objects[(int)b, (int)b2].RemoveAt(num);
						LevelObjects.objects[(int)b3, (int)b4].Add(levelObject);
					}
					if (levelObject.transform != null)
					{
						levelObject.transform.position = toPosition;
						levelObject.transform.rotation = toRotation;
						levelObject.transform.localScale = toScale;
					}
					if (levelObject.skybox != null)
					{
						levelObject.skybox.position = toPosition;
						levelObject.skybox.rotation = toRotation;
						levelObject.skybox.localScale = toScale;
						return;
					}
				}
				else
				{
					LevelBuildableObject levelBuildableObject = null;
					int num2 = -1;
					for (int j = 0; j < LevelObjects.buildables[(int)b, (int)b2].Count; j++)
					{
						if (LevelObjects.buildables[(int)b, (int)b2][j].transform == select)
						{
							levelBuildableObject = LevelObjects.buildables[(int)b, (int)b2][j];
							num2 = j;
							break;
						}
					}
					if (levelBuildableObject == null)
					{
						select.position = fromPosition;
						select.rotation = fromRotation;
						select.localScale = fromScale;
						return;
					}
					if (b != b3 || b2 != b4)
					{
						LevelObjects.buildables[(int)b, (int)b2].RemoveAt(num2);
						LevelObjects.buildables[(int)b3, (int)b4].Add(levelBuildableObject);
					}
					if (levelBuildableObject.transform != null)
					{
						levelBuildableObject.transform.position = toPosition;
						levelBuildableObject.transform.rotation = toRotation;
						return;
					}
				}
			}
			else
			{
				select.position = fromPosition;
				select.rotation = fromRotation;
				select.localScale = fromScale;
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000A50D5 File Offset: 0x000A32D5
		public static void registerTransformObject(Transform select, Vector3 toPosition, Quaternion toRotation, Vector3 toScale, Vector3 fromPosition, Quaternion fromRotation, Vector3 fromScale)
		{
			LevelObjects.register(new ReunObjectTransform(LevelObjects.step, select, fromPosition, fromRotation, fromScale, toPosition, toRotation, toScale));
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000A50F1 File Offset: 0x000A32F1
		[Obsolete]
		public static DevkitHierarchyWorldObject addDevkitObject(Guid GUID, Vector3 position, Quaternion rotation, Vector3 scale, ELevelObjectPlacementOrigin placementOrigin)
		{
			LevelObjects.addObject(position, rotation, scale, 0, GUID, placementOrigin);
			return null;
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000A5104 File Offset: 0x000A3304
		[Obsolete]
		public static void registerDevkitObject(LevelObject levelObject, out byte x, out byte y)
		{
			if (!Regions.tryGetCoordinate(levelObject.transform.position, out x, out y))
			{
				levelObject.SetIsActiveInRegion(true);
				return;
			}
			LevelObjects.objects[(int)x, (int)y].Add(levelObject);
			if (LevelObjects.regions[(int)x, (int)y])
			{
				levelObject.SetIsActiveInRegion(true);
				return;
			}
			levelObject.SetIsActiveInRegion(false);
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000A5160 File Offset: 0x000A3360
		[Obsolete]
		public static void moveDevkitObject(LevelObject levelObject, byte old_x, byte old_y, byte new_x, byte new_y)
		{
			if (Regions.checkSafe((int)old_x, (int)old_y))
			{
				LevelObjects.objects[(int)old_x, (int)old_y].Remove(levelObject);
			}
			LevelObjects.objects[(int)new_x, (int)new_y].Add(levelObject);
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000A5191 File Offset: 0x000A3391
		[Obsolete]
		public static void unregisterDevkitObject(LevelObject levelObject, byte x, byte y)
		{
			if (Regions.checkSafe((int)x, (int)y))
			{
				LevelObjects.objects[(int)x, (int)y].Remove(levelObject);
			}
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000A51AF File Offset: 0x000A33AF
		[Obsolete]
		public static Transform addObject(Vector3 position, Quaternion rotation, Vector3 scale, ushort id, string name, Guid GUID, ELevelObjectPlacementOrigin placementOrigin)
		{
			return LevelObjects.addObject(position, rotation, scale, id, GUID, placementOrigin);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000A51C0 File Offset: 0x000A33C0
		internal static Transform addObject(Vector3 position, Quaternion rotation, Vector3 scale, ushort id, Guid GUID, ELevelObjectPlacementOrigin placementOrigin)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(position, out b, out b2))
			{
				LevelObject levelObject = new LevelObject(position, rotation, scale, id, GUID, placementOrigin, LevelObjects.generateUniqueInstanceID(), AssetReference<MaterialPaletteAsset>.invalid, -1, NetId.INVALID, true);
				levelObject.SetIsActiveInRegion(true);
				LevelObjects.objects[(int)b, (int)b2].Add(levelObject);
				LevelObjects._total++;
				return levelObject.transform;
			}
			return null;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000A5228 File Offset: 0x000A3428
		public static Transform addBuildable(Vector3 position, Quaternion rotation, ushort id)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(position, out b, out b2))
			{
				LevelBuildableObject levelBuildableObject = new LevelBuildableObject(position, rotation, id);
				levelBuildableObject.enable();
				LevelObjects.buildables[(int)b, (int)b2].Add(levelBuildableObject);
				LevelObjects._total++;
				return levelBuildableObject.transform;
			}
			return null;
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000A5276 File Offset: 0x000A3476
		public static Transform registerAddObject(Vector3 position, Quaternion rotation, Vector3 scale, ObjectAsset objectAsset, ItemAsset itemAsset)
		{
			return LevelObjects.register(new ReunObjectAdd(LevelObjects.step, objectAsset, itemAsset, position, rotation, scale));
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000A5290 File Offset: 0x000A3490
		public static void removeObject(Transform select)
		{
			if (select == null)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(select.position, out b, out b2))
			{
				for (int i = 0; i < LevelObjects.objects[(int)b, (int)b2].Count; i++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][i].transform == select)
					{
						LevelObjects.objects[(int)b, (int)b2][i].destroy();
						LevelObjects.objects[(int)b, (int)b2].RemoveAt(i);
						LevelObjects._total--;
						return;
					}
				}
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000A532C File Offset: 0x000A352C
		public static void removeBuildable(Transform select)
		{
			if (select == null)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(select.position, out b, out b2))
			{
				for (int i = 0; i < LevelObjects.buildables[(int)b, (int)b2].Count; i++)
				{
					if (LevelObjects.buildables[(int)b, (int)b2][i].transform == select)
					{
						LevelObjects.buildables[(int)b, (int)b2][i].destroy();
						LevelObjects.buildables[(int)b, (int)b2].RemoveAt(i);
						LevelObjects._total--;
						return;
					}
				}
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000A53C8 File Offset: 0x000A35C8
		public static void registerRemoveObject(Transform select)
		{
			if (select == null)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(select.position, out b, out b2))
			{
				if (select.CompareTag("Barricade") || select.CompareTag("Structure"))
				{
					for (int i = 0; i < LevelObjects.buildables[(int)b, (int)b2].Count; i++)
					{
						if (LevelObjects.buildables[(int)b, (int)b2][i].transform == select)
						{
							LevelObjects.register(new ReunObjectRemove(LevelObjects.step, select, null, LevelObjects.buildables[(int)b, (int)b2][i].asset, select.position, select.rotation, select.localScale));
							return;
						}
					}
					return;
				}
				for (int j = 0; j < LevelObjects.objects[(int)b, (int)b2].Count; j++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][j].transform == select)
					{
						LevelObjects.register(new ReunObjectRemove(LevelObjects.step, select, LevelObjects.objects[(int)b, (int)b2][j].asset, null, select.position, select.rotation, select.localScale));
						return;
					}
				}
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A54FC File Offset: 0x000A36FC
		public static ObjectAsset getAsset(Transform select)
		{
			if (select != null)
			{
				select = select.root;
			}
			byte b;
			byte b2;
			if (select != null && Regions.tryGetCoordinate(select.position, out b, out b2))
			{
				for (int i = 0; i < LevelObjects.objects[(int)b, (int)b2].Count; i++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][i].transform == select)
					{
						return LevelObjects.objects[(int)b, (int)b2][i].asset;
					}
				}
			}
			return null;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000A5588 File Offset: 0x000A3788
		public static void getAssetEditor(Transform select, out ObjectAsset objectAsset, out ItemAsset itemAsset)
		{
			objectAsset = null;
			itemAsset = null;
			if (select == null)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(select.position, out b, out b2))
			{
				if (select.CompareTag("Barricade") || select.CompareTag("Structure"))
				{
					for (int i = 0; i < LevelObjects.buildables[(int)b, (int)b2].Count; i++)
					{
						if (LevelObjects.buildables[(int)b, (int)b2][i].transform == select)
						{
							itemAsset = LevelObjects.buildables[(int)b, (int)b2][i].asset;
							return;
						}
					}
					return;
				}
				for (int j = 0; j < LevelObjects.objects[(int)b, (int)b2].Count; j++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][j].transform == select)
					{
						objectAsset = LevelObjects.objects[(int)b, (int)b2][j].asset;
						return;
					}
				}
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000A5680 File Offset: 0x000A3880
		internal static LevelObject FindLevelObject(GameObject rootGameObject)
		{
			if (rootGameObject == null)
			{
				return null;
			}
			Transform transform = rootGameObject.transform;
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(transform.position, out b, out b2))
			{
				for (int i = 0; i < LevelObjects.objects[(int)b, (int)b2].Count; i++)
				{
					if (LevelObjects.objects[(int)b, (int)b2][i].transform == transform)
					{
						return LevelObjects.objects[(int)b, (int)b2][i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000A5700 File Offset: 0x000A3900
		public static void load()
		{
			LevelObjects._objects = new List<LevelObject>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelObjects._buildables = new List<LevelBuildableObject>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelObjects._total = 0;
			LevelObjects._regions = new bool[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelObjects._loads = new int[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelObjects.shouldInstantlyLoad = true;
			LevelObjects.isHierarchyReady = false;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					LevelObjects.loads[(int)b, (int)b2] = -1;
				}
			}
			for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					LevelObjects.objects[(int)b3, (int)b4] = new List<LevelObject>();
					LevelObjects.buildables[(int)b3, (int)b4] = new List<LevelBuildableObject>();
				}
			}
			LevelObjects.hash = new byte[20];
			if (ReadWrite.fileExists(Level.info.path + "/Level/Objects.dat", false, false))
			{
				River river = new River(Level.info.path + "/Level/Objects.dat", false);
				byte b5 = river.readByte();
				LegacyObjectRedirectorMap legacyObjectRedirectorMap = null;
				if (Level.shouldUseHolidayRedirects)
				{
					legacyObjectRedirectorMap = new LegacyObjectRedirectorMap();
				}
				bool flag = Level.isEditor && EditorAssetRedirector.HasRedirects;
				LevelBatching levelBatching = null;
				if (b5 > 0)
				{
					if (b5 > 1 && b5 < 3)
					{
						river.readSteamID();
					}
					if (b5 > 8)
					{
						LevelObjects.availableInstanceID = river.readUInt32();
					}
					else
					{
						LevelObjects.availableInstanceID = 1U;
					}
					for (byte b6 = 0; b6 < Regions.WORLD_SIZE; b6 += 1)
					{
						for (byte b7 = 0; b7 < Regions.WORLD_SIZE; b7 += 1)
						{
							ushort num = river.readUInt16();
							for (ushort num2 = 0; num2 < num; num2 += 1)
							{
								Vector3 vector = river.readSingleVector3();
								Quaternion roundedIfNearlyAxisAligned = river.readSingleQuaternion().GetRoundedIfNearlyAxisAligned(0.05f);
								Vector3 newScale;
								if (b5 > 3)
								{
									newScale = river.readSingleVector3().GetRoundedIfNearlyEqualToOne(0.001f);
								}
								else
								{
									newScale = Vector3.one;
								}
								ushort num3 = river.readUInt16();
								if (b5 > 5 && b5 < 10)
								{
									river.readString();
								}
								Guid guid = Guid.Empty;
								if (b5 > 7)
								{
									guid = river.readGUID();
								}
								ELevelObjectPlacementOrigin newPlacementOrigin = ELevelObjectPlacementOrigin.MANUAL;
								if (b5 > 6)
								{
									newPlacementOrigin = (ELevelObjectPlacementOrigin)river.readByte();
								}
								uint newInstanceID;
								if (b5 > 8)
								{
									newInstanceID = river.readUInt32();
								}
								else
								{
									newInstanceID = LevelObjects.generateUniqueInstanceID();
								}
								if (legacyObjectRedirectorMap != null)
								{
									ObjectAsset objectAsset = legacyObjectRedirectorMap.redirect(guid);
									if (objectAsset == null)
									{
										num3 = 0;
										guid = Guid.Empty;
									}
									else
									{
										num3 = objectAsset.id;
										guid = objectAsset.GUID;
									}
								}
								else if (flag)
								{
									ObjectAsset objectAsset2 = EditorAssetRedirector.RedirectObject(guid);
									if (objectAsset2 != null)
									{
										num3 = objectAsset2.id;
										guid = objectAsset2.GUID;
									}
								}
								AssetReference<MaterialPaletteAsset> customMaterialOverride;
								int materialIndexOverride;
								if (b5 >= 11)
								{
									customMaterialOverride = new AssetReference<MaterialPaletteAsset>(river.readGUID());
									materialIndexOverride = river.readInt32();
								}
								else
								{
									customMaterialOverride = default(AssetReference<MaterialPaletteAsset>);
									materialIndexOverride = -1;
								}
								bool isOwnedCullingVolumeAllowed = b5 < 12 || river.readBoolean();
								if (guid != Guid.Empty || num3 != 0)
								{
									NetId regularObjectNetId = LevelNetIdRegistry.GetRegularObjectNetId(b6, b7, num2);
									LevelObject levelObject = new LevelObject(vector, roundedIfNearlyAxisAligned, newScale, num3, guid, newPlacementOrigin, newInstanceID, customMaterialOverride, materialIndexOverride, regularObjectNetId, isOwnedCullingVolumeAllowed);
									if (levelObject.asset == null && Assets.shouldLoadAnyAssets)
									{
										UnturnedLog.error("Object with no asset in region {0}, {1}: {2} {3}", new object[]
										{
											b6,
											b7,
											num3,
											guid
										});
									}
									byte b8 = b6;
									byte b9 = b7;
									if (Level.isEditor)
									{
										byte b10;
										byte b11;
										if (Regions.tryGetCoordinate(vector, out b10, out b11))
										{
											if (b10 != b6 || b11 != b7)
											{
												UnturnedLog.error(string.Concat(new string[]
												{
													num3.ToString(),
													" should be in ",
													b10.ToString(),
													", ",
													b11.ToString(),
													" but was in ",
													b6.ToString(),
													", ",
													b7.ToString(),
													"!"
												}));
												b8 = b10;
												b9 = b11;
											}
										}
										else
										{
											string format = "Object '{0}' ({1}) is outside the map bounds. Position: {2}";
											object[] array = new object[3];
											int num4 = 0;
											ObjectAsset asset = levelObject.asset;
											array[num4] = ((asset != null) ? asset.name : null);
											array[1] = num3;
											array[2] = vector;
											UnturnedLog.warn(format, array);
										}
									}
									LevelObjects.objects[(int)b8, (int)b9].Add(levelObject);
									if (levelBatching != null)
									{
										levelBatching.AddLevelObject(levelObject);
									}
									LevelObjects._total++;
								}
							}
						}
					}
				}
				LevelObjects.hash = river.getHash();
				river.closeRiver();
			}
			else
			{
				for (byte b12 = 0; b12 < Regions.WORLD_SIZE; b12 += 1)
				{
					for (byte b13 = 0; b13 < Regions.WORLD_SIZE; b13 += 1)
					{
						if (ReadWrite.fileExists(string.Concat(new string[]
						{
							Level.info.path,
							"/Objects/Objects_",
							b12.ToString(),
							"_",
							b13.ToString(),
							".dat"
						}), false, false))
						{
							River river2 = new River(string.Concat(new string[]
							{
								Level.info.path,
								"/Objects/Objects_",
								b12.ToString(),
								"_",
								b13.ToString(),
								".dat"
							}), false);
							if (river2.readByte() > 0)
							{
								ushort num5 = river2.readUInt16();
								for (ushort num6 = 0; num6 < num5; num6 += 1)
								{
									Vector3 position = river2.readSingleVector3();
									Quaternion rotation = river2.readSingleQuaternion();
									ushort num7 = river2.readUInt16();
									Guid empty = Guid.Empty;
									ELevelObjectPlacementOrigin placementOrigin = ELevelObjectPlacementOrigin.MANUAL;
									if (num7 != 0)
									{
										LevelObjects.addObject(position, rotation, Vector3.one, num7, empty, placementOrigin);
									}
								}
							}
							river2.closeRiver();
						}
					}
				}
			}
			if ((Provider.isServer || Level.isEditor) && ReadWrite.fileExists(Level.info.path + "/Level/Buildables.dat", false, false))
			{
				River river3 = new River(Level.info.path + "/Level/Buildables.dat", false);
				river3.readByte();
				for (byte b14 = 0; b14 < Regions.WORLD_SIZE; b14 += 1)
				{
					for (byte b15 = 0; b15 < Regions.WORLD_SIZE; b15 += 1)
					{
						ushort num8 = river3.readUInt16();
						for (ushort num9 = 0; num9 < num8; num9 += 1)
						{
							Vector3 vector2 = river3.readSingleVector3();
							Quaternion newRotation = river3.readSingleQuaternion();
							ushort num10 = river3.readUInt16();
							if (num10 != 0)
							{
								LevelBuildableObject levelBuildableObject = new LevelBuildableObject(vector2, newRotation, num10);
								if (levelBuildableObject.asset == null)
								{
									UnturnedLog.warn(string.Format("Missing asset for default buildable object ID {0} in region ({1}, {2})", num10, b14, b15));
								}
								else if (!(levelBuildableObject.asset is ItemBarricadeAsset) && !(levelBuildableObject.asset is ItemStructureAsset))
								{
									UnturnedLog.warn(string.Format("Default buildable object ID {0} in region ({1}, {2}) loaded as {3} (this is probably an ID conflict)", new object[]
									{
										num10,
										b14,
										b15,
										levelBuildableObject.asset.name
									}));
								}
								if (Level.isEditor)
								{
									byte b16;
									byte b17;
									if (Regions.tryGetCoordinate(vector2, out b16, out b17))
									{
										if (b16 != b14 || b17 != b15)
										{
											UnturnedLog.error(string.Concat(new string[]
											{
												num10.ToString(),
												" should be in ",
												b16.ToString(),
												", ",
												b17.ToString(),
												" but was in ",
												b14.ToString(),
												", ",
												b15.ToString(),
												"!"
											}));
											b14 = b16;
											b15 = b17;
										}
									}
									else
									{
										UnturnedLog.warn("Buildable {0} is outside the map bounds. Position: {1}", new object[]
										{
											num10,
											vector2
										});
									}
								}
								LevelObjects.buildables[(int)b14, (int)b15].Add(levelBuildableObject);
								LevelObjects._total++;
							}
						}
					}
				}
				river3.closeRiver();
			}
			if (Level.isEditor)
			{
				LevelObjects.reun = new IReun[256];
				LevelObjects.step = 0;
				LevelObjects.frame = 0;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000A5F70 File Offset: 0x000A4170
		public static void save()
		{
			River river = new River(Level.info.path + "/Level/Objects.dat", false);
			river.writeByte(12);
			river.writeUInt32(LevelObjects.availableInstanceID);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						LevelObject levelObject = list[(int)num];
						Transform transform = levelObject.transform;
						if (transform == null)
						{
							transform = levelObject.placeholderTransform;
						}
						if (levelObject != null && transform != null && (levelObject.GUID != Guid.Empty || levelObject.id != 0))
						{
							Vector3 position = transform.position;
							if (Regions.clampPositionIntoBounds(ref position))
							{
								string format = "Object '{0}' ({1}) was clamped into map bounds. Position: {2}";
								object[] array = new object[3];
								int num2 = 0;
								ObjectAsset asset = levelObject.asset;
								array[num2] = ((asset != null) ? asset.name : null);
								array[1] = levelObject.id;
								array[2] = position;
								UnturnedLog.warn(format, array);
							}
							river.writeSingleVector3(position);
							river.writeSingleQuaternion(transform.rotation);
							river.writeSingleVector3(transform.localScale);
							river.writeUInt16(levelObject.id);
							river.writeGUID(levelObject.GUID);
							river.writeByte((byte)levelObject.placementOrigin);
							river.writeUInt32(levelObject.instanceID);
							river.writeGUID(levelObject.customMaterialOverride.GUID);
							river.writeInt32(levelObject.materialIndexOverride);
							river.writeBoolean(levelObject.isOwnedCullingVolumeAllowed);
						}
						else
						{
							river.writeSingleVector3(Vector3.zero);
							river.writeSingleQuaternion(Quaternion.identity);
							river.writeSingleVector3(Vector3.one);
							river.writeUInt16(0);
							river.writeGUID(Guid.Empty);
							river.writeByte(0);
							river.writeUInt32(0U);
							river.writeGUID(Guid.Empty);
							river.writeInt32(-1);
							river.writeBoolean(true);
							string[] array2 = new string[8];
							array2[0] = "Found invalid object at ";
							array2[1] = b.ToString();
							array2[2] = ", ";
							array2[3] = b2.ToString();
							array2[4] = " with model: ";
							int num3 = 5;
							Transform transform2 = levelObject.transform;
							array2[num3] = ((transform2 != null) ? transform2.ToString() : null);
							array2[6] = " and ID: ";
							array2[7] = levelObject.id.ToString();
							UnturnedLog.error(string.Concat(array2));
						}
						num += 1;
					}
				}
			}
			river.closeRiver();
			River river2 = new River(Level.info.path + "/Level/Buildables.dat", false);
			river2.writeByte(LevelObjects.SAVEDATA_VERSION);
			for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					List<LevelBuildableObject> list2 = LevelObjects.buildables[(int)b3, (int)b4];
					river2.writeUInt16((ushort)list2.Count);
					ushort num4 = 0;
					while ((int)num4 < list2.Count)
					{
						LevelBuildableObject levelBuildableObject = list2[(int)num4];
						if (levelBuildableObject != null && levelBuildableObject.transform != null && levelBuildableObject.id != 0)
						{
							river2.writeSingleVector3(levelBuildableObject.transform.position);
							river2.writeSingleQuaternion(levelBuildableObject.transform.rotation);
							river2.writeUInt16(levelBuildableObject.id);
						}
						else
						{
							river2.writeSingleVector3(Vector3.zero);
							river2.writeSingleQuaternion(Quaternion.identity);
							river2.writeUInt16(0);
							string[] array3 = new string[8];
							array3[0] = "Found invalid object at ";
							array3[1] = b3.ToString();
							array3[2] = ", ";
							array3[3] = b4.ToString();
							array3[4] = " with model: ";
							int num5 = 5;
							Transform transform3 = levelBuildableObject.transform;
							array3[num5] = ((transform3 != null) ? transform3.ToString() : null);
							array3[6] = " and ID: ";
							array3[7] = levelBuildableObject.id.ToString();
							UnturnedLog.error(string.Concat(array3));
						}
						num4 += 1;
					}
				}
			}
			river2.closeRiver();
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000A639C File Offset: 0x000A459C
		private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
		{
			bool flag = true;
			LevelObjects.onRegionUpdated(null, old_x, old_y, new_x, new_y, 0, ref flag);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000A63B8 File Offset: 0x000A45B8
		private static void onPlayerTeleported(Player player, Vector3 position)
		{
			LevelObjects.shouldInstantlyLoad = true;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000A63C0 File Offset: 0x000A45C0
		private static void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step != 0)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelObjects.regions[(int)b, (int)b2] && !Regions.checkArea(b, b2, new_x, new_y, LevelObjects.OBJECT_REGIONS))
					{
						LevelObjects.regions[(int)b, (int)b2] = false;
						if (LevelObjects.shouldInstantlyLoad)
						{
							List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
							for (int i = 0; i < list.Count; i++)
							{
								list[i].SetIsActiveInRegion(false);
							}
						}
						else
						{
							LevelObjects.loads[(int)b, (int)b2] = 0;
							LevelObjects.isRegionalVisibilityDirty = true;
						}
						if (Level.isEditor)
						{
							List<LevelBuildableObject> list2 = LevelObjects.buildables[(int)b, (int)b2];
							for (int j = 0; j < list2.Count; j++)
							{
								list2[j].disable();
							}
						}
					}
				}
			}
			if (Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int k = (int)(new_x - LevelObjects.OBJECT_REGIONS); k <= (int)(new_x + LevelObjects.OBJECT_REGIONS); k++)
				{
					for (int l = (int)(new_y - LevelObjects.OBJECT_REGIONS); l <= (int)(new_y + LevelObjects.OBJECT_REGIONS); l++)
					{
						if (Regions.checkSafe((int)((byte)k), (int)((byte)l)) && !LevelObjects.regions[k, l])
						{
							LevelObjects.regions[k, l] = true;
							if (LevelObjects.shouldInstantlyLoad)
							{
								List<LevelObject> list3 = LevelObjects.objects[k, l];
								for (int m = 0; m < list3.Count; m++)
								{
									list3[m].SetIsActiveInRegion(true);
								}
							}
							else
							{
								LevelObjects.loads[k, l] = 0;
								LevelObjects.isRegionalVisibilityDirty = true;
							}
							if (Level.isEditor)
							{
								List<LevelBuildableObject> list4 = LevelObjects.buildables[k, l];
								for (int n = 0; n < list4.Count; n++)
								{
									list4[n].enable();
								}
							}
						}
					}
				}
			}
			if (Level.isLoadingArea && Player.player != null && Provider.isServer)
			{
				Player.player.adjustStanceOrTeleportIfStuck();
			}
			Level.isLoadingArea = false;
			LevelObjects.shouldInstantlyLoad = false;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000A65F8 File Offset: 0x000A47F8
		private static void onPlayerCreated(Player player)
		{
			if (player.channel.IsLocalPlayer)
			{
				Player player2 = Player.player;
				player2.onPlayerTeleported = (PlayerTeleported)Delegate.Combine(player2.onPlayerTeleported, new PlayerTeleported(LevelObjects.onPlayerTeleported));
				PlayerMovement movement = Player.player.movement;
				movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(LevelObjects.onRegionUpdated));
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000A6663 File Offset: 0x000A4863
		private static void handleEditorAreaRegistered(EditorArea area)
		{
			area.onRegionUpdated = (EditorRegionUpdated)Delegate.Combine(area.onRegionUpdated, new EditorRegionUpdated(LevelObjects.onRegionUpdated));
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000A6687 File Offset: 0x000A4887
		private static void handleLevelHierarchyReady()
		{
			LevelObjects.isHierarchyReady = true;
		}

		/// <summary>
		/// Called by navmesh baking to complete pending object changes that may affect which nav objects are enabled.
		/// </summary>
		// Token: 0x060027A2 RID: 10146 RVA: 0x000A6690 File Offset: 0x000A4890
		internal static void ImmediatelySyncRegionalVisibility()
		{
			if (!LevelObjects.isRegionalVisibilityDirty)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelObjects.loads[(int)b, (int)b2] != -1)
					{
						bool isActiveInRegion = LevelObjects.regions[(int)b, (int)b2];
						foreach (LevelObject levelObject in LevelObjects.objects[(int)b, (int)b2])
						{
							levelObject.SetIsActiveInRegion(isActiveInRegion);
						}
						LevelObjects.loads[(int)b, (int)b2] = -1;
					}
				}
			}
			LevelObjects.isRegionalVisibilityDirty = false;
		}

		/// <summary>
		/// Stagger regional visibility across multiple frames.
		/// </summary>
		// Token: 0x060027A3 RID: 10147 RVA: 0x000A6744 File Offset: 0x000A4944
		private void tickRegionalVisibility()
		{
			bool flag = true;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					int num = LevelObjects.loads[(int)b, (int)b2];
					if (num != -1)
					{
						if (num >= LevelObjects.objects[(int)b, (int)b2].Count)
						{
							LevelObjects.loads[(int)b, (int)b2] = -1;
							RegionActivated regionActivated = LevelObjects.onRegionActivated;
							if (regionActivated != null)
							{
								regionActivated(b, b2);
							}
						}
						else
						{
							bool isActiveInRegion = LevelObjects.regions[(int)b, (int)b2];
							LevelObjects.objects[(int)b, (int)b2][num].SetIsActiveInRegion(isActiveInRegion);
							LevelObjects.loads[(int)b, (int)b2]++;
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				LevelObjects.isRegionalVisibilityDirty = false;
			}
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000A680A File Offset: 0x000A4A0A
		private void Update()
		{
			bool isLoaded = Level.isLoaded;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000A6814 File Offset: 0x000A4A14
		public void Start()
		{
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(LevelObjects.onPlayerCreated));
			EditorArea.registered += LevelObjects.handleEditorAreaRegistered;
			LevelHierarchy.ready += LevelObjects.handleLevelHierarchyReady;
		}

		/// <summary>
		/// Should objects that failed to load due to missing assets be saved?
		/// If true, a placeholder transform is created and used to save.
		/// If false, objects without assets are zeroed during save. (old default)  
		/// </summary>
		// Token: 0x040014ED RID: 5357
		public static CommandLineFlag preserveMissingAssets = new CommandLineFlag(true, "-NoPreserveMissingObjects");

		// Token: 0x040014EE RID: 5358
		private const byte SAVEDATA_VERSION_BEFORE_NAMED_VERSIONS = 10;

		// Token: 0x040014EF RID: 5359
		private const byte SAVEDATA_VERSION_ADDED_MATERIAL_OVERRIDES = 11;

		// Token: 0x040014F0 RID: 5360
		private const byte SAVEDATA_VERSION_ADDED_PER_OBJECT_CULLING_OVERRIDES = 12;

		// Token: 0x040014F1 RID: 5361
		private const byte SAVEDATA_VERSION_NEWEST = 12;

		// Token: 0x040014F2 RID: 5362
		public static readonly byte SAVEDATA_VERSION = 12;

		// Token: 0x040014F3 RID: 5363
		public static readonly byte OBJECT_REGIONS = 3;

		// Token: 0x040014F4 RID: 5364
		private static uint availableInstanceID;

		// Token: 0x040014F5 RID: 5365
		private static IReun[] reun;

		// Token: 0x040014F6 RID: 5366
		public static int step;

		// Token: 0x040014F7 RID: 5367
		private static int frame;

		// Token: 0x040014F8 RID: 5368
		private static Transform _models;

		// Token: 0x040014F9 RID: 5369
		private static List<LevelObject>[,] _objects;

		// Token: 0x040014FA RID: 5370
		private static List<LevelBuildableObject>[,] _buildables;

		// Token: 0x040014FB RID: 5371
		private static int _total;

		// Token: 0x040014FC RID: 5372
		private static bool[,] _regions;

		// Token: 0x040014FD RID: 5373
		private static bool isHierarchyReady;

		// Token: 0x040014FE RID: 5374
		private static int[,] _loads;

		// Token: 0x040014FF RID: 5375
		private static bool isRegionalVisibilityDirty = true;

		// Token: 0x04001501 RID: 5377
		public static RegionActivated onRegionActivated;
	}
}
