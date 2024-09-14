using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows mappers to bulk replace assets by listing pairs in a text file.
	/// https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/2275
	/// </summary>
	// Token: 0x020003FD RID: 1021
	public static class EditorAssetRedirector
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x0006DBE9 File Offset: 0x0006BDE9
		public static bool HasRedirects
		{
			get
			{
				return EditorAssetRedirector.mappings != null && EditorAssetRedirector.mappings.Count > 0;
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0006DC04 File Offset: 0x0006BE04
		public static ObjectAsset RedirectObject(Guid oldGuid)
		{
			Guid guid;
			if (EditorAssetRedirector.mappings.TryGetValue(oldGuid, ref guid))
			{
				return Assets.find(guid) as ObjectAsset;
			}
			return null;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0006DC30 File Offset: 0x0006BE30
		static EditorAssetRedirector()
		{
			string text = Path.Combine(ReadWrite.PATH, "EditorAssetRedirectors.txt");
			if (!File.Exists(text))
			{
				return;
			}
			EditorAssetRedirector.mappings = new Dictionary<Guid, Guid>();
			string[] array = File.ReadAllLines(text);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				if (!string.IsNullOrWhiteSpace(text2) && !text2.StartsWith("#") && !text2.StartsWith("//"))
				{
					int num = text2.IndexOf("->");
					if (num < 0 || num + 2 >= text2.Length)
					{
						UnturnedLog.warn("Unable to split \"->\" in editor asset redirect \"{0}\" (line {1})", new object[]
						{
							text2,
							i + 1
						});
					}
					else
					{
						string text3 = text2.Substring(0, num).Trim();
						string text4 = text2.Substring(num + 2).Trim();
						Guid guid;
						Guid guid2;
						Guid guid3;
						if (!Guid.TryParse(text3, ref guid))
						{
							UnturnedLog.warn("Unable to parse \"{0}\" as old guid from \"{1}\" (line {2})", new object[]
							{
								text3,
								text2,
								i + 1
							});
						}
						else if (!Guid.TryParse(text4, ref guid2))
						{
							UnturnedLog.warn("Unable to parse \"{0}\" as new guid from \"{1}\" (line {2})", new object[]
							{
								text4,
								text2,
								i + 1
							});
						}
						else if (EditorAssetRedirector.mappings.TryGetValue(guid, ref guid3))
						{
							UnturnedLog.warn("Editor asset redirect {0} to {1} (line {2}) conflicts with prior redirect to {3}", new object[]
							{
								guid,
								guid2,
								i + 1,
								guid3
							});
						}
						else
						{
							EditorAssetRedirector.mappings.Add(guid, guid2);
							UnturnedLog.info("Editor redirecting asset {0} to {1}", new object[]
							{
								guid,
								guid2
							});
						}
					}
				}
			}
		}

		// Token: 0x04000E74 RID: 3700
		private static Dictionary<Guid, Guid> mappings;
	}
}
