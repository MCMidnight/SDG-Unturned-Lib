using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000166 RID: 358
	internal class GlazierImage_IMGUI : GlazierElementBase_IMGUI, ISleekImage, ISleekElement
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
		public Texture Texture { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0001F9E1 File Offset: 0x0001DBE1
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x0001F9E9 File Offset: 0x0001DBE9
		public float RotationAngle { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0001F9F2 File Offset: 0x0001DBF2
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x0001F9FA File Offset: 0x0001DBFA
		public bool CanRotate { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0001FA03 File Offset: 0x0001DC03
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x0001FA0B File Offset: 0x0001DC0B
		public bool ShouldDestroyTexture { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001FA14 File Offset: 0x0001DC14
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		public SleekColor TintColor { get; set; } = 0;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000912 RID: 2322 RVA: 0x0001FA28 File Offset: 0x0001DC28
		// (remove) Token: 0x06000913 RID: 2323 RVA: 0x0001FA60 File Offset: 0x0001DC60
		public event Action OnClicked;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000914 RID: 2324 RVA: 0x0001FA98 File Offset: 0x0001DC98
		// (remove) Token: 0x06000915 RID: 2325 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
		public event Action OnRightClicked;

		// Token: 0x06000916 RID: 2326 RVA: 0x0001FB05 File Offset: 0x0001DD05
		public void UpdateTexture(Texture2D newTexture)
		{
			this.Texture = newTexture;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001FB0E File Offset: 0x0001DD0E
		public void SetTextureAndShouldDestroy(Texture2D texture, bool shouldDestroyTexture)
		{
			if (this.Texture != null && this.ShouldDestroyTexture)
			{
				Object.Destroy(this.Texture);
			}
			this.Texture = texture;
			this.ShouldDestroyTexture = shouldDestroyTexture;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001FB3F File Offset: 0x0001DD3F
		public override void InternalDestroy()
		{
			if (this.ShouldDestroyTexture && this.Texture != null)
			{
				Object.DestroyImmediate(this.Texture);
				this.Texture = null;
			}
			base.InternalDestroy();
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001FB70 File Offset: 0x0001DD70
		public override void OnGUI()
		{
			if (this.CanRotate)
			{
				GlazierUtils_IMGUI.drawAngledImageTexture(this.drawRect, this.Texture, this.RotationAngle, this.TintColor);
			}
			else
			{
				GlazierUtils_IMGUI.drawImageTexture(this.drawRect, this.Texture, this.TintColor);
			}
			base.ChildrenOnGUI();
			if (this.OnClicked != null || this.OnRightClicked != null)
			{
				GUI.enabled = (Event.current.type != 7 && Event.current.type != 12);
				Color backgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = ColorEx.BlackZeroAlpha;
				bool flag = GUI.Button(this.drawRect, string.Empty);
				GUI.enabled = true;
				GUI.backgroundColor = backgroundColor;
				if (flag)
				{
					if (Event.current.button == 0)
					{
						Action onClicked = this.OnClicked;
						if (onClicked == null)
						{
							return;
						}
						onClicked.Invoke();
						return;
					}
					else if (Event.current.button == 1)
					{
						Action onRightClicked = this.OnRightClicked;
						if (onRightClicked == null)
						{
							return;
						}
						onRightClicked.Invoke();
					}
				}
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001FC6C File Offset: 0x0001DE6C
		public GlazierImage_IMGUI(Texture texture)
		{
			this.Texture = texture;
		}
	}
}
