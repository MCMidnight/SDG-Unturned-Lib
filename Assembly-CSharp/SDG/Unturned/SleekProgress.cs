using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200072A RID: 1834
	public class SleekProgress : SleekWrapper
	{
		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x0011D925 File Offset: 0x0011BB25
		// (set) Token: 0x06003C79 RID: 15481 RVA: 0x0011D930 File Offset: 0x0011BB30
		public float state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = Mathf.Clamp01(value);
				this.foreground.SizeScale_X = this.state;
				if (this.suffix.Length == 0)
				{
					this.label.Text = Mathf.RoundToInt(this.foreground.SizeScale_X * 100f).ToString() + "%";
				}
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (set) Token: 0x06003C7A RID: 15482 RVA: 0x0011D99A File Offset: 0x0011BB9A
		public int measure
		{
			set
			{
				if (this.suffix.Length != 0)
				{
					this.label.Text = value.ToString() + this.suffix;
				}
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003C7B RID: 15483 RVA: 0x0011D9C6 File Offset: 0x0011BBC6
		// (set) Token: 0x06003C7C RID: 15484 RVA: 0x0011D9D8 File Offset: 0x0011BBD8
		public Color color
		{
			get
			{
				return this.foreground.TintColor;
			}
			set
			{
				Color color = value;
				color.a = 0.5f;
				this.background.TintColor = color;
				this.foreground.TintColor = value;
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0011DA18 File Offset: 0x0011BC18
		public SleekProgress(string newSuffix)
		{
			this.background = Glazier.Get().CreateImage();
			this.background.SizeScale_X = 1f;
			this.background.SizeScale_Y = 1f;
			this.background.Texture = GlazierResources.PixelTexture;
			base.AddChild(this.background);
			this.foreground = Glazier.Get().CreateImage();
			this.foreground.SizeScale_X = 1f;
			this.foreground.SizeScale_Y = 1f;
			this.foreground.Texture = GlazierResources.PixelTexture;
			base.AddChild(this.foreground);
			this.label = Glazier.Get().CreateLabel();
			this.label.SizeScale_X = 1f;
			this.label.PositionScale_Y = 0.5f;
			this.label.PositionOffset_Y = -15f;
			this.label.SizeOffset_Y = 30f;
			this.label.TextColor = Color.white;
			this.label.TextContrastContext = 2;
			base.AddChild(this.label);
			this.suffix = newSuffix;
		}

		// Token: 0x040025CB RID: 9675
		private ISleekImage background;

		// Token: 0x040025CC RID: 9676
		private ISleekImage foreground;

		// Token: 0x040025CD RID: 9677
		private ISleekLabel label;

		// Token: 0x040025CE RID: 9678
		public string suffix;

		// Token: 0x040025CF RID: 9679
		private float _state;
	}
}
