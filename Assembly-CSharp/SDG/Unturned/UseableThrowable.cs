using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007EC RID: 2028
	public class UseableThrowable : Useable
	{
		/// <summary>
		/// Plugin-only event when throwable is spawned on server.
		/// </summary>
		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x060045BB RID: 17851 RVA: 0x001A0D20 File Offset: 0x0019EF20
		// (remove) Token: 0x060045BC RID: 17852 RVA: 0x001A0D54 File Offset: 0x0019EF54
		public static event UseableThrowable.ThrowableSpawnedHandler onThrowableSpawned;

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x001A0D87 File Offset: 0x0019EF87
		public ItemThrowableAsset equippedThrowableAsset
		{
			get
			{
				return base.player.equipment.asset as ItemThrowableAsset;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x001A0D9E File Offset: 0x0019EF9E
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x001A0DB4 File Offset: 0x0019EFB4
		private bool isThrowable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.6f;
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x001A0DD0 File Offset: 0x0019EFD0
		private void toss(Vector3 origin, Vector3 force)
		{
			Transform transform = Object.Instantiate<GameObject>(this.equippedThrowableAsset.throwable).transform;
			transform.name = "Throwable";
			EffectManager.RegisterDebris(transform.gameObject);
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(force);
			Rigidbody component = transform.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.AddForce(force);
				component.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
			if (this.equippedThrowableAsset.isExplosive)
			{
				if (Provider.isServer)
				{
					Grenade grenade = transform.gameObject.AddComponent<Grenade>();
					grenade.killer = base.channel.owner.playerID.steamID;
					grenade.range = this.equippedThrowableAsset.range;
					grenade.playerDamage = this.equippedThrowableAsset.playerDamageMultiplier.damage;
					grenade.zombieDamage = this.equippedThrowableAsset.zombieDamageMultiplier.damage;
					grenade.animalDamage = this.equippedThrowableAsset.animalDamageMultiplier.damage;
					grenade.barricadeDamage = this.equippedThrowableAsset.barricadeDamage;
					grenade.structureDamage = this.equippedThrowableAsset.structureDamage;
					grenade.vehicleDamage = this.equippedThrowableAsset.vehicleDamage;
					grenade.resourceDamage = this.equippedThrowableAsset.resourceDamage;
					grenade.objectDamage = this.equippedThrowableAsset.objectDamage;
					grenade.explosionEffectGuid = this.equippedThrowableAsset.explosionEffectGuid;
					grenade.explosion = this.equippedThrowableAsset.explosion;
					grenade.fuseLength = this.equippedThrowableAsset.fuseLength;
					grenade.explosionLaunchSpeed = this.equippedThrowableAsset.explosionLaunchSpeed;
				}
				else
				{
					Object.Destroy(transform.gameObject, this.equippedThrowableAsset.fuseLength);
				}
			}
			else if (this.equippedThrowableAsset.isFlash)
			{
				Object.Destroy(transform.gameObject, this.equippedThrowableAsset.fuseLength);
			}
			else
			{
				transform.gameObject.AddComponent<Distraction>();
				Object.Destroy(transform.gameObject, this.equippedThrowableAsset.fuseLength);
			}
			if (this.equippedThrowableAsset.isSticky)
			{
				transform.gameObject.AddComponent<StickyGrenade>().ignoreTransform = base.transform;
			}
			if (this.equippedThrowableAsset.explodeOnImpact && Provider.isServer)
			{
				transform.gameObject.SetLayerRecursively(30);
				ImpactGrenade impactGrenade = transform.gameObject.AddComponent<ImpactGrenade>();
				impactGrenade.explodable = transform.GetComponent<IExplodableThrowable>();
				impactGrenade.ignoreTransform = base.transform;
			}
			Transform transform2 = transform.Find("Smoke");
			if (transform2 != null)
			{
				Object.Destroy(transform2.gameObject);
			}
			UseableThrowable.ThrowableSpawnedHandler throwableSpawnedHandler = UseableThrowable.onThrowableSpawned;
			if (throwableSpawnedHandler == null)
			{
				return;
			}
			throwableSpawnedHandler(this, transform.gameObject);
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x001A1062 File Offset: 0x0019F262
		private void swing()
		{
			this.isSwinging = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x001A109D File Offset: 0x0019F29D
		[Obsolete]
		public void askToss(CSteamID steamID, Vector3 origin, Vector3 force)
		{
			this.ReceiveToss(origin, force);
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x001A10A7 File Offset: 0x0019F2A7
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askToss")]
		public void ReceiveToss(Vector3 origin, Vector3 force)
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.toss(origin, force);
			}
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x001A10C3 File Offset: 0x0019F2C3
		[Obsolete]
		public void askSwing(CSteamID steamID)
		{
			this.ReceivePlaySwing();
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x001A10CB File Offset: 0x0019F2CB
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askSwing")]
		public void ReceivePlaySwing()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.swing();
			}
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x001A10E8 File Offset: 0x0019F2E8
		protected bool startAttack(ESwingMode newSwingMode)
		{
			if (base.player.equipment.isBusy || base.player.quests.IsCutsceneModeActive())
			{
				return false;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.swingMode = newSwingMode;
			this.swing();
			if (Provider.isServer)
			{
				if (this.equippedThrowableAsset.isExplosive)
				{
					base.player.life.markAggressive(false, true);
				}
				UseableThrowable.SendPlaySwing.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
			}
			return true;
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x001A118E File Offset: 0x0019F38E
		public override bool startPrimary()
		{
			return this.startAttack(ESwingMode.STRONG);
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x001A1197 File Offset: 0x0019F397
		public override bool startSecondary()
		{
			return this.startAttack(ESwingMode.WEAK);
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x001A11A0 File Offset: 0x0019F3A0
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x001A11D4 File Offset: 0x0019F3D4
		public override void tick()
		{
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if ((base.channel.IsLocalPlayer || Provider.isServer) && this.isSwinging && this.isThrowable)
			{
				Vector3 vector = base.player.look.aim.position;
				Vector3 forward = base.player.look.aim.forward;
				RaycastHit raycastHit;
				if (Physics.Raycast(new Ray(vector, forward), out raycastHit, 1.5f, RayMasks.DAMAGE_SERVER))
				{
					vector += forward * (raycastHit.distance - 0.5f);
				}
				else
				{
					vector += forward;
				}
				ESwingMode eswingMode = this.swingMode;
				float num;
				if (eswingMode != ESwingMode.WEAK && eswingMode == ESwingMode.STRONG)
				{
					num = this.equippedThrowableAsset.strongThrowForce;
				}
				else
				{
					num = this.equippedThrowableAsset.weakThrowForce;
				}
				if (base.player.skills.boost == EPlayerBoost.OLYMPIC)
				{
					num *= this.equippedThrowableAsset.boostForceMultiplier;
				}
				Vector3 vector2 = forward * num;
				this.toss(vector, vector2);
				if (base.channel.IsLocalPlayer)
				{
					int num2;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Throwables", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Throwables", num2 + 1);
					}
				}
				else
				{
					UseableThrowable.SendToss.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), vector, vector2);
				}
				if (Provider.isServer)
				{
					base.player.equipment.useStepA();
				}
				this.isSwinging = false;
			}
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x001A1370 File Offset: 0x0019F570
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					base.player.equipment.useStepB();
				}
			}
		}

		// Token: 0x04002F10 RID: 12048
		private float startedUse;

		// Token: 0x04002F11 RID: 12049
		private float useTime;

		// Token: 0x04002F12 RID: 12050
		private bool isUsing;

		// Token: 0x04002F13 RID: 12051
		private bool isSwinging;

		// Token: 0x04002F14 RID: 12052
		private ESwingMode swingMode;

		// Token: 0x04002F15 RID: 12053
		private static readonly ClientInstanceMethod<Vector3, Vector3> SendToss = ClientInstanceMethod<Vector3, Vector3>.Get(typeof(UseableThrowable), "ReceiveToss");

		// Token: 0x04002F16 RID: 12054
		private static readonly ClientInstanceMethod SendPlaySwing = ClientInstanceMethod.Get(typeof(UseableThrowable), "ReceivePlaySwing");

		// Token: 0x02000A1C RID: 2588
		// (Invoke) Token: 0x06004D85 RID: 19845
		public delegate void ThrowableSpawnedHandler(UseableThrowable useable, GameObject throwable);
	}
}
