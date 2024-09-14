using System;

namespace SDG.Unturned
{
	// Token: 0x020006F0 RID: 1776
	public class HolidayStatusData
	{
		/// <summary>
		/// Inclusive start date of Halloween event.
		/// Halloween event begins as soon as UTC day matches.
		/// </summary>
		// Token: 0x040024EF RID: 9455
		public DateTime HalloweenStart;

		/// <summary>
		/// Inclusive end date of Halloween event.
		/// Halloween event ends as soon as UTC day no longer matches.
		/// </summary>
		// Token: 0x040024F0 RID: 9456
		public DateTime HalloweenEnd;

		/// <summary>
		/// Inclusive start date of Christmas event.
		/// Christmas event begins as soon as UTC day matches.
		/// </summary>
		// Token: 0x040024F1 RID: 9457
		public DateTime ChristmasStart;

		/// <summary>
		/// Inclusive end date of Christmas event.
		/// Christmas event ends as soon as UTC day no longer matches.
		/// </summary>
		// Token: 0x040024F2 RID: 9458
		public DateTime ChristmasEnd;

		/// <summary>
		/// Inclusive start date of April Fools event.
		/// April Fools event begins as soon as UTC day matches.
		/// </summary>
		// Token: 0x040024F3 RID: 9459
		public DateTime AprilFools_Start;

		/// <summary>
		/// Inclusive end date of April Fools event.
		/// April Fools event ends as soon as UTC day no longer matches.
		/// </summary>
		// Token: 0x040024F4 RID: 9460
		public DateTime AprilFools_End;

		/// <summary>
		/// Inclusive start date of Valentine's Day event.
		/// Valentine's event begins as soon as UTC day matches.
		/// </summary>
		// Token: 0x040024F5 RID: 9461
		public DateTime ValentinesStart;

		/// <summary>
		/// Inclusive end date of Valentine's Day event.
		/// Valentine's event ends as soon as UTC day no longer matches.
		/// </summary>
		// Token: 0x040024F6 RID: 9462
		public DateTime ValentinesEnd;
	}
}
