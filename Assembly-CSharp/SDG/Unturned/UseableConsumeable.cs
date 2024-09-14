using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007DE RID: 2014
	public class UseableConsumeable : Useable
	{
		/// <summary>
		/// Broadcasts for plugins before applying consumeable stats to another player.
		/// </summary>
		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06004477 RID: 17527 RVA: 0x0018C8A0 File Offset: 0x0018AAA0
		// (remove) Token: 0x06004478 RID: 17528 RVA: 0x0018C8D4 File Offset: 0x0018AAD4
		public static event UseableConsumeable.PerformingAidHandler onPerformingAid;

		/// <summary>
		/// Broadcasts for plugins after applying consumeable stats to another player.
		/// </summary>
		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06004479 RID: 17529 RVA: 0x0018C908 File Offset: 0x0018AB08
		// (remove) Token: 0x0600447A RID: 17530 RVA: 0x0018C93C File Offset: 0x0018AB3C
		public static event UseableConsumeable.PerformedAidHandler onPerformedAid;

		/// <summary>
		/// Broadcasts for plugins before applying consumeable stats to self.
		/// </summary>
		// Token: 0x140000EC RID: 236
		// (add) Token: 0x0600447B RID: 17531 RVA: 0x0018C970 File Offset: 0x0018AB70
		// (remove) Token: 0x0600447C RID: 17532 RVA: 0x0018C9A4 File Offset: 0x0018ABA4
		public static event UseableConsumeable.ConsumeRequestedHandler onConsumeRequested;

		/// <summary>
		/// Broadcasts for plugins after applying consumeable stats to self.
		/// </summary>
		// Token: 0x140000ED RID: 237
		// (add) Token: 0x0600447D RID: 17533 RVA: 0x0018C9D8 File Offset: 0x0018ABD8
		// (remove) Token: 0x0600447E RID: 17534 RVA: 0x0018CA0C File Offset: 0x0018AC0C
		public static event UseableConsumeable.ConsumePerformedHandler onConsumePerformed;

		// Token: 0x0600447F RID: 17535 RVA: 0x0018CA40 File Offset: 0x0018AC40
		private bool invokeConsumeRequested(ItemConsumeableAsset asset)
		{
			if (UseableConsumeable.onConsumeRequested != null)
			{
				bool result = true;
				UseableConsumeable.onConsumeRequested(base.player, asset, ref result);
				return result;
			}
			return true;
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0018CA6C File Offset: 0x0018AC6C
		private void invokeConsumePerformed(ItemConsumeableAsset asset)
		{
			UseableConsumeable.ConsumePerformedHandler consumePerformedHandler = UseableConsumeable.onConsumePerformed;
			if (consumePerformedHandler == null)
			{
				return;
			}
			consumePerformedHandler(base.player, asset);
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004481 RID: 17537 RVA: 0x0018CA84 File Offset: 0x0018AC84
		private bool isUseable
		{
			get
			{
				if (this.consumeMode == EConsumeMode.USE)
				{
					return Time.realtimeSinceStartup - this.startedUse > this.useTime;
				}
				return this.consumeMode == EConsumeMode.AID && Time.realtimeSinceStartup - this.startedUse > this.aidTime;
			}
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0018CAC4 File Offset: 0x0018ACC4
		private void consume()
		{
			if (this.consumeMode == EConsumeMode.USE)
			{
				base.player.animator.play("Use", false);
			}
			else if (this.consumeMode == EConsumeMode.AID && this.hasAid)
			{
				base.player.animator.play("Aid", false);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0018CB34 File Offset: 0x0018AD34
		[Obsolete]
		public void askConsume(CSteamID steamID, byte mode)
		{
			this.ReceivePlayConsume((EConsumeMode)mode);
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0018CB3D File Offset: 0x0018AD3D
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askConsume")]
		public void ReceivePlayConsume(EConsumeMode mode)
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.consumeMode = mode;
				this.consume();
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0018CB60 File Offset: 0x0018AD60
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy || base.player.quests.IsCutsceneModeActive())
			{
				return false;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.consumeMode = EConsumeMode.USE;
			this.consume();
			if (Provider.isServer)
			{
				UseableConsumeable.SendPlayConsume.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), this.consumeMode);
			}
			return true;
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0018CBF0 File Offset: 0x0018ADF0
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy || base.player.quests.IsCutsceneModeActive())
			{
				return false;
			}
			if (!this.hasAid)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer)
			{
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.ConsumeableAid);
				if (!Provider.isServer && raycastInfo.player != null)
				{
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.consumeMode = EConsumeMode.AID;
					this.consume();
				}
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.ConsumeableAid);
				if (input == null)
				{
					return false;
				}
				if (input.type == ERaycastInfoType.PLAYER && input.player != null)
				{
					this.enemy = input.player;
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.consumeMode = EConsumeMode.AID;
					this.consume();
					UseableConsumeable.SendPlayConsume.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), this.consumeMode);
				}
			}
			return true;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0018CD80 File Offset: 0x0018AF80
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.hasAid = ((ItemConsumeableAsset)base.player.equipment.asset).hasAid;
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
			if (this.hasAid)
			{
				this.aidTime = base.player.animator.GetAnimationLength("Aid", true);
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0018CE04 File Offset: 0x0018B004
		protected void performHealth(Player target, byte delta)
		{
			if (delta == 0)
			{
				return;
			}
			float num = 1f + base.player.skills.mastery(2, 0) * 0.5f;
			int num2 = Mathf.RoundToInt((float)delta * num);
			target.life.askHeal((byte)num2, false, false);
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0018CE4D File Offset: 0x0018B04D
		protected void performBleeding(Player target, ItemConsumeableAsset.Bleeding bleedingModifier)
		{
			switch (bleedingModifier)
			{
			case ItemConsumeableAsset.Bleeding.None:
				return;
			case ItemConsumeableAsset.Bleeding.Heal:
				target.life.serverSetBleeding(false);
				return;
			case ItemConsumeableAsset.Bleeding.Cut:
				target.life.serverSetBleeding(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0018CE7C File Offset: 0x0018B07C
		protected void performBrokenBones(Player target, ItemConsumeableAsset.Bones bonesModifier)
		{
			switch (bonesModifier)
			{
			case ItemConsumeableAsset.Bones.None:
				return;
			case ItemConsumeableAsset.Bones.Heal:
				target.life.serverSetLegsBroken(false);
				return;
			case ItemConsumeableAsset.Bones.Break:
				target.life.serverSetLegsBroken(true);
				return;
			default:
				return;
			}
		}

		/// <summary>
		/// Called serverside when using consumeable on another player.
		/// </summary>
		// Token: 0x0600448B RID: 17547 RVA: 0x0018CEAC File Offset: 0x0018B0AC
		protected void performAid(ItemConsumeableAsset asset)
		{
			bool flag = true;
			UseableConsumeable.PerformingAidHandler performingAidHandler = UseableConsumeable.onPerformingAid;
			if (performingAidHandler != null)
			{
				performingAidHandler(base.player, this.enemy, asset, ref flag);
			}
			if (!flag)
			{
				base.player.equipment.dequip();
				return;
			}
			asset.GrantQuestRewards(this.enemy);
			asset.itemRewards.grantItems(this.enemy, EItemOrigin.CRAFT, false);
			byte health = this.enemy.life.health;
			byte virus = this.enemy.life.virus;
			bool isBleeding = this.enemy.life.isBleeding;
			bool isBroken = this.enemy.life.isBroken;
			float num = (float)base.player.equipment.quality / 100f;
			this.performHealth(this.enemy, asset.health);
			this.performBleeding(this.enemy, asset.bleedingModifier);
			this.performBrokenBones(this.enemy, asset.bonesModifier);
			byte food = this.enemy.life.food;
			this.enemy.life.askEat((byte)((float)asset.food * num));
			byte food2 = this.enemy.life.food;
			byte b = (byte)((float)asset.water * num);
			if (asset.foodConstrainsWater)
			{
				b = (byte)Mathf.Min((int)b, (int)(food2 - food));
			}
			this.enemy.life.askDrink(b);
			float num2 = 1f - this.enemy.skills.mastery(1, 2) * 0.5f;
			this.enemy.life.askInfect((byte)((float)asset.virus * num2));
			float num3 = 1f + this.enemy.skills.mastery(2, 0) * 0.5f;
			this.enemy.life.askDisinfect((byte)((float)asset.disinfectant * num3));
			if (base.player.equipment.quality < 50)
			{
				this.enemy.life.askInfect((byte)((float)(asset.food + asset.water) * 0.5f * (1f - (float)base.player.equipment.quality / 50f) * num2));
			}
			byte health2 = this.enemy.life.health;
			byte virus2 = this.enemy.life.virus;
			bool isBleeding2 = this.enemy.life.isBleeding;
			bool isBroken2 = this.enemy.life.isBroken;
			uint num4 = 0U;
			int num5 = 0;
			if (health2 > health)
			{
				num4 += (uint)Mathf.RoundToInt((float)(health2 - health) / 2f);
				num5++;
			}
			if (virus2 > virus)
			{
				num4 += (uint)Mathf.RoundToInt((float)(virus2 - virus) / 2f);
				num5++;
			}
			if (isBleeding && !isBleeding2)
			{
				num4 += 15U;
				num5++;
			}
			if (isBroken && !isBroken2)
			{
				num4 += 15U;
				num5++;
			}
			if (num4 > 0U)
			{
				base.player.skills.askPay(num4);
			}
			if (num5 > 0)
			{
				base.player.skills.askRep(num5);
			}
			this.enemy.life.serverModifyHallucination((float)asset.vision);
			this.enemy.life.serverModifyStamina((float)asset.energy);
			this.enemy.life.serverModifyWarmth(asset.warmth);
			this.enemy.skills.ServerModifyExperience(asset.experience);
			UseableConsumeable.PerformedAidHandler performedAidHandler = UseableConsumeable.onPerformedAid;
			if (performedAidHandler != null)
			{
				performedAidHandler(base.player, this.enemy);
			}
			if (asset.shouldDeleteAfterUse)
			{
				base.player.equipment.use();
				return;
			}
			base.player.equipment.dequip();
		}

		/// <summary>
		/// Called by owner and server when using consumeable on self.
		/// </summary>
		// Token: 0x0600448C RID: 17548 RVA: 0x0018D26C File Offset: 0x0018B46C
		protected void performUseOnSelf(ItemConsumeableAsset asset)
		{
			base.player.life.askRest(asset.energy);
			byte vision = base.player.life.vision;
			byte b = (byte)((float)asset.vision * (1f - base.player.skills.mastery(1, 2)));
			base.player.life.askView((byte)Mathf.Max((int)vision, (int)b));
			bool flag;
			if (base.channel.IsLocalPlayer && asset.vision > 0 && Provider.provider.achievementsService.getAchievement("Berries", out flag) && !flag)
			{
				Provider.provider.achievementsService.setAchievement("Berries");
			}
			base.player.life.simulatedModifyOxygen(asset.oxygen);
			base.player.life.simulatedModifyWarmth((short)asset.warmth);
			if (Provider.isServer)
			{
				if (!this.invokeConsumeRequested(asset))
				{
					base.player.equipment.dequip();
					return;
				}
				asset.GrantQuestRewards(base.player);
				asset.itemRewards.grantItems(base.player, EItemOrigin.CRAFT, false);
				Vector3 vector = base.transform.position + Vector3.up;
				this.performHealth(base.player, asset.health);
				this.performBleeding(base.player, asset.bleedingModifier);
				this.performBrokenBones(base.player, asset.bonesModifier);
				base.player.skills.ServerModifyExperience(asset.experience);
				byte food = base.player.life.food;
				base.player.life.askEat((byte)((float)asset.food * ((float)base.player.equipment.quality / 100f)));
				byte food2 = base.player.life.food;
				byte b2 = (byte)((float)asset.water * ((float)base.player.equipment.quality / 100f));
				if (asset.foodConstrainsWater)
				{
					b2 = (byte)Mathf.Min((int)b2, (int)(food2 - food));
				}
				base.player.life.askDrink(b2);
				base.player.life.askInfect((byte)((float)asset.virus * (1f - base.player.skills.mastery(1, 2) * 0.5f)));
				base.player.life.askDisinfect((byte)((float)asset.disinfectant * (1f + base.player.skills.mastery(2, 0) * 0.5f)));
				if (base.player.equipment.quality < 50)
				{
					base.player.life.askInfect((byte)((float)(asset.food + asset.water) * 0.5f * (1f - (float)base.player.equipment.quality / 50f) * (1f - base.player.skills.mastery(1, 2) * 0.5f)));
				}
				this.invokeConsumePerformed(asset);
				if (asset.shouldDeleteAfterUse)
				{
					base.player.equipment.use();
				}
				else
				{
					base.player.equipment.dequip();
				}
				if (asset.IsExplosive)
				{
					EffectAsset effectAsset = asset.FindExplosionEffectAsset();
					if (effectAsset != null)
					{
						EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
						{
							relevantDistance = EffectManager.LARGE,
							position = vector
						});
					}
					List<EPlayerKill> list;
					DamageTool.explode(vector, asset.range, EDeathCause.CHARGE, base.channel.owner.playerID.steamID, asset.playerDamageMultiplier.damage, asset.zombieDamageMultiplier.damage, asset.animalDamageMultiplier.damage, asset.barricadeDamage, asset.structureDamage, asset.vehicleDamage, asset.resourceDamage, asset.objectDamage, out list, EExplosionDamageType.CONVENTIONAL, 32f, true, false, EDamageOrigin.Food_Explosion, ERagdollEffect.NONE);
					if (asset.playerDamageMultiplier.damage > 0.5f)
					{
						EPlayerKill eplayerKill;
						base.player.life.askDamage(101, Vector3.up, EDeathCause.CHARGE, ELimb.SPINE, base.channel.owner.playerID.steamID, out eplayerKill);
					}
				}
			}
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0018D698 File Offset: 0x0018B898
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				ItemConsumeableAsset itemConsumeableAsset = (ItemConsumeableAsset)base.player.equipment.asset;
				if (itemConsumeableAsset == null)
				{
					base.player.equipment.dequip();
					return;
				}
				if (this.consumeMode == EConsumeMode.AID)
				{
					if (Provider.isServer && this.enemy != null)
					{
						this.performAid(itemConsumeableAsset);
						return;
					}
				}
				else
				{
					this.performUseOnSelf(itemConsumeableAsset);
				}
			}
		}

		// Token: 0x04002DE1 RID: 11745
		private float startedUse;

		// Token: 0x04002DE2 RID: 11746
		private float useTime;

		// Token: 0x04002DE3 RID: 11747
		private float aidTime;

		// Token: 0x04002DE4 RID: 11748
		private bool isUsing;

		// Token: 0x04002DE5 RID: 11749
		private EConsumeMode consumeMode;

		// Token: 0x04002DE6 RID: 11750
		private Player enemy;

		// Token: 0x04002DE7 RID: 11751
		private bool hasAid;

		// Token: 0x04002DE8 RID: 11752
		private static readonly ClientInstanceMethod<EConsumeMode> SendPlayConsume = ClientInstanceMethod<EConsumeMode>.Get(typeof(UseableConsumeable), "ReceivePlayConsume");

		// Token: 0x02000A11 RID: 2577
		// (Invoke) Token: 0x06004D5C RID: 19804
		public delegate void PerformingAidHandler(Player instigator, Player target, ItemConsumeableAsset asset, ref bool shouldAllow);

		// Token: 0x02000A12 RID: 2578
		// (Invoke) Token: 0x06004D60 RID: 19808
		public delegate void PerformedAidHandler(Player instigator, Player target);

		// Token: 0x02000A13 RID: 2579
		// (Invoke) Token: 0x06004D64 RID: 19812
		public delegate void ConsumeRequestedHandler(Player instigatingPlayer, ItemConsumeableAsset consumeableAsset, ref bool shouldAllow);

		// Token: 0x02000A14 RID: 2580
		// (Invoke) Token: 0x06004D68 RID: 19816
		public delegate void ConsumePerformedHandler(Player instigatingPlayer, ItemConsumeableAsset consumeableAsset);
	}
}
