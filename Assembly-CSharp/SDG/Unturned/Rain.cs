using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000503 RID: 1283
	public class Rain : MonoBehaviour
	{
		// Token: 0x0400154E RID: 5454
		private int _Rain_Puddle_Map = -1;

		// Token: 0x0400154F RID: 5455
		private int _Rain_Ripple_Map = -1;

		// Token: 0x04001550 RID: 5456
		private int _Rain_Water_Level = -1;

		// Token: 0x04001551 RID: 5457
		private int _Rain_Intensity = -1;

		// Token: 0x04001552 RID: 5458
		private int _Rain_Min_Height = -1;

		// Token: 0x04001553 RID: 5459
		public Texture2D Puddle_Map;

		// Token: 0x04001554 RID: 5460
		public Texture2D Ripple_Map;

		// Token: 0x04001555 RID: 5461
		public float Water_Level;

		// Token: 0x04001556 RID: 5462
		public float Intensity;

		// Token: 0x04001557 RID: 5463
		private bool isRaining;

		// Token: 0x04001558 RID: 5464
		private bool needsIsRainingUpdate;
	}
}
