using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200080B RID: 2059
	public class Parser
	{
		// Token: 0x06004674 RID: 18036 RVA: 0x001A4010 File Offset: 0x001A2210
		public static bool trySplitStart(string serial, out string start, out string end)
		{
			start = "";
			end = "";
			int num = serial.IndexOf(' ');
			if (num == -1)
			{
				return false;
			}
			start = serial.Substring(0, num);
			end = serial.Substring(num + 1, serial.Length - num - 1);
			return true;
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001A405C File Offset: 0x001A225C
		public static bool trySplitEnd(string serial, out string start, out string end)
		{
			start = "";
			end = "";
			int num = serial.LastIndexOf(' ');
			if (num == -1)
			{
				return false;
			}
			start = serial.Substring(0, num);
			end = serial.Substring(num + 1, serial.Length - num - 1);
			return true;
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x001A40A8 File Offset: 0x001A22A8
		public static string[] getComponentsFromSerial(string serial, char delimiter)
		{
			List<string> list = new List<string>();
			int num;
			for (int i = 0; i < serial.Length; i = num + 1)
			{
				num = serial.IndexOf(delimiter, i);
				if (num == -1)
				{
					list.Add(serial.Substring(i, serial.Length - i));
					break;
				}
				list.Add(serial.Substring(i, num - i));
			}
			return list.ToArray();
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x001A4108 File Offset: 0x001A2308
		public static string getSerialFromComponents(char delimiter, params object[] components)
		{
			string text = "";
			for (int i = 0; i < components.Length; i++)
			{
				text += components[i].ToString();
				if (i < components.Length - 1)
				{
					text += delimiter.ToString();
				}
			}
			return text;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x001A4150 File Offset: 0x001A2350
		public static bool checkIP(string ip)
		{
			int num = ip.IndexOf('.');
			if (num == -1)
			{
				return false;
			}
			int num2 = ip.IndexOf('.', num + 1);
			if (num2 == -1)
			{
				return false;
			}
			int num3 = ip.IndexOf('.', num2 + 1);
			return num3 != -1 && ip.IndexOf('.', num3 + 1) == -1;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x001A41A4 File Offset: 0x001A23A4
		public static bool TryGetUInt32FromIP(string ip, out uint value)
		{
			value = 0U;
			if (string.IsNullOrWhiteSpace(ip))
			{
				return false;
			}
			string[] array = ip.Split('.', 0);
			if (array.Length != 4)
			{
				return false;
			}
			uint num;
			if (!uint.TryParse(array[0], ref num))
			{
				return false;
			}
			value |= (num & 255U) << 24;
			if (!uint.TryParse(array[1], ref num))
			{
				return false;
			}
			value |= (num & 255U) << 16;
			if (!uint.TryParse(array[2], ref num))
			{
				return false;
			}
			value |= (num & 255U) << 8;
			if (uint.TryParse(array[3], ref num))
			{
				value |= (num & 255U);
				return true;
			}
			return false;
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x001A424C File Offset: 0x001A244C
		public static uint getUInt32FromIP(string ip)
		{
			uint result;
			Parser.TryGetUInt32FromIP(ip, out result);
			return result;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x001A4264 File Offset: 0x001A2464
		public static string getIPFromUInt32(uint ip)
		{
			return string.Concat(new string[]
			{
				(ip >> 24 & 255U).ToString(),
				".",
				(ip >> 16 & 255U).ToString(),
				".",
				(ip >> 8 & 255U).ToString(),
				".",
				(ip & 255U).ToString()
			});
		}
	}
}
