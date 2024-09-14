using System;
using System.IO;

namespace SDG.Unturned
{
	// Token: 0x020003E1 RID: 993
	public class ConsoleInputRedirector
	{
		// Token: 0x06001DA0 RID: 7584 RVA: 0x0006C504 File Offset: 0x0006A704
		public void enable()
		{
			if (this.defaultInputReader != null)
			{
				return;
			}
			this.defaultInputReader = Console.In;
			this.standardInputStream = Console.OpenStandardInput();
			this.standardInputReader = new StreamReader(this.standardInputStream, Console.InputEncoding);
			Console.SetIn(this.standardInputReader);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0006C554 File Offset: 0x0006A754
		public void disable()
		{
			if (this.standardInputReader != null)
			{
				this.standardInputReader.Close();
				this.standardInputReader.Dispose();
				this.standardInputReader = null;
			}
			if (this.standardInputStream != null)
			{
				this.standardInputStream.Close();
				this.standardInputStream.Dispose();
				this.standardInputStream = null;
			}
			if (this.defaultInputReader != null)
			{
				Console.SetIn(this.defaultInputReader);
				this.defaultInputReader = null;
			}
		}

		// Token: 0x04000DE6 RID: 3558
		private Stream standardInputStream;

		// Token: 0x04000DE7 RID: 3559
		private StreamReader standardInputReader;

		// Token: 0x04000DE8 RID: 3560
		private TextReader defaultInputReader;
	}
}
