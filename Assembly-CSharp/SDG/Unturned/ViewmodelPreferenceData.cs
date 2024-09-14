using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006EB RID: 1771
	public class ViewmodelPreferenceData
	{
		// Token: 0x06003B0C RID: 15116 RVA: 0x0011443C File Offset: 0x0011263C
		public void Clamp()
		{
			this.Field_Of_View_Aim = (this.Field_Of_View_Aim.IsFinite() ? Mathf.Clamp(this.Field_Of_View_Aim, 1f, 179f) : 60f);
			this.Field_Of_View_Hip = (this.Field_Of_View_Hip.IsFinite() ? Mathf.Clamp(this.Field_Of_View_Hip, 1f, 179f) : 60f);
			this.Offset_Horizontal = (this.Offset_Horizontal.IsFinite() ? Mathf.Clamp(this.Offset_Horizontal, -1f, 1f) : 0f);
			this.Offset_Vertical = (this.Offset_Vertical.IsFinite() ? Mathf.Clamp(this.Offset_Vertical, -1f, 1f) : 0f);
			this.Offset_Depth = (this.Offset_Depth.IsFinite() ? Mathf.Clamp(this.Offset_Depth, -0.5f, 0.5f) : 0f);
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x00114534 File Offset: 0x00112734
		public ViewmodelPreferenceData()
		{
			this.Field_Of_View_Aim = 60f;
			this.Field_Of_View_Hip = 60f;
			this.Offset_Horizontal = 0f;
			this.Offset_Vertical = 0f;
			this.Offset_Depth = 0f;
		}

		// Token: 0x040024DB RID: 9435
		public float Field_Of_View_Aim;

		// Token: 0x040024DC RID: 9436
		public float Field_Of_View_Hip;

		// Token: 0x040024DD RID: 9437
		public float Offset_Horizontal;

		// Token: 0x040024DE RID: 9438
		public float Offset_Vertical;

		// Token: 0x040024DF RID: 9439
		public float Offset_Depth;
	}
}
