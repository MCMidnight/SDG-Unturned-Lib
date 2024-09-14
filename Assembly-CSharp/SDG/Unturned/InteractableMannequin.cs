using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000459 RID: 1113
	public class InteractableMannequin : Interactable, IManualOnDestroy
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x00081A5B File Offset: 0x0007FC5B
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x00081A63 File Offset: 0x0007FC63
		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x00081A6B File Offset: 0x0007FC6B
		public bool isUpdatable
		{
			get
			{
				return Time.realtimeSinceStartup - this.updated > 0.5f;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x00081A80 File Offset: 0x0007FC80
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x00081A88 File Offset: 0x0007FC88
		public HumanClothes clothes { get; private set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x00081A91 File Offset: 0x0007FC91
		public int visualShirt
		{
			get
			{
				return this.clothes.visualShirt;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060021A4 RID: 8612 RVA: 0x00081A9E File Offset: 0x0007FC9E
		public int visualPants
		{
			get
			{
				return this.clothes.visualPants;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x00081AAB File Offset: 0x0007FCAB
		public int visualHat
		{
			get
			{
				return this.clothes.visualHat;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060021A6 RID: 8614 RVA: 0x00081AB8 File Offset: 0x0007FCB8
		public int visualBackpack
		{
			get
			{
				return this.clothes.visualBackpack;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x00081AC5 File Offset: 0x0007FCC5
		public int visualVest
		{
			get
			{
				return this.clothes.visualVest;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x00081AD2 File Offset: 0x0007FCD2
		public int visualMask
		{
			get
			{
				return this.clothes.visualMask;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x00081ADF File Offset: 0x0007FCDF
		public int visualGlasses
		{
			get
			{
				return this.clothes.visualGlasses;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060021AA RID: 8618 RVA: 0x00081AEC File Offset: 0x0007FCEC
		public ushort shirt
		{
			get
			{
				return this.clothes.shirt;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x00081AF9 File Offset: 0x0007FCF9
		public ushort pants
		{
			get
			{
				return this.clothes.pants;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x00081B06 File Offset: 0x0007FD06
		public ushort hat
		{
			get
			{
				return this.clothes.hat;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00081B13 File Offset: 0x0007FD13
		public ushort backpack
		{
			get
			{
				return this.clothes.backpack;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x00081B20 File Offset: 0x0007FD20
		public ushort vest
		{
			get
			{
				return this.clothes.vest;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00081B2D File Offset: 0x0007FD2D
		public ushort mask
		{
			get
			{
				return this.clothes.mask;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x00081B3A File Offset: 0x0007FD3A
		public ushort glasses
		{
			get
			{
				return this.clothes.glasses;
			}
		}

		/// <summary>
		/// Are any players standing on the mannequin?
		/// Used to prevent exploiting pose switches to push through objects.
		/// </summary>
		// Token: 0x060021B1 RID: 8625 RVA: 0x00081B48 File Offset: 0x0007FD48
		public bool isObstructedByPlayers()
		{
			Vector3 position = base.transform.position;
			Vector3 point = position + new Vector3(0f, -0.6f, 0f);
			Vector3 point2 = position + new Vector3(0f, 0.6f, 0f);
			int layerMask = base.IsChildOfVehicle ? RayMasks.BLOCK_CHAR_HINGE_OVERLAP_ON_VEHICLE : RayMasks.BLOCK_CHAR_HINGE_OVERLAP;
			return Physics.OverlapCapsuleNonAlloc(point, point2, 0.4f, InteractableDoor.checkColliders, layerMask, QueryTriggerInteraction.Ignore) > 0;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00081BC0 File Offset: 0x0007FDC0
		public bool checkUpdate(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00081C00 File Offset: 0x0007FE00
		public byte getComp(bool mirror, byte pose)
		{
			return (byte)((int)(mirror ? 1 : 0) << 7 | (int)pose);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00081C0F File Offset: 0x0007FE0F
		public void applyPose(byte poseComp)
		{
			this.pose_comp = poseComp;
			this.mirror = ((poseComp >> 7 & 1) == 1);
			this.pose = (poseComp & 127);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00081C31 File Offset: 0x0007FE31
		public void setPose(byte poseComp)
		{
			this.applyPose(poseComp);
			this.updatePose();
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00081C40 File Offset: 0x0007FE40
		public void rebuildState()
		{
			Block block = new Block();
			block.write(this.owner, this.group);
			block.writeInt32(this.visualShirt);
			block.writeInt32(this.visualPants);
			block.writeInt32(this.visualHat);
			block.writeInt32(this.visualBackpack);
			block.writeInt32(this.visualVest);
			block.writeInt32(this.visualMask);
			block.writeInt32(this.visualGlasses);
			block.writeUInt16(this.clothes.shirt);
			block.writeByte(this.shirtQuality);
			block.writeUInt16(this.clothes.pants);
			block.writeByte(this.pantsQuality);
			block.writeUInt16(this.clothes.hat);
			block.writeByte(this.hatQuality);
			block.writeUInt16(this.clothes.backpack);
			block.writeByte(this.backpackQuality);
			block.writeUInt16(this.clothes.vest);
			block.writeByte(this.vestQuality);
			block.writeUInt16(this.clothes.mask);
			block.writeByte(this.maskQuality);
			block.writeUInt16(this.clothes.glasses);
			block.writeByte(this.glassesQuality);
			block.writeByteArray(this.shirtState);
			block.writeByteArray(this.pantsState);
			block.writeByteArray(this.hatState);
			block.writeByteArray(this.backpackState);
			block.writeByteArray(this.vestState);
			block.writeByteArray(this.maskState);
			block.writeByteArray(this.glassesState);
			block.writeByte(this.pose_comp);
			int size;
			byte[] bytes = block.getBytes(out size);
			BarricadeManager.updateState(base.transform, bytes, size);
			this.updated = Time.realtimeSinceStartup;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00081E10 File Offset: 0x00080010
		public void updateVisuals(int newVisualShirt, int newVisualPants, int newVisualHat, int newVisualBackpack, int newVisualVest, int newVisualMask, int newVisualGlasses)
		{
			this.clothes.visualShirt = newVisualShirt;
			this.clothes.visualPants = newVisualPants;
			this.clothes.visualHat = newVisualHat;
			this.clothes.visualBackpack = newVisualBackpack;
			this.clothes.visualVest = newVisualVest;
			this.clothes.visualMask = newVisualMask;
			this.clothes.visualGlasses = newVisualGlasses;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00081E75 File Offset: 0x00080075
		public void clearVisuals()
		{
			this.updateVisuals(0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00081E84 File Offset: 0x00080084
		public void updateClothes(ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			this.clothes.shirt = newShirt;
			this.shirtQuality = newShirtQuality;
			this.shirtState = newShirtState;
			this.clothes.pants = newPants;
			this.pantsQuality = newPantsQuality;
			this.pantsState = newPantsState;
			this.clothes.hat = newHat;
			this.hatQuality = newHatQuality;
			this.hatState = newHatState;
			this.clothes.backpack = newBackpack;
			this.backpackQuality = newBackpackQuality;
			this.backpackState = newBackpackState;
			this.clothes.vest = newVest;
			this.vestQuality = newVestQuality;
			this.vestState = newVestState;
			this.clothes.mask = newMask;
			this.maskQuality = newMaskQuality;
			this.maskState = newMaskState;
			this.clothes.glasses = newGlasses;
			this.glassesQuality = newGlassesQuality;
			this.glassesState = newGlassesState;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00081F5C File Offset: 0x0008015C
		public void dropClothes()
		{
			if (this.shirt != 0)
			{
				ItemManager.dropItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), base.transform.position, false, true, true);
			}
			if (this.pants != 0)
			{
				ItemManager.dropItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), base.transform.position, false, true, true);
			}
			if (this.hat != 0)
			{
				ItemManager.dropItem(new Item(this.hat, 1, this.hatQuality, this.hatState), base.transform.position, false, true, true);
			}
			if (this.backpack != 0)
			{
				ItemManager.dropItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), base.transform.position, false, true, true);
			}
			if (this.vest != 0)
			{
				ItemManager.dropItem(new Item(this.vest, 1, this.vestQuality, this.vestState), base.transform.position, false, true, true);
			}
			if (this.mask != 0)
			{
				ItemManager.dropItem(new Item(this.mask, 1, this.maskQuality, this.maskState), base.transform.position, false, true, true);
			}
			if (this.glasses != 0)
			{
				ItemManager.dropItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), base.transform.position, false, true, true);
			}
			this.clearClothes();
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000820D4 File Offset: 0x000802D4
		public void clearClothes()
		{
			this.updateClothes(0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0]);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00082120 File Offset: 0x00080320
		public void updatePose()
		{
			string text;
			switch (this.pose)
			{
			case 0:
				text = "T";
				break;
			case 1:
				text = "Classic";
				break;
			case 2:
				text = "Lie";
				break;
			default:
				return;
			}
			if (this.anim != null)
			{
				this.anim.transform.localScale = new Vector3((float)(this.mirror ? -1 : 1), 1f, 1f);
				this.anim.Play(text);
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000821A4 File Offset: 0x000803A4
		public void updateState(byte[] state)
		{
			Block block = new Block(state);
			this._owner = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
			this._group = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
			this.clothes.skin = new Color32(210, 210, 210, byte.MaxValue);
			this.clothes.color = this.clothes.skin;
			this.clothes.visualShirt = block.readInt32();
			this.clothes.visualPants = block.readInt32();
			this.clothes.visualHat = block.readInt32();
			this.clothes.visualBackpack = block.readInt32();
			this.clothes.visualVest = block.readInt32();
			this.clothes.visualMask = block.readInt32();
			this.clothes.visualGlasses = block.readInt32();
			this.clothes.shirt = block.readUInt16();
			this.shirtQuality = block.readByte();
			this.clothes.pants = block.readUInt16();
			this.pantsQuality = block.readByte();
			this.clothes.hat = block.readUInt16();
			this.hatQuality = block.readByte();
			this.clothes.backpack = block.readUInt16();
			this.backpackQuality = block.readByte();
			this.clothes.vest = block.readUInt16();
			this.vestQuality = block.readByte();
			this.clothes.mask = block.readUInt16();
			this.maskQuality = block.readByte();
			this.clothes.glasses = block.readUInt16();
			this.glassesQuality = block.readByte();
			this.shirtState = block.readByteArray();
			this.pantsState = block.readByteArray();
			this.hatState = block.readByteArray();
			this.backpackState = block.readByteArray();
			this.vestState = block.readByteArray();
			this.maskState = block.readByteArray();
			this.glassesState = block.readByteArray();
			this.clothes.apply();
			this.setPose(block.readByte());
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000823DC File Offset: 0x000805DC
		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			Transform transform = base.transform.Find("Root");
			this.anim = transform.GetComponent<Animation>();
			this.clothes = transform.GetOrAddComponent<HumanClothes>();
			this.updateState(state);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008242A File Offset: 0x0008062A
		public override bool checkUseable()
		{
			return this.checkUpdate(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00082458 File Offset: 0x00080658
		public override void use()
		{
			if (!InputEx.GetKey(ControlsSettings.other))
			{
				PlayerUI.instance.mannequinUI.open(this);
				PlayerLifeUI.close();
				return;
			}
			if (Player.player.equipment.useable is UseableClothing)
			{
				this.ClientRequestUpdate(EMannequinUpdateMode.ADD);
				return;
			}
			this.ClientRequestUpdate(EMannequinUpdateMode.REMOVE);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000824AC File Offset: 0x000806AC
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000824DF File Offset: 0x000806DF
		public void ManualOnDestroy()
		{
			if (!Provider.isServer)
			{
				return;
			}
			this.dropClothes();
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000824EF File Offset: 0x000806EF
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePose(byte poseComp)
		{
			this.setPose(poseComp);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000824F8 File Offset: 0x000806F8
		internal void ClientSetPose(byte poseComp)
		{
			InteractableMannequin.SendPoseRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, poseComp);
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0008250C File Offset: 0x0008070C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceivePoseRequest(in ServerInvocationContext context, byte poseComp)
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
			if (this.checkUpdate(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				if (this.isObstructedByPlayers())
				{
					return;
				}
				byte x;
				byte y;
				ushort plant;
				BarricadeRegion barricadeRegion;
				if (BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
				{
					BarricadeManager.InternalSetMannequinPose(this, x, y, plant, poseComp);
				}
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000825B4 File Offset: 0x000807B4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveUpdate(byte[] state)
		{
			this.updateState(state);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000825BD File Offset: 0x000807BD
		internal void ClientRequestUpdate(EMannequinUpdateMode updateMode)
		{
			InteractableMannequin.SendUpdateRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, updateMode);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000825D4 File Offset: 0x000807D4
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveUpdateRequest(in ServerInvocationContext context, EMannequinUpdateMode updateMode)
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
			if (player.equipment.isBusy)
			{
				return;
			}
			if (player.equipment.HasValidUseable && !player.equipment.IsEquipAnimationFinished)
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
			if (this.isUpdatable && this.checkUpdate(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				switch (updateMode)
				{
				case EMannequinUpdateMode.COSMETICS:
					this.updateVisuals(player.clothing.visualShirt, player.clothing.visualPants, player.clothing.visualHat, player.clothing.visualBackpack, player.clothing.visualVest, player.clothing.visualMask, player.clothing.visualGlasses);
					if (this.shirt != 0)
					{
						player.inventory.forceAddItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), false);
					}
					if (this.pants != 0)
					{
						player.inventory.forceAddItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), false);
					}
					if (this.hat != 0)
					{
						player.inventory.forceAddItem(new Item(this.hat, 1, this.hatQuality, this.hatState), false);
					}
					if (this.backpack != 0)
					{
						player.inventory.forceAddItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), false);
					}
					if (this.vest != 0)
					{
						player.inventory.forceAddItem(new Item(this.vest, 1, this.vestQuality, this.vestState), false);
					}
					if (this.mask != 0)
					{
						player.inventory.forceAddItem(new Item(this.mask, 1, this.maskQuality, this.maskState), false);
					}
					if (this.glasses != 0)
					{
						player.inventory.forceAddItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), false);
					}
					this.clearClothes();
					break;
				case EMannequinUpdateMode.ADD:
				{
					if (!player.equipment.HasValidUseable || !player.equipment.IsEquipAnimationFinished || player.equipment.isBusy || player.equipment.asset == null || !(player.equipment.useable is UseableClothing))
					{
						return;
					}
					ItemJar item = player.inventory.getItem(player.equipment.equippedPage, player.inventory.getIndex(player.equipment.equippedPage, player.equipment.equipped_x, player.equipment.equipped_y));
					if (item == null || item.item == null)
					{
						return;
					}
					this.clearVisuals();
					switch (player.equipment.asset.type)
					{
					case EItemType.HAT:
						if (this.hat != 0)
						{
							player.inventory.forceAddItem(new Item(this.hat, 1, this.hatQuality, this.hatState), false);
						}
						this.clothes.hat = item.item.id;
						this.hatQuality = item.item.quality;
						this.hatState = item.item.state;
						break;
					case EItemType.PANTS:
						if (this.pants != 0)
						{
							player.inventory.forceAddItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), false);
						}
						this.clothes.pants = item.item.id;
						this.pantsQuality = item.item.quality;
						this.pantsState = item.item.state;
						break;
					case EItemType.SHIRT:
						if (this.shirt != 0)
						{
							player.inventory.forceAddItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), false);
						}
						this.clothes.shirt = item.item.id;
						this.shirtQuality = item.item.quality;
						this.shirtState = item.item.state;
						break;
					case EItemType.MASK:
						if (this.mask != 0)
						{
							player.inventory.forceAddItem(new Item(this.mask, 1, this.maskQuality, this.maskState), false);
						}
						this.clothes.mask = item.item.id;
						this.maskQuality = item.item.quality;
						this.maskState = item.item.state;
						break;
					case EItemType.BACKPACK:
						if (this.backpack != 0)
						{
							player.inventory.forceAddItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), false);
						}
						this.clothes.backpack = item.item.id;
						this.backpackQuality = item.item.quality;
						this.backpackState = item.item.state;
						break;
					case EItemType.VEST:
						if (this.vest != 0)
						{
							player.inventory.forceAddItem(new Item(this.vest, 1, this.vestQuality, this.vestState), false);
						}
						this.clothes.vest = item.item.id;
						this.vestQuality = item.item.quality;
						this.vestState = item.item.state;
						break;
					case EItemType.GLASSES:
						if (this.glasses != 0)
						{
							player.inventory.forceAddItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), false);
						}
						this.clothes.glasses = item.item.id;
						this.glassesQuality = item.item.quality;
						this.glassesState = item.item.state;
						break;
					default:
						return;
					}
					player.equipment.use();
					break;
				}
				case EMannequinUpdateMode.REMOVE:
					this.clearVisuals();
					if (this.shirt != 0)
					{
						player.inventory.forceAddItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), true, false);
					}
					if (this.pants != 0)
					{
						player.inventory.forceAddItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), true, false);
					}
					if (this.hat != 0)
					{
						player.inventory.forceAddItem(new Item(this.hat, 1, this.hatQuality, this.hatState), true, false);
					}
					if (this.backpack != 0)
					{
						player.inventory.forceAddItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), true, false);
					}
					if (this.vest != 0)
					{
						player.inventory.forceAddItem(new Item(this.vest, 1, this.vestQuality, this.vestState), true, false);
					}
					if (this.mask != 0)
					{
						player.inventory.forceAddItem(new Item(this.mask, 1, this.maskQuality, this.maskState), true, false);
					}
					if (this.glasses != 0)
					{
						player.inventory.forceAddItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), true, false);
					}
					this.clearClothes();
					break;
				case EMannequinUpdateMode.SWAP:
				{
					this.clearVisuals();
					ushort shirt = player.clothing.shirt;
					byte newShirtQuality = player.clothing.shirtQuality;
					byte[] newShirtState = player.clothing.shirtState;
					ushort pants = player.clothing.pants;
					byte newPantsQuality = player.clothing.pantsQuality;
					byte[] newPantsState = player.clothing.pantsState;
					ushort hat = player.clothing.hat;
					byte newHatQuality = player.clothing.hatQuality;
					byte[] newHatState = player.clothing.hatState;
					ushort backpack = player.clothing.backpack;
					byte newBackpackQuality = player.clothing.backpackQuality;
					byte[] newBackpackState = player.clothing.backpackState;
					ushort vest = player.clothing.vest;
					byte newVestQuality = player.clothing.vestQuality;
					byte[] newVestState = player.clothing.vestState;
					ushort mask = player.clothing.mask;
					byte newMaskQuality = player.clothing.maskQuality;
					byte[] newMaskState = player.clothing.maskState;
					ushort glasses = player.clothing.glasses;
					byte newGlassesQuality = player.clothing.glassesQuality;
					byte[] newGlassesState = player.clothing.glassesState;
					player.clothing.updateClothes(this.shirt, this.shirtQuality, this.shirtState, this.pants, this.pantsQuality, this.pantsState, this.hat, this.hatQuality, this.hatState, this.backpack, this.backpackQuality, this.backpackState, this.vest, this.vestQuality, this.vestState, this.mask, this.maskQuality, this.maskState, this.glasses, this.glassesQuality, this.glassesState);
					this.updateClothes(shirt, newShirtQuality, newShirtState, pants, newPantsQuality, newPantsState, hat, newHatQuality, newHatState, backpack, newBackpackQuality, newBackpackState, vest, newVestQuality, newVestState, mask, newMaskQuality, newMaskState, glasses, newGlassesQuality, newGlassesState);
					break;
				}
				default:
					return;
				}
				this.rebuildState();
				byte[] state = barricadeRegion.FindBarricadeByRootFast(base.transform).serversideData.barricade.state;
				InteractableMannequin.SendUpdate.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), state);
				EffectAsset effectAsset = InteractableMannequin.SleeveRef.Find();
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						position = base.transform.position,
						relevantDistance = EffectManager.SMALL
					});
				}
			}
		}

		// Token: 0x04001085 RID: 4229
		private CSteamID _owner;

		// Token: 0x04001086 RID: 4230
		private CSteamID _group;

		// Token: 0x04001087 RID: 4231
		private bool isLocked;

		// Token: 0x04001088 RID: 4232
		public byte pose_comp;

		// Token: 0x04001089 RID: 4233
		public bool mirror;

		// Token: 0x0400108A RID: 4234
		public byte pose;

		// Token: 0x0400108B RID: 4235
		private float updated;

		// Token: 0x0400108C RID: 4236
		private Animation anim;

		// Token: 0x0400108E RID: 4238
		public byte shirtQuality;

		// Token: 0x0400108F RID: 4239
		public byte pantsQuality;

		// Token: 0x04001090 RID: 4240
		public byte hatQuality;

		// Token: 0x04001091 RID: 4241
		public byte backpackQuality;

		// Token: 0x04001092 RID: 4242
		public byte vestQuality;

		// Token: 0x04001093 RID: 4243
		public byte maskQuality;

		// Token: 0x04001094 RID: 4244
		public byte glassesQuality;

		// Token: 0x04001095 RID: 4245
		public byte[] shirtState;

		// Token: 0x04001096 RID: 4246
		public byte[] pantsState;

		// Token: 0x04001097 RID: 4247
		public byte[] hatState;

		// Token: 0x04001098 RID: 4248
		public byte[] backpackState;

		// Token: 0x04001099 RID: 4249
		public byte[] vestState;

		// Token: 0x0400109A RID: 4250
		public byte[] maskState;

		// Token: 0x0400109B RID: 4251
		public byte[] glassesState;

		// Token: 0x0400109C RID: 4252
		internal static readonly ClientInstanceMethod<byte> SendPose = ClientInstanceMethod<byte>.Get(typeof(InteractableMannequin), "ReceivePose");

		// Token: 0x0400109D RID: 4253
		private static readonly ServerInstanceMethod<byte> SendPoseRequest = ServerInstanceMethod<byte>.Get(typeof(InteractableMannequin), "ReceivePoseRequest");

		// Token: 0x0400109E RID: 4254
		private static readonly ClientInstanceMethod<byte[]> SendUpdate = ClientInstanceMethod<byte[]>.Get(typeof(InteractableMannequin), "ReceiveUpdate");

		// Token: 0x0400109F RID: 4255
		private static readonly ServerInstanceMethod<EMannequinUpdateMode> SendUpdateRequest = ServerInstanceMethod<EMannequinUpdateMode>.Get(typeof(InteractableMannequin), "ReceiveUpdateRequest");

		// Token: 0x040010A0 RID: 4256
		private static readonly AssetReference<EffectAsset> SleeveRef = new AssetReference<EffectAsset>("704906b407fe4cb9b4a193ab7447d784");
	}
}
