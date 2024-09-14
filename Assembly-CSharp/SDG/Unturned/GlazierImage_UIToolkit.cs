using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x0200019B RID: 411
	internal class GlazierImage_UIToolkit : GlazierElementBase_UIToolkit, ISleekImage, ISleekElement
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00029784 File Offset: 0x00027984
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x0002978C File Offset: 0x0002798C
		public Texture Texture
		{
			get
			{
				return this.desiredTexture;
			}
			set
			{
				if (this.desiredTexture != value)
				{
					this.internalSetTexture(value, this.ShouldDestroyTexture);
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x000297A9 File Offset: 0x000279A9
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x000297B1 File Offset: 0x000279B1
		public float RotationAngle
		{
			get
			{
				return this._rotationAngle;
			}
			set
			{
				this._rotationAngle = value;
				this.SynchronizeRotation();
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000297C0 File Offset: 0x000279C0
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x000297C8 File Offset: 0x000279C8
		public bool CanRotate
		{
			get
			{
				return this._canRotate;
			}
			set
			{
				this._canRotate = value;
				this.SynchronizeRotation();
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x000297D7 File Offset: 0x000279D7
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x000297DF File Offset: 0x000279DF
		public bool ShouldDestroyTexture { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x000297E8 File Offset: 0x000279E8
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x000297F0 File Offset: 0x000279F0
		public SleekColor TintColor
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				this.imageElement.tintColor = this._color;
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002980F File Offset: 0x00027A0F
		public void UpdateTexture(Texture2D newTexture)
		{
			if (this.desiredTexture != newTexture)
			{
				this.internalSetTexture(newTexture, this.ShouldDestroyTexture);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002982C File Offset: 0x00027A2C
		public void SetTextureAndShouldDestroy(Texture2D newTexture, bool newShouldDestroyTexture)
		{
			if (this.desiredTexture != newTexture || this.ShouldDestroyTexture != newShouldDestroyTexture)
			{
				this.internalSetTexture(newTexture, newShouldDestroyTexture);
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0002984D File Offset: 0x00027A4D
		public override void InternalDestroy()
		{
			if (this.ShouldDestroyTexture && this.desiredTexture != null)
			{
				Object.Destroy(this.desiredTexture);
				this.desiredTexture = null;
			}
			base.InternalDestroy();
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06000C48 RID: 3144 RVA: 0x00029880 File Offset: 0x00027A80
		// (remove) Token: 0x06000C49 RID: 3145 RVA: 0x000298B8 File Offset: 0x00027AB8
		private event Action _onImageClicked;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000C4A RID: 3146 RVA: 0x000298ED File Offset: 0x00027AED
		// (remove) Token: 0x06000C4B RID: 3147 RVA: 0x00029904 File Offset: 0x00027B04
		public event Action OnClicked
		{
			add
			{
				if (this.clickable == null)
				{
					this.CreateClickable();
				}
				this._onImageClicked += value;
			}
			remove
			{
				this._onImageClicked -= value;
			}
		}

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06000C4C RID: 3148 RVA: 0x00029910 File Offset: 0x00027B10
		// (remove) Token: 0x06000C4D RID: 3149 RVA: 0x00029948 File Offset: 0x00027B48
		private event Action _onImageRightClicked;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06000C4E RID: 3150 RVA: 0x0002997D File Offset: 0x00027B7D
		// (remove) Token: 0x06000C4F RID: 3151 RVA: 0x00029994 File Offset: 0x00027B94
		public event Action OnRightClicked
		{
			add
			{
				if (this.clickable == null)
				{
					this.CreateClickable();
				}
				this._onImageRightClicked += value;
			}
			remove
			{
				this._onImageRightClicked -= value;
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x000299A0 File Offset: 0x00027BA0
		public GlazierImage_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.containerElement = new VisualElement();
			this.containerElement.userData = this;
			this.containerElement.AddToClassList("unturned-empty");
			this.containerElement.pickingMode = 1;
			this.imageElement = new Image();
			this.imageElement.AddToClassList("unturned-image");
			this.imageElement.scaleMode = 0;
			this.imageElement.pickingMode = 1;
			this.containerElement.Add(this.imageElement);
			this.visualElement = this.containerElement;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00029A43 File Offset: 0x00027C43
		internal override void SynchronizeColors()
		{
			this.imageElement.tintColor = this._color;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00029A5C File Offset: 0x00027C5C
		private void internalSetTexture(Texture newTexture, bool newShouldDestroyTexture)
		{
			if (this.ShouldDestroyTexture && this.desiredTexture != null)
			{
				Object.Destroy(this.desiredTexture);
				this.desiredTexture = null;
			}
			this.desiredTexture = newTexture;
			this.ShouldDestroyTexture = newShouldDestroyTexture;
			this.imageElement.image = this.desiredTexture;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00029AB0 File Offset: 0x00027CB0
		private void CreateClickable()
		{
			this.containerElement.pickingMode = 0;
			this.clickable = new Clickable(new Action<EventBase>(this.OnClickedWithEventInfo));
			GlazierUtils_UIToolkit.AddClickableActivators(this.clickable);
			VisualElementExtensions.AddManipulator(this.containerElement, this.clickable);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00029AFC File Offset: 0x00027CFC
		private void OnClickedWithEventInfo(EventBase eventBase)
		{
			IMouseEvent mouseEvent = eventBase as IMouseEvent;
			if (mouseEvent != null)
			{
				int button = mouseEvent.button;
				if (button != 0)
				{
					if (button != 1)
					{
						return;
					}
					Action onImageRightClicked = this._onImageRightClicked;
					if (onImageRightClicked == null)
					{
						return;
					}
					onImageRightClicked.Invoke();
				}
				else
				{
					Action onImageClicked = this._onImageClicked;
					if (onImageClicked == null)
					{
						return;
					}
					onImageClicked.Invoke();
					return;
				}
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00029B43 File Offset: 0x00027D43
		private void SynchronizeRotation()
		{
			this.imageElement.transform.rotation = (this._canRotate ? Quaternion.AngleAxis(this._rotationAngle, Vector3.forward) : Quaternion.identity);
		}

		// Token: 0x040004A5 RID: 1189
		private float _rotationAngle;

		// Token: 0x040004A6 RID: 1190
		private bool _canRotate;

		// Token: 0x040004A8 RID: 1192
		private SleekColor _color = 0;

		// Token: 0x040004AB RID: 1195
		private VisualElement containerElement;

		// Token: 0x040004AC RID: 1196
		private Image imageElement;

		// Token: 0x040004AD RID: 1197
		private Clickable clickable;

		// Token: 0x040004AE RID: 1198
		private Texture desiredTexture;
	}
}
