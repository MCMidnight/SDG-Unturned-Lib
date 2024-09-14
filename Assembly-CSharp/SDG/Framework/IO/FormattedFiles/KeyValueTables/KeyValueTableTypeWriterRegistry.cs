using System;
using System.Collections.Generic;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C3 RID: 195
	public class KeyValueTableTypeWriterRegistry
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x000149A0 File Offset: 0x00012BA0
		public static void write<T>(IFormattedFileWriter output, T value)
		{
			IFormattedTypeWriter formattedTypeWriter;
			if (KeyValueTableTypeWriterRegistry.writers.TryGetValue(typeof(T), ref formattedTypeWriter))
			{
				formattedTypeWriter.write(output, value);
				return;
			}
			output.writeValue(value.ToString());
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000149E8 File Offset: 0x00012BE8
		public static void write(IFormattedFileWriter output, object value)
		{
			IFormattedTypeWriter formattedTypeWriter;
			if (KeyValueTableTypeWriterRegistry.writers.TryGetValue(value.GetType(), ref formattedTypeWriter))
			{
				formattedTypeWriter.write(output, value);
				return;
			}
			output.writeValue(value.ToString());
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00014A1E File Offset: 0x00012C1E
		public static void add<T>(IFormattedTypeWriter writer)
		{
			KeyValueTableTypeWriterRegistry.add(typeof(T), writer);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00014A30 File Offset: 0x00012C30
		public static void add(Type type, IFormattedTypeWriter writer)
		{
			KeyValueTableTypeWriterRegistry.writers.Add(type, writer);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00014A3E File Offset: 0x00012C3E
		public static void remove<T>()
		{
			KeyValueTableTypeWriterRegistry.remove(typeof(T));
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014A4F File Offset: 0x00012C4F
		public static void remove(Type type)
		{
			KeyValueTableTypeWriterRegistry.writers.Remove(type);
		}

		// Token: 0x040001FD RID: 509
		private static Dictionary<Type, IFormattedTypeWriter> writers = new Dictionary<Type, IFormattedTypeWriter>();
	}
}
