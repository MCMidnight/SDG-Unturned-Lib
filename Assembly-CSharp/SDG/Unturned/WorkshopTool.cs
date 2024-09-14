using System;
using SDG.Provider;

namespace SDG.Unturned
{
	// Token: 0x0200076F RID: 1903
	public class WorkshopTool
	{
		// Token: 0x06003E2E RID: 15918 RVA: 0x0012F2C9 File Offset: 0x0012D4C9
		public static bool checkMapMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Map.meta", false, usePath);
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x0012F2E0 File Offset: 0x0012D4E0
		public static bool checkMapValid(string path, bool usePath)
		{
			string[] folders = ReadWrite.getFolders(path, usePath);
			return folders.Length == 1 && ReadWrite.fileExists(folders[0] + "/Level.dat", false, usePath);
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0012F314 File Offset: 0x0012D514
		private static bool findMapNestedPath(string basePath, string searchPath, out string path)
		{
			string[] folders = ReadWrite.getFolders(basePath, false);
			for (int i = 0; i < folders.Length; i++)
			{
				string text = folders[i] + searchPath;
				if (ReadWrite.folderExists(text, false))
				{
					path = text;
					return true;
				}
			}
			path = null;
			return false;
		}

		/// <summary>
		/// Given path to a workshop map, try to find its /Bundles folder.
		/// </summary>
		// Token: 0x06003E31 RID: 15921 RVA: 0x0012F353 File Offset: 0x0012D553
		public static bool findMapBundlesPath(string path, out string bundlesPath)
		{
			return WorkshopTool.findMapNestedPath(path, "/Bundles", out bundlesPath);
		}

		/// <summary>
		/// Given path to a workshop map, try to find its /Content folder.
		/// </summary>
		// Token: 0x06003E32 RID: 15922 RVA: 0x0012F361 File Offset: 0x0012D561
		public static bool findMapContentPath(string path, out string contentPath)
		{
			return WorkshopTool.findMapNestedPath(path, "/Content", out contentPath);
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0012F36F File Offset: 0x0012D56F
		[Obsolete]
		public static void loadMapBundlesAndContent(string workshopItemPath)
		{
			WorkshopTool.loadMapBundlesAndContent(workshopItemPath, 0UL);
		}

		/// <summary>
		/// Maps on the workshop are a root folder named after the published file id, containing
		/// the map folder itself with the level name. In order to load the map's bundles and content
		/// properly we need to find the nested Bundles and Content folders.
		/// </summary>
		// Token: 0x06003E34 RID: 15924 RVA: 0x0012F37C File Offset: 0x0012D57C
		public static void loadMapBundlesAndContent(string workshopItemPath, ulong workshopFileId)
		{
			string absoluteDirectoryPath;
			if (WorkshopTool.findMapBundlesPath(workshopItemPath, out absoluteDirectoryPath))
			{
				Assets.RequestAddSearchLocation(absoluteDirectoryPath, TempSteamworksWorkshop.FindOrAddOrigin(workshopFileId));
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0012F39F File Offset: 0x0012D59F
		public static bool checkLocalizationMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Localization.meta", false, usePath);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x0012F3B3 File Offset: 0x0012D5B3
		public static bool checkLocalizationValid(string path, bool usePath)
		{
			return ReadWrite.getFolders(path, usePath).Length != 0;
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x0012F3C0 File Offset: 0x0012D5C0
		public static bool checkObjectMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Object.meta", false, usePath);
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x0012F3D4 File Offset: 0x0012D5D4
		public static bool checkItemMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Item.meta", false, usePath);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x0012F3E8 File Offset: 0x0012D5E8
		public static bool checkVehicleMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Vehicle.meta", false, usePath);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x0012F3FC File Offset: 0x0012D5FC
		public static bool checkSkinMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Skin.meta", false, usePath);
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x0012F410 File Offset: 0x0012D610
		public static bool checkBundleValid(string path, bool usePath)
		{
			return ReadWrite.getFolders(path, usePath).Length != 0;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x0012F420 File Offset: 0x0012D620
		public static bool detectUGCMetaType(string path, bool usePath, out ESteamUGCType outType)
		{
			if (WorkshopTool.checkMapMeta(path, usePath))
			{
				outType = ESteamUGCType.MAP;
			}
			else if (WorkshopTool.checkLocalizationMeta(path, usePath))
			{
				outType = ESteamUGCType.LOCALIZATION;
			}
			else if (WorkshopTool.checkObjectMeta(path, usePath))
			{
				outType = ESteamUGCType.OBJECT;
			}
			else if (WorkshopTool.checkItemMeta(path, false))
			{
				outType = ESteamUGCType.ITEM;
			}
			else
			{
				if (!WorkshopTool.checkVehicleMeta(path, false))
				{
					outType = ESteamUGCType.ITEM;
					return false;
				}
				outType = ESteamUGCType.VEHICLE;
			}
			return true;
		}
	}
}
