using System;
using Pathfinding;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200045B RID: 1115
	public class InteractableObjectBinaryState : InteractableObject
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x0008306E File Offset: 0x0008126E
		public bool isUsed
		{
			get
			{
				return this._isUsed;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x00083076 File Offset: 0x00081276
		public bool isUsable
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityDelay && (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired);
			}
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000830A8 File Offset: 0x000812A8
		public bool checkCanReset(float multiplier)
		{
			return this.isUsed && base.objectAsset.interactabilityReset > 1f && Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000830E4 File Offset: 0x000812E4
		private void initAnimationComponent()
		{
			Transform transform = base.transform.Find("Root");
			if (transform != null)
			{
				this.animationComponent = transform.GetComponent<Animation>();
				this.animationComponent.playAutomatically = false;
				this.animationComponent.clip = null;
			}
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x00083130 File Offset: 0x00081330
		private void updateAnimationComponent(bool applyInstantly)
		{
			if (this.animationComponent == null)
			{
				return;
			}
			string text = this.isUsed ? "Open" : "Close";
			if (this.animationComponent.GetClip(text) == null)
			{
				return;
			}
			this.animationComponent.Play(text);
			if (applyInstantly)
			{
				this.animationComponent[text].normalizedTime = 1f;
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0008319C File Offset: 0x0008139C
		private void initAudioSourceComponent()
		{
			this.audioSourceComponent = base.transform.GetComponent<AudioSource>();
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000831AF File Offset: 0x000813AF
		private void updateAudioSourceComponent()
		{
			this.audioSourceComponent != null;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000831C0 File Offset: 0x000813C0
		private NavmeshCut initCutComponentFromBox(BoxCollider box)
		{
			NavmeshCut navmeshCut = box.gameObject.AddComponent<NavmeshCut>();
			navmeshCut.type = 0;
			navmeshCut.updateDistance = 0.4f;
			navmeshCut.isDual = false;
			navmeshCut.cutsAddedGeom = true;
			navmeshCut.updateRotationDistance = 10f;
			navmeshCut.useRotation = true;
			navmeshCut.center = box.center;
			navmeshCut.rectangleSize = new Vector2(box.size.x, box.size.z);
			navmeshCut.height = box.size.y;
			Object.Destroy(box);
			return navmeshCut;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00083250 File Offset: 0x00081450
		private void initCutComponent()
		{
			if (base.objectAsset.interactabilityNav != EObjectInteractabilityNav.NONE)
			{
				Transform transform = base.transform.Find("Nav");
				if (transform != null)
				{
					Transform transform2 = transform.Find("Blocker");
					if (transform2 != null)
					{
						this.cutComponent = transform2.GetComponent<NavmeshCut>();
						if (this.cutComponent == null)
						{
							BoxCollider component = transform2.GetComponent<BoxCollider>();
							if (component != null)
							{
								this.cutComponent = this.initCutComponentFromBox(component);
							}
						}
						if (this.cutComponent != null)
						{
							this.cutHeight = this.cutComponent.height;
						}
					}
				}
			}
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000832F4 File Offset: 0x000814F4
		private void updateCutComponent()
		{
			if (this.cutComponent != null)
			{
				if ((base.objectAsset.interactabilityNav == EObjectInteractabilityNav.ON && !this.isUsed) || (base.objectAsset.interactabilityNav == EObjectInteractabilityNav.OFF && this.isUsed))
				{
					this.cutHeight = this.cutComponent.height;
					this.cutComponent.height = 0f;
				}
				else
				{
					this.cutComponent.height = this.cutHeight;
				}
				this.cutComponent.ForceUpdate();
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x0008337C File Offset: 0x0008157C
		private void initToggleGameObject()
		{
			Transform transform = base.transform.FindChildRecursive("Toggle");
			LightLODTool.applyLightLOD(transform);
			if (transform != null)
			{
				this.material = HighlighterTool.getMaterialInstance(transform.parent);
				this.toggleGameObject = transform.gameObject;
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000833C8 File Offset: 0x000815C8
		private void updateToggleGameObject()
		{
			if (this.toggleGameObject != null)
			{
				if (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.STAY)
				{
					if (this.material != null)
					{
						this.material.SetColor("_EmissionColor", (this.isUsed && base.isWired) ? new Color(2f, 2f, 2f) : Color.black);
					}
					this.toggleGameObject.SetActive(this.isUsed && base.isWired);
					return;
				}
				if (this.material != null)
				{
					this.material.SetColor("_EmissionColor", this.isUsed ? new Color(2f, 2f, 2f) : Color.black);
				}
				this.toggleGameObject.SetActive(this.isUsed);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000834AC File Offset: 0x000816AC
		public void updateToggle(bool newUsed)
		{
			this.lastUsed = Time.realtimeSinceStartup;
			this._isUsed = newUsed;
			this.updateAnimationComponent(false);
			this.updateCutComponent();
			this.updateAudioSourceComponent();
			this.updateToggleGameObject();
			InteractableObjectBinaryState.UsedChanged usedChanged = this.onStateChanged;
			if (usedChanged == null)
			{
				return;
			}
			usedChanged(this);
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000834EA File Offset: 0x000816EA
		protected override void updateWired()
		{
			this.updateToggleGameObject();
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000834F4 File Offset: 0x000816F4
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isUsed = (state[0] == 1);
			if (!this.isInit)
			{
				this.isInit = true;
				this.initAnimationComponent();
				this.initCutComponent();
				this.initAudioSourceComponent();
				this.initToggleGameObject();
			}
			this.updateAnimationComponent(true);
			this.updateCutComponent();
			this.updateToggleGameObject();
			InteractableObjectBinaryState.UsedChanged usedChanged = this.onStateInitialized;
			if (usedChanged == null)
			{
				return;
			}
			usedChanged(this);
		}

		/// <summary>
		/// Invoked after state is first loaded, synced from server when entering relevancy, or reset.
		/// </summary>
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x060021DE RID: 8670 RVA: 0x00083560 File Offset: 0x00081760
		// (remove) Token: 0x060021DF RID: 8671 RVA: 0x00083598 File Offset: 0x00081798
		public event InteractableObjectBinaryState.UsedChanged onStateInitialized;

		/// <summary>
		/// Invoked after interaction changes state.
		/// </summary>
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x060021E0 RID: 8672 RVA: 0x000835D0 File Offset: 0x000817D0
		// (remove) Token: 0x060021E1 RID: 8673 RVA: 0x00083608 File Offset: 0x00081808
		public event InteractableObjectBinaryState.UsedChanged onStateChanged;

		// Token: 0x060021E2 RID: 8674 RVA: 0x00083640 File Offset: 0x00081840
		public void SetUsedFromClientOrServer(bool newUsed, InteractableObjectBinaryStateEventHook.EListenServerHostMode listenServerHostMode)
		{
			if (newUsed == this.isUsed)
			{
				return;
			}
			bool flag = false;
			if (flag)
			{
				ObjectManager.toggleObjectBinaryState(base.transform, newUsed);
				return;
			}
			ObjectManager.forceObjectBinaryState(base.transform, newUsed);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00083678 File Offset: 0x00081878
		public override void use()
		{
			bool flag = !this.isUsed;
			EffectAsset effectAsset = base.objectAsset.FindInteractabilityEffectAsset();
			if (effectAsset != null && Time.realtimeSinceStartup - this.lastEffect > 1f)
			{
				this.lastEffect = Time.realtimeSinceStartup;
				Transform transform = base.transform.Find("Effect");
				if (transform != null)
				{
					EffectManager.effect(effectAsset, transform.position, transform.forward);
				}
				else if (flag)
				{
					Transform transform2 = base.transform.Find("Effect_On");
					if (transform2 != null)
					{
						EffectManager.effect(effectAsset, transform2.position, transform2.forward);
					}
				}
				else if (!flag)
				{
					Transform transform3 = base.transform.Find("Effect_Off");
					if (transform3 != null)
					{
						EffectManager.effect(effectAsset, transform3.position, transform3.forward);
					}
				}
			}
			ObjectManager.toggleObjectBinaryState(base.transform, flag);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00083763 File Offset: 0x00081963
		public override bool checkInteractable()
		{
			return !base.objectAsset.interactabilityRemote;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x00083773 File Offset: 0x00081973
		public override bool checkUseable()
		{
			return (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired) && base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0008379C File Offset: 0x0008199C
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			for (int i = 0; i < base.objectAsset.interactabilityConditions.Length; i++)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					message = EPlayerMessage.CONDITION;
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					return true;
				}
			}
			if (base.objectAsset.interactabilityPower != EObjectInteractabilityPower.NONE && !base.isWired)
			{
				message = EPlayerMessage.POWER;
			}
			else if (this.isUsed)
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_CLOSE;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_OFF;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_OFF;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_OFF;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				case EObjectInteractabilityHint.CUSTOM:
					message = EPlayerMessage.INTERACT;
					text = base.objectAsset.interactabilityText;
					color = Color.white;
					return true;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			else
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_OPEN;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_ON;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_ON;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_ON;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				case EObjectInteractabilityHint.CUSTOM:
					message = EPlayerMessage.INTERACT;
					text = base.objectAsset.interactabilityText;
					color = Color.white;
					return true;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0008391A File Offset: 0x00081B1A
		private void OnEnable()
		{
			this.updateAnimationComponent(true);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00083923 File Offset: 0x00081B23
		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x0008393E File Offset: 0x00081B3E
		[Obsolete("Matches behavior from before addition of EListenServerHostMode.")]
		public void setUsedFromClientOrServer(bool newUsed)
		{
			this.SetUsedFromClientOrServer(newUsed, InteractableObjectBinaryStateEventHook.EListenServerHostMode.RequestAsClient);
		}

		// Token: 0x040010A2 RID: 4258
		private bool _isUsed;

		// Token: 0x040010A3 RID: 4259
		private bool isInit;

		// Token: 0x040010A4 RID: 4260
		private float lastUsed = -9999f;

		// Token: 0x040010A5 RID: 4261
		private Animation animationComponent;

		// Token: 0x040010A6 RID: 4262
		private AudioSource audioSourceComponent;

		// Token: 0x040010A7 RID: 4263
		private NavmeshCut cutComponent;

		// Token: 0x040010A8 RID: 4264
		private float cutHeight;

		// Token: 0x040010A9 RID: 4265
		private Material material;

		// Token: 0x040010AA RID: 4266
		private GameObject toggleGameObject;

		/// <summary>
		/// Number of event hooks monitoring or controlling this.
		/// Used to allow client to control remote objects on server.
		/// </summary>
		// Token: 0x040010AD RID: 4269
		public int modHookCounter;

		// Token: 0x040010AE RID: 4270
		private float lastEffect;

		// Token: 0x02000947 RID: 2375
		// (Invoke) Token: 0x06004AD5 RID: 19157
		public delegate void UsedChanged(InteractableObjectBinaryState sender);
	}
}
