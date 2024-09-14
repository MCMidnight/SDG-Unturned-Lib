using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000534 RID: 1332
	public class BarricadeDrop
	{
		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060029BD RID: 10685 RVA: 0x000B2308 File Offset: 0x000B0508
		// (remove) Token: 0x060029BE RID: 10686 RVA: 0x000B233C File Offset: 0x000B053C
		public static event BarricadeDrop.SalvageRequestHandler OnSalvageRequested_Global;

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x000B236F File Offset: 0x000B056F
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000B2377 File Offset: 0x000B0577
		public Interactable interactable
		{
			get
			{
				return this._interactable;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060029C1 RID: 10689 RVA: 0x000B237F File Offset: 0x000B057F
		public uint instanceID
		{
			get
			{
				if (this.serversideData == null)
				{
					return 0U;
				}
				return this.serversideData.instanceID;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x000B2396 File Offset: 0x000B0596
		// (set) Token: 0x060029C3 RID: 10691 RVA: 0x000B239E File Offset: 0x000B059E
		public ItemBarricadeAsset asset { get; protected set; }

		// Token: 0x060029C4 RID: 10692 RVA: 0x000B23A7 File Offset: 0x000B05A7
		public BarricadeData GetServersideData()
		{
			return this.serversideData;
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000B23AF File Offset: 0x000B05AF
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000B23B8 File Offset: 0x000B05B8
		internal void AssignNetId(NetId netId)
		{
			this._netId = netId;
			NetIdRegistry.Assign(netId, this);
			NetIdRegistry.AssignTransform(netId + 1U, this._model);
			if (this._interactable != null)
			{
				this._interactable.AssignNetId(netId + 2U);
			}
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000B2408 File Offset: 0x000B0608
		internal void ReleaseNetId()
		{
			if (this._interactable != null)
			{
				this._interactable.ReleaseNetId();
			}
			NetIdRegistry.ReleaseTransform(this._netId + 1U, this._model);
			NetIdRegistry.Release(this._netId);
			this._netId.Clear();
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000B245C File Offset: 0x000B065C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveHealth(byte hp)
		{
			Interactable2HP component = this.model.GetComponent<Interactable2HP>();
			if (component != null)
			{
				component.hp = hp;
			}
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x000B2488 File Offset: 0x000B0688
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveTransform(in ClientInvocationContext context, byte old_x, byte old_y, ushort oldPlant, Vector3 point, Quaternion rotation)
		{
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(old_x, old_y, oldPlant, out barricadeRegion))
			{
				return;
			}
			if (!Provider.isServer && !barricadeRegion.isNetworked)
			{
				return;
			}
			this.model.position = point;
			this.model.rotation = rotation;
			if (oldPlant == 65535)
			{
				byte b;
				byte b2;
				if (Regions.tryGetCoordinate(point, out b, out b2) && (old_x != b || old_y != b2))
				{
					BarricadeRegion barricadeRegion2 = BarricadeManager.regions[(int)b, (int)b2];
					barricadeRegion.drops.Remove(this);
					if (barricadeRegion2.isNetworked || Provider.isServer)
					{
						barricadeRegion2.drops.Add(this);
					}
					else if (!Provider.isServer)
					{
						this.CustomDestroy();
					}
					if (Provider.isServer)
					{
						barricadeRegion.barricades.Remove(this.serversideData);
						barricadeRegion2.barricades.Add(this.serversideData);
					}
				}
				if (Provider.isServer)
				{
					this.serversideData.point = point;
					this.serversideData.rotation = rotation;
					return;
				}
			}
			else if (Provider.isServer)
			{
				this.serversideData.point = this.model.localPosition;
				this.serversideData.rotation = this.model.localRotation;
			}
		}

		/// <summary>
		/// Not using rate limit attribute because this is potentially called for hundreds of barricades at once,
		/// and only admins will actually be allowed to apply the transform.
		/// </summary>
		// Token: 0x060029CA RID: 10698 RVA: 0x000B25B4 File Offset: 0x000B07B4
		[SteamCall(ESteamCallValidation.SERVERSIDE)]
		public void ReceiveTransformRequest(in ServerInvocationContext context, Vector3 point, Quaternion rotation)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if (!player.look.canUseWorkzone)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(this._model, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			if (BarricadeManager.onTransformRequested != null)
			{
				Vector3 eulerAngles = rotation.eulerAngles;
				byte b = MeasurementTool.angleToByte(eulerAngles.x);
				byte b2 = MeasurementTool.angleToByte(eulerAngles.y);
				byte b3 = MeasurementTool.angleToByte(eulerAngles.z);
				byte b4 = b;
				byte b5 = b2;
				byte b6 = b3;
				bool flag = true;
				BarricadeManager.onTransformRequested(player.channel.owner.playerID.steamID, x, y, plant, this.instanceID, ref point, ref b4, ref b5, ref b6, ref flag);
				if (b4 != b || b5 != b2 || b6 != b3)
				{
					float x2 = MeasurementTool.byteToAngle(b4);
					float y2 = MeasurementTool.byteToAngle(b5);
					float z = MeasurementTool.byteToAngle(b6);
					rotation = Quaternion.Euler(x2, y2, z);
				}
				if (!flag)
				{
					point = this.model.position;
					rotation = this.model.rotation;
				}
			}
			BarricadeManager.InternalSetBarricadeTransform(x, y, plant, this, point, rotation);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000B26DC File Offset: 0x000B08DC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveOwnerAndGroup(ulong newOwner, ulong newGroup)
		{
			BarricadeDrop.workingSalvageArray.Clear();
			this.model.GetComponentsInChildren<Interactable2SalvageBarricade>(BarricadeDrop.workingSalvageArray);
			foreach (Interactable2SalvageBarricade interactable2SalvageBarricade in BarricadeDrop.workingSalvageArray)
			{
				interactable2SalvageBarricade.owner = newOwner;
				interactable2SalvageBarricade.group = newGroup;
			}
		}

		/// <summary>
		/// Only used by plugins.
		/// </summary>
		// Token: 0x060029CC RID: 10700 RVA: 0x000B2750 File Offset: 0x000B0950
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveUpdateState(byte[] newState)
		{
			if (this.asset == null || this.interactable == null)
			{
				UnturnedLog.warn("tellBarricadeUpdateState was missing asset/interactable");
				return;
			}
			this.interactable.updateState(this.asset, newState);
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000B2788 File Offset: 0x000B0988
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10)]
		public void ReceiveSalvageRequest(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((this._model.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (!this.asset.shouldBypassPickupOwnership && !OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, this.serversideData.owner, player.quests.groupID, this.serversideData.group))
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(this._model, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			bool flag = true;
			if (BarricadeManager.onSalvageBarricadeRequested != null)
			{
				ushort index = (ushort)barricadeRegion.drops.IndexOf(this);
				BarricadeManager.onSalvageBarricadeRequested(player.channel.owner.playerID.steamID, x, y, plant, index, ref flag);
			}
			BarricadeDrop.SalvageRequestHandler onSalvageRequested_Global = BarricadeDrop.OnSalvageRequested_Global;
			if (onSalvageRequested_Global != null)
			{
				onSalvageRequested_Global(this, context.GetCallingPlayer(), ref flag);
			}
			if (!flag)
			{
				return;
			}
			if (this.asset.isUnpickupable)
			{
				return;
			}
			if (this.serversideData.barricade.health >= this.asset.health)
			{
				player.inventory.forceAddItem(new Item(this.serversideData.barricade.asset.id, EItemOrigin.NATURE), true);
			}
			else if (this.asset.isSalvageable)
			{
				ItemAsset itemAsset = this.asset.FindSalvageItemAsset();
				if (itemAsset != null)
				{
					player.inventory.forceAddItem(new Item(itemAsset, EItemOrigin.NATURE), true);
				}
			}
			BarricadeManager.destroyBarricade(this, x, y, plant);
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000B2930 File Offset: 0x000B0B30
		internal void CustomDestroy()
		{
			try
			{
				BarricadeDrop.destroyEventComponents.Clear();
				this.model.GetComponents<IManualOnDestroy>(BarricadeDrop.destroyEventComponents);
				foreach (IManualOnDestroy manualOnDestroy in BarricadeDrop.destroyEventComponents)
				{
					manualOnDestroy.ManualOnDestroy();
				}
				this.ReleaseNetId();
				this.model.position = Vector3.zero;
				BarricadeManager.instance.DestroyOrReleaseBarricade(this.asset, this.model.gameObject);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception destroying barricade {0}:", new object[]
				{
					this.asset
				});
			}
		}

		/// <summary>
		/// See BarricadeRegion.FindBarricadeByRootFast comment.
		/// </summary>
		// Token: 0x060029CF RID: 10703 RVA: 0x000B29F4 File Offset: 0x000B0BF4
		internal static BarricadeDrop FindByRootFast(Transform rootTransform)
		{
			return rootTransform.GetComponent<BarricadeRefComponent>().tempNotSureIfBarricadeShouldBeAComponentYet;
		}

		/// <summary>
		/// For code which does not know whether transform exists and/or even is a barricade.
		/// See BarricadeRegion.FindBarricadeByRootFast comment.
		/// </summary>
		// Token: 0x060029D0 RID: 10704 RVA: 0x000B2A01 File Offset: 0x000B0C01
		internal static BarricadeDrop FindByTransformFastMaybeNull(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			BarricadeRefComponent component = transform.root.GetComponent<BarricadeRefComponent>();
			if (component == null)
			{
				return null;
			}
			return component.tempNotSureIfBarricadeShouldBeAComponentYet;
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000B2A1E File Offset: 0x000B0C1E
		internal BarricadeDrop(Transform newModel, Interactable newInteractable, ItemBarricadeAsset newAsset)
		{
			this._model = newModel;
			this._interactable = newInteractable;
			this.asset = newAsset;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000B2A3B File Offset: 0x000B0C3B
		[Obsolete]
		public BarricadeDrop(Transform newModel, Interactable newInteractable, uint newInstanceID, ItemBarricadeAsset newAsset) : this(newModel, newInteractable, newAsset)
		{
		}

		// Token: 0x04001687 RID: 5767
		private Transform _model;

		// Token: 0x04001688 RID: 5768
		private Interactable _interactable;

		// Token: 0x0400168A RID: 5770
		internal static readonly ClientInstanceMethod<byte> SendHealth = ClientInstanceMethod<byte>.Get(typeof(BarricadeDrop), "ReceiveHealth");

		// Token: 0x0400168B RID: 5771
		internal static readonly ClientInstanceMethod<byte, byte, ushort, Vector3, Quaternion> SendTransform = ClientInstanceMethod<byte, byte, ushort, Vector3, Quaternion>.Get(typeof(BarricadeDrop), "ReceiveTransform");

		// Token: 0x0400168C RID: 5772
		internal static readonly ServerInstanceMethod<Vector3, Quaternion> SendTransformRequest = ServerInstanceMethod<Vector3, Quaternion>.Get(typeof(BarricadeDrop), "ReceiveTransformRequest");

		// Token: 0x0400168D RID: 5773
		private static List<Interactable2SalvageBarricade> workingSalvageArray = new List<Interactable2SalvageBarricade>();

		// Token: 0x0400168E RID: 5774
		internal static readonly ClientInstanceMethod<ulong, ulong> SendOwnerAndGroup = ClientInstanceMethod<ulong, ulong>.Get(typeof(BarricadeDrop), "ReceiveOwnerAndGroup");

		// Token: 0x0400168F RID: 5775
		internal static readonly ClientInstanceMethod<byte[]> SendUpdateState = ClientInstanceMethod<byte[]>.Get(typeof(BarricadeDrop), "ReceiveUpdateState");

		// Token: 0x04001690 RID: 5776
		internal static readonly ServerInstanceMethod SendSalvageRequest = ServerInstanceMethod.Get(typeof(BarricadeDrop), "ReceiveSalvageRequest");

		// Token: 0x04001691 RID: 5777
		private static List<IManualOnDestroy> destroyEventComponents = new List<IManualOnDestroy>();

		// Token: 0x04001692 RID: 5778
		private NetId _netId;

		// Token: 0x04001693 RID: 5779
		internal BarricadeData serversideData;

		// Token: 0x0200096C RID: 2412
		// (Invoke) Token: 0x06004B56 RID: 19286
		public delegate void SalvageRequestHandler(BarricadeDrop barricade, SteamPlayer instigatorClient, ref bool shouldAllow);
	}
}
