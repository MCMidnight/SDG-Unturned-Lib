using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200052C RID: 1324
	public class LoadingUI : MonoBehaviour
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002958 RID: 10584 RVA: 0x000B014C File Offset: 0x000AE34C
		public static bool isInitialized
		{
			get
			{
				return LoadingUI._isInitialized;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002959 RID: 10585 RVA: 0x000B0153 File Offset: 0x000AE353
		// (set) Token: 0x0600295A RID: 10586 RVA: 0x000B015A File Offset: 0x000AE35A
		public static GameObject loader { get; private set; }

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x0600295B RID: 10587 RVA: 0x000B0162 File Offset: 0x000AE362
		public static bool isBlocked
		{
			get
			{
				return Time.frameCount <= LoadingUI.lastLoading;
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000B0173 File Offset: 0x000AE373
		public static void SetLoadingText(string key)
		{
			CommandWindow.Log(LoadingUI.localization.format(key));
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000B0185 File Offset: 0x000AE385
		public static void NotifyLevelLoadingProgress(float progress)
		{
			CommandWindow.Log(LoadingUI.localization.format("Level_Load", (int)(progress * 100f)));
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000B01A8 File Offset: 0x000AE3A8
		private static void UpdateAssetBundleProgress(AssetLoadingStats loadingStats)
		{
			bool flag = loadingStats.isLoadingAssetBundles || LoadingUI.wasLoadingAssetBundles;
			if (loadingStats.isLoadingAssetBundles != LoadingUI.wasLoadingAssetBundles)
			{
				if (!LoadingUI.wasLoadingAssetBundles)
				{
					LoadingUI.previousAssetBundlesLoaded = -1;
					LoadingUI.previousAssetBundlesFound = -1;
				}
				LoadingUI.wasLoadingAssetBundles = loadingStats.isLoadingAssetBundles;
			}
			if (flag)
			{
				int assetBundlesLoaded = loadingStats.AssetBundlesLoaded;
				int assetBundlesFound = loadingStats.AssetBundlesFound;
				if (assetBundlesLoaded != LoadingUI.previousAssetBundlesLoaded || assetBundlesFound != LoadingUI.previousAssetBundlesFound)
				{
					LoadingUI.previousAssetBundlesLoaded = assetBundlesLoaded;
					LoadingUI.previousAssetBundlesFound = assetBundlesFound;
					string text = LoadingUI.localization.format("Loading_Asset_Bundles", Assets.loadingStats.AssetBundlesLoaded, Assets.loadingStats.AssetBundlesFound);
					CommandWindow.Log(text);
				}
			}
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000B0250 File Offset: 0x000AE450
		private static void UpdateSearchProgress(AssetLoadingStats loadingStats)
		{
			bool flag = loadingStats.SearchLocationsFinishedSearching < loadingStats.RegisteredSearchLocations;
			bool flag2 = flag || LoadingUI.wasSearching;
			if (flag != LoadingUI.wasSearching)
			{
				if (!LoadingUI.wasSearching)
				{
					LoadingUI.previousFilesFound = -1;
				}
				LoadingUI.wasSearching = flag;
			}
			if (flag2)
			{
				int filesFound = loadingStats.FilesFound;
				if (filesFound != LoadingUI.previousFilesFound)
				{
					LoadingUI.previousFilesFound = filesFound;
					string text = LoadingUI.localization.format("Loading_Search", Assets.loadingStats.SearchLocationsFinishedSearching, Assets.loadingStats.RegisteredSearchLocations, Assets.loadingStats.FilesFound);
					CommandWindow.Log(text);
				}
			}
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000B02F0 File Offset: 0x000AE4F0
		private static void UpdateReadProgress(AssetLoadingStats loadingStats)
		{
			bool flag = loadingStats.FilesRead < loadingStats.FilesFound;
			bool flag2 = flag || LoadingUI.wasReading;
			if (flag != LoadingUI.wasReading)
			{
				if (!LoadingUI.wasReading)
				{
					LoadingUI.previousReadFilesRead = -1;
					LoadingUI.previousReadFilesFound = -1;
				}
				LoadingUI.wasReading = flag;
			}
			if (flag2)
			{
				int filesRead = loadingStats.FilesRead;
				int filesFound = loadingStats.FilesFound;
				if (filesRead != LoadingUI.previousReadFilesRead || filesFound != LoadingUI.previousReadFilesFound)
				{
					LoadingUI.previousReadFilesRead = filesRead;
					LoadingUI.previousReadFilesFound = filesFound;
					string text = LoadingUI.localization.format("Loading_Read", filesRead, filesFound);
					CommandWindow.Log(text);
				}
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000B038C File Offset: 0x000AE58C
		private static void UpdateAssetLoadingProgress(AssetLoadingStats loadingStats)
		{
			int filesLoaded = loadingStats.FilesLoaded;
			int filesFound = loadingStats.FilesFound;
			if (filesLoaded != LoadingUI.previousAssetLoadingFilesLoaded || filesFound != LoadingUI.previousAssetLoadingFilesFound)
			{
				LoadingUI.previousAssetLoadingFilesLoaded = filesLoaded;
				LoadingUI.previousAssetLoadingFilesFound = filesFound;
				string text = LoadingUI.localization.format("Loading_Asset_Definitions", filesLoaded, filesFound);
				CommandWindow.Log(text);
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000B03E5 File Offset: 0x000AE5E5
		private static void HideAllLoadingBars()
		{
			if (LoadingUI.assetBundleProgressBar != null)
			{
				LoadingUI.assetBundleProgressBar.IsVisible = false;
				LoadingUI.searchProgressBar.IsVisible = false;
				LoadingUI.readProgressBar.IsVisible = false;
				LoadingUI.downloadProgressBar.IsVisible = false;
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000B041A File Offset: 0x000AE61A
		internal static void NotifyAssetDefinitionLoadingProgress()
		{
			AssetLoadingStats loadingStats = Assets.loadingStats;
			LoadingUI.UpdateAssetBundleProgress(loadingStats);
			LoadingUI.UpdateSearchProgress(loadingStats);
			LoadingUI.UpdateReadProgress(loadingStats);
			LoadingUI.UpdateAssetLoadingProgress(loadingStats);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000B0438 File Offset: 0x000AE638
		public static void SetIsDownloading(bool isDownloading)
		{
			if (LoadingUI.downloadProgressBar != null)
			{
				LoadingUI.downloadProgressBar.IsVisible = isDownloading;
				LoadingUI.UpdateLoadingBarPositions();
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000B0451 File Offset: 0x000AE651
		public static void SetDownloadFileName(string name)
		{
			if (LoadingUI.downloadProgressBar != null)
			{
				LoadingUI.downloadProgressBar.DescriptionText = LoadingUI.localization.format("Download_Progress", name);
			}
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000B0474 File Offset: 0x000AE674
		public static void NotifyDownloadProgress(float progress)
		{
			if (LoadingUI.downloadProgressBar != null)
			{
				LoadingUI.downloadProgressBar.ProgressPercentage = progress;
			}
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000B0488 File Offset: 0x000AE688
		private static bool loadBackgroundImage(string path)
		{
			if (LoadingUI.backgroundImage.Texture != null && LoadingUI.backgroundImage.ShouldDestroyTexture)
			{
				Object.Destroy(LoadingUI.backgroundImage.Texture);
				LoadingUI.backgroundImage.Texture = null;
			}
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (!File.Exists(path))
			{
				return false;
			}
			LoadingUI.backgroundImage.Texture = ReadWrite.readTextureFromFile(path, EReadTextureFromFileMode.UI);
			LoadingUI.backgroundImage.ShouldDestroyTexture = true;
			return true;
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000B0500 File Offset: 0x000AE700
		internal static string GetRandomImagePathInDirectory(string path, bool onlyWithoutHud)
		{
			try
			{
				List<string> list = new List<string>();
				foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles())
				{
					if (fileInfo.Length <= 10000000L && (!onlyWithoutHud || fileInfo.Name.Contains("NoUI")))
					{
						string extension = fileInfo.Extension;
						if (string.Equals(extension, ".png", 3) || string.Equals(extension, ".jpg", 3))
						{
							list.Add(fileInfo.FullName);
						}
					}
				}
				return list.RandomOrDefault<string>();
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception loading background image:");
			}
			return null;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000B05CC File Offset: 0x000AE7CC
		private static bool pickBackgroundImage(string path, bool onlyWithoutHud)
		{
			if (!Directory.Exists(path))
			{
				LoadingUI.loadBackgroundImage(null);
				return false;
			}
			string randomImagePathInDirectory = LoadingUI.GetRandomImagePathInDirectory(path, onlyWithoutHud);
			if (!string.IsNullOrEmpty(randomImagePathInDirectory))
			{
				LoadingUI.loadBackgroundImage(randomImagePathInDirectory);
				return true;
			}
			LoadingUI.loadBackgroundImage(null);
			return false;
		}

		/// <summary>
		/// Select a loading image while on the startup screen or a level without any images.
		/// </summary>
		// Token: 0x0600296A RID: 10602 RVA: 0x000B060B File Offset: 0x000AE80B
		private static void PickNonLevelBackgroundImage()
		{
			if (OptionsSettings.enableScreenshotsOnLoadingScreen && LoadingUI.pickBackgroundImage(PathEx.Join(UnturnedPaths.RootDirectory, "Screenshots"), true))
			{
				return;
			}
			LoadingUI.pickBackgroundImage(PathEx.Join(UnturnedPaths.RootDirectory, "LoadingScreens"), false);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000B0644 File Offset: 0x000AE844
		public static void updateScene()
		{
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000B0654 File Offset: 0x000AE854
		private static void onQueuePositionUpdated()
		{
			LoadingUI.loadingProgressBar.DescriptionText = LoadingUI.localization.format("Queue_Position", (int)(Provider.queuePosition + 1));
			LoadingUI.loadingBarBox.SizeOffset_X = -130f;
			LoadingUI.cancelButton.IsVisible = true;
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000B06A0 File Offset: 0x000AE8A0
		private static void onClickedCancelButton(ISleekElement button)
		{
			Provider.RequestDisconnect("clicked queue cancel button");
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000B06AC File Offset: 0x000AE8AC
		private void Update()
		{
			bool isBlocked = LoadingUI.isBlocked;
			UnturnedMasterVolume.mutedByLoadingScreen = isBlocked;
			this.placeholderCamera.enabled = isBlocked;
			if (isBlocked)
			{
				Glazier.Get().Root = LoadingUI.window;
				return;
			}
			if (PlayerUI.instance != null)
			{
				PlayerUI.instance.Player_OnGUI();
				return;
			}
			if (MenuUI.instance != null)
			{
				MenuUI.instance.Menu_OnGUI();
				return;
			}
			if (EditorUI.instance != null)
			{
				EditorUI.instance.Editor_OnGUI();
			}
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000B072B File Offset: 0x000AE92B
		private void Awake()
		{
			if (LoadingUI.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			LoadingUI._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000B0751 File Offset: 0x000AE951
		private void Start()
		{
			LoadingUI.localization = Localization.read("/Menu/MenuLoading.dat");
			LoadingUI.loader = base.gameObject;
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000B0778 File Offset: 0x000AE978
		private void OnDestroy()
		{
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000B077C File Offset: 0x000AE97C
		private static void UpdateLoadingBarPositions()
		{
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000B078C File Offset: 0x000AE98C
		private static void UpdateBackgroundAnim(float progress)
		{
			progress = Mathf.Max(LoadingUI.animMaxProgress, progress);
			LoadingUI.animMaxProgress = progress;
			LoadingUI.backgroundImage.PositionScale_X = Mathf.Lerp(LoadingUI.animStart_X, LoadingUI.animEnd_X, progress);
			LoadingUI.backgroundImage.PositionScale_Y = Mathf.Lerp(LoadingUI.animStart_Y, LoadingUI.animEnd_Y, progress);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000B07E0 File Offset: 0x000AE9E0
		private static void DisableBackgroundAnim()
		{
			LoadingUI.animMaxProgress = 0f;
			LoadingUI.backgroundImage.PositionScale_X = 0f;
			LoadingUI.backgroundImage.PositionScale_Y = 0f;
			LoadingUI.backgroundImage.SizeScale_X = 1f;
			LoadingUI.backgroundImage.SizeScale_Y = 1f;
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000B0834 File Offset: 0x000AEA34
		private static void EnableBackgroundAnim()
		{
			LoadingUI.animMaxProgress = 0f;
			LoadingUI.backgroundImage.SizeScale_X = 1.01f;
			LoadingUI.backgroundImage.SizeScale_Y = 1.01f;
			if (Random.value < 0.5f)
			{
				LoadingUI.animStart_X = -0.01f;
				LoadingUI.animEnd_X = 0f;
			}
			else
			{
				LoadingUI.animStart_X = 0f;
				LoadingUI.animEnd_X = -0.01f;
			}
			float num = Random.Range(0f, 0.01f);
			float num2 = Random.Range(0f, 0.01f - num);
			if (Random.value < 0.5f)
			{
				LoadingUI.animStart_Y = -num2 - num;
				LoadingUI.animEnd_Y = -num2;
			}
			else
			{
				LoadingUI.animStart_Y = -num2;
				LoadingUI.animEnd_Y = -num2 - num;
			}
			LoadingUI.backgroundImage.PositionScale_X = LoadingUI.animStart_X;
			LoadingUI.backgroundImage.PositionScale_Y = LoadingUI.animStart_Y;
		}

		// Token: 0x0400163A RID: 5690
		private static readonly byte TIP_COUNT = 31;

		// Token: 0x0400163B RID: 5691
		private static bool _isInitialized;

		/// <summary>
		/// Camera used while transitioning between scenes to prevent the "no cameras rendering" warning.
		/// </summary>
		// Token: 0x0400163D RID: 5693
		public Camera placeholderCamera;

		// Token: 0x0400163E RID: 5694
		public static SleekWindow window;

		// Token: 0x0400163F RID: 5695
		private static Local localization;

		// Token: 0x04001640 RID: 5696
		private static ISleekImage backgroundImage;

		// Token: 0x04001641 RID: 5697
		private static ISleekLabel tipLabel;

		// Token: 0x04001642 RID: 5698
		private static ISleekBox loadingBarBox;

		// Token: 0x04001643 RID: 5699
		private static SleekLoadingScreenProgressBar loadingProgressBar;

		// Token: 0x04001644 RID: 5700
		private static SleekLoadingScreenProgressBar assetBundleProgressBar;

		// Token: 0x04001645 RID: 5701
		private static SleekLoadingScreenProgressBar downloadProgressBar;

		// Token: 0x04001646 RID: 5702
		private static SleekLoadingScreenProgressBar searchProgressBar;

		// Token: 0x04001647 RID: 5703
		private static SleekLoadingScreenProgressBar readProgressBar;

		// Token: 0x04001648 RID: 5704
		private static ISleekButton cancelButton;

		// Token: 0x04001649 RID: 5705
		private static ISleekLabel creditsLabel;

		/// <summary>
		/// Set to Time.frameCount + 1 while loading.
		/// In the past used realtime, but that was unreliable if an individual frame took too long.
		/// </summary>
		// Token: 0x0400164A RID: 5706
		private static int lastLoading;

		// Token: 0x0400164B RID: 5707
		private static ELoadingTip tip;

		// Token: 0x0400164C RID: 5708
		private static bool wasLoadingAssetBundles;

		// Token: 0x0400164D RID: 5709
		private static int previousAssetBundlesLoaded;

		// Token: 0x0400164E RID: 5710
		private static int previousAssetBundlesFound;

		// Token: 0x0400164F RID: 5711
		private static bool wasSearching;

		// Token: 0x04001650 RID: 5712
		private static int previousFilesFound;

		// Token: 0x04001651 RID: 5713
		private static bool wasReading;

		// Token: 0x04001652 RID: 5714
		private static int previousReadFilesRead;

		// Token: 0x04001653 RID: 5715
		private static int previousReadFilesFound;

		// Token: 0x04001654 RID: 5716
		private static int previousAssetLoadingFilesLoaded = -1;

		// Token: 0x04001655 RID: 5717
		private static int previousAssetLoadingFilesFound = -1;

		// Token: 0x04001656 RID: 5718
		private static float animMaxProgress;

		// Token: 0x04001657 RID: 5719
		private static float animStart_X;

		// Token: 0x04001658 RID: 5720
		private static float animStart_Y;

		// Token: 0x04001659 RID: 5721
		private static float animEnd_X;

		// Token: 0x0400165A RID: 5722
		private static float animEnd_Y;
	}
}
