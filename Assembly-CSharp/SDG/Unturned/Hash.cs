using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020007F9 RID: 2041
	public class Hash
	{
		// Token: 0x06004616 RID: 17942 RVA: 0x001A2D99 File Offset: 0x001A0F99
		public static byte[] SHA1(byte[] bytes)
		{
			return Hash.service.ComputeHash(bytes);
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x001A2DA6 File Offset: 0x001A0FA6
		public static byte[] SHA1(Stream stream)
		{
			return Hash.service.ComputeHash(stream);
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x001A2DB3 File Offset: 0x001A0FB3
		public static byte[] SHA1(string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			return Hash.SHA1(Encoding.UTF8.GetBytes(text));
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x001A2DCF File Offset: 0x001A0FCF
		public static byte[] SHA1(CSteamID steamID)
		{
			return Hash.SHA1(BitConverter.GetBytes(steamID.m_SteamID));
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x001A2DE4 File Offset: 0x001A0FE4
		public static bool verifyHash(byte[] hash_0, byte[] hash_1)
		{
			if (hash_0.Length != 20 || hash_1.Length != 20)
			{
				return false;
			}
			for (int i = 0; i < hash_0.Length; i++)
			{
				if (hash_0[i] != hash_1[i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Combine two existing 20-byte hashes.
		/// </summary>
		// Token: 0x0600461B RID: 17947 RVA: 0x001A2E1A File Offset: 0x001A101A
		public static byte[] combineSHA1Hashes(byte[] a, byte[] b)
		{
			if (a.Length != 20 || b.Length != 20)
			{
				throw new Exception("both lengths should be 20");
			}
			a.CopyTo(Hash._40bytes, 0);
			b.CopyTo(Hash._40bytes, 20);
			return Hash.SHA1(Hash._40bytes);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x001A2E58 File Offset: 0x001A1058
		public static byte[] combine(params byte[][] hashes)
		{
			byte[] array = new byte[hashes.Length * 20];
			for (int i = 0; i < hashes.Length; i++)
			{
				hashes[i].CopyTo(array, i * 20);
			}
			return Hash.SHA1(array);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x001A2E94 File Offset: 0x001A1094
		public static byte[] combine(List<byte[]> hashes)
		{
			byte[] array = new byte[hashes.Count * 20];
			for (int i = 0; i < hashes.Count; i++)
			{
				hashes[i].CopyTo(array, i * 20);
			}
			return Hash.SHA1(array);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x001A2ED8 File Offset: 0x001A10D8
		public static string toString(byte[] hash)
		{
			if (hash == null)
			{
				return "null";
			}
			string text = "";
			for (int i = 0; i < hash.Length; i++)
			{
				text += hash[i].ToString("X2");
			}
			return text;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x001A2F1C File Offset: 0x001A111C
		internal static string ToCodeString(byte[] hash)
		{
			StringBuilder stringBuilder = new StringBuilder(hash.Length * 6);
			stringBuilder.Append("0x");
			stringBuilder.Append(hash[0].ToString("X2"));
			for (int i = 1; i < hash.Length; i++)
			{
				stringBuilder.Append(", 0x");
				stringBuilder.Append(hash[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x001A2F90 File Offset: 0x001A1190
		public static void log(byte[] hash)
		{
			if (hash == null || hash.Length != 20)
			{
				return;
			}
			CommandWindow.Log(Hash.toString(hash));
		}

		// Token: 0x04002F32 RID: 12082
		private static SHA1CryptoServiceProvider service = new SHA1CryptoServiceProvider();

		// Token: 0x04002F33 RID: 12083
		private static byte[] _40bytes = new byte[40];
	}
}
