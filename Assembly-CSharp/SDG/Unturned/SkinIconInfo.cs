using System;

namespace SDG.Unturned
{
	// Token: 0x02000800 RID: 2048
	public struct SkinIconInfo
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x001A348F File Offset: 0x001A168F
		public SkinIconInfo(ushort newID, ESkinIconSize newSize)
		{
			this.id = newID;
			this.size = newSize;
		}

		// Token: 0x04002F43 RID: 12099
		public ushort id;

		// Token: 0x04002F44 RID: 12100
		public ESkinIconSize size;
	}
}
