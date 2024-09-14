using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Foliage;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020004D9 RID: 1241
	public class Level : MonoBehaviour
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x00095E24 File Offset: 0x00094024
		public static ushort border
		{
			get
			{
				if (Level.info == null)
				{
					return 1;
				}
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_BORDER;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_BORDER;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_BORDER;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_BORDER;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_BORDER;
				}
				return 0;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x00095E9C File Offset: 0x0009409C
		public static ushort size
		{
			get
			{
				if (Level.info == null)
				{
					return 8;
				}
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_SIZE;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_SIZE;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_SIZE;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_SIZE;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_SIZE;
				}
				return 0;
			}
		}

		/// <summary>
		/// Is a point safely within the level bounds?
		/// Also checks player clip volumes if legacy borders are disabled.
		/// </summary>
		// Token: 0x060025CE RID: 9678 RVA: 0x00095F14 File Offset: 0x00094114
		public static bool checkSafeIncludingClipVolumes(Vector3 point)
		{
			if (Level.info != null && !Level.info.configData.Use_Legacy_Clip_Borders)
			{
				return !VolumeManager<PlayerClipVolume, PlayerClipVolumeManager>.Get().IsPositionInsideAnyVolume(point);
			}
			if (!Level.isPointWithinValidHeight(point.y))
			{
				return false;
			}
			Vector3 vector = new Vector3(Mathf.Abs(point.x), point.y, Mathf.Abs(point.z));
			return vector.x <= (float)(Level.size / 2 - Level.border) && vector.z <= (float)(Level.size / 2 - Level.border);
		}

		/// <summary>
		/// Is given Y (vertical) coordinate within level's height range?
		/// Maps using landscapes have a larger range than older maps.
		/// </summary>
		// Token: 0x060025CF RID: 9679 RVA: 0x00095FA9 File Offset: 0x000941A9
		public static bool isPointWithinValidHeight(float y)
		{
			return y >= -1024f && y <= 1024f;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00095FC0 File Offset: 0x000941C0
		[Obsolete("Replaced by checkSafeIncludingClipVolumes or the newer isPointWithinValidHeight")]
		public static bool checkLevel(Vector3 point)
		{
			return Level.checkSafeIncludingClipVolumes(point);
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060025D1 RID: 9681 RVA: 0x00095FC8 File Offset: 0x000941C8
		// (remove) Token: 0x060025D2 RID: 9682 RVA: 0x00095FFC File Offset: 0x000941FC
		public static event LevelLoadingStepHandler loadingSteps;

		/// <summary>
		/// Notify menus that levels list has changed.
		/// Used when creating/deleting levels, as well as following workshop changes.
		/// </summary>
		// Token: 0x060025D3 RID: 9683 RVA: 0x0009602F File Offset: 0x0009422F
		public static void broadcastLevelsRefreshed()
		{
			LevelsRefreshed levelsRefreshed = Level.onLevelsRefreshed;
			if (levelsRefreshed == null)
			{
				return;
			}
			levelsRefreshed();
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x00096040 File Offset: 0x00094240
		public static LevelInfo info
		{
			get
			{
				return Level._info;
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00096047 File Offset: 0x00094247
		private static void ResetCachedLevelAsset()
		{
			Level.cachedLevelAsset = null;
			Level.didResolveLevelAsset = false;
		}

		/// <summary>
		/// Get level's cached asset, if any.
		/// </summary>
		// Token: 0x060025D6 RID: 9686 RVA: 0x00096058 File Offset: 0x00094258
		public static LevelAsset getAsset()
		{
			if (!Level.didResolveLevelAsset)
			{
				Level.didResolveLevelAsset = true;
				if (Level.info != null && Level.info.configData != null && Level.info.configData.Asset.isValid)
				{
					Level.cachedLevelAsset = Assets.find<LevelAsset>(Level.info.configData.Asset);
					if (Level.cachedLevelAsset == null)
					{
						UnturnedLog.warn("Unable to find level asset {0} for {1}", new object[]
						{
							Level.info.configData.Asset,
							Level.info.name
						});
					}
				}
				if (Level.cachedLevelAsset == null)
				{
					Level.cachedLevelAsset = Assets.find<LevelAsset>(LevelAsset.defaultLevel);
					if (Level.cachedLevelAsset == null)
					{
						UnturnedLog.error("Unable to find default level asset");
					}
				}
			}
			return Level.cachedLevelAsset;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x0009611E File Offset: 0x0009431E
		private static void updateCachedHolidayRedirects()
		{
			Level.shouldUseHolidayRedirects = (!Level.isEditor && Level.info != null && Level.info.configData != null && Level.info.configData.Allow_Holiday_Redirects && HolidayUtil.getActiveHoliday() > ENPCHoliday.NONE);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x0009615B File Offset: 0x0009435B
		private static void UpdateShouldUseLevelBatching()
		{
		}

		/// <summary>
		/// Should loading code proceed with redirects?
		/// Disabled by level and when in the editor.
		/// </summary>
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060025D9 RID: 9689 RVA: 0x0009615D File Offset: 0x0009435D
		// (set) Token: 0x060025DA RID: 9690 RVA: 0x00096164 File Offset: 0x00094364
		public static bool shouldUseHolidayRedirects { get; private set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060025DB RID: 9691 RVA: 0x0009616C File Offset: 0x0009436C
		public static Transform level
		{
			get
			{
				return Level._level;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x00096173 File Offset: 0x00094373
		public static Transform roots
		{
			get
			{
				return Level._roots;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x0009617A File Offset: 0x0009437A
		public static Transform clips
		{
			get
			{
				return Level._clips;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x00096184 File Offset: 0x00094384
		[Obsolete("Was the parent of all effects in the past, but now empty for TransformHierarchy performance.")]
		public static Transform effects
		{
			get
			{
				if (Level._effects == null)
				{
					Level._effects = new GameObject().transform;
					Level._effects.name = "Effects";
					Level._effects.parent = Level.level;
					Level._effects.tag = "Logic";
					Level._effects.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing Level.effecs which has been deprecated.", Array.Empty<object>());
				}
				return Level._effects;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x00096200 File Offset: 0x00094400
		[Obsolete("Was the parent of gameplay objects in the past, but now empty for TransformHierarchy performance.")]
		public static Transform spawns
		{
			get
			{
				if (Level._spawns == null)
				{
					Level._spawns = new GameObject().transform;
					Level._spawns.name = "Spawns";
					Level._spawns.parent = Level.level;
					Level._spawns.tag = "Logic";
					Level._spawns.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing Level.spawns which has been deprecated.", Array.Empty<object>());
				}
				return Level._spawns;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x0009627A File Offset: 0x0009447A
		public static Transform editing
		{
			get
			{
				return Level._editing;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x00096281 File Offset: 0x00094481
		public static bool isInitialized
		{
			get
			{
				return Level._isInitialized;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x00096288 File Offset: 0x00094488
		public static bool isEditor
		{
			get
			{
				return Level._isEditor;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060025E3 RID: 9699 RVA: 0x0009628F File Offset: 0x0009448F
		// (set) Token: 0x060025E4 RID: 9700 RVA: 0x00096296 File Offset: 0x00094496
		public static bool isExiting { get; protected set; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x0009629E File Offset: 0x0009449E
		public static bool isVR
		{
			get
			{
				return PlaySettings.isVR && Level.isEditor;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x000962B0 File Offset: 0x000944B0
		public static bool isLoading
		{
			get
			{
				if (Provider.isConnected)
				{
					return Level.isLoadingContent || Level.isLoadingLighting || Level.isLoadingVehicles || Level.isLoadingBarricades || Level.isLoadingStructures || Level.isLoadingArea;
				}
				return Level.isEditor && Level.isLoadingContent;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060025E7 RID: 9703 RVA: 0x000962FD File Offset: 0x000944FD
		public static bool isLoaded
		{
			get
			{
				return Level._isLoaded;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00096304 File Offset: 0x00094504
		// (set) Token: 0x060025E9 RID: 9705 RVA: 0x0009630B File Offset: 0x0009450B
		public static byte[] hash { get; private set; }

		// Token: 0x060025EA RID: 9706 RVA: 0x00096313 File Offset: 0x00094513
		public static void includeHash(string id, byte[] pendingHash)
		{
			if (Level.shouldLogLevelHash)
			{
				UnturnedLog.info(string.Format("[{0}] Including \"{1}\" in level hash: {2}", Level.pendingHashes.Count, id, Hash.toString(pendingHash)));
			}
			Level.pendingHashes.Add(pendingHash);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x00096351 File Offset: 0x00094551
		private static void combineHashes()
		{
			Level.hash = Hash.combine(Level.pendingHashes);
			if (Level.shouldLogLevelHash)
			{
				UnturnedLog.info("Combined level hash: " + Hash.toString(Level.hash));
			}
		}

		/// <summary>
		/// Display version string of the currently loaded level.
		/// </summary>
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x00096387 File Offset: 0x00094587
		public static string version
		{
			get
			{
				if (Level.info == null || Level.info.configData == null)
				{
					return "0.0.0.0";
				}
				return Level.info.configData.Version;
			}
		}

		/// <summary>
		/// Version string of the currently loaded level packed into an integer.
		/// </summary>
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x000963B1 File Offset: 0x000945B1
		public static uint packedVersion
		{
			get
			{
				if (Level.info == null || Level.info.configData == null)
				{
					return 0U;
				}
				return Level.info.configData.PackedVersion;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000963D7 File Offset: 0x000945D7
		public static void setEnabled(bool isEnabled)
		{
			Level.clips.gameObject.SetActive(isEnabled);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000963EC File Offset: 0x000945EC
		public static void add(string name, ELevelSize size, ELevelType type)
		{
			if (!ReadWrite.folderExists("/Maps/" + name))
			{
				ReadWrite.createFolder("/Maps/" + name);
				Block block = new Block();
				block.writeByte(Level.SAVEDATA_VERSION);
				block.writeSteamID(Provider.client);
				block.writeByte((byte)size);
				block.writeByte((byte)type);
				ReadWrite.writeBlock("/Maps/" + name + "/Level.dat", false, block);
				ReadWrite.copyFile("/Extras/LevelTemplate/Charts.unity3d", "/Maps/" + name + "/Charts.unity3d");
				ReadWrite.copyFile("/Extras/LevelTemplate/Details.unity3d", "/Maps/" + name + "/Terrain/Details.unity3d");
				ReadWrite.copyFile("/Extras/LevelTemplate/Details.dat", "/Maps/" + name + "/Terrain/Details.dat");
				ReadWrite.copyFile("/Extras/LevelTemplate/Materials.unity3d", "/Maps/" + name + "/Terrain/Materials.unity3d");
				ReadWrite.copyFile("/Extras/LevelTemplate/Materials.dat", "/Maps/" + name + "/Terrain/Materials.dat");
				ReadWrite.copyFile("/Extras/LevelTemplate/Resources.dat", "/Maps/" + name + "/Terrain/Resources.dat");
				ReadWrite.copyFile("/Extras/LevelTemplate/Lighting.dat", "/Maps/" + name + "/Environment/Lighting.dat");
				ReadWrite.copyFile("/Extras/LevelTemplate/Roads.unity3d", "/Maps/" + name + "/Environment/Roads.unity3d");
				ReadWrite.copyFile("/Extras/LevelTemplate/Roads.dat", "/Maps/" + name + "/Environment/Roads.dat");
				ReadWrite.copyFile("/Extras/LevelTemplate/Ambience.unity3d", "/Maps/" + name + "/Environment/Ambience.unity3d");
				Level.broadcastLevelsRefreshed();
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0009656A File Offset: 0x0009476A
		public static void remove(string name)
		{
			ReadWrite.deleteFolder("/Maps/" + name);
			Level.broadcastLevelsRefreshed();
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x00096584 File Offset: 0x00094784
		public static void save()
		{
			DirtyManager.save();
			LevelObjects.save();
			LevelLighting.save();
			LevelGround.save();
			LevelRoads.save();
			if (!Level.isVR)
			{
				LevelNavigation.save();
				LevelNodes.save();
				LevelItems.save();
				LevelPlayers.save();
				LevelZombies.save();
				LevelVehicles.save();
				LevelAnimals.save();
				LevelVisibility.save();
			}
			Editor.save();
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000965E0 File Offset: 0x000947E0
		public static void edit(LevelInfo newInfo)
		{
			Level._isEditor = true;
			Level.isExiting = false;
			Level._info = newInfo;
			Level.ResetCachedLevelAsset();
			LoadingUI.updateScene();
			SceneManager.LoadScene("Game");
			Provider.resetChannels();
			Provider.updateRichPresence();
			DevkitTransactionManager.resetTransactions();
			Level.updateCachedHolidayRedirects();
			Level.UpdateShouldUseLevelBatching();
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0009662C File Offset: 0x0009482C
		public static void load(LevelInfo newInfo, bool hasAuthority)
		{
			Level._isEditor = false;
			Level.isExiting = false;
			Level._info = newInfo;
			Level.ResetCachedLevelAsset();
			LoadingUI.updateScene();
			SceneManager.LoadScene("Game");
			if (hasAuthority)
			{
				string text = LevelSavedata.transformName("Cyrpus Survival");
				string text2 = LevelSavedata.transformName("Cyprus Survival");
				if (ReadWrite.folderExists(text) && !ReadWrite.folderExists(text2))
				{
					ReadWrite.moveFolder(text, text2);
					UnturnedLog.info("Moved Cyprus save folder");
				}
			}
			Provider.updateRichPresence();
			DevkitTransactionManager.resetTransactions();
			Level.updateCachedHolidayRedirects();
			Level.UpdateShouldUseLevelBatching();
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000966AD File Offset: 0x000948AD
		public static void loading()
		{
			SceneManager.LoadScene("Loading");
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000966B9 File Offset: 0x000948B9
		public static void exit()
		{
			LevelExited levelExited = Level.onLevelExited;
			if (levelExited != null)
			{
				levelExited();
			}
			Level._isEditor = false;
			Level.isExiting = true;
			Level._info = null;
			Level.ResetCachedLevelAsset();
			LoadingUI.updateScene();
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000966E8 File Offset: 0x000948E8
		public static LevelInfo getLevel(string name)
		{
			Level.ScanKnownLevels();
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (string.Equals(name, levelInfo.name, 5))
				{
					return levelInfo;
				}
			}
			return null;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x00096750 File Offset: 0x00094950
		private static LevelInfo ReadLevelInfo(string path, bool usePath, ulong publishedFileId = 0UL)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			return Level.ReadLevelInfo(path, publishedFileId);
		}

		/// <summary>
		/// Load level details from Level.dat in directory path.
		/// </summary>
		// Token: 0x060025F8 RID: 9720 RVA: 0x0009676C File Offset: 0x0009496C
		private static LevelInfo ReadLevelInfo(string directoryPath, ulong publishedFileId = 0UL)
		{
			LevelInfo result;
			try
			{
				string text = Path.Combine(directoryPath, "Level.dat");
				if (!File.Exists(text))
				{
					result = null;
				}
				else
				{
					Block block = ReadWrite.readBlock(text, false, false, 0);
					byte b = block.readByte();
					byte[] hash = block.getHash();
					bool newEditable = block.readSteamID() == Provider.client || ReadWrite.fileExists(Path.Combine(directoryPath, ".unlocker"), false, false);
					ELevelSize newSize = (ELevelSize)block.readByte();
					ELevelType newType = ELevelType.SURVIVAL;
					if (b > 1)
					{
						newType = (ELevelType)block.readByte();
					}
					string text2 = ReadWrite.folderName(directoryPath);
					string text3 = Path.Combine(directoryPath, "Config.json");
					LevelInfoConfigData levelInfoConfigData;
					if (File.Exists(text3))
					{
						try
						{
							using (FileStream fileStream = new FileStream(text3, 3, 1, 1))
							{
								using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
								{
									using (StreamReader streamReader = new StreamReader(sha1Stream))
									{
										levelInfoConfigData = JsonConvert.DeserializeObject<LevelInfoConfigData>(streamReader.ReadToEnd());
										levelInfoConfigData.Hash = sha1Stream.Hash;
									}
								}
							}
						}
						catch
						{
							Assets.reportError(string.Format("Unable to parse {0}/Config.json! Consider validating with a JSON linter", text2));
							levelInfoConfigData = null;
						}
						if (levelInfoConfigData == null)
						{
							levelInfoConfigData = new LevelInfoConfigData();
						}
					}
					else
					{
						levelInfoConfigData = new LevelInfoConfigData();
					}
					if (!Parser.TryGetUInt32FromIP(levelInfoConfigData.Version, out levelInfoConfigData.PackedVersion))
					{
						Assets.reportError(string.Format("Unable to parse level \"{0}\" version \"{1}\". Expected format \"#.#.#.#\". Resetting to zero.", text2, levelInfoConfigData.PackedVersion));
						levelInfoConfigData.Version = "0.0.0.0";
						levelInfoConfigData.PackedVersion = 0U;
					}
					result = new LevelInfo(directoryPath, text2, newSize, newType, newEditable, levelInfoConfigData, publishedFileId, hash);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception reading level info file (" + directoryPath + "):");
				result = null;
			}
			return result;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x00096994 File Offset: 0x00094B94
		private static bool doesLevelPassFilter(LevelInfo levelInfo, ESingleplayerMapCategory categoryFilter)
		{
			switch (categoryFilter)
			{
			case ESingleplayerMapCategory.OFFICIAL:
				return levelInfo.configData.Category == ESingleplayerMapCategory.OFFICIAL;
			case ESingleplayerMapCategory.CURATED:
				return levelInfo.type != ELevelType.ARENA && levelInfo.isCurated;
			case ESingleplayerMapCategory.WORKSHOP:
				return levelInfo.isFromWorkshop && !levelInfo.isCurated;
			case ESingleplayerMapCategory.MISC:
			{
				bool flag = levelInfo.type != ELevelType.ARENA && levelInfo.isCurated;
				bool flag2 = levelInfo.configData.Category == ESingleplayerMapCategory.OFFICIAL || levelInfo.isFromWorkshop || flag;
				return levelInfo.configData.Category == ESingleplayerMapCategory.MISC || (levelInfo.isEditable && !flag2);
			}
			case ESingleplayerMapCategory.ALL:
				return true;
			case ESingleplayerMapCategory.EDITABLE:
				return levelInfo.isEditable;
			default:
				UnturnedLog.warn("Unknown map filter '{0}'", new object[]
				{
					categoryFilter
				});
				return true;
			}
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x00096A6C File Offset: 0x00094C6C
		public static LevelInfo[] getLevels(ESingleplayerMapCategory categoryFilter)
		{
			Level.ScanKnownLevels();
			List<LevelInfo> list = new List<LevelInfo>();
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (Level.doesLevelPassFilter(levelInfo, categoryFilter))
				{
					list.Add(levelInfo);
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// Server list allows player to enter a map name when searching, so we try to find a local
		/// copy of the level for version number comparison. (Server map version might differ.)
		/// </summary>
		// Token: 0x060025FB RID: 9723 RVA: 0x00096AD8 File Offset: 0x00094CD8
		public static LevelInfo findLevelForServerFilter(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter) || filter.Length < 2)
			{
				return null;
			}
			Level.ScanKnownLevels();
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (levelInfo.configData != null && levelInfo.configData.PackedVersion != 0U && levelInfo.name.StartsWith(filter, 5))
				{
					return levelInfo;
				}
			}
			return null;
		}

		/// <summary>
		/// New map filter uses lowercase map name and doesn't need startswith.
		/// </summary>
		// Token: 0x060025FC RID: 9724 RVA: 0x00096B68 File Offset: 0x00094D68
		public static LevelInfo FindLevelForServerFilterExact(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter) || filter.Length < 2)
			{
				return null;
			}
			Level.ScanKnownLevels();
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (levelInfo.configData != null && levelInfo.configData.PackedVersion != 0U && levelInfo.name.Equals(filter, 5))
				{
					return levelInfo;
				}
			}
			return null;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x00096BF8 File Offset: 0x00094DF8
		private static LevelInfo FindKnownLevelByPath(string path)
		{
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (string.Equals(levelInfo.path, path))
				{
					return levelInfo;
				}
			}
			return null;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x00096C58 File Offset: 0x00094E58
		private static LevelInfo FindKnownLevelByPublishedFileId(ulong fileId)
		{
			foreach (LevelInfo levelInfo in Level.knownLevels)
			{
				if (levelInfo.publishedFileId == fileId)
				{
					return levelInfo;
				}
			}
			return null;
		}

		/// <summary>
		/// Search all map folders to add any previously unregistered maps.
		/// </summary>
		// Token: 0x060025FF RID: 9727 RVA: 0x00096CB4 File Offset: 0x00094EB4
		private static void ScanKnownLevels()
		{
			try
			{
				for (int i = Level.knownLevels.Count - 1; i >= 0; i--)
				{
					LevelInfo levelInfo = Level.knownLevels[i];
					if (!Directory.Exists(levelInfo.path))
					{
						Level.knownLevels.RemoveAt(i);
						UnturnedLog.info(string.Concat(new string[]
						{
							"Removed previously discovered level \"",
							levelInfo.name,
							"\" at \"",
							levelInfo.path,
							"\" (no longer exists)"
						}));
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception checking for deleted levels:");
			}
			try
			{
				foreach (string text in Directory.GetDirectories(PathEx.Join(UnturnedPaths.RootDirectory, "Maps")))
				{
					if (Level.FindKnownLevelByPath(text) == null)
					{
						LevelInfo levelInfo2 = Level.ReadLevelInfo(text, 0UL);
						if (levelInfo2 != null)
						{
							Level.knownLevels.Add(levelInfo2);
							UnturnedLog.info(string.Concat(new string[]
							{
								"Discovered level \"",
								levelInfo2.name,
								"\" at \"",
								levelInfo2.path,
								"\""
							}));
						}
					}
				}
			}
			catch (Exception e2)
			{
				UnturnedLog.exception(e2, "Caught exception loading levels in root Maps folder:");
			}
			if (Provider.provider.workshopService.ugc != null)
			{
				try
				{
					foreach (SteamContent steamContent in Provider.provider.workshopService.ugc)
					{
						if (steamContent.type == ESteamUGCType.MAP && LocalWorkshopSettings.get().getEnabled(steamContent.publishedFileID) && Level.FindKnownLevelByPublishedFileId(steamContent.publishedFileID.m_PublishedFileId) == null)
						{
							LevelInfo levelInfo3 = Level.ReadLevelInfo(ReadWrite.folderFound(steamContent.path, false), steamContent.publishedFileID.m_PublishedFileId);
							if (levelInfo3 != null)
							{
								Level.knownLevels.Add(levelInfo3);
								UnturnedLog.info(string.Concat(new string[]
								{
									"Discovered level \"",
									levelInfo3.name,
									"\" at \"",
									levelInfo3.path,
									"\""
								}));
							}
						}
					}
					goto IL_46B;
				}
				catch (Exception e3)
				{
					UnturnedLog.exception(e3, "Caught exception loading levels from Steam Workshop:");
					goto IL_46B;
				}
			}
			string text2 = PathEx.Join(UnturnedPaths.RootDirectory, "Bundles", "Workshop", "Maps");
			try
			{
				if (!ReadWrite.folderExists(text2, false))
				{
					ReadWrite.createFolder(text2, false);
				}
				foreach (string text3 in Directory.GetDirectories(text2))
				{
					if (Level.FindKnownLevelByPath(text3) == null)
					{
						LevelInfo levelInfo4 = Level.ReadLevelInfo(text3, 0UL);
						if (levelInfo4 != null)
						{
							Level.knownLevels.Add(levelInfo4);
							UnturnedLog.info(string.Concat(new string[]
							{
								"Discovered level \"",
								levelInfo4.name,
								"\" at \"",
								levelInfo4.path,
								"\""
							}));
						}
					}
				}
			}
			catch (Exception e4)
			{
				UnturnedLog.exception(e4, "Caught exception loading levels in legacy server global workshop Maps folder (" + text2 + "):");
			}
			string text4 = PathEx.Join(UnturnedPaths.RootDirectory, ServerSavedata.directoryName, Provider.serverID, "Workshop", "Maps");
			try
			{
				if (!ReadWrite.folderExists(text4, false))
				{
					ReadWrite.createFolder(text4, false);
				}
				foreach (string text5 in Directory.GetDirectories(text4))
				{
					if (Level.FindKnownLevelByPath(text5) == null)
					{
						LevelInfo levelInfo5 = Level.ReadLevelInfo(text5, 0UL);
						if (levelInfo5 != null)
						{
							Level.knownLevels.Add(levelInfo5);
							UnturnedLog.info(string.Concat(new string[]
							{
								"Discovered level \"",
								levelInfo5.name,
								"\" at \"",
								levelInfo5.path,
								"\""
							}));
						}
					}
				}
			}
			catch (Exception e5)
			{
				UnturnedLog.exception(e5, "Caught exception loading levels in legacy per-server workshop Maps folder (" + text4 + "):");
			}
			string text6 = PathEx.Join(UnturnedPaths.RootDirectory, ServerSavedata.directoryName, Provider.serverID, "Maps");
			try
			{
				if (!ReadWrite.folderExists(text6, false))
				{
					ReadWrite.createFolder(text6, false);
				}
				foreach (string text7 in Directory.GetDirectories(text6))
				{
					if (Level.FindKnownLevelByPath(text7) == null)
					{
						LevelInfo levelInfo6 = Level.ReadLevelInfo(text7, 0UL);
						if (levelInfo6 != null)
						{
							Level.knownLevels.Add(levelInfo6);
							UnturnedLog.info(string.Concat(new string[]
							{
								"Discovered level \"",
								levelInfo6.name,
								"\" at \"",
								levelInfo6.path,
								"\""
							}));
						}
					}
				}
			}
			catch (Exception e6)
			{
				UnturnedLog.exception(e6, "Caught exception loading levels in legacy per-server Maps folder (" + text6 + "):");
			}
			IL_46B:
			if (DedicatedUGC.ugc != null)
			{
				try
				{
					foreach (SteamContent steamContent2 in DedicatedUGC.ugc)
					{
						if (steamContent2.type == ESteamUGCType.MAP && Level.FindKnownLevelByPublishedFileId(steamContent2.publishedFileID.m_PublishedFileId) == null)
						{
							LevelInfo levelInfo7 = Level.ReadLevelInfo(ReadWrite.folderFound(steamContent2.path, false), steamContent2.publishedFileID.m_PublishedFileId);
							if (levelInfo7 != null)
							{
								Level.knownLevels.Add(levelInfo7);
								UnturnedLog.info(string.Concat(new string[]
								{
									"Discovered level \"",
									levelInfo7.name,
									"\" at \"",
									levelInfo7.path,
									"\""
								}));
							}
						}
					}
				}
				catch (Exception e7)
				{
					UnturnedLog.exception(e7, "Caught exception loading levels from server Steam Workshop:");
				}
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06002600 RID: 9728 RVA: 0x00097274 File Offset: 0x00095474
		// (remove) Token: 0x06002601 RID: 9729 RVA: 0x000972A8 File Offset: 0x000954A8
		public static event Level.SatelliteCaptureDelegate onSatellitePreCapture;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06002602 RID: 9730 RVA: 0x000972DC File Offset: 0x000954DC
		// (remove) Token: 0x06002603 RID: 9731 RVA: 0x00097310 File Offset: 0x00095510
		public static event Level.SatelliteCaptureDelegate onSatellitePostCapture;

		// Token: 0x06002604 RID: 9732 RVA: 0x00097343 File Offset: 0x00095543
		public static void bindSatelliteCaptureInEditor(Level.SatelliteCaptureDelegate preCapture, Level.SatelliteCaptureDelegate postCapture)
		{
			if (Level.isEditor)
			{
				Level.onSatellitePreCapture += preCapture;
				Level.onSatellitePostCapture += postCapture;
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00097358 File Offset: 0x00095558
		public static void unbindSatelliteCapture(Level.SatelliteCaptureDelegate preCapture, Level.SatelliteCaptureDelegate postCapture)
		{
			Level.onSatellitePreCapture -= preCapture;
			Level.onSatellitePostCapture -= postCapture;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00097368 File Offset: 0x00095568
		private static Level.PreCaptureObjectState GetObjectState()
		{
			Level.PreCaptureObjectState preCaptureObjectState = new Level.PreCaptureObjectState();
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					foreach (LevelObject levelObject in LevelObjects.objects[(int)b, (int)b2])
					{
						ObjectAsset asset = levelObject.asset;
						bool isActiveOverrideForSatelliteCapture = asset != null && !asset.ShouldExcludeFromSatelliteCapture;
						levelObject.SetIsActiveOverrideForSatelliteCapture(isActiveOverrideForSatelliteCapture);
					}
					List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
					preCaptureObjectState.wasTreeEnabled[(int)b, (int)b2] = new bool[list.Count];
					preCaptureObjectState.wasTreeSkyboxEnabled[(int)b, (int)b2] = new bool[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						ResourceSpawnpoint resourceSpawnpoint = list[i];
						preCaptureObjectState.wasTreeEnabled[(int)b, (int)b2][i] = resourceSpawnpoint.isEnabled;
						preCaptureObjectState.wasTreeSkyboxEnabled[(int)b, (int)b2][i] = resourceSpawnpoint.isSkyboxEnabled;
						ResourceAsset asset2 = resourceSpawnpoint.asset;
						if (asset2 != null && asset2.holidayRestriction == ENPCHoliday.NONE)
						{
							resourceSpawnpoint.enable();
						}
						else
						{
							resourceSpawnpoint.disable();
						}
						resourceSpawnpoint.disableSkybox();
					}
				}
			}
			return preCaptureObjectState;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000974C8 File Offset: 0x000956C8
		private static void RestorePreCaptureState(Level.PreCaptureObjectState state)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					foreach (LevelObject levelObject in LevelObjects.objects[(int)b, (int)b2])
					{
						levelObject.UpdateActiveAndRenderersEnabled();
					}
					List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
					for (int i = 0; i < list.Count; i++)
					{
						ResourceSpawnpoint resourceSpawnpoint = list[i];
						if (state.wasTreeEnabled[(int)b, (int)b2][i])
						{
							resourceSpawnpoint.enable();
						}
						else
						{
							resourceSpawnpoint.disable();
						}
						if (state.wasTreeSkyboxEnabled[(int)b, (int)b2][i])
						{
							resourceSpawnpoint.enableSkybox();
						}
					}
				}
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000975B4 File Offset: 0x000957B4
		public static void CaptureSatelliteImage()
		{
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			int num;
			int num2;
			if (mainVolume != null)
			{
				Vector3 position;
				Quaternion rotation;
				mainVolume.GetSatelliteCaptureTransform(out position, out rotation);
				Level.satelliteCaptureTransform.SetPositionAndRotation(position, rotation);
				Vector3 size = mainVolume.CalculateLocalBounds().size;
				num = Mathf.CeilToInt(size.x);
				num2 = Mathf.CeilToInt(size.z);
				Level.satelliteCaptureCamera.aspect = size.x / size.z;
				Level.satelliteCaptureCamera.orthographicSize = size.z * 0.5f;
			}
			else
			{
				num = (int)Level.size;
				num2 = (int)Level.size;
				Level.satelliteCaptureTransform.position = new Vector3(0f, 1028f, 0f);
				Level.satelliteCaptureTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
				Level.satelliteCaptureCamera.orthographicSize = (float)(Level.size / 2 - Level.border);
				Level.satelliteCaptureCamera.aspect = 1f;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num * 2, num2 * 2, 32);
			temporary.name = "Satellite";
			temporary.filterMode = FilterMode.Bilinear;
			Level.satelliteCaptureCamera.targetTexture = temporary;
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			float lodBias = QualitySettings.lodBias;
			float seaFloat = LevelLighting.getSeaFloat("_Shininess");
			Color seaColor = LevelLighting.getSeaColor("_SpecularColor");
			ERenderMode renderMode = GraphicsSettings.renderMode;
			GraphicsSettings.renderMode = ERenderMode.FORWARD;
			GraphicsSettings.apply("capturing satellite");
			RenderSettings.fog = false;
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientSkyColor = Palette.AMBIENT;
			RenderSettings.ambientEquatorColor = Palette.AMBIENT;
			RenderSettings.ambientGroundColor = Palette.AMBIENT;
			LevelLighting.setSeaFloat("_Shininess", 500f);
			LevelLighting.setSeaColor("_SpecularColor", Color.black);
			QualitySettings.lodBias = float.MaxValue;
			Level.PreCaptureObjectState objectState = Level.GetObjectState();
			Level.SatelliteCaptureDelegate satelliteCaptureDelegate = Level.onSatellitePreCapture;
			if (satelliteCaptureDelegate != null)
			{
				satelliteCaptureDelegate();
			}
			Level.satelliteCaptureCamera.Render();
			Level.SatelliteCaptureDelegate satelliteCaptureDelegate2 = Level.onSatellitePostCapture;
			if (satelliteCaptureDelegate2 != null)
			{
				satelliteCaptureDelegate2();
			}
			Level.RestorePreCaptureState(objectState);
			GraphicsSettings.renderMode = renderMode;
			GraphicsSettings.apply("finished capturing satellite");
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			LevelLighting.setSeaFloat("_Shininess", seaFloat);
			LevelLighting.setSeaColor("_SpecularColor", seaColor);
			QualitySettings.lodBias = lodBias;
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2);
			Graphics.Blit(temporary, temporary2);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = temporary2;
			Texture2D texture2D = new Texture2D(num, num2);
			texture2D.name = "Satellite";
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0);
			RenderTexture.ReleaseTemporary(temporary2);
			for (int i = 0; i < texture2D.width; i++)
			{
				for (int j = 0; j < texture2D.height; j++)
				{
					Color pixel = texture2D.GetPixel(i, j);
					if (pixel.a < 1f)
					{
						pixel.a = 1f;
						texture2D.SetPixel(i, j, pixel);
					}
				}
			}
			texture2D.Apply();
			byte[] bytes = ImageConversion.EncodeToPNG(texture2D);
			ReadWrite.writeBytes(Level.info.path + "/Map.png", false, false, bytes);
			Object.DestroyImmediate(texture2D);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00097904 File Offset: 0x00095B04
		private static void FindChartHit(Vector3 pos, out EObjectChart chart, out RaycastHit hit)
		{
			Physics.Raycast(pos, Vector3.down, out hit, Level.HEIGHT, RayMasks.CHART);
			chart = EObjectChart.NONE;
			ObjectAsset asset = LevelObjects.getAsset(hit.transform);
			if (asset != null)
			{
				chart = asset.chart;
			}
			else
			{
				ResourceSpawnpoint resourceSpawnpoint = LevelGround.FindResourceSpawnpointByTransform(hit.transform);
				ResourceAsset resourceAsset = (resourceSpawnpoint != null) ? resourceSpawnpoint.asset : null;
				if (resourceAsset != null)
				{
					chart = resourceAsset.chart;
				}
			}
			if (chart == EObjectChart.IGNORE)
			{
				Level.FindChartHit(hit.point + new Vector3(0f, -0.01f, 0f), out chart, out hit);
				return;
			}
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x00097994 File Offset: 0x00095B94
		public static void CaptureChartImage()
		{
			Bundle bundle = Bundles.getBundle(Level.info.path + "/Charts.unity3d", false);
			if (bundle == null)
			{
				UnturnedLog.error("Unable to load chart colors");
				return;
			}
			Level.<>c__DisplayClass141_0 CS$<>8__locals1;
			CS$<>8__locals1.heightStrip = bundle.load<Texture2D>("Height_Strip");
			CS$<>8__locals1.layerStrip = bundle.load<Texture2D>("Layer_Strip");
			bundle.unload();
			if (CS$<>8__locals1.heightStrip == null || CS$<>8__locals1.layerStrip == null)
			{
				UnturnedLog.error("Unable to find height and layer strip textures");
				return;
			}
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			if (mainVolume != null)
			{
				Vector3 position;
				Quaternion rotation;
				mainVolume.GetSatelliteCaptureTransform(out position, out rotation);
				Level.satelliteCaptureTransform.SetPositionAndRotation(position, rotation);
				Bounds bounds = mainVolume.CalculateWorldBounds();
				CS$<>8__locals1.terrainMinHeight = bounds.min.y;
				CS$<>8__locals1.terrainMaxHeight = bounds.max.y;
				Vector3 size = mainVolume.CalculateLocalBounds().size;
				CS$<>8__locals1.imageWidth = Mathf.CeilToInt(size.x);
				CS$<>8__locals1.imageHeight = Mathf.CeilToInt(size.z);
				CS$<>8__locals1.captureWidth = size.x;
				CS$<>8__locals1.captureHeight = size.z;
			}
			else
			{
				CS$<>8__locals1.imageWidth = (int)Level.size;
				CS$<>8__locals1.imageHeight = (int)Level.size;
				CS$<>8__locals1.captureWidth = (float)Level.size - (float)Level.border * 2f;
				CS$<>8__locals1.captureHeight = (float)Level.size - (float)Level.border * 2f;
				Level.satelliteCaptureTransform.position = new Vector3(0f, 1028f, 0f);
				Level.satelliteCaptureTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
				CS$<>8__locals1.terrainMinHeight = WaterVolumeManager.worldSeaLevel;
				CS$<>8__locals1.terrainMaxHeight = Level.TERRAIN;
			}
			Texture2D texture2D = new Texture2D(CS$<>8__locals1.imageWidth, CS$<>8__locals1.imageHeight);
			texture2D.name = "Chart";
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			Level.PreCaptureObjectState objectState = Level.GetObjectState();
			CS$<>8__locals1.terrainGO = new GameObject();
			CS$<>8__locals1.terrainGO.layer = 20;
			for (int i = 0; i < CS$<>8__locals1.imageWidth; i++)
			{
				for (int j = 0; j < CS$<>8__locals1.imageHeight; j++)
				{
					Color color = Level.<CaptureChartImage>g__GetColor|141_0((float)i + 0.25f, (float)j + 0.25f, ref CS$<>8__locals1) * 0.25f + Level.<CaptureChartImage>g__GetColor|141_0((float)i + 0.25f, (float)j + 0.75f, ref CS$<>8__locals1) * 0.25f + Level.<CaptureChartImage>g__GetColor|141_0((float)i + 0.75f, (float)j + 0.25f, ref CS$<>8__locals1) * 0.25f + Level.<CaptureChartImage>g__GetColor|141_0((float)i + 0.75f, (float)j + 0.75f, ref CS$<>8__locals1) * 0.25f;
					color.a = 1f;
					texture2D.SetPixel(i, j, color);
				}
			}
			texture2D.Apply();
			Level.RestorePreCaptureState(objectState);
			byte[] bytes = ImageConversion.EncodeToPNG(texture2D);
			ReadWrite.writeBytes(Level.info.path + "/Chart.png", false, false, bytes);
			Object.DestroyImmediate(texture2D);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00097CD8 File Offset: 0x00095ED8
		private IEnumerator ReturnToMainMenu()
		{
			yield return null;
			UnturnedLog.info("Returning to main menu");
			SceneManager.LoadScene("Menu");
			if (Level.placeholderAudioListener != null)
			{
				Object.Destroy(Level.placeholderAudioListener);
				Level.placeholderAudioListener = null;
			}
			Provider.updateRichPresence();
			LevelBatching levelBatching = LevelBatching.Get();
			if (levelBatching != null)
			{
				levelBatching.Destroy();
			}
			DevkitTransactionManager.resetTransactions();
			Level.updateCachedHolidayRedirects();
			Level.UpdateShouldUseLevelBatching();
			Level.isExiting = false;
			yield break;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00097CE0 File Offset: 0x00095EE0
		public IEnumerator init(int id)
		{
			if (!Level.isVR)
			{
				LevelNavigation.load();
			}
			LoadingUI.NotifyLevelLoadingProgress(0.05263158f);
			yield return null;
			LevelObjects.load();
			LoadingUI.NotifyLevelLoadingProgress(0.10526316f);
			yield return null;
			LevelLighting.load(Level.size);
			LoadingUI.NotifyLevelLoadingProgress(0.15789473f);
			yield return null;
			LevelGround.load(Level.size);
			LoadingUI.NotifyLevelLoadingProgress(0.21052632f);
			yield return null;
			LevelRoads.load();
			LoadingUI.NotifyLevelLoadingProgress(0.2631579f);
			yield return null;
			if (!Level.isVR)
			{
				LevelNodes.load();
				LoadingUI.NotifyLevelLoadingProgress(0.31578946f);
				yield return null;
				LevelItems.load();
				LoadingUI.NotifyLevelLoadingProgress(0.36842105f);
				yield return null;
			}
			LevelPlayers.load();
			LoadingUI.NotifyLevelLoadingProgress(0.42105263f);
			yield return null;
			if (!Level.isVR)
			{
				LevelZombies.load();
				LoadingUI.NotifyLevelLoadingProgress(0.47368422f);
				yield return null;
				LevelVehicles.load();
				LoadingUI.NotifyLevelLoadingProgress(0.5263158f);
				yield return null;
				LevelAnimals.load();
				LoadingUI.NotifyLevelLoadingProgress(0.57894737f);
				yield return null;
			}
			LevelVisibility.load();
			LoadingUI.NotifyLevelLoadingProgress(0.6315789f);
			yield return null;
			Level.pendingHashes = new List<byte[]>();
			LevelLoadingStepHandler levelLoadingStepHandler = Level.loadingSteps;
			if (levelLoadingStepHandler != null)
			{
				levelLoadingStepHandler();
			}
			LoadingUI.NotifyLevelLoadingProgress(0.68421054f);
			yield return null;
			if (LevelGround.hasLegacyDataForConversion)
			{
				if (Landscape.instance == null)
				{
					LevelHierarchy.AssignInstanceIdAndMarkDirty(new GameObject().AddComponent<Landscape>());
				}
				yield return Landscape.instance.AutoConvertLegacyTerrain();
			}
			VolumeManager<LandscapeHoleVolume, LandscapeHoleVolumeManager>.Get().ApplyToTerrain();
			if (LevelNodes.hasLegacyVolumesForConversion)
			{
				LevelNodes.AutoConvertLegacyVolumes();
			}
			if (LevelNodes.hasLegacyNodesForConversion)
			{
				LevelNodes.AutoConvertLegacyNodes();
			}
			LoadingUI.NotifyLevelLoadingProgress(0.7368421f);
			yield return null;
			IL_368:
			Level.includeHash("Level.dat", Level.info.hash);
			if (Level.info.configData.Hash != null)
			{
				Level.includeHash("Config.json", Level.info.configData.Hash);
			}
			Level.includeHash("Lighting.dat", LevelLighting.hash);
			Level.includeHash("Nodes.dat", LevelNodes.hash);
			Level.includeHash("Objects.dat", LevelObjects.hash);
			Level.includeHash("Resources.dat", LevelGround.treesHash);
			Level.combineHashes();
			Physics.gravity = new Vector3(0f, Level.info.configData.Gravity, 0f);
			LoadingUI.NotifyLevelLoadingProgress(0.8947368f);
			yield return null;
			Resources.UnloadUnusedAssets();
			GC.Collect();
			LoadingUI.NotifyLevelLoadingProgress(0.94736844f);
			yield return null;
			Level._editing = new GameObject().transform;
			Level.editing.name = "Editing";
			Level.editing.parent = Level.level;
			if (Level.isEditor)
			{
				Object.Destroy(Level.placeholderAudioListener);
				Level.placeholderAudioListener = null;
				Level.satelliteCaptureGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Edit/Mapper"));
				Level.satelliteCaptureGameObject.name = "Mapper";
				Level.satelliteCaptureTransform = Level.satelliteCaptureGameObject.transform;
				Level.satelliteCaptureTransform.parent = Level.editing;
				Level.satelliteCaptureCamera = Level.satelliteCaptureGameObject.GetComponent<Camera>();
				Transform transform = ((GameObject)Object.Instantiate(Resources.Load(Level.isVR ? "Edit/VR" : "Edit/Editor"))).transform;
				transform.name = "Editor";
				transform.parent = Level.editing;
				transform.tag = "Logic";
				transform.gameObject.layer = 8;
			}
			yield return null;
			PrePreLevelLoaded prePreLevelLoaded = Level.onPrePreLevelLoaded;
			if (prePreLevelLoaded != null)
			{
				prePreLevelLoaded(id);
			}
			yield return null;
			PreLevelLoaded preLevelLoaded = Level.onPreLevelLoaded;
			if (preLevelLoaded != null)
			{
				preLevelLoaded(id);
			}
			yield return null;
			LevelLoaded levelLoaded = Level.onLevelLoaded;
			if (levelLoaded != null)
			{
				levelLoaded(id);
			}
			yield return null;
			PostLevelLoaded postLevelLoaded = Level.onPostLevelLoaded;
			if (postLevelLoaded != null)
			{
				postLevelLoaded(id);
			}
			yield return null;
			if (!Level.isEditor && Level.info != null)
			{
				string text = null;
				if (string.Equals(Level.info.name, "germany", 3))
				{
					text = "Level/Triggers_Germany";
				}
				else if (string.Equals(Level.info.name, "pei", 3))
				{
					text = "Level/Triggers_PEI";
				}
				else if (string.Equals(Level.info.name, "russia", 3))
				{
					text = "Level/Triggers_Russia";
				}
				else if (string.Equals(Level.info.name, "tutorial", 3))
				{
					text = "Level/Triggers_Tutorial";
				}
				if (string.IsNullOrEmpty(text))
				{
					UnturnedLog.info("Level \"" + Level.info.name + "\" not using hardcoded special events");
				}
				else
				{
					UnturnedLog.info(string.Concat(new string[]
					{
						"Loading hardcoded special events \"",
						text,
						"\" for level \"",
						Level.info.name,
						"\""
					}));
					Transform transform2 = Object.Instantiate<GameObject>(Resources.Load<GameObject>(text)).transform;
					transform2.position = Vector3.zero;
					transform2.rotation = Quaternion.identity;
					transform2.name = "Triggers";
					transform2.parent = Level.clips;
				}
			}
			LoadingUI.NotifyLevelLoadingProgress(1f);
			yield return null;
			if (Level.shouldLogSpawnTablesAfterLoadingLevel)
			{
				SpawnTableTool.LogAllSpawnTables();
			}
			Level._isLoaded = true;
			Level.isLoadingContent = false;
			yield break;
			LevelBatching levelBatching = LevelBatching.Get();
			if (levelBatching != null)
			{
				goto IL_341;
			}
			goto IL_346;
			IL_341:
			levelBatching.ApplyStaticBatching();
			IL_346:
			LoadingUI.NotifyLevelLoadingProgress(0.84210527f);
			yield return null;
			goto IL_368;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x00097CF0 File Offset: 0x00095EF0
		private void Awake()
		{
			if (Level.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Level._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			Level.instance = this;
			Level.foliageVolumeManager = new FoliageVolumeManager();
			Level.undergroundWhitelistVolumeManager = new UndergroundWhitelistVolumeManager();
			Level.playerClipVolumeManager = new PlayerClipVolumeManager();
			Level.navClipVolumeManager = new NavClipVolumeManager();
			Level.waterVolumeManager = new WaterVolumeManager();
			Level.landscapeHoleVolumeManager = new LandscapeHoleVolumeManager();
			Level.deadzoneVolumeManager = new DeadzoneVolumeManager();
			Level.killVolumeManager = new KillVolumeManager();
			Level.effectVolumeManager = new EffectVolumeManager();
			Level.ambianceVolumeManager = new AmbianceVolumeManager();
			Level.entranceVolumeManager = new TeleporterEntranceVolumeManager();
			Level.exitVolumeManager = new TeleporterExitVolumeManager();
			Level.safezoneVolumeManager = new SafezoneVolumeManager();
			Level.arenaVolumeManager = new ArenaCompactorVolumeManager();
			Level.hordePurchaseVolumeManager = new HordePurchaseVolumeManager();
			Level.cartographyVolumeManager = new CartographyVolumeManager();
			Level.oxygenVolumeManager = new OxygenVolumeManager();
			Level.cullingVolumeManager = new CullingVolumeManager();
			Level.rewardVolumeManager = new NPCRewardVolumeManager();
			Level.airdropNodeSystem = new AirdropDevkitNodeSystem();
			Level.locationNodeSystem = new LocationDevkitNodeSystem();
			Level.spawnpointSystem = new SpawnpointSystemV2();
			SceneManager.sceneLoaded += this.onSceneLoaded;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x00097E14 File Offset: 0x00096014
		private void onSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == Level.BUILD_INDEX_GAME || scene.buildIndex == Level.BUILD_INDEX_LOADING)
			{
				if (Level.placeholderAudioListener == null)
				{
					Level.placeholderAudioListener = Level.instance.gameObject.AddComponent<AudioListener>();
				}
			}
			else if (scene.buildIndex == Level.BUILD_INDEX_MENU && Level.placeholderAudioListener != null)
			{
				Object.Destroy(Level.placeholderAudioListener);
				Level.placeholderAudioListener = null;
			}
			if (scene.buildIndex == Level.BUILD_INDEX_LOADING)
			{
				return;
			}
			if (scene.buildIndex > Level.BUILD_INDEX_SETUP && Level.info != null)
			{
				Level._level = new GameObject().transform;
				Level.level.name = Level.info.name;
				Level.level.tag = "Logic";
				Level.level.gameObject.layer = 8;
				Level._roots = new GameObject().transform;
				Level.roots.name = "Roots";
				Level.roots.parent = Level.level;
				Level._clips = new GameObject().transform;
				Level.clips.name = "Clips";
				Level.clips.parent = Level.level;
				Level.clips.tag = "Clip";
				Level.clips.gameObject.layer = 21;
				if (Level.info.configData.Use_Legacy_Clip_Borders)
				{
					Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3(0f, -4f, 0f);
					transform.localScale = new Vector3((float)(Level.size - Level.border * 2 + Level.CLIP * 2), (float)(Level.size - Level.border * 2 + Level.CLIP * 2), 1f);
					transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					Transform transform2 = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform2.position = new Vector3(0f, Level.HEIGHT + 4f, 0f);
					transform2.localScale = new Vector3((float)(Level.size - Level.border * 2 + Level.CLIP * 2), (float)(Level.size - Level.border * 2 + Level.CLIP * 2), 1f);
					transform2.rotation = Quaternion.Euler(90f, 0f, 0f);
					transform2.name = "Cap";
					transform2.parent = Level.clips;
					Transform transform3 = ((GameObject)Object.Instantiate(Resources.Load(Level.isEditor ? "Level/Wall" : "Level/Clip"))).transform;
					transform3.position = new Vector3((float)(Level.size / 2 - Level.border), Level.HEIGHT / 8f, 0f);
					transform3.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform3.rotation = Quaternion.Euler(0f, -90f, 0f);
					transform3.name = "Clip";
					transform3.parent = Level.clips;
					if (Level.isEditor)
					{
						transform3.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform3 = ((GameObject)Object.Instantiate(Resources.Load(Level.isEditor ? "Level/Wall" : "Level/Clip"))).transform;
					transform3.position = new Vector3((float)(-Level.size / 2 + Level.border), Level.HEIGHT / 8f, 0f);
					transform3.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform3.rotation = Quaternion.Euler(0f, 90f, 0f);
					transform3.name = "Clip";
					transform3.parent = Level.clips;
					if (Level.isEditor)
					{
						transform3.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform3 = ((GameObject)Object.Instantiate(Resources.Load(Level.isEditor ? "Level/Wall" : "Level/Clip"))).transform;
					transform3.position = new Vector3(0f, Level.HEIGHT / 8f, (float)(Level.size / 2 - Level.border));
					transform3.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform3.rotation = Quaternion.Euler(0f, 180f, 0f);
					transform3.name = "Clip";
					transform3.parent = Level.clips;
					if (Level.isEditor)
					{
						transform3.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform3 = ((GameObject)Object.Instantiate(Resources.Load(Level.isEditor ? "Level/Wall" : "Level/Clip"))).transform;
					transform3.position = new Vector3(0f, Level.HEIGHT / 8f, (float)(-Level.size / 2 + Level.border));
					transform3.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform3.rotation = Quaternion.identity;
					transform3.name = "Clip";
					transform3.parent = Level.clips;
					if (Level.isEditor)
					{
						transform3.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
				}
				base.StartCoroutine(Level.instance.init(scene.buildIndex));
			}
			else
			{
				Level.isLoadingLighting = true;
				Level.isLoadingVehicles = true;
				Level.isLoadingBarricades = true;
				Level.isLoadingStructures = true;
				Level.isLoadingContent = true;
				Level.isLoadingArea = true;
				Level._isLoaded = false;
				LevelLoaded levelLoaded = Level.onLevelLoaded;
				if (levelLoaded != null)
				{
					levelLoaded(scene.buildIndex);
				}
				LevelLighting.resetForMainMenu();
			}
			int buildIndex = scene.buildIndex;
			int build_INDEX_MENU = Level.BUILD_INDEX_MENU;
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000984B0 File Offset: 0x000966B0
		[Obsolete("Replaced by LevelInfo.hash")]
		public static byte[] getLevelHash(string path)
		{
			LevelInfo levelInfo = Level.ReadLevelInfo(path, 0UL);
			if (levelInfo != null)
			{
				return levelInfo.hash;
			}
			return new byte[20];
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000984D7 File Offset: 0x000966D7
		[Obsolete("Was unused in vanilla, so simplified to just use the find level by name method.")]
		public static bool exists(string name)
		{
			return Level.getLevel(name) != null;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000985E8 File Offset: 0x000967E8
		[CompilerGenerated]
		internal static Color <CaptureChartImage>g__GetColor|141_0(float x, float y, ref Level.<>c__DisplayClass141_0 A_2)
		{
			float num = x / (float)A_2.imageWidth;
			float num2 = y / (float)A_2.imageHeight;
			Vector3 position = new Vector3((num - 0.5f) * A_2.captureWidth, (num2 - 0.5f) * A_2.captureHeight, 0f);
			Vector3 vector = Level.satelliteCaptureTransform.TransformPoint(position);
			EObjectChart eobjectChart;
			RaycastHit raycastHit;
			Level.FindChartHit(vector, out eobjectChart, out raycastHit);
			Transform transform = raycastHit.transform;
			Vector3 vector2 = raycastHit.point;
			if (transform == null)
			{
				transform = A_2.terrainGO.transform;
				vector2 = vector;
				vector2.y = LevelGround.getHeight(vector);
			}
			int num3 = transform.gameObject.layer;
			if (eobjectChart == EObjectChart.GROUND)
			{
				num3 = 20;
			}
			else if (eobjectChart == EObjectChart.HIGHWAY)
			{
				num3 = 0;
			}
			else if (eobjectChart == EObjectChart.ROAD)
			{
				num3 = 1;
			}
			else if (eobjectChart == EObjectChart.STREET)
			{
				num3 = 2;
			}
			else if (eobjectChart == EObjectChart.PATH)
			{
				num3 = 3;
			}
			else if (eobjectChart == EObjectChart.LARGE)
			{
				num3 = 15;
			}
			else if (eobjectChart == EObjectChart.MEDIUM)
			{
				num3 = 16;
			}
			else if (eobjectChart == EObjectChart.CLIFF)
			{
				num3 = 4;
			}
			if (num3 == 19)
			{
				RoadMaterial roadMaterial = LevelRoads.getRoadMaterial(transform);
				if (roadMaterial != null)
				{
					if (!roadMaterial.isConcrete)
					{
						num3 = 3;
					}
					else if (roadMaterial.width > 8f)
					{
						num3 = 0;
					}
					else
					{
						num3 = 1;
					}
				}
			}
			Color pixel;
			if (eobjectChart == EObjectChart.WATER)
			{
				pixel = A_2.heightStrip.GetPixel(0, 0);
			}
			else if (num3 == 20)
			{
				if (WaterUtility.isPointUnderwater(vector2))
				{
					pixel = A_2.heightStrip.GetPixel(0, 0);
				}
				else
				{
					float num4 = Mathf.InverseLerp(A_2.terrainMinHeight, A_2.terrainMaxHeight, vector2.y);
					pixel = A_2.heightStrip.GetPixel((int)(num4 * (float)(A_2.heightStrip.width - 1)) + 1, 0);
				}
			}
			else
			{
				pixel = A_2.layerStrip.GetPixel(num3, 0);
			}
			return pixel;
		}

		// Token: 0x04001356 RID: 4950
		private const float STEPS = 19f;

		// Token: 0x04001357 RID: 4951
		public static readonly int BUILD_INDEX_SETUP = 0;

		// Token: 0x04001358 RID: 4952
		public static readonly int BUILD_INDEX_MENU = 1;

		// Token: 0x04001359 RID: 4953
		public static readonly int BUILD_INDEX_GAME = 2;

		// Token: 0x0400135A RID: 4954
		public static readonly int BUILD_INDEX_LOADING = 3;

		// Token: 0x0400135B RID: 4955
		public static readonly float HEIGHT = 1024f;

		// Token: 0x0400135C RID: 4956
		public static readonly float TERRAIN = 256f;

		// Token: 0x0400135D RID: 4957
		public static readonly ushort CLIP = 8;

		// Token: 0x0400135E RID: 4958
		public static readonly ushort TINY_BORDER = 16;

		// Token: 0x0400135F RID: 4959
		public static readonly ushort SMALL_BORDER = 64;

		// Token: 0x04001360 RID: 4960
		public static readonly ushort MEDIUM_BORDER = 64;

		// Token: 0x04001361 RID: 4961
		public static readonly ushort LARGE_BORDER = 64;

		// Token: 0x04001362 RID: 4962
		public static readonly ushort INSANE_BORDER = 128;

		// Token: 0x04001363 RID: 4963
		public static readonly ushort TINY_SIZE = 512;

		// Token: 0x04001364 RID: 4964
		public static readonly ushort SMALL_SIZE = 1024;

		// Token: 0x04001365 RID: 4965
		public static readonly ushort MEDIUM_SIZE = 2048;

		// Token: 0x04001366 RID: 4966
		public static readonly ushort LARGE_SIZE = 4096;

		// Token: 0x04001367 RID: 4967
		public static readonly ushort INSANE_SIZE = 8192;

		// Token: 0x04001368 RID: 4968
		public static readonly byte SAVEDATA_VERSION = 2;

		// Token: 0x04001369 RID: 4969
		public static PrePreLevelLoaded onPrePreLevelLoaded;

		// Token: 0x0400136A RID: 4970
		public static PreLevelLoaded onPreLevelLoaded;

		// Token: 0x0400136B RID: 4971
		public static LevelLoaded onLevelLoaded;

		// Token: 0x0400136C RID: 4972
		public static PostLevelLoaded onPostLevelLoaded;

		// Token: 0x0400136D RID: 4973
		public static LevelsRefreshed onLevelsRefreshed;

		// Token: 0x0400136F RID: 4975
		public static LevelExited onLevelExited;

		// Token: 0x04001370 RID: 4976
		private static LevelInfo _info;

		// Token: 0x04001371 RID: 4977
		private static LevelAsset cachedLevelAsset;

		// Token: 0x04001372 RID: 4978
		private static bool didResolveLevelAsset;

		// Token: 0x04001374 RID: 4980
		public const bool shouldUseLevelBatching = false;

		// Token: 0x04001375 RID: 4981
		private static GameObject satelliteCaptureGameObject;

		// Token: 0x04001376 RID: 4982
		private static Transform satelliteCaptureTransform;

		// Token: 0x04001377 RID: 4983
		private static Camera satelliteCaptureCamera;

		// Token: 0x04001378 RID: 4984
		private static Transform _level;

		// Token: 0x04001379 RID: 4985
		private static Transform _roots;

		// Token: 0x0400137A RID: 4986
		private static Transform _clips;

		// Token: 0x0400137B RID: 4987
		private static Transform _effects;

		// Token: 0x0400137C RID: 4988
		private static Transform _spawns;

		// Token: 0x0400137D RID: 4989
		private static Transform _editing;

		// Token: 0x0400137E RID: 4990
		private static Level instance;

		/// <summary>
		/// Placeholder created between unloading the main menu and loading into game or editor.
		/// </summary>
		// Token: 0x0400137F RID: 4991
		internal static AudioListener placeholderAudioListener;

		// Token: 0x04001380 RID: 4992
		private static bool _isInitialized;

		// Token: 0x04001381 RID: 4993
		private static bool _isEditor;

		// Token: 0x04001383 RID: 4995
		public static bool isLoadingContent = true;

		// Token: 0x04001384 RID: 4996
		public static bool isLoadingLighting = true;

		// Token: 0x04001385 RID: 4997
		public static bool isLoadingVehicles = true;

		// Token: 0x04001386 RID: 4998
		public static bool isLoadingBarricades = true;

		// Token: 0x04001387 RID: 4999
		public static bool isLoadingStructures = true;

		// Token: 0x04001388 RID: 5000
		public static bool isLoadingArea = true;

		// Token: 0x04001389 RID: 5001
		private static bool _isLoaded;

		// Token: 0x0400138B RID: 5003
		private static List<byte[]> pendingHashes;

		/// <summary>
		/// Useful to narrow down why a player is getting kicked for modified level files when joining a server.
		/// </summary>
		// Token: 0x0400138C RID: 5004
		private static CommandLineFlag shouldLogLevelHash = new CommandLineFlag(false, "-LogLevelHash");

		// Token: 0x0400138D RID: 5005
		private static List<LevelInfo> knownLevels = new List<LevelInfo>();

		// Token: 0x04001390 RID: 5008
		private static FoliageVolumeManager foliageVolumeManager;

		// Token: 0x04001391 RID: 5009
		private static UndergroundWhitelistVolumeManager undergroundWhitelistVolumeManager;

		// Token: 0x04001392 RID: 5010
		private static PlayerClipVolumeManager playerClipVolumeManager;

		// Token: 0x04001393 RID: 5011
		private static NavClipVolumeManager navClipVolumeManager;

		// Token: 0x04001394 RID: 5012
		private static WaterVolumeManager waterVolumeManager;

		// Token: 0x04001395 RID: 5013
		private static LandscapeHoleVolumeManager landscapeHoleVolumeManager;

		// Token: 0x04001396 RID: 5014
		private static DeadzoneVolumeManager deadzoneVolumeManager;

		// Token: 0x04001397 RID: 5015
		private static KillVolumeManager killVolumeManager;

		// Token: 0x04001398 RID: 5016
		private static EffectVolumeManager effectVolumeManager;

		// Token: 0x04001399 RID: 5017
		private static AmbianceVolumeManager ambianceVolumeManager;

		// Token: 0x0400139A RID: 5018
		private static TeleporterEntranceVolumeManager entranceVolumeManager;

		// Token: 0x0400139B RID: 5019
		private static TeleporterExitVolumeManager exitVolumeManager;

		// Token: 0x0400139C RID: 5020
		private static SafezoneVolumeManager safezoneVolumeManager;

		// Token: 0x0400139D RID: 5021
		private static ArenaCompactorVolumeManager arenaVolumeManager;

		// Token: 0x0400139E RID: 5022
		private static HordePurchaseVolumeManager hordePurchaseVolumeManager;

		// Token: 0x0400139F RID: 5023
		private static CartographyVolumeManager cartographyVolumeManager;

		// Token: 0x040013A0 RID: 5024
		private static OxygenVolumeManager oxygenVolumeManager;

		// Token: 0x040013A1 RID: 5025
		private static CullingVolumeManager cullingVolumeManager;

		// Token: 0x040013A2 RID: 5026
		private static NPCRewardVolumeManager rewardVolumeManager;

		// Token: 0x040013A3 RID: 5027
		private static AirdropDevkitNodeSystem airdropNodeSystem;

		// Token: 0x040013A4 RID: 5028
		private static LocationDevkitNodeSystem locationNodeSystem;

		// Token: 0x040013A5 RID: 5029
		private static SpawnpointSystemV2 spawnpointSystem;

		// Token: 0x040013A6 RID: 5030
		private static CommandLineBool clUseLevelBatching = new CommandLineBool("-UseLevelBatching");

		// Token: 0x040013A7 RID: 5031
		private static CommandLineFlag shouldLogSpawnTablesAfterLoadingLevel = new CommandLineFlag(false, "-LogSpawnTablesAfterLoadingLevel");

		// Token: 0x0200094E RID: 2382
		// (Invoke) Token: 0x06004AF0 RID: 19184
		public delegate void SatelliteCaptureDelegate();

		// Token: 0x0200094F RID: 2383
		private class PreCaptureObjectState
		{
			// Token: 0x04003310 RID: 13072
			public bool[,][] wasTreeEnabled = new bool[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE][];

			// Token: 0x04003311 RID: 13073
			public bool[,][] wasTreeSkyboxEnabled = new bool[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE][];
		}
	}
}
