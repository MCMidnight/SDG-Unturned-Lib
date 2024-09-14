using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Utility for getting local hardware ID.
	///
	/// One option for future improvement would be using Windows Management Infrastructure (WMI) API:
	/// https://github.com/SmartlyDressedGames/Unturned-3.x/issues/1593
	/// </summary>
	// Token: 0x02000676 RID: 1654
	public static class LocalHwid
	{
		/// <summary>
		/// Get the local hardware ID(s).
		/// </summary>
		// Token: 0x060036EA RID: 14058 RVA: 0x00101450 File Offset: 0x000FF650
		public static byte[][] GetHwids()
		{
			byte[][] array = LocalHwid.InitHwids();
			if (array == null)
			{
				array = new byte[][]
				{
					LocalHwid.CreateRandomHwid()
				};
			}
			return array;
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x00101478 File Offset: 0x000FF678
		private static byte[] CreateRandomHwid()
		{
			byte[] array = new byte[20];
			for (int i = 0; i < 20; i++)
			{
				array[i] = (byte)Random.Range(0, 256);
			}
			return array;
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x001014AC File Offset: 0x000FF6AC
		private static byte[][] InitHwids()
		{
			List<byte[]> list = LocalHwid.GatherAvailableHwids();
			if (list == null || list.Count < 1)
			{
				return null;
			}
			if (list.Count > 8)
			{
				byte[][] array = new byte[8][];
				for (int i = 0; i < 8; i++)
				{
					int randomIndex = list.GetRandomIndex<byte[]>();
					array[i] = list[randomIndex];
					list.RemoveAtFast(randomIndex);
				}
				return array;
			}
			return list.ToArray();
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x0010150C File Offset: 0x000FF70C
		private static void GatherPlayerPrefsEntry(List<byte[]> results)
		{
			byte[] array = new byte[]
			{
				104,
				115,
				97,
				72,
				101,
				103,
				97,
				114,
				111,
				116,
				83,
				100,
				117,
				111,
				108,
				67
			};
			Array.Reverse<byte>(array);
			string @string = Encoding.UTF8.GetString(array);
			string text = PlayerPrefs.GetString(@string);
			bool flag = false;
			if (string.IsNullOrEmpty(text) || text.Length != 32)
			{
				text = Guid.NewGuid().ToString("N");
				PlayerPrefs.SetString(@string, text);
				flag = true;
			}
			byte b = 240;
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2.get_Chars(i);
				b = (byte)((char)(b * 3) + c);
			}
			if (PlayerPrefs.HasKey("unity.player_session_restoreflags"))
			{
				if (PlayerPrefs.GetInt("unity.player_session_restoreflags") != (int)b)
				{
					Provider.catPouncingMechanism = Random.Range(19.83f, 151.25f);
				}
			}
			else
			{
				PlayerPrefs.SetInt("unity.player_session_restoreflags", (int)b);
				flag = true;
			}
			if (flag)
			{
				PlayerPrefs.Save();
			}
			results.Add(Hash.SHA1("Zpsz+h>nJ!?4h2&nVPVw=DmG" + text));
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x00101608 File Offset: 0x000FF808
		private static void GatherConvenientSavedataEntry(List<byte[]> results)
		{
			byte[] array = new byte[]
			{
				101,
				104,
				99,
				97,
				67,
				101,
				114,
				111,
				116,
				83,
				109,
				101,
				116,
				73
			};
			Array.Reverse<byte>(array);
			string @string = Encoding.UTF8.GetString(array);
			string text;
			if (!ConvenientSavedata.get().read(@string, out text) || text.Length != 32)
			{
				text = Guid.NewGuid().ToString("N");
				ConvenientSavedata.get().write(@string, text);
			}
			byte b = 240;
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2.get_Chars(i);
				b = (byte)((char)(b * 3) + c);
			}
			long num;
			if (ConvenientSavedata.get().read("NewItemSeenPromotionId", out num))
			{
				if (num != (long)((ulong)b))
				{
					Provider.catPouncingMechanism = Random.Range(19.83f, 151.25f);
				}
			}
			else
			{
				ConvenientSavedata.get().write("NewItemSeenPromotionId", (long)((ulong)b));
			}
			results.Add(Hash.SHA1("Zpsz+h>nJ!?4h2&nVPVw=DmG" + text));
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x001016FB File Offset: 0x000FF8FB
		private static List<byte[]> GatherAvailableHwids()
		{
			List<byte[]> list = new List<byte[]>();
			LocalHwid.GatherPlayerPrefsEntry(list);
			LocalHwid.GatherConvenientSavedataEntry(list);
			return list;
		}

		/// <summary>
		/// Maximum number of HWIDs before server will reject connection request.
		/// </summary>
		// Token: 0x0400207D RID: 8317
		internal const byte MAX_HWIDS = 8;

		// Token: 0x0400207E RID: 8318
		private const string SALT = "Zpsz+h>nJ!?4h2&nVPVw=DmG";
	}
}
