using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000443 RID: 1091
	public class Bumper : MonoBehaviour
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x0007E53E File Offset: 0x0007C73E
		public void init(InteractableVehicle newVehicle)
		{
			this.vehicle = newVehicle;
		}

		/// <summary>
		/// Get SteamID of vehicle's driver, or nil if not driven.
		/// </summary>
		// Token: 0x060020CF RID: 8399 RVA: 0x0007E547 File Offset: 0x0007C747
		protected CSteamID getInstigatorSteamID()
		{
			if (this.vehicle && this.vehicle.isDriven)
			{
				return this.vehicle.passengers[0].player.playerID.steamID;
			}
			return CSteamID.Nil;
		}

		/// <summary>
		/// Crashed into something, if applicable take self damage from collision.
		/// </summary>
		// Token: 0x060020D0 RID: 8400 RVA: 0x0007E588 File Offset: 0x0007C788
		protected void takeCrashDamage(float damage, bool canRepair = true)
		{
			if (this.vehicle == null || this.vehicle.asset == null || !this.vehicle.asset.isVulnerableToBumper)
			{
				return;
			}
			EPlayerKill eplayerKill;
			DamageTool.damage(this.vehicle, false, base.transform.position, false, damage, 1f, canRepair, out eplayerKill, this.getInstigatorSteamID(), EDamageOrigin.Vehicle_Collision_Self_Damage);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x0007E5EC File Offset: 0x0007C7EC
		private void OnTriggerEnter(Collider other)
		{
			if (other == null)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (this.vehicle == null || this.vehicle.asset == null)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform.IsChildOf(this.vehicle.transform))
			{
				return;
			}
			if (DamageTool.getVehicle(other.transform) != null)
			{
				return;
			}
			if (other.CompareTag("Debris"))
			{
				return;
			}
			float num = Mathf.Clamp(this.vehicle.ReplicatedSpeed * this.vehicle.asset.bumperMultiplier, -10f, 10f);
			if (this.reverse)
			{
				num = -num;
			}
			if (num < 3f)
			{
				return;
			}
			if (other.transform.CompareTag("Player"))
			{
				if (this.vehicle.isDriven)
				{
					Player player = this.vehicle.passengers[0].player.player;
					Player player2 = DamageTool.getPlayer(other.transform);
					if (player != null && player2 != null && player2.movement.getVehicle() == null && DamageTool.isPlayerAllowedToDamagePlayer(player, player2))
					{
						EPlayerKill eplayerKill;
						DamageTool.damage(player2, EDeathCause.ROADKILL, ELimb.SPINE, this.vehicle.passengers[0].player.playerID.steamID, base.transform.forward, this.instakill ? 101f : Bumper.DAMAGE_PLAYER, num, out eplayerKill, true, true, ERagdollEffect.NONE);
						DamageTool.ServerSpawnLegacyImpact(other.transform.position + other.transform.up, -base.transform.forward, "Flesh", null, Provider.GatherClientConnectionsWithinSphere(other.transform.position, EffectManager.SMALL));
						this.takeCrashDamage(2f, true);
						return;
					}
				}
			}
			else if (other.transform.CompareTag("Agent"))
			{
				Zombie zombie = DamageTool.getZombie(other.transform);
				if (zombie != null)
				{
					EPlayerKill eplayerKill2;
					uint num2;
					DamageTool.damageZombie(new DamageZombieParameters(zombie, base.transform.forward, this.instakill ? 65000f : Bumper.DAMAGE_ZOMBIE)
					{
						times = num,
						instigator = this
					}, out eplayerKill2, out num2);
					DamageTool.ServerSpawnLegacyImpact(other.transform.position + other.transform.up, -base.transform.forward, zombie.isRadioactive ? "Alien" : "Flesh", null, Provider.GatherClientConnectionsWithinSphere(other.transform.position, EffectManager.SMALL));
					this.takeCrashDamage(2f, true);
					return;
				}
				Animal animal = DamageTool.getAnimal(other.transform);
				if (animal != null)
				{
					EPlayerKill eplayerKill3;
					uint num3;
					DamageTool.damageAnimal(new DamageAnimalParameters(animal, base.transform.forward, this.instakill ? 65000f : Bumper.DAMAGE_ANIMAL)
					{
						times = num,
						instigator = this
					}, out eplayerKill3, out num3);
					DamageTool.ServerSpawnLegacyImpact(other.transform.position + other.transform.up, -base.transform.forward, "Flesh", null, Provider.GatherClientConnectionsWithinSphere(other.transform.position, EffectManager.SMALL));
					this.takeCrashDamage(2f, true);
					return;
				}
			}
			else
			{
				bool flag = false;
				if (other.transform.CompareTag("Barricade"))
				{
					if (this.instakill)
					{
						Transform barricadeRootTransform = DamageTool.getBarricadeRootTransform(other.transform);
						if (barricadeRootTransform.parent == null || !barricadeRootTransform.parent.CompareTag("Vehicle"))
						{
							flag = true;
							BarricadeManager.damage(barricadeRootTransform, 65000f, num, false, this.getInstigatorSteamID(), EDamageOrigin.Vehicle_Bumper);
							this.takeCrashDamage(Bumper.DAMAGE_VEHICLE * num, true);
						}
					}
				}
				else if (other.transform.CompareTag("Structure"))
				{
					if (this.instakill)
					{
						StructureManager.damage(DamageTool.getStructureRootTransform(other.transform), base.transform.forward, 65000f, num, false, this.getInstigatorSteamID(), EDamageOrigin.Vehicle_Bumper);
						flag = true;
						this.takeCrashDamage(Bumper.DAMAGE_VEHICLE * num, true);
					}
				}
				else if (other.transform.CompareTag("Resource"))
				{
					Transform resourceRootTransform = DamageTool.getResourceRootTransform(other.transform);
					flag = true;
					EPlayerKill eplayerKill4;
					uint num4;
					ResourceManager.damage(resourceRootTransform, base.transform.forward, this.instakill ? 65000f : Bumper.DAMAGE_RESOURCE, num, 1f, out eplayerKill4, out num4, this.getInstigatorSteamID(), EDamageOrigin.Vehicle_Bumper, true);
					this.takeCrashDamage(Bumper.DAMAGE_VEHICLE * num, true);
				}
				else
				{
					InteractableObjectRubble componentInParent = other.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent != null)
					{
						EPlayerKill eplayerKill5;
						uint num5;
						DamageTool.damage(componentInParent.transform, base.transform.forward, componentInParent.getSection(other.transform), this.instakill ? 65000f : Bumper.DAMAGE_OBJECT, num, out eplayerKill5, out num5, this.getInstigatorSteamID(), EDamageOrigin.Vehicle_Bumper);
						if (Time.realtimeSinceStartup - this.lastDamageImpact > 0.2f)
						{
							this.lastDamageImpact = Time.realtimeSinceStartup;
							flag = true;
							this.takeCrashDamage(Bumper.DAMAGE_VEHICLE * num, true);
						}
					}
					else if (Time.realtimeSinceStartup - this.lastDamageImpact > 0.2f)
					{
						ObjectAsset asset = LevelObjects.getAsset(other.transform);
						if (asset != null && !asset.isSoft)
						{
							this.lastDamageImpact = Time.realtimeSinceStartup;
							flag = true;
							this.takeCrashDamage(Bumper.DAMAGE_VEHICLE * num, true);
						}
					}
				}
				if (flag)
				{
					Vector3 vector = base.transform.position;
					BoxCollider component = base.transform.GetComponent<BoxCollider>();
					if (component != null)
					{
						vector += base.transform.forward * component.size.z * 0.5f;
					}
					string materialName = PhysicsTool.GetMaterialName(vector, other.transform, other);
					if (!string.IsNullOrEmpty(materialName))
					{
						DamageTool.ServerSpawnLegacyImpact(vector, -base.transform.forward, materialName, null, Provider.GatherClientConnectionsWithinSphere(vector, EffectManager.SMALL));
					}
				}
				if (!this.vehicle.isDead && this.vehicle.asset.isVulnerableToBumper && !other.transform.CompareTag("Border") && ((this.vehicle.asset.engine == EEngine.PLANE && this.vehicle.ReplicatedSpeed > 20f) || (this.vehicle.asset.engine == EEngine.HELICOPTER && this.vehicle.ReplicatedSpeed > 10f)))
				{
					this.takeCrashDamage(20000f, false);
				}
			}
		}

		// Token: 0x0400100A RID: 4106
		public bool reverse;

		// Token: 0x0400100B RID: 4107
		public bool instakill;

		// Token: 0x0400100C RID: 4108
		private static readonly float DAMAGE_PLAYER = 10f;

		// Token: 0x0400100D RID: 4109
		private static readonly float DAMAGE_ZOMBIE = 15f;

		// Token: 0x0400100E RID: 4110
		private static readonly float DAMAGE_ANIMAL = 15f;

		// Token: 0x0400100F RID: 4111
		private static readonly float DAMAGE_OBJECT = 30f;

		// Token: 0x04001010 RID: 4112
		private static readonly float DAMAGE_VEHICLE = 8f;

		// Token: 0x04001011 RID: 4113
		private static readonly float DAMAGE_RESOURCE = 85f;

		// Token: 0x04001012 RID: 4114
		private InteractableVehicle vehicle;

		// Token: 0x04001013 RID: 4115
		private float lastDamageImpact;
	}
}
