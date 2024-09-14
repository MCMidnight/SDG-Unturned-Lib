using System;
using SDG.NetTransport;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000470 RID: 1136
	public class InteractableStorage : Interactable, IManualOnDestroy
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x00087F11 File Offset: 0x00086111
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x00087F19 File Offset: 0x00086119
		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x00087F21 File Offset: 0x00086121
		public Items items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x00087F29 File Offset: 0x00086129
		public bool isDisplay
		{
			get
			{
				return this._isDisplay;
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00087F34 File Offset: 0x00086134
		protected bool getDisplayStatTrackerValue(out EStatTrackerType type, out int kills)
		{
			DynamicEconDetails dynamicEconDetails = new DynamicEconDetails(this.displayTags, this.displayDynamicProps);
			return dynamicEconDetails.getStatTrackerValue(out type, out kills);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00087F60 File Offset: 0x00086160
		private void onStateUpdated()
		{
			if (this.isDisplay)
			{
				this.updateDisplay();
				BarricadeManager.sendStorageDisplay(base.transform, this.displayItem, this.displaySkin, this.displayMythic, this.displayTags, this.displayDynamicProps);
				this.refreshDisplay();
			}
			this.rebuildState();
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00087FB0 File Offset: 0x000861B0
		public void rebuildState()
		{
			if (this.items == null)
			{
				return;
			}
			Block block = new Block();
			block.write(this.owner, this.group, this.items.getItemCount());
			for (byte b = 0; b < this.items.getItemCount(); b += 1)
			{
				ItemJar item = this.items.getItem(b);
				block.write(item.x, item.y, item.rot, item.item.id, item.item.amount, item.item.quality, item.item.state);
			}
			if (this.isDisplay)
			{
				block.write(this.displaySkin);
				block.write(this.displayMythic);
				block.write(string.IsNullOrEmpty(this.displayTags) ? string.Empty : this.displayTags);
				block.write(string.IsNullOrEmpty(this.displayDynamicProps) ? string.Empty : this.displayDynamicProps);
				block.write(this.rot_comp);
			}
			int size;
			byte[] bytes = block.getBytes(out size);
			if (this.onStateRebuilt == null)
			{
				BarricadeManager.updateState(base.transform, bytes, size);
				return;
			}
			this.onStateRebuilt(this, bytes, size);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x00088134 File Offset: 0x00086334
		private void updateDisplay()
		{
			if (this.items != null && this.items.getItemCount() > 0)
			{
				if (this.displayItem == null || this.items.getItem(0).item != this.displayItem)
				{
					if (this.displayItem != null)
					{
						this.displaySkin = 0;
						this.displayMythic = 0;
						this.displayTags = string.Empty;
						this.displayDynamicProps = string.Empty;
					}
					this.displayItem = this.items.getItem(0).item;
					if (this.opener != null)
					{
						ItemAsset asset = this.displayItem.GetAsset();
						bool flag = asset != null && asset.sharedSkinLookupID != asset.id;
						ushort itemID = flag ? asset.sharedSkinLookupID : this.displayItem.id;
						int num;
						if (this.opener.channel.owner.getItemSkinItemDefID(itemID, out num))
						{
							if (!flag || asset.SharedSkinShouldApplyVisuals)
							{
								this.displaySkin = Provider.provider.economyService.getInventorySkinID(num);
							}
							else
							{
								this.displaySkin = 0;
							}
							this.displayMythic = Provider.provider.economyService.getInventoryMythicID(num);
							if (this.displayMythic == 0)
							{
								this.displayMythic = this.opener.channel.owner.getParticleEffectForItemDef(num);
							}
							this.opener.channel.owner.getTagsAndDynamicPropsForItem(num, out this.displayTags, out this.displayDynamicProps);
							return;
						}
					}
				}
			}
			else
			{
				this.displayItem = null;
				this.displaySkin = 0;
				this.displayMythic = 0;
				this.displayTags = string.Empty;
				this.displayDynamicProps = string.Empty;
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000882E0 File Offset: 0x000864E0
		public void setDisplay(ushort id, byte quality, byte[] state, ushort skin, ushort mythic, string tags, string dynamicProps)
		{
			if (id == 0)
			{
				this.displayItem = null;
			}
			else
			{
				this.displayItem = new Item(id, 0, quality, state);
			}
			this.displaySkin = skin;
			this.displayMythic = mythic;
			this.displayTags = tags;
			this.displayDynamicProps = dynamicProps;
			this.refreshDisplay();
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0008832E File Offset: 0x0008652E
		public byte getRotation(byte rot_x, byte rot_y, byte rot_z)
		{
			return (byte)((int)rot_x << 4 | (int)rot_y << 2 | (int)rot_z);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x0008833C File Offset: 0x0008653C
		public void applyRotation(byte rotComp)
		{
			this.rot_comp = rotComp;
			this.rot_x = (byte)(rotComp >> 4 & 3);
			this.rot_y = (byte)(rotComp >> 2 & 3);
			this.rot_z = (rotComp & 3);
			this.displayRotation = Quaternion.Euler((float)(this.rot_x * 90), (float)(this.rot_y * 90), (float)(this.rot_z * 90));
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0008839B File Offset: 0x0008659B
		public void setRotation(byte rotComp)
		{
			this.applyRotation(rotComp);
			this.refreshDisplay();
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000883AC File Offset: 0x000865AC
		public virtual void refreshDisplay()
		{
			if (this.displayModel != null)
			{
				Object.Destroy(this.displayModel.gameObject);
				this.displayModel = null;
				this.displayAsset = null;
			}
			if (this.displayItem == null)
			{
				return;
			}
			if (this.gunLargeTransform == null || this.gunSmallTransform == null || this.meleeTransform == null || this.itemTransform == null)
			{
				return;
			}
			this.displayAsset = this.displayItem.GetAsset();
			if (this.displayAsset == null)
			{
				return;
			}
			if (this.displaySkin != 0)
			{
				if (!(Assets.find(EAssetType.SKIN, this.displaySkin) is SkinAsset))
				{
					return;
				}
				this.displayModel = ItemTool.getItem(this.displayItem.id, this.displaySkin, this.displayItem.quality, this.displayItem.state, false, this.displayAsset, true, new GetStatTrackerValueHandler(this.getDisplayStatTrackerValue));
				if (this.displayMythic != 0)
				{
					ItemTool.ApplyMythicalEffect(this.displayModel, this.displayMythic, EEffectType.THIRD);
				}
			}
			else
			{
				this.displayModel = ItemTool.getItem(this.displayItem.id, 0, this.displayItem.quality, this.displayItem.state, false, this.displayAsset, true, new GetStatTrackerValueHandler(this.getDisplayStatTrackerValue));
				if (this.displayMythic != 0)
				{
					ItemTool.ApplyMythicalEffect(this.displayModel, this.displayMythic, EEffectType.HOOK);
				}
			}
			if (this.displayModel == null)
			{
				return;
			}
			if (this.displayAsset.type == EItemType.GUN)
			{
				if (this.displayAsset.slot == ESlotType.PRIMARY)
				{
					this.displayModel.parent = this.gunLargeTransform;
				}
				else
				{
					this.displayModel.parent = this.gunSmallTransform;
				}
			}
			else if (this.displayAsset.type == EItemType.MELEE)
			{
				this.displayModel.parent = this.meleeTransform;
			}
			else
			{
				this.displayModel.parent = this.itemTransform;
			}
			this.displayModel.localPosition = Vector3.zero;
			this.displayModel.localRotation = this.displayRotation;
			this.displayModel.localScale = Vector3.one;
			this.displayModel.DestroyRigidbody();
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000885DC File Offset: 0x000867DC
		public bool checkRot(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0008861C File Offset: 0x0008681C
		public bool checkStore(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			bool isServer = Provider.isServer;
			return (!this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group)) && !this.isOpen;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00088670 File Offset: 0x00086870
		public override void updateState(Asset asset, byte[] state)
		{
			this.gunLargeTransform = base.transform.FindChildRecursive("Gun_Large");
			this.gunSmallTransform = base.transform.FindChildRecursive("Gun_Small");
			this.meleeTransform = base.transform.FindChildRecursive("Melee");
			this.itemTransform = base.transform.FindChildRecursive("Item");
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._isDisplay = ((ItemStorageAsset)asset).isDisplay;
			this.shouldCloseWhenOutsideRange = ((ItemStorageAsset)asset).shouldCloseWhenOutsideRange;
			if (Provider.isServer)
			{
				Block block = new Block(state);
				this._owner = (CSteamID)block.read(Types.STEAM_ID_TYPE);
				this._group = (CSteamID)block.read(Types.STEAM_ID_TYPE);
				this._items = new Items(PlayerInventory.STORAGE);
				this.items.resize(((ItemStorageAsset)asset).storage_x, ((ItemStorageAsset)asset).storage_y);
				byte b = block.readByte();
				for (byte b2 = 0; b2 < b; b2 += 1)
				{
					if (BarricadeManager.version > 7)
					{
						object[] array = block.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
						if (Assets.find(EAssetType.ITEM, (ushort)array[3]) is ItemAsset)
						{
							this.items.loadItem((byte)array[0], (byte)array[1], (byte)array[2], new Item((ushort)array[3], (byte)array[4], (byte)array[5], (byte[])array[6]));
						}
					}
					else
					{
						object[] array2 = block.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
						if (Assets.find(EAssetType.ITEM, (ushort)array2[2]) is ItemAsset)
						{
							this.items.loadItem((byte)array2[0], (byte)array2[1], 0, new Item((ushort)array2[2], (byte)array2[3], (byte)array2[4], (byte[])array2[5]));
						}
					}
				}
				if (this.isDisplay)
				{
					this.displaySkin = block.readUInt16();
					this.displayMythic = block.readUInt16();
					if (BarricadeManager.version > 12)
					{
						this.displayTags = block.readString();
						this.displayDynamicProps = block.readString();
					}
					else
					{
						this.displayTags = string.Empty;
						this.displayDynamicProps = string.Empty;
					}
					if (BarricadeManager.version > 8)
					{
						this.applyRotation(block.readByte());
					}
					else
					{
						this.applyRotation(0);
					}
				}
				this.items.onStateUpdated = new StateUpdated(this.onStateUpdated);
				if (this.isDisplay)
				{
					this.updateDisplay();
					this.refreshDisplay();
					return;
				}
			}
			else
			{
				Block block2 = new Block(state);
				this._owner = new CSteamID((ulong)block2.read(Types.UINT64_TYPE));
				this._group = new CSteamID((ulong)block2.read(Types.UINT64_TYPE));
				if (state.Length > 16)
				{
					object[] array3 = block2.read(new Type[]
					{
						Types.UINT16_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_ARRAY_TYPE,
						Types.UINT16_TYPE,
						Types.UINT16_TYPE,
						Types.STRING_TYPE,
						Types.STRING_TYPE,
						Types.BYTE_TYPE
					});
					this.applyRotation((byte)array3[7]);
					this.setDisplay((ushort)array3[0], (byte)array3[1], (byte[])array3[2], (ushort)array3[3], (ushort)array3[4], (string)array3[5], (string)array3[6]);
				}
			}
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00088A3F File Offset: 0x00086C3F
		public override bool checkUseable()
		{
			return this.checkStore(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x00088A6C File Offset: 0x00086C6C
		public override void use()
		{
			this.ClientInteract(InputEx.GetKey(ControlsSettings.other));
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x00088A7E File Offset: 0x00086C7E
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = "";
			color = Color.white;
			if (this.checkUseable())
			{
				message = EPlayerMessage.STORAGE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			return true;
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x00088AA5 File Offset: 0x00086CA5
		private void Start()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (BarricadeManager.version < 13)
			{
				this.onStateUpdated();
			}
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x00088AC0 File Offset: 0x00086CC0
		public void ManualOnDestroy()
		{
			if (this.isDisplay)
			{
				this.setDisplay(0, 0, null, 0, 0, string.Empty, string.Empty);
			}
			if (!Provider.isServer)
			{
				return;
			}
			this.items.onStateUpdated = null;
			if (!this.despawnWhenDestroyed)
			{
				for (byte b = 0; b < this.items.getItemCount(); b += 1)
				{
					ItemManager.dropItem(this.items.getItem(b).item, base.transform.position, false, true, true);
				}
			}
			this.items.clear();
			this._items = null;
			if (this.isOpen)
			{
				if (this.opener != null)
				{
					if (this.opener.inventory.isStoring)
					{
						this.opener.inventory.closeStorageAndNotifyClient();
					}
					this.opener = null;
				}
				this.isOpen = false;
			}
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x00088B99 File Offset: 0x00086D99
		public void ClientInteract(bool quickGrab)
		{
			InteractableStorage.SendInteractRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, quickGrab);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00088BB0 File Offset: 0x00086DB0
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 4)]
		public void ReceiveInteractRequest(in ServerInvocationContext context, bool quickGrab)
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
			if (player.inventory.isStoring && player.inventory.isStorageTrunk)
			{
				return;
			}
			if (player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			Vector3 position = base.transform.position;
			if ((position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (Physics.Linecast(player.look.getEyesPosition(), position, RayMasks.BLOCK_BARRICADE_INTERACT_LOS, QueryTriggerInteraction.Ignore))
			{
				return;
			}
			if (player.inventory.isStoring)
			{
				player.inventory.closeStorage();
			}
			if (this.checkStore(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				bool flag = true;
				OpenStorageRequestHandler onOpenStorageRequested = BarricadeManager.onOpenStorageRequested;
				if (onOpenStorageRequested != null)
				{
					onOpenStorageRequested(player.channel.owner.playerID.steamID, this, ref flag);
				}
				if (!flag)
				{
					return;
				}
				if (!this.isDisplay || !quickGrab)
				{
					player.inventory.openStorage(this);
					return;
				}
				if (this.displayItem != null)
				{
					player.inventory.forceAddItem(this.displayItem, true);
					this.displayItem = null;
					this.displaySkin = 0;
					this.displayMythic = 0;
					this.displayTags = string.Empty;
					this.displayDynamicProps = string.Empty;
					this.items.removeItem(0);
					return;
				}
			}
			else
			{
				player.sendMessage(EPlayerMessage.BUSY);
			}
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x00088D30 File Offset: 0x00086F30
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveDisplay(ushort id, byte quality, byte[] state, ushort skin, ushort mythic, string tags, string dynamicProps)
		{
			this.setDisplay(id, quality, state, skin, mythic, tags, dynamicProps);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00088D43 File Offset: 0x00086F43
		public void ClientSetDisplayRotation(byte rotComp)
		{
			InteractableStorage.SendRotDisplayRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, rotComp);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00088D57 File Offset: 0x00086F57
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveRotDisplay(byte rotComp)
		{
			this.setRotation(rotComp);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00088D60 File Offset: 0x00086F60
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveRotDisplayRequest(in ServerInvocationContext context, byte rotComp)
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
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			if (this.checkRot(player.channel.owner.playerID.steamID, player.quests.groupID) && this.isDisplay)
			{
				InteractableStorage.SendRotDisplay.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), rotComp);
				this.rebuildState();
			}
		}

		// Token: 0x04001135 RID: 4405
		private CSteamID _owner;

		// Token: 0x04001136 RID: 4406
		private CSteamID _group;

		// Token: 0x04001137 RID: 4407
		private Items _items;

		// Token: 0x04001138 RID: 4408
		private Transform gunLargeTransform;

		// Token: 0x04001139 RID: 4409
		private Transform gunSmallTransform;

		// Token: 0x0400113A RID: 4410
		private Transform meleeTransform;

		// Token: 0x0400113B RID: 4411
		private Transform itemTransform;

		// Token: 0x0400113C RID: 4412
		protected Transform displayModel;

		// Token: 0x0400113D RID: 4413
		protected ItemAsset displayAsset;

		// Token: 0x0400113E RID: 4414
		public Item displayItem;

		// Token: 0x0400113F RID: 4415
		public ushort displaySkin;

		// Token: 0x04001140 RID: 4416
		public ushort displayMythic;

		// Token: 0x04001141 RID: 4417
		public string displayTags = string.Empty;

		// Token: 0x04001142 RID: 4418
		public string displayDynamicProps = string.Empty;

		// Token: 0x04001143 RID: 4419
		private Quaternion displayRotation;

		// Token: 0x04001144 RID: 4420
		public byte rot_comp;

		// Token: 0x04001145 RID: 4421
		public byte rot_x;

		// Token: 0x04001146 RID: 4422
		public byte rot_y;

		// Token: 0x04001147 RID: 4423
		public byte rot_z;

		// Token: 0x04001148 RID: 4424
		public bool isOpen;

		// Token: 0x04001149 RID: 4425
		public Player opener;

		// Token: 0x0400114A RID: 4426
		private bool isLocked;

		// Token: 0x0400114B RID: 4427
		private bool _isDisplay;

		// Token: 0x0400114C RID: 4428
		public bool despawnWhenDestroyed;

		/// <summary>
		/// If player gets too far away from this storage while using it, should we close out?
		/// False by default for trunk storage because player is inside vehicle.
		/// Plugins needed to be able to set this to false for "virtual storage" plugins,
		/// so we default to false and set to true if asset enables it.
		/// </summary>
		// Token: 0x0400114D RID: 4429
		public bool shouldCloseWhenOutsideRange;

		// Token: 0x0400114E RID: 4430
		public InteractableStorage.RebuiltStateHandler onStateRebuilt;

		// Token: 0x0400114F RID: 4431
		private static readonly ServerInstanceMethod<bool> SendInteractRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableStorage), "ReceiveInteractRequest");

		// Token: 0x04001150 RID: 4432
		internal static readonly ClientInstanceMethod<ushort, byte, byte[], ushort, ushort, string, string> SendDisplay = ClientInstanceMethod<ushort, byte, byte[], ushort, ushort, string, string>.Get(typeof(InteractableStorage), "ReceiveDisplay");

		// Token: 0x04001151 RID: 4433
		private static readonly ClientInstanceMethod<byte> SendRotDisplay = ClientInstanceMethod<byte>.Get(typeof(InteractableStorage), "ReceiveRotDisplay");

		// Token: 0x04001152 RID: 4434
		private static readonly ServerInstanceMethod<byte> SendRotDisplayRequest = ServerInstanceMethod<byte>.Get(typeof(InteractableStorage), "ReceiveRotDisplayRequest");

		// Token: 0x02000948 RID: 2376
		// (Invoke) Token: 0x06004AD9 RID: 19161
		public delegate void RebuiltStateHandler(InteractableStorage storage, byte[] state, int size);
	}
}
