using System;
using System.Collections;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200044F RID: 1103
	public class InteractableDoor : Interactable
	{
		/// <summary>
		/// Invoked when door is opened/closed, but not when loaded.
		/// </summary>
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06002128 RID: 8488 RVA: 0x0007FE4C File Offset: 0x0007E04C
		// (remove) Token: 0x06002129 RID: 8489 RVA: 0x0007FE80 File Offset: 0x0007E080
		public static event Action<InteractableDoor> OnDoorChanged_Global;

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x0007FEB3 File Offset: 0x0007E0B3
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x0007FEBB File Offset: 0x0007E0BB
		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x0007FEC3 File Offset: 0x0007E0C3
		public bool isOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x0007FECB File Offset: 0x0007E0CB
		public bool isOpenable
		{
			get
			{
				return Time.realtimeSinceStartup - this.opened > 0.75f;
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0007FEE0 File Offset: 0x0007E0E0
		public bool checkToggle(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			if (Provider.isServer && this.placeholderCollider != null && this.overlapBox(this.placeholderCollider) > 0)
			{
				return false;
			}
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0007FF54 File Offset: 0x0007E154
		public void updateToggle(bool newOpen)
		{
			this.opened = Time.realtimeSinceStartup;
			this._isOpen = newOpen;
			Animation component = base.GetComponent<Animation>();
			if (component != null)
			{
				this.playAnimation(component, false);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
			if (this.barrierTransform != null)
			{
				this.barrierTransform.gameObject.SetActive(!this.isOpen);
			}
			InteractableDoor.OnDoorChanged_Global.TryInvoke("OnDoorChanged_Global", this);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0007FFE0 File Offset: 0x0007E1E0
		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			this._isOpen = (state[16] == 1);
			Animation component = base.GetComponent<Animation>();
			if (component != null)
			{
				this.playAnimation(component, true);
			}
			Transform transform = base.transform.Find("Placeholder");
			if (transform != null)
			{
				this.placeholderCollider = transform.GetComponent<BoxCollider>();
			}
			else
			{
				this.placeholderCollider = null;
			}
			if (this.barrierTransform != null)
			{
				this.barrierTransform.gameObject.SetActive(!this.isOpen);
			}
			if (((ItemBarricadeAsset)asset).allowCollisionWhileAnimating)
			{
				this.doorColliders = null;
				return;
			}
			if (this.doorColliders == null)
			{
				this.doorColliders = new List<Collider>();
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000800C8 File Offset: 0x0007E2C8
		protected virtual void Start()
		{
			if (this.placeholderCollider != null && !base.IsChildOfVehicle)
			{
				this.barrierTransform = Object.Instantiate<GameObject>(this.placeholderCollider.gameObject).transform;
				this.barrierTransform.position = this.placeholderCollider.transform.position;
				this.barrierTransform.rotation = this.placeholderCollider.transform.rotation;
				this.barrierTransform.tag = "Barricade";
				this.barrierTransform.name = "ExpandedBarrier";
				this.barrierTransform.gameObject.layer = 27;
				this.barrierTransform.parent = base.transform;
				Rigidbody component = this.barrierTransform.GetComponent<Rigidbody>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				BoxCollider component2 = this.barrierTransform.GetComponent<BoxCollider>();
				if (component2 != null)
				{
					component2.size = new Vector3(component2.size.x + 0.25f, component2.size.y + 0.25f, 0.1f);
				}
				this.barrierTransform.gameObject.SetActive(!this.isOpen);
			}
			if (this.doorColliders != null)
			{
				base.GetComponentsInChildren<Collider>(this.doorColliders);
				for (int i = this.doorColliders.Count - 1; i >= 0; i--)
				{
					Collider collider = this.doorColliders[i];
					if (collider == this.placeholderCollider || collider.transform == this.barrierTransform)
					{
						this.doorColliders.RemoveAtFast(i);
					}
				}
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00080268 File Offset: 0x0007E468
		protected void playAnimation(Animation animationComponent, bool applyInstantly)
		{
			string text = this.isOpen ? "Open" : "Close";
			if (animationComponent.GetClip(text) == null)
			{
				return;
			}
			animationComponent.Play(text);
			if (applyInstantly)
			{
				animationComponent[text].normalizedTime = 1f;
				return;
			}
			if (this.doorColliders != null && this.doorColliders.Count > 0)
			{
				if (this.animCoroutine != null)
				{
					base.StopCoroutine(this.animCoroutine);
				}
				float length = animationComponent[text].length;
				this.animCoroutine = base.StartCoroutine(this.disableAnimatedColliders(length));
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00080304 File Offset: 0x0007E504
		protected int overlapBox(BoxCollider boxCollider)
		{
			int mask = base.IsChildOfVehicle ? RayMasks.BLOCK_CHAR_HINGE_OVERLAP_ON_VEHICLE : RayMasks.BLOCK_CHAR_HINGE_OVERLAP;
			return CollisionUtil.OverlapBoxColliderNonAlloc(boxCollider, InteractableDoor.checkColliders, mask, QueryTriggerInteraction.Collide);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00080334 File Offset: 0x0007E534
		protected bool areAnimatedCollidersOverlapping()
		{
			foreach (Collider collider in this.doorColliders)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				if (boxCollider != null && this.overlapBox(boxCollider) > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000803A0 File Offset: 0x0007E5A0
		protected IEnumerator disableAnimatedColliders(float delay)
		{
			foreach (Collider collider in this.doorColliders)
			{
				collider.enabled = false;
			}
			yield return new WaitForSeconds(delay);
			while (this.areAnimatedCollidersOverlapping())
			{
				yield return new WaitForSeconds(0.1f);
			}
			using (List<Collider>.Enumerator enumerator = this.doorColliders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Collider collider2 = enumerator.Current;
					collider2.enabled = true;
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000803B8 File Offset: 0x0007E5B8
		protected virtual void OnDisable()
		{
			if (this.animCoroutine != null)
			{
				base.StopCoroutine(this.animCoroutine);
				this.animCoroutine = null;
			}
			if (this.doorColliders != null)
			{
				foreach (Collider collider in this.doorColliders)
				{
					collider.enabled = true;
				}
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0008042C File Offset: 0x0007E62C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveOpen(bool newOpen)
		{
			this.updateToggle(newOpen);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00080435 File Offset: 0x0007E635
		public void ClientToggle()
		{
			InteractableDoor.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isOpen);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00080454 File Offset: 0x0007E654
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveToggleRequest(in ServerInvocationContext context, bool desiredOpen)
		{
			if (this.isOpen == desiredOpen)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out region))
			{
				return;
			}
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (this.isOpenable && this.checkToggle(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				BarricadeManager.ServerSetDoorOpenInternal(this, x, y, plant, region, !this.isOpen);
			}
		}

		// Token: 0x0400104B RID: 4171
		public static Collider[] checkColliders = new Collider[1];

		// Token: 0x0400104C RID: 4172
		private CSteamID _owner;

		// Token: 0x0400104D RID: 4173
		private CSteamID _group;

		// Token: 0x0400104E RID: 4174
		private bool _isOpen;

		// Token: 0x0400104F RID: 4175
		private bool isLocked;

		// Token: 0x04001050 RID: 4176
		private float opened;

		// Token: 0x04001051 RID: 4177
		private Transform barrierTransform;

		// Token: 0x04001052 RID: 4178
		private List<Collider> doorColliders;

		// Token: 0x04001053 RID: 4179
		private BoxCollider placeholderCollider;

		// Token: 0x04001054 RID: 4180
		protected Coroutine animCoroutine;

		// Token: 0x04001055 RID: 4181
		internal static readonly ClientInstanceMethod<bool> SendOpen = ClientInstanceMethod<bool>.Get(typeof(InteractableDoor), "ReceiveOpen");

		// Token: 0x04001056 RID: 4182
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableDoor), "ReceiveToggleRequest");
	}
}
