using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000463 RID: 1123
	public class InteractableObjectRubble : MonoBehaviour
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x000842FC File Offset: 0x000824FC
		// (set) Token: 0x0600221F RID: 8735 RVA: 0x00084304 File Offset: 0x00082504
		public ObjectAsset asset { get; protected set; }

		// Token: 0x06002220 RID: 8736 RVA: 0x0008430D File Offset: 0x0008250D
		public byte getSectionCount()
		{
			return (byte)this.rubbleInfos.Length;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00084318 File Offset: 0x00082518
		public Transform getSection(byte section)
		{
			return this.rubbleInfos[(int)section].section;
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00084327 File Offset: 0x00082527
		public RubbleInfo getSectionInfo(byte section)
		{
			return this.rubbleInfos[(int)section];
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00084334 File Offset: 0x00082534
		public bool isAllAlive()
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				if (this.rubbleInfos[(int)b].isDead)
				{
					return false;
				}
				b += 1;
			}
			return true;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00084368 File Offset: 0x00082568
		public bool isAllDead()
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				if (!this.rubbleInfos[(int)b].isDead)
				{
					return false;
				}
				b += 1;
			}
			return true;
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x0008439B File Offset: 0x0008259B
		public bool IsSectionIndexValid(byte sectionIndex)
		{
			return this.rubbleInfos != null && (int)sectionIndex < this.rubbleInfos.Length;
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000843B2 File Offset: 0x000825B2
		public bool isSectionDead(byte section)
		{
			return this.rubbleInfos[(int)section].isDead;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000843C1 File Offset: 0x000825C1
		public void askDamage(byte section, ushort amount)
		{
			if (section == 255)
			{
				section = 0;
				while ((int)section < this.rubbleInfos.Length)
				{
					this.rubbleInfos[(int)section].askDamage(amount);
					section += 1;
				}
				return;
			}
			this.rubbleInfos[(int)section].askDamage(amount);
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x00084400 File Offset: 0x00082600
		public byte checkCanReset(float multiplier)
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				if (this.rubbleInfos[(int)b].isDead && this.asset.rubbleReset > 1f && Time.realtimeSinceStartup - this.rubbleInfos[(int)b].lastDead > this.asset.rubbleReset * multiplier)
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0008446C File Offset: 0x0008266C
		public byte getSection(Transform hitTransform)
		{
			if (hitTransform != null)
			{
				byte b = 0;
				while ((int)b < this.rubbleInfos.Length)
				{
					RubbleInfo rubbleInfo = this.rubbleInfos[(int)b];
					if (hitTransform.IsChildOf(rubbleInfo.section))
					{
						return b;
					}
					b += 1;
				}
			}
			return byte.MaxValue;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000844B4 File Offset: 0x000826B4
		public void updateRubble(byte section, bool isAlive, bool playEffect, Vector3 ragdoll)
		{
			if (this.rubbleInfos == null || (int)section >= this.rubbleInfos.Length)
			{
				return;
			}
			RubbleInfo rubbleInfo = this.rubbleInfos[(int)section];
			if (isAlive)
			{
				rubbleInfo.health = this.asset.rubbleHealth;
			}
			else
			{
				rubbleInfo.lastDead = Time.realtimeSinceStartup;
				rubbleInfo.health = 0;
			}
			bool flag = this.isAllDead();
			if (rubbleInfo.aliveGameObject != null)
			{
				rubbleInfo.aliveGameObject.SetActive(!rubbleInfo.isDead);
			}
			if (rubbleInfo.deadGameObject != null)
			{
				rubbleInfo.deadGameObject.SetActive(rubbleInfo.isDead && (!flag || this.asset.IsRubbleFinaleEffectRefNull()));
			}
			if (this.aliveGameObject != null)
			{
				this.aliveGameObject.SetActive(!flag);
			}
			if (this.deadGameObject != null)
			{
				this.deadGameObject.SetActive(flag);
			}
			if (Provider.isServer && this.dropTransform != null && this.asset.rubbleRewardID > 0 && playEffect && flag && (this.asset.holidayRestriction == ENPCHoliday.NONE || Provider.modeConfigData.Objects.Allow_Holiday_Drops) && Random.value <= this.asset.rubbleRewardProbability)
			{
				int num = Random.Range((int)this.asset.rubbleRewardsMin, (int)(this.asset.rubbleRewardsMax + 1));
				num = Mathf.Clamp(num, 0, 100);
				for (int i = 0; i < num; i++)
				{
					ushort num2 = SpawnTableTool.ResolveLegacyId(this.asset.rubbleRewardID, EAssetType.ITEM, new Func<string>(this.OnGetSpawnTableErrorContext));
					if (num2 != 0)
					{
						ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), this.dropTransform.position, false, true, false);
					}
				}
			}
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00084678 File Offset: 0x00082878
		public void updateState(Asset asset, byte[] state)
		{
			this.asset = (asset as ObjectAsset);
			Transform transform = base.transform.Find("Sections");
			if (transform != null)
			{
				int num = Mathf.Min(transform.childCount, 8);
				this.rubbleInfos = new RubbleInfo[num];
				for (int i = 0; i < this.rubbleInfos.Length; i++)
				{
					Transform section = transform.Find("Section_" + i.ToString());
					RubbleInfo rubbleInfo = new RubbleInfo();
					rubbleInfo.section = section;
					this.rubbleInfos[i] = rubbleInfo;
				}
				Transform transform2 = base.transform.Find("Alive");
				if (transform2 != null)
				{
					this.aliveGameObject = transform2.gameObject;
				}
				Transform transform3 = base.transform.Find("Dead");
				if (transform3 != null)
				{
					this.deadGameObject = transform3.gameObject;
				}
				this.finaleTransform = base.transform.Find("Finale");
			}
			else
			{
				this.rubbleInfos = new RubbleInfo[1];
				RubbleInfo rubbleInfo2 = new RubbleInfo();
				rubbleInfo2.section = base.transform;
				this.rubbleInfos[0] = rubbleInfo2;
			}
			this.dropTransform = base.transform.Find("Drop");
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				RubbleInfo rubbleInfo3 = this.rubbleInfos[(int)b];
				Transform section2 = rubbleInfo3.section;
				Transform transform4 = section2.Find("Alive");
				if (transform4 != null)
				{
					rubbleInfo3.aliveGameObject = transform4.gameObject;
				}
				Transform transform5 = section2.Find("Dead");
				if (transform5 != null)
				{
					rubbleInfo3.deadGameObject = transform5.gameObject;
				}
				Transform transform6 = section2.Find("Ragdolls");
				if (transform6 != null)
				{
					rubbleInfo3.ragdolls = new RubbleRagdollInfo[transform6.childCount];
					for (int j = 0; j < rubbleInfo3.ragdolls.Length; j++)
					{
						Transform transform7 = transform6.Find("Ragdoll_" + j.ToString());
						Transform transform8 = transform7.Find("Ragdoll");
						if (transform8 != null)
						{
							rubbleInfo3.ragdolls[j] = new RubbleRagdollInfo();
							rubbleInfo3.ragdolls[j].ragdollGameObject = transform8.gameObject;
							rubbleInfo3.ragdolls[j].forceTransform = transform7.Find("Force");
						}
					}
				}
				else
				{
					Transform transform9 = section2.Find("Ragdoll");
					if (transform9 != null)
					{
						rubbleInfo3.ragdolls = new RubbleRagdollInfo[1];
						rubbleInfo3.ragdolls[0] = new RubbleRagdollInfo();
						rubbleInfo3.ragdolls[0].ragdollGameObject = transform9.gameObject;
						rubbleInfo3.ragdolls[0].forceTransform = section2.Find("Force");
					}
				}
				rubbleInfo3.effectTransform = section2.Find("Effect");
				b += 1;
			}
			byte b2 = 0;
			while ((int)b2 < this.rubbleInfos.Length)
			{
				bool isAlive = (state[state.Length - 1] & Types.SHIFTS[(int)b2]) == Types.SHIFTS[(int)b2];
				this.updateRubble(b2, isAlive, false, Vector3.zero);
				b2 += 1;
			}
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000849AA File Offset: 0x00082BAA
		private string OnGetSpawnTableErrorContext()
		{
			ObjectAsset asset = this.asset;
			return ((asset != null) ? asset.FriendlyName : null) + " rubble reward";
		}

		// Token: 0x040010D4 RID: 4308
		internal RubbleInfo[] rubbleInfos;

		// Token: 0x040010D5 RID: 4309
		private GameObject aliveGameObject;

		// Token: 0x040010D6 RID: 4310
		private GameObject deadGameObject;

		// Token: 0x040010D7 RID: 4311
		private Transform finaleTransform;

		// Token: 0x040010D8 RID: 4312
		private Transform dropTransform;
	}
}
