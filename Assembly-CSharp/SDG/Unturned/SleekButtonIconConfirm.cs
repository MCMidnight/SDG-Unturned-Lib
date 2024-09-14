using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000704 RID: 1796
	public class SleekButtonIconConfirm : SleekWrapper
	{
		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003B64 RID: 15204 RVA: 0x00116AFA File Offset: 0x00114CFA
		// (set) Token: 0x06003B65 RID: 15205 RVA: 0x00116B07 File Offset: 0x00114D07
		public string text
		{
			get
			{
				return this.mainButton.text;
			}
			set
			{
				this.mainButton.text = value;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003B66 RID: 15206 RVA: 0x00116B15 File Offset: 0x00114D15
		// (set) Token: 0x06003B67 RID: 15207 RVA: 0x00116B22 File Offset: 0x00114D22
		public string tooltip
		{
			get
			{
				return this.mainButton.tooltip;
			}
			set
			{
				this.mainButton.tooltip = value;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003B68 RID: 15208 RVA: 0x00116B30 File Offset: 0x00114D30
		// (set) Token: 0x06003B69 RID: 15209 RVA: 0x00116B3D File Offset: 0x00114D3D
		public ESleekFontSize fontSize
		{
			get
			{
				return this.mainButton.fontSize;
			}
			set
			{
				this.mainButton.fontSize = value;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003B6A RID: 15210 RVA: 0x00116B4B File Offset: 0x00114D4B
		// (set) Token: 0x06003B6B RID: 15211 RVA: 0x00116B58 File Offset: 0x00114D58
		public SleekColor iconColor
		{
			get
			{
				return this.mainButton.iconColor;
			}
			set
			{
				this.mainButton.iconColor = value;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003B6C RID: 15212 RVA: 0x00116B66 File Offset: 0x00114D66
		// (set) Token: 0x06003B6D RID: 15213 RVA: 0x00116B73 File Offset: 0x00114D73
		public bool isClickable
		{
			get
			{
				return this.mainButton.isClickable;
			}
			set
			{
				this.mainButton.isClickable = value;
			}
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x00116B81 File Offset: 0x00114D81
		public void reset()
		{
			this.mainButton.IsVisible = true;
			this.confirmButton.IsVisible = false;
			this.denyButton.IsVisible = false;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00116BA7 File Offset: 0x00114DA7
		private void onClickedConfirmButton(ISleekElement button)
		{
			this.reset();
			Confirm confirm = this.onConfirmed;
			if (confirm == null)
			{
				return;
			}
			confirm(this);
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00116BC0 File Offset: 0x00114DC0
		private void onClickedDenyButton(ISleekElement button)
		{
			this.reset();
			Deny deny = this.onDenied;
			if (deny == null)
			{
				return;
			}
			deny(this);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00116BD9 File Offset: 0x00114DD9
		private void onClickedMainButton(ISleekElement button)
		{
			this.mainButton.IsVisible = false;
			this.confirmButton.IsVisible = true;
			this.denyButton.IsVisible = true;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x00116BFF File Offset: 0x00114DFF
		public SleekButtonIconConfirm(Texture2D newIcon, string newConfirm, string newConfirmTooltip, string newDeny, string newDenyTooltip) : this(newIcon, newConfirm, newConfirmTooltip, newDeny, newDenyTooltip, 0)
		{
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x00116C10 File Offset: 0x00114E10
		public SleekButtonIconConfirm(Texture2D newIcon, string newConfirm, string newConfirmTooltip, string newDeny, string newDenyTooltip, int iconSize)
		{
			this.mainButton = new SleekButtonIcon(newIcon, iconSize);
			this.mainButton.SizeScale_X = 1f;
			this.mainButton.SizeScale_Y = 1f;
			this.mainButton.onClickedButton += new ClickedButton(this.onClickedMainButton);
			base.AddChild(this.mainButton);
			this.confirmButton = Glazier.Get().CreateButton();
			this.confirmButton.SizeOffset_X = -5f;
			this.confirmButton.SizeScale_X = 0.5f;
			this.confirmButton.SizeScale_Y = 1f;
			this.confirmButton.Text = newConfirm;
			this.confirmButton.TooltipText = newConfirmTooltip;
			this.confirmButton.OnClicked += new ClickedButton(this.onClickedConfirmButton);
			base.AddChild(this.confirmButton);
			this.confirmButton.IsVisible = false;
			this.denyButton = Glazier.Get().CreateButton();
			this.denyButton.PositionOffset_X = 5f;
			this.denyButton.PositionScale_X = 0.5f;
			this.denyButton.SizeOffset_X = -5f;
			this.denyButton.SizeScale_X = 0.5f;
			this.denyButton.SizeScale_Y = 1f;
			this.denyButton.Text = newDeny;
			this.denyButton.TooltipText = newDenyTooltip;
			this.denyButton.OnClicked += new ClickedButton(this.onClickedDenyButton);
			base.AddChild(this.denyButton);
			this.denyButton.IsVisible = false;
		}

		// Token: 0x0400253D RID: 9533
		public Confirm onConfirmed;

		// Token: 0x0400253E RID: 9534
		public Deny onDenied;

		// Token: 0x0400253F RID: 9535
		private SleekButtonIcon mainButton;

		// Token: 0x04002540 RID: 9536
		private ISleekButton confirmButton;

		// Token: 0x04002541 RID: 9537
		private ISleekButton denyButton;
	}
}
