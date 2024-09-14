using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	// Token: 0x02000427 RID: 1063
	public class Localization
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x0007BE95 File Offset: 0x0007A095
		public static List<string> messages
		{
			get
			{
				return Localization._messages;
			}
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0007BE9C File Offset: 0x0007A09C
		[Obsolete]
		public static Local tryRead(string path)
		{
			return Localization.tryRead(path, true);
		}

		/// <summary>
		/// Load {Language}.dat and/or English.dat from folder path.
		/// </summary>
		// Token: 0x0600201E RID: 8222 RVA: 0x0007BEA8 File Offset: 0x0007A0A8
		public static Local tryRead(string path, bool usePath)
		{
			if (usePath)
			{
				path = ReadWrite.PATH + path;
			}
			string path2 = Path.Combine(path, Provider.language + ".dat");
			string path3 = Path.Combine(path, "English.dat");
			if (ReadWrite.fileExists(path2, false, false))
			{
				DatDictionary data = ReadWrite.ReadDataWithoutHash(path2);
				DatDictionary fallbackData = Provider.languageIsEnglish ? null : ReadWrite.ReadDataWithoutHash(path3);
				return new Local(data, fallbackData);
			}
			if (ReadWrite.fileExists(path3, false, false))
			{
				return new Local(ReadWrite.ReadDataWithoutHash(path3));
			}
			return new Local();
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0007BF2C File Offset: 0x0007A12C
		public static Local read(string path)
		{
			string path2 = Provider.localizationRoot + path;
			string path3 = Localization.englishLocalizationRoot + path;
			if (ReadWrite.fileExists(path2, false, false))
			{
				DatDictionary data = ReadWrite.ReadDataWithoutHash(path2);
				DatDictionary fallbackData = Provider.languageIsEnglish ? null : ReadWrite.ReadDataWithoutHash(path3);
				return new Local(data, fallbackData);
			}
			if (ReadWrite.fileExists(path3, false, false))
			{
				return new Local(ReadWrite.ReadDataWithoutHash(path3));
			}
			return new Local();
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0007BF94 File Offset: 0x0007A194
		private static void scanFile(string path)
		{
			Dictionary<string, IDatNode> dictionary = ReadWrite.ReadDataWithoutHash(ReadWrite.PATH + "/Localization/English/" + path);
			DatDictionary datDictionary = ReadWrite.ReadDataWithoutHash(Provider.localizationRoot + path);
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (KeyValuePair<string, IDatNode> keyValuePair in dictionary)
			{
				DatValue datValue = keyValuePair.Value as DatValue;
				if (datValue != null)
				{
					list.Add(new KeyValuePair<string, string>(keyValuePair.Key, datValue.value));
				}
			}
			List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
			foreach (KeyValuePair<string, IDatNode> keyValuePair2 in datDictionary)
			{
				DatValue datValue2 = keyValuePair2.Value as DatValue;
				if (datValue2 != null)
				{
					list2.Add(new KeyValuePair<string, string>(keyValuePair2.Key, datValue2.value));
				}
			}
			Localization.keys.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				string key = list[i].Key;
				bool flag = false;
				for (int j = 0; j < list2.Count; j++)
				{
					string key2 = list2[j].Key;
					if (key == key2)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Localization.keys.Add(key);
				}
			}
			if (Localization.keys.Count > 0)
			{
				Localization.messages.Add(path + " has " + Localization.keys.Count.ToString() + " new keys:");
				for (int k = 0; k < Localization.keys.Count; k++)
				{
					Localization.messages.Add("[" + k.ToString() + "]: " + Localization.keys[k]);
				}
			}
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0007C198 File Offset: 0x0007A398
		private static void scanFolder(string path)
		{
			string[] files = ReadWrite.getFiles("/Localization/English/" + path, true);
			string[] files2 = ReadWrite.getFiles(Provider.localizationRoot + path, false);
			for (int i = 0; i < files.Length; i++)
			{
				string fileName = Path.GetFileName(files[i]);
				bool flag = false;
				for (int j = 0; j < files2.Length; j++)
				{
					string fileName2 = Path.GetFileName(files2[j]);
					if (fileName == fileName2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Localization.scanFile(path + "/" + fileName);
				}
				else
				{
					Localization.messages.Add("New file \"" + fileName + "\" in " + path);
				}
			}
			string[] folders = ReadWrite.getFolders("/Localization/English/" + path, true);
			string[] folders2 = ReadWrite.getFolders(Provider.localizationRoot + path, false);
			for (int k = 0; k < folders.Length; k++)
			{
				string fileName3 = Path.GetFileName(folders[k]);
				bool flag2 = false;
				for (int l = 0; l < folders2.Length; l++)
				{
					string fileName4 = Path.GetFileName(folders2[l]);
					if (fileName3 == fileName4)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					Localization.scanFolder(path + "/" + fileName3);
				}
				else
				{
					Localization.messages.Add("New folder \"" + fileName3 + "\" in " + path);
				}
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0007C2F0 File Offset: 0x0007A4F0
		public static void refresh()
		{
			if (Localization.messages == null)
			{
				Localization._messages = new List<string>();
			}
			else
			{
				Localization.messages.Clear();
			}
			Localization.scanFolder("/Player");
			Localization.scanFolder("/Menu");
			Localization.scanFolder("/Server");
			Localization.scanFolder("/Editor");
		}

		// Token: 0x04000FB5 RID: 4021
		private static List<string> _messages;

		// Token: 0x04000FB6 RID: 4022
		private static List<string> keys = new List<string>();

		// Token: 0x04000FB7 RID: 4023
		private static string englishLocalizationRoot = Path.Combine(ReadWrite.PATH, "Localization", "English");
	}
}
