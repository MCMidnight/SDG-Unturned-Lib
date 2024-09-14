using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200071B RID: 1819
	public class SleekItemIcon : SleekWrapper
	{
		/// <summary>
		/// Hide existing icon until refresh.
		/// Experimented with doing this for every refresh, but it looks bad in particular for hotbar.
		/// </summary>
		// Token: 0x06003C11 RID: 15377 RVA: 0x0011B43C File Offset: 0x0011963C
		public void Clear()
		{
			this.internalImage.Texture = null;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0011B44A File Offset: 0x0011964A
		public void Refresh(ushort id, byte quality, byte[] state)
		{
			ItemTool.getIcon(id, quality, state, new ItemIconReady(this.OnIconReady));
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0011B460 File Offset: 0x00119660
		public void Refresh(ushort id, byte quality, byte[] state, ItemAsset itemAsset)
		{
			ItemTool.getIcon(id, quality, state, itemAsset, new ItemIconReady(this.OnIconReady));
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0011B478 File Offset: 0x00119678
		public void Refresh(ushort id, byte quality, byte[] state, ItemAsset itemAsset, int widthOverride, int heightOverride)
		{
			ItemTool.getIcon(id, quality, state, itemAsset, widthOverride, heightOverride, new ItemIconReady(this.OnIconReady));
		}

		// Token: 0x17000B22 RID: 2850
		// (set) Token: 0x06003C15 RID: 15381 RVA: 0x0011B494 File Offset: 0x00119694
		public byte rot
		{
			set
			{
				this.internalImage.RotationAngle = (float)(value * 90);
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (set) Token: 0x06003C16 RID: 15382 RVA: 0x0011B4A6 File Offset: 0x001196A6
		public bool isAngled
		{
			set
			{
				this.internalImage.CanRotate = value;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x0011B4B4 File Offset: 0x001196B4
		// (set) Token: 0x06003C18 RID: 15384 RVA: 0x0011B4C1 File Offset: 0x001196C1
		public SleekColor color
		{
			get
			{
				return this.internalImage.TintColor;
			}
			set
			{
				this.internalImage.TintColor = value;
			}
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0011B4CF File Offset: 0x001196CF
		public override void OnDestroy()
		{
			this.internalImage = null;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x0011B4D8 File Offset: 0x001196D8
		public SleekItemIcon()
		{
			this.internalImage = Glazier.Get().CreateImage();
			this.internalImage.SizeScale_X = 1f;
			this.internalImage.SizeScale_Y = 1f;
			base.AddChild(this.internalImage);
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x0011B527 File Offset: 0x00119727
		private void OnIconReady(Texture2D texture)
		{
			if (this.internalImage != null)
			{
				this.internalImage.Texture = texture;
			}
		}

		// Token: 0x0400259C RID: 9628
		private ISleekImage internalImage;
	}
}
