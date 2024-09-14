using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200063C RID: 1596
	public class PlayerLife : PlayerCaller
	{
		/// <summary>
		/// Invoked prior to built-in death logic.
		/// </summary>
		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06003397 RID: 13207 RVA: 0x000EA418 File Offset: 0x000E8618
		// (remove) Token: 0x06003398 RID: 13208 RVA: 0x000EA44C File Offset: 0x000E864C
		public static event Action<PlayerLife> OnPreDeath;

		/// <summary>
		/// Event for plugins when player dies.
		/// </summary>
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06003399 RID: 13209 RVA: 0x000EA480 File Offset: 0x000E8680
		// (remove) Token: 0x0600339A RID: 13210 RVA: 0x000EA4B4 File Offset: 0x000E86B4
		public static event PlayerLife.PlayerDiedCallback onPlayerDied;

		// Token: 0x0600339B RID: 13211 RVA: 0x000EA4E8 File Offset: 0x000E86E8
		private static void broadcastPlayerDied(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
		{
			try
			{
				PlayerLife.PlayerDiedCallback playerDiedCallback = PlayerLife.onPlayerDied;
				if (playerDiedCallback != null)
				{
					playerDiedCallback(sender, cause, limb, instigator);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Plugin raised an exception from onPlayerDied:");
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x0600339C RID: 13212 RVA: 0x000EA52C File Offset: 0x000E872C
		// (remove) Token: 0x0600339D RID: 13213 RVA: 0x000EA560 File Offset: 0x000E8760
		public static event PlayerLife.RespawnPointSelector OnSelectingRespawnPoint;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x0600339E RID: 13214 RVA: 0x000EA594 File Offset: 0x000E8794
		// (remove) Token: 0x0600339F RID: 13215 RVA: 0x000EA5CC File Offset: 0x000E87CC
		public event Hurt onHurt;

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x000EA601 File Offset: 0x000E8801
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x000EA609 File Offset: 0x000E8809
		public bool wasPvPDeath { get; private set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x000EA612 File Offset: 0x000E8812
		public static EDeathCause deathCause
		{
			get
			{
				return PlayerLife._deathCause;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x000EA619 File Offset: 0x000E8819
		public static ELimb deathLimb
		{
			get
			{
				return PlayerLife._deathLimb;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x000EA620 File Offset: 0x000E8820
		public static CSteamID deathKiller
		{
			get
			{
				return PlayerLife._deathKiller;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060033A5 RID: 13221 RVA: 0x000EA627 File Offset: 0x000E8827
		public bool isAggressor
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN;
			}
		}

		/// <summary>
		/// Tracks this player as an aggressor if they were recently an aggressor or if they haven't been attacked recently.
		/// </summary>
		/// <param name="force">Ignores rules and just make aggressive.</param>
		/// <param name="spreadToGroup">Whether to call markAggressive on group members.</param>
		// Token: 0x060033A6 RID: 13222 RVA: 0x000EA63C File Offset: 0x000E883C
		public void markAggressive(bool force, bool spreadToGroup = true)
		{
			if (force || Time.realtimeSinceStartup - this.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN)
			{
				this.lastTimeAggressive = Time.realtimeSinceStartup;
			}
			else if (this.recentKiller == CSteamID.Nil || Time.realtimeSinceStartup - this.lastTimeTookDamage > PlayerLife.COMBAT_COOLDOWN)
			{
				this.lastTimeAggressive = Time.realtimeSinceStartup;
			}
			if (spreadToGroup && base.player.quests.isMemberOfAGroup)
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != base.channel.owner.playerID.steamID && base.player.quests.isMemberOfSameGroupAs(Provider.clients[i].player) && Provider.clients[i].player != null)
					{
						Provider.clients[i].player.life.markAggressive(force, false);
					}
				}
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060033A7 RID: 13223 RVA: 0x000EA759 File Offset: 0x000E8959
		public bool isDead
		{
			get
			{
				return this._isDead;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x000EA761 File Offset: 0x000E8961
		public bool IsAlive
		{
			get
			{
				return !this._isDead;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x000EA76C File Offset: 0x000E896C
		public byte health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x000EA774 File Offset: 0x000E8974
		public byte food
		{
			get
			{
				return this._food;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060033AB RID: 13227 RVA: 0x000EA77C File Offset: 0x000E897C
		public byte water
		{
			get
			{
				return this._water;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x000EA784 File Offset: 0x000E8984
		public byte virus
		{
			get
			{
				return this._virus;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060033AD RID: 13229 RVA: 0x000EA78C File Offset: 0x000E898C
		public byte vision
		{
			get
			{
				return this._vision;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x000EA794 File Offset: 0x000E8994
		public uint warmth
		{
			get
			{
				return this._warmth;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060033AF RID: 13231 RVA: 0x000EA79C File Offset: 0x000E899C
		public byte stamina
		{
			get
			{
				return this._stamina;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x000EA7A4 File Offset: 0x000E89A4
		public byte oxygen
		{
			get
			{
				return this._oxygen;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060033B1 RID: 13233 RVA: 0x000EA7AC File Offset: 0x000E89AC
		public bool isBleeding
		{
			get
			{
				return this._isBleeding;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x000EA7B4 File Offset: 0x000E89B4
		public bool isBroken
		{
			get
			{
				return this._isBroken;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060033B3 RID: 13235 RVA: 0x000EA7BC File Offset: 0x000E89BC
		public EPlayerTemperature temperature
		{
			get
			{
				return this._temperature;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000EA7C4 File Offset: 0x000E89C4
		public float lastRespawn
		{
			get
			{
				return this._lastRespawn;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x000EA7CC File Offset: 0x000E89CC
		public float lastDeath
		{
			get
			{
				return this._lastDeath;
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000EA7D4 File Offset: 0x000E89D4
		[Obsolete]
		public void tellDeath(CSteamID steamID, byte newCause, byte newLimb, CSteamID newKiller)
		{
			this.ReceiveDeath((EDeathCause)newCause, (ELimb)newLimb, newKiller);
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000EA7E0 File Offset: 0x000E89E0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellDeath")]
		public void ReceiveDeath(EDeathCause newCause, ELimb newLimb, CSteamID newKiller)
		{
			PlayerLife._deathCause = newCause;
			PlayerLife._deathLimb = newLimb;
			PlayerLife._deathKiller = newKiller;
			int num;
			if (base.channel.IsLocalPlayer && Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out num))
			{
				Provider.provider.statisticsService.userStatisticsService.setStatistic("Deaths_Players", num + 1);
			}
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x000EA846 File Offset: 0x000E8A46
		[Obsolete]
		public void tellDead(CSteamID steamID, Vector3 newRagdoll, byte newRagdollEffect)
		{
			this.ReceiveDead(newRagdoll, (ERagdollEffect)newRagdollEffect);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000EA850 File Offset: 0x000E8A50
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellDead")]
		public void ReceiveDead(Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			this._isDead = true;
			this._lastDeath = Time.realtimeSinceStartup;
			this.ragdoll = newRagdoll;
			this.ragdollEffect = newRagdollEffect;
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				base.player.movement.UpdateCharacterControllerEnabled();
			}
			if (this.onLifeUpdated != null)
			{
				this.onLifeUpdated(this.isDead);
			}
			if (PlayerLife.onPlayerLifeUpdated != null)
			{
				PlayerLife.onPlayerLifeUpdated(base.player);
			}
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000EA8D1 File Offset: 0x000E8AD1
		[Obsolete]
		public void tellRevive(CSteamID steamID, Vector3 position, byte angle)
		{
			this.ReceiveRevive(position, angle);
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000EA8DC File Offset: 0x000E8ADC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellRevive")]
		public void ReceiveRevive(Vector3 position, byte angle)
		{
			this._isDead = false;
			this._lastRespawn = Time.realtimeSinceStartup;
			base.player.ReceiveTeleport(position, angle);
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				base.player.movement.UpdateCharacterControllerEnabled();
			}
			LifeUpdated lifeUpdated = this.onLifeUpdated;
			if (lifeUpdated != null)
			{
				lifeUpdated(this.isDead);
			}
			PlayerLifeUpdated playerLifeUpdated = PlayerLife.onPlayerLifeUpdated;
			if (playerLifeUpdated != null)
			{
				playerLifeUpdated(base.player);
			}
			try
			{
				Action<PlayerLife> onRevived_Global = PlayerLife.OnRevived_Global;
				if (onRevived_Global != null)
				{
					onRevived_Global.Invoke(this);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Plugin threw an exception during OnRevived_Global:");
			}
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000EA98C File Offset: 0x000E8B8C
		[Obsolete("Prior to saving/loading oxygen the client assumed it started at 100, but now needs the exact value.")]
		public void tellLife(CSteamID steamID, byte newHealth, byte newFood, byte newWater, byte newVirus, bool newBleeding, bool newBroken)
		{
			this.tellLifeWithOxygen(steamID, newHealth, newFood, newWater, newVirus, 100, newBleeding, newBroken);
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000EA9AC File Offset: 0x000E8BAC
		[Obsolete]
		public void tellLifeWithOxygen(CSteamID steamID, byte newHealth, byte newFood, byte newWater, byte newVirus, byte newOxygen, bool newBleeding, bool newBroken)
		{
			this.ReceiveLifeStats(newHealth, newFood, newWater, newVirus, newOxygen, newBleeding, newBroken);
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000EA9C0 File Offset: 0x000E8BC0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLifeWithOxygen")]
		public void ReceiveLifeStats(byte newHealth, byte newFood, byte newWater, byte newVirus, byte newOxygen, bool newBleeding, bool newBroken)
		{
			Player.isLoadingLife = false;
			this.ReceiveHealth(newHealth);
			this.ReceiveFood(newFood);
			this.ReceiveWater(newWater);
			this.ReceiveVirus(newVirus);
			this.ReceiveBleeding(newBleeding);
			this.ReceiveBroken(newBroken);
			this._stamina = 100;
			this._oxygen = newOxygen;
			this._vision = 0;
			this._warmth = 0U;
			this._temperature = EPlayerTemperature.NONE;
			this.wasWarm = false;
			this.wasCovered = false;
			VisionUpdated visionUpdated = this.onVisionUpdated;
			if (visionUpdated != null)
			{
				visionUpdated(false);
			}
			StaminaUpdated staminaUpdated = this.onStaminaUpdated;
			if (staminaUpdated != null)
			{
				staminaUpdated(this.stamina);
			}
			OxygenUpdated oxygenUpdated = this.onOxygenUpdated;
			if (oxygenUpdated != null)
			{
				oxygenUpdated(this.oxygen);
			}
			TemperatureUpdated temperatureUpdated = this.onTemperatureUpdated;
			if (temperatureUpdated != null)
			{
				temperatureUpdated(this.temperature);
			}
			this.lastAlive = Time.realtimeSinceStartup;
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000EAA95 File Offset: 0x000E8C95
		[Obsolete]
		public void askLife(CSteamID steamID)
		{
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000EAA98 File Offset: 0x000E8C98
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			if (base.channel.owner == client)
			{
				PlayerLife.SendLifeStats.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.health, this.food, this.water, this.virus, this.oxygen, this.isBleeding, this.isBroken);
				return;
			}
			if (this.isDead)
			{
				PlayerLife.SendDead.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.ragdoll, this.ragdollEffect);
			}
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000EAB20 File Offset: 0x000E8D20
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			if (this.isDead)
			{
				PlayerLife.SendDead.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.ragdoll, this.ragdollEffect);
			}
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000EAB48 File Offset: 0x000E8D48
		[Obsolete]
		public void tellHealth(CSteamID steamID, byte newHealth)
		{
			this.ReceiveHealth(newHealth);
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000EAB54 File Offset: 0x000E8D54
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellHealth")]
		public void ReceiveHealth(byte newHealth)
		{
			this._health = newHealth;
			HealthUpdated healthUpdated = this.onHealthUpdated;
			if (healthUpdated != null)
			{
				healthUpdated(this.health);
			}
			if (newHealth < this.lastHealth - 3)
			{
				Damaged damaged = this.onDamaged;
				if (damaged != null)
				{
					damaged(this.lastHealth - newHealth);
				}
			}
			this.lastHealth = newHealth;
			Action<PlayerLife> onTellHealth_Global = PlayerLife.OnTellHealth_Global;
			if (onTellHealth_Global == null)
			{
				return;
			}
			onTellHealth_Global.Invoke(this);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000EABBB File Offset: 0x000E8DBB
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveDamagedEvent(byte damageAmount, Vector3 damageDirection)
		{
			base.player.look.FlinchFromDamage(damageAmount, damageDirection);
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000EABCF File Offset: 0x000E8DCF
		[Obsolete]
		public void tellFood(CSteamID steamID, byte newFood)
		{
			this.ReceiveFood(newFood);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000EABD8 File Offset: 0x000E8DD8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellFood")]
		public void ReceiveFood(byte newFood)
		{
			this._food = newFood;
			FoodUpdated foodUpdated = this.onFoodUpdated;
			if (foodUpdated != null)
			{
				foodUpdated(this.food);
			}
			Action<PlayerLife> onTellFood_Global = PlayerLife.OnTellFood_Global;
			if (onTellFood_Global == null)
			{
				return;
			}
			onTellFood_Global.Invoke(this);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000EAC08 File Offset: 0x000E8E08
		[Obsolete]
		public void tellWater(CSteamID steamID, byte newWater)
		{
			this.ReceiveWater(newWater);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000EAC11 File Offset: 0x000E8E11
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWater")]
		public void ReceiveWater(byte newWater)
		{
			this._water = newWater;
			WaterUpdated waterUpdated = this.onWaterUpdated;
			if (waterUpdated != null)
			{
				waterUpdated(this.water);
			}
			Action<PlayerLife> onTellWater_Global = PlayerLife.OnTellWater_Global;
			if (onTellWater_Global == null)
			{
				return;
			}
			onTellWater_Global.Invoke(this);
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000EAC41 File Offset: 0x000E8E41
		[Obsolete]
		public void tellVirus(CSteamID steamID, byte newVirus)
		{
			this.ReceiveVirus(newVirus);
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000EAC4A File Offset: 0x000E8E4A
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVirus")]
		public void ReceiveVirus(byte newVirus)
		{
			this._virus = newVirus;
			VirusUpdated virusUpdated = this.onVirusUpdated;
			if (virusUpdated != null)
			{
				virusUpdated(this.virus);
			}
			Action<PlayerLife> onTellVirus_Global = PlayerLife.OnTellVirus_Global;
			if (onTellVirus_Global == null)
			{
				return;
			}
			onTellVirus_Global.Invoke(this);
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000EAC7A File Offset: 0x000E8E7A
		[Obsolete]
		public void tellBleeding(CSteamID steamID, bool newBleeding)
		{
			this.ReceiveBleeding(newBleeding);
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000EAC83 File Offset: 0x000E8E83
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellBleeding")]
		public void ReceiveBleeding(bool newBleeding)
		{
			this._isBleeding = newBleeding;
			BleedingUpdated bleedingUpdated = this.onBleedingUpdated;
			if (bleedingUpdated != null)
			{
				bleedingUpdated(this.isBleeding);
			}
			Action<PlayerLife> onTellBleeding_Global = PlayerLife.OnTellBleeding_Global;
			if (onTellBleeding_Global == null)
			{
				return;
			}
			onTellBleeding_Global.Invoke(this);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000EACB3 File Offset: 0x000E8EB3
		[Obsolete]
		public void tellBroken(CSteamID steamID, bool newBroken)
		{
			this.ReceiveBroken(newBroken);
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000EACBC File Offset: 0x000E8EBC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellBroken")]
		public void ReceiveBroken(bool newBroken)
		{
			this._isBroken = newBroken;
			BrokenUpdated brokenUpdated = this.onBrokenUpdated;
			if (brokenUpdated != null)
			{
				brokenUpdated(this.isBroken);
			}
			Action<PlayerLife> onTellBroken_Global = PlayerLife.OnTellBroken_Global;
			if (onTellBroken_Global == null)
			{
				return;
			}
			onTellBroken_Global.Invoke(this);
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000EACEC File Offset: 0x000E8EEC
		public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
		{
			this.askDamage(amount, newRagdoll, newCause, newLimb, newKiller, out kill, false, ERagdollEffect.NONE, true);
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000EAD0C File Offset: 0x000E8F0C
		public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill, bool trackKill = false, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE)
		{
			this.askDamage(amount, newRagdoll, newCause, newLimb, newKiller, out kill, trackKill, newRagdollEffect, true);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000EAD30 File Offset: 0x000E8F30
		public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill, bool trackKill = false, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE, bool canCauseBleeding = true)
		{
			this.askDamage(amount, newRagdoll, newCause, newLimb, newKiller, out kill, trackKill, newRagdollEffect, canCauseBleeding, false);
		}

		/// <param name="bypassSafezone">Should damage be dealt even while inside safezone?</param>
		// Token: 0x060033D2 RID: 13266 RVA: 0x000EAD54 File Offset: 0x000E8F54
		public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill, bool trackKill = false, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE, bool canCauseBleeding = true, bool bypassSafezone = false)
		{
			kill = EPlayerKill.NONE;
			if (!bypassSafezone && !this.InternalCanDamage())
			{
				return;
			}
			this.doDamage(amount, newRagdoll, newCause, newLimb, newKiller, out kill, trackKill, newRagdollEffect, canCauseBleeding);
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000EAD88 File Offset: 0x000E8F88
		internal bool InternalCanDamage()
		{
			return (!base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && (this.lastRespawn <= 0f || Time.realtimeSinceStartup - this.lastRespawn >= 0.5f);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x000EADE4 File Offset: 0x000E8FE4
		private void doDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill, bool trackKill = false, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE, bool canCauseBleeding = true)
		{
			kill = EPlayerKill.NONE;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.health)
				{
					this._health = 0;
				}
				else
				{
					this._health -= amount;
				}
				this.ragdoll = newRagdoll;
				this.ragdollEffect = newRagdollEffect;
				if (this._health > 0 && amount > 3)
				{
					PlayerLife.SendDamagedEvent.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), amount, newRagdoll.normalized);
				}
				PlayerLife.SendHealth.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.health);
				Action<PlayerLife> onTellHealth_Global = PlayerLife.OnTellHealth_Global;
				if (onTellHealth_Global != null)
				{
					onTellHealth_Global.Invoke(this);
				}
				if (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE)
				{
					this.recentKiller = newKiller;
					this.lastTimeTookDamage = Time.realtimeSinceStartup;
					Player player = PlayerTool.getPlayer(this.recentKiller);
					if (player != null)
					{
						player.life.lastTimeCausedDamage = Time.realtimeSinceStartup;
						if (Time.realtimeSinceStartup - player.life.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN)
						{
							player.life.markAggressive(true, true);
						}
						else if ((player.life.recentKiller == CSteamID.Nil || Time.realtimeSinceStartup - player.life.lastTimeTookDamage > PlayerLife.COMBAT_COOLDOWN) && Time.realtimeSinceStartup - this.lastTimeCausedDamage > PlayerLife.COMBAT_COOLDOWN)
						{
							player.life.markAggressive(true, true);
						}
					}
				}
				if (this.health == 0)
				{
					if (this.recentKiller != CSteamID.Nil && this.recentKiller != base.channel.owner.playerID.steamID && Time.realtimeSinceStartup - this.lastTimeTookDamage < PlayerLife.COMBAT_COOLDOWN)
					{
						Player player2 = PlayerTool.getPlayer(this.recentKiller);
						if (player2 != null)
						{
							int num = Mathf.Abs(base.player.skills.reputation);
							num = Mathf.Clamp(num, 1, 25);
							if (player2.life.isAggressor)
							{
								num = -num;
							}
							player2.skills.askRep(num);
						}
					}
					kill = EPlayerKill.PLAYER;
					this.wasPvPDeath = (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE || newCause == EDeathCause.SENTRY);
					PlayerLife.OnPreDeath.TryInvoke("OnPreDeath", this);
					base.player.movement.forceRemoveFromVehicle();
					PlayerLife.RocketLegacyOnDeath.TryInvoke("RocketLegacyOnDeath", this, newCause, newLimb, newKiller);
					try
					{
						PlayerLife.SendDeath.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), newCause, newLimb, newKiller);
						PlayerLife.SendDead.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.ragdoll, this.ragdollEffect);
					}
					catch (Exception e)
					{
						UnturnedLog.warn("Exception during tellDeath or tellDead:");
						UnturnedLog.exception(e);
					}
					if (this.spawnpoint == null || (newCause != EDeathCause.SUICIDE && newCause != EDeathCause.BREATH) || Time.realtimeSinceStartup - this.lastSuicide > 60f)
					{
						this.spawnpoint = LevelPlayers.getSpawn(false);
					}
					if (newCause == EDeathCause.SUICIDE || newCause == EDeathCause.BREATH)
					{
						this.lastSuicide = Time.realtimeSinceStartup;
					}
					if (trackKill)
					{
						for (int i = 0; i < Provider.clients.Count; i++)
						{
							SteamPlayer steamPlayer = Provider.clients[i];
							if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && steamPlayer != base.channel.owner && (steamPlayer.player.transform.position - base.transform.position).sqrMagnitude < 90000f)
							{
								steamPlayer.player.quests.trackPlayerKill(base.player);
							}
						}
					}
					PlayerLife.broadcastPlayerDied(this, newCause, newLimb, newKiller);
					if (CommandWindow.shouldLogDeaths)
					{
						if (newCause == EDeathCause.BLEEDING)
						{
							CommandWindow.Log(Provider.localization.format("Bleeding", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.BONES)
						{
							CommandWindow.Log(Provider.localization.format("Bones", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.FREEZING)
						{
							CommandWindow.Log(Provider.localization.format("Freezing", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.BURNING)
						{
							CommandWindow.Log(Provider.localization.format("Burning", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.FOOD)
						{
							CommandWindow.Log(Provider.localization.format("Food", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.WATER)
						{
							CommandWindow.Log(Provider.localization.format("Water", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE || newCause == EDeathCause.SPLASH)
						{
							SteamPlayer steamPlayer2 = PlayerTool.getSteamPlayer(newKiller);
							string text;
							string text2;
							if (steamPlayer2 != null)
							{
								text = steamPlayer2.playerID.characterName;
								text2 = steamPlayer2.playerID.playerName;
							}
							else
							{
								text = "?";
								text2 = "?";
							}
							string text3 = "";
							if (newLimb == ELimb.LEFT_FOOT || newLimb == ELimb.LEFT_LEG || newLimb == ELimb.RIGHT_FOOT || newLimb == ELimb.RIGHT_LEG)
							{
								text3 = Provider.localization.format("Leg");
							}
							else if (newLimb == ELimb.LEFT_HAND || newLimb == ELimb.LEFT_ARM || newLimb == ELimb.RIGHT_HAND || newLimb == ELimb.RIGHT_ARM)
							{
								text3 = Provider.localization.format("Arm");
							}
							else if (newLimb == ELimb.SPINE)
							{
								text3 = Provider.localization.format("Spine");
							}
							else if (newLimb == ELimb.SKULL)
							{
								text3 = Provider.localization.format("Skull");
							}
							if (newCause == EDeathCause.GUN)
							{
								CommandWindow.Log(Provider.localization.format("Gun", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.MELEE)
							{
								CommandWindow.Log(Provider.localization.format("Melee", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.PUNCH)
							{
								CommandWindow.Log(Provider.localization.format("Punch", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.ROADKILL)
							{
								CommandWindow.Log(Provider.localization.format("Roadkill", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.GRENADE)
							{
								CommandWindow.Log(Provider.localization.format("Grenade", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.MISSILE)
							{
								CommandWindow.Log(Provider.localization.format("Missile", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.CHARGE)
							{
								CommandWindow.Log(Provider.localization.format("Charge", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.SPLASH)
							{
								CommandWindow.Log(Provider.localization.format("Splash", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
						}
						else if (newCause == EDeathCause.ZOMBIE)
						{
							CommandWindow.Log(Provider.localization.format("Zombie", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.ANIMAL)
						{
							CommandWindow.Log(Provider.localization.format("Animal", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.SUICIDE)
						{
							CommandWindow.Log(Provider.localization.format("Suicide", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.INFECTION)
						{
							CommandWindow.Log(Provider.localization.format("Infection", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.BREATH)
						{
							CommandWindow.Log(Provider.localization.format("Breath", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.ZOMBIE)
						{
							CommandWindow.Log(Provider.localization.format("Zombie", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.VEHICLE)
						{
							CommandWindow.Log(Provider.localization.format("Vehicle", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.SHRED)
						{
							CommandWindow.Log(Provider.localization.format("Shred", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.LANDMINE)
						{
							CommandWindow.Log(Provider.localization.format("Landmine", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.ARENA)
						{
							CommandWindow.Log(Provider.localization.format("Arena", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.SENTRY)
						{
							CommandWindow.Log(Provider.localization.format("Sentry", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.ACID)
						{
							CommandWindow.Log(Provider.localization.format("Acid", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.BOULDER)
						{
							CommandWindow.Log(Provider.localization.format("Boulder", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.BURNER)
						{
							CommandWindow.Log(Provider.localization.format("Burner", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.SPIT)
						{
							CommandWindow.Log(Provider.localization.format("Spit", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
						else if (newCause == EDeathCause.SPARK)
						{
							CommandWindow.Log(Provider.localization.format("Spark", base.channel.owner.playerID.characterName, base.channel.owner.playerID.playerName));
						}
					}
				}
				else if (Provider.modeConfigData.Players.Can_Start_Bleeding && canCauseBleeding && amount >= 20)
				{
					this.serverSetBleeding(true);
				}
				Hurt hurt = this.onHurt;
				if (hurt == null)
				{
					return;
				}
				hurt(base.player, amount, newRagdoll, newCause, newLimb, newKiller);
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000EBC5C File Offset: 0x000E9E5C
		public void askHeal(byte amount, bool healBleeding, bool healBroken)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.health)
				{
					this._health = 100;
				}
				else
				{
					this._health += amount;
				}
				PlayerLife.SendHealth.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.health);
				Action<PlayerLife> onTellHealth_Global = PlayerLife.OnTellHealth_Global;
				if (onTellHealth_Global != null)
				{
					onTellHealth_Global.Invoke(this);
				}
				if (this.isBleeding && healBleeding)
				{
					this.serverSetBleeding(false);
				}
				if (this.isBroken && healBroken)
				{
					this.serverSetLegsBroken(false);
				}
			}
		}

		/// <summary>
		/// Set bleeding state and replicate to owner if changed.
		/// </summary>
		// Token: 0x060033D6 RID: 13270 RVA: 0x000EBCF8 File Offset: 0x000E9EF8
		public void serverSetBleeding(bool newBleeding)
		{
			if (newBleeding)
			{
				this.lastBleeding = base.player.input.simulation;
				this.lastBleed = base.player.input.simulation;
			}
			if (this.isBleeding != newBleeding)
			{
				this._isBleeding = newBleeding;
				PlayerLife.SendBleeding.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.isBleeding);
				Action<PlayerLife> onTellBleeding_Global = PlayerLife.OnTellBleeding_Global;
				if (onTellBleeding_Global == null)
				{
					return;
				}
				onTellBleeding_Global.Invoke(this);
			}
		}

		/// <summary>
		/// Set legs broken state and replicate to owner if changed.
		/// </summary>
		// Token: 0x060033D7 RID: 13271 RVA: 0x000EBD78 File Offset: 0x000E9F78
		public void serverSetLegsBroken(bool newLegsBroken)
		{
			if (newLegsBroken)
			{
				this.lastBroken = base.player.input.simulation;
			}
			if (this.isBroken != newLegsBroken)
			{
				this._isBroken = newLegsBroken;
				PlayerLife.SendBroken.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.isBroken);
				Action<PlayerLife> onTellBroken_Global = PlayerLife.OnTellBroken_Global;
				if (onTellBroken_Global == null)
				{
					return;
				}
				onTellBroken_Global.Invoke(this);
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000EBDE0 File Offset: 0x000E9FE0
		public void askStarve(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.food)
				{
					this._food = 0;
				}
				else
				{
					this._food -= amount;
				}
				PlayerLife.SendFood.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.food);
				Action<PlayerLife> onTellFood_Global = PlayerLife.OnTellFood_Global;
				if (onTellFood_Global == null)
				{
					return;
				}
				onTellFood_Global.Invoke(this);
			}
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000EBE54 File Offset: 0x000EA054
		public void askEat(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.food)
				{
					this._food = 100;
				}
				else
				{
					this._food += amount;
				}
				PlayerLife.SendFood.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.food);
				Action<PlayerLife> onTellFood_Global = PlayerLife.OnTellFood_Global;
				if (onTellFood_Global == null)
				{
					return;
				}
				onTellFood_Global.Invoke(this);
			}
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000EBECC File Offset: 0x000EA0CC
		public void askDehydrate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.water)
				{
					this._water = 0;
				}
				else
				{
					this._water -= amount;
				}
				PlayerLife.SendWater.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.water);
				Action<PlayerLife> onTellWater_Global = PlayerLife.OnTellWater_Global;
				if (onTellWater_Global == null)
				{
					return;
				}
				onTellWater_Global.Invoke(this);
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000EBF40 File Offset: 0x000EA140
		public void askDrink(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.water)
				{
					this._water = 100;
				}
				else
				{
					this._water += amount;
				}
				PlayerLife.SendWater.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.water);
				Action<PlayerLife> onTellWater_Global = PlayerLife.OnTellWater_Global;
				if (onTellWater_Global == null)
				{
					return;
				}
				onTellWater_Global.Invoke(this);
			}
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000EBFB8 File Offset: 0x000EA1B8
		public void askInfect(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.virus)
				{
					this._virus = 0;
				}
				else
				{
					this._virus -= amount;
				}
				PlayerLife.SendVirus.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.virus);
				Action<PlayerLife> onTellVirus_Global = PlayerLife.OnTellVirus_Global;
				if (onTellVirus_Global == null)
				{
					return;
				}
				onTellVirus_Global.Invoke(this);
			}
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000EC02C File Offset: 0x000EA22C
		public void askRadiate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.virus)
				{
					this._virus = 0;
				}
				else
				{
					this._virus -= amount;
				}
				VirusUpdated virusUpdated = this.onVirusUpdated;
				if (virusUpdated != null)
				{
					virusUpdated(this.virus);
				}
				Action<PlayerLife> onTellVirus_Global = PlayerLife.OnTellVirus_Global;
				if (onTellVirus_Global == null)
				{
					return;
				}
				onTellVirus_Global.Invoke(this);
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x000EC098 File Offset: 0x000EA298
		public void askDisinfect(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.virus)
				{
					this._virus = 100;
				}
				else
				{
					this._virus += amount;
				}
				PlayerLife.SendVirus.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.virus);
				Action<PlayerLife> onTellVirus_Global = PlayerLife.OnTellVirus_Global;
				if (onTellVirus_Global == null)
				{
					return;
				}
				onTellVirus_Global.Invoke(this);
			}
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x000EC110 File Offset: 0x000EA310
		internal void internalSetStamina(byte value)
		{
			this._stamina = value;
			StaminaUpdated staminaUpdated = this.onStaminaUpdated;
			if (staminaUpdated == null)
			{
				return;
			}
			staminaUpdated(this.stamina);
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000EC130 File Offset: 0x000EA330
		public void askTire(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				this.lastTire = base.player.input.simulation;
				if (amount >= this.stamina)
				{
					this._stamina = 0;
				}
				else
				{
					this._stamina -= amount;
				}
				StaminaUpdated staminaUpdated = this.onStaminaUpdated;
				if (staminaUpdated == null)
				{
					return;
				}
				staminaUpdated(this.stamina);
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000EC1A0 File Offset: 0x000EA3A0
		public void askRest(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.stamina)
				{
					this._stamina = 100;
				}
				else
				{
					this._stamina += amount;
				}
				StaminaUpdated staminaUpdated = this.onStaminaUpdated;
				if (staminaUpdated == null)
				{
					return;
				}
				staminaUpdated(this.stamina);
			}
		}

		/// <summary>
		/// Add to or subtract from stamina level.
		/// Does not replicate the change.
		/// </summary>
		// Token: 0x060033E2 RID: 13282 RVA: 0x000EC1FC File Offset: 0x000EA3FC
		public void simulatedModifyStamina(short delta)
		{
			if (delta > 0)
			{
				this.askRest((byte)delta);
				return;
			}
			if (delta < 0)
			{
				this.askTire((byte)(-(byte)delta));
			}
		}

		/// <summary>
		/// Add to or subtract from stamina level.
		/// Does not replicate the change.
		/// </summary>
		// Token: 0x060033E3 RID: 13283 RVA: 0x000EC218 File Offset: 0x000EA418
		public void simulatedModifyStamina(float delta)
		{
			this.simulatedModifyStamina(MathfEx.RoundAndClampToShort(delta));
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x000EC226 File Offset: 0x000EA426
		[Obsolete]
		public void clientModifyStamina(CSteamID senderId, short delta)
		{
			this.ReceiveModifyStamina(delta);
		}

		/// <summary>
		/// Called from the server to modify stamina.
		/// </summary>
		// Token: 0x060033E5 RID: 13285 RVA: 0x000EC22F File Offset: 0x000EA42F
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "clientModifyStamina")]
		public void ReceiveModifyStamina(short delta)
		{
			this.simulatedModifyStamina(delta);
		}

		/// <summary>
		/// Add to or subtract from stamina level on the client and server.
		/// </summary>
		// Token: 0x060033E6 RID: 13286 RVA: 0x000EC238 File Offset: 0x000EA438
		public void serverModifyStamina(float delta)
		{
			short num = MathfEx.RoundAndClampToShort(delta);
			if (num != 0)
			{
				this.simulatedModifyStamina(num);
				if (!base.channel.IsLocalPlayer)
				{
					PlayerLife.SendModifyStamina.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), num);
				}
			}
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000EC280 File Offset: 0x000EA480
		public void askView(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				this.lastView = base.player.input.simulation;
				this._vision = amount;
				VisionUpdated visionUpdated = this.onVisionUpdated;
				if (visionUpdated == null)
				{
					return;
				}
				visionUpdated(true);
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000EC2CF File Offset: 0x000EA4CF
		[Obsolete]
		public void clientModifyHallucination(CSteamID senderId, short delta)
		{
			this.ReceiveModifyHallucination(delta);
		}

		/// <summary>
		/// Called from the server to induce a hallucination.
		/// </summary>
		// Token: 0x060033E9 RID: 13289 RVA: 0x000EC2D8 File Offset: 0x000EA4D8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "clientModifyHallucination")]
		public void ReceiveModifyHallucination(short delta)
		{
			if (delta > 0)
			{
				this.askView((byte)delta);
				return;
			}
			if (delta < 0)
			{
				this.askBlind((byte)(-(byte)delta));
			}
		}

		/// <summary>
		/// Add to or subtract from hallucination level on the client.
		/// </summary>
		// Token: 0x060033EA RID: 13290 RVA: 0x000EC2F4 File Offset: 0x000EA4F4
		public void serverModifyHallucination(float delta)
		{
			short num = MathfEx.RoundAndClampToShort(delta);
			if (num != 0)
			{
				PlayerLife.SendModifyHallucination.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), num);
			}
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x000EC328 File Offset: 0x000EA528
		[Obsolete("Use serverModifyHallucination instead.")]
		public void tellHallucinate(CSteamID senderId, byte amount)
		{
			this.clientModifyHallucination(senderId, (short)amount);
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x000EC332 File Offset: 0x000EA532
		[Obsolete("Use serverModifyHallucination instead.")]
		public void sendHallucination(byte amount)
		{
			this.serverModifyHallucination((float)amount);
		}

		/// <summary>
		/// Add to or subtract from warmth level.
		/// Does not replicate the change.
		/// </summary>
		// Token: 0x060033ED RID: 13293 RVA: 0x000EC33C File Offset: 0x000EA53C
		public void simulatedModifyWarmth(short delta)
		{
			if (delta == 0 || this.isDead)
			{
				return;
			}
			if (delta > 0)
			{
				this._warmth = (uint)((ulong)this._warmth + (ulong)((long)delta));
				return;
			}
			if (delta < 0)
			{
				this._warmth = (uint)Mathf.Max(0, (int)(this._warmth + (uint)delta));
			}
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x000EC378 File Offset: 0x000EA578
		[Obsolete]
		public void clientModifyWarmth(CSteamID senderId, short delta)
		{
			this.ReceiveModifyWarmth(delta);
		}

		/// <summary>
		/// Called from the server to modify warmth.
		/// </summary>
		// Token: 0x060033EF RID: 13295 RVA: 0x000EC381 File Offset: 0x000EA581
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "clientModifyWarmth")]
		public void ReceiveModifyWarmth(short delta)
		{
			this.simulatedModifyWarmth(delta);
		}

		/// <summary>
		/// Add to or subtract from warmth level on the client and server.
		/// </summary>
		// Token: 0x060033F0 RID: 13296 RVA: 0x000EC38C File Offset: 0x000EA58C
		public void serverModifyWarmth(float delta)
		{
			short num = MathfEx.RoundAndClampToShort(delta);
			if (num != 0)
			{
				this.simulatedModifyWarmth(num);
				if (!base.channel.IsLocalPlayer)
				{
					PlayerLife.SendModifyWarmth.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), num);
				}
			}
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000EC3D4 File Offset: 0x000EA5D4
		public void askBlind(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= this.vision)
				{
					this._vision = 0;
				}
				else
				{
					this._vision -= amount;
				}
				if (this.vision == 0)
				{
					VisionUpdated visionUpdated = this.onVisionUpdated;
					if (visionUpdated == null)
					{
						return;
					}
					visionUpdated(false);
				}
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000EC430 File Offset: 0x000EA630
		public void askSuffocate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				this.lastSuffocate = base.player.input.simulation;
				if (amount >= this.oxygen)
				{
					this._oxygen = 0;
				}
				else
				{
					this._oxygen -= amount;
				}
				OxygenUpdated oxygenUpdated = this.onOxygenUpdated;
				if (oxygenUpdated == null)
				{
					return;
				}
				oxygenUpdated(this.oxygen);
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000EC4A0 File Offset: 0x000EA6A0
		public void askBreath(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
				if (amount >= 100 - this.oxygen)
				{
					this._oxygen = 100;
				}
				else
				{
					this._oxygen += amount;
				}
				OxygenUpdated oxygenUpdated = this.onOxygenUpdated;
				if (oxygenUpdated == null)
				{
					return;
				}
				oxygenUpdated(this.oxygen);
			}
		}

		/// <summary>
		/// Add to or subtract from oxygen level.
		/// Does not replicate the change.
		/// </summary>
		// Token: 0x060033F4 RID: 13300 RVA: 0x000EC4FC File Offset: 0x000EA6FC
		public void simulatedModifyOxygen(sbyte delta)
		{
			if (delta > 0)
			{
				byte amount = (byte)delta;
				this.askBreath(amount);
				return;
			}
			if (delta < 0)
			{
				byte amount2 = (byte)(-(byte)delta);
				this.askSuffocate(amount2);
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000EC527 File Offset: 0x000EA727
		public void simulatedModifyOxygen(float delta)
		{
			this.simulatedModifyOxygen(MathfEx.RoundAndClampToSByte(delta));
		}

		/// <summary>
		/// Add to or subtract from health level.
		/// Replicates change to owner.
		/// </summary>
		// Token: 0x060033F6 RID: 13302 RVA: 0x000EC538 File Offset: 0x000EA738
		public void serverModifyHealth(float delta)
		{
			if (delta > 0f)
			{
				byte amount = MathfEx.RoundAndClampToByte(delta);
				this.askHeal(amount, false, false);
				return;
			}
			byte amount2 = MathfEx.RoundAndClampToByte(-delta);
			EPlayerKill eplayerKill;
			this.askDamage(amount2, Vector3.up, EDeathCause.SUICIDE, ELimb.SPINE, CSteamID.Nil, out eplayerKill);
		}

		/// <summary>
		/// Add to or subtract from food level.
		/// Replicates change to owner.
		/// </summary>
		// Token: 0x060033F7 RID: 13303 RVA: 0x000EC580 File Offset: 0x000EA780
		public void serverModifyFood(float delta)
		{
			if (delta > 0f)
			{
				byte amount = MathfEx.RoundAndClampToByte(delta);
				this.askEat(amount);
				return;
			}
			byte amount2 = MathfEx.RoundAndClampToByte(-delta);
			this.askStarve(amount2);
		}

		/// <summary>
		/// Add to or subtract from water level.
		/// Replicates change to owner.
		/// </summary>
		// Token: 0x060033F8 RID: 13304 RVA: 0x000EC5B4 File Offset: 0x000EA7B4
		public void serverModifyWater(float delta)
		{
			if (delta > 0f)
			{
				byte amount = MathfEx.RoundAndClampToByte(delta);
				this.askDrink(amount);
				return;
			}
			byte amount2 = MathfEx.RoundAndClampToByte(-delta);
			this.askDehydrate(amount2);
		}

		/// <summary>
		/// Add to or subtract from virus level.
		/// Replicates change to owner.
		/// </summary>
		// Token: 0x060033F9 RID: 13305 RVA: 0x000EC5E8 File Offset: 0x000EA7E8
		public void serverModifyVirus(float delta)
		{
			if (delta > 0f)
			{
				byte amount = MathfEx.RoundAndClampToByte(delta);
				this.askDisinfect(amount);
				return;
			}
			byte amount2 = MathfEx.RoundAndClampToByte(-delta);
			this.askInfect(amount2);
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000EC61B File Offset: 0x000EA81B
		[Obsolete]
		public void askRespawn(CSteamID steamID, bool atHome)
		{
			this.ReceiveRespawnRequest(atHome);
		}

		/// <summary>
		/// Used by plugins to respawn the player bypassing timers. Issue #2701
		/// </summary>
		// Token: 0x060033FB RID: 13307 RVA: 0x000EC624 File Offset: 0x000EA824
		public void ServerRespawn(bool atHome)
		{
			if (this.IsAlive)
			{
				return;
			}
			this.sendRevive();
			Vector3 vector;
			byte b;
			if (!atHome || !BarricadeManager.tryGetBed(base.channel.owner.playerID.steamID, out vector, out b))
			{
				if (this.spawnpoint == null)
				{
					this.spawnpoint = LevelPlayers.getSpawn(false);
				}
				if (this.spawnpoint == null)
				{
					vector = base.transform.position;
					b = 0;
				}
				else
				{
					vector = this.spawnpoint.point;
					b = MeasurementTool.angleToByte(this.spawnpoint.angle);
				}
				string npcSpawnId = base.player.quests.npcSpawnId;
				if (!string.IsNullOrEmpty(npcSpawnId))
				{
					Spawnpoint spawnpoint = SpawnpointSystemV2.Get().FindSpawnpoint(npcSpawnId);
					if (spawnpoint != null)
					{
						vector = spawnpoint.transform.position;
						b = MeasurementTool.angleToByte(spawnpoint.transform.rotation.eulerAngles.y);
					}
					else
					{
						LocationDevkitNode locationDevkitNode = LocationDevkitNodeSystem.Get().FindByName(npcSpawnId);
						if (locationDevkitNode != null)
						{
							vector = locationDevkitNode.transform.position;
							b = MeasurementTool.angleToByte(Random.Range(0f, 360f));
						}
						else
						{
							base.player.quests.npcSpawnId = null;
							UnturnedLog.warn("Unable to find spawnpoint or location matching NpcSpawnId \"" + npcSpawnId + "\"");
						}
					}
				}
			}
			if (PlayerLife.OnSelectingRespawnPoint != null)
			{
				float angle = MeasurementTool.byteToAngle(b);
				PlayerLife.OnSelectingRespawnPoint(this, atHome, ref vector, ref angle);
				b = MeasurementTool.angleToByte(angle);
			}
			vector += new Vector3(0f, 0.5f, 0f);
			PlayerLife.SendRevive.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vector, b);
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000EC7C8 File Offset: 0x000EA9C8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askRespawn")]
		public void ReceiveRespawnRequest(bool atHome)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.IsAlive)
			{
				return;
			}
			if (atHome)
			{
				if (Provider.isPvP)
				{
					if (Time.realtimeSinceStartup - this.lastDeath < Provider.modeConfigData.Gameplay.Timer_Home)
					{
						return;
					}
				}
				else if (Time.realtimeSinceStartup - this.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
				{
					return;
				}
			}
			else if (Time.realtimeSinceStartup - this.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
			{
				return;
			}
			this.ServerRespawn(atHome);
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000EC857 File Offset: 0x000EAA57
		[Obsolete]
		public void askSuicide(CSteamID steamID)
		{
			this.ReceiveSuicideRequest();
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000EC860 File Offset: 0x000EAA60
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askSuicide")]
		public void ReceiveSuicideRequest()
		{
			if (this.IsAlive && ((Level.info != null && Level.info.type == ELevelType.SURVIVAL) || !base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && Provider.modeConfigData.Gameplay.Can_Suicide)
			{
				EPlayerKill eplayerKill;
				this.doDamage(100, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, base.channel.owner.playerID.steamID, out eplayerKill, false, ERagdollEffect.NONE, true);
			}
		}

		/// <summary>
		/// Used to refill all client stats like stamina
		/// </summary>
		// Token: 0x060033FF RID: 13311 RVA: 0x000EC8FC File Offset: 0x000EAAFC
		public void sendRevive()
		{
			this._health = (byte)Provider.modeConfigData.Players.Health_Default;
			this._food = (byte)Provider.modeConfigData.Players.Food_Default;
			this._water = (byte)Provider.modeConfigData.Players.Water_Default;
			this._virus = (byte)Provider.modeConfigData.Players.Virus_Default;
			this._stamina = 100;
			this._oxygen = 100;
			this._vision = 0;
			this._warmth = 0U;
			this._isBleeding = false;
			this._isBroken = false;
			this._temperature = EPlayerTemperature.NONE;
			this.wasWarm = false;
			this.wasCovered = false;
			this.lastStarve = base.player.input.simulation;
			this.lastDehydrate = base.player.input.simulation;
			this.lastUncleaned = base.player.input.simulation;
			this.lastTire = base.player.input.simulation;
			this.lastRest = base.player.input.simulation;
			this.lastRadiate = base.player.input.simulation;
			this.lastOutsideDeadzoneFrame = base.player.input.simulation;
			this.pendingDeadzoneDamage = 0f;
			this.pendingDeadzoneRadiation = 0f;
			this.pendingDeadzoneMaskFilterQualityLoss = 0f;
			this.recentKiller = CSteamID.Nil;
			this.lastTimeAggressive = -100f;
			this.lastTimeTookDamage = -100f;
			this.lastTimeCausedDamage = -100f;
			PlayerLife.SendLifeStats.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.health, this.food, this.water, this.virus, this.oxygen, this.isBleeding, this.isBroken);
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000ECACF File Offset: 0x000EACCF
		public void sendRespawn(bool atHome)
		{
			PlayerLife.SendRespawnRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, atHome);
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000ECAE3 File Offset: 0x000EACE3
		public void sendSuicide()
		{
			PlayerLife.SendSuicideRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000ECAF8 File Offset: 0x000EACF8
		internal void SimulateStaminaFrame(uint simulation)
		{
			if ((base.player.stance.stance == EPlayerStance.SPRINT || (base.player.stance.stance == EPlayerStance.DRIVING && base.player.movement.getVehicle() != null && base.player.movement.getVehicle().isBoosting)) && (ulong)(simulation - this.lastTire) > (ulong)((long)(1 + base.player.skills.skills[0][4].level)))
			{
				this.lastTire = simulation;
				this.askTire(1);
			}
			if (this.stamina < 100 && simulation - this.lastTire > 32f * (1f - base.player.skills.mastery(0, 3) * 0.5f) && simulation - this.lastRest > 1U)
			{
				this.lastRest = simulation;
				this.askRest((byte)(1f + base.player.skills.mastery(0, 3) * 2f));
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000ECC00 File Offset: 0x000EAE00
		private void SetIsAsphyxiating(bool newIsAsphyxiating)
		{
			if (this.isAsphyxiating != newIsAsphyxiating)
			{
				this.isAsphyxiating = newIsAsphyxiating;
				Action onIsAsphyxiatingChanged = this.OnIsAsphyxiatingChanged;
				if (onIsAsphyxiatingChanged == null)
				{
					return;
				}
				onIsAsphyxiatingChanged.Invoke();
			}
		}

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06003404 RID: 13316 RVA: 0x000ECC24 File Offset: 0x000EAE24
		// (remove) Token: 0x06003405 RID: 13317 RVA: 0x000ECC5C File Offset: 0x000EAE5C
		internal event Action OnIsAsphyxiatingChanged;

		// Token: 0x06003406 RID: 13318 RVA: 0x000ECC94 File Offset: 0x000EAE94
		private void SimulateOxygenFrame(uint simulation)
		{
			Vector3 position = base.transform.position;
			float num;
			if (OxygenManager.checkPointBreathable(position))
			{
				num = 1f;
			}
			else
			{
				if (base.player.stance.isSubmerged)
				{
					num = -1f;
				}
				else if (Level.info != null && Level.info.type == ELevelType.SURVIVAL)
				{
					if (Level.info.configData != null && Level.info.configData.Use_Legacy_Oxygen_Height)
					{
						float waterSurfaceElevation = LevelLighting.getWaterSurfaceElevation(0f);
						float t = Mathf.Clamp01((position.y - waterSurfaceElevation) / (Level.HEIGHT - waterSurfaceElevation));
						num = Mathf.Lerp(1f, -1f, t);
					}
					else
					{
						num = 1f;
					}
				}
				else
				{
					num = 1f;
				}
				float t2;
				if (num > -0.9999f && VolumeManager<OxygenVolume, OxygenVolumeManager>.Get().IsPositionInsideNonBreathableVolume(position, out t2))
				{
					num = Mathf.Lerp(num, -1f, t2);
				}
				float t3;
				if (num < 0.9999f && VolumeManager<OxygenVolume, OxygenVolumeManager>.Get().IsPositionInsideBreathableVolume(position, out t3))
				{
					num = Mathf.Lerp(num, 1f, t3);
				}
			}
			if (num > 0f)
			{
				this.SetIsAsphyxiating(false);
				if (this.oxygen < 100 && simulation - this.lastBreath > (uint)(1 + Mathf.CeilToInt(10f * (1f - num))))
				{
					this.lastBreath = simulation;
					this.askBreath((byte)(1f + base.player.skills.mastery(0, 3) * 2f));
					return;
				}
			}
			else if (num < 0f)
			{
				this.SetIsAsphyxiating(true);
				if (this.oxygen > 0)
				{
					uint num2 = (uint)(1 + base.player.skills.skills[0][5].level);
					num2 += (uint)Mathf.CeilToInt((num + 1f) * 10f);
					if (base.player.clothing.backpackAsset != null && base.player.clothing.backpackAsset.proofWater && ((base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.proofWater) || (base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.proofWater)))
					{
						num2 *= 10U;
					}
					if (simulation - this.lastSuffocate > num2)
					{
						this.lastSuffocate = simulation;
						this.askSuffocate(1);
						return;
					}
				}
				else if (simulation - this.lastSuffocate > 10U)
				{
					this.lastSuffocate = simulation;
					if (Provider.isServer)
					{
						EPlayerKill eplayerKill;
						this.doDamage(10, Vector3.up, EDeathCause.BREATH, ELimb.SPINE, Provider.server, out eplayerKill, false, ERagdollEffect.NONE, true);
					}
				}
			}
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000ECF30 File Offset: 0x000EB130
		public void simulate(uint simulation)
		{
			if (Provider.isServer)
			{
				if (Level.info.type == ELevelType.SURVIVAL)
				{
					if (this.food > 0)
					{
						if (simulation - this.lastStarve > Provider.modeConfigData.Players.Food_Use_Ticks * (1f + base.player.skills.mastery(1, 6) * 0.25f) * (base.player.movement.inSnow ? (0.5f + base.player.skills.mastery(1, 5) * 0.5f) : 1f))
						{
							this.lastStarve = simulation;
							this.askStarve(1);
						}
					}
					else if (simulation - this.lastStarve > Provider.modeConfigData.Players.Food_Damage_Ticks)
					{
						this.lastStarve = simulation;
						EPlayerKill eplayerKill;
						this.askDamage(1, Vector3.up, EDeathCause.FOOD, ELimb.SPINE, Provider.server, out eplayerKill);
					}
					if (this.water > 0)
					{
						if (simulation - this.lastDehydrate > Provider.modeConfigData.Players.Water_Use_Ticks * (1f + base.player.skills.mastery(1, 6) * 0.25f))
						{
							this.lastDehydrate = simulation;
							this.askDehydrate(1);
						}
					}
					else if (simulation - this.lastDehydrate > Provider.modeConfigData.Players.Water_Damage_Ticks)
					{
						this.lastDehydrate = simulation;
						EPlayerKill eplayerKill2;
						this.askDamage(1, Vector3.up, EDeathCause.WATER, ELimb.SPINE, Provider.server, out eplayerKill2);
					}
					if (this.virus == 0)
					{
						if (simulation - this.lastInfect > Provider.modeConfigData.Players.Virus_Damage_Ticks)
						{
							this.lastInfect = simulation;
							EPlayerKill eplayerKill3;
							this.askDamage(1, Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out eplayerKill3);
						}
					}
					else if ((uint)this.virus < Provider.modeConfigData.Players.Virus_Infect && simulation - this.lastUncleaned > Provider.modeConfigData.Players.Virus_Use_Ticks)
					{
						this.lastUncleaned = simulation;
						this.askInfect(1);
					}
				}
				if (this.isBleeding)
				{
					if (simulation - this.lastBleed > Provider.modeConfigData.Players.Bleed_Damage_Ticks)
					{
						this.lastBleed = simulation;
						EPlayerKill eplayerKill4;
						this.askDamage(1, Vector3.up, EDeathCause.BLEEDING, ELimb.SPINE, Provider.server, out eplayerKill4);
					}
				}
				else if (this.health < 100 && (uint)this.food > Provider.modeConfigData.Players.Health_Regen_Min_Food && (uint)this.water > Provider.modeConfigData.Players.Health_Regen_Min_Water && simulation - this.lastRegenerate > Provider.modeConfigData.Players.Health_Regen_Ticks * (1f - base.player.skills.mastery(1, 1) * 0.5f))
				{
					this.lastRegenerate = simulation;
					this.askHeal(1, false, false);
				}
				if (Provider.modeConfigData.Players.Can_Stop_Bleeding && this.isBleeding && simulation - this.lastBleeding > Provider.modeConfigData.Players.Bleed_Regen_Ticks * (1f - base.player.skills.mastery(1, 4) * 0.5f))
				{
					this.serverSetBleeding(false);
				}
				if (Provider.modeConfigData.Players.Can_Fix_Legs && this.isBroken && simulation - this.lastBroken > Provider.modeConfigData.Players.Leg_Regen_Ticks * (1f - base.player.skills.mastery(1, 4) * 0.5f))
				{
					this.serverSetLegsBroken(false);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.vision > 0 && simulation - this.lastView > 12U)
				{
					this.lastView = simulation;
					this.askBlind(1);
				}
				if (this.IsAlive)
				{
					Provider.provider.economyService.updateInventory();
				}
			}
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				this.SimulateStaminaFrame(simulation);
				this.SimulateOxygenFrame(simulation);
				if (base.player.movement.isRadiated)
				{
					bool flag = base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.proofRadiation && base.player.clothing.maskQuality > 0;
					if (base.player.movement.ActiveDeadzone.DeadzoneType == EDeadzoneType.FullSuitRadiation)
					{
						flag &= (base.player.clothing.shirtAsset != null && base.player.clothing.shirtAsset.proofRadiation);
						flag &= (base.player.clothing.pantsAsset != null && base.player.clothing.pantsAsset.proofRadiation);
					}
					if (flag)
					{
						if (simulation - this.lastOutsideDeadzoneFrame > 2U)
						{
							this.pendingDeadzoneDamage += base.player.movement.ActiveDeadzone.ProtectedDamagePerSecond * PlayerInput.RATE;
							float num = base.player.movement.ActiveDeadzone.MaskFilterDamagePerSecond;
							num *= base.player.clothing.maskAsset.FilterDegradationRateMultiplier;
							this.pendingDeadzoneMaskFilterQualityLoss += num * PlayerInput.RATE;
							int num2 = Mathf.FloorToInt(this.pendingDeadzoneMaskFilterQualityLoss);
							if (num2 > 0)
							{
								this.pendingDeadzoneMaskFilterQualityLoss -= (float)num2;
								this.lastRadiate = simulation;
								PlayerClothing clothing = base.player.clothing;
								clothing.maskQuality -= 1;
								base.player.clothing.updateMaskQuality();
							}
						}
					}
					else
					{
						if (simulation - this.lastOutsideDeadzoneFrame > 2U)
						{
							this.pendingDeadzoneDamage += base.player.movement.ActiveDeadzone.UnprotectedDamagePerSecond * PlayerInput.RATE;
						}
						if (this.virus > 0)
						{
							if (simulation - this.lastRadiate > 1U)
							{
								this.lastRadiate = simulation;
							}
							if (simulation - this.lastOutsideDeadzoneFrame > 2U)
							{
								this.pendingDeadzoneRadiation += base.player.movement.ActiveDeadzone.UnprotectedRadiationPerSecond * PlayerInput.RATE;
							}
							int num3 = Mathf.FloorToInt(this.pendingDeadzoneRadiation);
							if (num3 > 0)
							{
								this.pendingDeadzoneRadiation -= (float)num3;
								this.askRadiate(MathfEx.ClampToByte(num3));
							}
						}
						else if (Provider.isServer && simulation - this.lastRadiate > 10U)
						{
							this.lastRadiate = simulation;
							EPlayerKill eplayerKill5;
							this.askDamage(10, Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out eplayerKill5);
						}
					}
					if (!this.isDead && Provider.isServer)
					{
						int num4 = Mathf.FloorToInt(this.pendingDeadzoneDamage);
						if (num4 > 0)
						{
							this.pendingDeadzoneDamage -= (float)num4;
							EPlayerKill eplayerKill6;
							this.askDamage(MathfEx.ClampToByte(num4), Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out eplayerKill6);
						}
					}
				}
				else
				{
					this.lastRadiate = simulation;
					this.lastOutsideDeadzoneFrame = simulation;
					this.pendingDeadzoneDamage = 0f;
					this.pendingDeadzoneRadiation = 0f;
					this.pendingDeadzoneMaskFilterQualityLoss = 0f;
				}
				if (this.warmth > 0U)
				{
					this.simulatedModifyWarmth(-1);
				}
				bool proofFire = false;
				if (base.player.clothing.shirtAsset != null && base.player.clothing.shirtAsset.proofFire && base.player.clothing.pantsAsset != null && base.player.clothing.pantsAsset.proofFire)
				{
					proofFire = true;
				}
				EPlayerTemperature eplayerTemperature = this.temperature;
				EPlayerTemperature eplayerTemperature2 = TemperatureManager.checkPointTemperature(base.transform.position, proofFire);
				if (eplayerTemperature2 == EPlayerTemperature.ACID)
				{
					eplayerTemperature = EPlayerTemperature.ACID;
					if (Provider.isServer && simulation - this.lastBurn > 10U)
					{
						this.lastBurn = simulation;
						EPlayerKill eplayerKill7;
						this.askDamage(10, Vector3.up, EDeathCause.SPIT, ELimb.SPINE, Provider.server, out eplayerKill7);
					}
				}
				else if (eplayerTemperature2 == EPlayerTemperature.BURNING)
				{
					eplayerTemperature = EPlayerTemperature.BURNING;
					if (Provider.isServer && simulation - this.lastBurn > 10U)
					{
						this.lastBurn = simulation;
						EPlayerKill eplayerKill8;
						this.askDamage(10, Vector3.up, EDeathCause.BURNING, ELimb.SPINE, Provider.server, out eplayerKill8);
					}
					this.lastWarm = simulation;
					this.wasWarm = true;
				}
				else if (eplayerTemperature2 == EPlayerTemperature.WARM || this.warmth > 0U)
				{
					eplayerTemperature = EPlayerTemperature.WARM;
					this.lastWarm = simulation;
					this.wasWarm = true;
				}
				else if (base.player.movement.inSnow && Level.info != null && Level.info.configData.Snow_Affects_Temperature)
				{
					if (base.player.stance.stance == EPlayerStance.SWIM)
					{
						eplayerTemperature = EPlayerTemperature.FREEZING;
						if (Provider.isServer && simulation - this.lastFreeze > 25U)
						{
							this.lastFreeze = simulation;
							byte b = 8;
							if (base.player.clothing.shirtAsset != null || base.player.clothing.vestAsset != null)
							{
								b -= 2;
							}
							if (base.player.clothing.pantsAsset != null)
							{
								b -= 2;
							}
							if (base.player.clothing.hatAsset != null)
							{
								b -= 2;
							}
							EPlayerKill eplayerKill9;
							this.askDamage(b, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out eplayerKill9);
						}
					}
					else if (!this.wasWarm || simulation - this.lastWarm > 250f * (1f + base.player.skills.mastery(1, 5)))
					{
						if ((base.player.movement.getVehicle() != null && !base.player.movement.getVehicle().asset.hasZip && !base.player.movement.getVehicle().asset.hasBicycle) || Physics.Raycast(base.transform.position + Vector3.up, Quaternion.Euler(45f, LevelLighting.wind, 0f) * -Vector3.forward, 32f, RayMasks.BLOCK_WIND))
						{
							eplayerTemperature = EPlayerTemperature.COVERED;
							this.lastCovered = simulation;
							this.wasCovered = true;
						}
						else
						{
							byte b2 = 1;
							if (base.player.clothing.shirtAsset != null || base.player.clothing.vestAsset != null)
							{
								b2 += 1;
							}
							if (base.player.clothing.pantsAsset != null)
							{
								b2 += 1;
							}
							if (base.player.clothing.hatAsset != null)
							{
								b2 += 1;
							}
							if (!this.wasCovered || (ulong)(simulation - this.lastCovered) > (ulong)((long)(50 * b2)))
							{
								eplayerTemperature = EPlayerTemperature.FREEZING;
								if (Provider.isServer && simulation - this.lastFreeze > 75U)
								{
									this.lastFreeze = simulation;
									byte b3 = 4;
									if (base.player.clothing.shirtAsset != null || base.player.clothing.vestAsset != null)
									{
										b3 -= 1;
									}
									if (base.player.clothing.pantsAsset != null)
									{
										b3 -= 1;
									}
									if (base.player.clothing.hatAsset != null)
									{
										b3 -= 1;
									}
									EPlayerKill eplayerKill10;
									this.askDamage(b3, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out eplayerKill10);
								}
							}
							else
							{
								eplayerTemperature = EPlayerTemperature.COLD;
							}
						}
					}
					else
					{
						eplayerTemperature = EPlayerTemperature.COLD;
						this.lastCovered = simulation;
						this.wasCovered = true;
					}
				}
				else
				{
					eplayerTemperature = EPlayerTemperature.NONE;
				}
				if (eplayerTemperature != this.temperature)
				{
					this._temperature = eplayerTemperature;
					TemperatureUpdated temperatureUpdated = this.onTemperatureUpdated;
					if (temperatureUpdated == null)
					{
						return;
					}
					temperatureUpdated(this.temperature);
				}
			}
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000EDA84 File Offset: 0x000EBC84
		public void breakLegs()
		{
			if (!this.isBroken)
			{
				EffectAsset effectAsset = PlayerLife.BonesRef.Find();
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						relevantDistance = EffectManager.SMALL,
						position = base.transform.position
					});
				}
			}
			this.serverSetLegsBroken(true);
		}

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06003409 RID: 13321 RVA: 0x000EDAE0 File Offset: 0x000EBCE0
		// (remove) Token: 0x0600340A RID: 13322 RVA: 0x000EDB18 File Offset: 0x000EBD18
		public event PlayerLife.FallDamageRequestHandler OnFallDamageRequested;

		// Token: 0x0600340B RID: 13323 RVA: 0x000EDB50 File Offset: 0x000EBD50
		private void onLanded(float velocity)
		{
			LevelAsset asset = Level.getAsset();
			float num = (asset != null && asset.fallDamageSpeedThreshold > 0.01f) ? asset.fallDamageSpeedThreshold : 22f;
			if (velocity < -num && base.player.movement.totalGravityMultiplier > 0.67f)
			{
				Transform transform = base.player.movement.ground.transform;
				ObjectAsset objectAsset = (transform != null) ? LevelObjects.getAsset(transform) : null;
				if (objectAsset != null && !objectAsset.causesFallDamage)
				{
					return;
				}
				if (transform != null)
				{
					FallDamageOverride componentInParent = transform.gameObject.GetComponentInParent<FallDamageOverride>();
					if (componentInParent != null)
					{
						FallDamageOverride.EMode mode = componentInParent.Mode;
						if (mode != FallDamageOverride.EMode.None)
						{
							if (mode == FallDamageOverride.EMode.PreventFallDamage)
							{
								return;
							}
							UnturnedLog.warn("Unknown fall damage override: {0}", new object[]
							{
								componentInParent.GetSceneHierarchyPath()
							});
						}
					}
				}
				float num2 = Mathf.Min(101f, Mathf.Abs(velocity));
				float num3 = 1f - base.player.skills.mastery(1, 4) * 0.75f;
				float num4 = num2 * num3;
				num4 *= base.player.clothing.fallingDamageMultiplier;
				if (!Provider.modeConfigData.Players.Can_Hurt_Legs)
				{
					num4 = 0f;
				}
				bool flag = Provider.modeConfigData.Players.Can_Break_Legs;
				flag &= !base.player.clothing.preventsFallingBrokenBones;
				if (this.OnFallDamageRequested != null)
				{
					try
					{
						this.OnFallDamageRequested(this, velocity, ref num4, ref flag);
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught exception during OnFallDamageRequested:");
					}
				}
				byte b = MathfEx.RoundAndClampToByte(num4);
				if (b > 0)
				{
					EPlayerKill eplayerKill;
					this.askDamage(b, Vector3.down, EDeathCause.BONES, ELimb.SPINE, Provider.server, out eplayerKill);
				}
				if (flag)
				{
					this.breakLegs();
				}
			}
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000EDD1C File Offset: 0x000EBF1C
		internal void InitializePlayer()
		{
			if (Provider.isServer)
			{
				PlayerMovement movement = base.player.movement;
				movement.onLanded = (Landed)Delegate.Combine(movement.onLanded, new Landed(this.onLanded));
				this.load();
			}
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000EDD58 File Offset: 0x000EBF58
		public void load()
		{
			this.wasLoadCalled = true;
			this._isDead = false;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Life.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Life.dat", 0);
				byte b = block.readByte();
				if (b > 1)
				{
					this._health = block.readByte();
					this._food = block.readByte();
					this._water = block.readByte();
					this._virus = block.readByte();
					this._stamina = 100;
					if (b < PlayerLife.SAVEDATA_VERSION_WITH_OXYGEN)
					{
						this._oxygen = 100;
					}
					else
					{
						this._oxygen = block.readByte();
					}
					this._isBleeding = block.readBoolean();
					this._isBroken = block.readBoolean();
					this._temperature = EPlayerTemperature.NONE;
					this.wasWarm = false;
					this.wasCovered = false;
					return;
				}
			}
			this._health = (byte)Provider.modeConfigData.Players.Health_Default;
			this._food = (byte)Provider.modeConfigData.Players.Food_Default;
			this._water = (byte)Provider.modeConfigData.Players.Water_Default;
			this._virus = (byte)Provider.modeConfigData.Players.Virus_Default;
			this._stamina = 100;
			this._oxygen = 100;
			this._isBleeding = false;
			this._isBroken = false;
			this._temperature = EPlayerTemperature.NONE;
			this.wasWarm = false;
			this.wasCovered = false;
			this.recentKiller = CSteamID.Nil;
			this.lastTimeAggressive = -100f;
			this.lastTimeTookDamage = -100f;
			this.lastTimeCausedDamage = -100f;
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000EDF08 File Offset: 0x000EC108
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			if (base.player.life.isDead)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Life.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Life.dat");
					return;
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerLife.SAVEDATA_VERSION_LATEST);
				block.writeByte(this.health);
				block.writeByte(this.food);
				block.writeByte(this.water);
				block.writeByte(this.virus);
				block.writeByte(this.oxygen);
				block.writeBoolean(this.isBleeding);
				block.writeBoolean(this.isBroken);
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Life.dat", block);
			}
		}

		// Token: 0x04001D8A RID: 7562
		public static readonly byte SAVEDATA_VERSION_LATEST = 3;

		// Token: 0x04001D8B RID: 7563
		public static readonly byte SAVEDATA_VERSION_WITH_OXYGEN = 3;

		// Token: 0x04001D8C RID: 7564
		[Obsolete("Future version numbers for all systems will specify what changed.")]
		public static readonly byte SAVEDATA_VERSION = PlayerLife.SAVEDATA_VERSION_LATEST;

		// Token: 0x04001D8D RID: 7565
		private static readonly float COMBAT_COOLDOWN = 30f;

		// Token: 0x04001D8E RID: 7566
		public static PlayerLifeUpdated onPlayerLifeUpdated;

		// Token: 0x04001D91 RID: 7569
		public static Action<PlayerLife> OnTellHealth_Global;

		// Token: 0x04001D92 RID: 7570
		public static Action<PlayerLife> OnTellFood_Global;

		// Token: 0x04001D93 RID: 7571
		public static Action<PlayerLife> OnTellWater_Global;

		// Token: 0x04001D94 RID: 7572
		public static Action<PlayerLife> OnTellVirus_Global;

		// Token: 0x04001D95 RID: 7573
		public static Action<PlayerLife> OnTellBleeding_Global;

		// Token: 0x04001D96 RID: 7574
		public static Action<PlayerLife> OnTellBroken_Global;

		// Token: 0x04001D97 RID: 7575
		public static Action<PlayerLife, EDeathCause, ELimb, CSteamID> RocketLegacyOnDeath;

		/// <summary>
		/// Invoked after player finishes respawning.
		/// </summary>
		// Token: 0x04001D98 RID: 7576
		public static Action<PlayerLife> OnRevived_Global;

		// Token: 0x04001D9A RID: 7578
		public LifeUpdated onLifeUpdated;

		// Token: 0x04001D9B RID: 7579
		public HealthUpdated onHealthUpdated;

		// Token: 0x04001D9C RID: 7580
		public FoodUpdated onFoodUpdated;

		// Token: 0x04001D9D RID: 7581
		public WaterUpdated onWaterUpdated;

		// Token: 0x04001D9E RID: 7582
		public VirusUpdated onVirusUpdated;

		// Token: 0x04001D9F RID: 7583
		public StaminaUpdated onStaminaUpdated;

		// Token: 0x04001DA0 RID: 7584
		public VisionUpdated onVisionUpdated;

		// Token: 0x04001DA1 RID: 7585
		public OxygenUpdated onOxygenUpdated;

		// Token: 0x04001DA2 RID: 7586
		public BleedingUpdated onBleedingUpdated;

		// Token: 0x04001DA3 RID: 7587
		public BrokenUpdated onBrokenUpdated;

		// Token: 0x04001DA4 RID: 7588
		public TemperatureUpdated onTemperatureUpdated;

		// Token: 0x04001DA5 RID: 7589
		public Damaged onDamaged;

		// Token: 0x04001DA8 RID: 7592
		private static EDeathCause _deathCause;

		// Token: 0x04001DA9 RID: 7593
		private static ELimb _deathLimb;

		// Token: 0x04001DAA RID: 7594
		private static CSteamID _deathKiller;

		// Token: 0x04001DAB RID: 7595
		private CSteamID recentKiller;

		// Token: 0x04001DAC RID: 7596
		private float lastTimeAggressive;

		// Token: 0x04001DAD RID: 7597
		private float lastTimeTookDamage;

		// Token: 0x04001DAE RID: 7598
		private float lastTimeCausedDamage;

		// Token: 0x04001DAF RID: 7599
		private bool _isDead;

		// Token: 0x04001DB0 RID: 7600
		private byte lastHealth;

		// Token: 0x04001DB1 RID: 7601
		private byte _health;

		// Token: 0x04001DB2 RID: 7602
		private byte _food;

		// Token: 0x04001DB3 RID: 7603
		private byte _water;

		// Token: 0x04001DB4 RID: 7604
		private byte _virus;

		// Token: 0x04001DB5 RID: 7605
		private byte _vision;

		// Token: 0x04001DB6 RID: 7606
		private uint _warmth;

		// Token: 0x04001DB7 RID: 7607
		private byte _stamina;

		// Token: 0x04001DB8 RID: 7608
		private byte _oxygen;

		// Token: 0x04001DB9 RID: 7609
		private bool _isBleeding;

		// Token: 0x04001DBA RID: 7610
		private bool _isBroken;

		// Token: 0x04001DBB RID: 7611
		private EPlayerTemperature _temperature;

		// Token: 0x04001DBC RID: 7612
		private uint lastStarve;

		// Token: 0x04001DBD RID: 7613
		private uint lastDehydrate;

		// Token: 0x04001DBE RID: 7614
		private uint lastUncleaned;

		// Token: 0x04001DBF RID: 7615
		private uint lastView;

		// Token: 0x04001DC0 RID: 7616
		internal uint lastTire;

		// Token: 0x04001DC1 RID: 7617
		private uint lastSuffocate;

		// Token: 0x04001DC2 RID: 7618
		internal uint lastRest;

		// Token: 0x04001DC3 RID: 7619
		private uint lastBreath;

		// Token: 0x04001DC4 RID: 7620
		private uint lastInfect;

		// Token: 0x04001DC5 RID: 7621
		private uint lastBleed;

		// Token: 0x04001DC6 RID: 7622
		private uint lastBleeding;

		// Token: 0x04001DC7 RID: 7623
		private uint lastBroken;

		// Token: 0x04001DC8 RID: 7624
		private uint lastFreeze;

		// Token: 0x04001DC9 RID: 7625
		private uint lastWarm;

		// Token: 0x04001DCA RID: 7626
		private uint lastBurn;

		// Token: 0x04001DCB RID: 7627
		private uint lastCovered;

		// Token: 0x04001DCC RID: 7628
		private uint lastRegenerate;

		// Token: 0x04001DCD RID: 7629
		private uint lastRadiate;

		// Token: 0x04001DCE RID: 7630
		private uint lastOutsideDeadzoneFrame;

		// Token: 0x04001DCF RID: 7631
		private float pendingDeadzoneDamage;

		// Token: 0x04001DD0 RID: 7632
		private float pendingDeadzoneRadiation;

		// Token: 0x04001DD1 RID: 7633
		private float pendingDeadzoneMaskFilterQualityLoss;

		// Token: 0x04001DD2 RID: 7634
		private bool wasWarm;

		// Token: 0x04001DD3 RID: 7635
		private bool wasCovered;

		// Token: 0x04001DD4 RID: 7636
		private float _lastRespawn = -1f;

		// Token: 0x04001DD5 RID: 7637
		private float _lastDeath;

		// Token: 0x04001DD6 RID: 7638
		private float lastSuicide;

		// Token: 0x04001DD7 RID: 7639
		private float lastAlive;

		// Token: 0x04001DD8 RID: 7640
		private Vector3 ragdoll;

		// Token: 0x04001DD9 RID: 7641
		private ERagdollEffect ragdollEffect;

		// Token: 0x04001DDA RID: 7642
		private PlayerSpawnpoint spawnpoint;

		// Token: 0x04001DDB RID: 7643
		private static readonly ClientInstanceMethod<EDeathCause, ELimb, CSteamID> SendDeath = ClientInstanceMethod<EDeathCause, ELimb, CSteamID>.Get(typeof(PlayerLife), "ReceiveDeath");

		// Token: 0x04001DDC RID: 7644
		private static readonly ClientInstanceMethod<Vector3, ERagdollEffect> SendDead = ClientInstanceMethod<Vector3, ERagdollEffect>.Get(typeof(PlayerLife), "ReceiveDead");

		// Token: 0x04001DDD RID: 7645
		private static readonly ClientInstanceMethod<Vector3, byte> SendRevive = ClientInstanceMethod<Vector3, byte>.Get(typeof(PlayerLife), "ReceiveRevive");

		// Token: 0x04001DDE RID: 7646
		private static readonly ClientInstanceMethod<byte, byte, byte, byte, byte, bool, bool> SendLifeStats = ClientInstanceMethod<byte, byte, byte, byte, byte, bool, bool>.Get(typeof(PlayerLife), "ReceiveLifeStats");

		// Token: 0x04001DDF RID: 7647
		private static readonly ClientInstanceMethod<byte> SendHealth = ClientInstanceMethod<byte>.Get(typeof(PlayerLife), "ReceiveHealth");

		// Token: 0x04001DE0 RID: 7648
		private static readonly ClientInstanceMethod<byte, Vector3> SendDamagedEvent = ClientInstanceMethod<byte, Vector3>.Get(typeof(PlayerLife), "ReceiveDamagedEvent");

		// Token: 0x04001DE1 RID: 7649
		private static readonly ClientInstanceMethod<byte> SendFood = ClientInstanceMethod<byte>.Get(typeof(PlayerLife), "ReceiveFood");

		// Token: 0x04001DE2 RID: 7650
		private static readonly ClientInstanceMethod<byte> SendWater = ClientInstanceMethod<byte>.Get(typeof(PlayerLife), "ReceiveWater");

		// Token: 0x04001DE3 RID: 7651
		private static readonly ClientInstanceMethod<byte> SendVirus = ClientInstanceMethod<byte>.Get(typeof(PlayerLife), "ReceiveVirus");

		// Token: 0x04001DE4 RID: 7652
		private static readonly ClientInstanceMethod<bool> SendBleeding = ClientInstanceMethod<bool>.Get(typeof(PlayerLife), "ReceiveBleeding");

		// Token: 0x04001DE5 RID: 7653
		private static readonly ClientInstanceMethod<bool> SendBroken = ClientInstanceMethod<bool>.Get(typeof(PlayerLife), "ReceiveBroken");

		// Token: 0x04001DE6 RID: 7654
		private static readonly ClientInstanceMethod<short> SendModifyStamina = ClientInstanceMethod<short>.Get(typeof(PlayerLife), "ReceiveModifyStamina");

		// Token: 0x04001DE7 RID: 7655
		private static readonly ClientInstanceMethod<short> SendModifyHallucination = ClientInstanceMethod<short>.Get(typeof(PlayerLife), "ReceiveModifyHallucination");

		// Token: 0x04001DE8 RID: 7656
		private static readonly ClientInstanceMethod<short> SendModifyWarmth = ClientInstanceMethod<short>.Get(typeof(PlayerLife), "ReceiveModifyWarmth");

		// Token: 0x04001DE9 RID: 7657
		private static readonly ServerInstanceMethod<bool> SendRespawnRequest = ServerInstanceMethod<bool>.Get(typeof(PlayerLife), "ReceiveRespawnRequest");

		// Token: 0x04001DEA RID: 7658
		private static readonly ServerInstanceMethod SendSuicideRequest = ServerInstanceMethod.Get(typeof(PlayerLife), "ReceiveSuicideRequest");

		/// <summary>
		/// Used by UI. True when underwater or inside non-breathable oxygen volume.
		/// </summary>
		// Token: 0x04001DEB RID: 7659
		internal bool isAsphyxiating;

		// Token: 0x04001DED RID: 7661
		private static readonly AssetReference<EffectAsset> BonesRef = new AssetReference<EffectAsset>("663158e0a71346068947b29978818ef7");

		// Token: 0x04001DEF RID: 7663
		private bool wasLoadCalled;

		// Token: 0x020009A8 RID: 2472
		// (Invoke) Token: 0x06004BF0 RID: 19440
		public delegate void PlayerDiedCallback(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator);

		// Token: 0x020009A9 RID: 2473
		// (Invoke) Token: 0x06004BF4 RID: 19444
		public delegate void RespawnPointSelector(PlayerLife sender, bool wantsToSpawnAtHome, ref Vector3 position, ref float yaw);

		// Token: 0x020009AA RID: 2474
		// (Invoke) Token: 0x06004BF8 RID: 19448
		public delegate void FallDamageRequestHandler(PlayerLife component, float velocity, ref float damage, ref bool shouldBreakLegs);
	}
}
