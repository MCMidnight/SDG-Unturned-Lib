using System;
using System.Collections.Generic;
using System.Globalization;

namespace SDG.Unturned
{
	// Token: 0x020007F6 RID: 2038
	public class CommandLine
	{
		/// <summary>
		/// When Steam parses a steam://connect/ip:port URL it requires the query port (e.g. 27015).
		/// </summary>
		// Token: 0x06004609 RID: 17929 RVA: 0x001A2600 File Offset: 0x001A0800
		public static bool TryGetSteamConnect(string line, out uint ip, out ushort queryPort, out string pass)
		{
			ip = 0U;
			queryPort = 0;
			pass = "";
			int num = line.ToLower().IndexOf("+connect ");
			if (num == -1)
			{
				return false;
			}
			int num2 = line.IndexOf(':', num + 9);
			string text = line.Substring(num + 9, num2 - num - 9);
			if (Parser.checkIP(text))
			{
				ip = Parser.getUInt32FromIP(text);
			}
			else if (!uint.TryParse(text, 511, CultureInfo.InvariantCulture, ref ip))
			{
				return false;
			}
			int num3 = line.IndexOf(' ', num2 + 1);
			if (num3 == -1)
			{
				if (!ushort.TryParse(line.Substring(num2 + 1, line.Length - num2 - 1), 511, CultureInfo.InvariantCulture, ref queryPort))
				{
					return false;
				}
				int num4 = line.ToLower().IndexOf("+password ");
				if (num4 != -1)
				{
					pass = line.Substring(num4 + 10, line.Length - num4 - 10);
				}
				return true;
			}
			else
			{
				if (!ushort.TryParse(line.Substring(num2 + 1, num3 - num2 - 1), 511, CultureInfo.InvariantCulture, ref queryPort))
				{
					return false;
				}
				int num5 = line.ToLower().IndexOf("+password ");
				if (num5 != -1)
				{
					pass = line.Substring(num5 + 10, line.Length - num5 - 10);
				}
				return true;
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x001A2738 File Offset: 0x001A0938
		public static bool tryGetLobby(string line, out ulong lobby)
		{
			lobby = 0UL;
			int num = line.ToLower().IndexOf("+connect_lobby ");
			if (num == -1)
			{
				return false;
			}
			int num2 = line.IndexOf(' ', num + 15);
			if (num2 == -1)
			{
				return ulong.TryParse(line.Substring(num + 15, line.Length - num - 15), 511, CultureInfo.InvariantCulture, ref lobby);
			}
			return ulong.TryParse(line.Substring(num + 15, num2 - num - 15), 511, CultureInfo.InvariantCulture, ref lobby);
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x001A27B8 File Offset: 0x001A09B8
		public static bool tryGetLanguage(out string local, out string path)
		{
			local = "";
			path = "";
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length)
			{
				string text = null;
				if (commandLineArgs[i].Length > 6 && (commandLineArgs[i].StartsWith("-Lang=", 3) || commandLineArgs[i].StartsWith("+Lang=", 3)))
				{
					text = commandLineArgs[i].Substring(6);
					goto IL_E6;
				}
				if (commandLineArgs[i].Length > 5 && (commandLineArgs[i].StartsWith("-Loc=", 3) || commandLineArgs[i].StartsWith("+Loc=", 3)))
				{
					text = commandLineArgs[i].Substring(5);
					goto IL_E6;
				}
				if (commandLineArgs[i].Length <= 1 || !commandLineArgs[i].StartsWith("+"))
				{
					goto IL_E6;
				}
				if (commandLineArgs[i].IndexOf('/') < 0 && !commandLineArgs[i].StartsWith("+connect") && !commandLineArgs[i].StartsWith("+password"))
				{
					text = commandLineArgs[i].Substring(1);
					goto IL_E6;
				}
				IL_229:
				i++;
				continue;
				IL_E6:
				if (string.IsNullOrEmpty(text))
				{
					goto IL_229;
				}
				if (Provider.provider.workshopService.ugc != null)
				{
					for (int j = 0; j < Provider.provider.workshopService.ugc.Count; j++)
					{
						SteamContent steamContent = Provider.provider.workshopService.ugc[j];
						if (steamContent.type == ESteamUGCType.LOCALIZATION && ReadWrite.folderExists(steamContent.path + "/" + text, false))
						{
							local = text;
							path = steamContent.path + "/";
							UnturnedLog.info("Parsed language '{0}' on command-line, and found in workshop item {1}", new object[]
							{
								text,
								steamContent.publishedFileID
							});
							return true;
						}
					}
				}
				if (ReadWrite.folderExists("/Localization/" + text))
				{
					local = text;
					path = ReadWrite.PATH + "/Localization/";
					UnturnedLog.info("Parsed language '{0}' on command-line, and found in root Localization directory", new object[]
					{
						text
					});
					return true;
				}
				if (ReadWrite.folderExists("/Sandbox/" + text))
				{
					local = text;
					path = ReadWrite.PATH + "/Sandbox/";
					UnturnedLog.info("Parsed language '{0}' on command-line, and found in Sandbox directory", new object[]
					{
						text
					});
					return true;
				}
				UnturnedLog.warn("Parsed language '{0}' on command-line, but unable to find related files", new object[]
				{
					text
				});
				goto IL_229;
			}
			return false;
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x001A29FC File Offset: 0x001A0BFC
		public static bool tryGetServer(out ESteamServerVisibility visibility, out string id)
		{
			visibility = ESteamServerVisibility.LAN;
			id = "";
			string commandLine = Environment.CommandLine;
			int num = commandLine.ToLower().IndexOf("+secureserver", 5);
			if (num != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num + 14, commandLine.Length - num - 14);
				return !(id == "Singleplayer");
			}
			int num2 = commandLine.ToLower().IndexOf("+insecureserver", 5);
			if (num2 != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num2 + 16, commandLine.Length - num2 - 16);
				return !(id == "Singleplayer");
			}
			int num3 = commandLine.ToLower().IndexOf("+internetserver", 5);
			if (num3 != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num3 + 16, commandLine.Length - num3 - 16);
				return !(id == "Singleplayer");
			}
			int num4 = commandLine.ToLower().IndexOf("+lanserver", 5);
			if (num4 != -1)
			{
				visibility = ESteamServerVisibility.LAN;
				id = commandLine.Substring(num4 + 11, commandLine.Length - num4 - 11);
				return !(id == "Singleplayer");
			}
			return false;
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x001A2B26 File Offset: 0x001A0D26
		public static bool tryGetVR()
		{
			return Environment.CommandLine.ToLower().IndexOf("-vr") != -1;
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x001A2B44 File Offset: 0x001A0D44
		public static string[] getCommands()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			List<string> list = new List<string>();
			GetCommands getCommands = CommandLine.onGetCommands;
			if (getCommands != null)
			{
				getCommands(list);
			}
			bool flag = false;
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Substring(0, 1) == "+")
				{
					flag = true;
				}
				else if (commandLineArgs[i].Substring(0, 1) == "-")
				{
					list.Add(commandLineArgs[i].Substring(1, commandLineArgs[i].Length - 1));
					flag = false;
				}
				else if (list.Count > 0 && !flag)
				{
					List<string> list2 = list;
					int num = list.Count - 1;
					list2[num] = list2[num] + " " + commandLineArgs[i];
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// Handles these cases:
		/// key value -&gt; value
		/// key=value -&gt; value
		/// key = value -&gt; value
		/// key  =  value -&gt; value
		/// key "value with spaces" -&gt; value with spaces
		/// key "value with \" quotation marks" -&gt; value with " quotation marks
		///
		/// Tested in CommandLineTests.cs
		/// </summary>
		// Token: 0x0600460F RID: 17935 RVA: 0x001A2C10 File Offset: 0x001A0E10
		public static bool TryParseValue(string input, string key, out string value)
		{
			value = null;
			if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			int num2;
			for (int i = 0; i < input.Length; i = num2)
			{
				int num = input.IndexOf(key, i, 3);
				if (num < 0)
				{
					return false;
				}
				num2 = num + key.Length;
				if (num2 >= input.Length)
				{
					return false;
				}
				char c = input.get_Chars(num2);
				if (c == '=' || char.IsWhiteSpace(c))
				{
					int j = num2 + 1;
					while (j < input.Length)
					{
						char c2 = input.get_Chars(j);
						if (c2 == '=' || char.IsWhiteSpace(c2))
						{
							j++;
						}
						else
						{
							if (input.get_Chars(j) != '"')
							{
								int num3 = input.IndexOf(' ', j);
								if (num3 < 0)
								{
									value = input.Substring(j);
								}
								else
								{
									int num4 = num3 - j;
									value = input.Substring(j, num4);
								}
								return true;
							}
							j++;
							int k = j;
							bool flag = false;
							value = string.Empty;
							while (k < input.Length)
							{
								char c3 = input.get_Chars(k);
								if (c3 == '\\')
								{
									k++;
									flag = true;
								}
								else
								{
									if (c3 == '"' && !flag)
									{
										return true;
									}
									value += c3.ToString();
									k++;
									flag = false;
								}
							}
							return false;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x001A2D58 File Offset: 0x001A0F58
		public static bool TryParseValue(string key, out string value)
		{
			return CommandLine.TryParseValue(Environment.CommandLine, key, out value);
		}

		// Token: 0x04002F31 RID: 12081
		public static GetCommands onGetCommands;
	}
}
