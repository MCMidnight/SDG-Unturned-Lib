using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000611 RID: 1553
	public class PlayerClothing : PlayerCaller
	{
		/// <summary>
		/// Called when the player clicks the cosmetic, visual or skin toggle buttons.
		/// </summary>
		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06003153 RID: 12627 RVA: 0x000D9E00 File Offset: 0x000D8000
		// (remove) Token: 0x06003154 RID: 12628 RVA: 0x000D9E38 File Offset: 0x000D8038
		public event VisualToggleChanged VisualToggleChanged;

		/// <summary>
		/// Invoked after any player's shirt values change (not including loading).
		/// </summary>
		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06003155 RID: 12629 RVA: 0x000D9E70 File Offset: 0x000D8070
		// (remove) Token: 0x06003156 RID: 12630 RVA: 0x000D9EA4 File Offset: 0x000D80A4
		public static event Action<PlayerClothing> OnShirtChanged_Global;

		/// <summary>
		/// Invoked after any player's shirt values change (not including loading).
		/// </summary>
		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06003157 RID: 12631 RVA: 0x000D9ED8 File Offset: 0x000D80D8
		// (remove) Token: 0x06003158 RID: 12632 RVA: 0x000D9F0C File Offset: 0x000D810C
		public static event Action<PlayerClothing> OnPantsChanged_Global;

		/// <summary>
		/// Invoked after any player's hat values change (not including loading).
		/// </summary>
		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06003159 RID: 12633 RVA: 0x000D9F40 File Offset: 0x000D8140
		// (remove) Token: 0x0600315A RID: 12634 RVA: 0x000D9F74 File Offset: 0x000D8174
		public static event Action<PlayerClothing> OnHatChanged_Global;

		/// <summary>
		/// Invoked after any player's backpack values change (not including loading).
		/// </summary>
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x0600315B RID: 12635 RVA: 0x000D9FA8 File Offset: 0x000D81A8
		// (remove) Token: 0x0600315C RID: 12636 RVA: 0x000D9FDC File Offset: 0x000D81DC
		public static event Action<PlayerClothing> OnBackpackChanged_Global;

		/// <summary>
		/// Invoked after any player's backpack values change (not including loading).
		/// </summary>
		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x0600315D RID: 12637 RVA: 0x000DA010 File Offset: 0x000D8210
		// (remove) Token: 0x0600315E RID: 12638 RVA: 0x000DA044 File Offset: 0x000D8244
		public static event Action<PlayerClothing> OnVestChanged_Global;

		/// <summary>
		/// Invoked after any player's backpack values change (not including loading).
		/// </summary>
		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x0600315F RID: 12639 RVA: 0x000DA078 File Offset: 0x000D8278
		// (remove) Token: 0x06003160 RID: 12640 RVA: 0x000DA0AC File Offset: 0x000D82AC
		public static event Action<PlayerClothing> OnMaskChanged_Global;

		/// <summary>
		/// Invoked after any player's glasses values change (not including loading).
		/// </summary>
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06003161 RID: 12641 RVA: 0x000DA0E0 File Offset: 0x000D82E0
		// (remove) Token: 0x06003162 RID: 12642 RVA: 0x000DA114 File Offset: 0x000D8314
		public static event Action<PlayerClothing> OnGlassesChanged_Global;

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x000DA147 File Offset: 0x000D8347
		// (set) Token: 0x06003164 RID: 12644 RVA: 0x000DA14F File Offset: 0x000D834F
		public HumanClothes firstClothes { get; private set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x000DA158 File Offset: 0x000D8358
		// (set) Token: 0x06003166 RID: 12646 RVA: 0x000DA160 File Offset: 0x000D8360
		public HumanClothes thirdClothes { get; private set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x000DA169 File Offset: 0x000D8369
		// (set) Token: 0x06003168 RID: 12648 RVA: 0x000DA171 File Offset: 0x000D8371
		public HumanClothes characterClothes { get; private set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x000DA17A File Offset: 0x000D837A
		public bool isVisual
		{
			get
			{
				return this.thirdClothes.isVisual;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x000DA187 File Offset: 0x000D8387
		// (set) Token: 0x0600316B RID: 12651 RVA: 0x000DA18F File Offset: 0x000D838F
		public bool isSkinned { get; private set; }

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x000DA198 File Offset: 0x000D8398
		public bool isMythic
		{
			get
			{
				return this.thirdClothes.isMythic;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x000DA1A5 File Offset: 0x000D83A5
		public ItemShirtAsset shirtAsset
		{
			get
			{
				return this.thirdClothes.shirtAsset;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000DA1B2 File Offset: 0x000D83B2
		public ItemPantsAsset pantsAsset
		{
			get
			{
				return this.thirdClothes.pantsAsset;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x000DA1BF File Offset: 0x000D83BF
		public ItemHatAsset hatAsset
		{
			get
			{
				return this.thirdClothes.hatAsset;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x000DA1CC File Offset: 0x000D83CC
		public ItemBackpackAsset backpackAsset
		{
			get
			{
				return this.thirdClothes.backpackAsset;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000DA1D9 File Offset: 0x000D83D9
		public ItemVestAsset vestAsset
		{
			get
			{
				return this.thirdClothes.vestAsset;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x000DA1E6 File Offset: 0x000D83E6
		public ItemMaskAsset maskAsset
		{
			get
			{
				return this.thirdClothes.maskAsset;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000DA1F3 File Offset: 0x000D83F3
		public ItemGlassesAsset glassesAsset
		{
			get
			{
				return this.thirdClothes.glassesAsset;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000DA200 File Offset: 0x000D8400
		public int visualShirt
		{
			get
			{
				return this.thirdClothes.visualShirt;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x000DA20D File Offset: 0x000D840D
		public int visualPants
		{
			get
			{
				return this.thirdClothes.visualPants;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000DA21A File Offset: 0x000D841A
		public int visualHat
		{
			get
			{
				return this.thirdClothes.visualHat;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x000DA227 File Offset: 0x000D8427
		public int visualBackpack
		{
			get
			{
				return this.thirdClothes.visualBackpack;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000DA234 File Offset: 0x000D8434
		public int visualVest
		{
			get
			{
				return this.thirdClothes.visualVest;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000DA241 File Offset: 0x000D8441
		public int visualMask
		{
			get
			{
				return this.thirdClothes.visualMask;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x000DA24E File Offset: 0x000D844E
		public int visualGlasses
		{
			get
			{
				return this.thirdClothes.visualGlasses;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x000DA25B File Offset: 0x000D845B
		public ushort shirt
		{
			get
			{
				return this.thirdClothes.shirt;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x000DA268 File Offset: 0x000D8468
		public ushort pants
		{
			get
			{
				return this.thirdClothes.pants;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x000DA275 File Offset: 0x000D8475
		public ushort hat
		{
			get
			{
				return this.thirdClothes.hat;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x000DA282 File Offset: 0x000D8482
		public ushort backpack
		{
			get
			{
				return this.thirdClothes.backpack;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x000DA28F File Offset: 0x000D848F
		public ushort vest
		{
			get
			{
				return this.thirdClothes.vest;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000DA29C File Offset: 0x000D849C
		public ushort mask
		{
			get
			{
				return this.thirdClothes.mask;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000DA2A9 File Offset: 0x000D84A9
		public ushort glasses
		{
			get
			{
				return this.thirdClothes.glasses;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000DA2B6 File Offset: 0x000D84B6
		public byte face
		{
			get
			{
				return this.thirdClothes.face;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000DA2C3 File Offset: 0x000D84C3
		public byte hair
		{
			get
			{
				return this.thirdClothes.hair;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x000DA2D0 File Offset: 0x000D84D0
		public byte beard
		{
			get
			{
				return this.thirdClothes.beard;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003185 RID: 12677 RVA: 0x000DA2DD File Offset: 0x000D84DD
		public Color skin
		{
			get
			{
				return this.thirdClothes.skin;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x000DA2EA File Offset: 0x000D84EA
		public Color color
		{
			get
			{
				return this.thirdClothes.color;
			}
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000DA2F7 File Offset: 0x000D84F7
		[Obsolete]
		public void tellUpdateShirtQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveShirtQuality(quality);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000DA300 File Offset: 0x000D8500
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateShirtQuality")]
		public void ReceiveShirtQuality(byte quality)
		{
			this.shirtQuality = quality;
			ShirtUpdated shirtUpdated = this.onShirtUpdated;
			if (shirtUpdated == null)
			{
				return;
			}
			shirtUpdated(this.shirt, this.shirtQuality, this.shirtState);
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000DA32B File Offset: 0x000D852B
		public void sendUpdateShirtQuality()
		{
			PlayerClothing.SendShirtQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.shirtQuality);
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000DA34F File Offset: 0x000D854F
		[Obsolete]
		public void tellUpdatePantsQuality(CSteamID steamID, byte quality)
		{
			this.ReceivePantsQuality(quality);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000DA358 File Offset: 0x000D8558
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdatePantsQuality")]
		public void ReceivePantsQuality(byte quality)
		{
			this.pantsQuality = quality;
			PantsUpdated pantsUpdated = this.onPantsUpdated;
			if (pantsUpdated == null)
			{
				return;
			}
			pantsUpdated(this.pants, this.pantsQuality, this.pantsState);
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000DA383 File Offset: 0x000D8583
		public void sendUpdatePantsQuality()
		{
			PlayerClothing.SendPantsQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.pantsQuality);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000DA3A7 File Offset: 0x000D85A7
		[Obsolete]
		public void tellUpdateHatQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveHatQuality(quality);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000DA3B0 File Offset: 0x000D85B0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateHatQuality")]
		public void ReceiveHatQuality(byte quality)
		{
			this.hatQuality = quality;
			HatUpdated hatUpdated = this.onHatUpdated;
			if (hatUpdated == null)
			{
				return;
			}
			hatUpdated(this.hat, this.hatQuality, this.hatState);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000DA3DB File Offset: 0x000D85DB
		public void sendUpdateHatQuality()
		{
			PlayerClothing.SendHatQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.hatQuality);
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000DA3FF File Offset: 0x000D85FF
		[Obsolete]
		public void tellUpdateBackpackQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveBackpackQuality(quality);
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000DA408 File Offset: 0x000D8608
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateBackpackQuality")]
		public void ReceiveBackpackQuality(byte quality)
		{
			this.backpackQuality = quality;
			BackpackUpdated backpackUpdated = this.onBackpackUpdated;
			if (backpackUpdated == null)
			{
				return;
			}
			backpackUpdated(this.backpack, this.backpackQuality, this.backpackState);
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000DA433 File Offset: 0x000D8633
		public void sendUpdateBackpackQuality()
		{
			PlayerClothing.SendBackpackQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.backpackQuality);
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000DA457 File Offset: 0x000D8657
		[Obsolete]
		public void tellUpdateVestQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveVestQuality(quality);
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000DA460 File Offset: 0x000D8660
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateVestQuality")]
		public void ReceiveVestQuality(byte quality)
		{
			this.vestQuality = quality;
			VestUpdated vestUpdated = this.onVestUpdated;
			if (vestUpdated == null)
			{
				return;
			}
			vestUpdated(this.vest, this.vestQuality, this.vestState);
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000DA48B File Offset: 0x000D868B
		public void sendUpdateVestQuality()
		{
			PlayerClothing.SendVestQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.vestQuality);
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000DA4AF File Offset: 0x000D86AF
		[Obsolete]
		public void tellUpdateMaskQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveMaskQuality(quality);
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000DA4B8 File Offset: 0x000D86B8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateMaskQuality")]
		public void ReceiveMaskQuality(byte quality)
		{
			this.maskQuality = quality;
			MaskUpdated maskUpdated = this.onMaskUpdated;
			if (maskUpdated == null)
			{
				return;
			}
			maskUpdated(this.mask, this.maskQuality, this.maskState);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x000DA4E3 File Offset: 0x000D86E3
		public void sendUpdateMaskQuality()
		{
			PlayerClothing.SendMaskQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.maskQuality);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000DA507 File Offset: 0x000D8707
		public void updateMaskQuality()
		{
			MaskUpdated maskUpdated = this.onMaskUpdated;
			if (maskUpdated == null)
			{
				return;
			}
			maskUpdated(this.mask, this.maskQuality, this.maskState);
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000DA52B File Offset: 0x000D872B
		[Obsolete]
		public void tellUpdateGlassesQuality(CSteamID steamID, byte quality)
		{
			this.ReceiveGlassesQuality(quality);
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000DA534 File Offset: 0x000D8734
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateGlassesQuality")]
		public void ReceiveGlassesQuality(byte quality)
		{
			this.glassesQuality = quality;
			GlassesUpdated glassesUpdated = this.onGlassesUpdated;
			if (glassesUpdated == null)
			{
				return;
			}
			glassesUpdated(this.glasses, this.glassesQuality, this.glassesState);
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000DA55F File Offset: 0x000D875F
		public void sendUpdateGlassesQuality()
		{
			PlayerClothing.SendGlassesQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.glassesQuality);
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000DA584 File Offset: 0x000D8784
		[Obsolete]
		public void tellWearShirt(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearShirt((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000DA5B4 File Offset: 0x000D87B4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearShirt")]
		public void ReceiveWearShirt(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.shirtGuid = id;
			this.shirtQuality = quality;
			this.shirtState = state;
			this.thirdClothes.apply();
			if (this.firstClothes != null)
			{
				this.firstClothes.shirtGuid = id;
				this.firstClothes.apply();
			}
			if (this.characterClothes != null)
			{
				this.characterClothes.shirtGuid = id;
				this.characterClothes.apply();
				Characters.active.shirt = this.shirt;
			}
			this.UpdateStatModifiers();
			ShirtUpdated shirtUpdated = this.onShirtUpdated;
			if (shirtUpdated != null)
			{
				shirtUpdated(this.shirt, quality, state);
			}
			Action<PlayerClothing> onShirtChanged_Global = PlayerClothing.OnShirtChanged_Global;
			if (onShirtChanged_Global != null)
			{
				onShirtChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.shirtAsset);
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000DA6A3 File Offset: 0x000D88A3
		[Obsolete]
		public void askSwapShirt(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapShirtRequest(page, x, y);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000DA6B0 File Offset: 0x000D88B0
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapShirt")]
		public void ReceiveSwapShirtRequest(byte page, byte x, byte y)
		{
			if (base.player.equipment.checkSelection(PlayerInventory.SHIRT))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page == 255)
			{
				if (this.shirtAsset == null)
				{
					return;
				}
				this.askWearShirt(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.SHIRT)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearShirt(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000DA798 File Offset: 0x000D8998
		public void askWearShirt(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemShirtAsset asset = Assets.find(EAssetType.ITEM, id) as ItemShirtAsset;
			this.askWearShirt(asset, quality, state, playEffect);
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000DA7C0 File Offset: 0x000D89C0
		public void askWearShirt(ItemShirtAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort shirt = this.shirt;
			byte newQuality = this.shirtQuality;
			byte[] newState = this.shirtState;
			PlayerClothing.SendWearShirt.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (shirt != 0)
			{
				base.player.inventory.forceAddItem(new Item(shirt, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000DA829 File Offset: 0x000D8A29
		public void sendSwapShirt(byte page, byte x, byte y)
		{
			if (page == 255 && this.shirtAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapShirtRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000DA850 File Offset: 0x000D8A50
		[Obsolete]
		public void tellWearPants(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearPants((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000DA880 File Offset: 0x000D8A80
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearPants")]
		public void ReceiveWearPants(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.pantsGuid = id;
			this.pantsQuality = quality;
			this.pantsState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.pantsGuid = id;
				this.characterClothes.apply();
				Characters.active.pants = this.pants;
			}
			this.UpdateStatModifiers();
			PantsUpdated pantsUpdated = this.onPantsUpdated;
			if (pantsUpdated != null)
			{
				pantsUpdated(this.pants, quality, state);
			}
			Action<PlayerClothing> onPantsChanged_Global = PlayerClothing.OnPantsChanged_Global;
			if (onPantsChanged_Global != null)
			{
				onPantsChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.pantsAsset);
			}
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x000DA94A File Offset: 0x000D8B4A
		[Obsolete]
		public void askSwapPants(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapPantsRequest(page, x, y);
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000DA958 File Offset: 0x000D8B58
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapPants")]
		public void ReceiveSwapPantsRequest(byte page, byte x, byte y)
		{
			if (base.player.equipment.checkSelection(PlayerInventory.PANTS))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page == 255)
			{
				if (this.pantsAsset == null)
				{
					return;
				}
				this.askWearPants(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.PANTS)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearPants(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000DAA40 File Offset: 0x000D8C40
		public void askWearPants(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemPantsAsset asset = Assets.find(EAssetType.ITEM, id) as ItemPantsAsset;
			this.askWearPants(asset, quality, state, playEffect);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000DAA68 File Offset: 0x000D8C68
		public void askWearPants(ItemPantsAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort pants = this.pants;
			byte newQuality = this.pantsQuality;
			byte[] newState = this.pantsState;
			PlayerClothing.SendWearPants.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (pants != 0)
			{
				base.player.inventory.forceAddItem(new Item(pants, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000DAAD1 File Offset: 0x000D8CD1
		public void sendSwapPants(byte page, byte x, byte y)
		{
			if (page == 255 && this.pantsAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapPantsRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000DAAF8 File Offset: 0x000D8CF8
		[Obsolete]
		public void tellWearHat(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearHat((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000DAB28 File Offset: 0x000D8D28
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearHat")]
		public void ReceiveWearHat(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.hatGuid = id;
			this.hatQuality = quality;
			this.hatState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.hatGuid = id;
				this.characterClothes.apply();
				Characters.active.hat = this.hat;
			}
			this.UpdateStatModifiers();
			HatUpdated hatUpdated = this.onHatUpdated;
			if (hatUpdated != null)
			{
				hatUpdated(this.hat, quality, state);
			}
			Action<PlayerClothing> onHatChanged_Global = PlayerClothing.OnHatChanged_Global;
			if (onHatChanged_Global != null)
			{
				onHatChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.hatAsset);
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000DABF2 File Offset: 0x000D8DF2
		[Obsolete]
		public void askSwapHat(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapHatRequest(page, x, y);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000DAC00 File Offset: 0x000D8E00
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapHat")]
		public void ReceiveSwapHatRequest(byte page, byte x, byte y)
		{
			if (page == 255)
			{
				if (this.hatAsset == null)
				{
					return;
				}
				this.askWearHat(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.HAT)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearHat(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000DACAC File Offset: 0x000D8EAC
		public void askWearHat(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemHatAsset asset = Assets.find(EAssetType.ITEM, id) as ItemHatAsset;
			this.askWearHat(asset, quality, state, playEffect);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000DACD4 File Offset: 0x000D8ED4
		public void askWearHat(ItemHatAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort hat = this.hat;
			byte newQuality = this.hatQuality;
			byte[] newState = this.hatState;
			PlayerClothing.SendWearHat.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (hat != 0)
			{
				base.player.inventory.forceAddItem(new Item(hat, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000DAD3D File Offset: 0x000D8F3D
		public void sendSwapHat(byte page, byte x, byte y)
		{
			if (page == 255 && this.hatAsset == null)
			{
				return;
			}
			if (Provider.isServer)
			{
				this.ReceiveSwapHatRequest(page, x, y);
				return;
			}
			PlayerClothing.SendSwapHatRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000DAD78 File Offset: 0x000D8F78
		[Obsolete]
		public void tellWearBackpack(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearBackpack((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000DADA8 File Offset: 0x000D8FA8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearBackpack")]
		public void ReceiveWearBackpack(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.backpackGuid = id;
			this.backpackQuality = quality;
			this.backpackState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.backpackGuid = id;
				this.characterClothes.apply();
				Characters.active.backpack = this.backpack;
			}
			this.UpdateStatModifiers();
			BackpackUpdated backpackUpdated = this.onBackpackUpdated;
			if (backpackUpdated != null)
			{
				backpackUpdated(this.backpack, quality, state);
			}
			Action<PlayerClothing> onBackpackChanged_Global = PlayerClothing.OnBackpackChanged_Global;
			if (onBackpackChanged_Global != null)
			{
				onBackpackChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.backpackAsset);
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000DAE72 File Offset: 0x000D9072
		[Obsolete]
		public void askSwapBackpack(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapBackpackRequest(page, x, y);
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000DAE80 File Offset: 0x000D9080
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapBackpack")]
		public void ReceiveSwapBackpackRequest(byte page, byte x, byte y)
		{
			if (base.player.equipment.checkSelection(PlayerInventory.BACKPACK))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page == 255)
			{
				if (this.backpackAsset == null)
				{
					return;
				}
				this.askWearBackpack(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.BACKPACK)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearBackpack(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000DAF68 File Offset: 0x000D9168
		public void askWearBackpack(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemBackpackAsset asset = Assets.find(EAssetType.ITEM, id) as ItemBackpackAsset;
			this.askWearBackpack(asset, quality, state, playEffect);
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000DAF90 File Offset: 0x000D9190
		public void askWearBackpack(ItemBackpackAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort backpack = this.backpack;
			byte newQuality = this.backpackQuality;
			byte[] newState = this.backpackState;
			PlayerClothing.SendWearBackpack.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (backpack != 0)
			{
				base.player.inventory.forceAddItem(new Item(backpack, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000DAFF9 File Offset: 0x000D91F9
		public void sendSwapBackpack(byte page, byte x, byte y)
		{
			if (page == 255 && this.backpackAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapBackpackRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000DB020 File Offset: 0x000D9220
		[Obsolete]
		public void tellVisualToggle(CSteamID steamID, byte index, bool toggle)
		{
			this.ReceiveVisualToggleState((EVisualToggleType)index, toggle);
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000DB02C File Offset: 0x000D922C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVisualToggle")]
		public void ReceiveVisualToggleState(EVisualToggleType type, bool toggle)
		{
			switch (type)
			{
			case EVisualToggleType.COSMETIC:
				if (this.thirdClothes != null)
				{
					this.thirdClothes.isVisual = toggle;
					this.thirdClothes.apply();
				}
				if (this.firstClothes != null)
				{
					this.firstClothes.isVisual = toggle;
					this.firstClothes.apply();
				}
				if (this.characterClothes != null)
				{
					this.characterClothes.isVisual = toggle;
					this.characterClothes.apply();
				}
				break;
			case EVisualToggleType.SKIN:
				this.isSkinned = toggle;
				if (base.player.equipment != null)
				{
					base.player.equipment.applySkinVisual();
					base.player.equipment.applyMythicVisual();
				}
				break;
			case EVisualToggleType.MYTHIC:
				if (this.thirdClothes != null)
				{
					this.thirdClothes.isMythic = toggle;
					this.thirdClothes.apply();
				}
				if (this.firstClothes != null)
				{
					this.firstClothes.isMythic = toggle;
					this.firstClothes.apply();
				}
				if (this.characterClothes != null)
				{
					this.characterClothes.isMythic = toggle;
					this.characterClothes.apply();
				}
				if (base.player.equipment != null)
				{
					base.player.equipment.applyMythicVisual();
				}
				break;
			}
			VisualToggleChanged visualToggleChanged = this.VisualToggleChanged;
			if (visualToggleChanged == null)
			{
				return;
			}
			visualToggleChanged(this);
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000DB1AC File Offset: 0x000D93AC
		public void ServerSetVisualToggleState(EVisualToggleType type, bool isVisible)
		{
			PlayerClothing.SendVisualToggleState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), type, isVisible);
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000DB1C8 File Offset: 0x000D93C8
		[Obsolete]
		public void askVisualToggle(CSteamID steamID, byte index)
		{
			if (index > 2)
			{
				return;
			}
			this.ReceiveVisualToggleRequest((EVisualToggleType)index);
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000DB1E4 File Offset: 0x000D93E4
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askVisualToggle")]
		public void ReceiveVisualToggleRequest(EVisualToggleType type)
		{
			switch (type)
			{
			case EVisualToggleType.COSMETIC:
				PlayerClothing.SendVisualToggleState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), type, !this.isVisual);
				return;
			case EVisualToggleType.SKIN:
				PlayerClothing.SendVisualToggleState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), type, !this.isSkinned);
				return;
			case EVisualToggleType.MYTHIC:
				PlayerClothing.SendVisualToggleState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), type, !this.isMythic);
				return;
			default:
				return;
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000DB266 File Offset: 0x000D9466
		public void sendVisualToggle(EVisualToggleType type)
		{
			PlayerClothing.SendVisualToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, type);
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000DB27C File Offset: 0x000D947C
		[Obsolete]
		public void tellWearVest(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearVest((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x000DB2AC File Offset: 0x000D94AC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearVest")]
		public void ReceiveWearVest(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.vestGuid = id;
			this.vestQuality = quality;
			this.vestState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.vestGuid = id;
				this.characterClothes.apply();
				Characters.active.vest = this.vest;
			}
			this.UpdateStatModifiers();
			VestUpdated vestUpdated = this.onVestUpdated;
			if (vestUpdated != null)
			{
				vestUpdated(this.vest, quality, state);
			}
			Action<PlayerClothing> onVestChanged_Global = PlayerClothing.OnVestChanged_Global;
			if (onVestChanged_Global != null)
			{
				onVestChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.vestAsset);
			}
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000DB376 File Offset: 0x000D9576
		[Obsolete]
		public void askSwapVest(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapVestRequest(page, x, y);
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000DB384 File Offset: 0x000D9584
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapVest")]
		public void ReceiveSwapVestRequest(byte page, byte x, byte y)
		{
			if (base.player.equipment.checkSelection(PlayerInventory.VEST))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page == 255)
			{
				if (this.vestAsset == null)
				{
					return;
				}
				this.askWearVest(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.VEST)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearVest(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000DB46C File Offset: 0x000D966C
		public void askWearVest(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemVestAsset asset = Assets.find(EAssetType.ITEM, id) as ItemVestAsset;
			this.askWearVest(asset, quality, state, playEffect);
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000DB494 File Offset: 0x000D9694
		public void askWearVest(ItemVestAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort vest = this.vest;
			byte newQuality = this.vestQuality;
			byte[] newState = this.vestState;
			PlayerClothing.SendWearVest.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (vest != 0)
			{
				base.player.inventory.forceAddItem(new Item(vest, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x000DB4FD File Offset: 0x000D96FD
		public void sendSwapVest(byte page, byte x, byte y)
		{
			if (page == 255 && this.vestAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapVestRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000DB524 File Offset: 0x000D9724
		[Obsolete]
		public void tellWearMask(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearMask((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000DB554 File Offset: 0x000D9754
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearMask")]
		public void ReceiveWearMask(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.maskGuid = id;
			this.maskQuality = quality;
			this.maskState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.maskGuid = id;
				this.characterClothes.apply();
				Characters.active.mask = this.mask;
			}
			this.UpdateStatModifiers();
			MaskUpdated maskUpdated = this.onMaskUpdated;
			if (maskUpdated != null)
			{
				maskUpdated(this.mask, quality, state);
			}
			Action<PlayerClothing> onMaskChanged_Global = PlayerClothing.OnMaskChanged_Global;
			if (onMaskChanged_Global != null)
			{
				onMaskChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.maskAsset);
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000DB61E File Offset: 0x000D981E
		[Obsolete]
		public void askSwapMask(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapMaskRequest(page, x, y);
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000DB62C File Offset: 0x000D982C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapMask")]
		public void ReceiveSwapMaskRequest(byte page, byte x, byte y)
		{
			if (page == 255)
			{
				if (this.maskAsset == null)
				{
					return;
				}
				this.askWearMask(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.MASK)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearMask(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000DB6D8 File Offset: 0x000D98D8
		public void askWearMask(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemMaskAsset asset = Assets.find(EAssetType.ITEM, id) as ItemMaskAsset;
			this.askWearMask(asset, quality, state, playEffect);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000DB700 File Offset: 0x000D9900
		public void askWearMask(ItemMaskAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort mask = this.mask;
			byte newQuality = this.maskQuality;
			byte[] newState = this.maskState;
			PlayerClothing.SendWearMask.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (mask != 0)
			{
				base.player.inventory.forceAddItem(new Item(mask, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x000DB769 File Offset: 0x000D9969
		public void sendSwapMask(byte page, byte x, byte y)
		{
			if (page == 255 && this.maskAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapMaskRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000DB790 File Offset: 0x000D9990
		[Obsolete]
		public void tellWearGlasses(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			Asset asset = Assets.find(EAssetType.ITEM, id);
			this.ReceiveWearGlasses((asset != null) ? asset.GUID : Guid.Empty, quality, state, false);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000DB7C0 File Offset: 0x000D99C0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWearGlasses")]
		public void ReceiveWearGlasses(Guid id, byte quality, byte[] state, bool playEffect)
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			this.thirdClothes.glassesGuid = id;
			this.glassesQuality = quality;
			this.glassesState = state;
			this.thirdClothes.apply();
			if (this.characterClothes != null)
			{
				this.characterClothes.glassesGuid = id;
				this.characterClothes.apply();
				Characters.active.glasses = this.glasses;
			}
			GlassesUpdated glassesUpdated = this.onGlassesUpdated;
			if (glassesUpdated != null)
			{
				glassesUpdated(this.glasses, quality, state);
			}
			this.UpdateStatModifiers();
			Action<PlayerClothing> onGlassesChanged_Global = PlayerClothing.OnGlassesChanged_Global;
			if (onGlassesChanged_Global != null)
			{
				onGlassesChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.glassesAsset);
			}
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000DB88A File Offset: 0x000D9A8A
		[Obsolete]
		public void askSwapGlasses(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveSwapGlassesRequest(page, x, y);
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000DB898 File Offset: 0x000D9A98
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapGlasses")]
		public void ReceiveSwapGlassesRequest(byte page, byte x, byte y)
		{
			if (page == 255)
			{
				if (this.glassesAsset == null)
				{
					return;
				}
				this.askWearGlasses(0, 0, new byte[0], true);
				return;
			}
			else
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				ItemAsset asset = item.GetAsset();
				if (asset != null && asset.type == EItemType.GLASSES)
				{
					base.player.inventory.removeItem(page, index);
					this.askWearGlasses(item.item.id, item.item.quality, item.item.state, true);
				}
				return;
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000DB944 File Offset: 0x000D9B44
		public void askWearGlasses(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ItemGlassesAsset asset = Assets.find(EAssetType.ITEM, id) as ItemGlassesAsset;
			this.askWearGlasses(asset, quality, state, playEffect);
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000DB96C File Offset: 0x000D9B6C
		public void askWearGlasses(ItemGlassesAsset asset, byte quality, byte[] state, bool playEffect)
		{
			ushort glasses = this.glasses;
			byte newQuality = this.glassesQuality;
			byte[] newState = this.glassesState;
			PlayerClothing.SendWearGlasses.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (asset != null) ? asset.GUID : Guid.Empty, quality, state, playEffect);
			if (glasses != 0)
			{
				base.player.inventory.forceAddItem(new Item(glasses, 1, newQuality, newState), false);
			}
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000DB9D5 File Offset: 0x000D9BD5
		public void sendSwapGlasses(byte page, byte x, byte y)
		{
			if (page == 255 && this.glassesAsset == null)
			{
				return;
			}
			PlayerClothing.SendSwapGlassesRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000DB9FC File Offset: 0x000D9BFC
		[Obsolete]
		public void tellClothing(CSteamID steamID, ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState, bool newVisual, bool newSkinned, bool newMythic)
		{
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000DBA00 File Offset: 0x000D9C00
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveClothingState(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid shirtGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref shirtGuid);
			byte newShirtQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newShirtQuality);
			byte[] newShirtState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newShirtState);
			Guid pantsGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref pantsGuid);
			byte newPantsQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newPantsQuality);
			byte[] newPantsState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newPantsState);
			Guid hatGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref hatGuid);
			byte newHatQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHatQuality);
			byte[] newHatState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newHatState);
			Guid backpackGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref backpackGuid);
			byte newBackpackQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newBackpackQuality);
			byte[] newBackpackState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newBackpackState);
			Guid vestGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref vestGuid);
			byte newVestQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newVestQuality);
			byte[] newVestState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newVestState);
			Guid maskGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref maskGuid);
			byte newMaskQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newMaskQuality);
			byte[] newMaskState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newMaskState);
			Guid glassesGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref glassesGuid);
			byte newGlassesQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newGlassesQuality);
			byte[] newGlassesState;
			SystemNetPakReaderEx.ReadStateArray(reader, ref newGlassesState);
			bool isVisual;
			reader.ReadBit(ref isVisual);
			bool isSkinned;
			reader.ReadBit(ref isSkinned);
			bool isMythic;
			reader.ReadBit(ref isMythic);
			base.player.animator.NotifyClothingIsVisible();
			if (base.channel.IsLocalPlayer)
			{
				Player.isLoadingClothing = false;
			}
			if (this.thirdClothes != null)
			{
				this.thirdClothes.face = base.channel.owner.face;
				this.thirdClothes.hair = base.channel.owner.hair;
				this.thirdClothes.beard = base.channel.owner.beard;
				this.thirdClothes.skin = base.channel.owner.skin;
				this.thirdClothes.color = base.channel.owner.color;
				this.thirdClothes.shirtGuid = shirtGuid;
				this.shirtQuality = newShirtQuality;
				this.shirtState = newShirtState;
				this.thirdClothes.pantsGuid = pantsGuid;
				this.pantsQuality = newPantsQuality;
				this.pantsState = newPantsState;
				this.thirdClothes.hatGuid = hatGuid;
				this.hatQuality = newHatQuality;
				this.hatState = newHatState;
				this.thirdClothes.backpackGuid = backpackGuid;
				this.backpackQuality = newBackpackQuality;
				this.backpackState = newBackpackState;
				this.thirdClothes.vestGuid = vestGuid;
				this.vestQuality = newVestQuality;
				this.vestState = newVestState;
				this.thirdClothes.maskGuid = maskGuid;
				this.maskQuality = newMaskQuality;
				this.maskState = newMaskState;
				this.thirdClothes.glassesGuid = glassesGuid;
				this.glassesQuality = newGlassesQuality;
				this.glassesState = newGlassesState;
				this.thirdClothes.isVisual = isVisual;
				this.thirdClothes.isMythic = isMythic;
				this.thirdClothes.apply();
			}
			if (this.firstClothes != null)
			{
				this.firstClothes.skin = base.channel.owner.skin;
				this.firstClothes.shirtGuid = shirtGuid;
				this.firstClothes.isVisual = isVisual;
				this.firstClothes.isMythic = isMythic;
				this.firstClothes.apply();
			}
			if (this.characterClothes != null)
			{
				this.characterClothes.face = base.channel.owner.face;
				this.characterClothes.hair = base.channel.owner.hair;
				this.characterClothes.beard = base.channel.owner.beard;
				this.characterClothes.skin = base.channel.owner.skin;
				this.characterClothes.color = base.channel.owner.color;
				this.characterClothes.shirtGuid = shirtGuid;
				this.characterClothes.pantsGuid = pantsGuid;
				this.characterClothes.hatGuid = hatGuid;
				this.characterClothes.backpackGuid = backpackGuid;
				this.characterClothes.vestGuid = vestGuid;
				this.characterClothes.maskGuid = maskGuid;
				this.characterClothes.glassesGuid = glassesGuid;
				this.characterClothes.isVisual = isVisual;
				this.characterClothes.isMythic = isMythic;
				this.characterClothes.apply();
				Characters.active.shirt = this.shirt;
				Characters.active.pants = this.pants;
				Characters.active.hat = this.hat;
				Characters.active.backpack = this.backpack;
				Characters.active.vest = this.vest;
				Characters.active.mask = this.mask;
				Characters.active.glasses = this.glasses;
				Characters.hasPlayed = true;
			}
			this.isSkinned = isSkinned;
			base.player.equipment.applySkinVisual();
			base.player.equipment.applyMythicVisual();
			this.UpdateStatModifiers();
			ShirtUpdated shirtUpdated = this.onShirtUpdated;
			if (shirtUpdated != null)
			{
				shirtUpdated(this.shirt, newShirtQuality, newShirtState);
			}
			Action<PlayerClothing> onShirtChanged_Global = PlayerClothing.OnShirtChanged_Global;
			if (onShirtChanged_Global != null)
			{
				onShirtChanged_Global.Invoke(this);
			}
			PantsUpdated pantsUpdated = this.onPantsUpdated;
			if (pantsUpdated != null)
			{
				pantsUpdated(this.pants, newPantsQuality, newPantsState);
			}
			Action<PlayerClothing> onPantsChanged_Global = PlayerClothing.OnPantsChanged_Global;
			if (onPantsChanged_Global != null)
			{
				onPantsChanged_Global.Invoke(this);
			}
			HatUpdated hatUpdated = this.onHatUpdated;
			if (hatUpdated != null)
			{
				hatUpdated(this.hat, newHatQuality, newHatState);
			}
			Action<PlayerClothing> onHatChanged_Global = PlayerClothing.OnHatChanged_Global;
			if (onHatChanged_Global != null)
			{
				onHatChanged_Global.Invoke(this);
			}
			BackpackUpdated backpackUpdated = this.onBackpackUpdated;
			if (backpackUpdated != null)
			{
				backpackUpdated(this.backpack, newBackpackQuality, newBackpackState);
			}
			Action<PlayerClothing> onBackpackChanged_Global = PlayerClothing.OnBackpackChanged_Global;
			if (onBackpackChanged_Global != null)
			{
				onBackpackChanged_Global.Invoke(this);
			}
			VestUpdated vestUpdated = this.onVestUpdated;
			if (vestUpdated != null)
			{
				vestUpdated(this.vest, newVestQuality, newVestState);
			}
			Action<PlayerClothing> onVestChanged_Global = PlayerClothing.OnVestChanged_Global;
			if (onVestChanged_Global != null)
			{
				onVestChanged_Global.Invoke(this);
			}
			MaskUpdated maskUpdated = this.onMaskUpdated;
			if (maskUpdated != null)
			{
				maskUpdated(this.mask, newMaskQuality, newMaskState);
			}
			Action<PlayerClothing> onMaskChanged_Global = PlayerClothing.OnMaskChanged_Global;
			if (onMaskChanged_Global != null)
			{
				onMaskChanged_Global.Invoke(this);
			}
			GlassesUpdated glassesUpdated = this.onGlassesUpdated;
			if (glassesUpdated != null)
			{
				glassesUpdated(this.glasses, newGlassesQuality, newGlassesState);
			}
			Action<PlayerClothing> onGlassesChanged_Global = PlayerClothing.OnGlassesChanged_Global;
			if (onGlassesChanged_Global != null)
			{
				onGlassesChanged_Global.Invoke(this);
			}
			if (base.channel.IsLocalPlayer && this.thirdClothes != null && !Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.shirtAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.pantsAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.hatAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.backpackAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.vestAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.maskAsset);
				ClientAssetIntegrity.QueueRequest(this.thirdClothes.glassesAsset);
			}
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000DC06C File Offset: 0x000DA26C
		public void updateClothes(ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			Asset newShirtAsset = Assets.find(EAssetType.ITEM, newShirt);
			Asset newPantsAsset = Assets.find(EAssetType.ITEM, newPants);
			Asset newHatAsset = Assets.find(EAssetType.ITEM, newHat);
			Asset newBackpackAsset = Assets.find(EAssetType.ITEM, newBackpack);
			Asset newVestAsset = Assets.find(EAssetType.ITEM, newVest);
			Asset newMaskAsset = Assets.find(EAssetType.ITEM, newMask);
			Asset newGlassesAsset = Assets.find(EAssetType.ITEM, newGlasses);
			PlayerClothing.SendClothingState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
			{
				Asset newShirtAsset = newShirtAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newShirtAsset != null) ? newShirtAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newShirtQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newShirtState);
				Asset newPantsAsset = newPantsAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newPantsAsset != null) ? newPantsAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newPantsQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newPantsState);
				Asset newHatAsset = newHatAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newHatAsset != null) ? newHatAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newHatQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newHatState);
				Asset newBackpackAsset = newBackpackAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newBackpackAsset != null) ? newBackpackAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newBackpackQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newBackpackState);
				Asset newVestAsset = newVestAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newVestAsset != null) ? newVestAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newVestQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newVestState);
				Asset newMaskAsset = newMaskAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newMaskAsset != null) ? newMaskAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newMaskQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newMaskState);
				Asset newGlassesAsset = newGlassesAsset;
				SystemNetPakWriterEx.WriteGuid(writer, (newGlassesAsset != null) ? newGlassesAsset.GUID : Guid.Empty);
				SystemNetPakWriterEx.WriteUInt8(writer, newGlassesQuality);
				SystemNetPakWriterEx.WriteStateArray(writer, newGlassesState);
				writer.WriteBit(this.isVisual);
				writer.WriteBit(this.isSkinned);
				writer.WriteBit(this.isMythic);
			});
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000DC177 File Offset: 0x000DA377
		[Obsolete]
		public void askClothing(CSteamID steamID)
		{
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000DC17C File Offset: 0x000DA37C
		private void WriteClothingState(NetPakWriter writer)
		{
			ItemShirtAsset shirtAsset = this.shirtAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (shirtAsset != null) ? shirtAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.shirtQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.shirtState);
			ItemPantsAsset pantsAsset = this.pantsAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (pantsAsset != null) ? pantsAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.pantsQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.pantsState);
			ItemHatAsset hatAsset = this.hatAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (hatAsset != null) ? hatAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.hatQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.hatState);
			ItemBackpackAsset backpackAsset = this.backpackAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (backpackAsset != null) ? backpackAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.backpackQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.backpackState);
			ItemVestAsset vestAsset = this.vestAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (vestAsset != null) ? vestAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.vestQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.vestState);
			ItemMaskAsset maskAsset = this.maskAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (maskAsset != null) ? maskAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.maskQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.maskState);
			ItemGlassesAsset glassesAsset = this.glassesAsset;
			SystemNetPakWriterEx.WriteGuid(writer, (glassesAsset != null) ? glassesAsset.GUID : Guid.Empty);
			SystemNetPakWriterEx.WriteUInt8(writer, this.glassesQuality);
			SystemNetPakWriterEx.WriteStateArray(writer, this.glassesState);
			writer.WriteBit(this.isVisual);
			writer.WriteBit(this.isSkinned);
			writer.WriteBit(this.isMythic);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000DC331 File Offset: 0x000DA531
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			PlayerClothing.SendClothingState.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, new Action<NetPakWriter>(this.WriteClothingState));
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x000DC356 File Offset: 0x000DA556
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			PlayerClothing.SendClothingState.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, new Action<NetPakWriter>(this.WriteClothingState));
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000DC376 File Offset: 0x000DA576
		[Obsolete]
		public void tellSwapFace(CSteamID steamID, byte index)
		{
			this.ReceiveFaceState(index);
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000DC380 File Offset: 0x000DA580
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSwapFace")]
		public void ReceiveFaceState(byte index)
		{
			base.channel.owner.face = index;
			if (this.thirdClothes != null)
			{
				this.thirdClothes.face = base.channel.owner.face;
				this.thirdClothes.apply();
			}
			if (this.characterClothes != null)
			{
				this.characterClothes.face = base.channel.owner.face;
				this.characterClothes.apply();
			}
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000DC408 File Offset: 0x000DA608
		public bool ServerSetFace(byte index)
		{
			if (index >= Customization.FACES_FREE + Customization.FACES_PRO)
			{
				return false;
			}
			if (!base.channel.owner.isPro && index >= Customization.FACES_FREE)
			{
				return false;
			}
			PlayerClothing.SendFaceState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), index);
			return true;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000DC459 File Offset: 0x000DA659
		[Obsolete]
		public void askSwapFace(CSteamID steamID, byte index)
		{
			this.ReceiveSwapFaceRequest(index);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000DC462 File Offset: 0x000DA662
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSwapFace")]
		public void ReceiveSwapFaceRequest(byte index)
		{
			this.ServerSetFace(index);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000DC46C File Offset: 0x000DA66C
		public void sendSwapFace(byte index)
		{
			PlayerClothing.SendSwapFaceRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, index);
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000DC480 File Offset: 0x000DA680
		private void onStanceUpdated()
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			if (base.player.movement.getVehicle() != null)
			{
				this.thirdClothes.hasBackpack = (base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].obj == null);
				return;
			}
			this.thirdClothes.hasBackpack = true;
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000DC500 File Offset: 0x000DA700
		private void onLifeUpdated(bool isDead)
		{
			if (isDead && Provider.isServer && (base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Clothes_PvP : Provider.modeConfigData.Players.Lose_Clothes_PvE))
			{
				if (this.shirtAsset != null && this.shirtAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), base.transform.position, false, true, true);
				}
				if (this.pantsAsset != null && this.pantsAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), base.transform.position, false, true, true);
				}
				if (this.hatAsset != null && this.hatAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.hat, 1, this.hatQuality, this.hatState), base.transform.position, false, true, true);
				}
				if (this.backpackAsset != null && this.backpackAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), base.transform.position, false, true, true);
				}
				if (this.vestAsset != null && this.vestAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.vest, 1, this.vestQuality, this.vestState), base.transform.position, false, true, true);
				}
				if (this.maskAsset != null && this.maskAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.mask, 1, this.maskQuality, this.maskState), base.transform.position, false, true, true);
				}
				if (this.glassesAsset != null && this.glassesAsset.shouldDropOnDeath)
				{
					ItemManager.dropItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), base.transform.position, false, true, true);
				}
				this.thirdClothes.shirtAsset = null;
				this.shirtQuality = 0;
				this.thirdClothes.pantsAsset = null;
				this.pantsQuality = 0;
				this.thirdClothes.hatAsset = null;
				this.hatQuality = 0;
				this.thirdClothes.backpackAsset = null;
				this.backpackQuality = 0;
				this.thirdClothes.vestAsset = null;
				this.vestQuality = 0;
				this.thirdClothes.maskAsset = null;
				this.maskQuality = 0;
				this.thirdClothes.glassesAsset = null;
				this.glassesQuality = 0;
				this.shirtState = new byte[0];
				this.pantsState = new byte[0];
				this.hatState = new byte[0];
				this.backpackState = new byte[0];
				this.vestState = new byte[0];
				this.maskState = new byte[0];
				this.glassesState = new byte[0];
				PlayerClothing.SendClothingState.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), new Action<NetPakWriter>(this.WriteClothingState));
			}
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000DC810 File Offset: 0x000DAA10
		internal void InitializePlayer()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (base.player.first != null)
				{
					this.firstClothes = base.player.first.Find("Camera").Find("Viewmodel").GetComponent<HumanClothes>();
					this.firstClothes.isMine = true;
				}
				if (base.player.third != null)
				{
					this.thirdClothes = base.player.third.GetComponent<HumanClothes>();
				}
				if (base.player.character != null)
				{
					this.characterClothes = base.player.character.GetComponent<HumanClothes>();
				}
			}
			else if (base.player.third != null)
			{
				this.thirdClothes = base.player.third.GetComponent<HumanClothes>();
			}
			if (this.firstClothes != null)
			{
				this.firstClothes.visualShirt = base.channel.owner.shirtItem;
				this.firstClothes.hand = base.channel.owner.IsLeftHanded;
			}
			if (this.thirdClothes != null)
			{
				this.thirdClothes.visualShirt = base.channel.owner.shirtItem;
				this.thirdClothes.visualPants = base.channel.owner.pantsItem;
				this.thirdClothes.visualHat = base.channel.owner.hatItem;
				this.thirdClothes.visualBackpack = base.channel.owner.backpackItem;
				this.thirdClothes.visualVest = base.channel.owner.vestItem;
				this.thirdClothes.visualMask = base.channel.owner.maskItem;
				this.thirdClothes.visualGlasses = base.channel.owner.glassesItem;
				this.thirdClothes.hand = base.channel.owner.IsLeftHanded;
			}
			if (this.characterClothes != null)
			{
				this.characterClothes.visualShirt = base.channel.owner.shirtItem;
				this.characterClothes.visualPants = base.channel.owner.pantsItem;
				this.characterClothes.visualHat = base.channel.owner.hatItem;
				this.characterClothes.visualBackpack = base.channel.owner.backpackItem;
				this.characterClothes.visualVest = base.channel.owner.vestItem;
				this.characterClothes.visualMask = base.channel.owner.maskItem;
				this.characterClothes.visualGlasses = base.channel.owner.glassesItem;
				this.characterClothes.hand = base.channel.owner.IsLeftHanded;
			}
			this.isSkinned = true;
			if (Provider.isServer)
			{
				this.load();
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000DCB4C File Offset: 0x000DAD4C
		public void load()
		{
			this.wasLoadCalled = true;
			this.thirdClothes.visualShirt = base.channel.owner.shirtItem;
			this.thirdClothes.visualPants = base.channel.owner.pantsItem;
			this.thirdClothes.visualHat = base.channel.owner.hatItem;
			this.thirdClothes.visualBackpack = base.channel.owner.backpackItem;
			this.thirdClothes.visualVest = base.channel.owner.vestItem;
			this.thirdClothes.visualMask = base.channel.owner.maskItem;
			this.thirdClothes.visualGlasses = base.channel.owner.glassesItem;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Clothing.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Clothing.dat", 0);
				byte b = block.readByte();
				if (b > 1)
				{
					if (b > 6)
					{
						this.thirdClothes.shirtGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.shirt = block.readUInt16();
					}
					this.shirtQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.pantsGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.pants = block.readUInt16();
					}
					this.pantsQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.hatGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.hat = block.readUInt16();
					}
					this.hatQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.backpackGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.backpack = block.readUInt16();
					}
					this.backpackQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.vestGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.vest = block.readUInt16();
					}
					this.vestQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.maskGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.mask = block.readUInt16();
					}
					this.maskQuality = block.readByte();
					if (b > 6)
					{
						this.thirdClothes.glassesGuid = block.readGUID();
					}
					else
					{
						this.thirdClothes.glasses = block.readUInt16();
					}
					this.glassesQuality = block.readByte();
					if (b > 2)
					{
						this.thirdClothes.isVisual = block.readBoolean();
					}
					if (b > 5)
					{
						this.isSkinned = block.readBoolean();
						this.thirdClothes.isMythic = block.readBoolean();
					}
					else
					{
						this.isSkinned = true;
						this.thirdClothes.isMythic = true;
					}
					if (b > 4)
					{
						this.shirtState = block.readByteArray();
						this.pantsState = block.readByteArray();
						this.hatState = block.readByteArray();
						this.backpackState = block.readByteArray();
						this.vestState = block.readByteArray();
						this.maskState = block.readByteArray();
						this.glassesState = block.readByteArray();
					}
					else
					{
						this.shirtState = new byte[0];
						this.pantsState = new byte[0];
						this.hatState = new byte[0];
						this.backpackState = new byte[0];
						this.vestState = new byte[0];
						this.maskState = new byte[0];
						this.glassesState = new byte[0];
						if (this.glasses == 334)
						{
							this.glassesState = new byte[1];
						}
					}
					this.thirdClothes.apply();
					this.UpdateStatModifiers();
					return;
				}
			}
			this.thirdClothes.shirtAsset = null;
			this.shirtQuality = 0;
			this.thirdClothes.pantsAsset = null;
			this.pantsQuality = 0;
			this.thirdClothes.hatAsset = null;
			this.hatQuality = 0;
			this.thirdClothes.backpackAsset = null;
			this.backpackQuality = 0;
			this.thirdClothes.vestAsset = null;
			this.vestQuality = 0;
			this.thirdClothes.maskAsset = null;
			this.maskQuality = 0;
			this.thirdClothes.glassesAsset = null;
			this.glassesQuality = 0;
			this.shirtState = new byte[0];
			this.pantsState = new byte[0];
			this.hatState = new byte[0];
			this.backpackState = new byte[0];
			this.vestState = new byte[0];
			this.maskState = new byte[0];
			this.glassesState = new byte[0];
			this.thirdClothes.apply();
			this.UpdateStatModifiers();
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000DCFF0 File Offset: 0x000DB1F0
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			bool flag = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Clothes_PvP : Provider.modeConfigData.Players.Lose_Clothes_PvE;
			if ((base.player.life.isDead && flag) || this.thirdClothes == null)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Clothing.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Clothing.dat");
					return;
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerClothing.SAVEDATA_VERSION);
				block.writeGUID(this.thirdClothes.shirtGuid);
				block.writeByte(this.shirtQuality);
				block.writeGUID(this.thirdClothes.pantsGuid);
				block.writeByte(this.pantsQuality);
				block.writeGUID(this.thirdClothes.hatGuid);
				block.writeByte(this.hatQuality);
				block.writeGUID(this.thirdClothes.backpackGuid);
				block.writeByte(this.backpackQuality);
				block.writeGUID(this.thirdClothes.vestGuid);
				block.writeByte(this.vestQuality);
				block.writeGUID(this.thirdClothes.maskGuid);
				block.writeByte(this.maskQuality);
				block.writeGUID(this.thirdClothes.glassesGuid);
				block.writeByte(this.glassesQuality);
				block.writeBoolean(this.isVisual);
				block.writeBoolean(this.isSkinned);
				block.writeBoolean(this.isMythic);
				block.writeByteArray(this.shirtState);
				block.writeByteArray(this.pantsState);
				block.writeByteArray(this.hatState);
				block.writeByteArray(this.backpackState);
				block.writeByteArray(this.vestState);
				block.writeByteArray(this.maskState);
				block.writeByteArray(this.glassesState);
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Clothing.dat", block);
			}
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000DD204 File Offset: 0x000DB404
		private void UpdateStatModifiers()
		{
			this.movementSpeedMultiplier = 1f;
			this.fallingDamageMultiplier = 1f;
			this.preventsFallingBrokenBones = false;
			if (this.thirdClothes != null)
			{
				float num = this.movementSpeedMultiplier;
				ItemShirtAsset shirtAsset = this.thirdClothes.shirtAsset;
				this.movementSpeedMultiplier = num * ((shirtAsset != null) ? shirtAsset.movementSpeedMultiplier : 1f);
				float num2 = this.movementSpeedMultiplier;
				ItemPantsAsset pantsAsset = this.thirdClothes.pantsAsset;
				this.movementSpeedMultiplier = num2 * ((pantsAsset != null) ? pantsAsset.movementSpeedMultiplier : 1f);
				float num3 = this.movementSpeedMultiplier;
				ItemHatAsset hatAsset = this.thirdClothes.hatAsset;
				this.movementSpeedMultiplier = num3 * ((hatAsset != null) ? hatAsset.movementSpeedMultiplier : 1f);
				float num4 = this.movementSpeedMultiplier;
				ItemBackpackAsset backpackAsset = this.thirdClothes.backpackAsset;
				this.movementSpeedMultiplier = num4 * ((backpackAsset != null) ? backpackAsset.movementSpeedMultiplier : 1f);
				float num5 = this.movementSpeedMultiplier;
				ItemVestAsset vestAsset = this.thirdClothes.vestAsset;
				this.movementSpeedMultiplier = num5 * ((vestAsset != null) ? vestAsset.movementSpeedMultiplier : 1f);
				float num6 = this.movementSpeedMultiplier;
				ItemMaskAsset maskAsset = this.thirdClothes.maskAsset;
				this.movementSpeedMultiplier = num6 * ((maskAsset != null) ? maskAsset.movementSpeedMultiplier : 1f);
				float num7 = this.movementSpeedMultiplier;
				ItemGlassesAsset glassesAsset = this.thirdClothes.glassesAsset;
				this.movementSpeedMultiplier = num7 * ((glassesAsset != null) ? glassesAsset.movementSpeedMultiplier : 1f);
				float num8 = this.fallingDamageMultiplier;
				ItemShirtAsset shirtAsset2 = this.thirdClothes.shirtAsset;
				this.fallingDamageMultiplier = num8 * ((shirtAsset2 != null) ? shirtAsset2.fallingDamageMultiplier : 1f);
				float num9 = this.fallingDamageMultiplier;
				ItemPantsAsset pantsAsset2 = this.thirdClothes.pantsAsset;
				this.fallingDamageMultiplier = num9 * ((pantsAsset2 != null) ? pantsAsset2.fallingDamageMultiplier : 1f);
				float num10 = this.fallingDamageMultiplier;
				ItemHatAsset hatAsset2 = this.thirdClothes.hatAsset;
				this.fallingDamageMultiplier = num10 * ((hatAsset2 != null) ? hatAsset2.fallingDamageMultiplier : 1f);
				float num11 = this.fallingDamageMultiplier;
				ItemBackpackAsset backpackAsset2 = this.thirdClothes.backpackAsset;
				this.fallingDamageMultiplier = num11 * ((backpackAsset2 != null) ? backpackAsset2.fallingDamageMultiplier : 1f);
				float num12 = this.fallingDamageMultiplier;
				ItemVestAsset vestAsset2 = this.thirdClothes.vestAsset;
				this.fallingDamageMultiplier = num12 * ((vestAsset2 != null) ? vestAsset2.fallingDamageMultiplier : 1f);
				float num13 = this.fallingDamageMultiplier;
				ItemMaskAsset maskAsset2 = this.thirdClothes.maskAsset;
				this.fallingDamageMultiplier = num13 * ((maskAsset2 != null) ? maskAsset2.fallingDamageMultiplier : 1f);
				float num14 = this.fallingDamageMultiplier;
				ItemGlassesAsset glassesAsset2 = this.thirdClothes.glassesAsset;
				this.fallingDamageMultiplier = num14 * ((glassesAsset2 != null) ? glassesAsset2.fallingDamageMultiplier : 1f);
				bool flag = this.preventsFallingBrokenBones;
				ItemShirtAsset shirtAsset3 = this.thirdClothes.shirtAsset;
				this.preventsFallingBrokenBones = (flag | (shirtAsset3 != null && shirtAsset3.preventsFallingBrokenBones));
				bool flag2 = this.preventsFallingBrokenBones;
				ItemPantsAsset pantsAsset3 = this.thirdClothes.pantsAsset;
				this.preventsFallingBrokenBones = (flag2 | (pantsAsset3 != null && pantsAsset3.preventsFallingBrokenBones));
				bool flag3 = this.preventsFallingBrokenBones;
				ItemHatAsset hatAsset3 = this.thirdClothes.hatAsset;
				this.preventsFallingBrokenBones = (flag3 | (hatAsset3 != null && hatAsset3.preventsFallingBrokenBones));
				bool flag4 = this.preventsFallingBrokenBones;
				ItemBackpackAsset backpackAsset3 = this.thirdClothes.backpackAsset;
				this.preventsFallingBrokenBones = (flag4 | (backpackAsset3 != null && backpackAsset3.preventsFallingBrokenBones));
				bool flag5 = this.preventsFallingBrokenBones;
				ItemVestAsset vestAsset3 = this.thirdClothes.vestAsset;
				this.preventsFallingBrokenBones = (flag5 | (vestAsset3 != null && vestAsset3.preventsFallingBrokenBones));
				bool flag6 = this.preventsFallingBrokenBones;
				ItemMaskAsset maskAsset3 = this.thirdClothes.maskAsset;
				this.preventsFallingBrokenBones = (flag6 | (maskAsset3 != null && maskAsset3.preventsFallingBrokenBones));
				bool flag7 = this.preventsFallingBrokenBones;
				ItemGlassesAsset glassesAsset3 = this.thirdClothes.glassesAsset;
				this.preventsFallingBrokenBones = (flag7 | (glassesAsset3 != null && glassesAsset3.preventsFallingBrokenBones));
			}
		}

		// Token: 0x04001C21 RID: 7201
		public static readonly byte SAVEDATA_VERSION = 7;

		// Token: 0x04001C22 RID: 7202
		public ShirtUpdated onShirtUpdated;

		// Token: 0x04001C23 RID: 7203
		public PantsUpdated onPantsUpdated;

		// Token: 0x04001C24 RID: 7204
		public HatUpdated onHatUpdated;

		// Token: 0x04001C25 RID: 7205
		public BackpackUpdated onBackpackUpdated;

		// Token: 0x04001C26 RID: 7206
		public VestUpdated onVestUpdated;

		// Token: 0x04001C27 RID: 7207
		public MaskUpdated onMaskUpdated;

		// Token: 0x04001C28 RID: 7208
		public GlassesUpdated onGlassesUpdated;

		// Token: 0x04001C35 RID: 7221
		public byte shirtQuality;

		// Token: 0x04001C36 RID: 7222
		public byte pantsQuality;

		// Token: 0x04001C37 RID: 7223
		public byte hatQuality;

		// Token: 0x04001C38 RID: 7224
		public byte backpackQuality;

		// Token: 0x04001C39 RID: 7225
		public byte vestQuality;

		// Token: 0x04001C3A RID: 7226
		public byte maskQuality;

		// Token: 0x04001C3B RID: 7227
		public byte glassesQuality;

		// Token: 0x04001C3C RID: 7228
		public byte[] shirtState;

		// Token: 0x04001C3D RID: 7229
		public byte[] pantsState;

		// Token: 0x04001C3E RID: 7230
		public byte[] hatState;

		// Token: 0x04001C3F RID: 7231
		public byte[] backpackState;

		// Token: 0x04001C40 RID: 7232
		public byte[] vestState;

		// Token: 0x04001C41 RID: 7233
		public byte[] maskState;

		// Token: 0x04001C42 RID: 7234
		public byte[] glassesState;

		// Token: 0x04001C43 RID: 7235
		private static readonly ClientInstanceMethod<byte> SendShirtQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveShirtQuality");

		// Token: 0x04001C44 RID: 7236
		private static readonly ClientInstanceMethod<byte> SendPantsQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceivePantsQuality");

		// Token: 0x04001C45 RID: 7237
		private static readonly ClientInstanceMethod<byte> SendHatQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveHatQuality");

		// Token: 0x04001C46 RID: 7238
		private static readonly ClientInstanceMethod<byte> SendBackpackQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveBackpackQuality");

		// Token: 0x04001C47 RID: 7239
		private static readonly ClientInstanceMethod<byte> SendVestQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveVestQuality");

		// Token: 0x04001C48 RID: 7240
		private static readonly ClientInstanceMethod<byte> SendMaskQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveMaskQuality");

		// Token: 0x04001C49 RID: 7241
		private static readonly ClientInstanceMethod<byte> SendGlassesQuality = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveGlassesQuality");

		// Token: 0x04001C4A RID: 7242
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearShirt = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearShirt");

		// Token: 0x04001C4B RID: 7243
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapShirtRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapShirtRequest");

		// Token: 0x04001C4C RID: 7244
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearPants = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearPants");

		// Token: 0x04001C4D RID: 7245
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapPantsRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapPantsRequest");

		// Token: 0x04001C4E RID: 7246
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearHat = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearHat");

		// Token: 0x04001C4F RID: 7247
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapHatRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapHatRequest");

		// Token: 0x04001C50 RID: 7248
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearBackpack = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearBackpack");

		// Token: 0x04001C51 RID: 7249
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapBackpackRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapBackpackRequest");

		// Token: 0x04001C52 RID: 7250
		private static readonly ClientInstanceMethod<EVisualToggleType, bool> SendVisualToggleState = ClientInstanceMethod<EVisualToggleType, bool>.Get(typeof(PlayerClothing), "ReceiveVisualToggleState");

		// Token: 0x04001C53 RID: 7251
		private static readonly ServerInstanceMethod<EVisualToggleType> SendVisualToggleRequest = ServerInstanceMethod<EVisualToggleType>.Get(typeof(PlayerClothing), "ReceiveVisualToggleRequest");

		// Token: 0x04001C54 RID: 7252
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearVest = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearVest");

		// Token: 0x04001C55 RID: 7253
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapVestRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapVestRequest");

		// Token: 0x04001C56 RID: 7254
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearMask = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearMask");

		// Token: 0x04001C57 RID: 7255
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapMaskRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapMaskRequest");

		// Token: 0x04001C58 RID: 7256
		private static readonly ClientInstanceMethod<Guid, byte, byte[], bool> SendWearGlasses = ClientInstanceMethod<Guid, byte, byte[], bool>.Get(typeof(PlayerClothing), "ReceiveWearGlasses");

		// Token: 0x04001C59 RID: 7257
		private static readonly ServerInstanceMethod<byte, byte, byte> SendSwapGlassesRequest = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerClothing), "ReceiveSwapGlassesRequest");

		// Token: 0x04001C5A RID: 7258
		private static readonly ClientInstanceMethod SendClothingState = ClientInstanceMethod.Get(typeof(PlayerClothing), "ReceiveClothingState");

		// Token: 0x04001C5B RID: 7259
		private static readonly ClientInstanceMethod<byte> SendFaceState = ClientInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveFaceState");

		// Token: 0x04001C5C RID: 7260
		private static readonly ServerInstanceMethod<byte> SendSwapFaceRequest = ServerInstanceMethod<byte>.Get(typeof(PlayerClothing), "ReceiveSwapFaceRequest");

		// Token: 0x04001C5D RID: 7261
		private bool wasLoadCalled;

		// Token: 0x04001C5E RID: 7262
		internal float movementSpeedMultiplier = 1f;

		// Token: 0x04001C5F RID: 7263
		internal float fallingDamageMultiplier = 1f;

		// Token: 0x04001C60 RID: 7264
		internal bool preventsFallingBrokenBones;
	}
}
