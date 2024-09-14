using System;
using System.Collections.Generic;
using SDG.Unturned;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C1 RID: 193
	public class KeyValueTableTypeReaderRegistry
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x000147A4 File Offset: 0x000129A4
		public static T read<T>(IFormattedFileReader input)
		{
			IFormattedTypeReader formattedTypeReader;
			if (KeyValueTableTypeReaderRegistry.readers.TryGetValue(typeof(T), ref formattedTypeReader))
			{
				object obj = formattedTypeReader.read(input);
				if (obj == null)
				{
					return default(T);
				}
				return (T)((object)obj);
			}
			else
			{
				if (!typeof(T).IsEnum)
				{
					string text = "Failed to find reader for: ";
					Type typeFromHandle = typeof(T);
					UnturnedLog.error(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
					return default(T);
				}
				string text2 = input.readValue();
				if (string.IsNullOrEmpty(text2))
				{
					return default(T);
				}
				return (T)((object)Enum.Parse(typeof(T), text2, true));
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00014854 File Offset: 0x00012A54
		public static object read(IFormattedFileReader input, Type type)
		{
			IFormattedTypeReader formattedTypeReader;
			if (KeyValueTableTypeReaderRegistry.readers.TryGetValue(type, ref formattedTypeReader))
			{
				object obj = formattedTypeReader.read(input);
				if (obj == null)
				{
					return type.getDefaultValue();
				}
				return obj;
			}
			else
			{
				if (!type.IsEnum)
				{
					UnturnedLog.error("Failed to find reader for: " + ((type != null) ? type.ToString() : null));
					return type.getDefaultValue();
				}
				string text = input.readValue();
				if (string.IsNullOrEmpty(text))
				{
					return type.getDefaultValue();
				}
				return Enum.Parse(type, text, true);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000148CD File Offset: 0x00012ACD
		public static void add<T>(IFormattedTypeReader reader)
		{
			KeyValueTableTypeReaderRegistry.add(typeof(T), reader);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000148DF File Offset: 0x00012ADF
		public static void add(Type type, IFormattedTypeReader reader)
		{
			KeyValueTableTypeReaderRegistry.readers.Add(type, reader);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000148ED File Offset: 0x00012AED
		public static void remove<T>()
		{
			KeyValueTableTypeReaderRegistry.remove(typeof(T));
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000148FE File Offset: 0x00012AFE
		public static void remove(Type type)
		{
			KeyValueTableTypeReaderRegistry.readers.Remove(type);
		}

		// Token: 0x040001FB RID: 507
		private static Dictionary<Type, IFormattedTypeReader> readers = new Dictionary<Type, IFormattedTypeReader>();
	}
}
