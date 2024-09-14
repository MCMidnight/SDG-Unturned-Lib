using System;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000353 RID: 851
	public class ObjectNPCAsset : ObjectAsset
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060019AD RID: 6573 RVA: 0x0005C822 File Offset: 0x0005AA22
		// (set) Token: 0x060019AE RID: 6574 RVA: 0x0005C82A File Offset: 0x0005AA2A
		public string npcName { get; protected set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x0005C833 File Offset: 0x0005AA33
		// (set) Token: 0x060019B0 RID: 6576 RVA: 0x0005C83B File Offset: 0x0005AA3B
		public NPCAssetOutfit defaultOutfit { get; protected set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0005C844 File Offset: 0x0005AA44
		// (set) Token: 0x060019B2 RID: 6578 RVA: 0x0005C84C File Offset: 0x0005AA4C
		public NPCAssetOutfit halloweenOutfit { get; protected set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0005C855 File Offset: 0x0005AA55
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x0005C85D File Offset: 0x0005AA5D
		public NPCAssetOutfit christmasOutfit { get; protected set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0005C868 File Offset: 0x0005AA68
		public NPCAssetOutfit currentOutfit
		{
			get
			{
				ENPCHoliday activeHoliday = HolidayUtil.getActiveHoliday();
				if (activeHoliday != ENPCHoliday.HALLOWEEN)
				{
					if (activeHoliday != ENPCHoliday.CHRISTMAS)
					{
						return this.defaultOutfit;
					}
					if (this.christmasOutfit == null)
					{
						return this.defaultOutfit;
					}
					return this.christmasOutfit;
				}
				else
				{
					if (this.halloweenOutfit == null)
					{
						return this.defaultOutfit;
					}
					return this.halloweenOutfit;
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x0005C8B7 File Offset: 0x0005AAB7
		// (set) Token: 0x060019B7 RID: 6583 RVA: 0x0005C8BF File Offset: 0x0005AABF
		public byte face { get; protected set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x0005C8C8 File Offset: 0x0005AAC8
		// (set) Token: 0x060019B9 RID: 6585 RVA: 0x0005C8D0 File Offset: 0x0005AAD0
		public byte hair { get; protected set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x0005C8D9 File Offset: 0x0005AAD9
		// (set) Token: 0x060019BB RID: 6587 RVA: 0x0005C8E1 File Offset: 0x0005AAE1
		public byte beard { get; protected set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x0005C8EA File Offset: 0x0005AAEA
		// (set) Token: 0x060019BD RID: 6589 RVA: 0x0005C8F2 File Offset: 0x0005AAF2
		public Color skin { get; protected set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x0005C8FB File Offset: 0x0005AAFB
		// (set) Token: 0x060019BF RID: 6591 RVA: 0x0005C903 File Offset: 0x0005AB03
		public Color color { get; protected set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0005C90C File Offset: 0x0005AB0C
		// (set) Token: 0x060019C1 RID: 6593 RVA: 0x0005C914 File Offset: 0x0005AB14
		public bool IsLeftHanded { get; protected set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0005C91D File Offset: 0x0005AB1D
		// (set) Token: 0x060019C3 RID: 6595 RVA: 0x0005C925 File Offset: 0x0005AB25
		[Obsolete]
		public bool isBackward
		{
			get
			{
				return this.IsLeftHanded;
			}
			protected set
			{
				this.IsLeftHanded = value;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0005C92E File Offset: 0x0005AB2E
		// (set) Token: 0x060019C5 RID: 6597 RVA: 0x0005C936 File Offset: 0x0005AB36
		[Obsolete]
		public ushort primary { get; protected set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0005C93F File Offset: 0x0005AB3F
		// (set) Token: 0x060019C7 RID: 6599 RVA: 0x0005C947 File Offset: 0x0005AB47
		[Obsolete]
		public ushort secondary { get; protected set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x0005C950 File Offset: 0x0005AB50
		// (set) Token: 0x060019C9 RID: 6601 RVA: 0x0005C958 File Offset: 0x0005AB58
		[Obsolete]
		public ushort tertiary { get; protected set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0005C961 File Offset: 0x0005AB61
		// (set) Token: 0x060019CB RID: 6603 RVA: 0x0005C969 File Offset: 0x0005AB69
		public ESlotType equipped { get; protected set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x0005C972 File Offset: 0x0005AB72
		// (set) Token: 0x060019CD RID: 6605 RVA: 0x0005C97A File Offset: 0x0005AB7A
		public ushort dialogue { [Obsolete] get; protected set; }

		// Token: 0x060019CE RID: 6606 RVA: 0x0005C983 File Offset: 0x0005AB83
		public bool IsDialogueRefNull()
		{
			return this.dialogue == 0 && GuidExtension.IsEmpty(this.dialogueGuid);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0005C99A File Offset: 0x0005AB9A
		public DialogueAsset FindDialogueAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<DialogueAsset>(this.dialogueGuid, this.dialogue);
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x0005C9AD File Offset: 0x0005ABAD
		// (set) Token: 0x060019D1 RID: 6609 RVA: 0x0005C9B5 File Offset: 0x0005ABB5
		public ENPCPose pose { get; protected set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0005C9BE File Offset: 0x0005ABBE
		// (set) Token: 0x060019D3 RID: 6611 RVA: 0x0005C9C6 File Offset: 0x0005ABC6
		public float poseLean { get; protected set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0005C9CF File Offset: 0x0005ABCF
		// (set) Token: 0x060019D5 RID: 6613 RVA: 0x0005C9D7 File Offset: 0x0005ABD7
		public float posePitch { get; protected set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0005C9E0 File Offset: 0x0005ABE0
		// (set) Token: 0x060019D7 RID: 6615 RVA: 0x0005C9E8 File Offset: 0x0005ABE8
		public float poseHeadOffset { get; protected set; }

		/// <summary>
		/// If non-zero, NPC name is shown as ??? until bool flag is true.
		/// </summary>
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x0005C9F1 File Offset: 0x0005ABF1
		// (set) Token: 0x060019D9 RID: 6617 RVA: 0x0005C9F9 File Offset: 0x0005ABF9
		public ushort playerKnowsNameFlagId { get; protected set; }

		// Token: 0x060019DA RID: 6618 RVA: 0x0005CA04 File Offset: 0x0005AC04
		public string GetNameShownToPlayer(Player player)
		{
			if (player == null || this.playerKnowsNameFlagId == 0)
			{
				return this.npcName;
			}
			short num;
			if (player.quests.getFlag(this.playerKnowsNameFlagId, out num) && num == 1)
			{
				return this.npcName;
			}
			return "???";
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0005CA50 File Offset: 0x0005AC50
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.npcName = localization.format("Character");
			this.npcName = ItemTool.filterRarityRichText(this.npcName);
			this.defaultOutfit = new NPCAssetOutfit(data, ENPCHoliday.NONE);
			if (data.ParseBool("Has_Halloween_Outfit", false))
			{
				this.halloweenOutfit = new NPCAssetOutfit(data, ENPCHoliday.HALLOWEEN);
			}
			if (data.ParseBool("Has_Christmas_Outfit", false))
			{
				this.christmasOutfit = new NPCAssetOutfit(data, ENPCHoliday.CHRISTMAS);
			}
			this.face = data.ParseUInt8("Face", 0);
			this.hair = data.ParseUInt8("Hair", 0);
			this.beard = data.ParseUInt8("Beard", 0);
			this.skin = Palette.hex(data.GetString("Color_Skin", null));
			this.color = Palette.hex(data.GetString("Color_Hair", null));
			this.IsLeftHanded = data.ContainsKey("Backward");
			this.primary = data.ParseGuidOrLegacyId("Primary", out this.primaryWeaponGuid);
			this.secondary = data.ParseGuidOrLegacyId("Secondary", out this.secondaryWeaponGuid);
			this.tertiary = data.ParseGuidOrLegacyId("Tertiary", out this.tertiaryWeaponGuid);
			if (data.ContainsKey("Equipped"))
			{
				this.equipped = (ESlotType)Enum.Parse(typeof(ESlotType), data.GetString("Equipped", null), true);
			}
			else
			{
				this.equipped = ESlotType.NONE;
			}
			this.dialogue = data.ParseGuidOrLegacyId("Dialogue", out this.dialogueGuid);
			if (data.ContainsKey("Pose"))
			{
				this.pose = (ENPCPose)Enum.Parse(typeof(ENPCPose), data.GetString("Pose", null), true);
			}
			else
			{
				this.pose = ENPCPose.STAND;
			}
			if (data.ContainsKey("Pose_Lean"))
			{
				this.poseLean = data.ParseFloat("Pose_Lean", 0f);
			}
			if (data.ContainsKey("Pose_Pitch"))
			{
				this.posePitch = data.ParseFloat("Pose_Pitch", 0f);
			}
			else
			{
				this.posePitch = 90f;
			}
			if (data.ContainsKey("Pose_Head_Offset"))
			{
				this.poseHeadOffset = data.ParseFloat("Pose_Head_Offset", 0f);
			}
			else if (this.pose == ENPCPose.CROUCH)
			{
				this.poseHeadOffset = 0.1f;
			}
			this.playerKnowsNameFlagId = data.ParseUInt16("PlayerKnowsNameFlagID", 0);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0005CCB4 File Offset: 0x0005AEB4
		[Obsolete("Server now tracks dialogue tree")]
		public bool doesPlayerHaveAccessToVendor(Player player, VendorAsset vendorAsset)
		{
			return true;
		}

		// Token: 0x04000BBA RID: 3002
		public Guid primaryWeaponGuid;

		// Token: 0x04000BBC RID: 3004
		public Guid secondaryWeaponGuid;

		// Token: 0x04000BBE RID: 3006
		public Guid tertiaryWeaponGuid;

		// Token: 0x04000BC1 RID: 3009
		public Guid dialogueGuid;
	}
}
