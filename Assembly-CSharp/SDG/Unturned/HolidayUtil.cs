using System;

namespace SDG.Unturned
{
	// Token: 0x020007FD RID: 2045
	public static class HolidayUtil
	{
		// Token: 0x06004637 RID: 17975 RVA: 0x001A320B File Offset: 0x001A140B
		public static bool isHolidayActive(ENPCHoliday holiday)
		{
			return holiday == Provider.authorityHoliday;
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x001A3215 File Offset: 0x001A1415
		public static ENPCHoliday getActiveHoliday()
		{
			return Provider.authorityHoliday;
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x001A321C File Offset: 0x001A141C
		internal static bool BackendIsHolidayActive(ENPCHoliday holiday)
		{
			return holiday == HolidayUtil.BackendGetActiveHoliday();
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x001A3228 File Offset: 0x001A1428
		internal static ENPCHoliday BackendGetActiveHoliday()
		{
			if (HolidayUtil.holidayOverride != ENPCHoliday.NONE)
			{
				return HolidayUtil.holidayOverride;
			}
			if (!Provider.isBackendRealtimeAvailable)
			{
				UnturnedLog.warn("getActiveHoliday called before backend realtime was available");
				return ENPCHoliday.NONE;
			}
			DateTime backendRealtimeDate = Provider.backendRealtimeDate;
			for (int i = 1; i < 6; i++)
			{
				DateTimeRange dateTimeRange = HolidayUtil.scheduledHolidays[i];
				if (dateTimeRange != null && dateTimeRange.isWithinRange(backendRealtimeDate))
				{
					return (ENPCHoliday)i;
				}
			}
			return ENPCHoliday.NONE;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x001A327E File Offset: 0x001A147E
		private static void scheduleHoliday(ENPCHoliday holiday, DateTime start, DateTime end)
		{
			HolidayUtil.scheduledHolidays[(int)holiday] = new DateTimeRange(start, end);
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x001A3290 File Offset: 0x001A1490
		public static void scheduleHolidays(HolidayStatusData data)
		{
			DateTime utcNow = DateTime.UtcNow;
			HolidayUtil.scheduleHoliday(ENPCHoliday.CHRISTMAS, data.ChristmasStart, data.ChristmasEnd);
			HolidayUtil.scheduleHoliday(ENPCHoliday.HALLOWEEN, data.HalloweenStart, data.HalloweenEnd);
			HolidayUtil.scheduleHoliday(ENPCHoliday.VALENTINES, data.ValentinesStart, data.ValentinesEnd);
			HolidayUtil.scheduleHoliday(ENPCHoliday.PRIDE_MONTH, new DateTime(utcNow.Year, 6, 1, 0, 0, 0, 1), new DateTime(utcNow.Year, 6, 30, 0, 0, 0, 1));
			if (data.AprilFools_Start.Ticks > 0L && data.AprilFools_End.Ticks > 0L)
			{
				HolidayUtil.scheduleHoliday(ENPCHoliday.APRIL_FOOLS, data.AprilFools_Start, data.AprilFools_End);
			}
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x001A3334 File Offset: 0x001A1534
		static HolidayUtil()
		{
			HolidayUtil.scheduledHolidays = new DateTimeRange[6];
			HolidayUtil.holidayOverride = ENPCHoliday.NONE;
			if (HolidayUtil.clHolidayOverride.hasValue)
			{
				string value = HolidayUtil.clHolidayOverride.value;
				if (string.Equals(value, "Halloween", 5) || string.Equals(value, "HW", 5))
				{
					HolidayUtil.holidayOverride = ENPCHoliday.HALLOWEEN;
					return;
				}
				if (string.Equals(value, "Christmas", 5) || string.Equals(value, "XMAS", 5))
				{
					HolidayUtil.holidayOverride = ENPCHoliday.CHRISTMAS;
					return;
				}
				if (string.Equals(value, "AprilFools", 5))
				{
					HolidayUtil.holidayOverride = ENPCHoliday.APRIL_FOOLS;
					return;
				}
				if (string.Equals(value, "Valentines", 5))
				{
					HolidayUtil.holidayOverride = ENPCHoliday.VALENTINES;
					return;
				}
				if (string.Equals(value, "PrideMonth", 5))
				{
					HolidayUtil.holidayOverride = ENPCHoliday.PRIDE_MONTH;
					return;
				}
				UnturnedLog.warn("Unknown holiday \"{0}\" requested by command-line override", new object[]
				{
					value
				});
			}
		}

		// Token: 0x04002F3A RID: 12090
		private static DateTimeRange[] scheduledHolidays;

		// Token: 0x04002F3B RID: 12091
		private static CommandLineString clHolidayOverride = new CommandLineString("-Holiday");

		// Token: 0x04002F3C RID: 12092
		private static ENPCHoliday holidayOverride;
	}
}
