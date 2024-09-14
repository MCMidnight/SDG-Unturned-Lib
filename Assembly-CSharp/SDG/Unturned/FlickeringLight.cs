using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004C6 RID: 1222
	public class FlickeringLight : MonoBehaviour
	{
		// Token: 0x06002573 RID: 9587 RVA: 0x00094FD4 File Offset: 0x000931D4
		private void Update()
		{
			float num = Random.Range(0.9f, 1.4f);
			if (Time.time - this.blackoutTime < 0.15f)
			{
				num = 0.15f;
			}
			else if (Time.time - this.blackoutTime > this.blackoutDelay)
			{
				this.blackoutTime = Time.time;
				this.blackoutDelay = Random.Range(7.3f, 13.2f);
			}
			if (this.target != null)
			{
				this.target.intensity = num;
			}
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", new Color(num, num, num));
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00095080 File Offset: 0x00093280
		private void Awake()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			this.blackoutTime = Time.time;
			this.blackoutDelay = Random.Range(0f, 13.2f);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000950B3 File Offset: 0x000932B3
		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x04001338 RID: 4920
		public Light target;

		// Token: 0x04001339 RID: 4921
		private Material material;

		// Token: 0x0400133A RID: 4922
		private float blackoutTime;

		// Token: 0x0400133B RID: 4923
		private float blackoutDelay;
	}
}
