using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Extensions to the built-in Screen class.
	/// We have run into multiple problems with the Screen.resolutions property over the years, so this class aims to
	/// protect against bad data.
	/// </summary>
	// Token: 0x02000818 RID: 2072
	public static class ScreenEx
	{
		// Token: 0x060046DC RID: 18140 RVA: 0x001A79D3 File Offset: 0x001A5BD3
		public static int GetWidthForLayout()
		{
			return Mathf.RoundToInt((float)Screen.width / GraphicsSettings.userInterfaceScale);
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x001A79E8 File Offset: 0x001A5BE8
		public static float GetCurrentAspectRatio()
		{
			Resolution currentResolution = Screen.currentResolution;
			if (currentResolution.height > 0)
			{
				return (float)currentResolution.width / (float)currentResolution.height;
			}
			return 1f;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x001A7A1C File Offset: 0x001A5C1C
		public static Resolution[] GetRecommendedResolutions()
		{
			if (ScreenEx.cachedResolutions == null)
			{
				ScreenEx.CacheResolutions();
			}
			return ScreenEx.cachedResolutions;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x001A7A30 File Offset: 0x001A5C30
		public static Resolution GetHighestRecommendedResolution()
		{
			Resolution[] recommendedResolutions = ScreenEx.GetRecommendedResolutions();
			if (recommendedResolutions.Length != 0)
			{
				return recommendedResolutions[recommendedResolutions.Length - 1];
			}
			return Screen.currentResolution;
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x001A7A58 File Offset: 0x001A5C58
		private static void CacheResolutions()
		{
			List<Resolution> list = new List<Resolution>();
			if (ScreenEx.clNoUnityResolutions)
			{
				list.Add(Screen.currentResolution);
			}
			else
			{
				Resolution[] resolutions = Screen.resolutions;
				int i;
				if (resolutions.Length > 200)
				{
					i = resolutions.Length - 200;
					UnturnedLog.warn("Unity returned {0} recommended resolutions, clamping to {1}", new object[]
					{
						resolutions.Length,
						200
					});
				}
				else
				{
					i = 0;
				}
				while (i < resolutions.Length)
				{
					Resolution resolution = resolutions[i];
					if (resolution.width >= 640 && resolution.height >= 480)
					{
						list.Add(resolution);
					}
					i++;
				}
			}
			list.Sort(delegate(Resolution lhs, Resolution rhs)
			{
				int num = lhs.width.CompareTo(rhs.width);
				if (num != 0)
				{
					return num;
				}
				int num2 = lhs.height.CompareTo(rhs.height);
				if (num2 == 0)
				{
					return lhs.refreshRate.CompareTo(rhs.refreshRate);
				}
				return num2;
			});
			ScreenEx.cachedResolutions = list.ToArray();
		}

		// Token: 0x04003043 RID: 12355
		private static Resolution[] cachedResolutions;

		// Token: 0x04003044 RID: 12356
		private static CommandLineFlag clNoUnityResolutions = new CommandLineFlag(false, "-NoUnityResolutions");
	}
}
