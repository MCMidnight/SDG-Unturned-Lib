using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200080E RID: 2062
	public class ProfanityFilter
	{
		// Token: 0x06004683 RID: 18051 RVA: 0x001A43C2 File Offset: 0x001A25C2
		public static string[] getCurseWords()
		{
			if (ProfanityFilter.curseWords == null)
			{
				ProfanityFilter.LoadCurseWords();
			}
			return ProfanityFilter.curseWords;
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x001A43D5 File Offset: 0x001A25D5
		internal static void InitSteam()
		{
			if (!ProfanityFilter.shouldInitSteamTextFiltering)
			{
				UnturnedLog.info("Not initializing Steam text filtering");
				return;
			}
			if (SteamUtils.InitFilterText(0U))
			{
				ProfanityFilter.filter = new ProfanityFilter.FilterDelegate(ProfanityFilter.SteamFilter);
				return;
			}
			UnturnedLog.info("Unable to initialize Steam text filtering");
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x001A4414 File Offset: 0x001A2614
		private static void SteamFilter(ref string message)
		{
			string text;
			if (SteamUtils.FilterText(ETextFilteringContext.k_ETextFilteringContextUnknown, CSteamID.Nil, message, out text, (uint)(message.Length * 2 + 1)) > 0)
			{
				message = text;
			}
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x001A4441 File Offset: 0x001A2641
		private static void NaiveDefaultFilter(ref string message)
		{
			ProfanityFilter.filterOutCurseWords(ref message, '#');
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x001A444C File Offset: 0x001A264C
		internal static void ApplyFilter(bool enableProfanityFilter, ref string message)
		{
			if (ProfanityFilter.NaiveContainsHardcodedBannedWord(message))
			{
				message = "<3";
				return;
			}
			if (enableProfanityFilter)
			{
				ProfanityFilter.filter(ref message);
			}
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x001A4470 File Offset: 0x001A2670
		public static bool filterOutCurseWords(ref string text, char replacementChar = '#')
		{
			bool result = false;
			if (text.Length > 0)
			{
				foreach (string text2 in ProfanityFilter.getCurseWords())
				{
					int num = ProfanityFilter.indexOfCurseWord(text, text2, 0);
					while (num != -1)
					{
						if ((num == 0 || !char.IsLetterOrDigit(text.get_Chars(num - 1))) && (num == text.Length - text2.Length || !char.IsLetterOrDigit(text.get_Chars(num + text2.Length))))
						{
							ProfanityFilter.replaceCurseWord(ref text, num, text2.Length, replacementChar);
							num = ProfanityFilter.indexOfCurseWord(text, text2, num);
							result = true;
						}
						else
						{
							num = ProfanityFilter.indexOfCurseWord(text, text2, num + 1);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x001A452C File Offset: 0x001A272C
		public static bool NaiveContainsHardcodedBannedWord(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return false;
			}
			foreach (string text in ProfanityFilter.hardcodedBannedWords)
			{
				for (int num = ProfanityFilter.indexOfCurseWord(message, text, 0); num != -1; num = ProfanityFilter.indexOfCurseWord(message, text, num + 1))
				{
					if ((num == 0 || !char.IsLetterOrDigit(message.get_Chars(num - 1))) && (num == message.Length - text.Length || !char.IsLetterOrDigit(message.get_Chars(num + text.Length))))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x001A45B0 File Offset: 0x001A27B0
		private static int indexOfCurseWord(string userText, string curseWord, int startIndex)
		{
			int num = userText.Length - curseWord.Length;
			for (int i = startIndex; i <= num; i++)
			{
				bool flag = true;
				for (int j = 0; j < curseWord.Length; j++)
				{
					char c = char.ToLower(userText.get_Chars(i + j));
					char c2 = curseWord.get_Chars(j);
					bool flag2 = c == c2;
					if (!flag2)
					{
						if (c2 <= 'l')
						{
							if (c2 != 'a')
							{
								switch (c2)
								{
								case 'e':
									flag2 = (c == '3');
									break;
								case 'f':
								case 'g':
									break;
								case 'h':
									flag2 = (c == '#');
									break;
								case 'i':
									flag2 = (c == '1');
									break;
								default:
									if (c2 == 'l')
									{
										flag2 = (c == '1');
									}
									break;
								}
							}
							else
							{
								flag2 = (c == '4' || c == '@');
							}
						}
						else if (c2 != 'o')
						{
							if (c2 != 's')
							{
								if (c2 == 't')
								{
									flag2 = (c == '7');
								}
							}
							else
							{
								flag2 = (c == '$' || c == '5');
							}
						}
						else
						{
							flag2 = (c == '0');
						}
						if (!flag2)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x001A46D4 File Offset: 0x001A28D4
		private static void replaceCurseWord(ref string text, int startIndex, int curseWordLength, char replacementChar)
		{
			string text2 = text.Substring(0, startIndex);
			for (int i = 0; i < curseWordLength; i++)
			{
				text2 += replacementChar.ToString();
			}
			text2 += text.Substring(startIndex + curseWordLength, text.Length - startIndex - curseWordLength);
			text = text2;
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001A4724 File Offset: 0x001A2924
		private static void LoadCurseWords()
		{
			if (string.IsNullOrEmpty(Provider.localizationRoot))
			{
				ProfanityFilter.curseWords = File.ReadAllLines(ReadWrite.PATH + "/Localization/English/Curse_Words.txt");
			}
			else
			{
				string text = Provider.localizationRoot + "/Curse_Words.txt";
				if (File.Exists(text))
				{
					ProfanityFilter.curseWords = File.ReadAllLines(text);
				}
				else
				{
					string text2 = Provider.path + "/English/Curse_Words.txt";
					if (File.Exists(text2))
					{
						ProfanityFilter.curseWords = File.ReadAllLines(text2);
					}
					else
					{
						ProfanityFilter.curseWords = new string[0];
					}
				}
			}
			if (ProfanityFilter.curseWords == null || ProfanityFilter.curseWords.Length < 1)
			{
				UnturnedLog.error("Failed to load list of curse words for profanity filter!");
				ProfanityFilter.curseWords = new string[0];
				return;
			}
			ProfanityFilter.ProcessLoadedCurseWords();
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x001A47DC File Offset: 0x001A29DC
		private static void ProcessLoadedCurseWords()
		{
			List<string> list = new List<string>();
			for (int i = ProfanityFilter.curseWords.Length - 1; i >= 0; i--)
			{
				string text = ProfanityFilter.curseWords[i];
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("#"))
				{
					list.Add(text);
				}
			}
			ProfanityFilter.curseWords = list.ToArray();
		}

		// Token: 0x04002F8F RID: 12175
		private static string[] curseWords = null;

		// Token: 0x04002F90 RID: 12176
		internal static ProfanityFilter.FilterDelegate filter = new ProfanityFilter.FilterDelegate(ProfanityFilter.NaiveDefaultFilter);

		// Token: 0x04002F91 RID: 12177
		public static CommandLineFlag shouldInitSteamTextFiltering = new CommandLineFlag(true, "-NoSteamTextFiltering");

		/// <summary>
		/// 2023-04-17: suggestion is to have a hardcoded list of hate speech that gets filtered
		/// regardless of whether profanity filter is enabled. (https://forum.smartlydressedgames.com/t/22477)
		/// </summary>
		// Token: 0x04002F92 RID: 12178
		private static readonly string[] hardcodedBannedWords = new string[]
		{
			"nigger",
			"niggers",
			"niger",
			"nigers",
			"jew",
			"jews",
			"fag",
			"fags",
			"faggot",
			"faggots",
			"fagot",
			"fagots",
			"faggit",
			"faggits",
			"fagit",
			"fagits",
			"rape",
			"raped"
		};

		// Token: 0x02000A1E RID: 2590
		// (Invoke) Token: 0x06004D8D RID: 19853
		internal delegate void FilterDelegate(ref string message);
	}
}
