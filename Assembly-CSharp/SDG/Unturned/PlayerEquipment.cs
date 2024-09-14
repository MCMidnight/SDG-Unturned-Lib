using System;
using System.Collections.Generic;
using SDG.NetTransport;
using SDG.Provider;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using Unturned.SystemEx;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x0200061C RID: 1564
	public class PlayerEquipment : PlayerCaller
	{
		/// <summary>
		/// Invoked from tellEquip after change.
		/// </summary>
		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06003217 RID: 12823 RVA: 0x000DEB50 File Offset: 0x000DCD50
		// (remove) Token: 0x06003218 RID: 12824 RVA: 0x000DEB84 File Offset: 0x000DCD84
		public static event Action<PlayerEquipment> OnUseableChanged_Global;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06003219 RID: 12825 RVA: 0x000DEBB8 File Offset: 0x000DCDB8
		// (remove) Token: 0x0600321A RID: 12826 RVA: 0x000DEBEC File Offset: 0x000DCDEC
		public static event Action<PlayerEquipment> OnInspectingUseable_Global;

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x000DEC1F File Offset: 0x000DCE1F
		public ushort itemID
		{
			get
			{
				ItemAsset asset = this.asset;
				if (asset == null)
				{
					return 0;
				}
				return asset.id;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x000DEC3B File Offset: 0x000DCE3B
		// (set) Token: 0x0600321C RID: 12828 RVA: 0x000DEC32 File Offset: 0x000DCE32
		public byte[] state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x000DEC43 File Offset: 0x000DCE43
		// (set) Token: 0x0600321F RID: 12831 RVA: 0x000DEC56 File Offset: 0x000DCE56
		public byte quality
		{
			get
			{
				if (this.isTurret)
				{
					return 100;
				}
				return this._quality;
			}
			set
			{
				if (this.isTurret)
				{
					return;
				}
				this._quality = value;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003220 RID: 12832 RVA: 0x000DEC68 File Offset: 0x000DCE68
		public Transform firstModel
		{
			get
			{
				return this._firstModel;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x000DEC70 File Offset: 0x000DCE70
		public Transform thirdModel
		{
			get
			{
				return this._thirdModel;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000DEC78 File Offset: 0x000DCE78
		public Transform characterModel
		{
			get
			{
				return this._characterModel;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003223 RID: 12835 RVA: 0x000DEC80 File Offset: 0x000DCE80
		public ItemAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x000DEC88 File Offset: 0x000DCE88
		public Useable useable
		{
			get
			{
				return this._useable;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x000DEC90 File Offset: 0x000DCE90
		public Transform thirdPrimaryMeleeSlot
		{
			get
			{
				return this._thirdPrimaryMeleeSlot;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x000DEC98 File Offset: 0x000DCE98
		public Transform thirdPrimaryLargeGunSlot
		{
			get
			{
				return this._thirdPrimaryLargeGunSlot;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x000DECA0 File Offset: 0x000DCEA0
		public Transform thirdPrimarySmallGunSlot
		{
			get
			{
				return this._thirdPrimarySmallGunSlot;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x000DECA8 File Offset: 0x000DCEA8
		public Transform thirdSecondaryMeleeSlot
		{
			get
			{
				return this._thirdSecondaryMeleeSlot;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000DECB0 File Offset: 0x000DCEB0
		public Transform thirdSecondaryGunSlot
		{
			get
			{
				return this._thirdSecondaryGunSlot;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x000DECB8 File Offset: 0x000DCEB8
		public Transform characterPrimaryMeleeSlot
		{
			get
			{
				return this._characterPrimaryMeleeSlot;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x000DECC0 File Offset: 0x000DCEC0
		public Transform characterPrimaryLargeGunSlot
		{
			get
			{
				return this._characterPrimaryLargeGunSlot;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x000DECC8 File Offset: 0x000DCEC8
		public Transform characterPrimarySmallGunSlot
		{
			get
			{
				return this._characterPrimarySmallGunSlot;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000DECD0 File Offset: 0x000DCED0
		public Transform characterSecondaryMeleeSlot
		{
			get
			{
				return this._characterSecondaryMeleeSlot;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x000DECD8 File Offset: 0x000DCED8
		public Transform characterSecondaryGunSlot
		{
			get
			{
				return this._characterSecondaryGunSlot;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000DECE0 File Offset: 0x000DCEE0
		public Transform firstLeftHook
		{
			get
			{
				return this._firstLeftHook;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000DECE8 File Offset: 0x000DCEE8
		public Transform firstRightHook
		{
			get
			{
				return this._firstRightHook;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x000DECF0 File Offset: 0x000DCEF0
		public Transform thirdLeftHook
		{
			get
			{
				return this._thirdLeftHook;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x000DECF8 File Offset: 0x000DCEF8
		public Transform thirdRightHook
		{
			get
			{
				return this._thirdRightHook;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000DED00 File Offset: 0x000DCF00
		public Transform characterLeftHook
		{
			get
			{
				return this._characterLeftHook;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x000DED08 File Offset: 0x000DCF08
		public Transform characterRightHook
		{
			get
			{
				return this._characterRightHook;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x000DED10 File Offset: 0x000DCF10
		public HotkeyInfo[] hotkeys
		{
			get
			{
				return this._hotkeys;
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000DED18 File Offset: 0x000DCF18
		public bool isItemHotkeyed(byte page, byte index, ItemJar jar, out byte button)
		{
			if (page < PlayerInventory.SLOTS)
			{
				button = page;
				return true;
			}
			byte b = 0;
			while ((int)b < this.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
				if (hotkeyInfo.page == page && hotkeyInfo.x == jar.x && hotkeyInfo.y == jar.y && hotkeyInfo.id == jar.item.id)
				{
					button = b + 2;
					return true;
				}
				b += 1;
			}
			button = 0;
			return false;
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x000DED96 File Offset: 0x000DCF96
		public bool HasValidUseable
		{
			get
			{
				return this.useable != null;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x000DEDA4 File Offset: 0x000DCFA4
		public bool IsEquipAnimationFinished
		{
			get
			{
				if (base.channel.IsLocalPlayer || Provider.isServer)
				{
					return base.player.input.simulation - this.equipAnimStartedFrame >= this.equipAnimLengthFrames;
				}
				return Time.timeAsDouble >= this.equipAnimCompletedTime;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x000DEDF8 File Offset: 0x000DCFF8
		// (set) Token: 0x0600323A RID: 12858 RVA: 0x000DEE00 File Offset: 0x000DD000
		public bool isTurret { get; private set; }

		/// <summary>
		/// Does equipped useable have a menu open?
		/// If so pause menu, dashboard, and other menus cannot be opened.
		/// </summary>
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x000DEE09 File Offset: 0x000DD009
		public bool isUseableShowingMenu
		{
			get
			{
				return this.useable != null && this.useable.isUseableShowingMenu;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600323C RID: 12860 RVA: 0x000DEE26 File Offset: 0x000DD026
		public byte equippedPage
		{
			get
			{
				return this._equippedPage;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x000DEE2E File Offset: 0x000DD02E
		public byte equipped_x
		{
			get
			{
				return this._equipped_x;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x000DEE36 File Offset: 0x000DD036
		public byte equipped_y
		{
			get
			{
				return this._equipped_y;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x000DEE3E File Offset: 0x000DD03E
		[Obsolete]
		public bool primary
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x000DEE41 File Offset: 0x000DD041
		[Obsolete]
		public bool secondary
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x000DEE44 File Offset: 0x000DD044
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x000DEE4C File Offset: 0x000DD04C
		public float lastPunching { get; private set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x000DEE55 File Offset: 0x000DD055
		public bool isInspecting
		{
			get
			{
				return Time.realtimeSinceStartup - PlayerEquipment.lastInspect < PlayerEquipment.inspectTime;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x000DEE6C File Offset: 0x000DD06C
		public bool canInspect
		{
			get
			{
				return this.HasValidUseable && this.IsEquipAnimationFinished && !this.isBusy && base.player.animator.checkExists("Inspect") && !this.isInspecting && this.useable.canInspect;
			}
		}

		/// <summary>
		/// Get ragdoll effect to use when the current weapon deals damage.
		/// </summary>
		// Token: 0x06003245 RID: 12869 RVA: 0x000DEEBD File Offset: 0x000DD0BD
		public ERagdollEffect getUseableRagdollEffect()
		{
			if (base.player.clothing.isMythic)
			{
				return this.skinRagdollEffect;
			}
			return ERagdollEffect.NONE;
		}

		/// <summary>
		/// It should be safe to call this immediately because hotkeys are loaded in InitializePlayer.
		/// </summary>
		// Token: 0x06003246 RID: 12870 RVA: 0x000DEEDC File Offset: 0x000DD0DC
		public void ServerBindItemHotkey(byte hotkeyIndex, ItemAsset expectedItem, byte page, byte x, byte y)
		{
			PlayerEquipment.SendItemHotkeySuggestion.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), hotkeyIndex, (expectedItem != null) ? expectedItem.GUID : Guid.Empty, page, x, y);
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000DEF1C File Offset: 0x000DD11C
		public void ServerClearItemHotkey(byte hotkeyIndex)
		{
			PlayerEquipment.SendItemHotkeySuggestion.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), hotkeyIndex, Guid.Empty, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000DEF5C File Offset: 0x000DD15C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveItemHotkeySuggeston(in ClientInvocationContext context, byte hotkeyIndex, Guid expectedAssetGuid, byte page, byte x, byte y)
		{
			if (this.hotkeys == null || (int)hotkeyIndex >= this.hotkeys.Length)
			{
				return;
			}
			ushort num = 0;
			if (!GuidExtension.IsEmpty(expectedAssetGuid))
			{
				ItemAsset itemAsset = Assets.find<ItemAsset>(expectedAssetGuid);
				if (itemAsset != null)
				{
					num = itemAsset.id;
				}
				else
				{
					UnturnedLog.warn(string.Format("Unable to use server item hotkey suggestion because asset was not found ({0})", expectedAssetGuid));
				}
			}
			if (num == 0)
			{
				page = byte.MaxValue;
				x = byte.MaxValue;
				y = byte.MaxValue;
			}
			HotkeyInfo hotkeyInfo = this.hotkeys[(int)hotkeyIndex];
			hotkeyInfo.id = num;
			hotkeyInfo.page = page;
			hotkeyInfo.x = x;
			hotkeyInfo.y = y;
			this.ClearDuplicateHotkeys((int)hotkeyIndex);
			HotkeysUpdated hotkeysUpdated = this.onHotkeysUpdated;
			if (hotkeysUpdated == null)
			{
				return;
			}
			hotkeysUpdated();
		}

		/// <summary>
		/// Prevent multiple hotkeys from referencing the same item.
		/// </summary>
		// Token: 0x06003249 RID: 12873 RVA: 0x000DF004 File Offset: 0x000DD204
		private void ClearDuplicateHotkeys(int newHotkeyIndex)
		{
			HotkeyInfo hotkeyInfo = this.hotkeys[newHotkeyIndex];
			for (int i = 0; i < this.hotkeys.Length; i++)
			{
				if (i != newHotkeyIndex)
				{
					HotkeyInfo hotkeyInfo2 = this.hotkeys[i];
					if (hotkeyInfo2.page == hotkeyInfo.page && hotkeyInfo2.x == hotkeyInfo.x && hotkeyInfo2.y == hotkeyInfo.y)
					{
						hotkeyInfo2.id = 0;
						hotkeyInfo2.page = byte.MaxValue;
						hotkeyInfo2.x = byte.MaxValue;
						hotkeyInfo2.y = byte.MaxValue;
					}
				}
			}
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000DF08C File Offset: 0x000DD28C
		public bool getUseableStatTrackerValue(out EStatTrackerType type, out int kills)
		{
			SteamPlayer owner = base.channel.owner;
			ItemAsset asset = this.asset;
			return owner.getStatTrackerValue((asset != null) ? asset.sharedSkinLookupID : this.itemID, out type, out kills);
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000DF0B8 File Offset: 0x000DD2B8
		protected bool getSlot0StatTrackerValue(out EStatTrackerType type, out int kills)
		{
			ItemJar item = base.player.inventory.getItem(0, 0);
			if (item != null)
			{
				SteamPlayer owner = base.channel.owner;
				ItemAsset asset = item.GetAsset();
				return owner.getStatTrackerValue((asset != null) ? asset.sharedSkinLookupID : item.item.id, out type, out kills);
			}
			type = EStatTrackerType.NONE;
			kills = -1;
			return false;
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000DF114 File Offset: 0x000DD314
		protected bool getSlot1StatTrackerValue(out EStatTrackerType type, out int kills)
		{
			ItemJar item = base.player.inventory.getItem(1, 0);
			if (item != null)
			{
				SteamPlayer owner = base.channel.owner;
				ItemAsset asset = item.GetAsset();
				return owner.getStatTrackerValue((asset != null) ? asset.sharedSkinLookupID : item.item.id, out type, out kills);
			}
			type = EStatTrackerType.NONE;
			kills = -1;
			return false;
		}

		/// <summary>
		/// Left-handed characters need the stat tracker to be flipped on the X axis so that the text reads properly.
		/// ItemTool doesn't know about left/right handedness, so for the moment that's handled here because only players need this fixed up.
		/// </summary>
		// Token: 0x0600324D RID: 12877 RVA: 0x000DF170 File Offset: 0x000DD370
		protected void fixStatTrackerHookScale(Transform itemModelTransform)
		{
			if (!base.channel.owner.IsLeftHanded)
			{
				return;
			}
			Transform transform = itemModelTransform.Find("Stat_Tracker");
			if (!transform)
			{
				return;
			}
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000DF1D4 File Offset: 0x000DD3D4
		private void ApplyEquipableLocalScale(ItemAsset asset, Transform itemModelTransform)
		{
			if (!base.channel.owner.IsLeftHanded || asset.shouldLeftHandedCharactersMirrorEquippedItem)
			{
				itemModelTransform.localScale = Vector3.one;
				return;
			}
			itemModelTransform.localScale = new Vector3(-1f, 1f, 1f);
		}

		/// <summary>
		/// Match stat tracker gameobject's isActive to whether skins are visible.
		/// </summary>
		// Token: 0x0600324F RID: 12879 RVA: 0x000DF224 File Offset: 0x000DD424
		protected void syncStatTrackTrackerVisibility(Transform itemModelTransform)
		{
			if (itemModelTransform == null)
			{
				return;
			}
			Transform transform = itemModelTransform.Find("Stat_Tracker");
			if (!transform)
			{
				return;
			}
			transform.gameObject.SetActive(base.player.clothing.isSkinned);
		}

		/// <summary>
		/// Match all stat tracker visibilities to whether skins are visible.
		/// </summary>
		// Token: 0x06003250 RID: 12880 RVA: 0x000DF26C File Offset: 0x000DD46C
		protected void syncAllStatTrackerVisibility()
		{
			this.syncStatTrackTrackerVisibility(this.firstModel);
			this.syncStatTrackTrackerVisibility(this.thirdModel);
			this.syncStatTrackTrackerVisibility(this.characterModel);
			if (this.thirdSlots != null)
			{
				foreach (Transform itemModelTransform in this.thirdSlots)
				{
					this.syncStatTrackTrackerVisibility(itemModelTransform);
				}
			}
			if (this.characterSlots != null)
			{
				foreach (Transform itemModelTransform2 in this.characterSlots)
				{
					this.syncStatTrackTrackerVisibility(itemModelTransform2);
				}
			}
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000DF2F0 File Offset: 0x000DD4F0
		public void inspect()
		{
			base.player.animator.setAnimationSpeed("Inspect", 1f);
			PlayerEquipment.lastInspect = Time.realtimeSinceStartup;
			PlayerEquipment.inspectTime = base.player.animator.GetAnimationLength("Inspect", true);
			base.player.animator.play("Inspect", false);
			foreach (UseableEventHook useableEventHook in this.EnumerateEventComponents())
			{
				UnityEvent onInspectStarted = useableEventHook.OnInspectStarted;
				if (onInspectStarted != null)
				{
					onInspectStarted.TryInvoke(this);
				}
			}
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000DF39C File Offset: 0x000DD59C
		internal void InvokeOnInspectingUseable()
		{
			PlayerEquipment.OnInspectingUseable_Global.TryInvoke("OnInspectingUseable_Global", this);
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000DF3AE File Offset: 0x000DD5AE
		public void uninspect()
		{
			base.player.animator.setAnimationSpeed("Inspect", float.MaxValue);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000DF3CA File Offset: 0x000DD5CA
		public bool checkSelection(byte page)
		{
			return page == this.equippedPage;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000DF3D5 File Offset: 0x000DD5D5
		public bool checkSelection(byte page, byte x, byte y)
		{
			return page == this.equippedPage && x == this.equipped_x && y == this.equipped_y;
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000DF3F4 File Offset: 0x000DD5F4
		public void applySkinVisual()
		{
			if (this.firstModel != null && this.firstSkinned != base.player.clothing.isSkinned)
			{
				this.firstSkinned = base.player.clothing.isSkinned;
				if (this.tempFirstMaterial != null)
				{
					Attachments component = this.firstModel.GetComponent<Attachments>();
					if (component != null)
					{
						component.isSkinned = this.firstSkinned;
						component.applyVisual();
					}
					if (this.tempFirstMesh.Count > 0)
					{
						HighlighterTool.remesh(this.firstModel, this.tempFirstMesh, this.tempFirstMesh);
					}
					HighlighterTool.rematerialize(this.firstModel, this.tempFirstMaterial, out this.tempFirstMaterial);
				}
			}
			if (this.thirdModel != null && this.thirdSkinned != base.player.clothing.isSkinned)
			{
				this.thirdSkinned = base.player.clothing.isSkinned;
				if (this.tempThirdMaterial != null)
				{
					Attachments component2 = this.thirdModel.GetComponent<Attachments>();
					if (component2 != null)
					{
						component2.isSkinned = this.thirdSkinned;
						component2.applyVisual();
					}
					if (this.tempThirdMesh.Count > 0)
					{
						HighlighterTool.remesh(this.thirdModel, this.tempThirdMesh, this.tempThirdMesh);
					}
					HighlighterTool.rematerialize(this.thirdModel, this.tempThirdMaterial, out this.tempThirdMaterial);
				}
			}
			if (this.characterModel != null && this.characterSkinned != base.player.clothing.isSkinned)
			{
				this.characterSkinned = base.player.clothing.isSkinned;
				if (this.tempCharacterMaterial != null)
				{
					Attachments component3 = this.characterModel.GetComponent<Attachments>();
					if (component3 != null)
					{
						component3.isSkinned = this.characterSkinned;
						component3.applyVisual();
					}
					if (this.tempCharacterMesh.Count > 0)
					{
						HighlighterTool.remesh(this.characterModel, this.tempCharacterMesh, this.tempCharacterMesh);
					}
					HighlighterTool.rematerialize(this.characterModel, this.tempCharacterMaterial, out this.tempCharacterMaterial);
				}
			}
			if (this.thirdSlots != null)
			{
				byte b = 0;
				while ((int)b < this.thirdSlots.Length)
				{
					if (this.thirdSlots[(int)b] != null && this.thirdSkinneds[(int)b] != base.player.clothing.isSkinned)
					{
						this.thirdSkinneds[(int)b] = base.player.clothing.isSkinned;
						if (this.tempThirdMaterials[(int)b] != null)
						{
							Attachments component4 = this.thirdSlots[(int)b].GetComponent<Attachments>();
							if (component4 != null)
							{
								component4.isSkinned = this.thirdSkinneds[(int)b];
								component4.applyVisual();
							}
							if (this.tempThirdMeshes[(int)b].Count > 0)
							{
								HighlighterTool.remesh(this.thirdSlots[(int)b], this.tempThirdMeshes[(int)b], this.tempThirdMeshes[(int)b]);
							}
							HighlighterTool.rematerialize(this.thirdSlots[(int)b], this.tempThirdMaterials[(int)b], out this.tempThirdMaterials[(int)b]);
						}
					}
					if (this.characterSlots != null && this.characterSlots[(int)b] != null && this.characterSkinneds[(int)b] != base.player.clothing.isSkinned)
					{
						this.characterSkinneds[(int)b] = base.player.clothing.isSkinned;
						if (this.tempCharacterMaterials[(int)b] != null)
						{
							Attachments component5 = this.characterSlots[(int)b].GetComponent<Attachments>();
							if (component5 != null)
							{
								component5.isSkinned = this.characterSkinneds[(int)b];
								component5.applyVisual();
							}
							if (this.tempCharacterMeshes[(int)b].Count > 0)
							{
								HighlighterTool.remesh(this.characterSlots[(int)b], this.tempCharacterMeshes[(int)b], this.tempCharacterMeshes[(int)b]);
							}
							HighlighterTool.rematerialize(this.characterSlots[(int)b], this.tempCharacterMaterials[(int)b], out this.tempCharacterMaterials[(int)b]);
						}
					}
					b += 1;
				}
			}
			this.syncAllStatTrackerVisibility();
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000DF7FC File Offset: 0x000DD9FC
		public void applyMythicVisual()
		{
			if (this.firstMythic != null)
			{
				this.firstMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.thirdMythic != null)
			{
				this.thirdMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.characterMythic != null)
			{
				this.characterMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.thirdSlots != null)
			{
				byte b = 0;
				while ((int)b < this.thirdSlots.Length)
				{
					if (this.thirdMythics[(int)b] != null)
					{
						this.thirdMythics[(int)b].IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					if (this.characterSlots != null && this.characterMythics[(int)b] != null)
					{
						this.characterMythics[(int)b].IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					b += 1;
				}
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000DF974 File Offset: 0x000DDB74
		private void updateSlot(byte slot, ushort id, byte[] state)
		{
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000DF981 File Offset: 0x000DDB81
		[Obsolete]
		public void askToggleVision(CSteamID steamID)
		{
			this.ReceiveToggleVisionRequest();
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x000DF98C File Offset: 0x000DDB8C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askToggleVision")]
		public void ReceiveToggleVisionRequest()
		{
			if (!this.hasVision)
			{
				return;
			}
			if (base.player.clothing.glassesState.Length != 1)
			{
				return;
			}
			PlayerEquipment.SendToggleVision.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections());
			if (base.player.clothing.glassesAsset != null)
			{
				if (base.player.clothing.glassesAsset.vision == ELightingVision.HEADLAMP)
				{
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
				if (base.player.clothing.glassesAsset.vision == ELightingVision.CIVILIAN || base.player.clothing.glassesAsset.vision == ELightingVision.MILITARY)
				{
					EffectAsset effectAsset = PlayerEquipment.BeepRef.Find();
					if (effectAsset != null)
					{
						EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
						{
							relevantDistance = EffectManager.SMALL,
							position = base.transform.position
						});
					}
				}
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000DFA79 File Offset: 0x000DDC79
		[Obsolete]
		public void tellToggleVision(CSteamID steamID)
		{
			this.ReceiveToggleVision();
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x000DFA84 File Offset: 0x000DDC84
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellToggleVision")]
		public void ReceiveToggleVision()
		{
			if (!this.hasVision)
			{
				return;
			}
			if (base.player.clothing.glassesState.Length != 1)
			{
				return;
			}
			base.player.clothing.glassesState[0] = ((base.player.clothing.glassesState[0] == 0) ? 1 : 0);
			this.updateVision();
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000DFAE1 File Offset: 0x000DDCE1
		[Obsolete]
		public void tellSlot(CSteamID steamID, byte slot, ushort id, byte[] state)
		{
			this.ReceiveSlot(slot, id, state);
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000DFAED File Offset: 0x000DDCED
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSlot")]
		public void ReceiveSlot(byte slot, ushort id, byte[] state)
		{
			this.updateSlot(slot, id, state);
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000DFAF8 File Offset: 0x000DDCF8
		[Obsolete]
		public void tellUpdateStateTemp(CSteamID steamID, byte[] newState)
		{
			this.ReceiveUpdateStateTemp(newState);
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000DFB04 File Offset: 0x000DDD04
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateStateTemp")]
		public void ReceiveUpdateStateTemp(byte[] newState)
		{
			this._state = newState;
			if (this.useable != null)
			{
				try
				{
					this.useable.updateState(this.state);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during ReceiveUpdateStateTemp.updateState:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
			}
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x000DFB6C File Offset: 0x000DDD6C
		[Obsolete]
		public void tellUpdateState(CSteamID steamID, byte page, byte index, byte[] newState)
		{
			this.ReceiveUpdateState(page, index, newState);
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x000DFB78 File Offset: 0x000DDD78
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateState")]
		public void ReceiveUpdateState(byte page, byte index, byte[] newState)
		{
			if (this.thirdSlots == null)
			{
				return;
			}
			this._state = newState;
			if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
			{
				this.updateSlot(this.slot, this.itemID, newState);
				this.thirdSlots[(int)this.slot].gameObject.SetActive(false);
				if (this.characterSlots != null)
				{
					this.characterSlots[(int)this.slot].gameObject.SetActive(false);
				}
			}
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				base.player.inventory.updateState(page, index, this.state);
			}
			if (this.useable != null)
			{
				try
				{
					this.useable.updateState(this.state);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during tellUpdateState.updateState:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
			}
			if (this.characterModel != null)
			{
				Object.Destroy(this.characterModel.gameObject);
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.itemID) as ItemAsset;
				if (itemAsset != null)
				{
					int num = 0;
					ushort id = 0;
					ushort num2 = 0;
					bool flag = itemAsset != null && itemAsset.sharedSkinLookupID != itemAsset.id;
					ushort num3 = flag ? itemAsset.sharedSkinLookupID : itemAsset.id;
					if (base.channel.owner.skinItems != null && base.channel.owner.itemSkins != null && base.channel.owner.itemSkins.TryGetValue(num3, ref num))
					{
						if (!flag || itemAsset.SharedSkinShouldApplyVisuals)
						{
							id = Provider.provider.economyService.getInventorySkinID(num);
						}
						num2 = Provider.provider.economyService.getInventoryMythicID(num);
						if (num2 == 0)
						{
							num2 = base.channel.owner.getParticleEffectForItemDef(num);
						}
					}
					SkinAsset skinAsset = Assets.find(EAssetType.SKIN, id) as SkinAsset;
					if (this.slot != 0)
					{
						byte b = this.slot;
					}
					GameObject prefabOverride = (itemAsset.equipablePrefab != null) ? itemAsset.equipablePrefab : itemAsset.item;
					this._characterModel = ItemTool.getItem(100, this.state, false, itemAsset, skinAsset, this.tempCharacterMesh, out this.tempCharacterMaterial, new GetStatTrackerValueHandler(this.getUseableStatTrackerValue), prefabOverride);
					this.fixStatTrackerHookScale(this._characterModel);
					this.syncStatTrackTrackerVisibility(this._characterModel);
					this.characterEventComponent = this._characterModel.GetComponent<UseableEventHook>();
					Transform parent;
					switch (itemAsset.EquipableModelParent)
					{
					default:
						parent = this.characterRightHook;
						break;
					case EEquipableModelParent.LeftHook:
						parent = this.characterLeftHook;
						break;
					case EEquipableModelParent.Spine:
						parent = this._characterSpine;
						break;
					case EEquipableModelParent.SpineHook:
						parent = this._characterSpineHook;
						break;
					}
					this.characterModel.transform.parent = parent;
					this.characterModel.localPosition = Vector3.zero;
					this.characterModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
					this.characterModel.localScale = Vector3.one;
					this.characterModel.gameObject.AddComponent<Rigidbody>();
					this.characterModel.GetComponent<Rigidbody>().useGravity = false;
					this.characterModel.GetComponent<Rigidbody>().isKinematic = true;
					if (num2 != 0)
					{
						this.characterMythic = ItemTool.ApplyMythicalEffect(this.characterModel, num2, EEffectType.THIRD);
					}
					else
					{
						this.characterMythic = null;
					}
					this.characterSkinned = true;
					this.applySkinVisual();
					if (this.characterMythic != null)
					{
						this.characterMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
				}
			}
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x000DFF50 File Offset: 0x000DE150
		[Obsolete]
		public void tellEquip(CSteamID steamID, byte page, byte x, byte y, ushort id, byte newQuality, byte[] newState)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveEquip(page, x, y, (asset != null) ? asset.GUID : Guid.Empty, newQuality, newState, default(NetId));
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x000DFF90 File Offset: 0x000DE190
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEquip")]
		public void ReceiveEquip(byte page, byte x, byte y, Guid newAssetGuid, byte newQuality, byte[] newState, NetId useableNetId)
		{
			if (this.thirdSlots == null)
			{
				return;
			}
			if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
			{
				this.thirdSlots[(int)this.slot].gameObject.SetActive(true);
				if (this.characterSlots != null)
				{
					this.characterSlots[(int)this.slot].gameObject.SetActive(true);
				}
			}
			this.slot = page;
			if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
			{
				this.thirdSlots[(int)this.slot].gameObject.SetActive(false);
				if (this.characterSlots != null)
				{
					this.characterSlots[(int)this.slot].gameObject.SetActive(false);
				}
			}
			if (this.useable != null)
			{
				try
				{
					this.useable.dequip();
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during tellEquip.dequip:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
				this._useable.ReleaseNetId();
				this.useable.hideFlags |= HideFlags.NotEditable;
				Object.Destroy(this.useable);
				this._useable = null;
				base.channel.markDirty();
			}
			this.firstEventComponent = null;
			this.thirdEventComponent = null;
			this.characterEventComponent = null;
			this.skinRagdollEffect = ERagdollEffect.NONE;
			if (this.firstModel != null)
			{
				Object.Destroy(this.firstModel.gameObject);
			}
			this.firstSkinned = false;
			this.tempFirstMaterial = null;
			this.firstMythic = null;
			if (this.thirdModel != null)
			{
				Object.Destroy(this.thirdModel.gameObject);
			}
			this.thirdSkinned = false;
			this.tempThirdMaterial = null;
			this.thirdMythic = null;
			if (this.characterModel != null)
			{
				Object.Destroy(this.characterModel.gameObject);
			}
			this.characterSkinned = false;
			this.tempCharacterMaterial = null;
			this.characterMythic = null;
			if (this.asset != null && this.asset.animations != null && this.asset.animations.Length != 0)
			{
				for (int i = 0; i < this.asset.animations.Length; i++)
				{
					base.player.animator.removeAnimation(this.asset.animations[i]);
				}
			}
			this.isBusy = false;
			if (GuidExtension.IsEmpty(newAssetGuid))
			{
				this._equippedPage = byte.MaxValue;
				this._equipped_x = byte.MaxValue;
				this._equipped_y = byte.MaxValue;
				this._asset = null;
				PlayerEquipment.OnUseableChanged_Global.TryInvoke("OnUseableChanged_Global", this);
				return;
			}
			this._equippedPage = page;
			this._equipped_x = x;
			this._equipped_y = y;
			this._asset = (Assets.find(newAssetGuid) as ItemAsset);
			if (this.asset != null && this.asset.useableType != null)
			{
				this.quality = newQuality;
				this._state = newState;
				int num = 0;
				ushort id = 0;
				ushort num2 = 0;
				bool flag = this.asset != null && this.asset.sharedSkinLookupID != this.asset.id;
				ushort num3 = flag ? this.asset.sharedSkinLookupID : this.asset.id;
				if (base.channel.owner.skinItems != null && base.channel.owner.itemSkins != null && base.channel.owner.itemSkins.TryGetValue(num3, ref num))
				{
					if (!flag || this.asset.SharedSkinShouldApplyVisuals)
					{
						id = Provider.provider.economyService.getInventorySkinID(num);
					}
					num2 = Provider.provider.economyService.getInventoryMythicID(num);
					if (num2 == 0)
					{
						num2 = base.channel.owner.getParticleEffectForItemDef(num);
					}
				}
				SkinAsset skinAsset = Assets.find(EAssetType.SKIN, id) as SkinAsset;
				this.skinRagdollEffect = ERagdollEffect.NONE;
				if (!base.channel.owner.getRagdollEffect(this.asset.sharedSkinLookupID, out this.skinRagdollEffect) && skinAsset != null)
				{
					this.skinRagdollEffect = skinAsset.ragdollEffect;
				}
				GameObject prefabOverride = (this.asset.equipablePrefab != null) ? this.asset.equipablePrefab : this.asset.item;
				if (base.channel.IsLocalPlayer)
				{
					ClientAssetIntegrity.QueueRequest(this._asset);
					this._firstModel = ItemTool.InstantiateItem(this.quality, this.state, true, this.asset, skinAsset, true, this.tempFirstMesh, out this.tempFirstMaterial, new GetStatTrackerValueHandler(this.getUseableStatTrackerValue), prefabOverride);
					this.fixStatTrackerHookScale(this._firstModel);
					this.syncStatTrackTrackerVisibility(this._firstModel);
					this.firstEventComponent = this.firstModel.GetComponent<UseableEventHook>();
					Transform parent;
					switch (this.asset.EquipableModelParent)
					{
					default:
						parent = this.firstRightHook;
						break;
					case EEquipableModelParent.LeftHook:
						parent = this.firstLeftHook;
						break;
					case EEquipableModelParent.Spine:
						parent = this._firstSpine;
						break;
					case EEquipableModelParent.SpineHook:
						parent = this._firstSpineHook;
						break;
					}
					this.firstModel.transform.parent = parent;
					this.firstModel.localPosition = Vector3.zero;
					this.firstModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
					this.ApplyEquipableLocalScale(this._asset, this.firstModel);
					this.firstModel.gameObject.SetActive(false);
					this.firstModel.gameObject.SetActive(true);
					this.firstModel.DestroyRigidbody();
					if (num2 != 0)
					{
						this.firstMythic = ItemTool.ApplyMythicalEffect(this.firstModel, num2, EEffectType.FIRST);
					}
					else
					{
						this.firstMythic = null;
					}
					this.firstSkinned = true;
					this.applySkinVisual();
					if (this.firstMythic != null)
					{
						this.firstMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					this._characterModel = ItemTool.getItem(this.quality, this.state, false, this.asset, skinAsset, this.tempCharacterMesh, out this.tempCharacterMaterial, new GetStatTrackerValueHandler(this.getUseableStatTrackerValue), prefabOverride);
					this.fixStatTrackerHookScale(this._characterModel);
					this.syncStatTrackTrackerVisibility(this._characterModel);
					Transform parent2;
					switch (this.asset.EquipableModelParent)
					{
					default:
						parent2 = this.characterRightHook;
						break;
					case EEquipableModelParent.LeftHook:
						parent2 = this.characterLeftHook;
						break;
					case EEquipableModelParent.Spine:
						parent2 = this._characterSpine;
						break;
					case EEquipableModelParent.SpineHook:
						parent2 = this._characterSpineHook;
						break;
					}
					this.characterModel.transform.parent = parent2;
					this.characterModel.localPosition = Vector3.zero;
					this.characterModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
					this.ApplyEquipableLocalScale(this._asset, this.characterModel);
					Rigidbody orAddComponent = this.characterModel.gameObject.GetOrAddComponent<Rigidbody>();
					orAddComponent.useGravity = false;
					orAddComponent.isKinematic = true;
					if (num2 != 0)
					{
						this.characterMythic = ItemTool.ApplyMythicalEffect(this.characterModel, num2, EEffectType.THIRD);
					}
					else
					{
						this.characterMythic = null;
					}
					this.characterSkinned = true;
					this.applySkinVisual();
					if (this.characterMythic != null)
					{
						this.characterMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
				}
				this._thirdModel = ItemTool.InstantiateItem(this.quality, this.state, false, this.asset, skinAsset, true, this.tempThirdMesh, out this.tempThirdMaterial, new GetStatTrackerValueHandler(this.getUseableStatTrackerValue), prefabOverride);
				this.fixStatTrackerHookScale(this._thirdModel);
				this.syncStatTrackTrackerVisibility(this._thirdModel);
				this.thirdEventComponent = this._thirdModel.GetComponent<UseableEventHook>();
				Transform parent3;
				switch (this.asset.EquipableModelParent)
				{
				default:
					parent3 = this.thirdRightHook;
					break;
				case EEquipableModelParent.LeftHook:
					parent3 = this.thirdLeftHook;
					break;
				case EEquipableModelParent.Spine:
					parent3 = this._thirdSpine;
					break;
				case EEquipableModelParent.SpineHook:
					parent3 = this._thirdSpineHook;
					break;
				}
				this.thirdModel.transform.parent = parent3;
				this.thirdModel.localPosition = Vector3.zero;
				this.thirdModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
				this.ApplyEquipableLocalScale(this._asset, this.thirdModel);
				this.thirdModel.gameObject.SetActive(false);
				this.thirdModel.gameObject.SetActive(true);
				Rigidbody orAddComponent2 = this.thirdModel.GetOrAddComponent<Rigidbody>();
				orAddComponent2.useGravity = false;
				orAddComponent2.isKinematic = true;
				Layerer.enemy(this.thirdModel);
				if (num2 != 0)
				{
					this.thirdMythic = ItemTool.ApplyMythicalEffect(this.thirdModel, num2, EEffectType.THIRD);
				}
				else
				{
					this.thirdMythic = null;
				}
				this.thirdSkinned = true;
				this.applySkinVisual();
				if (this.thirdMythic != null)
				{
					this.thirdMythic.IsMythicalEffectEnabled = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
				}
				if (this.asset.animations != null && this.asset.animations.Length != 0)
				{
					for (int j = 0; j < this.asset.animations.Length; j++)
					{
						base.player.animator.AddEquippedItemAnimation(this.asset.animations[j], this._firstModel, this._thirdModel, this._characterModel);
					}
				}
				this._useable = (base.gameObject.AddComponent(this.asset.useableType) as Useable);
				this._useable.AssignNetId(useableNetId);
				this.wasUsablePrimaryStarted = false;
				this.wasUsableSecondaryStarted = false;
				base.channel.markDirty();
				try
				{
					this.useable.equip();
				}
				catch (Exception e2)
				{
					UnturnedLog.warn("{0} raised an exception during tellEquip.equip:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e2);
				}
				this.equipAnimStartedFrame = base.player.input.simulation;
				float animationLength = base.player.animator.GetAnimationLength("Equip", true);
				this.equipAnimLengthFrames = MathfEx.CeilToUInt(animationLength / PlayerInput.RATE);
				this.equipAnimCompletedTime = Time.timeAsDouble + (double)animationLength;
				PlayerEquipment.OnUseableChanged_Global.TryInvoke("OnUseableChanged_Global", this);
			}
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x000E0A30 File Offset: 0x000DEC30
		[Obsolete("Renamed to ServerEquip")]
		public void tryEquip(byte page, byte x, byte y)
		{
			this.ServerEquip(page, x, y);
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x000E0A3B File Offset: 0x000DEC3B
		[Obsolete("No longer necessary after hash check was converted to newer system")]
		public void tryEquip(byte page, byte x, byte y, byte[] hash)
		{
			this.ServerEquip(page, x, y);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000E0A48 File Offset: 0x000DEC48
		public void ServerEquip(byte page, byte x, byte y)
		{
			if (this.isBusy || !this.canEquip || base.player.life.isDead || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.DRIVING)
			{
				return;
			}
			if (this.HasValidUseable && !this.IsEquipAnimationFinished)
			{
				return;
			}
			if (this.isTurret)
			{
				return;
			}
			if ((page == this.equippedPage && x == this.equipped_x && y == this.equipped_y) || page == 255)
			{
				bool flag = true;
				PlayerDequipRequestHandler playerDequipRequestHandler = this.onDequipRequested;
				if (playerDequipRequestHandler != null)
				{
					playerDequipRequestHandler(this, ref flag);
				}
				if (!flag)
				{
					return;
				}
				this.dequip();
				return;
			}
			else
			{
				if (page < 0 || page >= PlayerInventory.PAGES - 2)
				{
					return;
				}
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				if (item == null)
				{
					return;
				}
				if (ItemTool.checkUseable(page, item.item.id))
				{
					ItemAsset asset = item.GetAsset();
					if (asset == null)
					{
						return;
					}
					if ((base.player.stance.isSubmerged || base.player.stance.stance == EPlayerStance.SWIM) && !asset.canUseUnderwater)
					{
						return;
					}
					if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
					{
						return;
					}
					bool flag2 = true;
					PlayerEquipRequestHandler playerEquipRequestHandler = this.onEquipRequested;
					if (playerEquipRequestHandler != null)
					{
						playerEquipRequestHandler(this, item, asset, ref flag2);
					}
					if (!flag2)
					{
						return;
					}
					NetId arg = NetIdRegistry.Claim();
					if (item.item.state != null)
					{
						PlayerEquipment.SendEquip.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), page, x, y, asset.GUID, item.item.quality, item.item.state, arg);
						return;
					}
					PlayerEquipment.SendEquip.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), page, x, y, asset.GUID, item.item.quality, new byte[0], arg);
				}
				return;
			}
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x000E0C3E File Offset: 0x000DEE3E
		public void turretEquipClient()
		{
			this.isTurret = true;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x000E0C48 File Offset: 0x000DEE48
		public void turretEquipServer(ushort id, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			Guid guid = (asset != null) ? asset.GUID : Guid.Empty;
			NetId netId = NetIdRegistry.Claim();
			PlayerEquipment.SendEquip.Invoke(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), 254, 254, 254, guid, 100, state, netId);
			this.ReceiveEquip(254, 254, 254, guid, 100, state, netId);
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x000E0CB7 File Offset: 0x000DEEB7
		public void turretDequipClient()
		{
			this.isTurret = false;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x000E0CC0 File Offset: 0x000DEEC0
		public void turretDequipServer()
		{
			PlayerEquipment.SendEquip.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), byte.MaxValue, byte.MaxValue, byte.MaxValue, Guid.Empty, 0, new byte[0], default(NetId));
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000E0D07 File Offset: 0x000DEF07
		[Obsolete]
		public void askEquip(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			this.ServerEquip(page, x, y);
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x000E0D13 File Offset: 0x000DEF13
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5, legacyName = "askEquip")]
		public void ReceiveEquipRequest(byte page, byte x, byte y)
		{
			this.ServerEquip(page, x, y);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000E0D1E File Offset: 0x000DEF1E
		[Obsolete]
		public void askEquipment(CSteamID steamID)
		{
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000E0D20 File Offset: 0x000DEF20
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			for (byte b = 0; b < PlayerInventory.SLOTS; b += 1)
			{
				ItemJar item = base.player.inventory.getItem(b, 0);
				if (item != null)
				{
					if (item.item.state != null)
					{
						PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, b, item.item.id, item.item.state);
					}
					else
					{
						PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, b, item.item.id, new byte[0]);
					}
				}
				else
				{
					PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, b, 0, new byte[0]);
				}
			}
			if (this.HasValidUseable)
			{
				ItemAsset asset = this.asset;
				Guid arg = (asset != null) ? asset.GUID : Guid.Empty;
				NetId netId = this.useable.GetNetId();
				if (this.state != null)
				{
					PlayerEquipment.SendEquip.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.equippedPage, this.equipped_x, this.equipped_y, arg, this.quality, this.state, netId);
					return;
				}
				PlayerEquipment.SendEquip.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.equippedPage, this.equipped_x, this.equipped_y, arg, this.quality, new byte[0], netId);
			}
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000E0E88 File Offset: 0x000DF088
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			for (byte b = 0; b < PlayerInventory.SLOTS; b += 1)
			{
				ItemJar item = base.player.inventory.getItem(b, 0);
				if (item != null)
				{
					if (item.item.state != null)
					{
						PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, b, item.item.id, item.item.state);
					}
					else
					{
						PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, b, item.item.id, new byte[0]);
					}
				}
				else
				{
					PlayerEquipment.SendSlot.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, b, 0, new byte[0]);
				}
			}
			if (this.HasValidUseable)
			{
				ItemAsset asset = this.asset;
				Guid arg = (asset != null) ? asset.GUID : Guid.Empty;
				NetId netId = this.useable.GetNetId();
				if (this.state != null)
				{
					PlayerEquipment.SendEquip.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.equippedPage, this.equipped_x, this.equipped_y, arg, this.quality, this.state, netId);
					return;
				}
				PlayerEquipment.SendEquip.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.equippedPage, this.equipped_x, this.equipped_y, arg, this.quality, new byte[0], netId);
			}
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000E0FD8 File Offset: 0x000DF1D8
		public void updateState()
		{
			if (this.isTurret)
			{
				return;
			}
			byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
			if (index != 255)
			{
				base.player.inventory.updateState(this.equippedPage, index, this.state);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000E1038 File Offset: 0x000DF238
		public void updateQuality()
		{
			if (this.isTurret)
			{
				return;
			}
			byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
			if (index != 255)
			{
				base.player.inventory.updateQuality(this.equippedPage, index, this.quality);
			}
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000E1098 File Offset: 0x000DF298
		public void sendUpdateState()
		{
			if (this.isTurret)
			{
				PlayerEquipment.SendUpdateStateTemp.Invoke(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.state);
				this.ReceiveUpdateStateTemp(this.state);
				return;
			}
			byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
			if (index != 255)
			{
				PlayerEquipment.SendUpdateState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.equippedPage, index, this.state);
			}
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000E1124 File Offset: 0x000DF324
		public void sendUpdateQuality()
		{
			if (this.isTurret)
			{
				return;
			}
			base.player.inventory.sendUpdateQuality(this.equippedPage, this.equipped_x, this.equipped_y, this.quality);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000E1158 File Offset: 0x000DF358
		public void sendSlot(byte slot)
		{
			ItemJar item = base.player.inventory.getItem(slot, 0);
			if (item == null)
			{
				PlayerEquipment.SendSlot.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), slot, 0, new byte[0]);
				return;
			}
			if (item.item.state != null)
			{
				PlayerEquipment.SendSlot.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), slot, item.item.id, item.item.state);
				return;
			}
			PlayerEquipment.SendSlot.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), slot, item.item.id, new byte[0]);
		}

		/// <summary>
		/// Called clientside to ask server to equip an item in the inventory.
		/// </summary>
		// Token: 0x06003276 RID: 12918 RVA: 0x000E1200 File Offset: 0x000DF400
		public void equip(byte page, byte x, byte y)
		{
			if (page < 0 || page >= PlayerInventory.PAGES - 2)
			{
				return;
			}
			if (this.isBusy || !this.canEquip || base.player.life.isDead || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.DRIVING)
			{
				return;
			}
			if (this.HasValidUseable && !this.IsEquipAnimationFinished)
			{
				return;
			}
			byte index = base.player.inventory.getIndex(page, x, y);
			if (index == 255)
			{
				return;
			}
			ItemJar item = base.player.inventory.getItem(page, index);
			if (item == null)
			{
				return;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null)
			{
				return;
			}
			if ((base.player.stance.isSubmerged || base.player.stance.stance == EPlayerStance.SWIM) && !asset.canUseUnderwater)
			{
				return;
			}
			if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			this.lastEquip = Time.realtimeSinceStartup;
			PlayerEquipment.SendEquipRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		/// <summary>
		/// Hacked-in to bypass regular clientside checks when client would predict the item at given coords.
		/// </summary>
		// Token: 0x06003277 RID: 12919 RVA: 0x000E1316 File Offset: 0x000DF516
		internal void ClientEquipAfterItemDrag(byte page, byte x, byte y)
		{
			PlayerEquipment.SendEquipRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000E132C File Offset: 0x000DF52C
		public void dequip()
		{
			if (this.isTurret)
			{
				return;
			}
			if (this.ignoreDequip_A)
			{
				return;
			}
			if (Provider.isServer)
			{
				PlayerEquipment.SendEquip.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), byte.MaxValue, byte.MaxValue, byte.MaxValue, Guid.Empty, 0, new byte[0], default(NetId));
				return;
			}
			if (this.isBusy)
			{
				return;
			}
			PlayerEquipment.SendEquipRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000E13B8 File Offset: 0x000DF5B8
		public void use()
		{
			if (this.HasValidUseable)
			{
				ushort itemID = this.itemID;
				byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
				ItemJar item = base.player.inventory.getItem(this.equippedPage, index);
				byte equippedPage = this.equippedPage;
				byte equipped_x = this.equipped_x;
				byte equipped_y = this.equipped_y;
				byte rot = item.rot;
				base.player.inventory.removeItem(this.equippedPage, index);
				this.dequip();
				InventorySearch inventorySearch = base.player.inventory.has(itemID);
				if (inventorySearch != null)
				{
					base.player.inventory.ReceiveDragItem(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, equippedPage, equipped_x, equipped_y, rot);
					this.ServerEquip(equippedPage, equipped_x, equipped_y);
				}
			}
		}

		/// <summary>
		/// Remove the item from inventory so that if we die before <see cref="M:SDG.Unturned.PlayerEquipment.useStepB" /> the item isn't dropped
		/// </summary>
		// Token: 0x0600327A RID: 12922 RVA: 0x000E14A4 File Offset: 0x000DF6A4
		public void useStepA()
		{
			if (this.HasValidUseable)
			{
				byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
				ItemJar item = base.player.inventory.getItem(this.equippedPage, index);
				this.page_A = this.equippedPage;
				this.x_A = this.equipped_x;
				this.y_A = this.equipped_y;
				this.rot_A = item.rot;
				this.ignoreDequip_A = true;
				base.player.inventory.removeItem(this.equippedPage, index);
				this.ignoreDequip_A = false;
			}
		}

		/// <summary>
		/// Finish dequipping from <see cref="M:SDG.Unturned.PlayerEquipment.useStepA" />
		/// </summary>
		// Token: 0x0600327B RID: 12923 RVA: 0x000E154C File Offset: 0x000DF74C
		public void useStepB()
		{
			if (this.HasValidUseable)
			{
				ushort itemID = this.itemID;
				this.dequip();
				InventorySearch inventorySearch = base.player.inventory.has(itemID);
				if (inventorySearch != null)
				{
					base.player.inventory.ReceiveDragItem(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, this.page_A, this.x_A, this.y_A, this.rot_A);
					this.ServerEquip(this.page_A, this.x_A, this.y_A);
				}
			}
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000E15E0 File Offset: 0x000DF7E0
		internal void PlayPunchAudioClip()
		{
			AudioClip audioClip = PlayerEquipment.punchClipRef.loadAsset(true);
			if (audioClip == null)
			{
				UnturnedLog.warn("Missing built-in punching audio");
			}
			base.player.playSound(audioClip);
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000E161C File Offset: 0x000DF81C
		private void punch(EPlayerPunch mode)
		{
			if (base.channel.IsLocalPlayer)
			{
				this.PlayPunchAudioClip();
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 1.75f, RayMasks.DAMAGE_CLIENT, base.player);
				if (raycastInfo.player != null && PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER.damage > 1f && DamageTool.isPlayerAllowedToDamagePlayer(base.player, raycastInfo.player))
				{
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				else if ((raycastInfo.zombie != null && PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER.damage > 1f) || (raycastInfo.animal != null && PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER.damage > 1f))
				{
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && PlayerEquipment.DAMAGE_BARRICADE > 1f)
				{
					BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(raycastInfo.transform);
					if (barricadeDrop != null)
					{
						ItemBarricadeAsset asset = barricadeDrop.asset;
						if (asset != null && asset.canBeDamaged && asset.isVulnerable)
						{
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && PlayerEquipment.DAMAGE_STRUCTURE > 1f)
				{
					StructureDrop structureDrop = StructureDrop.FindByRootFast(raycastInfo.transform);
					if (structureDrop != null)
					{
						ItemStructureAsset asset2 = structureDrop.asset;
						if (asset2 != null && asset2.canBeDamaged && asset2.isVulnerable)
						{
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && PlayerEquipment.DAMAGE_VEHICLE > 1f)
				{
					if (raycastInfo.vehicle.asset != null && raycastInfo.vehicle.canBeDamaged && raycastInfo.vehicle.asset.isVulnerable)
					{
						PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && PlayerEquipment.DAMAGE_RESOURCE > 1f)
				{
					byte x;
					byte y;
					ushort index;
					if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
					{
						ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
						if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && resourceSpawnpoint.asset.vulnerableToFists)
						{
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.transform != null && PlayerEquipment.DAMAGE_OBJECT > 1f)
				{
					InteractableObjectRubble componentInParent = raycastInfo.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent != null)
					{
						raycastInfo.transform = componentInParent.transform;
						raycastInfo.section = componentInParent.getSection(raycastInfo.collider.transform);
						if (componentInParent.IsSectionIndexValid(raycastInfo.section) && !componentInParent.isSectionDead(raycastInfo.section) && componentInParent.asset.rubbleBladeID == 0 && componentInParent.asset.rubbleIsVulnerable)
						{
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Punch);
			}
			if (mode == EPlayerPunch.LEFT)
			{
				base.player.animator.play("Punch_Left", false);
				if (Provider.isServer)
				{
					base.player.animator.sendGesture(EPlayerGesture.PUNCH_LEFT, false);
				}
			}
			else if (mode == EPlayerPunch.RIGHT)
			{
				base.player.animator.play("Punch_Right", false);
				if (Provider.isServer)
				{
					base.player.animator.sendGesture(EPlayerGesture.PUNCH_RIGHT, false);
				}
			}
			PlayerEquipment.OnPunch_Global.TryInvoke("OnPunch_Global", this, mode);
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Punch);
				if (input == null)
				{
					return;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 36f)
				{
					return;
				}
				if (!string.IsNullOrEmpty(input.materialName))
				{
					DamageTool.ServerSpawnLegacyImpact(input.point, input.normal, input.materialName, input.colliderTransform, base.channel.GatherOwnerAndClientConnectionsWithinSphere(input.point, EffectManager.SMALL));
				}
				EPlayerKill eplayerKill = EPlayerKill.NONE;
				uint num = 0U;
				float num2 = 1f;
				num2 *= 1f + base.channel.owner.player.skills.mastery(0, 0) * 0.5f;
				if (input.type == ERaycastInfoType.PLAYER)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.player != null && DamageTool.isPlayerAllowedToDamagePlayer(base.player, input.player))
					{
						DamagePlayerParameters parameters = DamagePlayerParameters.make(input.player, EDeathCause.PUNCH, input.direction, PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER, input.limb);
						parameters.killer = base.channel.owner.playerID.steamID;
						parameters.times = num2;
						parameters.respectArmor = true;
						parameters.trackKill = true;
						if (base.player.input.IsUnderFakeLagPenalty)
						{
							parameters.times *= Provider.configData.Server.Fake_Lag_Damage_Penalty_Multiplier;
						}
						DamageTool.damagePlayer(parameters, out eplayerKill);
					}
				}
				else if (input.type == ERaycastInfoType.ZOMBIE)
				{
					if (input.zombie != null)
					{
						IDamageMultiplier damage_ZOMBIE_MULTIPLIER = PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER;
						DamageZombieParameters parameters2 = DamageZombieParameters.make(input.zombie, input.direction, damage_ZOMBIE_MULTIPLIER, input.limb);
						parameters2.times = num2;
						parameters2.allowBackstab = true;
						parameters2.respectArmor = true;
						parameters2.instigator = base.player;
						if (base.player.movement.nav != 255)
						{
							parameters2.AlertPosition = new Vector3?(base.transform.position);
						}
						DamageTool.damageZombie(parameters2, out eplayerKill, out num);
					}
				}
				else if (input.type == ERaycastInfoType.ANIMAL)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.animal != null)
					{
						IDamageMultiplier damage_ANIMAL_MULTIPLIER = PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER;
						DamageAnimalParameters parameters3 = DamageAnimalParameters.make(input.animal, input.direction, damage_ANIMAL_MULTIPLIER, input.limb);
						parameters3.times = num2;
						parameters3.instigator = base.player;
						parameters3.AlertPosition = new Vector3?(base.transform.position);
						DamageTool.damageAnimal(parameters3, out eplayerKill, out num);
					}
				}
				else if (input.type == ERaycastInfoType.VEHICLE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.vehicle != null && input.vehicle.asset != null && input.vehicle.canBeDamaged && input.vehicle.asset.isVulnerable)
					{
						DamageTool.damage(input.vehicle, false, Vector3.zero, false, PlayerEquipment.DAMAGE_VEHICLE, num2 * Provider.modeConfigData.Vehicles.Melee_Damage_Multiplier, true, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Punch);
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.transform != null && input.transform.CompareTag("Barricade"))
					{
						BarricadeDrop barricadeDrop2 = BarricadeDrop.FindByRootFast(input.transform);
						if (barricadeDrop2 != null)
						{
							ItemBarricadeAsset asset3 = barricadeDrop2.asset;
							if (asset3 != null && asset3.canBeDamaged && asset3.isVulnerable)
							{
								DamageTool.damage(input.transform, false, PlayerEquipment.DAMAGE_BARRICADE, num2 * Provider.modeConfigData.Barricades.Melee_Damage_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Punch);
							}
						}
					}
				}
				else if (input.type == ERaycastInfoType.STRUCTURE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.transform != null && input.transform.CompareTag("Structure"))
					{
						StructureDrop structureDrop2 = StructureDrop.FindByRootFast(input.transform);
						if (structureDrop2 != null)
						{
							ItemStructureAsset asset4 = structureDrop2.asset;
							if (asset4 != null && asset4.canBeDamaged && asset4.isVulnerable)
							{
								DamageTool.damage(input.transform, false, input.direction, PlayerEquipment.DAMAGE_STRUCTURE, num2 * Provider.modeConfigData.Structures.Melee_Damage_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Punch);
							}
						}
					}
				}
				else if (input.type == ERaycastInfoType.RESOURCE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					byte x2;
					byte y2;
					ushort index2;
					if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
					{
						ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
						if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead && resourceSpawnpoint2.asset.vulnerableToFists)
						{
							DamageTool.damage(input.transform, input.direction, PlayerEquipment.DAMAGE_RESOURCE, num2, 1f, out eplayerKill, out num, base.channel.owner.playerID.steamID, EDamageOrigin.Punch);
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
				{
					InteractableObjectRubble componentInParent2 = input.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent2 != null && componentInParent2.IsSectionIndexValid(input.section) && !componentInParent2.isSectionDead(input.section) && componentInParent2.asset.rubbleBladeID == 0 && componentInParent2.asset.rubbleIsVulnerable)
					{
						DamageTool.damage(componentInParent2.transform, input.direction, input.section, PlayerEquipment.DAMAGE_OBJECT, num2, out eplayerKill, out num, base.channel.owner.playerID.steamID, EDamageOrigin.Punch);
					}
				}
				if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
				{
					float num3 = 2f + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num3 *= num3;
					float num4 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num4 *= num4;
					Vector3 forward = base.player.look.aim.forward;
					for (int i = 0; i < Provider.clients.Count; i++)
					{
						if (Provider.clients[i] != base.channel.owner)
						{
							Player player = Provider.clients[i].player;
							if (!(player == null))
							{
								Vector3 vector = player.look.aim.position - base.player.look.aim.position;
								Vector3 a = Vector3.Project(vector, forward);
								if (a.sqrMagnitude < num3 && (a - vector).sqrMagnitude < num4)
								{
									base.player.life.markAggressive(false, true);
								}
							}
						}
					}
				}
				if (Level.info.type == ELevelType.HORDE)
				{
					if (input.zombie != null)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(10U);
						}
						else
						{
							base.player.skills.askPay(5U);
						}
					}
					if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(50U);
							return;
						}
						base.player.skills.askPay(25U);
						return;
					}
				}
				else
				{
					if (eplayerKill == EPlayerKill.PLAYER && Level.info.type == ELevelType.ARENA)
					{
						base.player.skills.askPay(100U);
					}
					base.player.sendStat(eplayerKill);
					if (num > 0U)
					{
						base.player.skills.askPay(num);
					}
				}
			}
		}

		/// <summary>
		/// (Temporarily?) separated out from simulate to try and get a better exception call stack.
		/// </summary>
		// Token: 0x0600327E RID: 12926 RVA: 0x000E22F8 File Offset: 0x000E04F8
		private bool simulate_MustDequip()
		{
			if (base.player.stance.stance == EPlayerStance.DRIVING && !this.isTurret)
			{
				return true;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				return !this.isBusy;
			}
			return (base.player.stance.isSubmerged || base.player.stance.stance == EPlayerStance.SWIM) && this.asset != null && !this.asset.canUseUnderwater && !this.isBusy;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000E2384 File Offset: 0x000E0584
		private bool StartUsablePrimary()
		{
			bool result = false;
			try
			{
				result = this.useable.startPrimary();
			}
			catch (Exception e)
			{
				UnturnedLog.warn("{0} raised an exception during simulate.startPrimary:", new object[]
				{
					this.asset
				});
				UnturnedLog.exception(e);
			}
			return result;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000E23D4 File Offset: 0x000E05D4
		private void StopUsablePrimary()
		{
			try
			{
				this.useable.stopPrimary();
			}
			catch (Exception e)
			{
				UnturnedLog.warn("{0} raised an exception during simulate.stopPrimary:", new object[]
				{
					this.asset
				});
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000E2420 File Offset: 0x000E0620
		private bool StartUsableSecondary()
		{
			bool result = false;
			try
			{
				result = this.useable.startSecondary();
			}
			catch (Exception e)
			{
				UnturnedLog.warn("{0} raised an exception during useable.startSecondary:", new object[]
				{
					this.asset
				});
				UnturnedLog.exception(e);
			}
			return result;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000E2470 File Offset: 0x000E0670
		private void StopUsableSecondary()
		{
			try
			{
				this.useable.stopSecondary();
			}
			catch (Exception e)
			{
				UnturnedLog.warn("{0} raised an exception during useable.stopSecondary:", new object[]
				{
					this.asset
				});
				UnturnedLog.exception(e);
			}
		}

		/// <summary>
		/// (Temporarily?) separated out from simulate to try and get a better exception call stack.
		/// </summary>
		// Token: 0x06003283 RID: 12931 RVA: 0x000E24BC File Offset: 0x000E06BC
		private void simulate_UseableInput(uint simulation, EAttackInputFlags inputPrimary, EAttackInputFlags inputSecondary, bool inputSteady)
		{
			if (inputPrimary.HasFlag(EAttackInputFlags.Start) && this.HasValidUseable && this.IsEquipAnimationFinished && !this.wasUsablePrimaryStarted)
			{
				this.wasUsablePrimaryStarted = this.StartUsablePrimary();
			}
			if (inputPrimary.HasFlag(EAttackInputFlags.Stop) && this.HasValidUseable && this.IsEquipAnimationFinished && this.wasUsablePrimaryStarted)
			{
				this.wasUsablePrimaryStarted = false;
				this.StopUsablePrimary();
			}
			if (inputSecondary.HasFlag(EAttackInputFlags.Start) && this.HasValidUseable && this.IsEquipAnimationFinished && !this.wasUsableSecondaryStarted)
			{
				this.wasUsableSecondaryStarted = this.StartUsableSecondary();
			}
			if (inputSecondary.HasFlag(EAttackInputFlags.Stop) && this.HasValidUseable && this.IsEquipAnimationFinished && this.wasUsableSecondaryStarted)
			{
				this.wasUsableSecondaryStarted = false;
				this.StopUsableSecondary();
			}
			if (this.HasValidUseable && this.IsEquipAnimationFinished)
			{
				try
				{
					this.useable.simulate(simulation, inputSteady);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during useable.simulate:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
			}
			if (Provider.isServer && this.HasValidUseable && this.IsEquipAnimationFinished && this.asset != null && this.asset.shouldDeleteAtZeroQuality && this.quality == 0)
			{
				this.use();
			}
		}

		/// <summary>
		/// (Temporarily?) separated out from simulate to try and get a better exception call stack.
		/// </summary>
		// Token: 0x06003284 RID: 12932 RVA: 0x000E2634 File Offset: 0x000E0834
		private void simulate_PunchInput(uint simulation, EAttackInputFlags inputPrimary, EAttackInputFlags inputSecondary)
		{
			if (inputPrimary.HasFlag(EAttackInputFlags.Start) && !this.isBusy && base.player.stance.stance != EPlayerStance.PRONE && simulation - this.lastPunch > 5U)
			{
				this.lastPunch = simulation;
				this.punch(EPlayerPunch.LEFT);
			}
			if (inputSecondary.HasFlag(EAttackInputFlags.Start) && !this.isBusy && base.player.stance.stance != EPlayerStance.PRONE && simulation - this.lastPunch > 5U)
			{
				this.lastPunch = simulation;
				this.punch(EPlayerPunch.RIGHT);
			}
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000E26D0 File Offset: 0x000E08D0
		public void simulate(uint simulation, EAttackInputFlags inputPrimary, EAttackInputFlags inputSecondary, bool inputSteady)
		{
			if (this.simulate_MustDequip())
			{
				if (this.HasValidUseable && Provider.isServer)
				{
					this.dequip();
				}
				return;
			}
			if (Time.realtimeSinceStartup - this.lastEquip < 0.1f || base.player.life.isDead)
			{
				return;
			}
			if (base.player.movement.isSafe)
			{
				if (this.asset == null)
				{
					if (base.player.movement.isSafeInfo == null || base.player.movement.isSafeInfo.noWeapons)
					{
						return;
					}
				}
				else if (base.player.movement.isSafeInfo == null || !this.asset.canBeUsedInSafezone(base.player.movement.isSafeInfo, base.channel.owner.isAdmin))
				{
					inputPrimary = EAttackInputFlags.Stop;
					inputSecondary = EAttackInputFlags.Stop;
				}
			}
			if (Level.info != null && Level.info.type != ELevelType.SURVIVAL && this.asset == null)
			{
				return;
			}
			if ((base.player.stance.isSubmerged || base.player.stance.stance == EPlayerStance.SWIM) && this.asset == null)
			{
				this.lastPunch = simulation;
				return;
			}
			if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			if (this.isTurret && (base.player.movement.getVehicle() == null || !base.player.movement.getVehicle().canUseTurret))
			{
				inputPrimary = EAttackInputFlags.Stop;
				inputSecondary = EAttackInputFlags.Stop;
			}
			if (this.HasValidUseable)
			{
				this.simulate_UseableInput(simulation, inputPrimary, inputSecondary, inputSteady);
				return;
			}
			this.simulate_PunchInput(simulation, inputPrimary, inputSecondary);
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000E286C File Offset: 0x000E0A6C
		public void tock(uint clock)
		{
			if (this.HasValidUseable && this.IsEquipAnimationFinished)
			{
				try
				{
					this.useable.tock(clock);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during tock.tock:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
			}
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000E28CC File Offset: 0x000E0ACC
		internal void updateVision()
		{
			if (this.hasVision && base.player.clothing.glassesState != null && base.player.clothing.glassesState.Length != 0 && base.player.clothing.glassesState[0] != 0)
			{
				if (base.player.clothing.glassesAsset.vision == ELightingVision.HEADLAMP)
				{
					base.player.enableHeadlamp(base.player.clothing.glassesAsset.lightConfig);
					if (base.channel.IsLocalPlayer)
					{
						LevelLighting.vision = ELightingVision.NONE;
						LevelLighting.updateLighting();
						LevelLighting.updateLocal();
						PlayerLifeUI.updateGrayscale();
					}
				}
				else
				{
					base.player.disableHeadlamp();
					if (base.channel.IsLocalPlayer)
					{
						ELightingVision vision = base.player.clothing.glassesAsset.vision;
						if (base.player.look.perspective != EPlayerPerspective.FIRST && !base.player.clothing.glassesAsset.isNightvisionAllowedInThirdPerson)
						{
							vision = ELightingVision.NONE;
						}
						LevelLighting.vision = vision;
						LevelLighting.nightvisionColor = base.player.clothing.glassesAsset.nightvisionColor;
						LevelLighting.nightvisionFogIntensity = base.player.clothing.glassesAsset.nightvisionFogIntensity;
						LevelLighting.updateLighting();
						LevelLighting.updateLocal();
						PlayerLifeUI.updateGrayscale();
					}
				}
				base.player.updateGlassesLights(true);
				return;
			}
			base.player.disableHeadlamp();
			if (base.channel.IsLocalPlayer)
			{
				LevelLighting.vision = ELightingVision.NONE;
				LevelLighting.updateLighting();
				LevelLighting.updateLocal();
				PlayerLifeUI.updateGrayscale();
			}
			base.player.updateGlassesLights(false);
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000E2A73 File Offset: 0x000E0C73
		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing)
			{
				this.arePrimaryAndSecondaryInputsReversedByHallucination = (Random.value < 0.25f);
				return;
			}
			this.arePrimaryAndSecondaryInputsReversedByHallucination = false;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000E2A92 File Offset: 0x000E0C92
		private void onPerspectiveUpdated(EPlayerPerspective perspective)
		{
			if (this.hasVision)
			{
				this.updateVision();
			}
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000E2AA2 File Offset: 0x000E0CA2
		private void onGlassesUpdated(ushort id, byte quality, byte[] state)
		{
			this.hasVision = (id != 0 && base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.vision > ELightingVision.NONE);
			this.updateVision();
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000E2AE0 File Offset: 0x000E0CE0
		private void OnVisualToggleChanged(PlayerClothing sender)
		{
			if (this.hasVision)
			{
				this.updateVision();
			}
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000E2AF0 File Offset: 0x000E0CF0
		private void onLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				if (base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Weapons_PvP : Provider.modeConfigData.Players.Lose_Weapons_PvE)
				{
					for (byte b = 0; b < PlayerInventory.SLOTS; b += 1)
					{
						this.updateSlot(b, 0, new byte[0]);
					}
				}
				if (Provider.isServer)
				{
					this.dequip();
				}
				this.isBusy = false;
				this.canEquip = true;
				this._equippedPage = byte.MaxValue;
				this._equipped_x = byte.MaxValue;
				this._equipped_y = byte.MaxValue;
			}
		}

		/// <summary>
		/// Allow UI to process input [0, 9] key press when cursor is visible.
		/// </summary>
		// Token: 0x0600328D RID: 12941 RVA: 0x000E2B94 File Offset: 0x000E0D94
		private void bindHotkey(byte button)
		{
			if (button < PlayerInventory.SLOTS)
			{
				return;
			}
			if (!PlayerDashboardUI.active || !PlayerDashboardInventoryUI.active)
			{
				return;
			}
			byte b = button - 2;
			if (PlayerDashboardInventoryUI.selectedPage >= PlayerInventory.SLOTS && PlayerDashboardInventoryUI.selectedPage < PlayerInventory.STORAGE)
			{
				if (ItemTool.checkUseable(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selectedJar.item.id))
				{
					HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
					hotkeyInfo.id = PlayerDashboardInventoryUI.selectedJar.item.id;
					hotkeyInfo.page = PlayerDashboardInventoryUI.selectedPage;
					hotkeyInfo.x = PlayerDashboardInventoryUI.selected_x;
					hotkeyInfo.y = PlayerDashboardInventoryUI.selected_y;
					PlayerDashboardInventoryUI.closeSelection();
					this.ClearDuplicateHotkeys((int)b);
					HotkeysUpdated hotkeysUpdated = this.onHotkeysUpdated;
					if (hotkeysUpdated == null)
					{
						return;
					}
					hotkeysUpdated();
					return;
				}
			}
			else if (PlayerDashboardInventoryUI.selectedPage == 255)
			{
				HotkeyInfo hotkeyInfo2 = this.hotkeys[(int)b];
				hotkeyInfo2.id = 0;
				hotkeyInfo2.page = byte.MaxValue;
				hotkeyInfo2.x = byte.MaxValue;
				hotkeyInfo2.y = byte.MaxValue;
				HotkeysUpdated hotkeysUpdated2 = this.onHotkeysUpdated;
				if (hotkeysUpdated2 == null)
				{
					return;
				}
				hotkeysUpdated2();
			}
		}

		/// <summary>
		/// Process input [0, 9] key press.
		/// </summary>
		// Token: 0x0600328E RID: 12942 RVA: 0x000E2C9C File Offset: 0x000E0E9C
		private void hotkey(byte button)
		{
			if (PlayerUI.window.showCursor)
			{
				this.bindHotkey(button);
				return;
			}
			if (!this.isBusy)
			{
				if (button < PlayerInventory.SLOTS)
				{
					ItemJar item = base.player.inventory.getItem(button, 0);
					if (item != null)
					{
						this.equip(button, item.x, item.y);
						return;
					}
					if (this.HasValidUseable && this.IsEquipAnimationFinished)
					{
						this.dequip();
						return;
					}
				}
				else
				{
					byte b = button - 2;
					HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
					if (hotkeyInfo.id != 0)
					{
						this.equip(hotkeyInfo.page, hotkeyInfo.x, hotkeyInfo.y);
						return;
					}
					if (this.HasValidUseable && this.IsEquipAnimationFinished)
					{
						this.dequip();
					}
				}
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000E2D58 File Offset: 0x000E0F58
		private void Update()
		{
			if (base.channel.IsLocalPlayer)
			{
				bool flag;
				bool flag2;
				if (!PlayerUI.window.showCursor && !PlayerDashboardInventoryUI.WasEventConsumed && !base.player.workzone.isBuilding && (base.player.movement.getVehicle() == null || base.player.look.perspective == EPlayerPerspective.FIRST))
				{
					KeyCode keyCode = ControlsSettings.primary;
					KeyCode keyCode2 = ControlsSettings.secondary;
					if (this.arePrimaryAndSecondaryInputsReversedByHallucination)
					{
						KeyCode keyCode3 = keyCode;
						keyCode = keyCode2;
						keyCode2 = keyCode3;
					}
					flag = InputEx.GetKey(keyCode);
					if (ControlsSettings.aiming == EControlMode.TOGGLE && this.asset != null && (this.asset.type == EItemType.GUN || this.asset.type == EItemType.OPTIC))
					{
						if (InputEx.GetKeyDown(keyCode2))
						{
							this.localWantsToAim = !this.localWantsToAim;
						}
						flag2 = this.localWantsToAim;
					}
					else
					{
						flag2 = InputEx.GetKey(keyCode2);
					}
					if (PlayerManager.IsClientUnderFakeLagPenalty)
					{
						flag = false;
						flag2 = false;
						this.localWantsToAim = false;
					}
					if (this.HasValidUseable && !this.IsEquipAnimationFinished)
					{
						flag = false;
						flag2 = false;
						this.localWantsToAim = false;
					}
				}
				else
				{
					flag = false;
					flag2 = false;
					this.localWantsToAim = false;
				}
				if (flag != this.localWasPrimaryHeldLastFrame)
				{
					if (flag)
					{
						this.localWasPrimaryPressedBetweenSimulationFrames = true;
					}
					else
					{
						this.localWasPrimaryReleasedBetweenSimulationFrames = true;
					}
				}
				this.localWasPrimaryHeldLastFrame = flag;
				if (flag2 != this.localWasSecondaryHeldLastFrame)
				{
					if (flag2)
					{
						this.localWasSecondaryPressedBetweenSimulationFrames = true;
					}
					else
					{
						this.localWasSecondaryReleasedBetweenSimulationFrames = true;
					}
				}
				this.localWasSecondaryHeldLastFrame = flag2;
			}
			this.wasTryingToSelect = false;
			if (base.channel.IsLocalPlayer)
			{
				if (!PlayerUI.window.showCursor && !base.player.workzone.isBuilding)
				{
					if (InputEx.GetKeyDown(ControlsSettings.vision) && this.hasVision && !PlayerLifeUI.scopeOverlay.IsVisible)
					{
						PlayerEquipment.SendToggleVisionRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
					}
					if (InputEx.GetKeyDown(ControlsSettings.dequip) && this.HasValidUseable && !this.isBusy && this.IsEquipAnimationFinished)
					{
						this.dequip();
					}
				}
				for (byte b = 0; b < 10; b += 1)
				{
					if (InputEx.GetKeyDown(ControlsSettings.getEquipmentHotbarKeyCode((int)b)))
					{
						this.hotkey(b);
					}
				}
			}
			if (this.HasValidUseable)
			{
				try
				{
					this.useable.tick();
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during Update.tick:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
			}
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000E2FC4 File Offset: 0x000E11C4
		internal void InitializePlayer()
		{
			this.hasVision = (base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.vision > ELightingVision.NONE);
			this.updateVision();
			this.thirdSlots = new Transform[(int)PlayerInventory.SLOTS];
			this.thirdSkinneds = new bool[(int)PlayerInventory.SLOTS];
			this.tempThirdMeshes = new List<Mesh>[(int)PlayerInventory.SLOTS];
			for (int i = 0; i < this.tempThirdMeshes.Length; i++)
			{
				this.tempThirdMeshes[i] = new List<Mesh>(4);
			}
			this.tempThirdMaterials = new Material[(int)PlayerInventory.SLOTS];
			this.thirdMythics = new MythicalEffectController[(int)PlayerInventory.SLOTS];
			this.tempThirdMesh = new List<Mesh>(4);
			if (base.channel.IsLocalPlayer && base.player.character != null)
			{
				this.tempFirstMesh = new List<Mesh>(4);
				this.tempCharacterMesh = new List<Mesh>(4);
				this.characterSlots = new Transform[(int)PlayerInventory.SLOTS];
				this.characterSkinneds = new bool[(int)PlayerInventory.SLOTS];
				this.tempCharacterMeshes = new List<Mesh>[(int)PlayerInventory.SLOTS];
				for (int j = 0; j < this.tempCharacterMeshes.Length; j++)
				{
					this.tempCharacterMeshes[j] = new List<Mesh>(4);
				}
				this.tempCharacterMaterials = new Material[(int)PlayerInventory.SLOTS];
				this.characterMythics = new MythicalEffectController[(int)PlayerInventory.SLOTS];
			}
			this.arePrimaryAndSecondaryInputsReversedByHallucination = false;
			this._equippedPage = byte.MaxValue;
			this._equipped_x = byte.MaxValue;
			this._equipped_y = byte.MaxValue;
			this.isBusy = false;
			this.canEquip = true;
			if (base.player.third != null)
			{
				this._thirdPrimaryMeleeSlot = base.player.animator.thirdSkeleton.Find("Spine").Find("Primary_Melee");
				this._thirdPrimaryLargeGunSlot = base.player.animator.thirdSkeleton.Find("Spine").Find("Primary_Large_Gun");
				this._thirdPrimarySmallGunSlot = base.player.animator.thirdSkeleton.Find("Spine").Find("Primary_Small_Gun");
				this._thirdSecondaryMeleeSlot = base.player.animator.thirdSkeleton.Find("Right_Hip").Find("Right_Leg").Find("Secondary_Melee");
				this._thirdSecondaryGunSlot = base.player.animator.thirdSkeleton.Find("Right_Hip").Find("Right_Leg").Find("Secondary_Gun");
			}
			if (base.channel.IsLocalPlayer)
			{
				this._characterPrimaryMeleeSlot = base.player.character.Find("Skeleton").Find("Spine").Find("Primary_Melee");
				this._characterPrimaryLargeGunSlot = base.player.character.Find("Skeleton").Find("Spine").Find("Primary_Large_Gun");
				this._characterPrimarySmallGunSlot = base.player.character.Find("Skeleton").Find("Spine").Find("Primary_Small_Gun");
				this._characterSecondaryMeleeSlot = base.player.character.Find("Skeleton").Find("Right_Hip").Find("Right_Leg").Find("Secondary_Melee");
				this._characterSecondaryGunSlot = base.player.character.Find("Skeleton").Find("Right_Hip").Find("Right_Leg").Find("Secondary_Gun");
			}
			if (base.player.first != null)
			{
				this._firstSpine = base.player.animator.firstSkeleton.Find("Spine");
				this._firstSpineHook = this._firstSpine.Find("Spine_Hook");
				this._firstLeftHook = this._firstSpine.Find("Left_Shoulder").Find("Left_Arm").Find("Left_Hand").Find("Left_Hook");
				this._firstRightHook = this._firstSpine.Find("Right_Shoulder").Find("Right_Arm").Find("Right_Hand").Find("Right_Hook");
			}
			if (base.player.third != null)
			{
				this._thirdSpine = base.player.animator.thirdSkeleton.Find("Spine");
				this._thirdSpineHook = this._thirdSpine.Find("Spine_Hook");
				this._thirdLeftHook = this._thirdSpine.Find("Left_Shoulder").Find("Left_Arm").Find("Left_Hand").Find("Left_Hook");
				this._thirdRightHook = this._thirdSpine.Find("Right_Shoulder").Find("Right_Arm").Find("Right_Hand").Find("Right_Hook");
			}
			if (base.channel.IsLocalPlayer && base.player.character != null)
			{
				this._characterSpine = base.player.character.Find("Skeleton/Spine");
				this._characterSpineHook = this._characterSpine.Find("Spine_Hook");
				this._characterLeftHook = this._characterSpine.Find("Left_Shoulder").Find("Left_Arm").Find("Left_Hand").Find("Left_Hook");
				this._characterRightHook = this._characterSpine.Find("Right_Shoulder").Find("Right_Arm").Find("Right_Hand").Find("Right_Hook");
			}
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
			}
			PlayerClothing clothing = base.player.clothing;
			clothing.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(clothing.onGlassesUpdated, new GlassesUpdated(this.onGlassesUpdated));
			base.player.clothing.VisualToggleChanged += this.OnVisualToggleChanged;
			if (base.channel.IsLocalPlayer)
			{
				this._hotkeys = new HotkeyInfo[8];
				byte b = 0;
				while ((int)b < this.hotkeys.Length)
				{
					this.hotkeys[(int)b] = new HotkeyInfo();
					b += 1;
				}
				this.load();
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			PlayerLife life2 = base.player.life;
			life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
		}

		/// <summary>
		/// Called by input when preparing for simulation frame.
		/// </summary>
		// Token: 0x06003291 RID: 12945 RVA: 0x000E36C0 File Offset: 0x000E18C0
		internal void CaptureAttackInputs(out EAttackInputFlags primaryAttack, out EAttackInputFlags secondaryAttack)
		{
			primaryAttack = EAttackInputFlags.None;
			secondaryAttack = EAttackInputFlags.None;
			if (this.localWasPrimaryPressedBetweenSimulationFrames || this.localWasPrimaryHeldLastFrame)
			{
				primaryAttack |= EAttackInputFlags.Start;
			}
			if (this.localWasPrimaryReleasedBetweenSimulationFrames)
			{
				primaryAttack |= EAttackInputFlags.Stop;
			}
			if (this.localWasSecondaryPressedBetweenSimulationFrames || this.localWasSecondaryHeldLastFrame)
			{
				secondaryAttack |= EAttackInputFlags.Start;
			}
			if (this.localWasSecondaryReleasedBetweenSimulationFrames)
			{
				secondaryAttack |= EAttackInputFlags.Stop;
			}
			this.localWasPrimaryPressedBetweenSimulationFrames = false;
			this.localWasPrimaryReleasedBetweenSimulationFrames = false;
			this.localWasSecondaryPressedBetweenSimulationFrames = false;
			this.localWasSecondaryReleasedBetweenSimulationFrames = false;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x000E3737 File Offset: 0x000E1937
		private IEnumerable<UseableEventHook> EnumerateEventComponents()
		{
			if (this.firstEventComponent)
			{
				yield return this.firstEventComponent;
			}
			if (this.thirdEventComponent)
			{
				yield return this.thirdEventComponent;
			}
			if (this.characterEventComponent)
			{
				yield return this.characterEventComponent;
			}
			yield break;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x000E3748 File Offset: 0x000E1948
		private void OnDestroy()
		{
			if (this.useable != null)
			{
				try
				{
					this.useable.dequip();
				}
				catch (Exception e)
				{
					UnturnedLog.warn("{0} raised an exception during OnDestroy.dequip:", new object[]
					{
						this.asset
					});
					UnturnedLog.exception(e);
				}
				this._useable.ReleaseNetId();
			}
			if (base.channel.IsLocalPlayer)
			{
				this.save();
			}
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000E37C0 File Offset: 0x000E19C0
		private string GetItemHotkeysFilePath()
		{
			if (Provider.clientTransport == null)
			{
				return string.Concat(new string[]
				{
					"/Worlds/Hotkeys/Equip_",
					Provider.serverID,
					"_",
					Provider.map,
					".dat"
				});
			}
			if (Provider.CurrentServerConnectParameters.steamId.IsValid())
			{
				string[] array = new string[7];
				array[0] = "/Worlds/Hotkeys/Equip_";
				int num = 1;
				CSteamID steamId = Provider.CurrentServerConnectParameters.steamId;
				array[num] = steamId.ToString();
				array[2] = "_";
				array[3] = Provider.map;
				array[4] = "_";
				array[5] = Characters.selected.ToString();
				array[6] = ".dat";
				return string.Concat(array);
			}
			uint value = Provider.CurrentServerConnectParameters.address.value;
			ushort connectionPort = Provider.CurrentServerConnectParameters.connectionPort;
			return string.Concat(new string[]
			{
				"/Worlds/Hotkeys/Equip_",
				value.ToString(),
				"_",
				connectionPort.ToString(),
				"_",
				Provider.map,
				"_",
				Characters.selected.ToString(),
				".dat"
			});
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000E38F6 File Offset: 0x000E1AF6
		private void LogItemHotkeys(string message)
		{
			UnturnedLog.info(message);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000E3900 File Offset: 0x000E1B00
		private void load()
		{
			string itemHotkeysFilePath = this.GetItemHotkeysFilePath();
			if (ReadWrite.fileExists(itemHotkeysFilePath, false))
			{
				Block block = ReadWrite.readBlock(itemHotkeysFilePath, false, 0);
				block.readByte();
				byte b = 0;
				while ((int)b < this.hotkeys.Length)
				{
					HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
					hotkeyInfo.id = block.readUInt16();
					hotkeyInfo.page = block.readByte();
					hotkeyInfo.x = block.readByte();
					hotkeyInfo.y = block.readByte();
					b += 1;
				}
				this.LogItemHotkeys("Loaded item hotkeys");
				return;
			}
			this.LogItemHotkeys("No item hotkeys to load");
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000E3990 File Offset: 0x000E1B90
		private void save()
		{
			if (this.hotkeys == null)
			{
				this.LogItemHotkeys("Ignoring request to save item hotkeys because they were not loaded yet");
				return;
			}
			bool flag = false;
			byte b = 0;
			while ((int)b < this.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
				if (hotkeyInfo.id != 0 || (hotkeyInfo.page != 255 && hotkeyInfo.x != 255 && hotkeyInfo.y != 255))
				{
					flag = true;
					break;
				}
				b += 1;
			}
			string itemHotkeysFilePath = this.GetItemHotkeysFilePath();
			if (flag)
			{
				Block block = new Block();
				block.writeByte(PlayerEquipment.SAVEDATA_VERSION);
				byte b2 = 0;
				while ((int)b2 < this.hotkeys.Length)
				{
					HotkeyInfo hotkeyInfo2 = this.hotkeys[(int)b2];
					block.writeUInt16(hotkeyInfo2.id);
					block.writeByte(hotkeyInfo2.page);
					block.writeByte(hotkeyInfo2.x);
					block.writeByte(hotkeyInfo2.y);
					b2 += 1;
				}
				ReadWrite.writeBlock(itemHotkeysFilePath, false, block);
				this.LogItemHotkeys("Saved item hotkeys");
				return;
			}
			if (ReadWrite.fileExists(itemHotkeysFilePath, false))
			{
				this.LogItemHotkeys("No item hotkeys to save, deleting old item hotkeys file");
				ReadWrite.deleteFile(itemHotkeysFilePath, false);
				return;
			}
			this.LogItemHotkeys("No item hotkeys to save");
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x000E3AB5 File Offset: 0x000E1CB5
		[Obsolete("Renamed to HasValidUseable")]
		public bool isSelected
		{
			get
			{
				return this.HasValidUseable;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000E3ABD File Offset: 0x000E1CBD
		[Obsolete("Renamed to IsEquipAnimationFinished")]
		public bool isEquipped
		{
			get
			{
				return this.IsEquipAnimationFinished;
			}
		}

		// Token: 0x04001C76 RID: 7286
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04001C77 RID: 7287
		private static readonly float DAMAGE_BARRICADE = 2f;

		// Token: 0x04001C78 RID: 7288
		private static readonly float DAMAGE_STRUCTURE = 2f;

		// Token: 0x04001C79 RID: 7289
		private static readonly float DAMAGE_VEHICLE = 0f;

		// Token: 0x04001C7A RID: 7290
		private static readonly float DAMAGE_RESOURCE = 20f;

		// Token: 0x04001C7B RID: 7291
		private static readonly float DAMAGE_OBJECT = 5f;

		// Token: 0x04001C7C RID: 7292
		private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(15f, 0.6f, 0.6f, 0.8f, 1.1f);

		// Token: 0x04001C7D RID: 7293
		private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(15f, 0.3f, 0.3f, 0.6f, 1.1f);

		// Token: 0x04001C7E RID: 7294
		private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(15f, 0.3f, 0.6f, 1.1f);

		// Token: 0x04001C7F RID: 7295
		public PlayerEquipRequestHandler onEquipRequested;

		// Token: 0x04001C80 RID: 7296
		public PlayerDequipRequestHandler onDequipRequested;

		// Token: 0x04001C83 RID: 7299
		private ERagdollEffect skinRagdollEffect;

		// Token: 0x04001C84 RID: 7300
		private byte[] _state;

		// Token: 0x04001C85 RID: 7301
		private byte _quality;

		// Token: 0x04001C86 RID: 7302
		private Transform[] thirdSlots;

		// Token: 0x04001C87 RID: 7303
		private bool[] thirdSkinneds;

		// Token: 0x04001C88 RID: 7304
		private List<Mesh>[] tempThirdMeshes;

		// Token: 0x04001C89 RID: 7305
		private Material[] tempThirdMaterials;

		// Token: 0x04001C8A RID: 7306
		private MythicalEffectController[] thirdMythics;

		// Token: 0x04001C8B RID: 7307
		private Transform[] characterSlots;

		// Token: 0x04001C8C RID: 7308
		private bool[] characterSkinneds;

		// Token: 0x04001C8D RID: 7309
		private List<Mesh>[] tempCharacterMeshes;

		// Token: 0x04001C8E RID: 7310
		private Material[] tempCharacterMaterials;

		// Token: 0x04001C8F RID: 7311
		private MythicalEffectController[] characterMythics;

		// Token: 0x04001C90 RID: 7312
		private Transform _firstModel;

		// Token: 0x04001C91 RID: 7313
		private bool firstSkinned;

		// Token: 0x04001C92 RID: 7314
		private List<Mesh> tempFirstMesh;

		// Token: 0x04001C93 RID: 7315
		private Material tempFirstMaterial;

		// Token: 0x04001C94 RID: 7316
		private MythicalEffectController firstMythic;

		// Token: 0x04001C95 RID: 7317
		private Transform _thirdModel;

		// Token: 0x04001C96 RID: 7318
		private bool thirdSkinned;

		// Token: 0x04001C97 RID: 7319
		private List<Mesh> tempThirdMesh;

		// Token: 0x04001C98 RID: 7320
		private Material tempThirdMaterial;

		// Token: 0x04001C99 RID: 7321
		private MythicalEffectController thirdMythic;

		// Token: 0x04001C9A RID: 7322
		private Transform _characterModel;

		// Token: 0x04001C9B RID: 7323
		private bool characterSkinned;

		// Token: 0x04001C9C RID: 7324
		private List<Mesh> tempCharacterMesh;

		// Token: 0x04001C9D RID: 7325
		private Material tempCharacterMaterial;

		// Token: 0x04001C9E RID: 7326
		private MythicalEffectController characterMythic;

		// Token: 0x04001C9F RID: 7327
		private ItemAsset _asset;

		// Token: 0x04001CA0 RID: 7328
		private Useable _useable;

		// Token: 0x04001CA1 RID: 7329
		private UseableEventHook firstEventComponent;

		// Token: 0x04001CA2 RID: 7330
		private UseableEventHook thirdEventComponent;

		// Token: 0x04001CA3 RID: 7331
		private UseableEventHook characterEventComponent;

		// Token: 0x04001CA4 RID: 7332
		private Transform _thirdPrimaryMeleeSlot;

		// Token: 0x04001CA5 RID: 7333
		private Transform _thirdPrimaryLargeGunSlot;

		// Token: 0x04001CA6 RID: 7334
		private Transform _thirdPrimarySmallGunSlot;

		// Token: 0x04001CA7 RID: 7335
		private Transform _thirdSecondaryMeleeSlot;

		// Token: 0x04001CA8 RID: 7336
		private Transform _thirdSecondaryGunSlot;

		// Token: 0x04001CA9 RID: 7337
		private Transform _characterPrimaryMeleeSlot;

		// Token: 0x04001CAA RID: 7338
		private Transform _characterPrimaryLargeGunSlot;

		// Token: 0x04001CAB RID: 7339
		private Transform _characterPrimarySmallGunSlot;

		// Token: 0x04001CAC RID: 7340
		private Transform _characterSecondaryMeleeSlot;

		// Token: 0x04001CAD RID: 7341
		private Transform _characterSecondaryGunSlot;

		// Token: 0x04001CAE RID: 7342
		private Transform _firstSpine;

		// Token: 0x04001CAF RID: 7343
		private Transform _firstSpineHook;

		// Token: 0x04001CB0 RID: 7344
		private Transform _firstLeftHook;

		// Token: 0x04001CB1 RID: 7345
		private Transform _firstRightHook;

		// Token: 0x04001CB2 RID: 7346
		private Transform _thirdSpine;

		// Token: 0x04001CB3 RID: 7347
		private Transform _thirdSpineHook;

		// Token: 0x04001CB4 RID: 7348
		private Transform _thirdLeftHook;

		// Token: 0x04001CB5 RID: 7349
		private Transform _thirdRightHook;

		// Token: 0x04001CB6 RID: 7350
		private Transform _characterSpine;

		// Token: 0x04001CB7 RID: 7351
		private Transform _characterSpineHook;

		// Token: 0x04001CB8 RID: 7352
		private Transform _characterLeftHook;

		// Token: 0x04001CB9 RID: 7353
		private Transform _characterRightHook;

		// Token: 0x04001CBA RID: 7354
		private HotkeyInfo[] _hotkeys;

		// Token: 0x04001CBB RID: 7355
		public HotkeysUpdated onHotkeysUpdated;

		// Token: 0x04001CBC RID: 7356
		public bool wasTryingToSelect;

		// Token: 0x04001CBE RID: 7358
		public bool isBusy;

		// Token: 0x04001CBF RID: 7359
		public bool canEquip;

		// Token: 0x04001CC0 RID: 7360
		private byte slot = byte.MaxValue;

		// Token: 0x04001CC1 RID: 7361
		internal bool arePrimaryAndSecondaryInputsReversedByHallucination;

		// Token: 0x04001CC2 RID: 7362
		private byte _equippedPage;

		// Token: 0x04001CC3 RID: 7363
		private byte _equipped_x;

		// Token: 0x04001CC4 RID: 7364
		private byte _equipped_y;

		// Token: 0x04001CC5 RID: 7365
		private bool wasUsablePrimaryStarted;

		// Token: 0x04001CC6 RID: 7366
		private bool wasUsableSecondaryStarted;

		/// <summary>
		/// For aiming toggle input.
		/// </summary>
		// Token: 0x04001CC7 RID: 7367
		private bool localWantsToAim;

		// Token: 0x04001CC8 RID: 7368
		private bool hasVision;

		// Token: 0x04001CCA RID: 7370
		private double equipAnimCompletedTime;

		// Token: 0x04001CCB RID: 7371
		private uint equipAnimStartedFrame;

		// Token: 0x04001CCC RID: 7372
		private uint equipAnimLengthFrames;

		// Token: 0x04001CCD RID: 7373
		private float lastEquip;

		// Token: 0x04001CCE RID: 7374
		private uint lastPunch;

		// Token: 0x04001CCF RID: 7375
		private static float lastInspect;

		// Token: 0x04001CD0 RID: 7376
		private static float inspectTime;

		// Token: 0x04001CD1 RID: 7377
		private bool localWasPrimaryHeldLastFrame;

		// Token: 0x04001CD2 RID: 7378
		private bool localWasPrimaryPressedBetweenSimulationFrames;

		// Token: 0x04001CD3 RID: 7379
		private bool localWasPrimaryReleasedBetweenSimulationFrames;

		// Token: 0x04001CD4 RID: 7380
		private bool localWasSecondaryHeldLastFrame;

		// Token: 0x04001CD5 RID: 7381
		private bool localWasSecondaryPressedBetweenSimulationFrames;

		// Token: 0x04001CD6 RID: 7382
		private bool localWasSecondaryReleasedBetweenSimulationFrames;

		// Token: 0x04001CD7 RID: 7383
		private static readonly ClientInstanceMethod<byte, Guid, byte, byte, byte> SendItemHotkeySuggestion = ClientInstanceMethod<byte, Guid, byte, byte, byte>.Get(typeof(PlayerEquipment), "ReceiveItemHotkeySuggeston");

		// Token: 0x04001CD8 RID: 7384
		private static readonly ServerInstanceMethod SendToggleVisionRequest = ServerInstanceMethod.Get(typeof(PlayerEquipment), "ReceiveToggleVisionRequest");

		// Token: 0x04001CD9 RID: 7385
		private static readonly AssetReference<EffectAsset> BeepRef = new AssetReference<EffectAsset>("f515fcbe1b5241e39217b52317e68d72");

		// Token: 0x04001CDA RID: 7386
		private static readonly ClientInstanceMethod SendToggleVision = ClientInstanceMethod.Get(typeof(PlayerEquipment), "ReceiveToggleVision");

		// Token: 0x04001CDB RID: 7387
		private static readonly ClientInstanceMethod<byte, ushort, byte[]> SendSlot = ClientInstanceMethod<byte, ushort, byte[]>.Get(typeof(PlayerEquipment), "ReceiveSlot");

		// Token: 0x04001CDC RID: 7388
		private static readonly ClientInstanceMethod<byte[]> SendUpdateStateTemp = ClientInstanceMethod<byte[]>.Get(typeof(PlayerEquipment), "ReceiveUpdateStateTemp");

		// Token: 0x04001CDD RID: 7389
		private static readonly ClientInstanceMethod<byte, byte, byte[]> SendUpdateState = ClientInstanceMethod<byte, byte, byte[]>.Get(typeof(PlayerEquipment), "ReceiveUpdateState");

		// Token: 0x04001CDE RID: 7390
		private static readonly ClientInstanceMethod<byte, byte, byte, Guid, byte, byte[], NetId> SendEquip = ClientInstanceMethod<byte, byte, byte, Guid, byte, byte[], NetId>.Get(typeof(PlayerEquipment), "ReceiveEquip");

		// Token: 0x04001CDF RID: 7391
		private static readonly ServerInstanceMethod<byte, byte, byte> SendEquipRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerEquipment), "ReceiveEquipRequest");

		// Token: 0x04001CE0 RID: 7392
		protected byte page_A;

		// Token: 0x04001CE1 RID: 7393
		protected byte x_A;

		// Token: 0x04001CE2 RID: 7394
		protected byte y_A;

		// Token: 0x04001CE3 RID: 7395
		protected byte rot_A;

		// Token: 0x04001CE4 RID: 7396
		protected bool ignoreDequip_A;

		/// <summary>
		/// Invoked before dealing damage regardless of whether the punch impacted anything.
		/// </summary>
		// Token: 0x04001CE5 RID: 7397
		public static Action<PlayerEquipment, EPlayerPunch> OnPunch_Global;

		// Token: 0x04001CE6 RID: 7398
		private static MasterBundleReference<AudioClip> punchClipRef = new MasterBundleReference<AudioClip>("core.masterbundle", "Sounds/MeleeAttack_02.mp3");
	}
}
