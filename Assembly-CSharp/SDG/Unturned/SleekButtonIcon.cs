using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000705 RID: 1797
	public class SleekButtonIcon : SleekWrapper
	{
		// Token: 0x17000B07 RID: 2823
		// (set) Token: 0x06003B74 RID: 15220 RVA: 0x00116DA4 File Offset: 0x00114FA4
		public Texture2D icon
		{
			set
			{
				this.iconImage.Texture = value;
				if (this.iconSize == 0 && !this.iconScale && this.iconImage.Texture != null)
				{
					this.iconImage.SizeOffset_X = (float)this.iconImage.Texture.width;
					this.iconImage.SizeOffset_Y = (float)this.iconImage.Texture.height;
					if (this.label != null)
					{
						this.label.PositionOffset_X = this.iconImage.SizeOffset_X + this.iconImage.PositionOffset_X * 2f;
						this.label.SizeOffset_X = -this.label.PositionOffset_X - 5f;
					}
				}
			}
		}

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06003B75 RID: 15221 RVA: 0x00116E70 File Offset: 0x00115070
		// (remove) Token: 0x06003B76 RID: 15222 RVA: 0x00116EA8 File Offset: 0x001150A8
		public event ClickedButton onClickedButton;

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06003B77 RID: 15223 RVA: 0x00116EE0 File Offset: 0x001150E0
		// (remove) Token: 0x06003B78 RID: 15224 RVA: 0x00116F18 File Offset: 0x00115118
		public event ClickedButton onRightClickedButton;

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003B79 RID: 15225 RVA: 0x00116F4D File Offset: 0x0011514D
		// (set) Token: 0x06003B7A RID: 15226 RVA: 0x00116F6E File Offset: 0x0011516E
		public string text
		{
			get
			{
				if (this.label == null)
				{
					return this.button.Text;
				}
				return this.label.Text;
			}
			set
			{
				if (this.label != null)
				{
					this.label.Text = value;
					return;
				}
				this.button.Text = value;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003B7B RID: 15227 RVA: 0x00116F91 File Offset: 0x00115191
		// (set) Token: 0x06003B7C RID: 15228 RVA: 0x00116F9E File Offset: 0x0011519E
		public string tooltip
		{
			get
			{
				return this.button.TooltipText;
			}
			set
			{
				this.button.TooltipText = value;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003B7D RID: 15229 RVA: 0x00116FAC File Offset: 0x001151AC
		// (set) Token: 0x06003B7E RID: 15230 RVA: 0x00116FB9 File Offset: 0x001151B9
		public ESleekFontSize fontSize
		{
			get
			{
				return this.button.FontSize;
			}
			set
			{
				this.button.FontSize = value;
				if (this.label != null)
				{
					this.label.FontSize = value;
				}
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003B7F RID: 15231 RVA: 0x00116FDB File Offset: 0x001151DB
		// (set) Token: 0x06003B80 RID: 15232 RVA: 0x00116FE8 File Offset: 0x001151E8
		public ETextContrastContext shadowStyle
		{
			get
			{
				return this.button.TextContrastContext;
			}
			set
			{
				this.button.TextContrastContext = value;
				if (this.label != null)
				{
					this.label.TextContrastContext = value;
				}
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06003B81 RID: 15233 RVA: 0x0011700A File Offset: 0x0011520A
		// (set) Token: 0x06003B82 RID: 15234 RVA: 0x00117017 File Offset: 0x00115217
		public SleekColor backgroundColor
		{
			get
			{
				return this.button.BackgroundColor;
			}
			set
			{
				this.button.BackgroundColor = value;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x00117025 File Offset: 0x00115225
		// (set) Token: 0x06003B84 RID: 15236 RVA: 0x00117032 File Offset: 0x00115232
		public SleekColor textColor
		{
			get
			{
				return this.button.TextColor;
			}
			set
			{
				this.button.TextColor = value;
				if (this.label != null)
				{
					this.label.TextColor = value;
				}
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x00117054 File Offset: 0x00115254
		// (set) Token: 0x06003B86 RID: 15238 RVA: 0x00117061 File Offset: 0x00115261
		public bool enableRichText
		{
			get
			{
				return this.button.AllowRichText;
			}
			set
			{
				this.button.AllowRichText = value;
				if (this.label != null)
				{
					this.label.AllowRichText = value;
				}
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x00117084 File Offset: 0x00115284
		public int iconPositionOffset
		{
			set
			{
				this.iconImage.PositionOffset_X = (float)value;
				this.iconImage.PositionOffset_Y = (float)value;
				if (this.label != null)
				{
					this.label.PositionOffset_X = this.iconImage.SizeOffset_X + this.iconImage.PositionOffset_X * 2f;
					this.label.SizeOffset_X = -this.label.PositionOffset_X - 5f;
				}
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x001170F8 File Offset: 0x001152F8
		// (set) Token: 0x06003B89 RID: 15241 RVA: 0x00117105 File Offset: 0x00115305
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

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003B8A RID: 15242 RVA: 0x00117113 File Offset: 0x00115313
		// (set) Token: 0x06003B8B RID: 15243 RVA: 0x00117120 File Offset: 0x00115320
		public bool isClickable
		{
			get
			{
				return this.button.IsClickable;
			}
			set
			{
				this.button.IsClickable = value;
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x00117130 File Offset: 0x00115330
		public SleekButtonIcon(Texture2D newIcon, int newSize, bool newScale)
		{
			this.iconSize = newSize;
			this.iconScale = newScale;
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.BackgroundColor = 1;
			this.button.OnClicked += new ClickedButton(this.onClickedInternalButton);
			this.button.OnRightClicked += new ClickedButton(this.onRightClickedInternalButton);
			base.AddChild(this.button);
			this.iconImage = Glazier.Get().CreateImage();
			this.iconImage.Texture = newIcon;
			this.iconPositionOffset = 5;
			if (this.iconScale)
			{
				this.iconImage.SizeOffset_X = -10f;
				this.iconImage.SizeOffset_Y = -10f;
				this.iconImage.SizeScale_X = 1f;
				this.iconImage.SizeScale_Y = 1f;
			}
			else
			{
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
			base.AddChild(this.iconImage);
			this.button.TextAlignment = 4;
			this.button.FontSize = 2;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0011735A File Offset: 0x0011555A
		public SleekButtonIcon(Texture2D newIcon, int newSize) : this(newIcon, newSize, false)
		{
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x00117365 File Offset: 0x00115565
		public SleekButtonIcon(Texture2D newIcon) : this(newIcon, 0, false)
		{
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00117370 File Offset: 0x00115570
		public SleekButtonIcon(Texture2D newIcon, bool newScale) : this(newIcon, 0, newScale)
		{
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0011737B File Offset: 0x0011557B
		private void onClickedInternalButton(ISleekElement internalButton)
		{
			ClickedButton clickedButton = this.onClickedButton;
			if (clickedButton == null)
			{
				return;
			}
			clickedButton.Invoke(this);
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0011738E File Offset: 0x0011558E
		private void onRightClickedInternalButton(ISleekElement internalButton)
		{
			ClickedButton clickedButton = this.onRightClickedButton;
			if (clickedButton == null)
			{
				return;
			}
			clickedButton.Invoke(this);
		}

		// Token: 0x04002544 RID: 9540
		private ISleekButton button;

		// Token: 0x04002545 RID: 9541
		private int iconSize;

		// Token: 0x04002546 RID: 9542
		private bool iconScale;

		// Token: 0x04002547 RID: 9543
		private ISleekImage iconImage;

		// Token: 0x04002548 RID: 9544
		private ISleekLabel label;
	}
}
