using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200045E RID: 1118
	public class InteractableObjectNPC : InteractableObject
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x00083C8D File Offset: 0x00081E8D
		public ObjectNPCAsset npcAsset
		{
			get
			{
				return this._npcAsset;
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00083C95 File Offset: 0x00081E95
		public NetId GetNpcNetId()
		{
			return NetIdRegistry.GetTransformNetId(base.transform);
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x00083CA2 File Offset: 0x00081EA2
		public static InteractableObjectNPC GetNpcFromObjectNetId(NetId netId)
		{
			Transform transform = NetIdRegistry.GetTransform(netId, null);
			if (transform == null)
			{
				return null;
			}
			return transform.GetComponent<InteractableObjectNPC>();
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00083CB8 File Offset: 0x00081EB8
		internal void SetFaceOverride(byte? faceOverride)
		{
			byte b = (faceOverride != null) ? faceOverride.Value : this._npcAsset.face;
			if (this.clothes.face != b)
			{
				this.clothes.face = b;
				this.clothes.apply();
			}
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00083D08 File Offset: 0x00081F08
		internal void OnStoppedTalkingWithLocalPlayer()
		{
			this.isLookingAtPlayer = false;
			this.SetFaceOverride(default(byte?));
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00083D2C File Offset: 0x00081F2C
		private void updateStance()
		{
			this.stanceActive = null;
			if (this.npcAsset.pose == ENPCPose.SIT)
			{
				if (Random.value < 0.5f)
				{
					this.stanceIdle = "Idle_Sit";
					return;
				}
				this.stanceIdle = "Idle_Drive";
				return;
			}
			else
			{
				if (this.npcAsset.pose == ENPCPose.CROUCH)
				{
					this.stanceIdle = "Idle_Crouch";
					return;
				}
				if (this.npcAsset.pose == ENPCPose.PRONE)
				{
					this.stanceIdle = "Idle_Prone";
					return;
				}
				if (this.npcAsset.pose == ENPCPose.UNDER_ARREST)
				{
					this.stanceIdle = "Gesture_Arrest";
					return;
				}
				if (this.npcAsset.pose == ENPCPose.REST)
				{
					this.stanceIdle = "Gesture_Rest";
					return;
				}
				if (this.npcAsset.pose == ENPCPose.SURRENDER)
				{
					this.stanceIdle = "Gesture_Surrender";
					return;
				}
				if (this.itemHasEquipAnimation || this.npcAsset.pose == ENPCPose.ASLEEP)
				{
					this.stanceIdle = "Idle_Stand";
					return;
				}
				if (Random.value < 0.5f)
				{
					this.stanceIdle = "Idle_Stand";
					return;
				}
				this.stanceIdle = "Idle_Hips";
				return;
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00083E39 File Offset: 0x00082039
		private void updateIdle()
		{
			this.lastIdle = Time.time;
			this.idleDelay = Random.Range(5f, 30f);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00083E5B File Offset: 0x0008205B
		private void updateAnimation()
		{
			this.hasPlayedEquipAnimation = false;
			this.isPlayingSafetyAnimation = false;
			this.updateStance();
			this.updateIdle();
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00083E78 File Offset: 0x00082078
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			if (!this.isInit)
			{
				this.isInit = true;
				this._npcAsset = (asset as ObjectNPCAsset);
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00083EA8 File Offset: 0x000820A8
		public override void use()
		{
			if (this.npcAsset.FindDialogueAsset() == null)
			{
				UnturnedLog.warn("Failed to find NPC dialogue: " + this.npcAsset.FriendlyName);
				return;
			}
			ObjectManager.SendTalkWithNpcRequest.Invoke(ENetReliability.Reliable, this.GetNpcNetId());
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00083EE3 File Offset: 0x000820E3
		public override bool checkUseable()
		{
			return !PlayerUI.window.showCursor && !this.npcAsset.IsDialogueRefNull();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00083F01 File Offset: 0x00082101
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.TALK;
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00083F28 File Offset: 0x00082128
		private void Update()
		{
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00083F38 File Offset: 0x00082138
		private void LateUpdate()
		{
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00083F45 File Offset: 0x00082145
		private void OnEnable()
		{
		}

		// Token: 0x040010B4 RID: 4276
		protected ObjectNPCAsset _npcAsset;

		// Token: 0x040010B5 RID: 4277
		public bool isLookingAtPlayer;

		// Token: 0x040010B6 RID: 4278
		private bool isInit;

		// Token: 0x040010B7 RID: 4279
		private Animation anim;

		// Token: 0x040010B8 RID: 4280
		private HumanAnimator humanAnim;

		// Token: 0x040010B9 RID: 4281
		private HumanClothes clothes;

		// Token: 0x040010BA RID: 4282
		private Transform skull;

		// Token: 0x040010BB RID: 4283
		private bool itemHasEquipAnimation;

		// Token: 0x040010BC RID: 4284
		private bool itemHasSafetyAnimation;

		// Token: 0x040010BD RID: 4285
		private bool itemHasInspectAnimation;

		// Token: 0x040010BE RID: 4286
		private bool hasPlayedEquipAnimation;

		// Token: 0x040010BF RID: 4287
		private bool isPlayingSafetyAnimation;

		// Token: 0x040010C0 RID: 4288
		private string stanceIdle;

		// Token: 0x040010C1 RID: 4289
		private string stanceActive;

		// Token: 0x040010C2 RID: 4290
		private float lastIdle;

		// Token: 0x040010C3 RID: 4291
		private float idleDelay;

		// Token: 0x040010C4 RID: 4292
		private float headBlend;

		// Token: 0x040010C5 RID: 4293
		private Quaternion headRotation;
	}
}
