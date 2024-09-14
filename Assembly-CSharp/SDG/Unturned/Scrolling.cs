using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004A2 RID: 1186
	public class Scrolling : MonoBehaviour
	{
		// Token: 0x060024DF RID: 9439 RVA: 0x00093070 File Offset: 0x00091270
		private void Update()
		{
			this.material.mainTextureOffset = new Vector2(this.x * Time.time, this.y * Time.time);
		}

		// Token: 0x040012BE RID: 4798
		public Material material;

		// Token: 0x040012BF RID: 4799
		public float x;

		// Token: 0x040012C0 RID: 4800
		public float y;
	}
}
