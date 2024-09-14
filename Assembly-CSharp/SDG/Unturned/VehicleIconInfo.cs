using System;

namespace SDG.Unturned
{
	// Token: 0x0200076D RID: 1901
	public class VehicleIconInfo
	{
		// Token: 0x04002711 RID: 10001
		public ushort id;

		// Token: 0x04002712 RID: 10002
		public ushort skin;

		// Token: 0x04002713 RID: 10003
		public VehicleAsset vehicleAsset;

		// Token: 0x04002714 RID: 10004
		public SkinAsset skinAsset;

		// Token: 0x04002715 RID: 10005
		public int x;

		// Token: 0x04002716 RID: 10006
		public int y;

		// Token: 0x04002717 RID: 10007
		public bool readableOnCPU;

		// Token: 0x04002718 RID: 10008
		public VehicleIconReady callback;
	}
}
