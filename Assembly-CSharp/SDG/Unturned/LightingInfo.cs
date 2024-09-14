using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F8 RID: 1272
	public class LightingInfo
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x000A9099 File Offset: 0x000A7299
		public Color[] colors
		{
			get
			{
				return this._colors;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x000A90A1 File Offset: 0x000A72A1
		public float[] singles
		{
			get
			{
				return this._singles;
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000A90A9 File Offset: 0x000A72A9
		public LightingInfo(Color[] newColors, float[] newSingles)
		{
			this._colors = newColors;
			this._singles = newSingles;
		}

		// Token: 0x04001526 RID: 5414
		private Color[] _colors;

		// Token: 0x04001527 RID: 5415
		private float[] _singles;
	}
}
