using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000701 RID: 1793
	public class SleekBoxIcon : SleekWrapper
	{
		// Token: 0x17000AFD RID: 2813
		// (set) Token: 0x06003B51 RID: 15185 RVA: 0x00116838 File Offset: 0x00114A38
		public Texture2D icon
		{
			set
			{
				this.iconImage.Texture = value;
				if (this.iconSize == 0 && this.iconImage.Texture != null)
				{
					this.iconImage.SizeOffset_X = (float)this.iconImage.Texture.width;
					this.iconImage.SizeOffset_Y = (float)this.iconImage.Texture.height;
					this.label.PositionOffset_X = this.iconImage.SizeOffset_X + this.iconImage.PositionOffset_X * 2f;
					this.label.SizeOffset_X = -this.label.PositionOffset_X - 5f;
				}
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003B52 RID: 15186 RVA: 0x001168EC File Offset: 0x00114AEC
		// (set) Token: 0x06003B53 RID: 15187 RVA: 0x001168F9 File Offset: 0x00114AF9
		public string text
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003B54 RID: 15188 RVA: 0x00116907 File Offset: 0x00114B07
		// (set) Token: 0x06003B55 RID: 15189 RVA: 0x00116914 File Offset: 0x00114B14
		public string tooltip
		{
			get
			{
				return this.box.TooltipText;
			}
			set
			{
				this.box.TooltipText = value;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x00116922 File Offset: 0x00114B22
		// (set) Token: 0x06003B57 RID: 15191 RVA: 0x0011692F File Offset: 0x00114B2F
		public ESleekFontSize fontSize
		{
			get
			{
				return this.label.FontSize;
			}
			set
			{
				this.label.FontSize = value;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003B58 RID: 15192 RVA: 0x0011693D File Offset: 0x00114B3D
		// (set) Token: 0x06003B59 RID: 15193 RVA: 0x0011694A File Offset: 0x00114B4A
		public SleekColor iconColor
		{
			get
			{
				return this.iconImage.TintColor;
			}
			set
			{
				this.iconImage.TintColor = value;
			}
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x00116958 File Offset: 0x00114B58
		public SleekBoxIcon(Texture2D newIcon, int newSize)
		{
			this.iconSize = newSize;
			this.box = Glazier.Get().CreateBox();
			this.box.SizeScale_X = 1f;
			this.box.SizeScale_Y = 1f;
			base.AddChild(this.box);
			this.iconImage = Glazier.Get().CreateImage();
			this.iconImage.PositionOffset_X = 5f;
			this.iconImage.PositionOffset_Y = 5f;
			this.iconImage.Texture = newIcon;
			base.AddChild(this.iconImage);
			if (this.iconSize == 0)
			{
				if (this.iconImage.Texture != null)
				{
					this.iconImage.SizeOffset_X = (float)this.iconImage.Texture.width;
					this.iconImage.SizeOffset_Y = (float)this.iconImage.Texture.height;
				}
			}
			else
			{
				this.iconImage.SizeOffset_X = (float)this.iconSize;
				this.iconImage.SizeOffset_Y = (float)this.iconSize;
			}
			this.label = Glazier.Get().CreateLabel();
			this.label.SizeScale_X = 1f;
			this.label.SizeScale_Y = 1f;
			this.label.PositionOffset_X = this.iconImage.SizeOffset_X + this.iconImage.PositionOffset_X * 2f;
			this.label.SizeOffset_X = -this.label.PositionOffset_X - 5f;
			base.AddChild(this.label);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x00116AF0 File Offset: 0x00114CF0
		public SleekBoxIcon(Texture2D newIcon) : this(newIcon, 0)
		{
		}

		// Token: 0x04002539 RID: 9529
		private ISleekBox box;

		// Token: 0x0400253A RID: 9530
		private ISleekImage iconImage;

		// Token: 0x0400253B RID: 9531
		private int iconSize;

		// Token: 0x0400253C RID: 9532
		private ISleekLabel label;
	}
}
