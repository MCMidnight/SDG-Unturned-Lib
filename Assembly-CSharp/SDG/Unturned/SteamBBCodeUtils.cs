using System;

namespace SDG.Unturned
{
	// Token: 0x02000819 RID: 2073
	public static class SteamBBCodeUtils
	{
		/// <summary>
		/// In-game rich text does not support embedded YouTube videos, but they look great in the web browser,
		/// so we simply remove them from the in-game text.
		/// </summary>
		// Token: 0x060046E2 RID: 18146 RVA: 0x001A7B44 File Offset: 0x001A5D44
		public static void removeYouTubePreviews(ref string bbcode)
		{
			int num;
			for (int i = 0; i < bbcode.Length; i = num)
			{
				num = bbcode.IndexOf("[previewyoutube=", i);
				if (num < 0)
				{
					return;
				}
				int num2 = bbcode.IndexOf("[/previewyoutube]", num + "[previewyoutube=".Length);
				if (num2 < 0)
				{
					return;
				}
				bbcode = bbcode.Remove(num, num2 + "[/previewyoutube]".Length - num);
			}
		}

		/// <summary>
		/// Unfortunately in-game rich text does not have code formatting yet, so remove the tags while preserving text.
		/// </summary>
		// Token: 0x060046E3 RID: 18147 RVA: 0x001A7BAA File Offset: 0x001A5DAA
		public static void removeCodeFormatting(ref string bbcode)
		{
			bbcode = bbcode.Replace("[code]", string.Empty);
			bbcode = bbcode.Replace("[/code]", string.Empty);
		}
	}
}
