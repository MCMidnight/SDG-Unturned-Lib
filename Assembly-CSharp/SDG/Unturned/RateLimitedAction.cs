using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	///             	Note: new official code should be using per-method rate limit attribute.
	/// This is kept for backwards compatibility with plugins however.
	///
	/// Timestamp for server-side rate limiting.
	/// </summary>
	// Token: 0x020005E3 RID: 1507
	public struct RateLimitedAction
	{
		/// <summary>
		/// Realtime since performedRealtime.
		/// </summary>
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x000D4BA2 File Offset: 0x000D2DA2
		public float realtimeSincePerformed
		{
			get
			{
				return Time.realtimeSinceStartup - this.performedRealtime;
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000D4BB0 File Offset: 0x000D2DB0
		public bool hasIntervalPassed(float interval)
		{
			return this.realtimeSincePerformed > interval;
		}

		/// <summary>
		/// if(myRateLimit.throttle(1.0))
		/// 	return; // less than 1s passed
		/// </summary>
		// Token: 0x0600303F RID: 12351 RVA: 0x000D4BBB File Offset: 0x000D2DBB
		public bool throttle(float interval)
		{
			if (this.hasIntervalPassed(interval))
			{
				this.performedRealtime = Time.realtimeSinceStartup;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Realtime this action was performed.
		/// </summary>
		// Token: 0x04001A4F RID: 6735
		public float performedRealtime;
	}
}
