using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Each level should have a 380x80 Icon.png file.
	/// This class caches them so that the server list can show them quickly.
	/// </summary>
	// Token: 0x02000790 RID: 1936
	public static class LevelIconCache
	{
		// Token: 0x0600404C RID: 16460 RVA: 0x0014A824 File Offset: 0x00148A24
		public static Texture2D GetOrLoadIcon(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			name = name.Trim();
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Texture2D result;
			if (LevelIconCache.icons.TryGetValue(name, ref result))
			{
				return result;
			}
			Texture2D texture2D = LevelIconCache.LoadIcon(name);
			LevelIconCache.icons.Add(name, texture2D);
			return texture2D;
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0014A874 File Offset: 0x00148A74
		public static Texture2D GetOrLoadIcon(LevelInfo levelInfo)
		{
			Texture2D result;
			if (LevelIconCache.icons.TryGetValue(levelInfo.name, ref result))
			{
				return result;
			}
			Texture2D texture2D = LevelIconCache.LoadIcon(levelInfo);
			LevelIconCache.icons.Add(levelInfo.name, texture2D);
			return texture2D;
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0014A8B0 File Offset: 0x00148AB0
		private static Texture2D LoadIcon(string name)
		{
			foreach (CuratedMapLink curatedMapLink in Provider.statusData.Maps.Curated_Map_Links)
			{
				if (string.Equals(curatedMapLink.Name, name, 5))
				{
					string text = PathEx.Join(UnturnedPaths.RootDirectory, "CuratedMapIcons", string.Format("{0}.png", curatedMapLink.Workshop_File_Id));
					if (ReadWrite.fileExists(text, false, false))
					{
						return ReadWrite.readTextureFromFile(text, EReadTextureFromFileMode.UI);
					}
				}
			}
			LevelInfo level = Level.getLevel(name);
			if (level != null)
			{
				return LevelIconCache.LoadIcon(level);
			}
			return null;
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0014A964 File Offset: 0x00148B64
		private static Texture2D LoadIcon(LevelInfo levelInfo)
		{
			string text = Path.Combine(levelInfo.path, "Icon.png");
			if (ReadWrite.fileExists(text, false, false))
			{
				return ReadWrite.readTextureFromFile(text, EReadTextureFromFileMode.UI);
			}
			return null;
		}

		// Token: 0x04002927 RID: 10535
		private static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
	}
}
