using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200016E RID: 366
	internal class GlazierSprite_IMGUI : GlazierElementBase_IMGUI, ISleekSprite, ISleekElement
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0002086B File Offset: 0x0001EA6B
		// (set) Token: 0x06000979 RID: 2425 RVA: 0x00020873 File Offset: 0x0001EA73
		public Sprite Sprite { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0002087C File Offset: 0x0001EA7C
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x00020884 File Offset: 0x0001EA84
		public SleekColor TintColor { get; set; } = 0;

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0002088D File Offset: 0x0001EA8D
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x00020895 File Offset: 0x0001EA95
		public ESleekSpriteType DrawMethod { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0002089E File Offset: 0x0001EA9E
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x000208A6 File Offset: 0x0001EAA6
		public bool IsRaycastTarget { get; set; } = true;

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x000208AF File Offset: 0x0001EAAF
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x000208B7 File Offset: 0x0001EAB7
		public Vector2Int TileRepeatHintForUITK { get; set; }

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000982 RID: 2434 RVA: 0x000208C0 File Offset: 0x0001EAC0
		// (remove) Token: 0x06000983 RID: 2435 RVA: 0x000208F8 File Offset: 0x0001EAF8
		public event Action OnClicked;

		// Token: 0x06000984 RID: 2436 RVA: 0x00020930 File Offset: 0x0001EB30
		public override void OnGUI()
		{
			if (this.Sprite != null)
			{
				switch (this.DrawMethod)
				{
				case 0:
					GlazierUtils_IMGUI.drawTile(this.drawRect, this.Sprite.texture, this.TintColor);
					break;
				case 1:
					if (this.style == null)
					{
						this.style = new GUIStyle();
						this.style.normal.background = this.Sprite.texture;
						this.style.border = new RectOffset(20, 20, 20, 20);
					}
					GlazierUtils_IMGUI.drawSliced(this.drawRect, this.Sprite.texture, this.TintColor, this.style);
					break;
				case 2:
					GlazierUtils_IMGUI.drawImageTexture(this.drawRect, this.Sprite.texture, this.TintColor);
					break;
				}
			}
			base.ChildrenOnGUI();
			if (this.OnClicked != null)
			{
				GUI.enabled = (this.IsRaycastTarget && Event.current.type != 7 && Event.current.type != 12);
				Color backgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = ColorEx.BlackZeroAlpha;
				bool flag = GUI.Button(this.drawRect, string.Empty);
				GUI.enabled = true;
				GUI.backgroundColor = backgroundColor;
				if (flag && Event.current.button == 0)
				{
					Action onClicked = this.OnClicked;
					if (onClicked == null)
					{
						return;
					}
					onClicked.Invoke();
				}
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00020AA8 File Offset: 0x0001ECA8
		public GlazierSprite_IMGUI(Sprite sprite)
		{
			this.Sprite = sprite;
		}

		// Token: 0x040003AE RID: 942
		private GUIStyle style;
	}
}
