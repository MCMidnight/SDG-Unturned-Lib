using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200065C RID: 1628
	public class PlayerSkills : PlayerCaller
	{
		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x060035CE RID: 13774 RVA: 0x000F7828 File Offset: 0x000F5A28
		// (remove) Token: 0x060035CF RID: 13775 RVA: 0x000F785C File Offset: 0x000F5A5C
		public static event ApplyingDefaultSkillsHandler onApplyingDefaultSkills;

		/// <summary>
		/// Invoked after any player's experience value changes (not including loading).
		/// </summary>
		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x060035D0 RID: 13776 RVA: 0x000F7890 File Offset: 0x000F5A90
		// (remove) Token: 0x060035D1 RID: 13777 RVA: 0x000F78C4 File Offset: 0x000F5AC4
		public static event Action<PlayerSkills, uint> OnExperienceChanged_Global;

		/// <summary>
		/// Invoked after any player's reputation value changes (not including loading).
		/// </summary>
		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x060035D2 RID: 13778 RVA: 0x000F78F8 File Offset: 0x000F5AF8
		// (remove) Token: 0x060035D3 RID: 13779 RVA: 0x000F792C File Offset: 0x000F5B2C
		public static event Action<PlayerSkills, int> OnReputationChanged_Global;

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x000F795F File Offset: 0x000F5B5F
		public Skill[][] skills
		{
			get
			{
				return this._skills;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000F7967 File Offset: 0x000F5B67
		public EPlayerBoost boost
		{
			get
			{
				return this._boost;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x000F796F File Offset: 0x000F5B6F
		public uint experience
		{
			get
			{
				return this._experience;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060035D7 RID: 13783 RVA: 0x000F7977 File Offset: 0x000F5B77
		public int reputation
		{
			get
			{
				return this._reputation;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x000F797F File Offset: 0x000F5B7F
		public bool doesLevelAllowSkills
		{
			get
			{
				return Level.info == null || Level.info.configData == null || Level.info.configData.Allow_Skills;
			}
		}

		/// <summary>
		/// Ugly hack for the awful skills enums. Eventually skills should be replaced.
		/// </summary>
		// Token: 0x060035D9 RID: 13785 RVA: 0x000F79A8 File Offset: 0x000F5BA8
		public static bool TryParseIndices(string input, out int specialityIndex, out int skillIndex)
		{
			EPlayerOffense eplayerOffense;
			if (Enum.TryParse<EPlayerOffense>(input, true, ref eplayerOffense))
			{
				specialityIndex = 0;
				skillIndex = (int)eplayerOffense;
				return true;
			}
			EPlayerDefense eplayerDefense;
			if (Enum.TryParse<EPlayerDefense>(input, true, ref eplayerDefense))
			{
				specialityIndex = 1;
				skillIndex = (int)eplayerDefense;
				return true;
			}
			EPlayerSupport eplayerSupport;
			if (Enum.TryParse<EPlayerSupport>(input, true, ref eplayerSupport))
			{
				specialityIndex = 2;
				skillIndex = (int)eplayerSupport;
				return true;
			}
			specialityIndex = -1;
			skillIndex = -1;
			return false;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000F79F5 File Offset: 0x000F5BF5
		[Obsolete]
		public void tellExperience(CSteamID steamID, uint newExperience)
		{
			this.ReceiveExperience(newExperience);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000F7A00 File Offset: 0x000F5C00
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellExperience")]
		public void ReceiveExperience(uint newExperience)
		{
			uint experience = this._experience;
			if (base.channel.IsLocalPlayer && newExperience > this.experience && Level.info.type != ELevelType.HORDE && this.wasLoaded)
			{
				int num;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Experience", out num))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Experience", num + (int)(newExperience - this.experience));
				}
				PlayerUI.message(EPlayerMessage.EXPERIENCE, (newExperience - this.experience).ToString(), 2f);
			}
			this._experience = newExperience;
			ExperienceUpdated experienceUpdated = this.onExperienceUpdated;
			if (experienceUpdated != null)
			{
				experienceUpdated(this.experience);
			}
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000F7ACB File Offset: 0x000F5CCB
		[Obsolete]
		public void tellReputation(CSteamID steamID, int newReputation)
		{
			this.ReceiveReputation(newReputation);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000F7AD4 File Offset: 0x000F5CD4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellReputation")]
		public void ReceiveReputation(int newReputation)
		{
			int reputation = this._reputation;
			if (base.channel.IsLocalPlayer && newReputation != this.reputation && Level.info.type != ELevelType.HORDE && this.wasLoaded)
			{
				bool flag2;
				if (newReputation <= -200)
				{
					bool flag;
					if (Provider.provider.achievementsService.getAchievement("Villain", out flag) && !flag)
					{
						Provider.provider.achievementsService.setAchievement("Villain");
					}
				}
				else if (newReputation >= 200 && Provider.provider.achievementsService.getAchievement("Paragon", out flag2) && !flag2)
				{
					Provider.provider.achievementsService.setAchievement("Paragon");
				}
				if (base.player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowReputationChangeNotification))
				{
					string text = (newReputation - this.reputation).ToString();
					if (newReputation > this.reputation)
					{
						text = "+" + text;
					}
					PlayerUI.message(EPlayerMessage.REPUTATION, text, 2f);
				}
			}
			this._reputation = newReputation;
			ReputationUpdated reputationUpdated = this.onReputationUpdated;
			if (reputationUpdated != null)
			{
				reputationUpdated(this.reputation);
			}
			Action<PlayerSkills, int> onReputationChanged_Global = PlayerSkills.OnReputationChanged_Global;
			if (onReputationChanged_Global == null)
			{
				return;
			}
			onReputationChanged_Global.Invoke(this, reputation);
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000F7C06 File Offset: 0x000F5E06
		[Obsolete]
		public void tellBoost(CSteamID steamID, byte newBoost)
		{
			this.ReceiveBoost((EPlayerBoost)newBoost);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000F7C0F File Offset: 0x000F5E0F
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellBoost")]
		public void ReceiveBoost(EPlayerBoost newBoost)
		{
			this._boost = newBoost;
			BoostUpdated boostUpdated = this.onBoostUpdated;
			if (boostUpdated != null)
			{
				boostUpdated(this.boost);
			}
			this.wasLoaded = true;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000F7C36 File Offset: 0x000F5E36
		[Obsolete]
		public void tellSkill(CSteamID steamID, byte speciality, byte index, byte level)
		{
			this.ReceiveSingleSkillLevel(speciality, index, level);
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000F7C44 File Offset: 0x000F5E44
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSkill")]
		public void ReceiveSingleSkillLevel(byte speciality, byte index, byte level)
		{
			if ((int)index >= this.skills[(int)speciality].Length)
			{
				return;
			}
			this.skills[(int)speciality][(int)index].level = level;
			if (base.channel.IsLocalPlayer)
			{
				bool flag = true;
				bool flag2 = true;
				bool flag3 = true;
				for (int i = 0; i < this.skills[0].Length; i++)
				{
					if (this.skills[0][i].level < this.skills[0][i].max)
					{
						flag = false;
						break;
					}
				}
				for (int j = 0; j < this.skills[1].Length; j++)
				{
					if (this.skills[1][j].level < this.skills[1][j].max)
					{
						flag2 = false;
						break;
					}
				}
				for (int k = 0; k < this.skills[2].Length; k++)
				{
					if (this.skills[2][k].level < this.skills[2][k].max)
					{
						flag3 = false;
						break;
					}
				}
				bool flag4;
				if (flag && Provider.provider.achievementsService.getAchievement("Offense", out flag4) && !flag4)
				{
					Provider.provider.achievementsService.setAchievement("Offense");
				}
				bool flag5;
				if (flag2 && Provider.provider.achievementsService.getAchievement("Defense", out flag5) && !flag5)
				{
					Provider.provider.achievementsService.setAchievement("Defense");
				}
				bool flag6;
				if (flag3 && Provider.provider.achievementsService.getAchievement("Support", out flag6) && !flag6)
				{
					Provider.provider.achievementsService.setAchievement("Support");
				}
				bool flag7;
				if (flag && flag2 && flag3 && Provider.provider.achievementsService.getAchievement("Mastermind", out flag7) && !flag7)
				{
					Provider.provider.achievementsService.setAchievement("Mastermind");
				}
			}
			SkillsUpdated skillsUpdated = this.onSkillsUpdated;
			if (skillsUpdated == null)
			{
				return;
			}
			skillsUpdated();
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000F7E20 File Offset: 0x000F6020
		public float mastery(int speciality, int index)
		{
			return this.skills[speciality][index].mastery;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000F7E34 File Offset: 0x000F6034
		public uint cost(int speciality, int index)
		{
			if (Level.info != null && Level.info.type != ELevelType.ARENA)
			{
				byte b = 0;
				while ((int)b < PlayerSkills.SKILLSETS[(int)((byte)base.channel.owner.skillset)].Length)
				{
					SpecialitySkillPair specialitySkillPair = PlayerSkills.SKILLSETS[(int)((byte)base.channel.owner.skillset)][(int)b];
					if (speciality == specialitySkillPair.speciality && index == specialitySkillPair.skill)
					{
						return this.skills[speciality][index].cost / 2U;
					}
					b += 1;
				}
			}
			return this.skills[speciality][index].cost;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000F7EC8 File Offset: 0x000F60C8
		public void askSpend(uint cost)
		{
			if (base.channel.IsLocalPlayer)
			{
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience - cost);
				return;
			}
			uint experience = this._experience;
			this._experience -= cost;
			PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000F7F50 File Offset: 0x000F6150
		public void askAward(uint award)
		{
			if (base.channel.IsLocalPlayer)
			{
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience + award);
				return;
			}
			uint experience = this._experience;
			this._experience += award;
			PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000F7FD7 File Offset: 0x000F61D7
		public void ServerSetExperience(uint newExperience)
		{
			if (newExperience > this._experience)
			{
				this.askAward(newExperience - this._experience);
				return;
			}
			if (newExperience < this._experience)
			{
				this.askSpend(this._experience - newExperience);
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000F8008 File Offset: 0x000F6208
		public void ServerModifyExperience(int delta)
		{
			if (delta > 0)
			{
				this.askAward((uint)delta);
				return;
			}
			if (delta < 0)
			{
				uint num = (uint)(-(uint)delta);
				num = MathfEx.Min(num, this._experience);
				this.askSpend(num);
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000F803C File Offset: 0x000F623C
		public void askRep(int rep)
		{
			PlayerSkills.SendReputation.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.reputation + rep);
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000F805C File Offset: 0x000F625C
		public void askPay(uint pay)
		{
			if (pay == 0U)
			{
				return;
			}
			pay = (uint)(pay * Provider.modeConfigData.Players.Experience_Multiplier);
			if (base.channel.IsLocalPlayer)
			{
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience + pay);
				return;
			}
			uint experience = this._experience;
			this._experience += pay;
			PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000F8100 File Offset: 0x000F6300
		public void modRep(int rep)
		{
			int reputation = this._reputation;
			this._reputation += rep;
			ReputationUpdated reputationUpdated = this.onReputationUpdated;
			if (reputationUpdated != null)
			{
				reputationUpdated(this.reputation);
			}
			Action<PlayerSkills, int> onReputationChanged_Global = PlayerSkills.OnReputationChanged_Global;
			if (onReputationChanged_Global == null)
			{
				return;
			}
			onReputationChanged_Global.Invoke(this, reputation);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000F814C File Offset: 0x000F634C
		public void modXp(uint xp)
		{
			uint experience = this._experience;
			this._experience += xp;
			ExperienceUpdated experienceUpdated = this.onExperienceUpdated;
			if (experienceUpdated != null)
			{
				experienceUpdated(this.experience);
			}
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000F8198 File Offset: 0x000F6398
		public void modXp2(uint xp)
		{
			uint experience = this._experience;
			this._experience -= xp;
			ExperienceUpdated experienceUpdated = this.onExperienceUpdated;
			if (experienceUpdated != null)
			{
				experienceUpdated(this.experience);
			}
			Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
			if (onExperienceChanged_Global == null)
			{
				return;
			}
			onExperienceChanged_Global.Invoke(this, experience);
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x060035ED RID: 13805 RVA: 0x000F81E4 File Offset: 0x000F63E4
		// (remove) Token: 0x060035EE RID: 13806 RVA: 0x000F8218 File Offset: 0x000F6418
		public static event Action<PlayerSkills, byte, byte, byte> OnSkillUpgraded_Global;

		// Token: 0x060035EF RID: 13807 RVA: 0x000F824B File Offset: 0x000F644B
		[Obsolete]
		public void askUpgrade(CSteamID steamID, byte speciality, byte index, bool force)
		{
			this.ReceiveUpgradeRequest(speciality, index, force);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000F8258 File Offset: 0x000F6458
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askUpgrade")]
		public void ReceiveUpgradeRequest(byte speciality, byte index, bool force)
		{
			if (!this.doesLevelAllowSkills)
			{
				return;
			}
			if (speciality >= PlayerSkills.SPECIALITIES)
			{
				return;
			}
			if ((int)index >= this.skills[(int)speciality].Length)
			{
				return;
			}
			Skill skill = this.skills[(int)speciality][(int)index];
			byte level = skill.level;
			uint experience = this._experience;
			while (this.experience >= this.cost((int)speciality, (int)index) && (int)skill.level < skill.GetClampedMaxUnlockableLevel())
			{
				this._experience -= this.cost((int)speciality, (int)index);
				Skill skill2 = skill;
				skill2.level += 1;
				if (!force)
				{
					break;
				}
			}
			if (skill.level > level)
			{
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
				PlayerSkills.SendSingleSkillLevel.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), speciality, index, skill.level);
				if (this._experience != experience && PlayerSkills.OnExperienceChanged_Global != null)
				{
					PlayerSkills.OnExperienceChanged_Global.Invoke(this, experience);
				}
				PlayerSkills.OnSkillUpgraded_Global.TryInvoke("OnSkillUpgraded_Global", this, speciality, index, level);
			}
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000F835C File Offset: 0x000F655C
		public bool ServerSetSkillLevel(int specialityIndex, int skillIndex, int newLevel)
		{
			if (specialityIndex >= this.skills.Length)
			{
				throw new ArgumentOutOfRangeException("specialityIndex");
			}
			if (skillIndex >= this.skills[specialityIndex].Length)
			{
				throw new ArgumentOutOfRangeException("skillIndex");
			}
			Skill skill = this.skills[specialityIndex][skillIndex];
			if (newLevel > (int)skill.max)
			{
				throw new ArgumentOutOfRangeException("newLevel");
			}
			if ((int)skill.level != newLevel)
			{
				skill.level = (byte)newLevel;
				PlayerSkills.SendSingleSkillLevel.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)specialityIndex, (byte)skillIndex, skill.level);
				return true;
			}
			return false;
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000F83EA File Offset: 0x000F65EA
		[Obsolete]
		public void askBoost(CSteamID steamID)
		{
			this.ReceiveBoostRequest();
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000F83F4 File Offset: 0x000F65F4
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askBoost")]
		public void ReceiveBoostRequest()
		{
			if (!this.doesLevelAllowSkills)
			{
				return;
			}
			if (this.experience >= PlayerSkills.BOOST_COST)
			{
				uint experience = this._experience;
				this._experience -= PlayerSkills.BOOST_COST;
				byte b;
				do
				{
					b = (byte)Random.Range(1, (int)(PlayerSkills.BOOST_COUNT + 1));
				}
				while (b == (byte)this.boost);
				this._boost = (EPlayerBoost)b;
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
				PlayerSkills.SendBoost.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.boost);
				Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
				if (onExperienceChanged_Global == null)
				{
					return;
				}
				onExperienceChanged_Global.Invoke(this, experience);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000F84A2 File Offset: 0x000F66A2
		[Obsolete]
		public void askPurchase(CSteamID steamID, byte index)
		{
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000F84A4 File Offset: 0x000F66A4
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askPurchase")]
		public void ReceivePurchaseRequest(NetId volumeNetId)
		{
			HordePurchaseVolume hordePurchaseVolume = NetIdRegistry.Get<HordePurchaseVolume>(volumeNetId);
			if (hordePurchaseVolume == null)
			{
				return;
			}
			if (this.experience >= hordePurchaseVolume.cost)
			{
				uint experience = this._experience;
				this._experience -= hordePurchaseVolume.cost;
				PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.experience);
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, hordePurchaseVolume.id) as ItemAsset;
				if (itemAsset.type == EItemType.GUN && base.player.inventory.has(hordePurchaseVolume.id) != null)
				{
					base.player.inventory.tryAddItem(new Item(((ItemGunAsset)itemAsset).getMagazineID(), EItemOrigin.ADMIN), true);
				}
				else
				{
					base.player.inventory.tryAddItem(new Item(hordePurchaseVolume.id, EItemOrigin.ADMIN), true);
				}
				Action<PlayerSkills, uint> onExperienceChanged_Global = PlayerSkills.OnExperienceChanged_Global;
				if (onExperienceChanged_Global == null)
				{
					return;
				}
				onExperienceChanged_Global.Invoke(this, experience);
			}
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x000F8596 File Offset: 0x000F6796
		public void sendUpgrade(byte speciality, byte index, bool force)
		{
			PlayerSkills.SendUpgradeRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, speciality, index, force);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000F85AC File Offset: 0x000F67AC
		public void sendBoost()
		{
			PlayerSkills.SendBoostRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000F85BF File Offset: 0x000F67BF
		public void sendPurchase(HordePurchaseVolume node)
		{
			PlayerSkills.SendPurchaseRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, node.GetNetIdFromInstanceId());
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000F85D8 File Offset: 0x000F67D8
		[Obsolete]
		public void tellSkills(CSteamID steamID, byte speciality, byte[] newLevels)
		{
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000F85DC File Offset: 0x000F67DC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveMultipleSkillLevels(in ClientInvocationContext context)
		{
			if (this.skills == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			for (int i = 0; i < this.skills.Length; i++)
			{
				Skill[] array = this.skills[i];
				for (int j = 0; j < array.Length; j++)
				{
					SystemNetPakReaderEx.ReadUInt8(reader, ref array[j].level);
				}
			}
			SkillsUpdated skillsUpdated = this.onSkillsUpdated;
			if (skillsUpdated == null)
			{
				return;
			}
			skillsUpdated();
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000F8644 File Offset: 0x000F6844
		private void WriteSkillLevels(NetPakWriter writer)
		{
			for (int i = 0; i < this.skills.Length; i++)
			{
				Skill[] array = this.skills[i];
				for (int j = 0; j < array.Length; j++)
				{
					SystemNetPakWriterEx.WriteUInt8(writer, array[j].level);
				}
			}
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000F868A File Offset: 0x000F688A
		[Obsolete]
		public void askSkills(CSteamID steamID)
		{
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000F868C File Offset: 0x000F688C
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			PlayerSkills.SendMultipleSkillLevels.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, new Action<NetPakWriter>(this.WriteSkillLevels));
			PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.experience);
			PlayerSkills.SendReputation.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.reputation);
			PlayerSkills.SendBoost.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.boost);
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000F8714 File Offset: 0x000F6914
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			PlayerSkills.SendMultipleSkillLevels.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, new Action<NetPakWriter>(this.WriteSkillLevels));
			PlayerSkills.SendExperience.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.experience);
			PlayerSkills.SendReputation.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.reputation);
			PlayerSkills.SendBoost.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.boost);
		}

		/// <summary>
		/// Set every level to max and replicate.
		/// </summary>
		// Token: 0x060035FF RID: 13823 RVA: 0x000F8788 File Offset: 0x000F6988
		public void ServerUnlockAllSkills()
		{
			foreach (Skill[] array in this.skills)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j].setLevelToMax();
				}
			}
			PlayerSkills.SendMultipleSkillLevels.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), new Action<NetPakWriter>(this.WriteSkillLevels));
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000F87E8 File Offset: 0x000F69E8
		private void onLifeUpdated(bool isDead)
		{
			if (isDead && Provider.isServer)
			{
				if (Level.info == null || Level.info.type == ELevelType.SURVIVAL)
				{
					bool flag = false;
					float num = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Skills_PvP : Provider.modeConfigData.Players.Lose_Skills_PvE;
					if (num < 0.999f)
					{
						for (int i = 0; i < this.skills.Length; i++)
						{
							Skill[] array = this.skills[i];
							for (int j = 0; j < array.Length; j++)
							{
								if (this.CanDecreaseLevelOfSkill(i, j))
								{
									byte b = (byte)((float)array[j].level * num);
									flag |= (array[j].level != b);
									array[j].level = b;
								}
							}
						}
					}
					uint num2 = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Skill_Levels_PvP : Provider.modeConfigData.Players.Lose_Skill_Levels_PvE;
					if (num2 > 0U)
					{
						this.LoseNumberOfSkills(num2, ref flag);
					}
					if (flag)
					{
						PlayerSkills.SendMultipleSkillLevels.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), new Action<NetPakWriter>(this.WriteSkillLevels));
					}
					float num3 = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Experience_PvP : Provider.modeConfigData.Players.Lose_Experience_PvE;
					this._experience = (uint)(this.experience * num3);
				}
				else
				{
					byte b2 = 0;
					while ((int)b2 < this.skills.Length)
					{
						byte b3 = 0;
						while ((int)b3 < this.skills[(int)b2].Length)
						{
							this.skills[(int)b2][(int)b3].level = 0;
							b3 += 1;
						}
						b2 += 1;
					}
					this.applyDefaultSkills();
					PlayerSkills.SendMultipleSkillLevels.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), new Action<NetPakWriter>(this.WriteSkillLevels));
					if (Level.info.type == ELevelType.ARENA)
					{
						this._experience = 0U;
					}
					else
					{
						this._experience = (uint)(this.experience * 0.75f);
					}
				}
				this._boost = EPlayerBoost.NONE;
				PlayerSkills.SendExperience.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.experience);
				PlayerSkills.SendBoost.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.boost);
			}
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000F8A50 File Offset: 0x000F6C50
		internal void InitializePlayer()
		{
			this._skills = new Skill[(int)PlayerSkills.SPECIALITIES][];
			this.skills[0] = new Skill[7];
			this.skills[0][0] = new Skill(0, 7, 10U, 1f);
			this.skills[0][1] = new Skill(0, 7, 10U, 1f);
			this.skills[0][2] = new Skill(0, 5, 10U, 0.5f);
			this.skills[0][3] = new Skill(0, 5, 10U, 0.5f);
			this.skills[0][4] = new Skill(0, 5, 10U, 0.5f);
			this.skills[0][5] = new Skill(0, 5, 10U, 0.5f);
			this.skills[0][6] = new Skill(0, 5, 20U, 0.5f);
			this.skills[1] = new Skill[7];
			this.skills[1][0] = new Skill(0, 7, 10U, 1f);
			this.skills[1][1] = new Skill(0, 5, 10U, 0.5f);
			this.skills[1][2] = new Skill(0, 5, 10U, 0.5f);
			this.skills[1][3] = new Skill(0, 5, 10U, 0.5f);
			this.skills[1][4] = new Skill(0, 5, 10U, 0.5f);
			this.skills[1][5] = new Skill(0, 5, 10U, 0.5f);
			this.skills[1][6] = new Skill(0, 5, 10U, 0.5f);
			this.skills[2] = new Skill[8];
			this.skills[2][0] = new Skill(0, 7, 10U, 1f);
			this.skills[2][1] = new Skill(0, 3, 20U, 1.5f);
			this.skills[2][2] = new Skill(0, 5, 10U, 0.5f);
			this.skills[2][3] = new Skill(0, 3, 20U, 1.5f);
			this.skills[2][4] = new Skill(0, 5, 10U, 0.5f);
			this.skills[2][5] = new Skill(0, 7, 10U, 1f);
			this.skills[2][6] = new Skill(0, 5, 10U, 0.5f);
			this.skills[2][7] = new Skill(0, 3, 20U, 1.5f);
			LevelAsset asset = Level.getAsset();
			if (asset != null && asset.skillRules != null)
			{
				for (int i = 0; i < this.skills.Length; i++)
				{
					for (int j = 0; j < this.skills[i].Length; j++)
					{
						LevelAsset.SkillRule skillRule = asset.skillRules[i][j];
						if (skillRule != null)
						{
							if (skillRule.maxUnlockableLevel > -1)
							{
								this.skills[i][j].maxUnlockableLevel = skillRule.maxUnlockableLevel;
							}
							this.skills[i][j].costMultiplier = skillRule.costMultiplier;
						}
					}
				}
			}
			if (Provider.isServer)
			{
				this.load();
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
				return;
			}
			this._experience = uint.MaxValue;
			this._reputation = 0;
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000F8D6C File Offset: 0x000F6F6C
		public void load()
		{
			this.wasLoadCalled = true;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Skills.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Skills.dat", 0);
				byte b = block.readByte();
				if (b > 4)
				{
					this._experience = block.readUInt32();
					if (b >= 7)
					{
						this._reputation = block.readInt32();
					}
					else
					{
						this._reputation = 0;
					}
					this._boost = (EPlayerBoost)block.readByte();
					if (b >= 6)
					{
						byte b2 = 0;
						while ((int)b2 < this.skills.Length)
						{
							if (this.skills[(int)b2] != null)
							{
								byte b3 = 0;
								while ((int)b3 < this.skills[(int)b2].Length)
								{
									this.skills[(int)b2][(int)b3].level = block.readByte();
									if (this.skills[(int)b2][(int)b3].level > this.skills[(int)b2][(int)b3].max)
									{
										this.skills[(int)b2][(int)b3].level = this.skills[(int)b2][(int)b3].max;
									}
									b3 += 1;
								}
							}
							b2 += 1;
						}
						return;
					}
				}
			}
			else
			{
				this.applyDefaultSkills();
			}
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000F8EA4 File Offset: 0x000F70A4
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			Block block = new Block();
			block.writeByte(PlayerSkills.SAVEDATA_VERSION);
			block.writeUInt32(this.experience);
			block.writeInt32(this.reputation);
			block.writeByte((byte)this.boost);
			byte b = 0;
			while ((int)b < this.skills.Length)
			{
				if (this.skills[(int)b] != null)
				{
					byte b2 = 0;
					while ((int)b2 < this.skills[(int)b].Length)
					{
						block.writeByte(this.skills[(int)b][(int)b2].level);
						b2 += 1;
					}
				}
				b += 1;
			}
			PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Skills.dat", block);
		}

		/// <summary>
		/// Serverside only.
		/// Called when skills weren't loaded (no save, or in arena mode), as well as when reseting skills after death.
		/// </summary>
		// Token: 0x06003604 RID: 13828 RVA: 0x000F8F54 File Offset: 0x000F7154
		private void applyDefaultSkills()
		{
			if (Provider.modeConfigData.Players.Spawn_With_Max_Skills)
			{
				byte b = 0;
				while ((int)b < this.skills.Length)
				{
					Skill[] array = this.skills[(int)b];
					byte b2 = 0;
					while ((int)b2 < array.Length)
					{
						array[(int)b2].setLevelToMax();
						b2 += 1;
					}
					b += 1;
				}
			}
			else
			{
				LevelAsset asset = Level.getAsset();
				if (asset != null && asset.skillRules != null)
				{
					for (int i = 0; i < this.skills.Length; i++)
					{
						for (int j = 0; j < this.skills[i].Length; j++)
						{
							LevelAsset.SkillRule skillRule = asset.skillRules[i][j];
							if (skillRule != null)
							{
								this.skills[i][j].level = (byte)skillRule.defaultLevel;
							}
						}
					}
				}
				if (Provider.modeConfigData.Players.Spawn_With_Stamina_Skills)
				{
					this.skills[0][3].setLevelToMax();
					this.skills[0][5].setLevelToMax();
					this.skills[0][4].setLevelToMax();
					this.skills[0][6].setLevelToMax();
				}
			}
			ApplyingDefaultSkillsHandler applyingDefaultSkillsHandler = PlayerSkills.onApplyingDefaultSkills;
			if (applyingDefaultSkillsHandler == null)
			{
				return;
			}
			applyingDefaultSkillsHandler(base.player, this.skills);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000F9080 File Offset: 0x000F7280
		private bool CanDecreaseLevelOfSkill(int specialityIndex, int skillIndex)
		{
			int skillset = (int)base.channel.owner.skillset;
			for (int i = 0; i < PlayerSkills.SKILLSETS[skillset].Length; i++)
			{
				SpecialitySkillPair specialitySkillPair = PlayerSkills.SKILLSETS[skillset][i];
				if (specialityIndex == specialitySkillPair.speciality && skillIndex == specialitySkillPair.skill)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000F90D4 File Offset: 0x000F72D4
		private void LoseNumberOfSkills(uint numberOfSkillsToLose, ref bool modifiedAnySkillLevels)
		{
			PlayerSkills.availableSkillsToLoseLevels.Clear();
			for (int i = 0; i < this.skills.Length; i++)
			{
				Skill[] array = this.skills[i];
				for (int j = 0; j < array.Length; j++)
				{
					if (this.CanDecreaseLevelOfSkill(i, j) && array[j].level > 0)
					{
						PlayerSkills.availableSkillsToLoseLevels.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			while (numberOfSkillsToLose > 0U && PlayerSkills.availableSkillsToLoseLevels.Count > 0)
			{
				int randomIndex = PlayerSkills.availableSkillsToLoseLevels.GetRandomIndex<Tuple<int, int>>();
				Tuple<int, int> tuple = PlayerSkills.availableSkillsToLoseLevels[randomIndex];
				PlayerSkills.availableSkillsToLoseLevels.RemoveAtFast(randomIndex);
				Skill skill = this.skills[tuple.Item1][tuple.Item2];
				skill.level -= 1;
				modifiedAnySkillLevels = true;
				numberOfSkillsToLose -= 1U;
			}
		}

		// Token: 0x04001EF8 RID: 7928
		public static readonly SpecialitySkillPair[][] SKILLSETS = new SpecialitySkillPair[][]
		{
			new SpecialitySkillPair[0],
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(0, 3),
				new SpecialitySkillPair(1, 4)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(0, 4),
				new SpecialitySkillPair(1, 3)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(0, 1),
				new SpecialitySkillPair(0, 2)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(2, 5),
				new SpecialitySkillPair(1, 6)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(2, 4),
				new SpecialitySkillPair(0, 5)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(1, 5),
				new SpecialitySkillPair(2, 2)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(2, 1),
				new SpecialitySkillPair(2, 7),
				new SpecialitySkillPair(2, 6)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(2, 3),
				new SpecialitySkillPair(1, 1)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(0, 6),
				new SpecialitySkillPair(1, 0)
			},
			new SpecialitySkillPair[]
			{
				new SpecialitySkillPair(1, 2),
				new SpecialitySkillPair(2, 0)
			}
		};

		// Token: 0x04001EF9 RID: 7929
		public static readonly byte SAVEDATA_VERSION = 7;

		// Token: 0x04001EFA RID: 7930
		public static readonly byte SPECIALITIES = 3;

		// Token: 0x04001EFB RID: 7931
		public static readonly byte BOOST_COUNT = 4;

		// Token: 0x04001EFC RID: 7932
		public static readonly uint BOOST_COST = 25U;

		// Token: 0x04001F00 RID: 7936
		public ExperienceUpdated onExperienceUpdated;

		// Token: 0x04001F01 RID: 7937
		public ReputationUpdated onReputationUpdated;

		// Token: 0x04001F02 RID: 7938
		public BoostUpdated onBoostUpdated;

		// Token: 0x04001F03 RID: 7939
		public SkillsUpdated onSkillsUpdated;

		// Token: 0x04001F04 RID: 7940
		private Skill[][] _skills;

		// Token: 0x04001F05 RID: 7941
		private EPlayerBoost _boost;

		// Token: 0x04001F06 RID: 7942
		private uint _experience;

		// Token: 0x04001F07 RID: 7943
		private int _reputation;

		// Token: 0x04001F08 RID: 7944
		private bool wasLoaded;

		// Token: 0x04001F09 RID: 7945
		private static readonly ClientInstanceMethod<uint> SendExperience = ClientInstanceMethod<uint>.Get(typeof(PlayerSkills), "ReceiveExperience");

		// Token: 0x04001F0A RID: 7946
		private static readonly ClientInstanceMethod<int> SendReputation = ClientInstanceMethod<int>.Get(typeof(PlayerSkills), "ReceiveReputation");

		// Token: 0x04001F0B RID: 7947
		private static readonly ClientInstanceMethod<EPlayerBoost> SendBoost = ClientInstanceMethod<EPlayerBoost>.Get(typeof(PlayerSkills), "ReceiveBoost");

		// Token: 0x04001F0C RID: 7948
		private static readonly ClientInstanceMethod<byte, byte, byte> SendSingleSkillLevel = ClientInstanceMethod<byte, byte, byte>.Get(typeof(PlayerSkills), "ReceiveSingleSkillLevel");

		// Token: 0x04001F0E RID: 7950
		private static readonly ServerInstanceMethod<byte, byte, bool> SendUpgradeRequest = ServerInstanceMethod<byte, byte, bool>.Get(typeof(PlayerSkills), "ReceiveUpgradeRequest");

		// Token: 0x04001F0F RID: 7951
		private static readonly ServerInstanceMethod SendBoostRequest = ServerInstanceMethod.Get(typeof(PlayerSkills), "ReceiveBoostRequest");

		// Token: 0x04001F10 RID: 7952
		private static readonly ServerInstanceMethod<NetId> SendPurchaseRequest = ServerInstanceMethod<NetId>.Get(typeof(PlayerSkills), "ReceivePurchaseRequest");

		// Token: 0x04001F11 RID: 7953
		private static readonly ClientInstanceMethod SendMultipleSkillLevels = ClientInstanceMethod.Get(typeof(PlayerSkills), "ReceiveMultipleSkillLevels");

		// Token: 0x04001F12 RID: 7954
		private bool wasLoadCalled;

		// Token: 0x04001F13 RID: 7955
		private static List<Tuple<int, int>> availableSkillsToLoseLevels = new List<Tuple<int, int>>();
	}
}
