using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007FE RID: 2046
	public class ItemDefIconInfo
	{
		// Token: 0x0600463E RID: 17982 RVA: 0x001A3414 File Offset: 0x001A1614
		public void onSmallItemIconReady(Texture2D texture)
		{
			this.hasSmall = true;
			this.complete();
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x001A3424 File Offset: 0x001A1624
		public void onLargeItemIconReady(Texture2D texture)
		{
			byte[] bytes = ImageConversion.EncodeToPNG(texture);
			UnturnedLog.info(this.extraPath);
			ReadWrite.writeBytes(this.extraPath + ".png", false, false, bytes);
			this.hasLarge = true;
			this.complete();
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x001A3468 File Offset: 0x001A1668
		private void complete()
		{
			if (!this.hasSmall || !this.hasLarge)
			{
				return;
			}
			IconUtils.icons.Remove(this);
		}

		/// <summary>
		/// Icon saved for community members in Extras folder.
		/// </summary>
		// Token: 0x04002F3D RID: 12093
		public string extraPath;

		/// <summary>
		/// Has the small icon been captured yet?
		/// </summary>
		// Token: 0x04002F3E RID: 12094
		private bool hasSmall;

		/// <summary>
		/// Has the large icon been captured yet?
		/// </summary>
		// Token: 0x04002F3F RID: 12095
		private bool hasLarge;
	}
}
