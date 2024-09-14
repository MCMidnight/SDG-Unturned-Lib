using System;
using System.IO;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C4 RID: 196
	public class KeyValueTableWriter : IFormattedFileWriter
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00014A74 File Offset: 0x00012C74
		public virtual void writeKey(string key)
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('"');
			this.writer.Write(key);
			this.writer.Write('"');
			this.hasWritten = true;
			this.wroteKey = true;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00014ACE File Offset: 0x00012CCE
		public virtual void writeValue(string key, string value)
		{
			this.writeKey(key);
			this.writeValue(value);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00014AE0 File Offset: 0x00012CE0
		public virtual void writeValue(string value)
		{
			if (this.wroteKey)
			{
				this.writer.Write(' ');
			}
			else
			{
				if (this.hasWritten)
				{
					this.writer.WriteLine();
				}
				this.writeIndents();
			}
			this.writer.Write('"');
			this.writer.Write(value);
			this.writer.Write('"');
			this.wroteKey = false;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00014B4A File Offset: 0x00012D4A
		public virtual void writeValue(string key, object value)
		{
			this.writeKey(key);
			this.writeValue(value);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual void writeValue(object value)
		{
			if (value is IFormattedFileWritable)
			{
				(value as IFormattedFileWritable).write(this);
				return;
			}
			KeyValueTableTypeWriterRegistry.write(this, value);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00014B78 File Offset: 0x00012D78
		public virtual void writeValue<T>(string key, T value)
		{
			this.writeKey(key);
			this.writeValue<T>(value);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00014B88 File Offset: 0x00012D88
		public virtual void writeValue<T>(T value)
		{
			if (value is IFormattedFileWritable)
			{
				(value as IFormattedFileWritable).write(this);
				return;
			}
			KeyValueTableTypeWriterRegistry.write<T>(this, value);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00014BB0 File Offset: 0x00012DB0
		public virtual void beginObject(string key)
		{
			this.writeKey(key);
			this.beginObject();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00014BC0 File Offset: 0x00012DC0
		public virtual void beginObject()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('{');
			this.indentationCount++;
			this.hasWritten = true;
			this.wroteKey = false;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00014C0F File Offset: 0x00012E0F
		public virtual void endObject()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.indentationCount--;
			this.writeIndents();
			this.writer.Write('}');
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00014C45 File Offset: 0x00012E45
		public virtual void beginArray(string key)
		{
			this.writeKey(key);
			this.beginArray();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00014C54 File Offset: 0x00012E54
		public virtual void beginArray()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('[');
			this.indentationCount++;
			this.hasWritten = true;
			this.wroteKey = false;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00014CA3 File Offset: 0x00012EA3
		public virtual void endArray()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.indentationCount--;
			this.writeIndents();
			this.writer.Write(']');
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014CDC File Offset: 0x00012EDC
		protected virtual void writeIndents()
		{
			for (int i = 0; i < this.indentationCount; i++)
			{
				this.writer.Write('\t');
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00014D07 File Offset: 0x00012F07
		public KeyValueTableWriter(StreamWriter writer)
		{
			this.writer = writer;
			this.indentationCount = 0;
			this.hasWritten = false;
			this.wroteKey = false;
		}

		// Token: 0x040001FE RID: 510
		protected StreamWriter writer;

		// Token: 0x040001FF RID: 511
		protected int indentationCount;

		// Token: 0x04000200 RID: 512
		protected bool hasWritten;

		// Token: 0x04000201 RID: 513
		protected bool wroteKey;
	}
}
