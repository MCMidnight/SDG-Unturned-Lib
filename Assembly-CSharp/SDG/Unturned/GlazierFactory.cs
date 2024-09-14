using System;

namespace SDG.Unturned
{
	// Token: 0x0200015E RID: 350
	public static class GlazierFactory
	{
		/// <summary>
		/// Create glazier implementation. Invoked early during startup.
		/// </summary>
		// Token: 0x060008C9 RID: 2249 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
		public static void Create()
		{
			throw new NotSupportedException("Glazier should not be used by dedicated server");
		}

		// Token: 0x0400035E RID: 862
		private static CommandLineString clImpl = new CommandLineString("-Glazier");
	}
}
