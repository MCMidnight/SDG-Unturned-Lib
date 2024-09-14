using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000707 RID: 1799
	public class SleekButtonState : SleekWrapper
	{
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x001173A1 File Offset: 0x001155A1
		// (set) Token: 0x06003B97 RID: 15255 RVA: 0x001173A9 File Offset: 0x001155A9
		public GUIContent[] states { get; private set; }

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003B98 RID: 15256 RVA: 0x001173B2 File Offset: 0x001155B2
		// (set) Token: 0x06003B99 RID: 15257 RVA: 0x001173BA File Offset: 0x001155BA
		public int state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.synchronizeActiveContent();
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06003B9A RID: 15258 RVA: 0x001173C9 File Offset: 0x001155C9
		// (set) Token: 0x06003B9B RID: 15259 RVA: 0x001173D6 File Offset: 0x001155D6
		public string tooltip
		{
			get
			{
				return this.button.tooltip;
			}
			set
			{
				this.button.tooltip = value;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x001173E4 File Offset: 0x001155E4
		// (set) Token: 0x06003B9D RID: 15261 RVA: 0x001173F1 File Offset: 0x001155F1
		public bool isInteractable
		{
			get
			{
				return this.button.isClickable;
			}
			set
			{
				this.button.isClickable = value;
			}
		}

		/// <summary>
		/// If true, button tooltip will be overridden with tooltip from states array.
		/// </summary>
		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x001173FF File Offset: 0x001155FF
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x00117408 File Offset: 0x00115608
		public bool UseContentTooltip
		{
			get
			{
				return this._useContentTooltip;
			}
			set
			{
				this._useContentTooltip = value;
				if (this._useContentTooltip)
				{
					if (this.states != null && this.state >= 0 && this.state < this.states.Length && this.states[this.state] != null)
					{
						this.button.tooltip = this.states[this.state].tooltip;
						return;
					}
					this.button.tooltip = string.Empty;
				}
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x00117484 File Offset: 0x00115684
		private void synchronizeActiveContent()
		{
			if (this.states != null && this.state >= 0 && this.state < this.states.Length && this.states[this.state] != null)
			{
				this.button.text = this.states[this.state].text;
				this.button.icon = (this.states[this.state].image as Texture2D);
				if (this._useContentTooltip)
				{
					this.button.tooltip = this.states[this.state].tooltip;
					return;
				}
			}
			else
			{
				this.button.text = string.Empty;
				this.button.icon = null;
				if (this._useContentTooltip)
				{
					this.button.tooltip = string.Empty;
				}
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00117560 File Offset: 0x00115760
		protected virtual void onClickedState(ISleekElement button)
		{
			this._state++;
			if (this.state >= this.states.Length)
			{
				this._state = 0;
			}
			this.synchronizeActiveContent();
			SwappedState swappedState = this.onSwappedState;
			if (swappedState == null)
			{
				return;
			}
			swappedState(this, this.state);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001175B0 File Offset: 0x001157B0
		protected virtual void onRightClickedState(ISleekElement button)
		{
			this._state--;
			if (this.state < 0)
			{
				this._state = this.states.Length - 1;
			}
			this.synchronizeActiveContent();
			SwappedState swappedState = this.onSwappedState;
			if (swappedState == null)
			{
				return;
			}
			swappedState(this, this.state);
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00117601 File Offset: 0x00115801
		public void setContent(params GUIContent[] newStates)
		{
			this.states = newStates;
			if (this.state >= this.states.Length)
			{
				this._state = 0;
			}
			this.synchronizeActiveContent();
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x00117627 File Offset: 0x00115827
		public SleekButtonState(params GUIContent[] newStates) : this(0, newStates)
		{
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00117634 File Offset: 0x00115834
		public SleekButtonState(int iconSize, params GUIContent[] newStates)
		{
			this._state = 0;
			this.button = new SleekButtonIcon(null, iconSize);
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			base.AddChild(this.button);
			if (newStates != null)
			{
				this.setContent(newStates);
			}
			this.button.onClickedButton += new ClickedButton(this.onClickedState);
			this.button.onRightClickedButton += new ClickedButton(this.onRightClickedState);
		}

		// Token: 0x0400254A RID: 9546
		private int _state;

		// Token: 0x0400254B RID: 9547
		private bool _useContentTooltip;

		// Token: 0x0400254C RID: 9548
		public SwappedState onSwappedState;

		// Token: 0x0400254D RID: 9549
		internal SleekButtonIcon button;
	}
}
