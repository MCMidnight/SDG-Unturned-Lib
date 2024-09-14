using System;

namespace SDG.Unturned
{
	// Token: 0x020005B2 RID: 1458
	public class DateTimeRange
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x000D2080 File Offset: 0x000D0280
		public DateTimeRange(DateTime start, DateTime end)
		{
			this.start = start;
			this.end = end;
			if (start.Kind != 1)
			{
				throw new ArgumentException("DateTimeRange kind should be UTC", "start");
			}
			if (end.Kind != 1)
			{
				throw new ArgumentException("DateTimeRange kind should be UTC", "end");
			}
			if (start > end)
			{
				throw new ArgumentException("DateTimeRange start and end are out of order");
			}
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000D20E9 File Offset: 0x000D02E9
		public bool isWithinRange(DateTime dateTime)
		{
			return dateTime >= this.start && dateTime <= this.end;
		}

		/// <summary>
		/// Is client UTC time within this time range?
		/// </summary>
		// Token: 0x06002F82 RID: 12162 RVA: 0x000D2108 File Offset: 0x000D0308
		public bool isNowWithinRange()
		{
			DateTime utcNow = DateTime.UtcNow;
			return utcNow >= this.start && utcNow <= this.end;
		}

		/// <summary>
		/// Is server UTC time within this time range?
		/// </summary>
		// Token: 0x06002F83 RID: 12163 RVA: 0x000D2138 File Offset: 0x000D0338
		public bool isBackendNowWithinRange()
		{
			DateTime backendRealtimeDate = Provider.backendRealtimeDate;
			return backendRealtimeDate >= this.start && backendRealtimeDate <= this.end;
		}

		// Token: 0x040019A7 RID: 6567
		public DateTime start;

		// Token: 0x040019A8 RID: 6568
		public DateTime end;
	}
}
