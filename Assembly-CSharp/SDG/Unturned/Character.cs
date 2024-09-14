using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005AE RID: 1454
	public class Character
	{
		// Token: 0x06002F51 RID: 12113 RVA: 0x000D0298 File Offset: 0x000CE498
		public void applyHero()
		{
			this.shirt = 0;
			this.pants = 0;
			this.hat = 0;
			this.backpack = 0;
			this.vest = 0;
			this.mask = 0;
			this.glasses = 0;
			this.primaryItem = 0;
			this.primaryState = new byte[0];
			this.secondaryItem = 0;
			this.secondaryState = new byte[0];
			for (int i = 0; i < PlayerInventory.SKILLSETS_HERO[(int)((byte)this.skillset)].Length; i++)
			{
				ushort id = PlayerInventory.SKILLSETS_HERO[(int)((byte)this.skillset)][i];
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
				if (itemAsset != null)
				{
					switch (itemAsset.type)
					{
					case EItemType.HAT:
						this.hat = id;
						break;
					case EItemType.PANTS:
						this.pants = id;
						break;
					case EItemType.SHIRT:
						this.shirt = id;
						break;
					case EItemType.MASK:
						this.mask = id;
						break;
					case EItemType.BACKPACK:
						this.backpack = id;
						break;
					case EItemType.VEST:
						this.vest = id;
						break;
					case EItemType.GLASSES:
						this.glasses = id;
						break;
					case EItemType.GUN:
					case EItemType.MELEE:
						if (itemAsset.slot == ESlotType.PRIMARY)
						{
							this.primaryItem = id;
							this.primaryState = itemAsset.getState(EItemOrigin.ADMIN);
						}
						else
						{
							this.secondaryItem = id;
							this.secondaryState = itemAsset.getState(EItemOrigin.ADMIN);
						}
						break;
					}
				}
			}
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000D0404 File Offset: 0x000CE604
		public Character()
		{
			this.face = (byte)Random.Range(0, (int)Customization.FACES_FREE);
			this.hair = (byte)Random.Range(0, (int)Customization.HAIRS_FREE);
			this.beard = 0;
			this.skin = Customization.SKINS[Random.Range(0, Customization.SKINS.Length)];
			this.color = Customization.COLORS[Random.Range(0, Customization.COLORS.Length)];
			this.markerColor = Customization.MARKER_COLORS[Random.Range(0, Customization.MARKER_COLORS.Length)];
			this.hand = false;
			this.name = Provider.clientName;
			this.nick = Provider.clientName;
			this.group = CSteamID.Nil;
			this.skillset = (EPlayerSkillset)Random.Range(1, (int)Customization.SKILLSETS);
			this.applyHero();
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000D04D8 File Offset: 0x000CE6D8
		public Character(ushort newShirt, ushort newPants, ushort newHat, ushort newBackpack, ushort newVest, ushort newMask, ushort newGlasses, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ushort newPrimaryItem, byte[] newPrimaryState, ushort newSecondaryItem, byte[] newSecondaryState, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, Color newMarkerColor, bool newHand, string newName, string newNick, CSteamID newGroup, EPlayerSkillset newSkillset)
		{
			this.shirt = newShirt;
			this.pants = newPants;
			this.hat = newHat;
			this.backpack = newBackpack;
			this.vest = newVest;
			this.mask = newMask;
			this.glasses = newGlasses;
			this.packageShirt = newPackageShirt;
			this.packagePants = newPackagePants;
			this.packageHat = newPackageHat;
			this.packageBackpack = newPackageBackpack;
			this.packageVest = newPackageVest;
			this.packageMask = newPackageMask;
			this.packageGlasses = newPackageGlasses;
			this.primaryItem = newPrimaryItem;
			this.secondaryItem = newSecondaryItem;
			this.primaryState = newPrimaryState;
			this.secondaryState = newSecondaryState;
			this.face = newFace;
			this.hair = newHair;
			this.beard = newBeard;
			this.skin = newSkin;
			this.color = newColor;
			this.markerColor = newMarkerColor;
			this.hand = newHand;
			this.name = newName;
			this.nick = newNick;
			this.group = newGroup;
			this.skillset = newSkillset;
		}

		// Token: 0x0400196A RID: 6506
		public ushort shirt;

		// Token: 0x0400196B RID: 6507
		public ushort pants;

		// Token: 0x0400196C RID: 6508
		public ushort hat;

		// Token: 0x0400196D RID: 6509
		public ushort backpack;

		// Token: 0x0400196E RID: 6510
		public ushort vest;

		// Token: 0x0400196F RID: 6511
		public ushort mask;

		// Token: 0x04001970 RID: 6512
		public ushort glasses;

		// Token: 0x04001971 RID: 6513
		public ulong packageShirt;

		// Token: 0x04001972 RID: 6514
		public ulong packagePants;

		// Token: 0x04001973 RID: 6515
		public ulong packageHat;

		// Token: 0x04001974 RID: 6516
		public ulong packageBackpack;

		// Token: 0x04001975 RID: 6517
		public ulong packageVest;

		// Token: 0x04001976 RID: 6518
		public ulong packageMask;

		// Token: 0x04001977 RID: 6519
		public ulong packageGlasses;

		// Token: 0x04001978 RID: 6520
		public ushort primaryItem;

		// Token: 0x04001979 RID: 6521
		public byte[] primaryState;

		// Token: 0x0400197A RID: 6522
		public ushort secondaryItem;

		// Token: 0x0400197B RID: 6523
		public byte[] secondaryState;

		// Token: 0x0400197C RID: 6524
		public byte face;

		// Token: 0x0400197D RID: 6525
		public byte hair;

		// Token: 0x0400197E RID: 6526
		public byte beard;

		// Token: 0x0400197F RID: 6527
		public Color skin;

		// Token: 0x04001980 RID: 6528
		public Color color;

		// Token: 0x04001981 RID: 6529
		public Color markerColor;

		// Token: 0x04001982 RID: 6530
		public bool hand;

		// Token: 0x04001983 RID: 6531
		public string name;

		// Token: 0x04001984 RID: 6532
		public string nick;

		// Token: 0x04001985 RID: 6533
		public CSteamID group;

		// Token: 0x04001986 RID: 6534
		public EPlayerSkillset skillset;
	}
}
