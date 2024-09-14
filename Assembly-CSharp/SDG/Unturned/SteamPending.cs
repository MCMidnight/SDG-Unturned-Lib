using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000692 RID: 1682
	public class SteamPending : SteamConnectedClientBase
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x0010B97B File Offset: 0x00109B7B
		public SteamPlayerID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x0010B983 File Offset: 0x00109B83
		public bool isPro
		{
			get
			{
				return this._isPro;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x0010B98B File Offset: 0x00109B8B
		public byte face
		{
			get
			{
				return this._face;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x0010B993 File Offset: 0x00109B93
		public byte hair
		{
			get
			{
				return this._hair;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060038B4 RID: 14516 RVA: 0x0010B99B File Offset: 0x00109B9B
		public byte beard
		{
			get
			{
				return this._beard;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x0010B9A3 File Offset: 0x00109BA3
		public Color skin
		{
			get
			{
				return this._skin;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x0010B9AB File Offset: 0x00109BAB
		public Color color
		{
			get
			{
				return this._color;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x0010B9B3 File Offset: 0x00109BB3
		public Color markerColor
		{
			get
			{
				return this._markerColor;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x0010B9BB File Offset: 0x00109BBB
		public bool hand
		{
			get
			{
				return this._hand;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060038B9 RID: 14521 RVA: 0x0010B9C3 File Offset: 0x00109BC3
		public bool canAcceptYet
		{
			get
			{
				return this.hasAuthentication && this.hasProof && this.hasGroup;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x0010B9DD File Offset: 0x00109BDD
		public EPlayerSkillset skillset
		{
			get
			{
				return this._skillset;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x0010B9E5 File Offset: 0x00109BE5
		public string language
		{
			get
			{
				return this._language;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060038BC RID: 14524 RVA: 0x0010B9ED File Offset: 0x00109BED
		public bool hasSentVerifyPacket
		{
			get
			{
				return this._hasSentVerifyPacket;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060038BD RID: 14525 RVA: 0x0010B9F5 File Offset: 0x00109BF5
		public float realtimeSinceSentVerifyPacket
		{
			get
			{
				return (float)(Time.realtimeSinceStartupAsDouble - this.sentVerifyPacketRealtime);
			}
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0010BA04 File Offset: 0x00109C04
		public void sendVerifyPacket()
		{
			if (this.hasSentVerifyPacket)
			{
				return;
			}
			this.sentVerifyPacketRealtime = Time.realtimeSinceStartupAsDouble;
			this._hasSentVerifyPacket = true;
			if (!this.playerID.steamID.IsValid())
			{
				return;
			}
			UnturnedLog.info(string.Format("Sending verification request to queued player {0}", this.playerID));
			NetMessages.SendMessageToClient(EClientMessage.Verify, ENetReliability.Reliable, base.transportConnection, delegate(NetPakWriter writer)
			{
			});
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060038BF RID: 14527 RVA: 0x0010BA83 File Offset: 0x00109C83
		// (set) Token: 0x060038C0 RID: 14528 RVA: 0x0010BA8B File Offset: 0x00109C8B
		public CSteamID lobbyID { get; private set; }

		// Token: 0x060038C1 RID: 14529 RVA: 0x0010BA94 File Offset: 0x00109C94
		public void inventoryDetailsReady()
		{
			this.shirtItem = this.getInventoryItem(this.packageShirt);
			this.pantsItem = this.getInventoryItem(this.packagePants);
			this.hatItem = this.getInventoryItem(this.packageHat);
			this.backpackItem = this.getInventoryItem(this.packageBackpack);
			this.vestItem = this.getInventoryItem(this.packageVest);
			this.maskItem = this.getInventoryItem(this.packageMask);
			this.glassesItem = this.getInventoryItem(this.packageGlasses);
			List<int> list = new List<int>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			for (int i = 0; i < this.packageSkins.Length; i++)
			{
				ulong num = this.packageSkins[i];
				if (num != 0UL)
				{
					int inventoryItem = this.getInventoryItem(num);
					if (inventoryItem != 0)
					{
						list.Add(inventoryItem);
						DynamicEconDetails dynamicEconDetails;
						if (this.dynamicInventoryDetails.TryGetValue(num, ref dynamicEconDetails))
						{
							list2.Add(dynamicEconDetails.tags);
							list3.Add(dynamicEconDetails.dynamic_props);
						}
						else
						{
							list2.Add(string.Empty);
							list3.Add(string.Empty);
						}
					}
				}
			}
			this.skinItems = list.ToArray();
			this.skinTags = list2.ToArray();
			this.skinDynamicProps = list3.ToArray();
			this.hasProof = true;
			if (this.canAcceptYet)
			{
				Provider.accept(this);
			}
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0010BBE4 File Offset: 0x00109DE4
		public int getInventoryItem(ulong package)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Length; i++)
				{
					if (this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID == package)
					{
						return this.inventoryDetails[i].m_iDefinition.m_SteamItemDef;
					}
				}
			}
			return 0;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0010BC40 File Offset: 0x00109E40
		public SteamPending(ITransportConnection transportConnection, SteamPlayerID newPlayerID, bool newPro, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, Color newMarkerColor, bool newHand, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ulong[] newPackageSkins, EPlayerSkillset newSkillset, string newLanguage, CSteamID newLobbyID, EClientPlatform clientPlatform)
		{
			base.transportConnection = transportConnection;
			this._playerID = newPlayerID;
			this._isPro = newPro;
			this._face = newFace;
			this._hair = newHair;
			this._beard = newBeard;
			this._skin = newSkin;
			this._color = newColor;
			this._markerColor = newMarkerColor;
			this._hand = newHand;
			this._skillset = newSkillset;
			this._language = newLanguage;
			this.packageShirt = newPackageShirt;
			this.packagePants = newPackagePants;
			this.packageHat = newPackageHat;
			this.packageBackpack = newPackageBackpack;
			this.packageVest = newPackageVest;
			this.packageMask = newPackageMask;
			this.packageGlasses = newPackageGlasses;
			this.packageSkins = newPackageSkins;
			this.lastReceivedPingRequestRealtime = Time.realtimeSinceStartup;
			this.sentVerifyPacketRealtime = -1.0;
			this.lobbyID = newLobbyID;
			this.clientPlatform = clientPlatform;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0010BD30 File Offset: 0x00109F30
		public SteamPending()
		{
			this._playerID = new SteamPlayerID(CSteamID.Nil, 0, "Player Name", "Character Name", "Nick Name", CSteamID.Nil);
			this.lastReceivedPingRequestRealtime = Time.realtimeSinceStartup;
			this.sentVerifyPacketRealtime = -1.0;
		}

		/// <summary>
		/// Used when kicking player in queue to log what backend system might be failing.
		/// </summary>
		// Token: 0x060038C5 RID: 14533 RVA: 0x0010BD98 File Offset: 0x00109F98
		internal string GetQueueStateDebugString()
		{
			if (!this.hasSentVerifyPacket)
			{
				return "normal waiting in queue";
			}
			if (this.canAcceptYet)
			{
				return string.Format("ready to accept from queue, elapsed: {0}s", this.realtimeSinceSentVerifyPacket);
			}
			return string.Format("hasAuthentication: {0} hasProof: {1} hasGroup: {2} elapsed: {3}s", new object[]
			{
				this.hasAuthentication,
				this.hasProof,
				this.hasGroup,
				this.realtimeSinceSentVerifyPacket
			});
		}

		// Token: 0x0400218A RID: 8586
		private SteamPlayerID _playerID;

		// Token: 0x0400218B RID: 8587
		private bool _isPro;

		// Token: 0x0400218C RID: 8588
		private byte _face;

		// Token: 0x0400218D RID: 8589
		private byte _hair;

		// Token: 0x0400218E RID: 8590
		private byte _beard;

		// Token: 0x0400218F RID: 8591
		private Color _skin;

		// Token: 0x04002190 RID: 8592
		private Color _color;

		// Token: 0x04002191 RID: 8593
		private Color _markerColor;

		// Token: 0x04002192 RID: 8594
		private bool _hand;

		// Token: 0x04002193 RID: 8595
		public int shirtItem;

		// Token: 0x04002194 RID: 8596
		public int pantsItem;

		// Token: 0x04002195 RID: 8597
		public int hatItem;

		// Token: 0x04002196 RID: 8598
		public int backpackItem;

		// Token: 0x04002197 RID: 8599
		public int vestItem;

		// Token: 0x04002198 RID: 8600
		public int maskItem;

		// Token: 0x04002199 RID: 8601
		public int glassesItem;

		// Token: 0x0400219A RID: 8602
		public int[] skinItems;

		// Token: 0x0400219B RID: 8603
		public string[] skinTags;

		// Token: 0x0400219C RID: 8604
		public string[] skinDynamicProps;

		// Token: 0x0400219D RID: 8605
		public ulong packageShirt;

		// Token: 0x0400219E RID: 8606
		public ulong packagePants;

		// Token: 0x0400219F RID: 8607
		public ulong packageHat;

		// Token: 0x040021A0 RID: 8608
		public ulong packageBackpack;

		// Token: 0x040021A1 RID: 8609
		public ulong packageVest;

		// Token: 0x040021A2 RID: 8610
		public ulong packageMask;

		// Token: 0x040021A3 RID: 8611
		public ulong packageGlasses;

		// Token: 0x040021A4 RID: 8612
		public ulong[] packageSkins;

		// Token: 0x040021A5 RID: 8613
		public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;

		// Token: 0x040021A6 RID: 8614
		public SteamItemDetails_t[] inventoryDetails;

		// Token: 0x040021A7 RID: 8615
		public Dictionary<ulong, DynamicEconDetails> dynamicInventoryDetails = new Dictionary<ulong, DynamicEconDetails>();

		// Token: 0x040021A8 RID: 8616
		public bool assignedPro;

		// Token: 0x040021A9 RID: 8617
		public bool assignedAdmin;

		// Token: 0x040021AA RID: 8618
		public bool hasAuthentication;

		// Token: 0x040021AB RID: 8619
		public bool hasProof;

		// Token: 0x040021AC RID: 8620
		public bool hasGroup;

		// Token: 0x040021AD RID: 8621
		private EPlayerSkillset _skillset;

		// Token: 0x040021AE RID: 8622
		private string _language;

		// Token: 0x040021AF RID: 8623
		public float lastReceivedPingRequestRealtime;

		// Token: 0x040021B0 RID: 8624
		private double sentVerifyPacketRealtime;

		// Token: 0x040021B1 RID: 8625
		private bool _hasSentVerifyPacket;

		// Token: 0x040021B3 RID: 8627
		internal EClientPlatform clientPlatform;

		// Token: 0x040021B4 RID: 8628
		internal int lastNotifiedQueuePosition;
	}
}
