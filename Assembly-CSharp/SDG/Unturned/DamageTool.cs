using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000751 RID: 1873
	public class DamageTool
	{
		/// <summary>
		/// Replacement for playerDamaged.
		/// </summary>
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06003D2B RID: 15659 RVA: 0x00123DDC File Offset: 0x00121FDC
		// (remove) Token: 0x06003D2C RID: 15660 RVA: 0x00123E10 File Offset: 0x00122010
		public static event DamageTool.DamagePlayerHandler damagePlayerRequested;

		/// <summary>
		/// Replacement for zombieDamaged.
		/// </summary>
		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06003D2D RID: 15661 RVA: 0x00123E44 File Offset: 0x00122044
		// (remove) Token: 0x06003D2E RID: 15662 RVA: 0x00123E78 File Offset: 0x00122078
		public static event DamageTool.DamageZombieHandler damageZombieRequested;

		/// <summary>
		/// Replacement for animalDamaged.
		/// </summary>
		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06003D2F RID: 15663 RVA: 0x00123EAC File Offset: 0x001220AC
		// (remove) Token: 0x06003D30 RID: 15664 RVA: 0x00123EE0 File Offset: 0x001220E0
		public static event DamageTool.DamageAnimalHandler damageAnimalRequested;

		// Token: 0x06003D31 RID: 15665 RVA: 0x00123F14 File Offset: 0x00122114
		public static ELimb getLimb(Transform limb)
		{
			if (limb.CompareTag("Player") || limb.CompareTag("Enemy") || limb.CompareTag("Zombie") || limb.CompareTag("Animal"))
			{
				string name = limb.name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1038775360U)
				{
					if (num <= 628306286U)
					{
						if (num != 517858570U)
						{
							if (num != 588717584U)
							{
								if (num == 628306286U)
								{
									if (name == "Right_Foot")
									{
										return ELimb.RIGHT_FOOT;
									}
								}
							}
							else if (name == "Right_Arm")
							{
								return ELimb.RIGHT_ARM;
							}
						}
						else if (name == "Left_Back")
						{
							return ELimb.LEFT_BACK;
						}
					}
					else if (num <= 747356269U)
					{
						if (num != 632426989U)
						{
							if (num == 747356269U)
							{
								if (name == "Right_Back")
								{
									return ELimb.RIGHT_BACK;
								}
							}
						}
						else if (name == "Left_Arm")
						{
							return ELimb.LEFT_ARM;
						}
					}
					else if (num != 955854986U)
					{
						if (num == 1038775360U)
						{
							if (name == "Left_Front")
							{
								return ELimb.LEFT_FRONT;
							}
						}
					}
					else if (name == "Skull")
					{
						return ELimb.SKULL;
					}
				}
				else if (num <= 1823764719U)
				{
					if (num != 1309047352U)
					{
						if (num != 1413449684U)
						{
							if (num == 1823764719U)
							{
								if (name == "Left_Leg")
								{
									return ELimb.LEFT_LEG;
								}
							}
						}
						else if (name == "Spine")
						{
							return ELimb.SPINE;
						}
					}
					else if (name == "Left_Hand")
					{
						return ELimb.LEFT_HAND;
					}
				}
				else if (num <= 2765984090U)
				{
					if (num != 2757759699U)
					{
						if (num == 2765984090U)
						{
							if (name == "Right_Leg")
							{
								return ELimb.RIGHT_LEG;
							}
						}
					}
					else if (name == "Right_Hand")
					{
						return ELimb.RIGHT_HAND;
					}
				}
				else if (num != 3048219633U)
				{
					if (num == 3833091225U)
					{
						if (name == "Right_Front")
						{
							return ELimb.RIGHT_FRONT;
						}
					}
				}
				else if (name == "Left_Foot")
				{
					return ELimb.LEFT_FOOT;
				}
			}
			return ELimb.SPINE;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0012415C File Offset: 0x0012235C
		public static Player getPlayer(Transform limb)
		{
			Player player = limb.GetComponentInParent<Player>();
			if (player != null && player.life.isDead)
			{
				player = null;
			}
			return player;
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0012418C File Offset: 0x0012238C
		public static Zombie getZombie(Transform limb)
		{
			Zombie zombie = limb.GetComponentInParent<Zombie>();
			if (zombie != null && zombie.isDead)
			{
				zombie = null;
			}
			return zombie;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x001241B4 File Offset: 0x001223B4
		public static Animal getAnimal(Transform limb)
		{
			Animal animal = limb.GetComponentInParent<Animal>();
			if (animal != null && animal.isDead)
			{
				animal = null;
			}
			return animal;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x001241DC File Offset: 0x001223DC
		public static InteractableVehicle getVehicle(Transform model)
		{
			if (model == null)
			{
				return null;
			}
			model = model.root;
			InteractableVehicle component = model.GetComponent<InteractableVehicle>();
			if (component != null)
			{
				return component;
			}
			VehicleRef component2 = model.GetComponent<VehicleRef>();
			if (component2 != null)
			{
				return component2.vehicle;
			}
			return null;
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00124228 File Offset: 0x00122428
		public static Transform getBarricadeRootTransform(Transform barricadeTransform)
		{
			Transform transform = barricadeTransform;
			for (;;)
			{
				Transform parent = transform.parent;
				if (parent == null)
				{
					break;
				}
				if (parent.CompareTag("Vehicle"))
				{
					return transform;
				}
				transform = parent;
			}
			return transform;
		}

		/// <summary>
		/// Was necessary when structures were children of level transform.
		/// </summary>
		// Token: 0x06003D37 RID: 15671 RVA: 0x0012425B File Offset: 0x0012245B
		public static Transform getStructureRootTransform(Transform structureTransform)
		{
			return structureTransform.root;
		}

		/// <summary>
		/// Was necessary when trees were children of ground transform.
		/// </summary>
		// Token: 0x06003D38 RID: 15672 RVA: 0x00124263 File Offset: 0x00122463
		public static Transform getResourceRootTransform(Transform resourceTransform)
		{
			return resourceTransform.root;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x0012426C File Offset: 0x0012246C
		public static void damagePlayer(DamagePlayerParameters parameters, out EPlayerKill kill)
		{
			if (parameters.player == null || parameters.player.life.isDead)
			{
				kill = EPlayerKill.NONE;
				return;
			}
			bool flag = true;
			DamageTool.DamagePlayerHandler damagePlayerHandler = DamageTool.damagePlayerRequested;
			if (damagePlayerHandler != null)
			{
				damagePlayerHandler(ref parameters, ref flag);
			}
			if (DamageTool.playerDamaged != null)
			{
				DamageTool.playerDamaged(parameters.player, ref parameters.cause, ref parameters.limb, ref parameters.killer, ref parameters.direction, ref parameters.damage, ref parameters.times, ref flag);
			}
			if (!flag)
			{
				kill = EPlayerKill.NONE;
				return;
			}
			if (parameters.respectArmor)
			{
				parameters.times *= DamageTool.getPlayerArmor(parameters.limb, parameters.player);
			}
			if (parameters.applyGlobalArmorMultiplier)
			{
				parameters.times *= Provider.modeConfigData.Players.Armor_Multiplier;
			}
			int num = Mathf.FloorToInt(parameters.damage * parameters.times);
			if (num == 0)
			{
				kill = EPlayerKill.NONE;
				return;
			}
			byte b = (byte)Mathf.Min(255, num);
			bool flag2 = parameters.player.life.InternalCanDamage();
			bool canCauseBleeding;
			switch (parameters.bleedingModifier)
			{
			default:
				canCauseBleeding = true;
				break;
			case DamagePlayerParameters.Bleeding.Always:
				canCauseBleeding = false;
				if (flag2)
				{
					parameters.player.life.serverSetBleeding(true);
				}
				break;
			case DamagePlayerParameters.Bleeding.Never:
				canCauseBleeding = false;
				break;
			case DamagePlayerParameters.Bleeding.Heal:
				canCauseBleeding = false;
				parameters.player.life.serverSetBleeding(false);
				break;
			}
			parameters.player.life.askDamage(b, parameters.direction * (float)b, parameters.cause, parameters.limb, parameters.killer, out kill, parameters.trackKill, parameters.ragdollEffect, canCauseBleeding);
			switch (parameters.bonesModifier)
			{
			case DamagePlayerParameters.Bones.Always:
				if (flag2)
				{
					parameters.player.life.serverSetLegsBroken(true);
				}
				break;
			case DamagePlayerParameters.Bones.Heal:
				parameters.player.life.serverSetLegsBroken(false);
				break;
			}
			if (parameters.foodModifier > 0f || flag2)
			{
				parameters.player.life.serverModifyFood(parameters.foodModifier);
			}
			if (parameters.waterModifier > 0f || flag2)
			{
				parameters.player.life.serverModifyWater(parameters.waterModifier);
			}
			if (parameters.virusModifier > 0f || flag2)
			{
				parameters.player.life.serverModifyVirus(parameters.virusModifier);
			}
			if (parameters.hallucinationModifier < 0f || flag2)
			{
				parameters.player.life.serverModifyHallucination(parameters.hallucinationModifier);
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x001244F0 File Offset: 0x001226F0
		public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, float damage, float times, out EPlayerKill kill, bool applyGlobalArmorMultiplier = true, bool trackKill = false, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageTool.damagePlayer(new DamagePlayerParameters(player)
			{
				cause = cause,
				limb = limb,
				killer = killer,
				direction = direction,
				damage = damage,
				times = times,
				applyGlobalArmorMultiplier = applyGlobalArmorMultiplier,
				trackKill = trackKill,
				ragdollEffect = ragdollEffect
			}, out kill);
		}

		/// <summary>
		/// Get average explosionArmor of player's equipped clothing.
		/// </summary>
		// Token: 0x06003D3B RID: 15675 RVA: 0x0012455C File Offset: 0x0012275C
		public static float getPlayerExplosionArmor(Player player)
		{
			if (player == null)
			{
				return 1f;
			}
			float num = 0f;
			ItemPantsAsset pantsAsset = player.clothing.pantsAsset;
			float num2 = num + ((pantsAsset != null) ? pantsAsset.explosionArmor : 1f);
			ItemShirtAsset shirtAsset = player.clothing.shirtAsset;
			float num3 = num2 + ((shirtAsset != null) ? shirtAsset.explosionArmor : 1f);
			ItemVestAsset vestAsset = player.clothing.vestAsset;
			float num4 = num3 + ((vestAsset != null) ? vestAsset.explosionArmor : 1f);
			ItemHatAsset hatAsset = player.clothing.hatAsset;
			return (num4 + ((hatAsset != null) ? hatAsset.explosionArmor : 1f)) / 4f;
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001245F4 File Offset: 0x001227F4
		public static float getPlayerArmor(ELimb limb, Player player)
		{
			if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
			{
				ItemClothingAsset pantsAsset = player.clothing.pantsAsset;
				if (pantsAsset != null)
				{
					if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && player.clothing.pantsQuality > 0)
					{
						PlayerClothing clothing = player.clothing;
						clothing.pantsQuality -= 1;
						player.clothing.sendUpdatePantsQuality();
					}
					return pantsAsset.armor + (1f - pantsAsset.armor) * (1f - (float)player.clothing.pantsQuality / 100f);
				}
			}
			else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM)
			{
				ItemClothingAsset shirtAsset = player.clothing.shirtAsset;
				if (shirtAsset != null)
				{
					if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && player.clothing.shirtQuality > 0)
					{
						PlayerClothing clothing2 = player.clothing;
						clothing2.shirtQuality -= 1;
						player.clothing.sendUpdateShirtQuality();
					}
					return shirtAsset.armor + (1f - shirtAsset.armor) * (1f - (float)player.clothing.shirtQuality / 100f);
				}
			}
			else
			{
				if (limb == ELimb.SPINE)
				{
					float num = 1f;
					if (player.clothing.vestAsset != null)
					{
						ItemClothingAsset vestAsset = player.clothing.vestAsset;
						if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && player.clothing.vestQuality > 0)
						{
							PlayerClothing clothing3 = player.clothing;
							clothing3.vestQuality -= 1;
							player.clothing.sendUpdateVestQuality();
						}
						num *= vestAsset.armor + (1f - vestAsset.armor) * (1f - (float)player.clothing.vestQuality / 100f);
					}
					if (player.clothing.shirtAsset != null)
					{
						ItemClothingAsset shirtAsset2 = player.clothing.shirtAsset;
						if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && player.clothing.shirtQuality > 0)
						{
							PlayerClothing clothing4 = player.clothing;
							clothing4.shirtQuality -= 1;
							player.clothing.sendUpdateShirtQuality();
						}
						num *= shirtAsset2.armor + (1f - shirtAsset2.armor) * (1f - (float)player.clothing.shirtQuality / 100f);
					}
					return num;
				}
				if (limb == ELimb.SKULL)
				{
					ItemClothingAsset hatAsset = player.clothing.hatAsset;
					if (hatAsset != null)
					{
						if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && player.clothing.hatQuality > 0)
						{
							PlayerClothing clothing5 = player.clothing;
							clothing5.hatQuality -= 1;
							player.clothing.sendUpdateHatQuality();
						}
						return hatAsset.armor + (1f - hatAsset.armor) * (1f - (float)player.clothing.hatQuality / 100f);
					}
				}
			}
			return 1f;
		}

		/// <summary>
		/// Refer to getPlayerExplosionArmor for explanation of total/average.
		/// </summary>
		// Token: 0x06003D3D RID: 15677 RVA: 0x001248C4 File Offset: 0x00122AC4
		public static float GetZombieExplosionArmor(Zombie zombie)
		{
			if ((int)zombie.type < LevelZombies.tables.Count)
			{
				float num = 0f;
				if (zombie.pants != 255 && (int)zombie.pants < LevelZombies.tables[(int)zombie.type].slots[1].table.Count)
				{
					ItemClothingAsset itemClothingAsset = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[1].table[(int)zombie.pants].item) as ItemClothingAsset;
					num += ((itemClothingAsset != null) ? itemClothingAsset.explosionArmor : 1f);
				}
				else
				{
					num += 1f;
				}
				if (zombie.shirt != 255 && (int)zombie.shirt < LevelZombies.tables[(int)zombie.type].slots[0].table.Count)
				{
					ItemClothingAsset itemClothingAsset2 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[0].table[(int)zombie.shirt].item) as ItemClothingAsset;
					num += ((itemClothingAsset2 != null) ? itemClothingAsset2.explosionArmor : 1f);
				}
				else
				{
					num += 1f;
				}
				if (zombie.gear != 255 && (int)zombie.gear < LevelZombies.tables[(int)zombie.type].slots[3].table.Count)
				{
					ItemClothingAsset itemClothingAsset3 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[3].table[(int)zombie.gear].item) as ItemClothingAsset;
					num += ((itemClothingAsset3 != null) ? itemClothingAsset3.explosionArmor : 1f);
				}
				else
				{
					num += 1f;
				}
				if (zombie.hat != 255 && (int)zombie.hat < LevelZombies.tables[(int)zombie.type].slots[2].table.Count)
				{
					ItemClothingAsset itemClothingAsset4 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[2].table[(int)zombie.hat].item) as ItemClothingAsset;
					num += ((itemClothingAsset4 != null) ? itemClothingAsset4.explosionArmor : 1f);
				}
				else
				{
					num += 1f;
				}
				return num / 4f;
			}
			return 1f;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00124B28 File Offset: 0x00122D28
		public static float getZombieArmor(ELimb limb, Zombie zombie)
		{
			if ((int)zombie.type < LevelZombies.tables.Count)
			{
				if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
				{
					if (zombie.pants != 255 && (int)zombie.pants < LevelZombies.tables[(int)zombie.type].slots[1].table.Count)
					{
						ItemClothingAsset itemClothingAsset = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[1].table[(int)zombie.pants].item) as ItemClothingAsset;
						if (itemClothingAsset != null)
						{
							return itemClothingAsset.armor;
						}
					}
				}
				else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM)
				{
					if (zombie.shirt != 255 && (int)zombie.shirt < LevelZombies.tables[(int)zombie.type].slots[0].table.Count)
					{
						ItemClothingAsset itemClothingAsset2 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[0].table[(int)zombie.shirt].item) as ItemClothingAsset;
						if (itemClothingAsset2 != null)
						{
							return itemClothingAsset2.armor;
						}
					}
				}
				else
				{
					if (limb == ELimb.SPINE)
					{
						float num = 1f;
						if (zombie.gear != 255 && (int)zombie.gear < LevelZombies.tables[(int)zombie.type].slots[3].table.Count)
						{
							ItemAsset itemAsset = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[3].table[(int)zombie.gear].item) as ItemAsset;
							if (itemAsset != null && itemAsset.type == EItemType.VEST)
							{
								num *= ((ItemClothingAsset)itemAsset).armor;
							}
						}
						if (zombie.shirt != 255 && (int)zombie.shirt < LevelZombies.tables[(int)zombie.type].slots[0].table.Count)
						{
							ItemClothingAsset itemClothingAsset3 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[0].table[(int)zombie.shirt].item) as ItemClothingAsset;
							if (itemClothingAsset3 != null)
							{
								num *= itemClothingAsset3.armor;
							}
						}
						return num;
					}
					if (limb == ELimb.SKULL && zombie.hat != 255 && (int)zombie.hat < LevelZombies.tables[(int)zombie.type].slots[2].table.Count)
					{
						ItemClothingAsset itemClothingAsset4 = Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[2].table[(int)zombie.hat].item) as ItemClothingAsset;
						if (itemClothingAsset4 != null)
						{
							return itemClothingAsset4.armor;
						}
					}
				}
			}
			return 1f;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00124E0C File Offset: 0x0012300C
		public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, IDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill, bool trackKill = false, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamagePlayerParameters parameters = DamagePlayerParameters.make(player, cause, direction, multiplier, limb);
			parameters.killer = killer;
			parameters.times = times;
			parameters.respectArmor = armor;
			parameters.trackKill = trackKill;
			parameters.ragdollEffect = ragdollEffect;
			DamageTool.damagePlayer(parameters, out kill);
		}

		/// <summary>
		/// Do damage to a zombie.
		/// </summary>
		// Token: 0x06003D40 RID: 15680 RVA: 0x00124E5C File Offset: 0x0012305C
		public static void damageZombie(DamageZombieParameters parameters, out EPlayerKill kill, out uint xp)
		{
			if (parameters.zombie == null || parameters.zombie.isDead)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			if (parameters.respectArmor)
			{
				parameters.times *= DamageTool.getZombieArmor(parameters.limb, parameters.zombie);
			}
			if (parameters.allowBackstab && (double)Vector3.Dot(parameters.zombie.transform.forward, parameters.direction) > 0.5)
			{
				parameters.times *= Provider.modeConfigData.Zombies.Backstab_Multiplier;
				if (Provider.modeConfigData.Zombies.Only_Critical_Stuns && parameters.zombieStunOverride == EZombieStunOverride.None)
				{
					parameters.zombieStunOverride = EZombieStunOverride.Always;
				}
			}
			bool flag = true;
			DamageTool.DamageZombieHandler damageZombieHandler = DamageTool.damageZombieRequested;
			if (damageZombieHandler != null)
			{
				damageZombieHandler(ref parameters, ref flag);
			}
			if (DamageTool.zombieDamaged != null)
			{
				DamageTool.zombieDamaged(parameters.zombie, ref parameters.direction, ref parameters.damage, ref parameters.times, ref flag);
			}
			if (!flag)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			if (parameters.applyGlobalArmorMultiplier)
			{
				if (parameters.limb == ELimb.SKULL)
				{
					parameters.times *= Provider.modeConfigData.Zombies.Armor_Multiplier;
				}
				else
				{
					parameters.times *= Provider.modeConfigData.Zombies.NonHeadshot_Armor_Multiplier;
				}
			}
			int num = Mathf.FloorToInt(parameters.damage * parameters.times);
			if (num == 0)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			ushort num2 = (ushort)Mathf.Min(65535, num);
			parameters.zombie.askDamage(num2, parameters.direction * (float)num2, out kill, out xp, true, true, parameters.zombieStunOverride, parameters.ragdollEffect);
			if (parameters.AlertPosition != null)
			{
				parameters.zombie.alert(parameters.AlertPosition.Value, true);
			}
		}

		/// <summary>
		/// Legacy function replaced by damageZombie.
		/// </summary>
		// Token: 0x06003D41 RID: 15681 RVA: 0x00125030 File Offset: 0x00123230
		public static void damage(Zombie zombie, Vector3 direction, float damage, float times, out EPlayerKill kill, out uint xp, EZombieStunOverride zombieStunOverride = EZombieStunOverride.None, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageTool.damageZombie(new DamageZombieParameters(zombie, direction, damage)
			{
				times = times,
				zombieStunOverride = zombieStunOverride,
				ragdollEffect = ragdollEffect
			}, out kill, out xp);
		}

		/// <summary>
		/// Legacy function replaced by damageZombie.
		/// </summary>
		// Token: 0x06003D42 RID: 15682 RVA: 0x0012506C File Offset: 0x0012326C
		public static void damage(Zombie zombie, ELimb limb, Vector3 direction, IDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill, out uint xp, EZombieStunOverride zombieStunOverride = EZombieStunOverride.None, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageZombieParameters parameters = DamageZombieParameters.make(zombie, direction, multiplier, limb);
			parameters.legacyArmor = armor;
			parameters.times = times;
			parameters.zombieStunOverride = zombieStunOverride;
			parameters.ragdollEffect = ragdollEffect;
			DamageTool.damageZombie(parameters, out kill, out xp);
		}

		/// <summary>
		/// Do damage to an animal.
		/// </summary>
		// Token: 0x06003D43 RID: 15683 RVA: 0x001250B4 File Offset: 0x001232B4
		public static void damageAnimal(DamageAnimalParameters parameters, out EPlayerKill kill, out uint xp)
		{
			if (parameters.animal == null || parameters.animal.isDead)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			bool flag = true;
			DamageTool.DamageAnimalHandler damageAnimalHandler = DamageTool.damageAnimalRequested;
			if (damageAnimalHandler != null)
			{
				damageAnimalHandler(ref parameters, ref flag);
			}
			if (DamageTool.animalDamaged != null)
			{
				DamageTool.animalDamaged(parameters.animal, ref parameters.direction, ref parameters.damage, ref parameters.times, ref flag);
			}
			if (!flag)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			if (parameters.applyGlobalArmorMultiplier)
			{
				parameters.times *= Provider.modeConfigData.Animals.Armor_Multiplier;
			}
			int num = Mathf.FloorToInt(parameters.damage * parameters.times);
			if (num == 0)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			ushort num2 = (ushort)Mathf.Min(65535, num);
			parameters.animal.askDamage(num2, parameters.direction * (float)num2, out kill, out xp, true, true, parameters.ragdollEffect);
			if (parameters.AlertPosition != null)
			{
				parameters.animal.alertDamagedFromPoint(parameters.AlertPosition.Value);
			}
		}

		/// <summary>
		/// Legacy function replaced by damageAnimal.
		/// </summary>
		// Token: 0x06003D44 RID: 15684 RVA: 0x001251CC File Offset: 0x001233CC
		public static void damage(Animal animal, Vector3 direction, float damage, float times, out EPlayerKill kill, out uint xp, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageTool.damageAnimal(new DamageAnimalParameters(animal, direction, damage)
			{
				times = times,
				ragdollEffect = ragdollEffect
			}, out kill, out xp);
		}

		/// <summary>
		/// Legacy function replaced by damageAnimal.
		/// </summary>
		// Token: 0x06003D45 RID: 15685 RVA: 0x00125200 File Offset: 0x00123400
		public static void damage(Animal animal, ELimb limb, Vector3 direction, IDamageMultiplier multiplier, float times, out EPlayerKill kill, out uint xp, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageAnimalParameters parameters = DamageAnimalParameters.make(animal, direction, multiplier, limb);
			parameters.times = times;
			parameters.ragdollEffect = ragdollEffect;
			DamageTool.damageAnimal(parameters, out kill, out xp);
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00125234 File Offset: 0x00123434
		public static void damage(InteractableVehicle vehicle, bool damageTires, Vector3 position, bool isRepairing, float vehicleDamage, float times, bool canRepair, out EPlayerKill kill, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			kill = EPlayerKill.NONE;
			if (vehicle == null)
			{
				return;
			}
			if (isRepairing)
			{
				if (!vehicle.isExploded && !vehicle.isRepaired)
				{
					VehicleManager.repair(vehicle, vehicleDamage, times, instigatorSteamID);
					return;
				}
			}
			else
			{
				if (!vehicle.isDead)
				{
					VehicleManager.damage(vehicle, vehicleDamage, times, canRepair, instigatorSteamID, damageOrigin);
				}
				if (damageTires && !vehicle.isExploded)
				{
					int hitTireIndex = vehicle.getHitTireIndex(position);
					if (hitTireIndex != -1)
					{
						VehicleManager.damageTire(vehicle, hitTireIndex, instigatorSteamID, damageOrigin);
					}
				}
			}
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x001252A9 File Offset: 0x001234A9
		public static void damage(Transform barricade, bool isRepairing, float barricadeDamage, float times, out EPlayerKill kill, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			kill = EPlayerKill.NONE;
			if (barricade == null)
			{
				return;
			}
			if (isRepairing)
			{
				BarricadeManager.repair(barricade, barricadeDamage, times, instigatorSteamID);
				return;
			}
			BarricadeManager.damage(barricade, barricadeDamage, times, true, instigatorSteamID, damageOrigin);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x001252D4 File Offset: 0x001234D4
		public static void damage(Transform structure, bool isRepairing, Vector3 direction, float structureDamage, float times, out EPlayerKill kill, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			kill = EPlayerKill.NONE;
			if (structure == null)
			{
				return;
			}
			if (isRepairing)
			{
				StructureManager.repair(structure, structureDamage, times, instigatorSteamID);
				return;
			}
			StructureManager.damage(structure, direction, structureDamage, times, true, instigatorSteamID, damageOrigin);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00125304 File Offset: 0x00123504
		public static void damage(Transform resource, Vector3 direction, float resourceDamage, float times, float drops, out EPlayerKill kill, out uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			if (resource == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			ResourceManager.damage(resource, direction, resourceDamage, times, drops, out kill, out xp, instigatorSteamID, damageOrigin, true);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00125338 File Offset: 0x00123538
		public static void damage(Transform obj, Vector3 direction, byte section, float objectDamage, float times, out EPlayerKill kill, out uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			if (obj == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0U;
				return;
			}
			ObjectManager.damage(obj, direction, section, objectDamage, times, out kill, out xp, instigatorSteamID, damageOrigin, true);
		}

		/// <summary>
		/// This unwieldy mess is the original explode function, but should be maintained for backwards compatibility with plugins.
		/// </summary>
		// Token: 0x06003D4B RID: 15691 RVA: 0x0012536C File Offset: 0x0012356C
		public static void explode(Vector3 point, float damageRadius, EDeathCause cause, CSteamID killer, float playerDamage, float zombieDamage, float animalDamage, float barricadeDamage, float structureDamage, float vehicleDamage, float resourceDamage, float objectDamage, out List<EPlayerKill> kills, EExplosionDamageType damageType = EExplosionDamageType.CONVENTIONAL, float alertRadius = 32f, bool playImpactEffect = true, bool penetrateBuildables = false, EDamageOrigin damageOrigin = EDamageOrigin.Unknown, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			DamageTool.explode(new ExplosionParameters(point, damageRadius, cause, killer)
			{
				playerDamage = playerDamage,
				zombieDamage = zombieDamage,
				animalDamage = animalDamage,
				barricadeDamage = barricadeDamage,
				structureDamage = structureDamage,
				vehicleDamage = vehicleDamage,
				resourceDamage = resourceDamage,
				objectDamage = objectDamage,
				damageType = damageType,
				alertRadius = alertRadius,
				playImpactEffect = playImpactEffect,
				penetrateBuildables = penetrateBuildables,
				damageOrigin = damageOrigin,
				ragdollEffect = ragdollEffect,
				launchSpeed = playerDamage * 0.1f
			}, out kills);
		}

		/// <summary>
		/// Do radial damage.
		/// </summary>
		// Token: 0x06003D4C RID: 15692 RVA: 0x0012541C File Offset: 0x0012361C
		public static void explode(ExplosionParameters parameters, out List<EPlayerKill> kills)
		{
			DamageTool.explosionKills.Clear();
			kills = DamageTool.explosionKills;
			DamageTool.explosionRangeComparator.point = parameters.point;
			float num = parameters.damageRadius * parameters.damageRadius;
			DamageTool.regionsInRadius.Clear();
			Regions.getRegionsInRadius(parameters.point, parameters.damageRadius, DamageTool.regionsInRadius);
			int layerMask;
			if (parameters.penetrateBuildables)
			{
				layerMask = RayMasks.BLOCK_EXPLOSION_PENETRATE_BUILDABLES;
			}
			else
			{
				layerMask = RayMasks.BLOCK_EXPLOSION;
			}
			if (parameters.structureDamage > 0.5f)
			{
				DamageTool.structuresInRadius.Clear();
				StructureManager.getStructuresInRadius(parameters.point, num, DamageTool.regionsInRadius, DamageTool.structuresInRadius);
				DamageTool.structuresInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int i = 0; i < DamageTool.structuresInRadius.Count; i++)
				{
					Transform transform = DamageTool.structuresInRadius[i];
					if (!(transform == null))
					{
						StructureDrop structureDrop = StructureDrop.FindByRootFast(transform);
						if (structureDrop != null)
						{
							ItemStructureAsset asset = structureDrop.asset;
							if (asset != null && !asset.proofExplosion)
							{
								Vector3 a = CollisionUtil.ClosestPoint(transform.gameObject, parameters.point, false) - parameters.point;
								float sqrMagnitude = a.sqrMagnitude;
								if (sqrMagnitude < num)
								{
									float num2 = Mathf.Sqrt(sqrMagnitude);
									Vector3 direction = a / num2;
									if (num2 > 0.01f)
									{
										Ray ray = new Ray(parameters.point, direction);
										float maxDistance = num2 - 0.01f;
										RaycastHit raycastHit;
										Physics.Raycast(ray, out raycastHit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
										if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(transform))
										{
											goto IL_1B6;
										}
									}
									StructureManager.damage(transform, a.normalized, parameters.structureDamage, 1f - num2 / parameters.damageRadius, true, parameters.killer, parameters.damageOrigin);
								}
							}
						}
					}
					IL_1B6:;
				}
			}
			if (parameters.resourceDamage > 0.5f)
			{
				DamageTool.resourcesInRadius.Clear();
				ResourceManager.getResourcesInRadius(parameters.point, num, DamageTool.regionsInRadius, DamageTool.resourcesInRadius);
				DamageTool.resourcesInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int j = 0; j < DamageTool.resourcesInRadius.Count; j++)
				{
					Transform transform2 = DamageTool.resourcesInRadius[j];
					if (!(transform2 == null))
					{
						Vector3 a2 = CollisionUtil.ClosestPoint(transform2.gameObject, parameters.point, false) - parameters.point;
						float sqrMagnitude2 = a2.sqrMagnitude;
						if (sqrMagnitude2 < num)
						{
							float num3 = Mathf.Sqrt(sqrMagnitude2);
							Vector3 direction2 = a2 / num3;
							if (num3 > 0.01f)
							{
								Ray ray2 = new Ray(parameters.point, direction2);
								float maxDistance2 = num3 - 0.01f;
								RaycastHit raycastHit;
								Physics.Raycast(ray2, out raycastHit, maxDistance2, layerMask, QueryTriggerInteraction.Ignore);
								if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(transform2))
								{
									goto IL_304;
								}
							}
							EPlayerKill eplayerKill;
							uint num4;
							ResourceManager.damage(transform2, a2.normalized, parameters.resourceDamage, 1f - num3 / parameters.damageRadius, 1f, out eplayerKill, out num4, parameters.killer, parameters.damageOrigin, true);
							if (eplayerKill != EPlayerKill.NONE)
							{
								kills.Add(eplayerKill);
							}
						}
					}
					IL_304:;
				}
			}
			if (parameters.objectDamage > 0.5f)
			{
				DamageTool.objectsInRadius.Clear();
				ObjectManager.getObjectsInRadius(parameters.point, num, DamageTool.regionsInRadius, DamageTool.objectsInRadius);
				DamageTool.objectsInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int k = 0; k < DamageTool.objectsInRadius.Count; k++)
				{
					Transform transform3 = DamageTool.objectsInRadius[k];
					if (!(transform3 == null))
					{
						InteractableObjectRubble componentInParent = transform3.GetComponentInParent<InteractableObjectRubble>();
						if (!(componentInParent == null) && !componentInParent.asset.rubbleProofExplosion)
						{
							for (byte b = 0; b < componentInParent.getSectionCount(); b += 1)
							{
								RubbleInfo sectionInfo = componentInParent.getSectionInfo(b);
								if (!sectionInfo.isDead)
								{
									Vector3 a3 = sectionInfo.section.position;
									if (sectionInfo.aliveGameObject != null)
									{
										a3 = CollisionUtil.ClosestPoint(sectionInfo.section.gameObject, parameters.point, false);
									}
									Vector3 a4 = a3 - parameters.point;
									float sqrMagnitude3 = a4.sqrMagnitude;
									if (sqrMagnitude3 < num)
									{
										float num5 = Mathf.Sqrt(sqrMagnitude3);
										Vector3 direction3 = a4 / num5;
										if (num5 > 0.01f)
										{
											Ray ray3 = new Ray(parameters.point, direction3);
											float maxDistance3 = num5 - 0.01f;
											RaycastHit raycastHit;
											Physics.Raycast(ray3, out raycastHit, maxDistance3, layerMask, QueryTriggerInteraction.Ignore);
											if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(componentInParent.transform))
											{
												goto IL_4C5;
											}
										}
										EPlayerKill eplayerKill;
										uint num4;
										ObjectManager.damage(componentInParent.transform, a4.normalized, b, parameters.objectDamage, 1f - num5 / parameters.damageRadius, out eplayerKill, out num4, parameters.killer, parameters.damageOrigin, true);
										if (eplayerKill != EPlayerKill.NONE)
										{
											kills.Add(eplayerKill);
										}
									}
								}
								IL_4C5:;
							}
						}
					}
				}
			}
			if (parameters.barricadeDamage > 0.5f)
			{
				DamageTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(parameters.point, num, DamageTool.regionsInRadius, DamageTool.barricadesInRadius);
				BarricadeManager.getBarricadesInRadius(parameters.point, num, DamageTool.barricadesInRadius);
				DamageTool.barricadesInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int l = 0; l < DamageTool.barricadesInRadius.Count; l++)
				{
					Transform transform4 = DamageTool.barricadesInRadius[l];
					if (!(transform4 == null))
					{
						Vector3 a5 = CollisionUtil.ClosestPoint(transform4.gameObject, parameters.point, false) - parameters.point;
						float sqrMagnitude4 = a5.sqrMagnitude;
						if (sqrMagnitude4 < num)
						{
							float num6 = Mathf.Sqrt(sqrMagnitude4);
							Vector3 direction4 = a5 / num6;
							if (num6 > 0.01f)
							{
								Ray ray4 = new Ray(parameters.point, direction4);
								float maxDistance4 = num6 - 0.01f;
								RaycastHit raycastHit;
								Physics.Raycast(ray4, out raycastHit, maxDistance4, layerMask, QueryTriggerInteraction.Ignore);
								if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(transform4))
								{
									goto IL_641;
								}
							}
							BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(transform4);
							if (barricadeDrop != null)
							{
								ItemBarricadeAsset asset2 = barricadeDrop.asset;
								if (asset2 != null && !asset2.proofExplosion)
								{
									BarricadeManager.damage(transform4, parameters.barricadeDamage, 1f - num6 / parameters.damageRadius, true, parameters.killer, parameters.damageOrigin);
								}
							}
						}
					}
					IL_641:;
				}
			}
			bool flag = (Provider.isPvP || parameters.damageType == EExplosionDamageType.ZOMBIE_ACID || parameters.damageType == EExplosionDamageType.ZOMBIE_FIRE || parameters.damageType == EExplosionDamageType.ZOMBIE_ELECTRIC) && parameters.playerDamage > 0.5f;
			if (flag || parameters.launchSpeed > 0.01f)
			{
				DamageTool.playersInRadius.Clear();
				PlayerTool.getPlayersInRadius(parameters.point, num, DamageTool.playersInRadius);
				for (int m = 0; m < DamageTool.playersInRadius.Count; m++)
				{
					Player player = DamageTool.playersInRadius[m];
					if (!(player == null) && !player.life.isDead && (parameters.damageType != EExplosionDamageType.ZOMBIE_FIRE || player.clothing.shirtAsset == null || !player.clothing.shirtAsset.proofFire || player.clothing.pantsAsset == null || !player.clothing.pantsAsset.proofFire))
					{
						Vector3 a6 = CollisionUtil.ClosestPoint(player.gameObject, parameters.point, false) - parameters.point;
						float sqrMagnitude5 = a6.sqrMagnitude;
						if (sqrMagnitude5 < num)
						{
							float num7 = Mathf.Sqrt(sqrMagnitude5);
							Vector3 vector = a6 / num7;
							if (num7 > 0.01f)
							{
								Ray ray5 = new Ray(parameters.point, vector);
								float maxDistance5 = num7 - 0.01f;
								RaycastHit raycastHit;
								Physics.Raycast(ray5, out raycastHit, maxDistance5, layerMask, QueryTriggerInteraction.Ignore);
								if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(player.transform))
								{
									goto IL_992;
								}
							}
							if (flag)
							{
								if (parameters.playImpactEffect)
								{
									EffectAsset effectAsset = DamageTool.FleshDynamicRef.Find();
									if (effectAsset != null)
									{
										TriggerEffectParameters parameters2 = new TriggerEffectParameters(effectAsset)
										{
											relevantDistance = EffectManager.SMALL,
											position = player.transform.position + Vector3.up
										};
										EffectManager.triggerEffect(parameters2);
										parameters2.SetDirection(-vector);
										EffectManager.triggerEffect(parameters2);
									}
								}
								float num8 = 1f - MathfEx.Square(num7 / parameters.damageRadius);
								if (player.movement.getVehicle() != null && player.movement.getVehicle().asset != null)
								{
									num8 *= player.movement.getVehicle().asset.passengerExplosionArmor;
								}
								float playerExplosionArmor = DamageTool.getPlayerExplosionArmor(player);
								num8 *= playerExplosionArmor;
								EPlayerKill eplayerKill;
								DamageTool.damage(player, parameters.cause, ELimb.SPINE, parameters.killer, vector, parameters.playerDamage, num8, out eplayerKill, true, true, ERagdollEffect.NONE);
								if (eplayerKill != EPlayerKill.NONE && player.channel.owner.playerID.steamID != parameters.killer)
								{
									kills.Add(eplayerKill);
								}
							}
							if (parameters.launchSpeed > 0.01f)
							{
								Vector3 normalized = (player.transform.position + Vector3.up - parameters.point).normalized;
								float num9 = 1f - MathfEx.Square(num7 / parameters.damageRadius);
								num9 *= Provider.modeConfigData.Gameplay.Explosion_Launch_Speed_Multiplier;
								player.movement.pendingLaunchVelocity += normalized * parameters.launchSpeed * num9;
							}
						}
					}
					IL_992:;
				}
			}
			if (parameters.damageType == EExplosionDamageType.ZOMBIE_FIRE || parameters.zombieDamage > 0.5f)
			{
				DamageTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(parameters.point, num, DamageTool.zombiesInRadius);
				for (int n = 0; n < DamageTool.zombiesInRadius.Count; n++)
				{
					Zombie zombie = DamageTool.zombiesInRadius[n];
					if (!(zombie == null) && !zombie.isDead)
					{
						if (parameters.damageType == EExplosionDamageType.ZOMBIE_FIRE)
						{
							if (zombie.speciality == EZombieSpeciality.NORMAL)
							{
								ZombieManager.sendZombieSpeciality(zombie, EZombieSpeciality.BURNER);
							}
						}
						else
						{
							Vector3 a7 = CollisionUtil.ClosestPoint(zombie.gameObject, parameters.point, false) - parameters.point;
							float sqrMagnitude6 = a7.sqrMagnitude;
							if (sqrMagnitude6 < num)
							{
								float num10 = Mathf.Sqrt(sqrMagnitude6);
								Vector3 vector2 = a7 / num10;
								if (num10 > 0.01f)
								{
									Ray ray6 = new Ray(parameters.point, vector2);
									float maxDistance6 = num10 - 0.01f;
									RaycastHit raycastHit;
									Physics.Raycast(ray6, out raycastHit, maxDistance6, layerMask, QueryTriggerInteraction.Ignore);
									if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(zombie.transform))
									{
										goto IL_B93;
									}
								}
								if (parameters.playImpactEffect)
								{
									EffectAsset effectAsset2 = zombie.isRadioactive ? DamageTool.AlienDynamicRef.Find() : DamageTool.FleshDynamicRef.Find();
									if (effectAsset2 != null)
									{
										TriggerEffectParameters parameters3 = new TriggerEffectParameters(effectAsset2)
										{
											relevantDistance = EffectManager.SMALL,
											position = zombie.transform.position + Vector3.up
										};
										EffectManager.triggerEffect(parameters3);
										parameters3.SetDirection(-vector2);
										EffectManager.triggerEffect(parameters3);
									}
								}
								float num11 = 1f - num10 / parameters.damageRadius;
								float zombieExplosionArmor = DamageTool.GetZombieExplosionArmor(zombie);
								num11 *= zombieExplosionArmor;
								EPlayerKill eplayerKill;
								uint num4;
								DamageTool.damage(zombie, vector2, parameters.zombieDamage, num11, out eplayerKill, out num4, EZombieStunOverride.None, parameters.ragdollEffect);
								if (eplayerKill != EPlayerKill.NONE)
								{
									kills.Add(eplayerKill);
								}
							}
						}
					}
					IL_B93:;
				}
			}
			if (parameters.animalDamage > 0.5f)
			{
				DamageTool.animalsInRadius.Clear();
				AnimalManager.getAnimalsInRadius(parameters.point, num, DamageTool.animalsInRadius);
				for (int num12 = 0; num12 < DamageTool.animalsInRadius.Count; num12++)
				{
					Animal animal = DamageTool.animalsInRadius[num12];
					if (!(animal == null) && !animal.isDead)
					{
						Vector3 a8 = CollisionUtil.ClosestPoint(animal.gameObject, parameters.point, false) - parameters.point;
						float sqrMagnitude7 = a8.sqrMagnitude;
						if (sqrMagnitude7 < num)
						{
							float num13 = Mathf.Sqrt(sqrMagnitude7);
							Vector3 vector3 = a8 / num13;
							if (num13 > 0.01f)
							{
								Ray ray7 = new Ray(parameters.point, vector3);
								float maxDistance7 = num13 - 0.01f;
								RaycastHit raycastHit;
								Physics.Raycast(ray7, out raycastHit, maxDistance7, layerMask, QueryTriggerInteraction.Ignore);
								if (raycastHit.transform != null && !raycastHit.transform.IsChildOf(animal.transform))
								{
									goto IL_D44;
								}
							}
							if (parameters.playImpactEffect)
							{
								EffectAsset effectAsset3 = DamageTool.FleshDynamicRef.Find();
								if (effectAsset3 != null)
								{
									TriggerEffectParameters parameters4 = new TriggerEffectParameters(effectAsset3)
									{
										relevantDistance = EffectManager.SMALL,
										position = animal.transform.position + Vector3.up + Vector3.up
									};
									EffectManager.triggerEffect(parameters4);
									parameters4.SetDirection(-vector3);
									EffectManager.triggerEffect(parameters4);
								}
							}
							EPlayerKill eplayerKill;
							uint num4;
							DamageTool.damage(animal, vector3, parameters.animalDamage, 1f - num13 / parameters.damageRadius, out eplayerKill, out num4, parameters.ragdollEffect);
							if (eplayerKill != EPlayerKill.NONE)
							{
								kills.Add(eplayerKill);
							}
						}
					}
					IL_D44:;
				}
			}
			if (parameters.vehicleDamage > 0.5f)
			{
				DamageTool.vehiclesInRadius.Clear();
				VehicleManager.getVehiclesInRadius(parameters.point, num, DamageTool.vehiclesInRadius);
				for (int num14 = 0; num14 < DamageTool.vehiclesInRadius.Count; num14++)
				{
					InteractableVehicle interactableVehicle = DamageTool.vehiclesInRadius[num14];
					if (!(interactableVehicle == null) && !interactableVehicle.isDead && interactableVehicle.asset != null && interactableVehicle.asset.isVulnerableToExplosions)
					{
						Vector3 a9 = interactableVehicle.getClosestPointOnHull(parameters.point) - parameters.point;
						float sqrMagnitude8 = a9.sqrMagnitude;
						if (sqrMagnitude8 < num)
						{
							float num15 = Mathf.Sqrt(sqrMagnitude8);
							Vector3 direction5 = a9 / num15;
							float num16 = 1f - num15 / parameters.damageRadius;
							if (num15 > 0.01f)
							{
								Ray ray8 = new Ray(parameters.point, direction5);
								float maxDistance8 = num15 - 0.01f;
								RaycastHit raycastHit;
								Physics.Raycast(ray8, out raycastHit, maxDistance8, layerMask, QueryTriggerInteraction.Ignore);
								if (raycastHit.transform != null)
								{
									if (!raycastHit.transform.IsChildOf(interactableVehicle.transform))
									{
										goto IL_EB4;
									}
									num16 *= interactableVehicle.asset.childExplosionArmorMultiplier;
									num16 *= Provider.modeConfigData.Vehicles.Child_Explosion_Armor_Multiplier;
								}
							}
							VehicleManager.damage(interactableVehicle, parameters.vehicleDamage, num16, false, parameters.killer, parameters.damageOrigin);
						}
					}
					IL_EB4:;
				}
			}
			AlertTool.alert(parameters.point, parameters.alertRadius);
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00126305 File Offset: 0x00124505
		[Obsolete("Physics material enum replaced by string names")]
		public static EPhysicsMaterial getMaterial(Vector3 point, Transform transform, Collider collider)
		{
			return PhysicsTool.GetLegacyMaterialByName(PhysicsTool.GetMaterialName(point, transform, collider));
		}

		/// <summary>
		/// Server spawn impact effect for all players within range.
		/// </summary>
		// Token: 0x06003D4E RID: 15694 RVA: 0x00126314 File Offset: 0x00124514
		[Obsolete("Replaced by separate melee and bullet impact methods")]
		public static void impact(Vector3 point, Vector3 normal, EPhysicsMaterial material, bool forceDynamic)
		{
			DamageTool.impact(point, normal, material, forceDynamic, CSteamID.Nil, point);
		}

		/// <summary>
		/// Server spawn impact effect for all players within range. Optional "spectator" receives effect regardless of distance.
		/// </summary>
		// Token: 0x06003D4F RID: 15695 RVA: 0x00126328 File Offset: 0x00124528
		[Obsolete("Replaced by separate melee and bullet impact methods")]
		public static void impact(Vector3 point, Vector3 normal, EPhysicsMaterial material, bool forceDynamic, CSteamID spectatorID, Vector3 spectatorPoint)
		{
			if (material == EPhysicsMaterial.NONE)
			{
				return;
			}
			ushort id = 0;
			if (material == EPhysicsMaterial.CLOTH_DYNAMIC || material == EPhysicsMaterial.TILE_DYNAMIC || material == EPhysicsMaterial.CONCRETE_DYNAMIC)
			{
				id = 38;
			}
			else if (material == EPhysicsMaterial.CLOTH_STATIC || material == EPhysicsMaterial.TILE_STATIC || material == EPhysicsMaterial.CONCRETE_STATIC)
			{
				id = (forceDynamic ? 38 : 13);
			}
			else if (material == EPhysicsMaterial.FLESH_DYNAMIC)
			{
				id = 5;
			}
			else if (material == EPhysicsMaterial.GRAVEL_DYNAMIC)
			{
				id = 44;
			}
			else if (material == EPhysicsMaterial.GRAVEL_STATIC || material == EPhysicsMaterial.SAND_STATIC)
			{
				id = (forceDynamic ? 44 : 14);
			}
			else if (material == EPhysicsMaterial.METAL_DYNAMIC)
			{
				id = 18;
			}
			else if (material == EPhysicsMaterial.METAL_STATIC || material == EPhysicsMaterial.METAL_SLIP)
			{
				id = (forceDynamic ? 18 : 12);
			}
			else if (material == EPhysicsMaterial.WOOD_DYNAMIC)
			{
				id = 17;
			}
			else if (material == EPhysicsMaterial.WOOD_STATIC)
			{
				id = (forceDynamic ? 17 : 2);
			}
			else if (material == EPhysicsMaterial.FOLIAGE_STATIC || material == EPhysicsMaterial.FOLIAGE_DYNAMIC)
			{
				id = 15;
			}
			else if (material == EPhysicsMaterial.SNOW_STATIC || material == EPhysicsMaterial.ICE_STATIC)
			{
				id = 41;
			}
			else if (material == EPhysicsMaterial.WATER_STATIC)
			{
				id = 16;
			}
			else if (material == EPhysicsMaterial.ALIEN_DYNAMIC)
			{
				id = 95;
			}
			DamageTool.impact(point, normal, id, spectatorID, spectatorPoint);
		}

		/// <summary>
		/// Server spawn effect by ID for all players within range. Optional "spectator" receives effect regardless of distance.
		/// </summary>
		// Token: 0x06003D50 RID: 15696 RVA: 0x0012640D File Offset: 0x0012460D
		[Obsolete("Replaced by ServerTriggerImpactEffectForMagazinesV2")]
		public static void impact(Vector3 point, Vector3 normal, ushort id, CSteamID spectatorID, Vector3 spectatorPoint)
		{
			if (id == 0)
			{
				return;
			}
			DamageTool.ServerTriggerImpactEffectForMagazinesV2(Assets.find(EAssetType.EFFECT, id) as EffectAsset, point, normal, PlayerTool.getSteamPlayer(spectatorID));
		}

		/// <summary>
		/// Server spawn effect for all players within range and instigator receives effect regardless of distance.
		/// </summary>
		// Token: 0x06003D51 RID: 15697 RVA: 0x0012642C File Offset: 0x0012462C
		public static void ServerTriggerImpactEffectForMagazinesV2(EffectAsset asset, Vector3 position, Vector3 normal, SteamPlayer instigatingClient)
		{
			if (asset == null)
			{
				return;
			}
			position += normal * Random.Range(0.04f, 0.06f);
			TriggerEffectParameters parameters = new TriggerEffectParameters(asset);
			parameters.position = position;
			parameters.SetDirection(normal);
			parameters.relevantDistance = EffectManager.SMALL;
			if (instigatingClient != null && instigatingClient.player != null && instigatingClient.player.channel != null)
			{
				parameters.SetRelevantTransportConnections(instigatingClient.player.channel.GatherOwnerAndClientConnectionsWithinSphere(position, EffectManager.SMALL));
			}
			EffectManager.triggerEffect(parameters);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x001264C5 File Offset: 0x001246C5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveSpawnBulletImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform, NetId instigatorNetId)
		{
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x001264C8 File Offset: 0x001246C8
		internal static void ServerSpawnBulletImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform, SteamPlayer instigatingClient, List<ITransportConnection> transportConnections)
		{
			position += normal * Random.Range(0.04f, 0.06f);
			NetId arg = (instigatingClient != null) ? instigatingClient.GetNetId() : NetId.INVALID;
			DamageTool.SendSpawnBulletImpact.Invoke(ENetReliability.Unreliable, transportConnections, position, normal, materialName, colliderTransform, arg);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00126517 File Offset: 0x00124717
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveSpawnLegacyImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform)
		{
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00126519 File Offset: 0x00124719
		internal static void ServerSpawnLegacyImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform, List<ITransportConnection> transportConnections)
		{
			position += normal * Random.Range(0.04f, 0.06f);
			DamageTool.SendSpawnLegacyImpact.Invoke(ENetReliability.Unreliable, transportConnections, position, normal, materialName, colliderTransform);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x00126549 File Offset: 0x00124749
		public static RaycastInfo raycast(Ray ray, float range, int mask)
		{
			return DamageTool.raycast(ray, range, mask, null);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00126554 File Offset: 0x00124754
		public static RaycastInfo raycast(Ray ray, float range, int mask, Player ignorePlayer = null)
		{
			RaycastHit hit;
			Physics.Raycast(ray, out hit, range, mask);
			RaycastInfo raycastInfo = new RaycastInfo(hit);
			raycastInfo.direction = ray.direction;
			raycastInfo.limb = ELimb.SPINE;
			if (raycastInfo.transform != null)
			{
				if (raycastInfo.transform.CompareTag("Barricade"))
				{
					raycastInfo.transform = DamageTool.getBarricadeRootTransform(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Structure"))
				{
					raycastInfo.transform = DamageTool.getStructureRootTransform(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Resource"))
				{
					raycastInfo.transform = DamageTool.getResourceRootTransform(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Enemy"))
				{
					raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
					if (raycastInfo.player == ignorePlayer)
					{
						raycastInfo.player = null;
					}
					raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Zombie"))
				{
					raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
					raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Animal"))
				{
					raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);
					raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
				}
				else if (raycastInfo.transform.CompareTag("Vehicle"))
				{
					raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);
				}
				if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
				{
					raycastInfo.materialName = "Alien_Dynamic";
					raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
				}
				else
				{
					raycastInfo.materialName = PhysicsTool.GetMaterialName(hit);
					raycastInfo.material = PhysicsTool.GetLegacyMaterialByName(raycastInfo.materialName);
				}
			}
			return raycastInfo;
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06003D58 RID: 15704 RVA: 0x0012673C File Offset: 0x0012493C
		// (remove) Token: 0x06003D59 RID: 15705 RVA: 0x00126770 File Offset: 0x00124970
		public static event DamageTool.PlayerAllowedToDamagePlayerHandler onPlayerAllowedToDamagePlayer;

		// Token: 0x06003D5A RID: 15706 RVA: 0x001267A4 File Offset: 0x001249A4
		public static bool isPlayerAllowedToDamagePlayer(Player instigator, Player victim)
		{
			bool result = Provider.isPvP && (Provider.modeConfigData.Gameplay.Friendly_Fire || !instigator.quests.isMemberOfSameGroupAs(victim));
			if (!instigator.movement.canAddSimulationResultsToUpdates)
			{
				result = false;
			}
			if (DamageTool.onPlayerAllowedToDamagePlayer != null)
			{
				try
				{
					DamageTool.onPlayerAllowedToDamagePlayer(instigator, victim, ref result);
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Plugin raised an exception from onPlayerAllowedToDamagePlayer:");
					UnturnedLog.exception(e);
				}
			}
			return result;
		}

		// Token: 0x04002676 RID: 9846
		[Obsolete("Use damagePlayerRequested")]
		public static DamageToolPlayerDamagedHandler playerDamaged;

		// Token: 0x04002677 RID: 9847
		[Obsolete("Use damageZombieRequested")]
		public static DamageToolZombieDamagedHandler zombieDamaged;

		// Token: 0x04002678 RID: 9848
		[Obsolete("Use damageAnimalRequested")]
		public static DamageToolAnimalDamagedHandler animalDamaged;

		// Token: 0x0400267C RID: 9852
		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		// Token: 0x0400267D RID: 9853
		private static List<Player> playersInRadius = new List<Player>();

		// Token: 0x0400267E RID: 9854
		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		// Token: 0x0400267F RID: 9855
		private static List<Animal> animalsInRadius = new List<Animal>();

		// Token: 0x04002680 RID: 9856
		private static List<Transform> barricadesInRadius = new List<Transform>();

		// Token: 0x04002681 RID: 9857
		private static List<Transform> structuresInRadius = new List<Transform>();

		// Token: 0x04002682 RID: 9858
		private static List<InteractableVehicle> vehiclesInRadius = new List<InteractableVehicle>();

		// Token: 0x04002683 RID: 9859
		private static List<Transform> resourcesInRadius = new List<Transform>();

		// Token: 0x04002684 RID: 9860
		private static List<Transform> objectsInRadius = new List<Transform>();

		// Token: 0x04002685 RID: 9861
		private static ExplosionRangeComparator explosionRangeComparator = new ExplosionRangeComparator();

		// Token: 0x04002686 RID: 9862
		private static List<EPlayerKill> explosionKills = new List<EPlayerKill>();

		// Token: 0x04002687 RID: 9863
		private static ClientStaticMethod<Vector3, Vector3, string, Transform, NetId> SendSpawnBulletImpact = ClientStaticMethod<Vector3, Vector3, string, Transform, NetId>.Get(new ClientStaticMethod<Vector3, Vector3, string, Transform, NetId>.ReceiveDelegate(DamageTool.ReceiveSpawnBulletImpact));

		// Token: 0x04002688 RID: 9864
		private static ClientStaticMethod<Vector3, Vector3, string, Transform> SendSpawnLegacyImpact = ClientStaticMethod<Vector3, Vector3, string, Transform>.Get(new ClientStaticMethod<Vector3, Vector3, string, Transform>.ReceiveDelegate(DamageTool.ReceiveSpawnLegacyImpact));

		// Token: 0x0400268A RID: 9866
		private static readonly AssetReference<EffectAsset> FleshDynamicRef = new AssetReference<EffectAsset>("cea791255ba74b43a20e511a52ebcbec");

		// Token: 0x0400268B RID: 9867
		private static readonly AssetReference<EffectAsset> AlienDynamicRef = new AssetReference<EffectAsset>("67a4addd45174d7e9ca5c8ec24f8010f");

		// Token: 0x020009F9 RID: 2553
		// (Invoke) Token: 0x06004D1C RID: 19740
		public delegate void DamagePlayerHandler(ref DamagePlayerParameters parameters, ref bool shouldAllow);

		// Token: 0x020009FA RID: 2554
		// (Invoke) Token: 0x06004D20 RID: 19744
		public delegate void DamageZombieHandler(ref DamageZombieParameters parameters, ref bool shouldAllow);

		// Token: 0x020009FB RID: 2555
		// (Invoke) Token: 0x06004D24 RID: 19748
		public delegate void DamageAnimalHandler(ref DamageAnimalParameters parameters, ref bool shouldAllow);

		// Token: 0x020009FC RID: 2556
		// (Invoke) Token: 0x06004D28 RID: 19752
		public delegate void PlayerAllowedToDamagePlayerHandler(Player instigator, Player victim, ref bool isAllowed);
	}
}
