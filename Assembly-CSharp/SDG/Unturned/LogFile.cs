using System;
using System.IO;
using System.Text;

namespace SDG.Unturned
{
	// Token: 0x0200057D RID: 1405
	public class LogFile
	{
		/// <summary>
		/// Absolute path to *.log file.
		/// </summary>
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000C314A File Offset: 0x000C134A
		// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x000C3152 File Offset: 0x000C1352
		public string path { get; private set; }

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000C315C File Offset: 0x000C135C
		public LogFile(string path)
		{
			this.path = path;
			this.stream = new FileStream(path, 2, 2, 3);
			Encoding encoding = Encoding.GetEncoding(65001, new EncoderReplacementFallback(), new DecoderReplacementFallback());
			this.writer = new StreamWriter(this.stream, encoding);
			this.writer.AutoFlush = true;
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000C31B8 File Offset: 0x000C13B8
		public void writeLine(string line)
		{
			this.writer.WriteLine(line);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000C31C8 File Offset: 0x000C13C8
		public void close()
		{
			if (this.writer != null)
			{
				this.writer.Flush();
				this.writer.Close();
				this.writer.Dispose();
				this.writer = null;
			}
			if (this.stream != null)
			{
				this.stream.Close();
				this.stream.Dispose();
				this.stream = null;
			}
		}

		// Token: 0x0400184E RID: 6222
		private FileStream stream;

		// Token: 0x0400184F RID: 6223
		private StreamWriter writer;
	}
}
