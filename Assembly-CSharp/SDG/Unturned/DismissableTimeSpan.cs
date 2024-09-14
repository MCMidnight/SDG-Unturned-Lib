using System;

namespace SDG.Unturned
{
	// Token: 0x020005B3 RID: 1459
	public class DismissableTimeSpan
	{
		// Token: 0x06002F84 RID: 12164 RVA: 0x000D2167 File Offset: 0x000D0367
		public DismissableTimeSpan(DateTime start, DateTime end, string key)
		{
			this.range = new DateTimeRange(start, end);
			this.key = key;
		}

		/// <summary>
		/// Is current UTC time within this time span, and player has not dismissed?
		/// </summary>
		// Token: 0x06002F85 RID: 12165 RVA: 0x000D2183 File Offset: 0x000D0383
		public bool isRelevant()
		{
			return this.isNowWithinSpan() && !this.hasDismissedSpan();
		}

		/// <summary>
		/// Has the current time span been dismissed?
		/// For example, player may have dismissed a previous event but not this current one.
		/// </summary>
		// Token: 0x06002F86 RID: 12166 RVA: 0x000D219C File Offset: 0x000D039C
		public bool hasDismissedSpan()
		{
			DateTime dateTime;
			return this.getDismissedTime(out dateTime) && dateTime >= this.range.start;
		}

		/// <summary>
		/// Is current UTC time within this time span?
		/// </summary>
		// Token: 0x06002F87 RID: 12167 RVA: 0x000D21C6 File Offset: 0x000D03C6
		public bool isNowWithinSpan()
		{
			return this.range.isNowWithinRange();
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000D21D3 File Offset: 0x000D03D3
		public bool getDismissedTime(out DateTime dismissedTime)
		{
			return ConvenientSavedata.get().read(this.key, out dismissedTime);
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000D21E8 File Offset: 0x000D03E8
		public void dismiss()
		{
			DateTime utcNow = DateTime.UtcNow;
			ConvenientSavedata.get().write(this.key, utcNow);
		}

		// Token: 0x040019A9 RID: 6569
		private DateTimeRange range;

		// Token: 0x040019AA RID: 6570
		private string key;
	}
}
