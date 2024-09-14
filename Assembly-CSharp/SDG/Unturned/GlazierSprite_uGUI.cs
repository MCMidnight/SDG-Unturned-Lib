using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000189 RID: 393
	internal class GlazierSprite_uGUI : GlazierElementBase_uGUI, ISleekSprite, ISleekElement
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x000259F3 File Offset: 0x00023BF3
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x00025A00 File Offset: 0x00023C00
		public Sprite Sprite
		{
			get
			{
				return this.imageComponent.sprite;
			}
			set
			{
				this.imageComponent.sprite = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00025A0E File Offset: 0x00023C0E
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x00025A16 File Offset: 0x00023C16
		public SleekColor TintColor
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				this.SynchronizeColors();
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00025A25 File Offset: 0x00023C25
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x00025A30 File Offset: 0x00023C30
		public ESleekSpriteType DrawMethod
		{
			get
			{
				return this._drawMethod;
			}
			set
			{
				this._drawMethod = value;
				switch (value)
				{
				case 0:
					this.imageComponent.type = 2;
					return;
				case 1:
					this.imageComponent.type = 1;
					return;
				}
				this.imageComponent.type = 0;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00025A7E File Offset: 0x00023C7E
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00025A85 File Offset: 0x00023C85
		public bool IsRaycastTarget
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00025A8C File Offset: 0x00023C8C
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x00025A94 File Offset: 0x00023C94
		public Vector2Int TileRepeatHintForUITK { get; set; }

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06000B2B RID: 2859 RVA: 0x00025AA0 File Offset: 0x00023CA0
		// (remove) Token: 0x06000B2C RID: 2860 RVA: 0x00025AD8 File Offset: 0x00023CD8
		private event Action _onImageClicked;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06000B2D RID: 2861 RVA: 0x00025B0D File Offset: 0x00023D0D
		// (remove) Token: 0x06000B2E RID: 2862 RVA: 0x00025B2A File Offset: 0x00023D2A
		public event Action OnClicked
		{
			add
			{
				if (this.buttonComponent == null)
				{
					this.CreateButton();
				}
				this._onImageClicked += value;
			}
			remove
			{
				this._onImageClicked -= value;
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00025B33 File Offset: 0x00023D33
		public GlazierSprite_uGUI(Glazier_uGUI glazier, Sprite sprite) : base(glazier)
		{
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00025B3C File Offset: 0x00023D3C
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.enabled = false;
			this.imageComponent.raycastTarget = false;
			this.imageComponent.sprite = this.Sprite;
			this._color = 0;
			this.DrawMethod = 0;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00025B9C File Offset: 0x00023D9C
		public override void SynchronizeColors()
		{
			if (this.Sprite != null)
			{
				this.imageComponent.color = this._color;
				this.imageComponent.enabled = true;
				return;
			}
			if (this.imageComponent.raycastTarget)
			{
				this.imageComponent.color = ColorEx.BlackZeroAlpha;
				this.imageComponent.enabled = true;
				return;
			}
			this.imageComponent.enabled = false;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00025C10 File Offset: 0x00023E10
		protected override void EnableComponents()
		{
			this.imageComponent.enabled = (this.Sprite != null || this.imageComponent.raycastTarget);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00025C3C File Offset: 0x00023E3C
		private void CreateButton()
		{
			this.imageComponent.raycastTarget = true;
			this.buttonComponent = base.gameObject.AddComponent<ButtonEx>();
			this.buttonComponent.transition = 0;
			this.buttonComponent.onClick.AddListener(new UnityAction(this.OnUnityClick));
			this.SynchronizeColors();
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00025C94 File Offset: 0x00023E94
		private void OnUnityClick()
		{
			Action onImageClicked = this._onImageClicked;
			if (onImageClicked == null)
			{
				return;
			}
			onImageClicked.Invoke();
		}

		// Token: 0x04000440 RID: 1088
		private SleekColor _color;

		// Token: 0x04000441 RID: 1089
		private ESleekSpriteType _drawMethod;

		// Token: 0x04000444 RID: 1092
		private Image imageComponent;

		// Token: 0x04000445 RID: 1093
		private ButtonEx buttonComponent;
	}
}
