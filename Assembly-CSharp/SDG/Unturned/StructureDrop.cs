using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000593 RID: 1427
	public class StructureDrop
	{
		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06002D92 RID: 11666 RVA: 0x000C689C File Offset: 0x000C4A9C
		// (remove) Token: 0x06002D93 RID: 11667 RVA: 0x000C68D0 File Offset: 0x000C4AD0
		public static event StructureDrop.SalvageRequestHandler OnSalvageRequested_Global;

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x000C6903 File Offset: 0x000C4B03
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002D95 RID: 11669 RVA: 0x000C690B File Offset: 0x000C4B0B
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

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000C6922 File Offset: 0x000C4B22
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x000C692A File Offset: 0x000C4B2A
		public ItemStructureAsset asset { get; protected set; }

		// Token: 0x06002D98 RID: 11672 RVA: 0x000C6933 File Offset: 0x000C4B33
		public StructureData GetServersideData()
		{
			return this.serversideData;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000C693B File Offset: 0x000C4B3B
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000C6943 File Offset: 0x000C4B43
		internal void AssignNetId(NetId netId)
		{
			this._netId = netId;
			NetIdRegistry.Assign(netId, this);
			NetIdRegistry.AssignTransform(netId + 1U, this._model);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000C6965 File Offset: 0x000C4B65
		internal void ReleaseNetId()
		{
			NetIdRegistry.ReleaseTransform(this._netId + 1U, this._model);
			NetIdRegistry.Release(this._netId);
			this._netId.Clear();
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000C6998 File Offset: 0x000C4B98
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveHealth(byte hp)
		{
			Interactable2HP component = this.model.GetComponent<Interactable2HP>();
			if (component != null)
			{
				component.hp = hp;
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000C69C4 File Offset: 0x000C4BC4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveTransform(in ClientInvocationContext context, byte old_x, byte old_y, Vector3 point, Quaternion rotation)
		{
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(old_x, old_y, out structureRegion))
			{
				return;
			}
			if (!Provider.isServer && !structureRegion.isNetworked)
			{
				return;
			}
			try
			{
				StructureManager.housingConnections.UnlinkConnections(this);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception while unlinking housing connections:");
			}
			this.model.position = point;
			this.model.rotation = rotation;
			try
			{
				StructureManager.housingConnections.LinkConnections(this);
			}
			catch (Exception e2)
			{
				UnturnedLog.exception(e2, "Caught exception while linking housing connections:");
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(point, out b, out b2) && (old_x != b || old_y != b2))
			{
				StructureRegion structureRegion2 = StructureManager.regions[(int)b, (int)b2];
				structureRegion.drops.Remove(this);
				if (structureRegion2.isNetworked || Provider.isServer)
				{
					structureRegion2.drops.Add(this);
				}
				else if (!Provider.isServer)
				{
					StructureManager.instance.DestroyOrReleaseStructure(this);
					this.ReleaseNetId();
				}
				if (Provider.isServer)
				{
					structureRegion.structures.Remove(this.serversideData);
					structureRegion2.structures.Add(this.serversideData);
				}
			}
			if (Provider.isServer)
			{
				this.serversideData.point = point;
				this.serversideData.rotation = rotation;
			}
		}

		/// <summary>
		/// Not using rate limit attribute because this is potentially called for hundreds of structures at once,
		/// and only admins will actually be allowed to apply the transform.
		/// </summary>
		// Token: 0x06002D9E RID: 11678 RVA: 0x000C6B08 File Offset: 0x000C4D08
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
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(this._model, out x, out y, out structureRegion))
			{
				return;
			}
			if (StructureManager.onTransformRequested != null)
			{
				Vector3 eulerAngles = rotation.eulerAngles;
				byte b = MeasurementTool.angleToByte(eulerAngles.x);
				byte b2 = MeasurementTool.angleToByte(eulerAngles.y);
				byte b3 = MeasurementTool.angleToByte(eulerAngles.z);
				byte b4 = b;
				byte b5 = b2;
				byte b6 = b3;
				bool flag = true;
				StructureManager.onTransformRequested(player.channel.owner.playerID.steamID, x, y, this.instanceID, ref point, ref b4, ref b5, ref b6, ref flag);
				if (b4 != b || b5 != b2 || b6 != b3)
				{
					float x2 = MeasurementTool.byteToAngle(b4);
					float y2 = MeasurementTool.byteToAngle(b5);
					float z = MeasurementTool.byteToAngle(b6);
					rotation = Quaternion.Euler(x2, y2, z);
				}
				if (!flag)
				{
					point = this.serversideData.point;
					rotation = this.serversideData.rotation;
				}
			}
			StructureManager.InternalSetStructureTransform(x, y, this, point, rotation);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000C6C2C File Offset: 0x000C4E2C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveOwnerAndGroup(ulong newOwner, ulong newGroup)
		{
			StructureDrop.workingSalvageArray.Clear();
			this._model.GetComponentsInChildren<Interactable2SalvageStructure>(StructureDrop.workingSalvageArray);
			foreach (Interactable2SalvageStructure interactable2SalvageStructure in StructureDrop.workingSalvageArray)
			{
				interactable2SalvageStructure.owner = newOwner;
				interactable2SalvageStructure.group = newGroup;
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000C6CA0 File Offset: 0x000C4EA0
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10)]
		public void ReceiveSalvageRequest(in ServerInvocationContext context)
		{
			byte x;
			byte y;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(this._model, out x, out y, out structureRegion))
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
			if ((this._model.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (!OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, this.serversideData.owner, player.quests.groupID, this.serversideData.group))
			{
				return;
			}
			bool flag = true;
			if (StructureManager.onSalvageStructureRequested != null)
			{
				ushort index = (ushort)structureRegion.drops.IndexOf(this);
				StructureManager.onSalvageStructureRequested(player.channel.owner.playerID.steamID, x, y, index, ref flag);
			}
			StructureDrop.SalvageRequestHandler onSalvageRequested_Global = StructureDrop.OnSalvageRequested_Global;
			if (onSalvageRequested_Global != null)
			{
				onSalvageRequested_Global(this, context.GetCallingPlayer(), ref flag);
			}
			if (!flag)
			{
				return;
			}
			if (this.asset != null)
			{
				if (this.asset.isUnpickupable)
				{
					return;
				}
				if (this.serversideData.structure.health >= this.asset.health)
				{
					player.inventory.forceAddItem(new Item(this.asset.id, EItemOrigin.NATURE), true);
				}
				else if (this.asset.isSalvageable)
				{
					ItemAsset itemAsset = this.asset.FindSalvageItemAsset();
					if (itemAsset != null)
					{
						player.inventory.forceAddItem(new Item(itemAsset, EItemOrigin.NATURE), true);
					}
				}
			}
			StructureManager.destroyStructure(this, x, y, (this._model.position - player.transform.position).normalized * 100f, true);
		}

		/// <summary>
		/// See BarricadeRegion.FindBarricadeByRootFast comment.
		/// </summary>
		// Token: 0x06002DA1 RID: 11681 RVA: 0x000C6E61 File Offset: 0x000C5061
		internal static StructureDrop FindByRootFast(Transform rootTransform)
		{
			return rootTransform.GetComponent<StructureRefComponent>().tempNotSureIfStructureShouldBeAComponentYet;
		}

		/// <summary>
		/// For code which does not know whether transform exists and/or even is part of a house.
		/// See BarricadeRegion.FindBarricadeByRootFast comment.
		/// </summary>
		// Token: 0x06002DA2 RID: 11682 RVA: 0x000C6E6E File Offset: 0x000C506E
		internal static StructureDrop FindByTransformFastMaybeNull(Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			StructureRefComponent component = transform.root.GetComponent<StructureRefComponent>();
			if (component == null)
			{
				return null;
			}
			return component.tempNotSureIfStructureShouldBeAComponentYet;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000C6E8B File Offset: 0x000C508B
		internal StructureDrop(Transform newModel, ItemStructureAsset newAsset)
		{
			this._model = newModel;
			this.asset = newAsset;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000C6EA1 File Offset: 0x000C50A1
		[Obsolete]
		public StructureDrop(Transform newModel, uint newInstanceID) : this(newModel, null)
		{
		}

		// Token: 0x04001899 RID: 6297
		private Transform _model;

		// Token: 0x0400189B RID: 6299
		internal static readonly ClientInstanceMethod<byte> SendHealth = ClientInstanceMethod<byte>.Get(typeof(StructureDrop), "ReceiveHealth");

		// Token: 0x0400189C RID: 6300
		internal static readonly ClientInstanceMethod<byte, byte, Vector3, Quaternion> SendTransform = ClientInstanceMethod<byte, byte, Vector3, Quaternion>.Get(typeof(StructureDrop), "ReceiveTransform");

		// Token: 0x0400189D RID: 6301
		internal static readonly ServerInstanceMethod<Vector3, Quaternion> SendTransformRequest = ServerInstanceMethod<Vector3, Quaternion>.Get(typeof(StructureDrop), "ReceiveTransformRequest");

		// Token: 0x0400189E RID: 6302
		private static List<Interactable2SalvageStructure> workingSalvageArray = new List<Interactable2SalvageStructure>();

		// Token: 0x0400189F RID: 6303
		internal static readonly ClientInstanceMethod<ulong, ulong> SendOwnerAndGroup = ClientInstanceMethod<ulong, ulong>.Get(typeof(StructureDrop), "ReceiveOwnerAndGroup");

		// Token: 0x040018A0 RID: 6304
		internal static readonly ServerInstanceMethod SendSalvageRequest = ServerInstanceMethod.Get(typeof(StructureDrop), "ReceiveSalvageRequest");

		// Token: 0x040018A1 RID: 6305
		private NetId _netId;

		// Token: 0x040018A2 RID: 6306
		internal StructureData serversideData;

		// Token: 0x040018A3 RID: 6307
		internal HousingConnectionData housingConnectionData;

		// Token: 0x02000981 RID: 2433
		// (Invoke) Token: 0x06004B81 RID: 19329
		public delegate void SalvageRequestHandler(StructureDrop structure, SteamPlayer instigatorClient, ref bool shouldAllow);
	}
}
