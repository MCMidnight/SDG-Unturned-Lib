using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003FF RID: 1023
	public class EditorDrag
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x0006DE44 File Offset: 0x0006C044
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006DE4C File Offset: 0x0006C04C
		public Vector3 screen
		{
			get
			{
				return this._screen;
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0006DE54 File Offset: 0x0006C054
		public EditorDrag(Transform newTransform, Vector3 newScreen)
		{
			this._transform = newTransform;
			this._screen = newScreen;
		}

		// Token: 0x04000E7A RID: 3706
		private Transform _transform;

		// Token: 0x04000E7B RID: 3707
		private Vector3 _screen;
	}
}
