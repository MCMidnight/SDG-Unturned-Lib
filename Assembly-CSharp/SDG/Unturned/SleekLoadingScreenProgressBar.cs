using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000727 RID: 1831
	public class SleekLoadingScreenProgressBar : SleekWrapper
	{
		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003C63 RID: 15459 RVA: 0x0011C55C File Offset: 0x0011A75C
		// (set) Token: 0x06003C64 RID: 15460 RVA: 0x0011C564 File Offset: 0x0011A764
		public float ProgressPercentage
		{
			get
			{
				return this._progressPercentage;
			}
			set
			{
				this._progressPercentage = value;
				this.foregroundImage.PositionScale_X = value;
				this.foregroundImage.SizeScale_X = 1f - value;
				this.percentageLabel.Text = value.ToString("P");
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003C65 RID: 15461 RVA: 0x0011C5A2 File Offset: 0x0011A7A2
		// (set) Token: 0x06003C66 RID: 15462 RVA: 0x0011C5AF File Offset: 0x0011A7AF
		public string DescriptionText
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

		// Token: 0x06003C67 RID: 15463 RVA: 0x0011C5C0 File Offset: 0x0011A7C0
		public SleekLoadingScreenProgressBar()
		{
			this.backgroundImage = Glazier.Get().CreateImage();
			this.backgroundImage.SizeScale_X = 1f;
			this.backgroundImage.SizeScale_Y = 1f;
			this.backgroundImage.Texture = GlazierResources.PixelTexture;
			this.backgroundImage.TintColor = 2;
			base.AddChild(this.backgroundImage);
			this.foregroundImage = Glazier.Get().CreateImage();
			this.foregroundImage.SizeScale_X = 1f;
			this.foregroundImage.SizeScale_Y = 1f;
			this.foregroundImage.Texture = GlazierResources.PixelTexture;
			this.foregroundImage.TintColor = new Color(0f, 0f, 0f, 0.75f);
			this.backgroundImage.AddChild(this.foregroundImage);
			this.label = Glazier.Get().CreateLabel();
			this.label.PositionOffset_X = 10f;
			this.label.PositionOffset_Y = -15f;
			this.label.PositionScale_Y = 0.5f;
			this.label.SizeOffset_X = -20f;
			this.label.SizeOffset_Y = 30f;
			this.label.SizeScale_X = 1f;
			this.label.TextContrastContext = 2;
			base.AddChild(this.label);
			this.percentageLabel = Glazier.Get().CreateLabel();
			this.percentageLabel.PositionOffset_X = -100f;
			this.percentageLabel.PositionOffset_Y = -15f;
			this.percentageLabel.PositionScale_X = 1f;
			this.percentageLabel.PositionScale_Y = 0.5f;
			this.percentageLabel.SizeOffset_X = 100f;
			this.percentageLabel.SizeOffset_Y = 30f;
			this.percentageLabel.TextContrastContext = 2;
			base.AddChild(this.percentageLabel);
		}

		// Token: 0x040025B9 RID: 9657
		private float _progressPercentage;

		// Token: 0x040025BA RID: 9658
		private ISleekImage backgroundImage;

		// Token: 0x040025BB RID: 9659
		private ISleekImage foregroundImage;

		// Token: 0x040025BC RID: 9660
		private ISleekLabel label;

		// Token: 0x040025BD RID: 9661
		private ISleekLabel percentageLabel;
	}
}
