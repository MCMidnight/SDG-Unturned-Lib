using System;

namespace SDG.Unturned
{
	// Token: 0x02000426 RID: 1062
	public class Local
	{
		// Token: 0x0600200E RID: 8206 RVA: 0x0007BB18 File Offset: 0x00079D18
		public string read(string key)
		{
			if (this.data != null)
			{
				return this.data.GetString(key, null);
			}
			return null;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0007BB34 File Offset: 0x00079D34
		public string format(string key)
		{
			string result;
			if (this.TryReadString(key, out result))
			{
				return result;
			}
			return key;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0007BB50 File Offset: 0x00079D50
		public string format(string key, object arg0)
		{
			string text;
			if (this.TryReadString(key, out text))
			{
				try
				{
					return string.Format(text, arg0);
				}
				catch
				{
					UnturnedLog.error(string.Format("Caught localization string formatting exception (key: \"{0}\" text: \"{1}\" arg0: \"{2}\")", key, text, arg0));
					return key;
				}
				return key;
			}
			return key;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0007BB9C File Offset: 0x00079D9C
		internal static string FormatText(string text, object arg0)
		{
			string result;
			try
			{
				result = string.Format(text, arg0);
			}
			catch
			{
				UnturnedLog.error(string.Format("Caught localization string formatting exception (text: \"{0}\" arg0: \"{1}\")", text, arg0));
				result = text;
			}
			return result;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0007BBDC File Offset: 0x00079DDC
		public string format(string key, object arg0, object arg1)
		{
			string text;
			if (this.TryReadString(key, out text))
			{
				try
				{
					return string.Format(text, arg0, arg1);
				}
				catch
				{
					UnturnedLog.error(string.Format("Caught localization string formatting exception (key: \"{0}\" text: \"{1}\" arg0: \"{2}\" arg1: \"{3}\")", new object[]
					{
						key,
						text,
						arg0,
						arg1
					}));
					return key;
				}
				return key;
			}
			return key;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0007BC3C File Offset: 0x00079E3C
		internal static string FormatText(string text, object arg0, object arg1)
		{
			string result;
			try
			{
				result = string.Format(text, arg0, arg1);
			}
			catch
			{
				UnturnedLog.error(string.Format("Caught localization string formatting exception (text: \"{0}\" arg0: \"{1}\" arg1: \"{2}\")", text, arg0, arg1));
				result = text;
			}
			return result;
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0007BC7C File Offset: 0x00079E7C
		public string format(string key, object arg0, object arg1, object arg2)
		{
			string text;
			if (this.TryReadString(key, out text))
			{
				try
				{
					return string.Format(text, arg0, arg1, arg2);
				}
				catch
				{
					UnturnedLog.error(string.Format("Caught localization string formatting exception (key: \"{0}\" text: \"{1}\" arg0: \"{2}\" arg1: \"{3}\" arg2: \"{4}\")", new object[]
					{
						key,
						text,
						arg0,
						arg1,
						arg2
					}));
					return key;
				}
				return key;
			}
			return key;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0007BCE4 File Offset: 0x00079EE4
		public static string FormatText(string text, object arg0, object arg1, object arg2)
		{
			string result;
			try
			{
				result = string.Format(text, arg0, arg1, arg2);
			}
			catch
			{
				UnturnedLog.error(string.Format("Caught localization string formatting exception (text: \"{0}\" arg0: \"{1}\" arg1: \"{2}\" arg2: \"{3}\")", new object[]
				{
					text,
					arg0,
					arg1,
					arg2
				}));
				result = text;
			}
			return result;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0007BD38 File Offset: 0x00079F38
		public string format(string key, params object[] args)
		{
			string text;
			if (this.TryReadString(key, out text))
			{
				try
				{
					return string.Format(text, args);
				}
				catch
				{
					string text2 = string.Empty;
					for (int i = 0; i < args.Length; i++)
					{
						if (text2.Length > 0)
						{
							text2 += " ";
						}
						text2 += string.Format("arg{0}: \"{1}\"", i, args[i]);
					}
					UnturnedLog.error(string.Concat(new string[]
					{
						"Caught localization string formatting exception (key: \"",
						key,
						"\" text: \"",
						text,
						"\" ",
						text2,
						")"
					}));
					return key;
				}
				return key;
			}
			return key;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0007BDF8 File Offset: 0x00079FF8
		public bool has(string key)
		{
			return this.data != null && this.data.ContainsKey(key);
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0007BE10 File Offset: 0x0007A010
		public Local(DatDictionary newData) : this(newData, null)
		{
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0007BE1A File Offset: 0x0007A01A
		public Local(DatDictionary data, DatDictionary fallbackData)
		{
			this.data = data;
			this.fallbackData = fallbackData;
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0007BE30 File Offset: 0x0007A030
		public Local()
		{
			this.data = null;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0007BE40 File Offset: 0x0007A040
		private bool TryReadString(string key, out string text)
		{
			text = null;
			return (this.data != null && this.data.TryGetString(key, out text) && !string.IsNullOrEmpty(text)) || (this.fallbackData != null && this.fallbackData.TryGetString(key, out text) && !string.IsNullOrEmpty(text));
		}

		// Token: 0x04000FB3 RID: 4019
		private DatDictionary data;

		// Token: 0x04000FB4 RID: 4020
		private DatDictionary fallbackData;
	}
}
