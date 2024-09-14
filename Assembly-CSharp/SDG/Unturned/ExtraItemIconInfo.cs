using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000801 RID: 2049
	public class ExtraItemIconInfo
	{
		// Token: 0x06004643 RID: 17987 RVA: 0x001A34A0 File Offset: 0x001A16A0
		public void onItemIconReady(Texture2D texture)
		{
			byte[] bytes = ImageConversion.EncodeToPNG(texture);
			ReadWrite.writeBytes(this.extraPath + ".png", false, false, bytes);
			Object.Destroy(texture);
			IconUtils.extraIcons.Remove(this);
		}

		// Token: 0x04002F45 RID: 12101
		public string extraPath;
	}
}
