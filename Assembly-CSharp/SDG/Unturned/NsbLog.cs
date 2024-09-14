using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	/// <summary>
	/// Logs enabled when WITH_NSB_LOGGING is defined.
	/// Tracking down an issue where snapshot buffer stops working for groups of networked objects.
	/// </summary>
	// Token: 0x020005E2 RID: 1506
	public static class NsbLog
	{
		// Token: 0x06003039 RID: 12345 RVA: 0x000D4B9A File Offset: 0x000D2D9A
		[Conditional("WITH_NSB_LOGGING")]
		public static void Warning(object message)
		{
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000D4B9C File Offset: 0x000D2D9C
		[Conditional("WITH_NSB_LOGGING")]
		public static void ConditionalWarning(bool condition, object message)
		{
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000D4B9E File Offset: 0x000D2D9E
		[Conditional("WITH_NSB_LOGGING")]
		public static void WarningFormat(string format, params object[] args)
		{
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000D4BA0 File Offset: 0x000D2DA0
		[Conditional("WITH_NSB_LOGGING")]
		public static void ConditionalWarningFormat(bool condition, string format, params object[] args)
		{
		}
	}
}
