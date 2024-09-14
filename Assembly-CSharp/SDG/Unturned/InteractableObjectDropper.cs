using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200045C RID: 1116
	public class InteractableObjectDropper : InteractableObjectTriggerableBase
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060021EB RID: 8683 RVA: 0x0008395B File Offset: 0x00081B5B
		public bool isUsable
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityDelay && (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired);
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0008398D File Offset: 0x00081B8D
		private void initAudioSourceComponent()
		{
			this.audioSourceComponent = base.transform.GetComponent<AudioSource>();
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000839A0 File Offset: 0x00081BA0
		private void updateAudioSourceComponent()
		{
			this.audioSourceComponent != null;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000839AF File Offset: 0x00081BAF
		private void initDropTransform()
		{
			this.dropTransform = base.transform.Find("Drop");
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000839C7 File Offset: 0x00081BC7
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this.interactabilityDrops = ((ObjectAsset)asset).interactabilityDrops;
			this.interactabilityRewardID = ((ObjectAsset)asset).interactabilityRewardID;
			this.initAudioSourceComponent();
			this.initDropTransform();
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00083A00 File Offset: 0x00081C00
		public void drop()
		{
			this.lastUsed = Time.realtimeSinceStartup;
			if (this.dropTransform == null)
			{
				return;
			}
			if (base.objectAsset.holidayRestriction == ENPCHoliday.NONE || Provider.modeConfigData.Objects.Allow_Holiday_Drops)
			{
				if (this.interactabilityRewardID != 0)
				{
					ushort num = SpawnTableTool.ResolveLegacyId(this.interactabilityRewardID, EAssetType.ITEM, new Func<string>(this.OnGetDropSpawnTableErrorContext));
					if (num != 0)
					{
						ItemManager.dropItem(new Item(num, EItemOrigin.NATURE), this.dropTransform.position, false, true, false);
						return;
					}
				}
				else
				{
					ushort num2 = this.interactabilityDrops[Random.Range(0, this.interactabilityDrops.Length)];
					if (num2 != 0)
					{
						ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), this.dropTransform.position, false, true, false);
					}
				}
			}
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00083AB7 File Offset: 0x00081CB7
		public override void use()
		{
			this.updateAudioSourceComponent();
			ObjectManager.useObjectDropper(base.transform);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00083ACA File Offset: 0x00081CCA
		public override bool checkUseable()
		{
			return (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired) && base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00083AF4 File Offset: 0x00081CF4
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			for (int i = 0; i < base.objectAsset.interactabilityConditions.Length; i++)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					message = EPlayerMessage.CONDITION;
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					return true;
				}
			}
			if (base.objectAsset.interactabilityPower != EObjectInteractabilityPower.NONE && !base.isWired)
			{
				message = EPlayerMessage.POWER;
			}
			else
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_OPEN;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_ON;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_ON;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_ON;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				case EObjectInteractabilityHint.CUSTOM:
					message = EPlayerMessage.INTERACT;
					text = base.objectAsset.interactabilityText;
					color = Color.white;
					return true;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00083BEB File Offset: 0x00081DEB
		private string OnGetDropSpawnTableErrorContext()
		{
			ObjectAsset objectAsset = base.objectAsset;
			return ((objectAsset != null) ? objectAsset.FriendlyName : null) + " drop";
		}

		// Token: 0x040010AF RID: 4271
		private float lastUsed = -9999f;

		// Token: 0x040010B0 RID: 4272
		private ushort[] interactabilityDrops;

		// Token: 0x040010B1 RID: 4273
		private ushort interactabilityRewardID;

		// Token: 0x040010B2 RID: 4274
		private AudioSource audioSourceComponent;

		// Token: 0x040010B3 RID: 4275
		private Transform dropTransform;
	}
}
