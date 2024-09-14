using System;

namespace SDG.Unturned
{
	// Token: 0x02000688 RID: 1672
	[AttributeUsage(64)]
	public class SteamCall : Attribute
	{
		// Token: 0x06003833 RID: 14387 RVA: 0x00109D5F File Offset: 0x00107F5F
		public SteamCall(ESteamCallValidation validation)
		{
			this.validation = validation;
		}

		// Token: 0x0400215F RID: 8543
		public ESteamCallValidation validation;

		/// <summary>
		/// Maximum number of calls per-second per-player.
		/// </summary>
		// Token: 0x04002160 RID: 8544
		public int ratelimitHz = -1;

		/// <summary>
		/// Minimum seconds between calls per-player.
		/// Initialized from ratelimitHz when gathering RPCs.
		/// </summary>
		// Token: 0x04002161 RID: 8545
		public float ratelimitSeconds = -1f;

		/// <summary>
		/// Index into per-connection rate limiting array.
		/// </summary>
		// Token: 0x04002162 RID: 8546
		public int rateLimitIndex = -1;

		/// <summary>
		/// Backwards compatibility for older invoke by name code e.g. plugins.
		/// </summary>
		// Token: 0x04002163 RID: 8547
		public string legacyName;

		// Token: 0x04002164 RID: 8548
		public ENetInvocationDeferMode deferMode;
	}
}
