using System;
using System.IO;

namespace SDG.Unturned
{
	// Token: 0x020003E2 RID: 994
	public class ConsoleOutputRedirector
	{
		// Token: 0x06001DA3 RID: 7587 RVA: 0x0006C5D0 File Offset: 0x0006A7D0
		public void enable(bool shouldProxy)
		{
			if (this.defaultOutputWriter != null)
			{
				return;
			}
			this.defaultOutputWriter = Console.Out;
			this.standardOutputStream = Console.OpenStandardOutput();
			this.standardOutputWriter = new StreamWriter(this.standardOutputStream, Console.OutputEncoding);
			this.standardOutputWriter.AutoFlush = true;
			if (shouldProxy)
			{
				this.proxyWriter = new ConsoleWriterProxy(this.standardOutputWriter, this.defaultOutputWriter);
				Console.SetOut(this.proxyWriter);
				return;
			}
			Console.SetOut(this.standardOutputWriter);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0006C650 File Offset: 0x0006A850
		public void disable()
		{
			if (this.proxyWriter != null)
			{
				this.proxyWriter.Close();
				this.proxyWriter.Dispose();
				this.proxyWriter = null;
			}
			if (this.standardOutputWriter != null)
			{
				this.standardOutputWriter.Close();
				this.standardOutputWriter.Dispose();
				this.standardOutputWriter = null;
			}
			if (this.standardOutputStream != null)
			{
				this.standardOutputStream.Close();
				this.standardOutputStream.Dispose();
				this.standardOutputStream = null;
			}
			if (this.defaultOutputWriter != null)
			{
				Console.SetOut(this.defaultOutputWriter);
				this.defaultOutputWriter = null;
			}
		}

		// Token: 0x04000DE9 RID: 3561
		private Stream standardOutputStream;

		// Token: 0x04000DEA RID: 3562
		private StreamWriter standardOutputWriter;

		// Token: 0x04000DEB RID: 3563
		private ConsoleWriterProxy proxyWriter;

		// Token: 0x04000DEC RID: 3564
		private TextWriter defaultOutputWriter;
	}
}
