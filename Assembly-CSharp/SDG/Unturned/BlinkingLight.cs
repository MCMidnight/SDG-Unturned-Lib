using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004AE RID: 1198
	public class BlinkingLight : MonoBehaviour
	{
		// Token: 0x06002514 RID: 9492 RVA: 0x00093BCA File Offset: 0x00091DCA
		private void Update()
		{
			if (Time.time - this.blinkTime < 1f)
			{
				return;
			}
			this.blinkTime = Time.time;
			this.target.SetActive(!this.target.activeSelf);
		}

		// Token: 0x040012D2 RID: 4818
		public GameObject target;

		// Token: 0x040012D3 RID: 4819
		private float blinkTime;
	}
}
