using System;

namespace SDG.Unturned
{
	// Token: 0x0200080D RID: 2061
	public class PlayerContinuousIntegrationReport
	{
		// Token: 0x06004681 RID: 18049 RVA: 0x001A4396 File Offset: 0x001A2596
		public PlayerContinuousIntegrationReport()
		{
			this.ExitCode = 0;
			this.ErrorMessage = null;
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x001A43AC File Offset: 0x001A25AC
		public PlayerContinuousIntegrationReport(string ErrorMessage)
		{
			this.ExitCode = 1;
			this.ErrorMessage = ErrorMessage;
		}

		/// <summary>
		/// Error code that the server exited with.
		/// 0 is succesful, anything else is an error.
		/// </summary>
		// Token: 0x04002F8D RID: 12173
		public int ExitCode;

		/// <summary>
		/// Empty if successful,
		/// otherwise an explanation of the first error encountered.
		/// </summary>
		// Token: 0x04002F8E RID: 12174
		public string ErrorMessage;
	}
}
